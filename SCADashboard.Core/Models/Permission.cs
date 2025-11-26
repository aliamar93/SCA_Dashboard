using System;
using System.Collections.Generic;

namespace SCADashboard.Core.Models;

public partial class Permission
{
    public int PermissionId { get; set; }

    public string PermissionName { get; set; } = null!;

    public string? Description { get; set; }
}
