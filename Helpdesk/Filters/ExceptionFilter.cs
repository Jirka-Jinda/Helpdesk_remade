using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Helpdesk.Filters;

/// <summary>
/// Redirects unhandled exceptions to error controller and logs the error.
/// </summary>
public class ExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ExceptionFilter> _logger;

    public ExceptionFilter(ILogger<ExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        RedirectToActionResult result;

        switch (context.Exception)
        {
            default:
                context.ExceptionHandled = true;
                result = new("Code500", "Error", new {});
                break;
        }
        _logger.LogError(context.Exception, $"An unhandled exception occurred: {context.Exception.Message}");

        context.Result = result;
    }
}
