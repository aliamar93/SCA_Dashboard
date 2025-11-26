using System;
using System.Collections.Generic;

namespace SCADashboard.Core.Models;

public partial class User
{
    public int UserId { get; set; }

    public int TenantId { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? FullName { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public bool? IsLogin { get; set; }

    public string? Ipaddress { get; set; }

    public DateTime? LoginDateTime { get; set; }

    public string? State { get; set; }

    public string? Country { get; set; }

    public string? AddressLine1 { get; set; }

    public string? AddressLine2 { get; set; }

    public string? PhoneNo { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public bool IsEmail { get; set; }

    public virtual ICollection<AuditLog> AuditLogs { get; set; } = new List<AuditLog>();

    public virtual Tenant Tenant { get; set; } = null!;

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    public virtual ICollection<Userkunde> Userkundes { get; set; } = new List<Userkunde>();
}
