using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Runtime.ExceptionServices;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Pico.WebApi;

public static class CommonDependencyInjectionExtensions
{
    public static IServiceCollection AddCustomExceptionHandlerMiddleware(
        this IServiceCollection services
    ) => services.AddSingleton<ExceptionToProblemMiddleware>();

    public static IServiceCollection AddExceptionToProblemHandler<THandler>(
        this IServiceCollection services
    )
        where THandler : class, IExceptionToProblemHandler
    {
        services.TryAddEnumerable(
            ServiceDescriptor.Singleton<IExceptionToProblemHandler, THandler>()
        );

        return services;
    }

    public static IApplicationBuilder UseExceptionToProblemMiddleware(
        this IApplicationBuilder services
    ) => services.UseMiddleware<ExceptionToProblemMiddleware>();
}

public record CreateProblemDetailsContext(HttpContext HttpContext, Exception Exception);

public interface IExceptionToProblemHandler
{
    bool TryCreateProblemDetails(
        CreateProblemDetailsContext context,
        [NotNullWhen(true)] out ProblemDetails? problemDetails
    );
}

public class FluentValidationExceptionToProblemHandler : IExceptionToProblemHandler
{
    public bool TryCreateProblemDetails(
        CreateProblemDetailsContext context,
        [NotNullWhen(true)] out ProblemDetails? problemDetails
    )
    {
        if (context.Exception is ValidationException validationException)
        {
            problemDetails = new ValidationProblemDetails(
                validationException
                    .Errors.GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).Distinct().ToArray()
                    )
            )
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "One or more validation errors occurred.",
                Detail = validationException.ToString(),
                Status = (int)HttpStatusCode.BadRequest,
            };
            return true;
        }

        problemDetails = null;
        return false;
    }
}

public sealed class ExceptionToProblemMiddleware(
    IEnumerable<IExceptionToProblemHandler> handlers,
    IProblemDetailsService problemDetailsService
) : IMiddleware
{
    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        ExceptionDispatchInfo edi;

        try
        {
            var task = next(context);

            if (task.IsCompletedSuccessfully)
            {
                return Task.CompletedTask;
            }

            return Awaited(this, context, task);
        }
        catch (Exception exception)
        {
            edi = ExceptionDispatchInfo.Capture(exception);
        }

        return HandleException(context, edi);

        static async Task Awaited(
            ExceptionToProblemMiddleware middleware,
            HttpContext context,
            Task task
        )
        {
            ExceptionDispatchInfo? edi = null;
            try
            {
                await task;
            }
            catch (Exception exception)
            {
                edi = ExceptionDispatchInfo.Capture(exception);
            }

            if (edi != null)
            {
                await middleware.HandleException(context, edi);
            }
        }
    }

    private async Task HandleException(HttpContext context, ExceptionDispatchInfo edi)
    {
        if (
            edi.SourceException is OperationCanceledException or IOException
            && context.RequestAborted.IsCancellationRequested
        )
        {
            // Add metrics/logging if needed for cancelled operations
            return;
        }

        var problemDetails = CreateProblemDetails(context, edi.SourceException);

        await problemDetailsService.TryWriteAsync(
            new ProblemDetailsContext
            {
                HttpContext = context,
                Exception = edi.SourceException,
                ProblemDetails = problemDetails,
            }
        );
    }

    private ProblemDetails CreateProblemDetails(HttpContext context, Exception exception)
    {
        foreach (var handler in handlers)
        {
            if (
                handler.TryCreateProblemDetails(
                    new CreateProblemDetailsContext(context, exception),
                    out var problem
                )
            )
            {
                return problem;
            }
        }

        // Add metrics/logging if needed for unhandled exceptions

        return new ProblemDetails
        {
            Status = (int)HttpStatusCode.InternalServerError,
            Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
            Title = "Internal Server Error",
        };
    }
}
