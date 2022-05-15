using AutoMapper;
using BlogEngine.API.MediatoR.CQRS.Commands;
using BlogEnginer.API.Data;
using BlogEnginer.API.Entites;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogEngine.API.MediatoR.Handlers.Commands
{
    public class UpdateAPostHandler : IRequestHandler<UpdateAPostCommand, bool>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _appContext;
        private readonly ILogger<UpdateAPostHandler> _logger;
        public UpdateAPostHandler(IMapper mapper, AppDbContext appContext, ILogger<UpdateAPostHandler> logger)
        {
            _mapper = mapper;
            _appContext = appContext;
            _logger = logger;
        }
        public async Task<bool> Handle(UpdateAPostCommand request, CancellationToken cancellationToken)
        {
            var post = _mapper.Map<Post>(request.PostViewModel);

            _appContext.Entry(post).State = EntityState.Modified;

            try
            {
                await _appContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!IsPostExisting(request.Id))
                {
                    _logger.LogInformation($"post with Id {request.Id} is not existing in database so can not update it");
                    return false;
                }
                else
                {
                    _logger.LogError(request.Id, ex, $"something went wrong while updating post with id: {request.Id}");
                    throw;
                }
            }
            _logger.LogInformation($"update post with Id {request.Id} successfully");
            return true;
        }

        private bool IsPostExisting(int id)
        {
            return _appContext.Posts.Any(e => e.Id == id);
        }
    }
}
