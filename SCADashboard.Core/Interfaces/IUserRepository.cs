using SCADashboard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADashboard.Core.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<bool> GetUserEmailAndUserNameAsync(string UserName, string Email,int UserId);
        Task<IEnumerable<User>> GetUsersByRoleAsync(int TenantId);
        Task<User> AddAsync(User user);
        Task<User> GetUsersIncludeTablesByIdAsync(int id);
    }
}
