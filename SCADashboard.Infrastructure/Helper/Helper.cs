using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using SCADashboard.Core.ModelView;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;

namespace SCADashboard.Infrastructure.Helper
{
    public static class PasswordHelper
    {
        private const int WorkFactor = 12;

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, WorkFactor);
        }

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }

    public static class PermissionHelper
    {
        public static PagePermissionDto? GetPagePermission(List<ModulePermissionDto> modules, string pageNameOrUrl)
        {
            return modules?
                .SelectMany(m => m.Pages)
                .FirstOrDefault(p =>
                    p.PageName.Equals(pageNameOrUrl, StringComparison.OrdinalIgnoreCase) ||
                    p.PageUrl.Equals(pageNameOrUrl, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Get page permission from current session and current URL
        /// </summary>
        /// <param name="httpContext">Current HttpContext</param>
        /// <returns>PagePermissionDto or null if not found</returns>
        public static PagePermissionDto GetCurrentPagePermission(HttpContext httpContext)
        {
            if (httpContext == null)
                return null;

            var sessionJson = httpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(sessionJson))
                return null;

            var userSession = JsonConvert.DeserializeObject<UserSession>(sessionJson);
            var modules = userSession?.Permissions ?? new List<ModulePermissionDto>();

            // Get current page URL
            var currentUrl = httpContext.Request.Path.Value?.ToLower();

            // Find page permission
            return modules
                .SelectMany(m => m.Pages)
                .FirstOrDefault(p => p.PageUrl?.ToLower() == currentUrl);
        }

        /// <summary>
        /// Optional helper to check a specific permission type
        /// </summary>
        public static bool HasPermission(HttpContext httpContext, string permissionType)
        {
            var pagePermission = GetCurrentPagePermission(httpContext);
            if (pagePermission == null)
                return false;

            return permissionType switch
            {
                "CanView" => pagePermission.CanView,
                "CanAdd" => pagePermission.CanAdd,
                "CanEdit" => pagePermission.CanEdit,
                "CanDelete" => pagePermission.CanDelete,
                _ => false
            };
        }

        public static List<ModulePermissionDto> GetModulePermission(HttpContext httpContext)
        {
            if (httpContext == null)
                return null;

            var sessionJson = httpContext.Session.GetString("UserSession");
            if (string.IsNullOrEmpty(sessionJson))
                return null;

            var userSession = JsonConvert.DeserializeObject<UserSession>(sessionJson);
            var modules = userSession?.Permissions ?? new List<ModulePermissionDto>();

            return modules.ToList();
        }
    }

    public static class GUID
    {
        public static Guid Int2Guid(int value)
        {
            // Convert the integer to bytes
            byte[] bytes = new byte[16];      // GUID is 16 bytes
            BitConverter.GetBytes(value).CopyTo(bytes, 0); // put int in first 4 bytes
            // Remaining 12 bytes are zeros (or you can fill with any pattern)
            Guid guid = new Guid(bytes);
            return guid;
        }

        public static int Guid2Int(Guid value)
        {
            byte[] b = value.ToByteArray();
            int bint = BitConverter.ToInt32(b, 0);
            return bint;
        }
    }

    public static class ClientInfo
    {
        public static string GetClientIp(HttpContext context)
        {
            var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(forwardedFor))
            {
                return forwardedFor.Split(',')[0]; // real client IP
            }

            return context.Connection.RemoteIpAddress?.ToString();
        }
        //-------Install-Package UAParser
        //public static (string Browser, string OS, string Device) ParseUserAgent(string userAgent)
        //{
        //    var uaParser = Parser.GetDefault();
        //    var client = uaParser.Parse(userAgent);

        //    string browser = $"{client.UA.Family} {client.UA.Major}";
        //    string os = $"{client.OS.Family} {client.OS.Major}";
        //    string device = client.Device.Family;

        //    return (browser, os, device);
        //}

    }

    //public static class ClientInfoHelper
    //{
    //    public static string GetClientIp(HttpContext context)
    //    {
    //        var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

    //        if (!string.IsNullOrWhiteSpace(forwardedFor))
    //        {
    //            return forwardedFor.Split(',')[0];
    //        }

    //        return context.Connection.RemoteIpAddress?.ToString();
    //    }
    [AttributeUsage(AttributeTargets.Field)]
    public class statusTypeAttribute : Attribute
    {
        public string Text { get; }
        public statusTypeAttribute(string text)
        {
            Text = text;
        }
    }

    public enum UserStatus
    {
        Active = 1,
        InActive = 2,
        Suspended = 3
    }

    public enum UserStatusEnum
    {
        [statusType("bg-success")]
        DotSuccess = 1,
        [statusType("bg-warning")]
        DotWarning = 2,
        [statusType("bg-danger")]
        DotDanger = 3,


        [statusType("text-success")]
        TextSuccess = 4,
        [statusType("text-warning")]
        TextWarning = 5,
        [statusType("text-danger")]
        TextDanger = 6,
        [statusType("text-info")]
        TextInfo = 7,
    }

    public enum Icons
    {
        [statusType("text-info ni ni-alarm-alt")]
        AlarmTextIcon = 1,
        [statusType("text-success ni ni-check-circle")]
        CircleTextIcon = 2,
        [statusType("text-warning ni ni-alert-circle")]
        AlertTextIcon = 3,
        [statusType("ni ni-wallet-fill")]
        WalletIcon = 4,
        [statusType("ni ni-mail-fill")]
        MailIcon = 5,
        [statusType("ni ni-user-cross-fill")]
        UserIcon = 6,
        [statusType("ni ni-na")]
        TextInfo = 7,
        [statusType("ni ni-check-thick")]
        CheckIcon = 8,
        [statusType("ni ni-eye-fill")]
        EyeIcon = 9,
        [statusType("ni ni-shield-off")]
        ShieldIcon = 10,
        [statusType("ni ni-check-fill-c")]
        CheckFillIcon = 11,
        [statusType("ni ni-cross-fill-c")]
        CrossIcon = 12,
        [statusType("ni ni-alert-circle")]
        AlertIcon = 13,
        [statusType("ni ni-edit-fill")]
        EditIcon = 14,
        [statusType("ni ni-trash")]
        TrashIcon = 15,
        [statusType("ni ni-edit")]
        PermissionIcon = 16,
        //


    }
    public static class EnumExtensions
    {
        public static string GetText(this Enum value)
        {
            var type = value.GetType();
            var memInfo = type.GetMember(value.ToString());
            var attribute = memInfo[0]
                .GetCustomAttributes(typeof(statusTypeAttribute), false)
                .FirstOrDefault() as statusTypeAttribute;

            return attribute?.Text ?? value.ToString();
        }
        public static UserStatusEnum ToEnum(this string value)
        {
            return (UserStatusEnum)Enum.Parse(typeof(UserStatusEnum), value);
        }
    }
}
