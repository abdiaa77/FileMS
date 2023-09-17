using FileApi.DTO.ViewModels;
using Newtonsoft.Json;
using System.Net;

namespace FileApi.Filters
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


        private static async Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger<ExceptionHandlerMiddleware> _logger, IWebHostEnvironment env)
        {
            var errorResponse = new ErrorResponse();

            if (exception is UnauthorizedAccessException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                errorResponse.StatusCode = (int)HttpStatusCode.Unauthorized;

                errorResponse.Detail = exception.Message;
                errorResponse.StackTrace = exception.StackTrace;
                errorResponse.Source = exception.Source;
            }
            else if (exception is ApplicationException || exception is InvalidOperationException ||
                    exception is ArgumentNullException | exception is ArgumentException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse.StatusCode = (int)HttpStatusCode.BadRequest;

                errorResponse.Detail = exception.Message;
                errorResponse.StackTrace = exception.StackTrace;
                errorResponse.Source = exception.Source;

            }
            else if (exception is FileNotFoundException || exception is DirectoryNotFoundException || exception is KeyNotFoundException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse.StatusCode = (int)HttpStatusCode.NotFound;

                errorResponse.Detail = exception.Message;
                errorResponse.StackTrace = exception.StackTrace;
                errorResponse.Source = exception.Source;
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse.StatusCode = (int)HttpStatusCode.InternalServerError;

                errorResponse.Detail = exception.Message;
                errorResponse.StackTrace = exception.StackTrace;
                errorResponse.Source = exception.Source;
            }

            if (!env.IsDevelopment())
            {
                _logger.LogError("Error", exception);
                await context.Response.WriteAsync(errorResponse.Detail);
                return;
            }

            var json = JsonConvert.SerializeObject(errorResponse);
            _logger.LogError("Error", json);
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