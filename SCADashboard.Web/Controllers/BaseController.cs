using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using SCADashboard.Application.Services;
using SCADashboard.Core.Interfaces;
using SCADashboard.Core.Models;
using SCADashboard.Infrastructure.Helper;

namespace SCADashboard.Web.Controllers
{
    //[ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    //[Authorize]
    public class BaseController : Controller
    {
        private readonly IAuditService _auditService;
        //private readonly IClientInfoHelper _Helper;
        public  BaseController(IAuditService auditService)
        {
            _auditService = auditService;
        }
        // Protected property to access TenantId in derived controllers
        protected int TenantId
        {
            get
            {
                var tenantId = HttpContext.Session.GetString("TenantId");
                return string.IsNullOrEmpty(tenantId) ? 0 : Convert.ToInt32(tenantId);
            }
        }
        protected int UserId
        {
            get
            {
                var UserId = HttpContext.Session.GetString("UserId");
                return string.IsNullOrEmpty(UserId) ? 0 : Convert.ToInt32(UserId);
            }
        }


        // Executes **before** each action
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            // 1️⃣ Get UserSession from session
            var sessionJson = HttpContext.Session.GetString("UserSession");
            // Example: Check if Session exists
            if (string.IsNullOrEmpty(sessionJson))
            {
                // Redirect to login if Session is missing
                context.Result = RedirectToAction("Index", "Login");
            }
        }

        // Executes **after** each action
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            // Example: Logging action execution
            var controllerName = context.ActionDescriptor.RouteValues["controller"];
            var actionName = context.ActionDescriptor.RouteValues["action"];

            //Tracking the changes made in the DbContext
            //        var tracked = _db.ChangeTracker.Entries()
            //.Where(e => e.State != EntityState.Unchanged)
            //.ToList();

            _auditService.AddAsync(new Core.Models.AuditLog
            {
                TenantId = this.TenantId,
                UserId = this.UserId,
                Description = $"{SCADashboard.Infrastructure.Helper.ClientInfoHelper.GetBrowserName(HttpContext)}",
                Action = $"{controllerName}/{actionName}",
                Timestamp = DateTime.Now,
                Ipaddress = SCADashboard.Infrastructure.Helper.ClientInfoHelper.GetClientIp(HttpContext)
            });

        }

        //How to call the Audit method in the action methods in other controllers
        //await AuditAsync();

        //protected async Task AuditAsync()
        //{
        //    var request = HttpContext.Request;

        //    var ip = ClientInfoHelper.GetClientIp(HttpContext);
        //    var ua = Request.Headers["User-Agent"].ToString();
        //    var deviceInfo = ClientInfoHelper.ParseUserAgent(ua);

        //    var log = new AuditLogDto
        //    {
        //        UserId = User?.Identity?.Name,
        //        IpAddress = ip,
        //        Browser = deviceInfo.Browser,
        //        OS = deviceInfo.OS,
        //        Device = deviceInfo.Device,
        //        Area = RouteData.Values["area"]?.ToString(),
        //        Controller = RouteData.Values["controller"]?.ToString(),
        //        Action = RouteData.Values["action"]?.ToString(),
        //        RequestMethod = request.Method,
        //        Url = request.Path,
        //        RequestBody = null, // optional
        //        Timestamp = DateTime.UtcNow
        //    };

        //    await _auditService.LogAsync(log);
        //}



    }
}
