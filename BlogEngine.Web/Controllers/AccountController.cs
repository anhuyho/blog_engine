using BlogEngine.DataTransferObject;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlogEngine.Web.Controllers
{
    public class AccountController : Controller
    {
        [Authorize]
        public IActionResult Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            //await _signInManager.SignOutAsync();
            //return RedirectToAction("Index", "Home");
            await HttpContext.SignOutAsync("Cookie");
            var identityServerUri = Contanst.IdentityServerEndPoint;
            return RedirectToAction("Index", "Home");
        }



        //[HttpPost]
        //public IActionResult Login(LoginViewModel model)
        //{
        //    if (model.Email.ToLower() == "an.huy90@gmail.com" && model.Password == "123456") ;
        //        //HttpContext.User.s
        //    return RedirectToAction("Index", "Home");
        //}
        //public IActionResult SignUp()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult SignUp(SignUpViewModel model)
        //{
        //    return RedirectToAction("Index", "Home");
        //}
    }
}