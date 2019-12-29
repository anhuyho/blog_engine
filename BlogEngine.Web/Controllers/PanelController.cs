using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BlogEngine.DataTransferObject;
using BlogEngine.Web.Helpers;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BlogEngine.Web.Controllers
{
 
    [Authorize]
    public class PanelController : Controller
    {

        private readonly IControllerHelpers _controllerHelpers = null;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly Endpoint _endpoint;
        public PanelController(IControllerHelpers controllerHelpers, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _controllerHelpers = controllerHelpers;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _endpoint = new Endpoint(_configuration);
        }


        public async Task<IActionResult> Index()
        {
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
            return View(posts.OrderByDescending(c => c.TimeStamp));
        }


        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> Publish([Bind("Id,PostName,PostDescription,Content,TimeStamp")] PostViewModel post)
        {

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var idToken = await HttpContext.GetTokenAsync("id_token");
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            var claims = User.Claims.ToList();
            var _accessToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            var _idToken = new JwtSecurityTokenHandler().ReadJwtToken(idToken);

            if (ModelState.IsValid)
            {
                var baseUri = _endpoint.Api;

                var uri = $"{baseUri}/api/Posts";
                //var request = new HttpRequestMessage(method, uri);
                var content = new StringContent(JsonConvert.SerializeObject(post), System.Text.Encoding.UTF8, "application/json");

                var client = _httpClientFactory.CreateClient();
                client.SetBearerToken(accessToken);


                var response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStreamAsync();

                    return RedirectToAction("Index","Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Error", "Home"); ;
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = new PostViewModel();
            try
            {
                var response = await _controllerHelpers.GetAsync("Posts/" + id);
                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    post = await System.Text.Json.JsonSerializer.DeserializeAsync<PostViewModel>(responseStream);
                }
            }
            catch (Exception ex)
            {
                post = null;
            }
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PostName,PostDescription,Content,TimeStamp")] PostViewModel post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var baseUri = _endpoint.Api;
                    var uri = $"{baseUri}/api/Posts/" + id;

                    var content = new StringContent(JsonConvert.SerializeObject(post), System.Text.Encoding.UTF8, "application/json");

                    var client = _httpClientFactory.CreateClient();

                    var accessToken = await HttpContext.GetTokenAsync("access_token");
                    var idToken = await HttpContext.GetTokenAsync("id_token");
                    var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

                    var claims = User.Claims.ToList();
                    var _accessToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
                    var _idToken = new JwtSecurityTokenHandler().ReadJwtToken(idToken);

                    client.SetBearerToken(accessToken);

                    var response = await client.PutAsync(uri, content);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Details","Home",new { Id = id});
                    }
                }
                catch (Exception ex)
                {

                }
            }
            return View(post);
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([Bind("Id,PostName,PostDescription,Content,TimeStamp")] PostViewModel post)
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Preview([Bind("Id,PostName,PostDescription,Content,TimeStamp")] PostViewModel post)
        {
            return RedirectToAction("Index", "Home");
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var baseUri = _endpoint.Api;
            var uri = $"{baseUri}/api/Posts/" + id;

            var client = _httpClientFactory.CreateClient();

            var accessToken = await HttpContext.GetTokenAsync("access_token");
            client.SetBearerToken(accessToken);

            var response = await client.DeleteAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Panel");
            }
            return RedirectToAction("Index", "Panel");
        }

        //// POST: Posts/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var post = await _context.BlogPosts.FindAsync(id);
        //    _context.BlogPosts.Remove(post);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool PostExists(int id)
        //{
        //    return _context.BlogPosts.Any(e => e.Id == id);
        //}
    }
}