namespace TestShop.Application.DTOs.Cart
{
    public record CartItemDto(int ProductId, string Name, decimal Price, int Quantity, decimal LineTotal);
    public record CartUpdateRequest(int ProductId, int Quantity);
    public record CartSummaryDto(IReadOnlyList<CartItemDto> Items, decimal Total);
}
