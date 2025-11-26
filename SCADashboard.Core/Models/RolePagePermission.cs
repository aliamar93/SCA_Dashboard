using System;
using System.Collections.Generic;

namespace SCADashboard.Core.Models;

public partial class RolePagePermission
{
    public int RolePagePermissionId { get; set; }

    public int TenantId { get; set; }

    public int RoleId { get; set; }

    public int PageId { get; set; }

    public bool? CanView { get; set; }

    public bool? CanAdd { get; set; }

    public bool? CanEdit { get; set; }

    public bool? CanDelete { get; set; }

    public virtual Page Page { get; set; } = null!;

    public virtual Role Role { get; set; } = null!;

    public virtual Tenant Tenant { get; set; } = null!;
}
