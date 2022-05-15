using AutoMapper;
using BlogEngine.API.MediatoR.CQRS.Queries;
using BlogEngine.DataTransferObject;
using BlogEnginer.API.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BlogEngine.API.MediatoR.Handlers.Queries
{

    public class GetPostByIdHandler : IRequestHandler<GetPostByIdQuery, PostViewModel>
    {
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;
        public GetPostByIdHandler(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<PostViewModel> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {

            var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == request.Id);
            return _mapper.Map<PostViewModel>(post);
        }
    }
}
