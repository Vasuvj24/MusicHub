using System.Net;
using System.Text.Json;

namespace MusicHub.API.Middleware
{
    public sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                var (status, message) = ex switch
                {
                    KeyNotFoundException => (HttpStatusCode.NotFound,ex.Message),
                    UnauthorizedAccessException => (HttpStatusCode.Unauthorized,ex.Message),        
                    InvalidOperationException => (HttpStatusCode.BadRequest,ex.Message),
                    ArgumentException => (HttpStatusCode.BadRequest,ex.Message),
                    _=> (HttpStatusCode.InternalServerError,"something went wrong")
                };
                context.Response.StatusCode = (int)status;
                var payload = new
                {
                    error = message,
                    //special identifier for http request
                    traceid = context.TraceIdentifier
                };
                await context.Response.WriteAsync(JsonSerializer.Serialize(payload));
            }
        }
    }
}
