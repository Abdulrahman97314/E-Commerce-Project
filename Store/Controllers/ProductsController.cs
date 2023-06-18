using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Store.APIs.Dtos;
using Store.APIs.Errors;
using Store.APIs.Helpers;
using Store.Core;
using Store.Core.Entities;
using Store.Core.Entities.Identity;
using Store.Core.Repositories;
using Store.Core.Services;
using Store.Core.Specifications;
using Store.Core.Specifications.OrderSpecifications;
using System.Collections.Generic;
using System.Security.Claims;

namespace Store.APIs.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly UserManager<AppUser> userManager;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.userManager = userManager;
        }
        [HttpGet("GetProducts")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyList<Pagination<ProductDto>>))]
        [Cached(100)]
        public async Task<ActionResult<Pagination<ProductDto>>> GetProducts([FromQuery] ProductSpecPrams productSpecPrams)
        {
            var spec = new ProductWithBrandAndTypeSpecification(productSpecPrams);
            var products = await unitOfWork.Repository<Product>().GetAllWithSpecAsync(spec);
            var countSpec = new ProductWithFilterationForCountSpecification(productSpecPrams);
            var count = await unitOfWork.Repository<Product>().GetCountWithSpecAsync(countSpec);
            var mappedProducts = mapper.Map<IReadOnlyList<ProductDto>>(products);
            var pagination = new Pagination<ProductDto>(productSpecPrams.PageIndex, productSpecPrams.PageSize, count, mappedProducts);

            return Ok(pagination);
        }
        [HttpGet("GetProduct")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse))]
        [Cached(100)]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecification(id);
            var product = await unitOfWork.Repository<Product>().GetEntityWithSpecAsync(spec);
            if (product is null)
                return NotFound(new ApiResponse(404));
            var mappedProduct = mapper.Map<ProductDto>(product);

            return Ok(mappedProduct);
        }
        [HttpGet("GetProductTypes")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyList<ProductType>))]
        public async Task<ActionResult<ProductType>> GetProductTypes()
        {
            var types = await unitOfWork.Repository<ProductType>().GetAllAsync();
            return Ok(types);
        }
        [HttpGet("GetProductBrands")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyList<ProductBrand>))]
        public async Task<ActionResult<ProductBrand>> GetProductBrands()
        {
            var brands = await unitOfWork.Repository<ProductBrand>().GetAllAsync();
            return Ok(brands);
        }
        [Authorize]
        [HttpPost("AddRating")]
        public async Task<ActionResult<RatingDto>> AddRating(RatingDto RatingDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);
            var spec = new ProductRatingSpecification(RatingDto.ProductId, user.Email);
            var existingRating = await unitOfWork.Repository<ProductRating>().GetEntityWithSpecAsync(spec);
            if (existingRating is not null)
            {
                existingRating.Rating = RatingDto.Rating;
                existingRating.Message = RatingDto.Message;
                unitOfWork.Repository<ProductRating>().Update(existingRating);
            }
            else
            {
                var product = await unitOfWork.Repository<Product>().GetByIdAsync(RatingDto.ProductId);
                if (product is null) return BadRequest(new ApiResponse(404, "product Not found"));
                var rating = new ProductRating()
                {
                    Email = user.Email,
                    ProductId = RatingDto.ProductId,
                    Rating = RatingDto.Rating,
                    Message = RatingDto.Message,
                    UserName = user.UserName
                };
                await unitOfWork.Repository<ProductRating>().AddAsync(rating);
            }
            await unitOfWork.CompleteAsync();
            return Ok(RatingDto);
        }

    }
}
