using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Helpdesk.Filters;

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
                result = new("Error", "Code500", context.Exception.Message);
                break;
        }

        _logger.LogError("Internal Server Error:\n" + context.Exception.Message);

        context.Result = result;
    }
}
