using FileWeb.DTO.ViewModels;
using Newtonsoft.Json;
using System.Net;

namespace AspnetCoreMysqlDapper.Filters
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger, IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public Task Invoke(HttpContext httpContext)
        {
            try
            {
                return _next(httpContext);
            }
            catch (Exception ex)
            {
                return HandleExceptionAsync(httpContext, ex, _logger, _env);
            }
        }


        private static async Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger<ExceptionHandlerMiddleware> logger, IWebHostEnvironment env)
        {
            
            var statusCode = exception switch
            {
                UnauthorizedAccessException => 401,
                ArgumentException or ArgumentNullException or
               InvalidOperationException or BadHttpRequestException => 400,
                FileNotFoundException or DirectoryNotFoundException or KeyNotFoundException => 404,
                _ => 500
            };

            var errorResponse = new ReponseVM(
            
                false,
                statusCode,
                "error",
                statusCode == 500 ? "Internal server error occured" : exception.Message
            );



            if (!env.IsDevelopment())
            {
                logger.LogError("Error", exception);
                await context.Response.WriteAsync(errorResponse.Title);
                return;
            }

            var json = JsonConvert.SerializeObject(errorResponse);
            logger.LogError("Error", json);
            await context.Response.WriteAsync(json);
            return;

        }


    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}