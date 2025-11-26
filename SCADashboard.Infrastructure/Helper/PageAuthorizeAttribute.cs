using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SCADashboard.Core.ModelView;
using System.Text;

namespace SCADashboard.Infrastructure.Helper
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class PageAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _pageNameOrUrl;
        private readonly string _permissionType;

        public PageAuthorizeAttribute(string pageNameOrUrl, string permissionType)
        {
            _pageNameOrUrl = pageNameOrUrl;
            _permissionType = permissionType;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var httpContextAccessor = context.HttpContext.RequestServices.GetService<IHttpContextAccessor>();
            var httpContext = httpContextAccessor?.HttpContext;
            // 1️⃣ Get UserSession from session
            var sessionJson = httpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(sessionJson))
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
                return;
            }

            var userSession = JsonConvert.DeserializeObject<UserSession>(sessionJson);
            var modules = userSession?.Permissions ?? new List<ModulePermissionDto>();

            //var pagePermission = modules
            //    .SelectMany(m => m.Pages)
            //    .FirstOrDefault(p =>
            //        p.PageName.Equals(_pageNameOrUrl, StringComparison.OrdinalIgnoreCase) ||
            //        p.PageUrl.Equals(_pageNameOrUrl, StringComparison.OrdinalIgnoreCase));


            // 🔥 Get current route info
            var currentController = "/"+context.RouteData.Values["controller"]?.ToString();
            var currentAction = context.RouteData.Values["action"]?.ToString();
            var currentPage = $"/{currentController}/{currentAction}".ToLower();

            // 🔥 Match page from permissions
            var pagePermission = modules
                .SelectMany(m => m.Pages)
                .FirstOrDefault(p =>
                    p.PageUrl == currentController
                    //||
                    //p.PageName.Equals(currentController, StringComparison.OrdinalIgnoreCase)
                    );

            if (pagePermission == null)
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Error", null);
                return;
            }

            bool allowed = _permissionType switch
            {
                "CanView" => pagePermission.CanView,
                "CanAdd" => pagePermission.CanAdd,
                "CanEdit" => pagePermission.CanEdit,
                "CanDelete" => pagePermission.CanDelete,
                _ => false
            };

            if (!allowed)
            {
                context.Result = new RedirectToActionResult("AccessDenied", "Error", null);
            }
        }


    }
}
