using E_commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Specifications
{
    public class ProductWithIdsSpecifications:BaseSpecifications<Product,int>
    {
        public ProductWithIdsSpecifications(HashSet<int> productIds):base(p=>productIds.Contains(p.Id))
        {

            
        }
    }
}
