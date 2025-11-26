using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SCADashboard.Core.Interfaces;
using SCADashboard.Core.Models;
using SCADashboard.Infrastructure.Helper;
using System.Linq.Expressions;
using Twilio;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;
using static SCADashboard.Infrastructure.Helper.ClientInfoHelper;

namespace SCADashboard.Web.Controllers
{

    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IKundeService _kundeService;
        private readonly IPasswordLogService _passwordlogService;

        public UserController(IUserService userService, IRoleService roleService, IKundeService kundeService, IAuditService auditService, IPasswordLogService passwordlogService) : base(auditService)
        {
            _userService = userService;
            _roleService = roleService;
            _kundeService = kundeService;
            _passwordlogService = passwordlogService;
        }

        [PageAuthorize("User", "CanView")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var filters = new Expression<Func<User, bool>>[]
                {
                    u => u.IsActive,
                    u => u.TenantId == TenantId
                };
                var user = await _userService.GetAllAsync(filters, q => q.Include(u => u.UserRoles).ThenInclude(ur => ur.Role));

                //await _userService.GetUserByRoleAsync()
                return View(user);
            }
            catch (Exception ex)
            {
                // Log the exception (not shown here for brevity)
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving users.");
            }
            //Filter example with multiple conditions

        }

        // Load partial view for Create User
        public async Task<IActionResult> Create(int id)
        {

            var roles = await _roleService.GetRoleAsync(TenantId);
            ViewBag.Roles = new SelectList(roles, "RoleId", "RoleName");
            var filters = new Expression<Func<Kunde, bool>>[]
                {
                    u => (bool)u.IsActive,
                    u => u.TenantId == TenantId
                };


            var Kunde = await _kundeService.GetAllAsync(filters);
            ViewBag.Kunde = new SelectList(
                Kunde.Select(k => new
                {
                    KundeId = k.KundeId,
                    KundeName = k.KundeNo + " - " + k.KundeName
                }),
                "KundeId",
                "KundeName"
                );

            if (id == 0)
            {
                return PartialView("_CreatePartial", new User());
            }
            else
            {
                User user = await _userService.GetUserWithRoleByIdAsync(id);
                user.PasswordHash = "****************";
                user.RoleId = user.UserRoles.First().Role.RoleId;
                user.KundeId = user.Userkundes.Where(uk => uk.KundeId.HasValue)
                .Select(uk => uk.KundeId.Value)
                .ToList();
                return PartialView("_CreatePartial", user);
            }
            // Log the exception (not shown here for brevity)
            //return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while saving the user.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User model)
        {
            string message = string.Empty;
            // Remove TenantId from model state if you’re overriding it
            ModelState.Remove(nameof(model.Tenant));

            if (!ModelState.IsValid)
                return PartialView("_CreatePartial", model);
            try
            {
                if (model.UserId == 0)
                {
                    model.TenantId = TenantId;
                    model.PasswordHash = PasswordHelper.HashPassword(model.PasswordHash);
                    User user = await _userService.SaveUser(model);
                    message = "User created successfully";
                }
                else
                {
                    model.UpdatedDate = DateTime.Now;
                    await _userService.UpdateExceptAsync(model, x => x.CreatedDate, x => x.IsActive, x => x.TenantId, x => x.PasswordHash, x => x.IsLogin);
                    await _userService.SaveUser(model);

                    message = "User Updated successfully";
                }


                return Json(new { success = true, message = message });
            }
            catch (DbUpdateException ex)
            {
                //_logger.LogError(ex, "Database error while creating user {UserName}", model.Name);
                ModelState.AddModelError("", "Database error occurred while saving the user.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unexpected error occurred. Please try again later.");
            }
            // If we reach here, something failed → return partial with errors
            return PartialView("_CreatePartial", model);
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> CheckUsername(string userName, int UserId)
        {
            bool status = await _userService.GetUserEmailAndUserNameAsync(userName, null, UserId);

            return Json(status);
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> CheckEmail(string email, int UserId)
        {
            bool status = await _userService.GetUserEmailAndUserNameAsync(null, email, UserId);
            return Json(status);
        }


        [HttpGet]
        public async Task<IActionResult> Delete(int Id, bool Confirm = false)
        {
            if (!Confirm)
                return PartialView("_ModalDelete");
            var result = await _userService.GetByIdAsync(Id);
            if (result != null)
            {
                // Soft Delete Implementation
                result.IsActive = false;
                await _userService.DeleteAsync(result);
                return Json(new { success = true, message = "User deleted successfully.", });
            }
            else
                return Json(new { success = false, message = "Error deleting User." });
        }

        [HttpGet]
        public async Task<IActionResult> Profile(Guid uid)
        {
            int userId = SCADashboard.Infrastructure.Helper.GUID.Guid2Int(uid);
            var user = await _userService.GetByIdAsync(userId);
            return View("Profile", user);
        }

        public async Task<IActionResult> DetailProfile(Guid uid)
        {
            int userId = SCADashboard.Infrastructure.Helper.GUID.Guid2Int(uid);
            var user = await _userService.GetByIdAsync(userId);
            return PartialView("_Profile", user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProfile(User model, string tabtarget)
        {
            model.UpdatedDate = DateTime.Now;
            if (tabtarget == "personal")
            {
                await _userService.UpdateOnlyAsync(model, x => x.DateOfBirth, x => x.FullName, x => x.PhoneNo, x => x.UpdatedDate);
            }
            else if (tabtarget == "address")
            {
                await _userService.UpdateOnlyAsync(model, x => x.AddressLine1, x => x.AddressLine2, x => x.State, x => x.Country, x => x.UpdatedDate);
            }
            return Json(new { success = true, message = "Profile Updated Successfully" });
        }

        [HttpPost]
        public async Task<JsonResult> ResetPassword(string password)
        {

            var user = await _userService.GetByIdAsync(UserId);
            await _passwordlogService.AddAsync(new Core.Models.PasswordLog
            {
                UserId = user.UserId,
                OldPassword = user.PasswordHash,
                ChangePasswordDt = DateTime.Now
            });
            user.PasswordHash = PasswordHelper.HashPassword(password);
            await _userService.UpdateOnlyAsync(user, x => x.PasswordHash);
            return Json(new { success = true, message = "Password Changed Successfully" });
        }


        public async Task<IActionResult> LoginActivity()
        {
            return View();
        }


        public async Task<IActionResult> Security()
        {
            return View();
        }

        public async Task<IActionResult> Auth()
        {
            try
            {
                var user =await _userService.GetByIdAsync(UserId);

                if(!string.IsNullOrEmpty(user.PhoneNo))
                {
                    var twilioCredentials = ReadJsonFile(null);
                    var accountSid = Decryption(twilioCredentials.Twilio.accountSid);
                    var authToken = Decryption(twilioCredentials.Twilio.authToken);

                    
                    TwilioClient.Init(accountSid, authToken);

                    var messageOptions = new CreateMessageOptions(
                      new PhoneNumber("whatsapp:+4917677546203"));
                    messageOptions.From = new PhoneNumber("whatsapp:" + Decryption(twilioCredentials.Twilio.twilioNumber));
                    messageOptions.ContentSid = "HX229f5a04fd0510ce1b071852155d3e75";
                    //"{"1":"409173"}";
                    messageOptions.ContentVariables = "{\"1\":\"409173\"}";
                    //messageOptions.Body = "This is for SCA Logistik & Fulfillment GmbH";
                    var message = MessageResource.Create(messageOptions);
                }
                
            }
            catch (Exception ex)
            {
                throw;
            }

            //Console.WriteLine(message.Body);
            return View("Security");
            //return Json(new { success = true , message="Working..."});
        }


        public async Task<IActionResult> Calender()
        {
            return View();
        }
    }
}
