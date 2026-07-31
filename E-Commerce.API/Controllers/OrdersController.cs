using E_commerce.Application.Contracts;
using E_commerce.Application.DTOS.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    public class OrdersController : APIBaseController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        #region Create Order
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder( [FromBody] OrderDto orderDto,CancellationToken ct)
            => ToActionResult(await _orderService.CreateOrderAsync(orderDto, GetEmailFromToken(), ct));

        #endregion
        #region Get All Delivery Method
        [AllowAnonymous]
        [HttpGet("DeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethodDto>>>GetDeliveryMethods(CancellationToken ct)
            =>ToActionResult(await _orderService.GetAllDeliveryMethodsAsync(ct));
        #endregion
        #region Get All Orders
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>>GetAllOrders(CancellationToken ct)
            =>ToActionResult(await _orderService.GetAllOrdersAsync(GetEmailFromToken(), ct));
        #endregion
        #region GetOrder By Email and Id
        [Authorize]
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdAndEmail(Guid id, CancellationToken ct)
            => ToActionResult(await _orderService.GetOrderByIdAndEmailAsync(id, GetEmailFromToken(), ct));

        #endregion
    }
}
