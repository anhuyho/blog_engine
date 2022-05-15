using Microsoft.AspNetCore.Mvc;

namespace BlogEngine.Web.Controllers
{
    public class MyMVCControllerBase : Controller
    {
        public bool IsAuthentciated
        {
            get
            {
                var isAutheticated = HttpContext?.User?.Identity?.IsAuthenticated;
                return isAutheticated.HasValue && isAutheticated.Value;
            }
        }
    }
}