using SCADashboard.Core.Models;
using SCADashboard.Core.ModelView;

namespace SCADashboard.Core.Interfaces
{
    public interface IRoleService:IBaseService<Core.Models.Role>
    {
        Task<IEnumerable<Role>> GetRoleAsync(int TenantId);
        Task SavePermissionsByRoleAsync(RoleWisePermissionDto dto);
        Task<RoleWisePermissionDto> GetRoleWisePermissionsAsync(int RoleId, int TenantId);
    }
}
