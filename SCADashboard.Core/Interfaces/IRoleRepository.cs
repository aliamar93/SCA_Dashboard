using SCADashboard.Core.Models;
using SCADashboard.Core.ModelView;


namespace SCADashboard.Core.Interfaces
{
    public interface IRoleRepository: IBaseRepository<Role>
    {
        Task<IEnumerable<Role>> GetRolesAsync(int TenantId);
        Task SaveRolePermissionsAsync(RoleWisePermissionDto dto);
        Task<RoleWisePermissionDto> GetRoleWisePermissionsAsync(int RoleId,int TenantId);
    }
}
