using System;
using System.Collections.Generic;

namespace SCADashboard.Core.Models;

public partial class UserValidation
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Otpvalidation { get; set; } = null!;

    public string? EmailValidation { get; set; }

    public bool IsStatus { get; set; }

    public DateTime DateTime { get; set; }

    public bool IsExpire { get; set; }
}
