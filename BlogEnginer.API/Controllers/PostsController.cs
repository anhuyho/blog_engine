using BlogEngine.API.MediatoR.CQRS.Commands;
using BlogEngine.API.MediatoR.CQRS.Queries;
using BlogEngine.DataTransferObject;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogEnginer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IGetAllPostsQuery _getAllPostsQuery;
        private readonly IGetPostByIdQuery _getPostByIdQuery;
        private readonly IUpdateAPostCommand _updateAPostCommand;
        private readonly IPostAPostCommand _postAPostCommand;
        private readonly IDeleteAPostCommand _deleteAPostCommand;

        public PostsController(IMediator mediator, IGetAllPostsQuery getAllPostsQuery,
                                IGetPostByIdQuery getPostByIdQuery, IUpdateAPostCommand updatePostCommand,
                                IPostAPostCommand postAPostCommand, IDeleteAPostCommand deleteAPostCommand)
        {
            _mediator = mediator;
            _getAllPostsQuery = getAllPostsQuery;
            _getPostByIdQuery = getPostByIdQuery;
            _updateAPostCommand = updatePostCommand;
            _postAPostCommand = postAPostCommand;
            _deleteAPostCommand = deleteAPostCommand;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPosts()
        {
            var result = await _mediator.Send(_getAllPostsQuery);
            if (result is null || result.Any()) return NotFound();
            return Ok(result);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            _getPostByIdQuery.Id = id;
            var result = await _mediator.Send(_getPostByIdQuery);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, PostViewModel postVM)
        {
            if (!ModelState.IsValid || id != postVM.Id)
            {
                return BadRequest();
            }

            _updateAPostCommand.Id = id;
            _updateAPostCommand.PostViewModel = postVM;

            var result = await _mediator.Send(_updateAPostCommand);

            return result ? NoContent() : NotFound();
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(PostViewModel postVM)
        {
            if (!ModelState.IsValid) return BadRequest();

            _postAPostCommand.PostViewModel = postVM;

            var result = _mediator.Send(_postAPostCommand);

            if (result is null) return NotFound();

            return CreatedAtAction("GetPost", new { id = result.Id }, result);
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            _deleteAPostCommand.DeleteId = id;
            var result = await _mediator.Send(_deleteAPostCommand);
            return result ? NoContent() : NotFound();
        }



    }
}
