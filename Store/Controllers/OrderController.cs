using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.Dtos;
using Store.APIs.Errors;
using Store.Core.Entities.Order;
using Store.Core.Services;
using System.Security.Claims;

namespace Store.APIs.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public OrderController(IOrderService orderService,IMapper mapper)
        {
            this.orderService = orderService;
            this.mapper = mapper;
        }
        [HttpPost("CreateOrder")]
        [ProducesResponseType(typeof(Order), 200)]
        [ProducesResponseType(typeof(ApiResponse), 400)]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {
            var BuyerEmail =User.FindFirstValue(ClaimTypes.Email);
            var mappedShippingAddres= mapper.Map<Address>(orderDto.ShippingAddress);
            var order = await orderService.CreateOrderAsync(BuyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, mappedShippingAddres);
            if (order is null)
                return BadRequest(new ApiResponse(400));
            var mappedOrder = mapper.Map<OrderToReturnDto>(order);
            return Ok(mappedOrder);
        }
        [HttpGet("GetOrdersForUser")]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var orders = await orderService.GetOrdersForUserAsync(BuyerEmail);
            var mappedOrder = mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders);
            return Ok(mappedOrder);
        }
        [HttpGet("GetOrderByIdForUser")]

        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int orderId)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var order = await orderService.GetOrderByIdForUserAsync(BuyerEmail, orderId);
            var mappedOrder = mapper.Map<OrderToReturnDto>(order);
            return Ok(mappedOrder);
        }
        [HttpGet("GetDeliveryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var deliveryMethod = await orderService.GetDeliveryMethods();
            return Ok(deliveryMethod);
        }
    }
}
