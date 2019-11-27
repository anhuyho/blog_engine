using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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

        public PostController(IRepository repo, IFileManager fileManager)
        {
            _repo = repo;
            _fileManager = fileManager;
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
                //ImageName = post.Image,
                Title = post.Title
            };

        }

        // POST: api/Post
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Post()
        {
            var request = HttpContext.Request;
            var file = request.Form?.Files?["file"];

            var vals = request.Form?["vm"];

            var vals2 = HttpUtility.ParseQueryString(vals);

            //example values:
            var ids = vals2?.GetValues("Id")?.FirstOrDefault();
            var title = vals2?.GetValues("Title")?.FirstOrDefault();
            var body = vals2?.GetValues("Body")?.FirstOrDefault();
            var post = new Entites.Post
            {
                Title = title,
                Body = body,
                Image = await _fileManager.SaveImage(file)
            };
            int.TryParse(ids, out var id);
            if (id > 0)
            {
                post.Id = id;
                await _repo.Update(post);
            }
            else
            {
                await _repo.Add(post);
            }

            if (await _repo.SaveChangesAsync())
            {
                return Ok(post);
            }
            return NotFound();
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
