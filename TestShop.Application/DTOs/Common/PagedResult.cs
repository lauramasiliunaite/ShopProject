namespace TestShop.Application.DTOs.Common
{
    public record PagedResult<T>(IReadOnlyList<T> Items, int Page, int PageSize, int TotalCount);
}
