using Microsoft.AspNetCore.Mvc;
using SCADashboard.Core.Interfaces;

namespace SCADashboard.Web.Controllers
{
    public class InventoryTransactionController : BaseController
    {
        public InventoryTransactionController(IAuditService auditService):base(auditService)
        { }
        public IActionResult Index()
        {
            return View();
        }
    }
}
