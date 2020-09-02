using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using Library.API.Helper;

namespace Library.API.Extensions
{
    public static class IQueryableExtention
    {
        const string OrderSequence_Asc = "asc";
        const string OrderSequence_Desc = "desc";

        public static IQueryable<T> Sort<T>(this IQueryable<T> source,
            string orderBy,
            Dictionary<string, PropertyMapping> mapping) where T : class
        {
            var allQueryParts = orderBy.Split(',');
            var sortParts = new List<string>();
            foreach (var item in allQueryParts)
            {
                var property = string.Empty;
                var isDescending = false;
                if (item.ToLower().EndsWith(OrderSequence_Desc))
                {
                    property = item.Substring(0, item.Length - OrderSequence_Desc.Length).Trim();
                    isDescending = true;
                }
                else
                {
                    property = item.Trim();
                }

                if (!mapping.ContainsKey(property)) continue;
                if (mapping[property].IsRevert)
                {
                    isDescending = !isDescending;
                }

                sortParts.Add(!isDescending
                    ? $"{mapping[property].TargetProperty} {OrderSequence_Asc}"
                    : $"{mapping[property].TargetProperty} {OrderSequence_Desc}");
            }

            var finalExpression = string.Join(',', sortParts);
            source = source.OrderBy(finalExpression);
            return source;
        }
    }
}