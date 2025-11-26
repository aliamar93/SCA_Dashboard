using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SCADashboard.Core.Interfaces;
using SCADashboard.Core.Models;
using SCADashboard.Core.ModelView;
using SCADashboard.Infrastructure.Helper;
using System.Threading.Tasks;

namespace SCADashboard.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILoginService _loginService;
        public LoginController(ILoginService loginService, IWebHostEnvironment env)
        {
            _loginService = loginService;
            _env = env;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel formData)
        {
            try
            {
                var captcha = formData.captcha;
                if (!await VerifyCaptcha(captcha))
                {
                    ModelState.AddModelError("", "Invalid CAPTCHA. Try again.");
                    return Json(new { success = false, message = "Invalid Captcha" });
                }
                if (ModelState.IsValid)
                {
                    var user = await _loginService.AuthenticateUserRoleAsync(formData.Email, formData.PasswordHash);

                    if (user == null)
                        return Unauthorized(new { message = "Invalid email or password" });

                    // Store session object in HttpContext.Session
                    HttpContext.Session.SetString("UserSession", JsonConvert.SerializeObject(user));

                    // Optionally store simple properties for easy access
                    HttpContext.Session.SetString("UserId", user.UserId.ToString());
                    //HttpContext.Session.SetString("Kunde", user.kunde);
                    HttpContext.Session.SetString("UserEmail", user.UserEmail);
                    HttpContext.Session.SetString("RoleName", user.RoleName);
                    HttpContext.Session.SetString("UserName", user.UserName);
                    HttpContext.Session.SetString("TenantId", Convert.ToString(user.TenantId));

                    return Json(new { success = true, message = "Login Successfully.", redirectUrl = Url.Action("Index", "Dashboard") });
                }
                return Json(new { success = false, message = "Invalid Login Credentials." });
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)

                return Json(new { success = false, message = "An error occurred during login. Please try again later." });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public async Task<IActionResult> Logout()
        {
            if (HttpContext.Session.GetString("UserId") == null)
            {
                return RedirectToAction("Index");
            }
            var UserId = HttpContext.Session.GetString("UserId");
            User user = await _loginService.GetByIdAsync(Convert.ToInt32(UserId));
            user.IsLogin = false;
            await _loginService.UpdateOnlyAsync(user, x => x.IsLogin);
            SessionManager.ClearUserSession(HttpContext.Session);
            return RedirectToAction("Index");
        }

        public async Task<ActionResult> ForgetPassword()
        {
            return View();
        }
        public async Task<ActionResult> TermsCondition()
        {
            return View();
        }
        public async Task<ActionResult> PrivacyPolicy()
        {
            return View();
        }
        public async Task<ActionResult> Help()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> ResetPassword(string Email)
        {
            try
            {
                User user = await _loginService.GetByEmailAsync(Email);
                if (user != null)
                {
                    string templatePath = Path.Combine(_env.WebRootPath, "Templates", ClientInfoHelper.GetTemplate(ClientInfoHelper.TemplateType.PasswordReset));
                    ClientInfoHelper.SendSecurityEmailAsync(user.Email, user.Username, "", "", templatePath);
                }
                return Json(new { success = true, message = "Please Check your Email for Password Reset",RedirectUrl= Url.Action("Index", "Login") });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task<bool> VerifyCaptcha(string captchaResponse)
        {
            using var client = new HttpClient();
            var response = await client.PostAsync(
                $"https://www.google.com/recaptcha/api/siteverify?secret=6LfmrxcsAAAAADzf1bRCpmYXW0pD69gE40IEoVYi&response={captchaResponse}",
                null);

            var json = await response.Content.ReadAsStringAsync();
            dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            return result.success == "true";
        }
    }
}
