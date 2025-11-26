using SCADashboard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADashboard.Core.Interfaces
{
    public interface IUserService: IBaseService<Core.Models.User>
    {
        Task <bool> GetUserEmailAndUserNameAsync(string UserName, string Email, int UserId);
        Task<IEnumerable<User>> GetUserByRoleAsync(int TenantId);
        Task<User> SaveUser(User user);
        Task<User> GetUserWithRoleByIdAsync(int id);
    }
}
