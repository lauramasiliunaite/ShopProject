using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using TestShop.Application.DTOs.Common;

namespace TestShop.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        public ExceptionHandlingMiddleware(RequestDelegate requestDelegate) 
        { 
            _requestDelegate = requestDelegate; 
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (ValidationException vex)
            {
                var details = vex.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

                await Write(context, HttpStatusCode.BadRequest, "VALIDATION_FAILED",
                    "Request validation failed.", details);
            }
            catch (UnauthorizedAccessException uex)
            {
                await Write(context, HttpStatusCode.Unauthorized, "UNAUTHORIZED", uex.Message);
            }
            catch (KeyNotFoundException kex)
            {
                await Write(context, HttpStatusCode.NotFound, "NOT_FOUND", kex.Message);
            }
            catch (DbUpdateException dbex)
            {
                await Write(context, HttpStatusCode.Conflict, "DB_CONFLICT", dbex.Message);
            }
            catch (Exception ex)
            {
                await Write(context, HttpStatusCode.InternalServerError, "SERVER_ERROR", ex.Message);
            }
        }

        private static Task Write(
            HttpContext context,
            HttpStatusCode status,
            string code,
            string message,
            IDictionary<string, string[]>? details = null)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;
            var payload = new ErrorResponse(code, message, details);
            return context.Response.WriteAsync(JsonSerializer.Serialize(payload));
        }
    }
}
