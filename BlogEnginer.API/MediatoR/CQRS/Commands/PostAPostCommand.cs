using BlogEngine.DataTransferObject;

namespace BlogEngine.API.MediatoR.CQRS.Commands
{
    public class PostAPostCommand: IPostAPostCommand
    {
        public PostViewModel PostViewModel { get; set; }
    }

    public class DeleteAPostCommand : IDeleteAPostCommand
    {
        public int DeleteId { get; set; }
    }
}
