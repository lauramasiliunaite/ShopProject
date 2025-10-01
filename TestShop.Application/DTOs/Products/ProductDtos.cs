namespace TestShop.Application.DTOs.Products
{
    public record ProductDto(int Id, string Name, decimal Price, string? ImageUrl, string Description);
}
