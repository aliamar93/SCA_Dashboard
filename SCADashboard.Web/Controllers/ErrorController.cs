using Microsoft.AspNetCore.Mvc;

namespace SCADashboard.Web.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error404()
        {
            return View("Error404");
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
