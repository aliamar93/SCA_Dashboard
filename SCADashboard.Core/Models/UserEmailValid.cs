using System;
using System.Collections.Generic;

namespace SCADashboard.Core.Models;

public partial class UserEmailValid
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public bool EmailValidation { get; set; }
}
