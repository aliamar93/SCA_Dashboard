using Microsoft.AspNetCore.Mvc;
using SCADashboard.Core.Interfaces;
using SCADashboard.Infrastructure.Helper;

namespace SCADashboard.Web.Controllers
{
    [Route("Dashboard")]
    public class DashboardController : BaseController
    {

        public DashboardController(IAuditService auditService) : base(auditService)
        { }
        [PageAuthorize("Dashboard", "CanView")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
