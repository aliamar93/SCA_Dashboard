using SCADashboard.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SCADashboard.Infrastructure.ModelView.UserSession;

namespace SCADashboard.Infrastructure.ModelView
{
    // User session object
    public class UserSession
    {
        public int UserId { get; set; }
        public string UserEmail { get; set; }     // User's email
        public string RoleName { get; set; }      // Role name

        public List<ModulePermissionDto> Permissions { get; set; } = new List<ModulePermissionDto>();

    }

    // Top-level DTO: Module with its pages
    public class ModulePermissionDto
    {
        public string ModuleName { get; set; }
        public List<PagePermissionDto> Pages { get; set; } = new List<PagePermissionDto>();
    }

    // Page-level DTO: Page with its permissions
    public class PagePermissionDto
    {
        public string PageName { get; set; }
        public string PageUrl { get; set; }
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }
}
