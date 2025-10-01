namespace TestShop.Application.DTOs.Auth
{
    public record LoginRequest(string UserName, string Password);
    public record UserProfileDto(int Id, string Username, string Email);
    public record LoginResponse(string Token, UserProfileDto User);
}
