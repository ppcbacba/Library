using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Helper
{
    public class PropertyMapping
    {
        public PropertyMapping(string targetProperty,bool revert=false)
        {
            TargetProperty = targetProperty;
            IsRevert = revert;

        }

        public bool IsRevert { get; set; }

        public string TargetProperty { get; set; }
    }
}
