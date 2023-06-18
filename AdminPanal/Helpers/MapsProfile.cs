using AdminPanal.Models;
using AutoMapper;
using Store.Core.Entities;

namespace AdminPanal.Helpers
{
    public class MapsProfile:Profile
    {
        public MapsProfile()
        {
            CreateMap<Product, ProductViewModel>()
                .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom<ProductPictureUrlResolver>()).ReverseMap();
        }
    }
}
