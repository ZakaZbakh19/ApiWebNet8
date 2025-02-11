using AutoMapper;
using GestionApi.Service.Interfaces;

namespace GestionApi.Service
{
    public abstract class BaseService : IBaseService
    {
        private readonly IMapper _mapper;

        public BaseService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public T GetEntity<T, TBody>(TBody? body) where T : class
        {
            return _mapper.Map<T>(body);
        }
    }
}
