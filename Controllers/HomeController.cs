using BlogEngine.Data.FileManager;
using BlogEngine.Data.Repository;
using BlogEngine.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlogEngine.Controllers
{
    public class HomeController: Controller
    {
        private IRepository _repo;
        private IFileManager _fileManager;

        public HomeController(IRepository repo, IFileManager fileManager)
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
                return View(new Post());
            }
            var post = _repo.GetPost((int)id);
            return View(post);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Post post)
        {
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
        [HttpGet("/Image/{image}")]
        public IActionResult Image(string image)
        {
            var mine = image.Substring(image.LastIndexOf('.') + 1);
            return new FileStreamResult(_fileManager.ImageStream(image), $"image/{mine}");
        }
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
