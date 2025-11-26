using SCADashboard.Core.Models;
using SCADashboard.Core.ModelView;

namespace SCADashboard.Core.Interfaces
{
    public interface ILoginService:IBaseService<User>
    {
        Task<User?> AuthenticateAsync(string email, string password);
        Task<User?> GetByEmailAsync(string email);

        Task<UserSession> AuthenticateUserRoleAsync(string email, string password);
    }
}
