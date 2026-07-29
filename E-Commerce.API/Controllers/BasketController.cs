using E_commerce.Application.Contracts;
using E_commerce.Application.DTOS.Baskets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController(IBasketService basketService) : APIBaseController
    {
        #region Get
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BasketDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BasketDto>> GetBasket(string id, CancellationToken ct = default)
        {
            var basket = await basketService.GetBasketAsync(id, ct);
            return ToActionResult(basket);
        }
        #endregion

        #region Create Or Update
        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateOrUpdateBasket(BasketDto basketDto, CancellationToken ct)
        {
            var saved = await basketService.CreateOrUpdateBasketAsync(basketDto, ct);
            return ToActionResult(saved);
        }
        #endregion

        #region Delete

        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBasket(string id, CancellationToken ct)
        {
            var result = await basketService.DeleteBasketAsync(id, ct);
            return ToActionResult(result);
        }
        #endregion 


    }
}
