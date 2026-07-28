using E_commerce.Application.Common;
using E_commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Specifications
{
    public class ProductWithBrandAndTypeSpecifications:BaseSpecifications<Product,int>
    {
        public ProductWithBrandAndTypeSpecifications(ProductQueryParams queryParams) : base
            (p => (!queryParams.BrandId.HasValue || p.BrandId == queryParams.BrandId.Value) && (!queryParams.TypeId.HasValue || p.TypeId == queryParams.TypeId.Value)
            && (string.IsNullOrEmpty(queryParams.Search) || p.Name.ToLower().Contains(queryParams.Search)))
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
            switch(queryParams.Sort)
            {
                case ProductSortingOptions.NameAsc:
                    AddOrderBy(p => p.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDescending(p => p.Name);
                    break;
                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(p => p.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDescending(p => p.Price);
                    break;
                default:
                    AddOrderBy(p => p.Id);
                    break;
            
            }
            ApplyPagination(queryParams.pageSize, queryParams.PageIndex);   
        }
        public ProductWithBrandAndTypeSpecifications(int id ) : base(x => x.Id == id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);

        }

        }
}
