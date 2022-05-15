using AutoMapper;
using BlogEngine.API.MediatoR.CQRS.Queries;
using BlogEngine.DataTransferObject;
using BlogEnginer.API.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogEngine.API.MediatoR.Handlers.Queries
{
    public class GetAllPostsHandler : IRequestHandler<GetAllPostsQuery, IList<PostViewModel>>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        public GetAllPostsHandler(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<IList<PostViewModel>> Handle(GetAllPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _context.Posts.ToListAsync();
            return _mapper.Map<IList<PostViewModel>>(posts);
        }
    }
}
