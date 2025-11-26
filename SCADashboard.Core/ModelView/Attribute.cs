using SCADashboard.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace SCADashboard.Infrastructure.Helper
{
    //public class UniqueUsernameAttribute : ValidationAttribute
    //{
    //    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    //    {
    //        if (value == null) return ValidationResult.Success;

    //        var dbContext = (AppDbContext)validationContext.GetService(typeof(AppDbContext))!;
    //        var entity = (User)validationContext.ObjectInstance;

    //        bool exists = dbContext.Users.Any(u => u.UserName == value.ToString() && u.UserId != entity.UserId);

    //        return exists ? new ValidationResult("Username already exists.") : ValidationResult.Success;
    //    }
    //}

    //public class UniqueEmailAttribute : ValidationAttribute
    //{
    //    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    //    {
    //        if (value == null) return ValidationResult.Success;

    //        var dbContext = (SCADashboardDBC)validationContext.GetService(typeof(AppDbContext))!;
    //        var entity = (User)validationContext.ObjectInstance;

    //        bool exists = dbContext.Users.Any(u => u.Email == value.ToString() && u.UserId != entity.UserId);

    //        return exists ? new ValidationResult("Email already registered.") : ValidationResult.Success;
    //    }
    //}
}
