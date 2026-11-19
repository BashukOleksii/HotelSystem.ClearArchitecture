using Microsoft.AspNetCore.Mvc;

namespace HotelSystem.API.Errors;

internal static class ApiProblems
{
    public static ProblemDetails Create(HttpContext context, int status, string title, string? detail = null)
    {
        var problem = new ProblemDetails { Status = status, Title = title, Detail = detail, Instance = context.Request.Path };
        problem.Extensions["traceId"] = context.TraceIdentifier;
        return problem;
    }

    public static async Task WriteAsync(HttpContext context, ProblemDetails problem, CancellationToken ct = default)
    {
        context.Response.StatusCode = problem.Status ?? 500;
        problem.Extensions["traceId"] = context.TraceIdentifier;
        problem.Instance ??= context.Request.Path;
        if (problem is ValidationProblemDetails validation)
            await context.Response.WriteAsJsonAsync(validation, options: null, contentType: "application/problem+json", cancellationToken: ct);
        else
            await context.Response.WriteAsJsonAsync(problem, options: null, contentType: "application/problem+json", cancellationToken: ct);
    }
}
