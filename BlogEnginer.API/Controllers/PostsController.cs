﻿using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using BlogEngine.API.Entities;
using BlogEngine.DataTransferObject;
using BlogEnginer.API.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogEnginer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class PostsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PostsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Posts
        [HttpGet]
        //[Authorize]
        [Route("{sitename?}")]
        public async Task<ActionResult<IEnumerable<Post>>> Get(string? siteName)
        {
            return await _context.Posts.Where(p=>p.User.SiteName == siteName)
                
                .ToListAsync();
        }


        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> Get(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return post;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, PostViewModel post)
        {
            if (id != post.Id)
            {
                return BadRequest();
            }
            
            //var id = post.Id;
            var p = new Post
            {
                Id = post.Id,
                Content = post.Content,
                PostDescription = post.PostDescription,
                PostName = post.PostName,
                TimeStamp = post.TimeStamp
            };
            _context.Entry(p).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
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
        public async Task<ActionResult<PostViewModel>> Post(PostViewModel vm)
        {
            var user = await _context.Users.FindAsync(vm.UserId);
            var post = new Post
            {
                PostDescription = vm.PostDescription,
                Content = vm.Content,
                PostName = vm.PostName,
                User = user
            };
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPost", new { id = post.Id }, post);
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Post>> Delete(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return post;
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }

    }
}
