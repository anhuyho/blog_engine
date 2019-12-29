using System;
using BlogEngine.DataTransferObject;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using BlogEngine.Web.Helpers;
using IdentityModel.Client;
using Newtonsoft.Json;

namespace BlogEngine.Web.Controllers
{
    public class AccountController : Controller
    {
        private IConfiguration _configuration;
        private readonly Endpoint _endpoint;

        private readonly IControllerHelpers _controllerHelpers = null;
        private readonly IHttpClientFactory _httpClientFactory;
        public AccountController(IConfiguration configuration,
                IControllerHelpers controllerHelpers,
                IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _endpoint = new Endpoint(configuration);
            _httpClientFactory = httpClientFactory;
        }

        [Authorize]
        public async Task<IActionResult> Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var accessToken = await HttpContext.GetTokenAsync("access_token");
                var idToken = await HttpContext.GetTokenAsync("id_token");
                var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

                var claims = User.Claims.ToList();
                var _accessToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
                var _idToken = new JwtSecurityTokenHandler().ReadJwtToken(idToken);


                var baseUri = _endpoint.Api;

                var uri = $"{baseUri}/api/account";
                var client = _httpClientFactory.CreateClient();

                client.SetBearerToken(accessToken);

                var email = _idToken?.Claims?.Where(c => c.Type == "email").FirstOrDefault()?.Value;
                var name = _idToken?.Claims?.Where(c => c.Type == "name").FirstOrDefault()?.Value;

                var user = new UserViewModel
                {
                    Email = email,
                    Username = name
                };

                var content = new StringContent(JsonConvert.SerializeObject(user), System.Text.Encoding.UTF8, "application/json");


                client.SetBearerToken(accessToken);


                var response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    ViewBag.SiteName = user.SiteName;
                    return RedirectToAction("Profile", new { 
                    userName = user.Username
                    });
                }
                //MVC call to User API
                //API check identity server id to create new user or not into db
                //API return userid to MVC
                //redirect to Profile with userid


            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            //await _signInManager.SignOutAsync();
            //return RedirectToAction("Index", "Home");
            await HttpContext.SignOutAsync("Cookie");
            var identityServerUri = _endpoint.Id4;
            return RedirectToAction("Index", "Home");
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile(string userName)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var baseUri = _endpoint.Api;

            var uri = $"{baseUri}/api/profile";
            var client = _httpClientFactory.CreateClient();

            client.SetBearerToken(accessToken);


            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = await response.Content.ReadAsStreamAsync();
                var user = await System.Text.Json.JsonSerializer.DeserializeAsync<UserViewModel>(responseStream);
                return View(user);
            }

            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Profile(UserViewModel vm)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var baseUri = _endpoint.Api;

            var uri = $"{baseUri}/api/Profile";
            var client = _httpClientFactory.CreateClient();


            client.SetBearerToken(accessToken);
            var content = new StringContent(JsonConvert.SerializeObject(vm), System.Text.Encoding.UTF8, "application/json");
            client.SetBearerToken(accessToken);


            var response = await client.PutAsync(uri, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Panel");
            }
            return View();
        }
    }
}