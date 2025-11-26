using Microsoft.AspNetCore.Mvc;
using SCADashboard.Core.Interfaces;

namespace SCADashboard.Web.Controllers
{
    public class ProductAssignmentController : BaseController
    {
        public ProductAssignmentController(IAuditService auditService):base(auditService)
        { }
        public IActionResult Index()
        {
            return View();
        }
    }
}
