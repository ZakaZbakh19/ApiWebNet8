using AutoMapper;
using FluentValidation;
using GestionApi.Config;
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

        public OrderService(IBaseRepository baseRepository, IMapper _mapper, 
            ILogger<OrderService> logger) : base(_mapper, logger)
        {
            _baseRepository = baseRepository;
        }

        #region Repository Methods

        public async Task<ResultModel<OrderDto>> AddOrderAsync(CreateOrderDto orderDto)
        {
            return await RunCommand<ResultModel<OrderDto>>(async () =>
            {
                return await AddOrderJob(orderDto);
            });
        }

        private async Task<ResultModel<OrderDto>> AddOrderJob(CreateOrderDto orderDto)
        {
            _log.LogInformation("Starting AddOrderAsync method to create a new order. Order details: {@OrderDetails}", orderDto);

            var entity = GetEntity<Order, CreateOrderDto>(orderDto);

            entity.OrderNumber = await GenerateOrderNumberAsync();

            _log.LogInformation("Generated order number: {OrderNumber}", entity.OrderNumber);

            var existOrder = await ExistOrderAsync(new OrderQuery { OrderNumber = entity.OrderNumber });

            if (existOrder)
            {
                return ResultModel<OrderDto>.Failure(ExceptionsBuilder.ExistEntityException());
            }

            _log.LogInformation("Adding order to the repository with OrderNumber: {OrderNumber}.", entity.OrderNumber);

            var orderAdded = await _baseRepository.AddAsync<Order>(entity);

            if (orderAdded != null)
            {
                _log.LogInformation("Successfully added order with OrderNumber: {OrderNumber}.", entity.OrderNumber);
                return ResultModel<OrderDto>.SuccessResult(GetEntity<OrderDto, Order>(orderAdded));
            }
            else
            {
                _log.LogError("Failed to add order with OrderNumber: {OrderNumber}.", entity.OrderNumber);
                return ResultModel<OrderDto>.Failure(ExceptionsBuilder.GenericException());
            }
        }

        public async Task<ResultModel<bool>> DeleteOrderAsync(OrderQuery query = null)
        {
            _log.LogInformation("Starting DeleteOrderAsync with query: {@OrderQuery}", query);

            return await RunCommand<ResultModel<bool>>(async () =>
            {
                return await DeleteOrderJob(query);
            });
        }

        public async Task<ResultModel<bool>> DeleteOrderJob(OrderQuery query = null)
        {
            _log.LogInformation("Attempting to delete order with query: {@OrderQuery}", query);

            var orderDeleted = await _baseRepository.DeleteAsync<Order>(GetOrderPredicate(query));

            if (orderDeleted)
            {
                _log.LogInformation("Successfully deleted order with query: {@OrderQuery}", query);
                return ResultModel<bool>.SuccessResult(true);
            }
            else
            {
                _log.LogWarning("Failed to delete order with query: {@OrderQuery}. No matching order found.", query);
                return ResultModel<bool>.Failure(ExceptionsBuilder.NoContentException());
            }
        }

        public async Task<bool> ExistOrderAsync(OrderQuery query = null)
        {
            _log.LogInformation("Checking if order exists with query: {@OrderQuery}", query);

            return await RunCommand<bool>(async () =>
            {
                return await ExistOrderjob(query);
            });
        }

        public async Task<bool> ExistOrderjob(OrderQuery query = null)
        {
            _log.LogInformation("Checking existence of order with query: {@OrderQuery}", query);

            return await _baseRepository.ExistAsync<Order>(GetOrderPredicate(query));
        }

        public async Task<ResultModel<OrderDto>> GetOrderAsync(OrderQuery query = null)
        {
            _log.LogInformation("Fetching order with query: {@OrderQuery}", query);

            return await RunCommand<ResultModel<OrderDto>>(async () =>
            {
                return await GetOrderJob(query);
            });
        }

        public async Task<ResultModel<OrderDto>> GetOrderJob(OrderQuery query = null)
        {
            _log.LogInformation("Attempting to retrieve order with query: {@OrderQuery}", query);

            var entity = await _baseRepository.GetByFuncAsync<Order>(GetOrderPredicate(query));

            if (entity != null)
            {
                _log.LogInformation("Successfully found order: {@OrderDetails}", entity);
                return ResultModel<OrderDto>.SuccessResult(GetEntity<OrderDto, Order>(entity));
            }
            else
            {
                _log.LogWarning("Order not found with query: {@OrderQuery}", query);
                return ResultModel<OrderDto>.Failure(ExceptionsBuilder.NotFoundException());
            }
        }

        public async Task<ResultModel<OrderDto>> GetOrderByIdAsync(Guid id)
        {
            _log.LogInformation("Fetching order with ID: {OrderId}", id);

            return await RunCommand<ResultModel<OrderDto>>(async () =>
            {
                return await GetOrderByIdJob(id);
            });
        }

        public async Task<ResultModel<OrderDto>> GetOrderByIdJob(Guid id)
        {
            _log.LogInformation("Attempting to retrieve order by ID: {OrderId}", id);

            var entity = await _baseRepository.GetByIdAsync<Order>(id);

            if (entity != null)
            {
                _log.LogInformation("Successfully found order with ID: {OrderId}", id);
                return ResultModel<OrderDto>.SuccessResult(GetEntity<OrderDto, Order>(entity));
            }
            else
            {
                _log.LogWarning("Order not found with ID: {OrderId}", id);
                return ResultModel<OrderDto>.Failure(ExceptionsBuilder.NotFoundException());
            }
        }

        public async Task<ResultModel<IEnumerable<OrderDto>>> GetOrdersAsync()
        {
            _log.LogInformation("Fetching all orders.");

            return await RunCommand<ResultModel<IEnumerable<OrderDto>>>(async () =>
            {
                return await GetOrdersJob();
            });
        }

        public async Task<ResultModel<IEnumerable<OrderDto>>> GetOrdersJob()
        {
            _log.LogInformation("Retrieving all orders from the repository.");

            var results = await _baseRepository.GetAllAsync<Order>();

            if (results == null)
            {
                _log.LogWarning("No orders found in the repository.");
                return ResultModel<IEnumerable<OrderDto>>.Failure(ExceptionsBuilder.GenericException());
            }

            var ordersDtos = GetEntity<IEnumerable<OrderDto>, IEnumerable<Order>>(results);

            _log.LogInformation("Successfully retrieved {OrderCount} orders.", ordersDtos.Count());
            return ResultModel<IEnumerable<OrderDto>>.SuccessResult(ordersDtos);
        }

        public async Task<ResultModel<OrderDto>> UpdateOrderAsync(OrderDto orderDto)
        {
            _log.LogInformation("Starting UpdateOrderAsync for OrderNumber: {OrderNumber}", orderDto.OrderNumber);

            return await RunCommand<ResultModel<OrderDto>>(async () =>
            {
                return await UpdateOrderJob(orderDto);
            });
        }

        public async Task<ResultModel<OrderDto>> UpdateOrderJob(OrderDto orderDto)
        {
            _log.LogInformation("Attempting to update order with OrderNumber: {OrderNumber}", orderDto.OrderNumber);

            var entityUpdated = await _baseRepository.UpdateAsync<Order>(GetEntity<Order, OrderDto>(orderDto));

            if (entityUpdated != null)
            {
                _log.LogInformation("Successfully updated order with OrderId: {OrderNumber}", orderDto.OrderNumber);
                return ResultModel<OrderDto>.SuccessResult(GetEntity<OrderDto, Order>(entityUpdated));
            }
            else
            {
                _log.LogWarning("Failed to update order with OrderNumber: {OrderNumber}. No matching order found.", orderDto.OrderNumber);
                return ResultModel<OrderDto>.Failure(ExceptionsBuilder.NoContentException());
            }
        }


        #endregion

        #region Private Methods

        private async Task<long> GenerateOrderNumberAsync()
        {
            var lastOrder = ((await _baseRepository.GetAllAsync<Order>()).OrderBy(x => x.OrderNumber)).LastOrDefault();
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
