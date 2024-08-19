using AutoMapper;
using BookStore.Dtos;
using BookStore.Models;

namespace BookStore.MappingProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDto>().ReverseMap();

            CreateMap<ShoppingCart, ShoppingCartDto>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<ShoppingCartDto, ShoppingCart>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

            CreateMap<CartItem, CartItemDto>().ReverseMap();

            CreateMap<Order, OrderDto>().ReverseMap();

            CreateMap<OrderItem, OrderItemDto>().ReverseMap();
        }
    }
}
