using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogEngine.Data.FileManager;
using BlogEngine.DataTransferObject;
using BlogEnginer.API.Data.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogEnginer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private IRepository _repo;
        private IFileManager _fileManager;

        public PostController(IRepository repo)
        {
            _repo = repo;
        }

        // GET: api/Post
        [HttpGet]
        public async Task<IEnumerable<PostViewModel>> Get()
        {
            var posts = await _repo.Get();
            var result = posts.ToList().Select(x =>
                new PostViewModel
                {
                    Id = x.Id,
                    Body = x.Body,
                    ImageName = x.Image,
                    Title = x.Title
                }
            );
            return result.AsQueryable();
        }

        // GET: api/Post/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<PostViewModel> Get(int id)
        {
            var post = await _repo.Get(id);
            return new PostViewModel
            {
                Body = post.Body,
                Id = post.Id,
                ImageName = post.Image,
                Title = post.Title
            };

        }

        // POST: api/Post
        [HttpPost]
        public async Task<IActionResult> Post([FromForm(Name="Image")] IFormFile image)
        {
            //var post = new Entites.Post
            //{
            //    Title = vm.Title,
            //    Body = vm.Body,

            //};
            //if (vm.Id > 0)
            //{
            //    post.Id = vm.Id;
            //    await _repo.Update(post);
            //}
            //else
            //{
            //    await _repo.Add(post);
            //}

            //if (await _repo.SaveChangesAsync())
            //{
            //    return Ok(vm);
            //}
            //return NotFound();
            return Ok();
        }

        // PUT: api/Post/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
