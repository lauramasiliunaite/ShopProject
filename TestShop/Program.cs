using System.Text;
using TestShop.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using FluentValidation;
using TestShop.Application.DTOs.Cart;
using TestShop.Application.Validators.Auth;
using TestShop.Application.Validators.Cart;
using TestShop.Application.DTOs.Auth;
using Microsoft.AspNetCore.Mvc;
using TestShop.Infrastructure.Persistence;
using TestShop.Api.Middleware;

namespace TestShop
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var config = builder.Configuration;

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IValidator<LoginRequest>, LoginRequestValidator>();
            builder.Services.AddScoped<IValidator<CartUpdateRequest>, CartUpdateRequestValidator>();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var details = context.ModelState
                        .Where(kvp => kvp.Value?.Errors.Count > 0)
                        .ToDictionary(
                            kvp => kvp.Key,
                            kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());
                    var err = new TestShop.Application.DTOs.Common.ErrorResponse("MODEL_VALIDATION_FAILED", "Model validation failed.", details);
                    return new BadRequestObjectResult(err);
                };
            });

            builder.Services.AddInfrastructureServices(config);

            var jwt = config.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwt["Key"]!);
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwt["Issuer"],
                        ValidAudience = jwt["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
                await SeedData.EnsureSeedAsync(scope.ServiceProvider);

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();

        }
    }
}
