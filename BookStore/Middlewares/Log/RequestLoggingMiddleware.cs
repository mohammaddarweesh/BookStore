using Serilog.Context;
using System.Text;

namespace BookStore.Middlewares.Log
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestPath = context.Request.Path;
            var userName = context.User.Identity?.Name ?? "Anonymous";

            using (LogContext.PushProperty("RequestPath", requestPath))
            using (LogContext.PushProperty("UserName", userName))
            {
                // Capture the original response body stream
                var originalResponseBodyStream = context.Response.Body;

                // Create a new memory stream to temporarily store the response
                using (var responseBody = new MemoryStream())
                {
                    context.Response.Body = responseBody;

                    // Continue down the middleware pipeline
                    await _next(context);

                    // Read and format the response body
                    var response = await FormatResponse(context.Response);

                    using (LogContext.PushProperty("ResponseBody", response))
                    {
                        // Log the relevant information
                        _logger.LogInformation("Request processed.");

                        // Copy the contents of the new memory stream to the original stream
                        await responseBody.CopyToAsync(originalResponseBodyStream);
                    }
                }
            }
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            // Set the position of the response body stream to the beginning
            response.Body.Seek(0, SeekOrigin.Begin);

            // Read the response body into a string
            var text = await new StreamReader(response.Body, Encoding.UTF8).ReadToEndAsync();

            // Reset the position of the stream for the next middleware
            response.Body.Seek(0, SeekOrigin.Begin);

            return text;
        }
    }
}
