using SCADashboard.Core.Interfaces;
using SCADashboard.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADashboard.Application.Services
{
    public class PasswordLogService:BaseService<PasswordLog>, IPasswordLogService
    {
        private readonly IPasswordLogRepository _passwordLogRepository;

        public PasswordLogService(IPasswordLogRepository passwordLogRepository):base(passwordLogRepository)
        {
            _passwordLogRepository = passwordLogRepository;
        }
    }
}
