using TestShop.Application.DTOs.Auth;

namespace TestShop.Application.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken);
    }
}
