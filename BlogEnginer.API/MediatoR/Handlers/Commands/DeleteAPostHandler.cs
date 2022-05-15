using AutoMapper;
using BlogEngine.API.MediatoR.CQRS.Commands;
using BlogEnginer.API.Data;
using MediatR;

namespace BlogEngine.API.MediatoR.Handlers.Commands
{
    public class DeleteAPostHandler : IRequestHandler<DeleteAPostCommand, bool>
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<PostAPostHandler> _logger;
        public DeleteAPostHandler(AppDbContext dbContext, IMapper mapper, ILogger<PostAPostHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<bool> Handle(DeleteAPostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var post = await _dbContext.Posts.FindAsync(request.DeleteId);

                if (post is null)
                {
                    _logger.LogInformation($"the post with id: {request.DeleteId} is not exsting in databse so can not detele it");
                    return false;
                }

                _dbContext.Posts.Remove(post);

                await _dbContext.SaveChangesAsync();
                _logger.LogInformation($"delete the post with id {request.DeleteId} successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(request.DeleteId, ex, $"Something went wrong while deteling a post with id {request.DeleteId}");
                throw;
            }
        }
    }
}