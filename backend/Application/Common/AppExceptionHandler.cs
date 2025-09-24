using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Crochetbiznis.Application.Common.Exceptions;

public sealed class AppExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception ex, CancellationToken ct)
    {
        var (status, title, detail, errors) = ex switch
        {
            DuplicateSlugException d => (StatusCodes.Status409Conflict, "Slug already exists", d.Message, new { slug = new[] { "duplicate" } }),
            OperationCanceledException => (499, "Request canceled", "The request was canceled by the client.", null),
            ProductNotFoundException d => (StatusCodes.Status404NotFound, "Product not found", d.Message, null),
            _ => (StatusCodes.Status500InternalServerError, "Server error", "An unexpected error occurred.", null)
        };

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Detail = detail,
            Type = status switch
            {
                409 => "/errors/duplicate-slug",
                499 => "/errors/request-canceled",
                _ => "/errors/server-error"
            }
        };
        if (errors is not null) problem.Extensions["errors"] = errors;

        httpContext.Response.StatusCode = status;
        httpContext.Response.ContentType = "application/problem+json";
        await httpContext.Response.WriteAsJsonAsync(problem, ct);
        return true;
    }
}
