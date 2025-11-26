using Microsoft.EntityFrameworkCore;
using SCADashboard.Core.Interfaces;
using SCADashboard.Core.Models;
using SCADashboard.Core.ModelView;
using SCADashboard.Infrastructure.Helper;

namespace SCADashboard.Infrastructure.Repositories
{
    public class LoginRepository : BaseRepository<User>, ILoginRepository
    {
        public LoginRepository(SCADashboardDbContext context) : base(context)
        {
        }
        public async Task<User?> GetByEmailAndPasswordAsync(string email, string password)
        {
            return await Task.FromResult(_context.Users.AsNoTracking().FirstOrDefault(u => u.Email == email && u.PasswordHash == password));
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await Task.FromResult(_context.Users.AsNoTracking().FirstOrDefault(u => u.Email == email  && u.IsActive));
        }

        public async Task<UserSession?> GetBypermissionByRoleAsync(string email, string password)
        {
            // Step 1: Find user
            var user = await _context.Users.AsNoTracking()
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => (u.Email == email || u.Username == email) && u.IsActive);
                //.FirstOrDefaultAsync(u => (u.Email == email || u.Username == email) && u.PasswordHash == password && u.IsActive);

            if (user == null )
                return null;
            bool passwordValid = PasswordHelper.VerifyPassword(password, user.PasswordHash);
            if (!passwordValid)
                return null;


            // Update User Login Status
            user.IsLogin = true;
            user.LoginDateTime = DateTime.Now;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();


            // Step 2: Collect roles (in case user has multiple roles)
            var roleIds = user.UserRoles.Select(ur => ur.RoleId).ToList();

            // Step 3: Get all permissions linked to those roles
            var permissionData = await (from mod in _context.Modules
                                        join pg in _context.Pages on mod.ModuleId equals pg.ModuleId
                                        join perm in _context.RolePagePermissions on pg.PageId equals perm.PageId
                                        where roleIds.Contains(perm.RoleId) && pg.IsActive
                                        select new
                                        {
                                            mod.ModuleName,
                                            mod.Icon,
                                            mod.IsChild,
                                            pg.PageName,
                                            pg.PageUrl,
                                            perm.CanView,
                                            perm.CanAdd,
                                            perm.CanEdit,
                                            perm.CanDelete
                                        })
                                        .AsNoTracking()
                                        .ToListAsync();

            // Step 4: Group by module and map to DTOs
            var grouped = permissionData
                .GroupBy(x => x.ModuleName)
                .Select(g => new ModulePermissionDto
                {
                    ModuleName = g.Key,
                    ModuleIcon = g.First().Icon,
                    IsChild = g.First().IsChild ??0,
                    Pages = g
                        .Where(p =>
                            (p.CanView ?? false) ||
                            (p.CanAdd ?? false) ||
                            (p.CanEdit ?? false) ||
                            (p.CanDelete ?? false)
                        )
                        .Select(p => new PagePermissionDto
                        {
                            PageName = p.PageName,
                            PageUrl = p.PageUrl,
                            CanView = p.CanView ?? false,
                            CanAdd = p.CanAdd ?? false,
                            CanEdit = p.CanEdit ?? false,
                            CanDelete = p.CanDelete ?? false
                        })
                        .ToList()
                })
                // Filter out modules that have no visible pages
                .Where(m => m.Pages.Any())
                .ToList();

            // Step 5: Build user session
            var session = new UserSession
            {
                UserId = user.UserId,
                UserEmail = user.Email,
                FullName = user.FullName,
                UserName= user.Username,
                TenantId =user.TenantId,
                RoleName = string.Join(", ", user.UserRoles.Select(ur => ur.Role.RoleName)),
                Permissions = grouped
            };

            return session;
        }


    }
}
