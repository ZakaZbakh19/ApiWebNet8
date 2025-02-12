using FluentValidation;
using GestionApi.Dtos;
using GestionApi.Exceptions;
using GestionApi.Extensions;
using GestionApi.Models;
using GestionApi.Repository.Interfaces;
using GestionApi.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IValidator<OrderDto> _orderValidator;

        public OrdersController(IOrderService orderService,
            IValidator<OrderDto> validator)
        {
            _orderService = orderService;
            _orderValidator = validator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public IActionResult Get()
        {
            return Ok("Orders");
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            return Ok($"Order {id}");
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Post([FromBody] OrderDto orderDto)
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


        [HttpPut("{id}")]
        public IActionResult Put(int id)
        {
            return Ok($"Order {id} updated");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            return Ok($"Order {id} deleted");
        }
    }
}
