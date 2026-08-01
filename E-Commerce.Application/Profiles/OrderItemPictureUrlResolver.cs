using AutoMapper;
using AutoMapper.Execution;
using E_commerce.Application.DTOS.Order;
using E_commerce.Domain.Entities.Orders;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Profiles
{
    public class OrderItemPictureUrlResolver(IOptions<UrlSettings> options) : AutoMapper.IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly UrlSettings _urlSettings=options.Value;
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
           if(string.IsNullOrEmpty(source.product.PictureUrl))
                return string.Empty;

            return $"{_urlSettings.BaseUrl}{source.product.PictureUrl}"; 

        }
    }
}
