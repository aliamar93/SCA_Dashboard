using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCADashboard.Application.Services;
using SCADashboard.Core.Interfaces;
using SCADashboard.Core.Models;
using SCADashboard.Core.ModelView;
using SCADashboard.Infrastructure.Helper;
using System.Linq.Expressions;

namespace SCADashboard.Web.Controllers
{
    
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService,IAuditService auditService)
            :base(auditService)
        {
            _roleService = roleService;
        }
        [PageAuthorize("Role", "CanView")]
        public async Task<IActionResult> Index()
        {

            var filters = new Expression<Func<Role, bool>>[]
                {
                    u => u.IsActive,
                    u => u.TenantId == TenantId
                };
            var role = await _roleService.GetAllAsync(filters);
            return View(role);
        }
        public async Task<IActionResult> Create(int id)
        {
            if (id == 0)
            {
                return PartialView("_CreatePartial", new Role());
            }
            else
            {
                var role =await _roleService.GetByIdAsync(id);
                return PartialView("_CreatePartial", role);
            }
            // Log the exception (not shown here for brevity)
            //return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while saving the user.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Role model)
        {
            string message = string.Empty;
            // Remove TenantId from model state if you’re overriding it
            ModelState.Remove(nameof(model.Tenant));

            if (!ModelState.IsValid)
                return PartialView("_CreatePartial", model);
            try
            {
                if (model.RoleId == 0)
                {

                    model.TenantId = TenantId; 
                    model.CreatedDate = DateTime.Now;
                    model.IsActive = true;
                    await _roleService.AddAsync(model);
                    message = "Role created successfully";
                }
                else
                {
                    //model.UpdatedDate = DateTime.Now;
                    await _roleService.UpdateOnlyAsync(model,x=>x.RoleName, x => x.Description, x => x.IsActive);

                    message = "Role Updated successfully";
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

        [HttpGet]
        public async Task<IActionResult> RolePermissions(int roleId)
        {
            var rolePermissions = await _roleService.GetRoleWisePermissionsAsync(roleId,TenantId);
            return PartialView("_RolePermissionModal", rolePermissions);
        }

        [HttpPost]
        public async Task<IActionResult> SaveRolePermissions([FromBody] RoleWisePermissionDto model)
        {
            if (model == null) return BadRequest("Invalid data");
            model.TenantId = TenantId; 
            await _roleService.SavePermissionsByRoleAsync(model);
            return Json(new { success = true, message = "Permissions saved successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id,bool Confirm=false)
        {
            if(!Confirm)
                return PartialView("_ModalDelete");
            var result = await _roleService.GetByIdAsync(Id);
            if (result!=null)
            {
                // Soft Delete Implementation
                result.IsActive = false;
                await _roleService.DeleteAsync(result);
                return Json(new { success = true, message = "Role Deleted Successfully.", });
            }
            else
                return Json(new { success = false, message = "Error deleting Role." });
        }

        
    }
}
