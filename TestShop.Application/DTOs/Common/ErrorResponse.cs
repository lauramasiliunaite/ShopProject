namespace TestShop.Application.DTOs.Common
{
    public record ErrorResponse(string Code, string Message, IDictionary<string, string[]>? Details = null);
}
