using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace SCADashboard.Core.Models;

public partial class SCADashboardDbContext : DbContext
{
    private readonly IConfiguration? _configuration;
    public SCADashboardDbContext()
    {
    }

    public SCADashboardDbContext(DbContextOptions<SCADashboardDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Kunde> Kundes { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<Page> Pages { get; set; }

    public virtual DbSet<PasswordLog> PasswordLogs { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePagePermission> RolePagePermissions { get; set; }

    public virtual DbSet<Tenant> Tenants { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<UserValidation> UserValidations { get; set; }

    public virtual DbSet<Userkunde> Userkundes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured && _configuration != null)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.AuditId).HasName("PK__AuditLog__A17F239810775EE3");

            entity.Property(e => e.Action)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(45)
                .IsUnicode(false)
                .HasColumnName("IPAddress");
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Tenant).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AuditLogs__Tenan__5DCAEF64");

            entity.HasOne(d => d.User).WithMany(p => p.AuditLogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AuditLogs__UserI__5EBF139D");
        });

        modelBuilder.Entity<Kunde>(entity =>
        {
            entity.ToTable("Kunde");

            entity.Property(e => e.KundeNo).HasMaxLength(200);
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.ModuleId).HasName("PK__Modules__2B7477A7AA82A89B");

            entity.Property(e => e.Icon)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.ModuleName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.OrderIndex).HasDefaultValue(0);

            entity.HasOne(d => d.Tenant).WithMany(p => p.Modules)
                .HasForeignKey(d => d.TenantId)
                .HasConstraintName("FK__Modules__TenantI__66603565");
        });

        modelBuilder.Entity<Page>(entity =>
        {
            entity.HasKey(e => e.PageId).HasName("PK__Pages__C565B104C0DAADEA");

            entity.Property(e => e.PageId).ValueGeneratedNever();
            entity.Property(e => e.ComponentName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.OrderIndex).HasDefaultValue(0);
            entity.Property(e => e.PageName)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.PageUrl)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Module).WithMany(p => p.Pages)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pages__ModuleId__60A75C0F");

            entity.HasOne(d => d.Tenant).WithMany(p => p.Pages)
                .HasForeignKey(d => d.TenantId)
                .HasConstraintName("FK__Pages__TenantId__619B8048");
        });

        modelBuilder.Entity<PasswordLog>(entity =>
        {
            entity.ToTable("PasswordLog");

            entity.Property(e => e.ChangePasswordDt)
                .HasColumnType("datetime")
                .HasColumnName("ChangePasswordDT");
            entity.Property(e => e.NewPassword).HasMaxLength(250);
            entity.Property(e => e.OldPassword).HasMaxLength(250);
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.PermissionId).HasName("PK__Permissi__EFA6FB2F28527080");

            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PermissionName)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1A4CD15E20");

            entity.HasIndex(e => new { e.TenantId, e.RoleName }, "UQ_Role_Tenant").IsUnique();

            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.RoleName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Tenant).WithMany(p => p.Roles)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Roles__TenantId__6C190EBB");
        });

        modelBuilder.Entity<RolePagePermission>(entity =>
        {
            entity.HasKey(e => e.RolePagePermissionId).HasName("PK__RolePage__E3DB7019A0500EEB");

            entity.HasIndex(e => new { e.TenantId, e.RoleId, e.PageId }, "UQ_RolePage_Tenant").IsUnique();

            entity.Property(e => e.CanAdd).HasDefaultValue(false);
            entity.Property(e => e.CanDelete).HasDefaultValue(false);
            entity.Property(e => e.CanEdit).HasDefaultValue(false);
            entity.Property(e => e.CanView).HasDefaultValue(false);

            entity.HasOne(d => d.Page).WithMany(p => p.RolePagePermissions)
                .HasForeignKey(d => d.PageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RolePageP__PageI__628FA481");

            entity.HasOne(d => d.Role).WithMany(p => p.RolePagePermissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RolePageP__RoleI__6383C8BA");

            entity.HasOne(d => d.Tenant).WithMany(p => p.RolePagePermissions)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RolePageP__Tenan__6477ECF3");
        });

        modelBuilder.Entity<Tenant>(entity =>
        {
            entity.HasKey(e => e.TenantId).HasName("PK__Tenants__2E9B47E139276B21");

            entity.Property(e => e.ContactEmail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ContactPhone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Domain)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.TenantName)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4C20858E84");

            entity.HasIndex(e => new { e.TenantId, e.Username }, "UQ_User_Tenant").IsUnique();

            entity.Property(e => e.AddressLine1).HasMaxLength(250);
            entity.Property(e => e.AddressLine2).HasMaxLength(250);
            entity.Property(e => e.Country).HasMaxLength(250);
            entity.Property(e => e.CreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Ipaddress)
                .HasMaxLength(250)
                .HasColumnName("IPAddress");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsLogin).HasDefaultValue(false);
            entity.Property(e => e.LoginDateTime).HasColumnType("datetime");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNo).HasMaxLength(50);
            entity.Property(e => e.State).HasMaxLength(250);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Tenant).WithMany(p => p.Users)
                .HasForeignKey(d => d.TenantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__TenantId__6A30C649");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.UserRoleId).HasName("PK__UserRole__3D978A35EE5480C7");

            entity.HasIndex(e => new { e.UserId, e.RoleId }, "UQ_UserRole").IsUnique();

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRoles__RoleI__6EF57B66");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserRoles__UserI__693CA210");
        });

        modelBuilder.Entity<UserValidation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UserEmailValid");

            entity.ToTable("UserValidation");

            entity.Property(e => e.DateTime).HasColumnType("datetime");
            entity.Property(e => e.EmailValidation).HasMaxLength(50);
            entity.Property(e => e.Otpvalidation)
                .HasMaxLength(50)
                .HasDefaultValueSql("((0))")
                .HasColumnName("OTPValidation");
        });

        modelBuilder.Entity<Userkunde>(entity =>
        {
            entity.ToTable("Userkunde");

            entity.HasOne(d => d.Kunde).WithMany(p => p.Userkundes)
                .HasForeignKey(d => d.KundeId)
                .HasConstraintName("FK_Userkunde_Kunde");

            entity.HasOne(d => d.User).WithMany(p => p.Userkundes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Userkunde_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
