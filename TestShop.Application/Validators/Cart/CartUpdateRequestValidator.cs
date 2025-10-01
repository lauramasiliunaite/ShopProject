using FluentValidation;
using TestShop.Application.DTOs.Cart;

namespace TestShop.Application.Validators.Cart
{
    public class CartUpdateRequestValidator : AbstractValidator<CartUpdateRequest>
    {
        public CartUpdateRequestValidator()
        {
            RuleFor(x => x.ProductId).GreaterThan(0);
            RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0).LessThanOrEqualTo(100);
        }
    }
}
