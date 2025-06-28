using Database.Context;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Storage;

namespace Helpdesk.Filters;

internal class TransactionFilter : ActionFilterAttribute
{
    private readonly ApplicationDbContext _dbContext;
    private IDbContextTransaction? _transaction;

    public TransactionFilter(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        _transaction = _dbContext.Database.BeginTransaction();
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception == null)
        {
            _transaction?.Commit();
        }
        else
        {
            _transaction?.Rollback();
        }

        _transaction?.Dispose();
    }
}
