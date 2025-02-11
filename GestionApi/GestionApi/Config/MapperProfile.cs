
using AutoMapper;
using GestionApi.Dtos;

namespace GestionApi.Config
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            #region order        
                CreateMap<Models.Order, OrderDto>().ReverseMap();           
            #endregion    
        }
    }

}
