using System;
using System.Collections.Generic;

namespace SCADashboard.Core.Models;

public partial class Page
{
    public int PageId { get; set; }

    public int ModuleId { get; set; }

    public int? TenantId { get; set; }

    public string PageName { get; set; } = null!;

    public string? PageUrl { get; set; }

    public string? ComponentName { get; set; }

    public int? OrderIndex { get; set; }

    public bool IsActive { get; set; }

    public virtual Module Module { get; set; } = null!;

    public virtual ICollection<RolePagePermission> RolePagePermissions { get; set; } = new List<RolePagePermission>();

    public virtual Tenant? Tenant { get; set; }
}
