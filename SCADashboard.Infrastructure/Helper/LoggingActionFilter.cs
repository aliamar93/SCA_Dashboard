using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace SCADashboard.Infrastructure.Helper
{
    public class LoggingActionFilter : IActionFilter
    {
        private readonly ILogger<LoggingActionFilter> _logger;

        public LoggingActionFilter(ILogger<LoggingActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.RouteData.Values["controller"];
            var action = context.RouteData.Values["action"];
            var user = context.HttpContext.Session.GetString("UserName") ?? "Anonymous";
            //var user = context.HttpContext.User.Identity?.Name ?? "Anonymous";
            var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString();

            _logger.LogInformation(
                "User {User} is accessing {Controller}/{Action} from {IP} at {Time}",
                user, controller, action, ip, DateTime.Now
            );
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                _logger.LogError(
                    context.Exception,
                    "An error occurred in {Controller}/{Action} for user {User} at {Time}",
                    context.RouteData.Values["controller"],
                    context.RouteData.Values["action"],
                    context.HttpContext.User.Identity?.Name ?? "Anonymous",
                    DateTime.Now
                );
            }
        }
    }
}
