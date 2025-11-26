using Microsoft.AspNetCore.Mvc;
using SCADashboard.Core.Interfaces;

namespace SCADashboard.Web.Controllers
{
    public class TransportUnitController : BaseController
    {
        public TransportUnitController(IAuditService auditService):base(auditService)
        { }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            return PartialView();
        }
    }
}
