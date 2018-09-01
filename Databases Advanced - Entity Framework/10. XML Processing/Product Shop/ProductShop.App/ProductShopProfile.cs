using AutoMapper;
using ProductShop.App.Dto.Import;
using ProductShop.Models;

namespace ProductShop.App
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            this.CreateMap<UserDto, User>();
            this.CreateMap<ProductDto, Product>();
            this.CreateMap<CategoryDto, Category>();
        }
    }
}
