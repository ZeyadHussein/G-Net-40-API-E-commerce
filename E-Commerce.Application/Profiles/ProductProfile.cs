using AutoMapper;
using E_commerce.Application.DTOS.Products;
using E_commerce.Domain.Entities.Products;

namespace E_commerce.Application.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(d => d.ProductBrand,
                    o => o.MapFrom(s => s.ProductBrand.Name))
                .ForMember(d => d.ProductType,
                    o => o.MapFrom(s => s.ProductType.Name))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<PictureUrlReslover>());

            CreateMap<ProductBrand, BrandDto>();

            CreateMap<ProductType, TypeDto>();
        }
    }
}