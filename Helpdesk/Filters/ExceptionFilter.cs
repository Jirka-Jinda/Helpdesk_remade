using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Helpdesk.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        RedirectToActionResult result;

        switch (context.Exception)
        {
            default:
                context.ExceptionHandled = true;
                result = new("Code500", "Error", new { details = context.Exception.Message });
                break;
        }

        context.Result = result;
    }
}
