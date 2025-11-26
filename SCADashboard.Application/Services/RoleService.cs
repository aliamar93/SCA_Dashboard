using Microsoft.EntityFrameworkCore;
using SCADashboard.Core.Interfaces;
using SCADashboard.Core.Models;
using SCADashboard.Core.ModelView;

namespace SCADashboard.Application.Services
{
    public class RoleService :BaseService<Role>, IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository):base (roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<Role>> GetRoleAsync(int TenantId)
        {
            return await _roleRepository.GetRolesAsync(TenantId);
        }

        public async Task<RoleWisePermissionDto> GetRoleWisePermissionsAsync(int RoleId,int tenantId)
        {
            return await _roleRepository.GetRoleWisePermissionsAsync(RoleId, tenantId);
        }

        public async Task SavePermissionsByRoleAsync(RoleWisePermissionDto dto)
        {

            await _roleRepository.SaveRolePermissionsAsync(dto);
            //foreach (var module in dto.Modules)
            //{
            //    foreach (var page in module.Pages)
            //    {
            //        var existing = await _context.RolePagePermissions
            //            .FirstOrDefaultAsync(r => r.RoleId == dto.RoleId && r.PageId == page.PageId);

            //        if (existing != null)
            //        {
            //            existing.CanView = page.CanView;
            //            existing.CanEdit = page.CanEdit;
            //            _context.RolePagePermissions.Update(existing);
            //        }
            //        else
            //        {
            //            _context.RolePagePermissions.Add(new RolePagePermission
            //            {
            //                RoleId = dto.RoleId,
            //                PageId = page.PageId,
            //                CanView = page.CanView,
            //                CanEdit = page.CanEdit
            //            });
            //        }
            //    }
            //}

            //await _context.SaveChangesAsync();
        }
    }
}
