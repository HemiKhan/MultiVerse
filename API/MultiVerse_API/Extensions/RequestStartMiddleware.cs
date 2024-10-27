using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

public class RequestStartMiddleware
{
    private readonly RequestDelegate _next;

    public RequestStartMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Set the start datetime of the request
        context.Items["RequestStartTime"] = DateTime.UtcNow;

        // Call the next middleware in the pipeline
        await _next(context);
    }
}