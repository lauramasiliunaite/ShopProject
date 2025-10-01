using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading;
using TestShop.Application.DTOs.Cart;
using TestShop.Application.DTOs.Common;
using TestShop.Application.Interfaces;

namespace TestShop.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IValidator<CartUpdateRequest> _validator;

        public CartController(ICartRepository cartRepository, IValidator<CartUpdateRequest> validator) 
        { 
            _cartRepository = cartRepository; _validator = validator; 
        }

        private int UserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

        [HttpGet]
        public async Task<ActionResult<Result<CartSummaryDto>>> Get(CancellationToken cancellationToken)
        {
            var items = await _cartRepository.GetUserCartAsync(UserId(), cancellationToken);
            var dto = items.Select(i => new CartItemDto(i.ProductId, i.Product!.Name, i.Product.Price, i.Quantity, i.Product.Price * i.Quantity)).ToList();
            return Ok(Result<CartSummaryDto>.Ok(new CartSummaryDto(dto, dto.Sum(x => x.LineTotal))));
        }

        [HttpPost]
        public async Task<IActionResult> Upsert([FromBody] CartUpdateRequest request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);
            await _cartRepository.UpsertAsync(UserId(), request.ProductId, request.Quantity, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{productId:int}")]
        public async Task<IActionResult> Remove([FromRoute] int productId, CancellationToken cancellationToken)
        {
            await _cartRepository.RemoveAsync(UserId(), productId, cancellationToken);
            return NoContent();
        }
    }
}
