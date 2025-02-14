using AutoMapper;
using GestionApi.Config;
using GestionApi.Dtos;
using GestionApi.Service.Interfaces;
using Microsoft.Data.SqlClient;

namespace GestionApi.Service
{
    public abstract class BaseService : BaseRunCommand ,IBaseService
    {
        private readonly IMapper _mapper;

        public BaseService(IMapper mapper, 
            ILogger<BaseService> logger) : base(logger)
        {
            _mapper = mapper;
        }

        public T GetEntity<T, TBody>(TBody? body) where T : class
        {
            _log.LogInformation("Converting {SourceType} to {TargetType} entity. Details: {@BodyDetails}", typeof(TBody).Name, typeof(T).Name, body);
            return _mapper.Map<T>(body);
        }
    }
}
