using BlogEngine.DataTransferObject;

namespace BlogEngine.API.MediatoR.CQRS.Commands
{
    public class UpdateAPostCommand : IUpdateAPostCommand
    {
        public int Id { get; set; }
        public PostViewModel PostViewModel { get; set; }
    }
}
