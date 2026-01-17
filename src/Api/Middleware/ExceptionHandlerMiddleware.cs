using Application.Exceptions;
using Domain.Exceptions;

namespace Api.Middleware;

public class ExceptionHandlerMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await ConvertException(context, ex);
        }
    }

    private static async Task ConvertException(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/problem+json";

        var problem = exception switch
        {
            DomainException ex => Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest,
                title: "Business rule violation"),

            BadRequestException ex => Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status400BadRequest,
                title: "Invalid request"),

            NotFoundException ex => Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status404NotFound,
                title: "Resource not found"),

            _ => Results.Problem(
                detail: "An unexpected error occurred.",
                statusCode: StatusCodes.Status500InternalServerError,
                title: "Internal server error")
        };

        await problem.ExecuteAsync(context);
    }
}
