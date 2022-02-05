using BlogEngine.DataTransferObject;
using BlogEngine.Web.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
//using Newtonsoft.Json;


namespace BlogEngine.Controllers
{
    public class HomeController : Controller
    {
        private readonly IControllerHelpers _controllerHelpers = null;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(IControllerHelpers controllerHelpers, IHttpClientFactory httpClientFactory)
        {
            _controllerHelpers = controllerHelpers;
            _httpClientFactory = httpClientFactory;
        }
        // GET: Posts
        public async Task<IActionResult> Index(int? pageNumber)
        {
            var pageSize = 2;
            var posts = new List<PostViewModel>();
            try
            {
                var response = await _controllerHelpers.GetAsync("Posts");
                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    var p = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<PostViewModel>>(responseStream);
                    posts = p.ToList();
                }
                else
                {
                    posts = Array.Empty<PostViewModel>().ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return View(PaginatedList<PostViewModel>.CreateAsync(posts, pageNumber ?? 1, pageSize));
            //var model = posts.OrderByDescending(c => c.TimeStamp).Skip(0).Take(2);
            //return View(model);
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = new PostViewModel();
            try
            {
                //var accessToken = await HttpContext.GetTokenAsync("access_token");
                //var idToken = await HttpContext.GetTokenAsync("id_token");
                //var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

                //var claims = User.Claims.ToList();
                //var _accessToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
                //var _idToken = new JwtSecurityTokenHandler().ReadJwtToken(idToken);


                var response = await _controllerHelpers.GetAsync("Posts/"+id);
                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    post = await System.Text.Json.JsonSerializer.DeserializeAsync<PostViewModel>(responseStream);
                }
            }
            catch (Exception ex)
            {
                post = new PostViewModel();
            }

            return View(post);
        }

        public async Task<IActionResult> About()
        {
            var profile = new Profile
            {
                Content = "A son, a hushband, a father and a software developer",
                UserName = "Huy Ho",
                Facebook = "#",
                Youtube = "https://www.youtube.com/channel/UCvLAedWEOD_35UqPl7jV7pQ?view_as=subscriber",
                LinkedIn = "https://www.linkedin.com/in/huy-ho/",
                Twitter = "",
                ImageUrl = "/content/images/about.jpg"
            };
            return View(profile);
        }
        [HttpPost]
        public async Task<IActionResult> About(Profile profile)
        {
            return View(profile);
        }

    }
}