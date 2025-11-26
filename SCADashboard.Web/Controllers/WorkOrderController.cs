using Microsoft.AspNetCore.Mvc;
using SCADashboard.Core.Interfaces;
using SCADashboard.Core.Models;

namespace SCADashboard.Web.Controllers
{
    public class WorkOrderController : BaseController
    {
        public WorkOrderController(IAuditService auditService) : base(auditService)
        { }
        public async Task<IActionResult> Index()
        {
            return View();
        }

        //parameter Id is work order for kunde Id
        public IActionResult GetWorkOrdersAsync()
        {
            //var workOrders = await AuditService.GetWorkOrdersAsync();
            return View("KanbanProject");
        }
    }
}
