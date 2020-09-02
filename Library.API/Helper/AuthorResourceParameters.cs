using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.API.Helper
{
    public class AuthorResourceParameters
    {
        public const int MaxPageSize = 50;
        private int _pageSize = 10;
        public int PageNumber { get; set; } = 1;
        public string BirthPlace { get; set; }
        public string SearchQuery { get; set; }
        public string Sortby { get; set; } = "Name";

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
    }
}
