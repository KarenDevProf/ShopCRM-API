using AutoMapper;
using Shop.BusinessLayer.Enums;
using Shop.BusinessLayer.Models.Goods.ResponseModels;
using Shop.BusinessLayer.Models.Orders.ResponseModels;
using Shop.DAL.Models;

namespace Shop.API.Helpers
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Good, GoodsResponseModel>();
            CreateMap<Order, OrderResponse>()
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ((OrderStatus)src.Status).ToString()))
                .ForMember(dest => dest.OrderedGoods, opt => opt.MapFrom(src => src.OrderGoods));


            CreateMap<OrderGood, OrderedGoods>()
                 .ForMember(dest => dest.GoodsName, opt => opt.MapFrom(src => src.Goods.Name))
                 .ForMember(dest => dest.Count, opt => opt.MapFrom(src => src.Count));
        }
    }
}
