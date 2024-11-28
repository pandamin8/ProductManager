using System.Net;
using System.Text.Json;
using ProductManager.Domain.Exceptions;

namespace ProductManager.API.Middlewares;

public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);

            if (context.Response.StatusCode == (int)HttpStatusCode.Unauthorized)
            {
                await context.Response.WriteAsync("You shall not pass!");
            }
        }
        catch (ConflictException conflictException)
        {
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            await context.Response.WriteAsync(conflictException.Message);
            
            logger.LogWarning(conflictException.Message);
        }
        catch (NotFoundException notFound)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync(notFound.Message);

            logger.LogWarning(notFound.Message);
        }
        catch (ForbidException forbid)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync(forbid.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, ex.Message);
            
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("Something went wrong.");
        }
    }
}
