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
        

        public OrderService(IBaseRepository baseRepository, IMapper _mapper) : base(_mapper)
        {
            _baseRepository = baseRepository;
        }

        public async Task<ResultModel<OrderDto>> AddOrderAsync(OrderDto orderDto)
        {
            //Mapper Dto to Entity
            var entity = GetEntity<Order, OrderDto>(orderDto);

            entity.OrderNumber =  await GenerateOrderNumberAsync();

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

        private async Task<string> GenerateOrderNumberAsync()
        {
            var lastOrder = (await _baseRepository.GetAllAsync<Order>(x => int.Parse(x.OrderNumber))).LastOrDefault();
            var newOrderNumber = int.Parse(lastOrder.OrderNumber) + 1;
            return newOrderNumber.ToString();
        }
    }
}
