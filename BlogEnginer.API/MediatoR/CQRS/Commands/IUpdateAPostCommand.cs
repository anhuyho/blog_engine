using BlogEngine.DataTransferObject;
using MediatR;

namespace BlogEngine.API.MediatoR.CQRS.Commands
{
    public interface IUpdateAPostCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public PostViewModel PostViewModel { get; set; }
    }
}
