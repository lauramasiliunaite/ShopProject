using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TestShop.Application.DTOs.Auth;
using TestShop.Application.DTOs.Common;
using TestShop.Application.Interfaces;

namespace TestShop.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly INotificationService _notificationService;
        private readonly IValidator<LoginRequest> _validator;

        public AuthController(IAuthService authService, 
            INotificationService notificationService, 
            IValidator<LoginRequest> validator)
        {
            _authService = authService;
            _notificationService = notificationService; 
            _validator = validator;
        }

        [HttpPost("login")]
        public async Task<ActionResult<Result<LoginResponse>>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);
            var response = await _authService.LoginAsync(request, cancellationToken);
            await _notificationService.EnqueueLoginNotificationAsync(response.User.Id, cancellationToken);
            return Ok(Result<LoginResponse>.Ok(response));
        }
    }
}
