using System.Data;
using System.Security.Claims;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ToTask.Utils;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class IsOwnerAttribute : Attribute, IAsyncActionFilter
{
    private readonly string _tableName;

    public IsOwnerAttribute(string tableName)
    {
        _tableName = tableName;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ActionArguments.ContainsKey("id"))
        {
            var idValue = context.ActionArguments["id"];
            if (idValue != null)
            {
                var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var dbConnection = context.HttpContext.RequestServices.GetRequiredService<IDbConnection>();
                var isOwned = await dbConnection.ExecuteScalarAsync<bool>(
                    $"SELECT * FROM {_tableName} WHERE Id = @id AND UserId = @userId",
                    new { id = idValue, userId });

                if (!isOwned)
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }
        }

        await next();
    }
}
