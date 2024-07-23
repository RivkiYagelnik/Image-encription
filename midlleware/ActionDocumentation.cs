using Serilog;
using System.Net;

namespace Image_Encryption.midlleware
{
    public class ActionDocumentation
    {
        private readonly RequestDelegate _next;
        public ActionDocumentation(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                var myAction = httpContext.GetRouteData().Values["action"]?.ToString();
                Log.Information("action:" + " " + myAction);
                Log.Information("from new middleware");
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                // Log the exception
                Log.Error(ex, "An error occurred while processing the request.");

                // Optionally, handle the response to the client
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";
                var result = new { message = "An error occurred while processing your request." };
                await httpContext.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(result));
            }
        }

    }
}
