using AutoMapper;
using BlogEngine.API.MediatoR.CQRS.Commands;
using BlogEngine.DataTransferObject;
using BlogEnginer.API.Data;
using BlogEnginer.API.Entites;
using MediatR;

namespace BlogEngine.API.MediatoR.Handlers.Commands
{
    public class PostAPostHandler : IRequestHandler<PostAPostCommand, PostViewModel>
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<PostAPostHandler> _logger;
        public PostAPostHandler(AppDbContext dbContext, IMapper mapper, ILogger<PostAPostHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<PostViewModel> Handle(PostAPostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var post = _mapper.Map<Post>(request.PostViewModel);
                _dbContext.Posts.Add(post);
                await _dbContext.SaveChangesAsync();
                return _mapper.Map<PostViewModel>(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "Something went wrong while create a new post");
                throw;
            }
        }
    }
}
