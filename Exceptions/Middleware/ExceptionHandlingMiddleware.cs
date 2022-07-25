using System.Net;
using Microsoft.AspNetCore.Http.Features;

namespace Exceptions.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next.Invoke(httpContext);
        }
        catch (Exception e)
        {
            await HandleException(httpContext.Response, e);
        }
    }

    private static async Task HandleException(HttpResponse httpResponse, Exception exception)
    {
        httpResponse.Headers.Add("Exception-Type", exception.GetType().Name);
        httpResponse.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Huston we have a problem";
        httpResponse.StatusCode = (int)HttpStatusCode.BadRequest;
        await httpResponse.WriteAsync(exception.Message).ConfigureAwait(false);

    }
}

public static class AppBuilderExtensions
{
    public static void UseCustomExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}