
using SCADashboard.Core.Models;
using SCADashboard.Core.ModelView;

namespace SCADashboard.Core.Interfaces
{
    public interface ILoginRepository :IBaseRepository<User>
    {
        Task<User?> GetByEmailAndPasswordAsync(string email, string password);
        Task<User?> GetByEmailAsync(string email);

        Task<UserSession> GetBypermissionByRoleAsync(string email, string password);
    }
}
