using AutoMapper;
using AdminPanal.Models;
using Store.Core.Entities;

namespace AdminPanal.Helpers
{
    public class ProductPictureUrlResolver : IValueResolver<Product, ProductViewModel, string>
    {
        private readonly IConfiguration _configuration;

        public ProductPictureUrlResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string Resolve(Product source, ProductViewModel destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.PictureUrl))
                return null;

            var pictureUrl = $"{_configuration["BaseUrl"]}{source.PictureUrl}";

            return pictureUrl;
        }
    }
}