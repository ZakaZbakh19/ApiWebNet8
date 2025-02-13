using FluentValidation;
using GestionApi.Dtos;
using GestionApi.Models;
using GestionApi.Repository.Interfaces;
using GestionApi.Service.Interfaces;

namespace GestionApi.Validators
{
    public class OrderDtoValidator : AbstractValidator<OrderBaseDto>
    {
        private readonly IOrderService _orderService;

        public OrderDtoValidator(IOrderService orderService)
        {
            _orderService = orderService;

            RuleFor(x => x)
            .Must((order, cancellation) =>
            {
                if (order is OrderDto orderDto)
                {
                    return orderDto.Id != null;
                }

                return true;
            })
            .WithMessage("Id is incorrect");

            RuleFor(x => x)
            .Must((order, cancellation) =>
            {
                if (order is OrderDto orderDto)
                {
                    return orderDto.OrderNumber >= 80000;
                }

                return true;
            })
            .WithMessage("OrderNumber is incorrect");

            RuleFor(x => x)
            .MustAsync(async (order, cancellation) =>
            {
                if (order is OrderDto orderDto)
                {
                    return await _orderService.ExistOrderAsync(new OrderQuery() { Id = orderDto.Id, OrderNumber = orderDto.OrderNumber });
                }

                return true;
            })
            .WithMessage("Order with the given Id or OrderNumber does not exist.");


            RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.");

            RuleFor(x => x.CreationDate.ToUniversalTime())
            .NotNull()
            .WithMessage("Creation date is required.")
            .GreaterThan(DateTime.UtcNow.AddDays(-1)).WithMessage("Creation date needs to be greater than today.");
        }
    }
}
