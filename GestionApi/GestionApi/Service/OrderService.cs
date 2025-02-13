using AutoMapper;
using FluentValidation;
using GestionApi.Dtos;
using GestionApi.Exceptions;
using GestionApi.Exceptions.Types;
using GestionApi.Extensions;
using GestionApi.Models;
using GestionApi.Repository.Interfaces;
using GestionApi.Service.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GestionApi.Service
{
    public class OrderService : BaseService, IOrderService
    {
        private long INIT_ORDER_NUMBER = 80000;

        private readonly IBaseRepository _baseRepository;
        
        public OrderService(IBaseRepository baseRepository, IMapper _mapper) : base(_mapper)
        {
            _baseRepository = baseRepository;
        }

        #region Repository Methods

        public async Task<ResultModel<OrderDto>> AddOrderAsync(CreateOrderDto orderDto)
        {
            var entity = GetEntity<Order, CreateOrderDto>(orderDto);

            entity.OrderNumber =  await GenerateOrderNumberAsync();

            var existOrder = await ExistOrderAsync(new OrderQuery { OrderNumber = entity.OrderNumber });

            if (existOrder)
            {
                return ResultModel<OrderDto>.Failure(new CustomException("Order number already exists"));
            }

            var orderAdded = await _baseRepository.AddAsync<Order>(entity);

            return orderAdded != null ? ResultModel<OrderDto>.SuccessResult(GetEntity<OrderDto, Order>(orderAdded)) : 
                ResultModel<OrderDto>.Failure(new CustomException("Error al agregar"));
        }

        public async Task<ResultModel<bool>> DeleteOrderAsync(OrderQuery query = null)
        {
            var orderDeleted = await _baseRepository.DeleteAsync<Order>(GetOrderPredicate(query));

            return orderDeleted ? ResultModel<bool>.SuccessResult(true) :
                ResultModel<bool>.Failure(new CustomException("Error in delete"));
        }

        public async Task<bool> ExistOrderAsync(OrderQuery query = null)
        {
            return await _baseRepository.ExistAsync<Order>(GetOrderPredicate(query));
        }

        public async Task<ResultModel<OrderDto>> GetOrderAsync(OrderQuery query = null)
        {
            var entity = await _baseRepository.GetByFuncAsync<Order>(GetOrderPredicate(query));

            return entity != null ? ResultModel<OrderDto>.SuccessResult(GetEntity<OrderDto, Order>(entity)) :
                ResultModel<OrderDto>.Failure(new CustomException("Error to found"));
        }

        public async Task<ResultModel<OrderDto>> GetOrderByIdAsync(Guid id)
        {
            var entity = await _baseRepository.GetByIdAsync<Order>(id);

            return entity != null ? ResultModel<OrderDto>.SuccessResult(GetEntity<OrderDto, Order>(entity)) :
                ResultModel<OrderDto>.Failure(new CustomException("Error to found"));
        }

        public async Task<ResultModel<IEnumerable<OrderDto>>> GetOrdersAsync()
        {
            var results = await _baseRepository.GetAllAsync<Order>();

            if(results == null)
            {
                return ResultModel<IEnumerable<OrderDto>>.Failure(new CustomException("Error to found"));
            }

            var ordersDtos = GetEntity<IEnumerable<OrderDto>, IEnumerable<Order>>(results);

            return ResultModel<IEnumerable<OrderDto>>.SuccessResult(ordersDtos);
        }

        public async Task<ResultModel<OrderDto>> UpdateOrderAsync(OrderDto orderDto)
        {
            var entityUpdated = await _baseRepository.UpdateAsync<Order>(GetEntity<Order, OrderDto>(orderDto));

            return entityUpdated != null ? ResultModel<OrderDto>.SuccessResult(GetEntity<OrderDto, Order>(entityUpdated)) :
                ResultModel<OrderDto>.Failure(new CustomException("Error to updated"));
        }

        #endregion

        #region Private Methods

        private async Task<long> GenerateOrderNumberAsync()
        {
            var lastOrder = (await _baseRepository.GetAllAsync<Order>(x => x.OrderNumber)).LastOrDefault();
            return lastOrder != null ? lastOrder.OrderNumber + 1 : INIT_ORDER_NUMBER;
        }

        private Expression<Func<Order, bool>> GetOrderPredicate(OrderQuery query)
        {
            if (query == null)
            {
                return null;
            }

            return query switch
            {
                { Id: not null } => x => x.Id == query.Id, // Verifica si 'Id' no es null
                { OrderNumber: not null } => x => x.OrderNumber == query.OrderNumber, // Verifica si 'OrderNumber' no es null
                _ => null, // Si no se cumple ninguna condición, devuelve null
            };
        }

        private (Expression<Func<Order, object>>?, bool) GetOrderPredicateWithObject(OrderQuery query)
        {
            if (query == null)
            {
                return (null, false);
            }

            return query switch
            {
                { Id: not null } => (x => x.Id, query.Ascending),
                { OrderNumber: not null } => (x => x.OrderNumber, query.Ascending),
                _ => (null, query.Ascending),
            };
        }

        #endregion
    }
}
