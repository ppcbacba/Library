using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.API.Entities;
using Library.API.Helper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using Library.API.Extensions;

namespace Library.API.Services
{
    public class AuthorRepository : RepositoryBase<Author, Guid>, IAuthorRepository
    {
        private Dictionary<string, PropertyMapping> mappingDict = null;
        public AuthorRepository(DbContext dbContext) : base(dbContext)
        {
            mappingDict=new Dictionary<string, PropertyMapping>(StringComparer.OrdinalIgnoreCase);
            mappingDict.Add("Name",new PropertyMapping("Name"));
            mappingDict.Add("Age", new PropertyMapping("BirthDate", true));
            mappingDict.Add("BirthPlace",new PropertyMapping("BirthPlace"));
        }

        public async Task<PagedList<Author>> GetAllAsync(AuthorResourceParameters parameters)
        {
            var queryableAuthors = DbContext.Set<Author>().AsQueryable();
            if (!string.IsNullOrWhiteSpace(parameters.BirthPlace))//过滤
            {
                queryableAuthors = queryableAuthors.Where("BirthPlace =@0", parameters.BirthPlace);//用dynamic linq
                // queryableAuthors = queryableAuthors.Where(m => m.BirthPlace.ToLower() == parameters.BirthPlace);
            }

            if (!string.IsNullOrWhiteSpace(parameters.SearchQuery))//搜索
            {
                queryableAuthors = queryableAuthors.Where(m =>
                    m.Name.ToLower().Contains(parameters.SearchQuery.ToLower()) ||
                    m.BirthPlace.ToLower().Contains(parameters.SearchQuery.ToLower()));
            }

            // using System.Linq.Dynamic.Core必须引用
            queryableAuthors = queryableAuthors.AsQueryable().OrderBy(parameters.Sortby);

            var totalCount = queryableAuthors.Count();
            var orderedAuthors = queryableAuthors.Sort(parameters.Sortby, mappingDict);
            var items = await queryableAuthors.Skip((parameters.PageNumber - 1) * parameters.PageSize)
                .Take(parameters.PageSize).ToListAsync();

            return new PagedList<Author>(items, totalCount, parameters.PageNumber, parameters.PageSize);
        }
    }
}