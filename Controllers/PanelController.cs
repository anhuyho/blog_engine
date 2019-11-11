using System.Threading.Tasks;
using BlogEngine.Data.FileManager;
using BlogEngine.Data.Repository;
using BlogEngine.Models;
using BlogEngine.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogEngine.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PanelController : Controller
    {
        private IRepository _repo;
        private IFileManager _fileManager;

        public PanelController(IRepository repo, IFileManager fileManager)
        {
            _repo = repo;
            _fileManager = fileManager;
        }
        public IActionResult Index()
        {
            var post = _repo.GetAllPost();
            return View(post);
        }
        public IActionResult Post(int id)
        {
            var post = _repo.GetPost(id);
            return View(post);
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View(new PostViewModel());
            }
            var post = _repo.GetPost((int)id);
            return View(new PostViewModel { 
                Id = post.Id,
                Title = post.Title,
                Body = post.Body
            });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(PostViewModel vm)
        {
            var post = new Post
            {
                Id = vm.Id,
                Title = vm.Title,
                Body = vm.Body,
                Image = await _fileManager.SaveImage(vm.Image)
            };
            if (post.Id > 0)
            {
                _repo.UpdatePost(post);
            }
            else
            {
                _repo.AddPost(post);
            }

            if (await _repo.SaveChangesAsync())
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Remove(int id)
        {
            _repo.RemovePost(id);
            await _repo.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}