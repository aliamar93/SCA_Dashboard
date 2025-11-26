using SCADashboard.Core.Interfaces;
using SCADashboard.Core.Models;
using SCADashboard.Core.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADashboard.Application.Services
{
    public class LoginService : BaseService<User>, ILoginService
    {
        private readonly ILoginRepository _loginRepository;
        public LoginService(ILoginRepository loginRepository):base(loginRepository)
        {
            _loginRepository = loginRepository;
        }
        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            return await _loginRepository.GetByEmailAndPasswordAsync(email, password);
        }

        public async Task<UserSession> AuthenticateUserRoleAsync(string email, string password)
        {
            return await _loginRepository.GetBypermissionByRoleAsync(email,password);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _loginRepository.GetByEmailAsync(email);
        }
    }
}
