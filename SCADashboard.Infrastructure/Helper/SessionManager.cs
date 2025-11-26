using Microsoft.AspNetCore.Http;
using SCADashboard.Core.ModelView;
using System.Text;
using System.Text.Json;

namespace SCADashboard.Infrastructure.Helper
{
    public static class SessionManager
    {
        private const string UserSessionKey ="UserSession";

        // Save user session object
        public static void SetUserSession(ISession session, UserSession userSession)
        {
            string jsonData = JsonSerializer.Serialize(userSession);
            session.Set(UserSessionKey, Encoding.UTF8.GetBytes(jsonData));
        }

        // Retrieve user session
        public static UserSession? GetUserSession(ISession session)
        {
            if (!session.TryGetValue(UserSessionKey, out var data))
                return null;

            var jsonData = Encoding.UTF8.GetString(data);
            return JsonSerializer.Deserialize<UserSession>(jsonData);
        }

        // Clear user session
        public static void ClearUserSession(ISession session)
        {
            foreach (var item in session.Keys)
            {
                session.Remove(item);
            }
            
        }

        // ----------------------------
        // Extra utility methods
        // ----------------------------

        //public static bool HasModulePermission(ISession session, string moduleName)
        //{
        //    var userSession = GetUserSession(session);
        //    if (userSession == null) return false;
        //    return userSession.ModulePermission.Contains(moduleName, StringComparer.OrdinalIgnoreCase);
        //}

        //public static bool HasPagePermission(ISession session, string pageName)
        //{
        //    var userSession = GetUserSession(session);
        //    if (userSession == null) return false;
        //    return userSession.PagePermission.Contains(pageName, StringComparer.OrdinalIgnoreCase);
        //}

        //public static string? GetRole(ISession session)
        //{
        //    return GetUserSession(session)?.Role;
        //}

        //public static string? GetUserName(ISession session)
        //{
        //    return GetUserSession(session)?.UserName;
        //}

        //public static int? GetUserId(ISession session)
        //{
        //    return GetUserSession(session)?.UserId;
        //}
    }
}
