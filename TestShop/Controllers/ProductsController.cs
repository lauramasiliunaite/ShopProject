using Microsoft.AspNetCore.Mvc;
using TestShop.Application.DTOs.Common;
using TestShop.Application.DTOs.Products;
using TestShop.Application.Interfaces;

namespace TestShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productsRepository;
        public ProductsController(IProductRepository productsRepository) { _productsRepository = productsRepository; }

        [HttpGet]
        public async Task<ActionResult<Result<PagedResult<ProductDto>>>> Get([FromQuery] int page = 1, 
            [FromQuery] int pageSize = 12, CancellationToken cancellationToken = default)
        {
            if (page < 1 || pageSize is < 1 or > 100)
                return BadRequest(Result<PagedResult<ProductDto>>.Fail("BAD_PAGING", "Invalid page or pageSize."));

            var product = await _productsRepository.GetPagedAsync(page, pageSize, cancellationToken);
            var items = product.Items.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.ImageUrl, p.Description)).ToList();
            var dto = new PagedResult<ProductDto>(items, product.Page, product.PageSize, product.TotalCount);
            return Ok(Result<PagedResult<ProductDto>>.Ok(dto));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Result<ProductDto>>> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var product = await _productsRepository.GetByIdAsync(id, cancellationToken);
            if (product is null) 
                return NotFound(Result<ProductDto>.Fail("NOT_FOUND", "Product not found."));
            return Ok(Result<ProductDto>.Ok(new ProductDto(product.Id, product.Name, product.Price, product.ImageUrl, product.Description)));
        }
    }
}
