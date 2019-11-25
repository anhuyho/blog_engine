using BlogEngine.DataTransferObject;
using BlogEngine.Web.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly IControllerHelpers _controllerHelpers = null;
        public HomeController(IControllerHelpers controllerHelpers)
        {
            _controllerHelpers = controllerHelpers;
        }
        
        public async Task<IActionResult> Index()
        {
            var posts = new List<PostViewModel>();
            try
            {
                var response = await _controllerHelpers.GetAsync("Post");
                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    var p = await JsonSerializer.DeserializeAsync<IEnumerable<PostViewModel>>(responseStream);
                    posts = p.ToList();
                }
                else
                {
                    posts = Array.Empty<PostViewModel>().ToList();
                }
                return View(posts);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View();
        }
        public async Task<IActionResult> Post(int id)
        {
            var post = new PostViewModel();
            try
            {
                var response = await _controllerHelpers.GetAsync($"Post/{id}");
                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    post = await JsonSerializer.DeserializeAsync<PostViewModel>(responseStream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return View(post);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View(new PostViewModel());
            }
            var post = await _controllerHelpers.GetAPost(id.Value);
            return View(post);
        }
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
