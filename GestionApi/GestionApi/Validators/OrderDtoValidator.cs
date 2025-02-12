using FluentValidation;
using GestionApi.Dtos;
using GestionApi.Models;
using GestionApi.Repository.Interfaces;

namespace GestionApi.Validators
{
    public class OrderDtoValidator : AbstractValidator<OrderDto>
    {
        private readonly IBaseRepository _baseRepository;

        public OrderDtoValidator(IBaseRepository baseRepository)
        {
            _baseRepository = baseRepository;

            RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

            RuleFor(x => x.CreationDate.ToUniversalTime())
            .NotNull().WithMessage("Creation date is required.")
            .GreaterThan(DateTime.UtcNow).WithMessage("Creation date is required.");
        }
    }
}
