using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BlogEngine.DataTransferObject;

namespace BlogEngine.Web.Controllers
{
    public class AccountController : MyMVCControllerBase
    {
        IConfiguration _config;
        public AccountController(IConfiguration config)
        {
            _config = config;
        }
        [Authorize]
        public IActionResult Login()
        {
            var isAutheticated = HttpContext?.User?.Identity?.IsAuthenticated;
            if (IsAuthentciated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            var endPoint = new MyEndpoint(_config);
            //await _signInManager.SignOutAsync();
            //return RedirectToAction("Index", "Home");
            await HttpContext.SignOutAsync("Cookie");
            var identityServerUri = endPoint.Id4;
            return RedirectToAction("Index", "Home");
        }
    }
}