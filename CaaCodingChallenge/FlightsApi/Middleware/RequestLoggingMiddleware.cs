using Serilog;
using System.Text;

namespace FlightsApi.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Log the Request
        Log.Information("Request {0}: {1}", context.Request?.Method, context.Request?.Path.Value);

        // Read and log the request body data
        string requestBodyPayload = await ReadRequestBody(context);
        Log.Information("Request Payload: {0}", requestBodyPayload);

        // Copy a pointer to the original response body stream
        var originalBodyStream = context.Response.Body;

        using (var responseBody = new MemoryStream())
        {
            // Point the response body to a memory stream
            context.Response.Body = responseBody;

            await _next(context);

            // Read and log the response body data
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            string responseBodyPayload = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            Log.Information("Response {0}: {1}", context.Response?.StatusCode, responseBodyPayload);

            // Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }

    private async Task<string> ReadRequestBody(HttpContext context)
    {
        context.Request.EnableBuffering();

        var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
        await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
        string bodyAsText = Encoding.UTF8.GetString(buffer);
        context.Request.Body.Seek(0, SeekOrigin.Begin);

        return bodyAsText;
    }
}