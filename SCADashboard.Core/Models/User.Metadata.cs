using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SCADashboard.Infrastructure.Helper;

namespace SCADashboard.Core.Models
{
    [ModelMetadataType(typeof(UserMetadata))]   // 👈 Link metadata to the entity
    public partial class User {}

    public class UserMetadata
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(100, ErrorMessage = "Username cannot exceed 100 characters.")]
        //[UniqueUsername]
        [Remote(action: "CheckUsername", controller: "User", AdditionalFields = nameof(UserId), ErrorMessage = "Username already exists.")]
        public string Username { get; set; } = null!;



        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        //[UniqueEmail]
        [Remote(action: "CheckEmail", controller: "User", AdditionalFields = nameof(UserId), ErrorMessage = "Email already registered.")]
        public string Email { get; set; } = null!;
    }
}
