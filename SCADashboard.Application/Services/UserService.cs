using SCADashboard.Core.Interfaces;
using SCADashboard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADashboard.Application.Services
{
    public class UserService:BaseService<User>,IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository) : base(userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> GetUserEmailAndUserNameAsync(string UserName, string Email,int UserId)
        {
            return await _userRepository.GetUserEmailAndUserNameAsync(UserName,Email,UserId);
        }

        public async Task<IEnumerable<User>> GetUserByRoleAsync(int TenantId)
        {
            return await _userRepository.GetUsersByRoleAsync(TenantId);
        }

        public async Task<User> SaveUser(User user)
        {
            return await _userRepository.AddAsync(user);
        }

        public async Task<User> GetUserWithRoleByIdAsync(int id)
        {
            return await _userRepository.GetUsersIncludeTablesByIdAsync(id);
        }
    }
}
