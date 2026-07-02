using E_commerce.Application.Common;
using E_commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Specifications
{
    public class ProductCountSpecifications : BaseSpecifications<Product, int>
    {
        public ProductCountSpecifications(ProductQueryParams queryParams) : base
            (p => (!queryParams.BrandId.HasValue || p.BrandId == queryParams.BrandId.Value) && 
                  (!queryParams.TypeId.HasValue || p.TypeId == queryParams.TypeId.Value) &&
                  (string.IsNullOrEmpty(queryParams.Search) || p.Name.ToLower().Contains(queryParams.Search)))
        {
        }
    }
}
