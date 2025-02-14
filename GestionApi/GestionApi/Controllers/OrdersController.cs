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
        ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService,
            IValidator<OrderBaseDto> validator, IValidator<OrderQuery> orderQueryValidator,
            ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _orderValidator = validator;
            _orderQueryValidator = orderQueryValidator;
            _logger = logger;
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

        [HttpGet("{id:Guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrderById([FromRoute] Guid id)
        {
            _logger.LogInformation("Validation started - {RequestName} with {id}", nameof(GetOrderById), id);
            var validation = await _orderQueryValidator.ValidateAsync(new OrderQuery() { Id = id });

            if (!validation.IsValid)
            {
                _logger.LogError("Validation failed - {RequestName} with {id}", nameof(GetOrderById), id);
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

        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrder([FromQuery] OrderQuery query)
        {
            _logger.LogInformation("Validation started - {RequestName} with {OrderQuery}", nameof(GetOrder), query);
            var validation = await _orderQueryValidator.ValidateAsync(query);

            if (!validation.IsValid)
            {
                _logger.LogError("Validation failed - {RequestName} with {OrderQuery}", nameof(GetOrder), query);
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
            _logger.LogInformation("Validation started - {RequestName} with {CreateOrderDto}", nameof(CreateOrder), orderDto);
            var validationResult = await _orderValidator.ValidateAsync(orderDto);

            if (!validationResult.IsValid)
            {
                _logger.LogError("Validation failed - {RequestName} with {CreateOrderDto}", nameof(CreateOrder), orderDto);
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderDto orderDto)
        {
            _logger.LogInformation("Validation started - {RequestName} with {OrderDto}", nameof(UpdateOrder), orderDto);
            var validation = await _orderValidator.ValidateAsync(orderDto);

            if (!validation.IsValid)
            {
                _logger.LogError("Validation failed - {RequestName} with {OrderDto}", nameof(UpdateOrder), orderDto);
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
        public async Task<IActionResult> DeleteOrder([FromQuery] OrderQuery query)
        {
            _logger.LogInformation("Validation started - {RequestName} with {OrderQuery}", nameof(DeleteOrder), query);
            var validation = await _orderQueryValidator.ValidateAsync(query);

            if (!validation.IsValid)
            {
                _logger.LogError("Validation failed - {RequestName} with {OrderQuery}", nameof(DeleteOrder), query);
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
