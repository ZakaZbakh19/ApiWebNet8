using AutoMapper;
using FluentValidation;
using GestionApi.Dtos;
using GestionApi.Exceptions;
using GestionApi.Extensions;
using GestionApi.Models;
using GestionApi.Repository.Interfaces;
using GestionApi.Service.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Linq.Expressions;

namespace GestionApi.Service
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly IBaseRepository _baseRepository;
        private readonly IValidator<Order> _orderValidator;
        

        public OrderService(IBaseRepository baseRepository, 
            IValidator<Order> orderValidator, IMapper _mapper) : base(_mapper)
        {
            _baseRepository = baseRepository;
            _orderValidator = orderValidator;
        }

        public async Task<ResultModel<OrderDto>> AddOrderAsync(OrderDto orderDto)
        {
            //Mapper Dto to Entity
            var entity = GetEntity<Order, OrderDto>(orderDto);

            var result = await _orderValidator.ValidateAsync(entity);
            if (!result.IsValid)
            {
                //Se hace en el controller
                //result.AddToModelState(ModelState);
                //return UnprocessableEntity(ModelState);
                return ResultModel<OrderDto>.Failure(new CustomException("Num repetido"));
            }

            var orderAdded = await _baseRepository.AddAsync<Order>(entity);

            return orderAdded != null ? ResultModel<OrderDto>.SuccessResult(GetEntity<OrderDto, Order>(orderAdded), null) : 
                ResultModel<OrderDto>.Failure(new CustomException("Error al agregar"));
        }

        public Task<ResultModel<bool>> DeleteOrderAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ResultModel<bool>> ExistOrderAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ResultModel<OrderDto>> GetOrderAsync(Expression<Func<Order, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<ResultModel<OrderDto>> GetOrderAsync(Expression<Func<OrderDto, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<ResultModel<OrderDto>> GetOrderByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ResultModel<IEnumerable<OrderDto>>> GetOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ResultModel<OrderDto>> UpdateOrderAsync(Guid id, Order order)
        {
            throw new NotImplementedException();
        }
    }
}
