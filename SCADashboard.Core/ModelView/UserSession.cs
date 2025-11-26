using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCADashboard.Core.ModelView
{
    // User session object
    public class UserSession
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string UserEmail { get; set; }     // User's email
        public string RoleName { get; set; }      // Role name
        public int TenantId { get; set; }

        public List<int> kunde { get; set; }
        public List<ModulePermissionDto> Permissions { get; set; } = new List<ModulePermissionDto>();

    }

    public class RoleWisePermissionDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int TenantId { get; set; }
        public List<ModulePermissionDto> Modules { get; set; }
    }

    // Top-level DTO: Module with its pages
    public class ModulePermissionDto
    {
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string ModuleIcon { get; set; }
        public int IsChild { get; set; }
        public List<PagePermissionDto> Pages { get; set; } = new List<PagePermissionDto>();
    }

    // Page-level DTO: Page with its permissions
    public class PagePermissionDto
    {
        public int PageId { get; set; }
        public string PageName { get; set; }
        public string PageUrl { get; set; }
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }
}
