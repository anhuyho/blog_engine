using System;
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
using Newtonsoft.Json;

//using Newtonsoft.Json;


namespace BlogEngine.Web.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Authorize]
    public class PanelController : Controller
    {

        private readonly IControllerHelpers _controllerHelpers = null;
        private readonly IHttpClientFactory _httpClientFactory;

        public PanelController(IControllerHelpers controllerHelpers, IHttpClientFactory httpClientFactory)
        {
            _controllerHelpers = controllerHelpers;
            _httpClientFactory = httpClientFactory;
        }

        [Authorize]
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
                var baseUri = Contanst.ApiEndPoint;

                var uri = $"{baseUri}/api/Posts";
                //var request = new HttpRequestMessage(method, uri);
                var content = new StringContent(JsonConvert.SerializeObject(post), System.Text.Encoding.UTF8, "application/json");

                var client = _httpClientFactory.CreateClient();
                client.SetBearerToken(accessToken);


                var response = await client.PostAsync(uri, content);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index","Home");
                }
            }
            return View(post);
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
                    var baseUri = Contanst.ApiEndPoint;
                    var method = HttpMethod.Post;
                    var uri = $"{baseUri}/api/Posts/" + id;
                    //var request = new HttpRequestMessage(method, uri);
                    var content = new StringContent(JsonConvert.SerializeObject(post), System.Text.Encoding.UTF8, "application/json");

                    var client = _httpClientFactory.CreateClient();
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

        //// GET: Posts/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var post = await _context.BlogPosts
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (post == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(post);
        //}

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