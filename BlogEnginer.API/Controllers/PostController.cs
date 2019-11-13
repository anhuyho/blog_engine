using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogEngine.DataTransferObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogEnginer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        // GET: api/Post
        [HttpGet]
        public IEnumerable<PostViewModel> Get()
        {
            return new List<PostViewModel>()
            {
                new PostViewModel
                {
                    Body = "fsfa",
                    Title = "efe",
                    ImageName = "fasf",
                    Id = 1
                },
                new PostViewModel
                {
                    Body = "fsfa",
                    Title = "efe",
                    ImageName = "fasf",
                    Id = 1
                },
                new PostViewModel
                {
                    Body = "fsfa",
                    Title = "efe",
                    ImageName = "fasf",
                    Id = 1
                }
            };
        }

        // GET: api/Post/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Post
        [HttpPost]
        public void Post([FromBody] string value)
        {
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
