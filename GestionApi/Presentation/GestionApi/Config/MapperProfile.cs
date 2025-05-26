
using AutoMapper;
using GestionApi.Dtos.Orders;
using GestionApi.Models;

namespace GestionApi.Config
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            #region order        
            CreateMap<Order, OrderDto>().ReverseMap();

            CreateMap<CreateOrderDto, Order>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OrderNumber, opt => opt.Ignore());
            #endregion
        }
    }

}
