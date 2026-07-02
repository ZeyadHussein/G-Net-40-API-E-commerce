using AutoMapper;
using E_commerce.Application.Common;
using E_commerce.Application.Contracts;
using E_commerce.Application.DTOS.Products;
using E_commerce.Application.Specifications;
using E_commerce.Domain.Contracts;
using E_commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Application.Services
{
    public class ProductService:IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork,IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        

        public async Task<Result<PaginatedResult<ProductDto>>> GetAllProductsAsync(ProductQueryParams queryParams, CancellationToken ct = default)
        {
            var Spec=new ProductWithBrandAndTypeSpecifications(queryParams);
            var Repo = _unitOfWork.GetRepository<Product, int>();
            var Products = await Repo.GetAllAsync(Spec, ct);
            var Data = _mapper.Map<IReadOnlyList<ProductDto>>(Products);
           var CountSpec = new ProductCountSpecifications(queryParams);
            var countOfAllProducts = await _unitOfWork.GetRepository<Product, int>().CountAsync(CountSpec);
            var result = new PaginatedResult<ProductDto>(queryParams.PageIndex, queryParams.PageSize, countOfAllProducts, Data);
            return Result<PaginatedResult<ProductDto>>.Ok(result);
        }

        public async Task<Result<ProductDto>> GetProductAsync(int id, CancellationToken ct = default)
        {
            var Spec = new ProductWithBrandAndTypeSpecifications(id);
            var produt=await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(Spec,ct);
            if(produt is null)
            {
                return Result<ProductDto>.Fail(Error.NotFound("Product.NotFound",$"Product with Id:{id}Was Not Found"));

            }
            return _mapper.Map<ProductDto>(produt);
           


            
        }

        public async Task<Result<IReadOnlyList<BrandDto>>> GetAllBrandsAsync(CancellationToken ct = default)
        {
          var brands = await _unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync(ct);
            return Result<IReadOnlyList<BrandDto>>.Ok(_mapper.Map<IReadOnlyList<BrandDto>>(brands));

        }

        public async Task<Result<IReadOnlyList<TypeDto>>> GetAllTypesAsync(CancellationToken ct = default)
        {
           var types = await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync(ct);
            return Result<IReadOnlyList<TypeDto>>.Ok(_mapper.Map<IReadOnlyList<TypeDto>>(types));
        }

      
    }
}
