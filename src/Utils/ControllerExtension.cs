using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace ToTask.Utils;

public static class ControllerExtensions
{
    public static int GetUserId(this ControllerBase controller)
    {
        return Int32.Parse(controller.User.FindFirstValue(ClaimTypes.NameIdentifier));
    }
}