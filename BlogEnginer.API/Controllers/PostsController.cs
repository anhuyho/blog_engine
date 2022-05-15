using AutoMapper;
using BlogEngine.DataTransferObject;
using BlogEnginer.API.Data;
using BlogEnginer.API.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogEnginer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PostsController : ControllerBase
    {
        private readonly AppDbContext _context = null;
        private readonly ILogger<PostsController> _logger = null;
        private readonly IMapper _mapper = null;
        public PostsController(AppDbContext context, ILogger<PostsController> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostViewModel>>> Get()
        {
            var posts = await _context.Posts.ToListAsync();
            var vm =  _mapper.Map<IEnumerable<PostViewModel>>(posts);
            if (vm is null)
            {
                return NotFound();
            }
            return Ok(vm);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> Get(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post is null)
                return NotFound();

            return Ok(post);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, PostViewModel postVM)
        {
            if (!ModelState.IsValid || id != postVM.Id)
            {
                return BadRequest();
            }


            var post = _mapper.Map<Post>(postVM);

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IsPostExisting(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

      
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<ActionResult<Post>> Post(Post post)
        {
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPost", new { id = post.Id }, post);
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<PostViewModel>> Delete(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post is null) return NotFound();

            _context.Posts.Remove(post);

            await _context.SaveChangesAsync();

            return _mapper.Map<PostViewModel>(post);
        }

        private bool IsPostExisting(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }

    }
}
