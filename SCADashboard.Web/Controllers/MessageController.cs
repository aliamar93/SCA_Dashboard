using Microsoft.AspNetCore.Mvc;
using SCADashboard.Core.Interfaces;

namespace SCADashboard.Web.Controllers
{
    public class MessageController : BaseController
    {
        public MessageController(IAuditService auditService):base(auditService)
        { }
        public IActionResult Index()
        {
            return View();
        }
    }
}
