using Microsoft.AspNetCore.Mvc;

namespace SCADashboard.Web.Controllers
{
    public class OrderPlannerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
