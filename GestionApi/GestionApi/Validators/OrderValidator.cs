using FluentValidation;
using GestionApi.Dtos;
using GestionApi.Models;
using GestionApi.Repository.Interfaces;

namespace GestionApi.Validators
{
    public class OrderValidator : AbstractValidator<Order>
    {
        private readonly IBaseRepository _baseRepository;

        public OrderValidator(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;

            RuleFor(x => x.OrderNumber)
                .MustAsync(async (orderNumber, cancellation) =>
                    !await _baseRepository.ExistAsync<Order>(x => x.OrderNumber == orderNumber))
                .WithMessage("Order number is added.");

            RuleFor(x => x.Id)
                .MustAsync(async (id, cancellation) =>
                    !await _baseRepository.ExistAsync<Order>(x => x.Id.CompareTo(id) == 0))
                .WithMessage("Order is added.");
        }
    }
}
