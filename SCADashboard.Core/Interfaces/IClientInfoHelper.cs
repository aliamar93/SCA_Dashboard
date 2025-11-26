
namespace SCADashboard.Core.Interfaces
{
    public interface IClientInfoHelper
    {
        string GetClientIp(Microsoft.AspNetCore.Http.HttpContext context);
        string GetBrowserName(Microsoft.AspNetCore.Http.HttpContext context);
        object ReadJsonFile(string Path);
    }
}
