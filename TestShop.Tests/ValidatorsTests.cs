using FluentAssertions;
using TestShop.Application.DTOs.Auth;
using TestShop.Application.DTOs.Cart;
using TestShop.Application.Validators.Auth;
using TestShop.Application.Validators.Cart;

namespace TestShop.Tests
{
    public class ValidatorsTests
    {
        [Fact]
        public void LoginRequestValidator_Rejects_Empty_Fields()
        {
            var v = new LoginRequestValidator();
            var res = v.Validate(new LoginRequest("", ""));
            res.IsValid.Should().BeFalse();
            res.Errors.Should().NotBeEmpty();
        }

        [Fact]
        public void CartUpdateRequestValidator_Rejects_Negative_And_TooBig_Quantity()
        {
            var v = new CartUpdateRequestValidator();

            v.Validate(new CartUpdateRequest(1, -1)).IsValid.Should().BeFalse();
            v.Validate(new CartUpdateRequest(1, 0)).IsValid.Should().BeTrue();
            v.Validate(new CartUpdateRequest(1, 101)).IsValid.Should().BeFalse();
        }
    }
}
