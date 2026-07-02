using E_commerce.Application.Common;
using E_commerce.Application.Contracts;
using E_commerce.Application.DTOS.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
   
    public class ProductsController(IProductService productservice) : APIBaseController
    {

        #region Get All Products
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        public async Task<ActionResult<PaginatedResult<ProductDto>>> GetAllProducts([FromQuery] ProductQueryParams queryParams, CancellationToken ct)
        {
            var products = await productservice.GetAllProductsAsync(queryParams, ct);
            return ToActionResult(products);
        }
        #endregion
        #region Get Product
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> GetProduct(int id, CancellationToken ct)
        {
            var product = await productservice.GetProductAsync(id, ct);
            return ToActionResult(product);
        }
        #endregion
        #region Get All Brands
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<BrandDto>>> GetAllBrands(CancellationToken ct)
       => ToActionResult(await productservice.GetAllBrandsAsync(ct));
        #endregion


        #region Get All Types
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<TypeDto>>> GetAllTypes(CancellationToken ct)
        => ToActionResult(await productservice.GetAllTypesAsync(ct));

        #endregion

    }
}
