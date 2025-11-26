using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using SCADashboard.Core.Interfaces;
using SCADashboard.Core.Models;
using SCADashboard.Core.ModelView;

namespace SCADashboard.Infrastructure.Repositories
{
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(SCADashboardDbContext context) : base(context)
        { }

        public async Task<IEnumerable<Role>> GetRolesAsync(int TenantId)
        {
            return await _context.Roles.AsNoTracking()
                .Where(x => x.IsActive && x.TenantId== TenantId)
                .Select(x => new Role
                {
                    RoleId = x.RoleId,
                    RoleName = x.RoleName
                }).ToListAsync();
        }
        public async Task SaveRolePermissionsAsync(RoleWisePermissionDto dto)
        {
            try
            {
                foreach (var module in dto.Modules)
                {
                    foreach (var page in module.Pages)
                    {
                        var existing = await _context.RolePagePermissions.AsNoTracking()
                            .FirstOrDefaultAsync(r => r.RoleId == dto.RoleId && r.PageId == page.PageId);

                        if (existing != null)
                        {
                            existing.CanView = page.CanView;
                            existing.CanEdit = page.CanEdit;
                            existing.CanDelete = page.CanDelete;
                            existing.CanAdd = page.CanAdd;
                            _context.RolePagePermissions.Update(existing);
                        }
                        else
                        {
                            _context.RolePagePermissions.Add(new RolePagePermission
                            {
                                TenantId=dto.TenantId,
                                RoleId = dto.RoleId,
                                PageId = page.PageId,
                                CanView = page.CanView,
                                CanEdit = page.CanEdit,
                                CanAdd = page.CanAdd,
                                CanDelete = page.CanDelete
                            });
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
            

            
        }

        public async Task<RoleWisePermissionDto> GetRoleWisePermissionsAsync(int RoleId,int TenantId)
        {

            //Get Specific Role with all Modules and Pages along with their permissions
                        var result = await _context.Roles.AsNoTracking()
                    .Where(r => r.RoleId == RoleId && r.TenantId== TenantId)
                .Select(r => new RoleWisePermissionDto
                {
                    RoleId = r.RoleId,
                    RoleName = r.RoleName,
                    Modules = _context.Modules
                        .Select(m => new ModulePermissionDto
                        {
                            ModuleId = m.ModuleId,
                            ModuleName = m.ModuleName,
                            Pages = m.Pages.Select(p => new PagePermissionDto
                            {
                                PageId = p.PageId,
                                PageName = p.PageName,
                                // Match permissions if they exist, else false
                                CanView = r.RolePagePermissions
                                    .Where(rpp => rpp.PageId == p.PageId)
                                    .Select(rpp => rpp.CanView ?? false)
                                    .FirstOrDefault(),
                                CanAdd = r.RolePagePermissions
                                    .Where(rpp => rpp.PageId == p.PageId)
                                    .Select(rpp => rpp.CanAdd ?? false)
                                    .FirstOrDefault(),
                                CanEdit = r.RolePagePermissions
                                    .Where(rpp => rpp.PageId == p.PageId)
                                    .Select(rpp => rpp.CanEdit ?? false)
                                    .FirstOrDefault(),
                                CanDelete = r.RolePagePermissions
                                    .Where(rpp => rpp.PageId == p.PageId)
                                    .Select(rpp => rpp.CanDelete ?? false)
                                    .FirstOrDefault()
                            }).ToList()
                        }).ToList()
                })
                .FirstOrDefaultAsync();

            return result;
        }

    }
}
