using Microsoft.AspNetCore.Mvc;
using Store.APIs.Errors;
using Store.APIs.Helpers;
using Store.Core;
using Store.Core.Repositories;
using Store.Core.Services;
using Store.Repository;
using Store.Service;

namespace Store.APIs.ExtensionsMethods
{
    public static class ApplicationServiceExtension
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped<IBasketRepository,BasketRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IResponseCacheService, ResponseCacheService>();
            services.AddTransient<IEmailSettings, EmailSettings>();
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddApiBehaviorOptions();
            return services;
        }
        private static void AddApiBehaviorOptions(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var errors = context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    var errorResponse = new ApiValidationErrorResponse { Errors = errors };
                    return new BadRequestObjectResult(errorResponse);
                };
            });
        }
    }
}
