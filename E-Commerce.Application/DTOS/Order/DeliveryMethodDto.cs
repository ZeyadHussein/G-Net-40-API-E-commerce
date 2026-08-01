using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.DTOS.Order
{
    public class DeliveryMethodDto
    {
        public int Id { get; set; } = default!;
        public string ShortName { get; set; } = default!;
        public string Desciption { get; set; } = default!;
        public string DeliveryTime { get; set; } = default!;
        public decimal Price { get; set; } 
    }
}
