using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogEngine.DataTransferObject;
using BlogEngine.Web.Helpers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
//using Newtonsoft.Json;

namespace BlogEngine.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class PanelController : Controller
    {
        //private IRepository _repo;
        //private IFileManager _fileManager;

        //public PanelController(IRepository repo, IFileManager fileManager)
        //{
        //    _repo = repo;
        //    _fileManager = fileManager;
        //}
        private readonly IControllerHelpers _controllerHelpers = null;
        public PanelController(IControllerHelpers controllerHelpers)
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
                    var p = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<PostViewModel>>(responseStream);
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
        //public IActionResult Post(int id)
        //{
        //    var post = _repo.GetPost(id);
        //    return View(post);
        //}
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var post = await _controllerHelpers.GetAPost(id.Value);
            if (id == null)
            {
                return View(new PostViewModel());
            }
            return View(new PostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body
            });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(PostViewModel vm)
        {
            string content = JsonConvert.SerializeObject(vm);
            var formData = vm.ToFormData();

            var result = await _controllerHelpers.PostAsync("Post", formData);
            if (result.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            Console.WriteLine(result.RequestMessage);
        //var post = new Post
        //    {
        //        Id = vm.Id,
        //        Title = vm.Title,
        //        Body = vm.Body,
        //        Image = await _fileManager.SaveImage(vm.Image)
        //    };
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
            return View();
        }

        //public async Task<IActionResult> Remove(int id)
        //{
        //    //_repo.RemovePost(id);
        //    //await _repo.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}
    }
}