using BlogEngine.DataTransferObject;
using MediatR;

namespace BlogEngine.API.MediatoR.CQRS.Queries
{
    public interface IGetAllPostsQuery : IRequest<IList<PostViewModel>>
    {

    }
    public class GetAllPostsQuery : IGetAllPostsQuery
    {
    }

    public interface IGetPostByIdQuery : IRequest<PostViewModel>
    {
        public int Id { get; set; }
    }
    public class GetPostByIdQuery : IGetPostByIdQuery
    {
        public int Id { get; set; }
    }
}
