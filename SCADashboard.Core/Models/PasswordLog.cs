using System;
using System.Collections.Generic;

namespace SCADashboard.Core.Models;

public partial class PasswordLog
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? OldPassword { get; set; }

    public string? NewPassword { get; set; }

    public DateTime? ChangePasswordDt { get; set; }
}
