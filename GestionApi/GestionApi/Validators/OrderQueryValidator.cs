using FluentValidation;
using GestionApi.Dtos;
using GestionApi.Repository.Interfaces;
using GestionApi.Service.Interfaces;

namespace GestionApi.Validators
{
    public class OrderQueryValidator : AbstractValidator<OrderQuery>
    {
        private readonly IOrderService _orderService;

        public OrderQueryValidator(IOrderService orderService)
        {
            _orderService = orderService;

            RuleFor(x => x)
            .MustAsync(async (query, cancellation) =>
            {
                return await _orderService.ExistOrderAsync(query);
            })
            .WithMessage("Order does not exist.");
        }
    }
}
