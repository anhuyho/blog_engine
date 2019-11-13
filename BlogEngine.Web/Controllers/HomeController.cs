using BlogEngine.DataTransferObject;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlogEngine.Controllers
{
    public class HomeController: Controller
    {
        IHttpClientFactory _clientFactory;
        public HomeController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                //var post = _repo.GetAllPost();
                var post = new List<PostViewModel>();
                var method = HttpMethod.Get;
                var requestUri = "https://localhost:5001/api/Post";
                var request = new HttpRequestMessage(method, requestUri);
                var client = _clientFactory.CreateClient();

                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    var p = await JsonSerializer.DeserializeAsync<IEnumerable<PostViewModel>>(responseStream);
                    post = p.ToList();
                }
                else
                {
                    post = Array.Empty<PostViewModel>().ToList();
                }
                return View(post);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }
        //public IActionResult Post(int id)
        //{
        //    var post = _repo.GetPost(id);
        //    return View(post);
        //}
        //[HttpGet]
        //public IActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return View(new PostViewModel());
        //    }
        //    var post = _repo.GetPost((int)id);
        //    return View(post);
        //}
        //[HttpPost]
        //public async Task<IActionResult> Edit(PostViewModel post)
        //{
        //    if (post.Id > 0)
        //    {
        //        _repo.UpdatePost(post);
        //    }
        //    else
        //    {
        //        _repo.AddPost(post);
        //    }

        //    if (await _repo.SaveChangesAsync())
        //    {
        //        return RedirectToAction("Index");
        //    }
        //    return View();
        //}

        //public async Task<IActionResult> Remove(int id)
        //{
        //    _repo.RemovePost(id);
        //    await _repo.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}
        //[HttpGet("/Image/{image}")]
        //public IActionResult Image(string image)
        //{
        //    var mine = image.Substring(image.LastIndexOf('.') + 1);
        //    return new FileStreamResult(_fileManager.ImageStream(image), $"image/{mine}");
        //}
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Travel()
        {
            return View();
        }
        public IActionResult Photo()
        {
            return View();
        }
    }
}
