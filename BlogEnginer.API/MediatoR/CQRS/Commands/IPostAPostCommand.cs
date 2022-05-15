using BlogEngine.DataTransferObject;
using MediatR;

namespace BlogEngine.API.MediatoR.CQRS.Commands
{
    public interface IPostAPostCommand : IRequest<PostViewModel>
    {
        public PostViewModel PostViewModel { get; set; }
    }

    public interface IDeleteAPostCommand : IRequest<bool>
    {
        public int DeleteId { get; set; }
    }
}
