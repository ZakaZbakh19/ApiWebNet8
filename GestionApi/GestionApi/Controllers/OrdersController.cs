using FluentValidation;
using GestionApi.Dtos;
using GestionApi.Exceptions;
using GestionApi.Extensions;
using GestionApi.Models;
using GestionApi.Repository.Interfaces;
using GestionApi.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace GestionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IValidator<OrderBaseDto> _orderValidator;
        private readonly IValidator<OrderQuery> _orderQueryValidator;

        public OrdersController(IOrderService orderService,
            IValidator<OrderBaseDto> validator, IValidator<OrderQuery> orderQueryValidator)
        {
            _orderService = orderService;
            _orderValidator = validator;
            _orderQueryValidator = orderQueryValidator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllOrder()
        {
            var result = await _orderService.GetOrdersAsync();

            if (!result.Success)
            {
                return StatusCode(result.Exception.ErrorCode, result);
            }

            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrderById([FromQuery] Guid id)
        {
            var validation = _orderQueryValidator.Validate(new OrderQuery() { Id = id });

            if (!validation.IsValid)
            {
                validation.AddToModelState(ModelState);
                return UnprocessableEntity(ModelState);
            }

            var result = await _orderService.GetOrderByIdAsync(id);

            if (!result.Success)
            {
                return StatusCode(result.Exception.ErrorCode, result);
            }

            return Ok(result);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrder([FromQuery] OrderQuery query)
        {
            var validation = _orderQueryValidator.Validate(query);

            if (!validation.IsValid)
            {
                validation.AddToModelState(ModelState);
                return UnprocessableEntity(ModelState);
            }

            var result = await _orderService.GetOrderAsync(query);

            if (!result.Success)
            {
                return StatusCode(result.Exception.ErrorCode, result);
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto orderDto)
        {
            var validationResult = await _orderValidator.ValidateAsync(orderDto);

            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(ModelState);
                return UnprocessableEntity(ModelState);
            }

            var result = await _orderService.AddOrderAsync(orderDto);

            if (!result.Success)
            {
                return StatusCode(result.Exception.ErrorCode, result);
            }

            return Ok(result);
        }


        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Put([FromBody] OrderDto orderDto)
        {
            var validation = _orderValidator.Validate(orderDto);

            if (!validation.IsValid)
            {
                validation.AddToModelState(ModelState);
                return UnprocessableEntity(ModelState);
            }

            var result = await _orderService.UpdateOrderAsync(orderDto);

            if (!result.Success)
            {
                return StatusCode(result.Exception.ErrorCode, result);
            }

            return Ok(result);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromQuery] OrderQuery query)
        {
            var validation = _orderQueryValidator.Validate(query);

            if (!validation.IsValid)
            {
                validation.AddToModelState(ModelState);
                return UnprocessableEntity(ModelState);
            }

            var result = await _orderService.DeleteOrderAsync(query);

            if (!result.Success)
            {
                return StatusCode(result.Exception.ErrorCode, result);
            }

            return Ok(result);
        }
    }
}
