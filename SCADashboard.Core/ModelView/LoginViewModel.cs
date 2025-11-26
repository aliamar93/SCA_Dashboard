using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADashboard.Core.ModelView
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string captcha { get; set; }
    }
}
