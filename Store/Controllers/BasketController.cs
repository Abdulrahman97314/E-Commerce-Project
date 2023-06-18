using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.Dtos;
using Store.APIs.Errors;
using Store.Core.Entities;
using Store.Core.Repositories;

namespace Store.APIs.Controllers
{
    public class BasketController : BaseController
    {
        private readonly IBasketRepository basketRepository;
        private readonly IMapper mapper;

        public BasketController(IBasketRepository basketRepository,IMapper mapper)
        {
            this.basketRepository = basketRepository;
            this.mapper = mapper;
        }

        [HttpGet("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string basketId)
        {
            var basket = await basketRepository.GetBasketAsync(basketId);
            return Ok(basket ?? new CustomerBasket(basketId));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasketDto customerBasketDto )
        {

            var mappedBasket = mapper.Map<CustomerBasket>(customerBasketDto); 
            var updatedBasket = await basketRepository.UpdateBasketAsync(mappedBasket);
            if (updatedBasket is null) return BadRequest(new ApiResponse(404));
           
            return updatedBasket;
        }

        [HttpDelete("{basketId}")]
        public async Task<ActionResult> DeleteBasket(string basketId)
        {
            var deleted = await basketRepository.DeleteBasketAsync(basketId);
            if (deleted)
            {
                return Ok(new ApiResponse(200));
            }
            return NotFound(new ApiResponse(404));
        }
    }
}
