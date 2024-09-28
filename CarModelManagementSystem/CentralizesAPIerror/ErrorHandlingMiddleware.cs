using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;
using System;

namespace CarModelManagementSystem.CentralizesAPIerror
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Net;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                // Call the next middleware in the pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception details
                _logger.LogError(ex, "An unexpected error occurred while processing the request.");

                // Set the response status code to 500 (Internal Server Error)
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // Create a structured error response
                var errorResponse = new
                {
                    StatusCode = context.Response.StatusCode,
                    Message = "An unexpected error occurred.",
                    Details = ex.Message // Consider removing this in production for security reasons
                };

                // Serialize the error response to JSON
                var jsonResponse = JsonSerializer.Serialize(errorResponse);

                // Set content type and write the response
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }

}

