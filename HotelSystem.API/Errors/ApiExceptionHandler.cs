using FluentValidation;
using HotelSystem.Application.Common.Exceptions;
using HotelSystem.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace HotelSystem.API.Errors;

public sealed class ApiExceptionHandler(ILogger<ApiExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        if (exception is OperationCanceledException && context.RequestAborted.IsCancellationRequested)
        {
            context.Response.StatusCode = 499;
            return true;
        }
        ProblemDetails problem;
        if (exception is ValidationException validation)
        {
            problem = new ValidationProblemDetails(validation.Errors.GroupBy(x => x.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).Distinct().ToArray()))
            { Status = 400, Title = "Помилки перевірки даних." };
        }
        else if (exception is DomainValidationException domain)
        {
            problem = new ValidationProblemDetails(new Dictionary<string, string[]> { [domain.Field] = [domain.Message] })
            { Status = 400, Title = "Порушено правило предметної області." };
        }
        else
        {
            var (status, title) = exception switch
            {
                AuthenticationFailedException => (401, "Не вдалося підтвердити облікові дані."),
                ForbiddenOperationException => (403, "Недостатньо прав для виконання операції."),
                NotFoundException => (404, "Ресурс не знайдено."),
                ConflictException => (409, "Конфлікт даних."),
                BadHttpRequestException { StatusCode: 413 } => (413, "Завеликий HTTP-запит."),
                BadHttpRequestException => (400, "Некоректний HTTP-запит."),
                _ => (500, "Внутрішня помилка сервера.")
            };
            problem = ApiProblems.Create(context, status, title,
                status == 500 ? "Повторіть спробу пізніше." : exception.Message);
            if (status == 500) logger.LogError(exception, "Request failed. TraceId: {TraceId}", context.TraceIdentifier);
        }
        if (problem.Status == 401) context.Response.Headers.WWWAuthenticate = "Bearer";
        await ApiProblems.WriteAsync(context, problem, ct);
        return true;
    }
}
