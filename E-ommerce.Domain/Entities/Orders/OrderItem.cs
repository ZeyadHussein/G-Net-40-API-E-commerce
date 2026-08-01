using E_commerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Domain.Entities.Orders
{
    public class OrderItem:BaseEntity<int>
    {
        public ProductItemOrdered product { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity {  get; set; }


    }
}
