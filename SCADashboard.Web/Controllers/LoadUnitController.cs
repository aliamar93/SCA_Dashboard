using Microsoft.AspNetCore.Mvc;
using SCADashboard.Core.Interfaces;

namespace SCADashboard.Web.Controllers
{
    public class LoadUnitController : BaseController
    {
        public LoadUnitController(IAuditService auditService):base(auditService)
        { }
        public IActionResult Index()
        {
            return View();
        }
    }
}
