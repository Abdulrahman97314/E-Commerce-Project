using AutoMapper;
using Store.APIs.Dtos;
using Store.Core.Entities;

namespace Store.APIs.Helpers
{
    public class ProductPictureUrlResolver :IValueResolver<Product, ProductDto, string>
    {
        private readonly IConfiguration configuration;

        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.PictureUrl))
                return null;
            var pictureUrl = $"{configuration["BaseUrl"]}{source.PictureUrl}";

            return pictureUrl;
        }
    }
}
