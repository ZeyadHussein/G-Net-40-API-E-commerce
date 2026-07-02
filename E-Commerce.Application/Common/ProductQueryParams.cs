using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Common
{
    public class ProductQueryParams
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? Search { get; set; }
        public ProductSortingOptions? Sort { get; set; }
        public int PageIndex { get; set; } = 1;
        private const int DefaultPageSize = 5;
        private int MaxPageSize = 10;
        public int PageSize=DefaultPageSize;
        public int pageSize
        {
            get => PageSize;
            set => PageSize = value > MaxPageSize ? MaxPageSize : (value<1?DefaultPageSize:value);
        }

    }
}
