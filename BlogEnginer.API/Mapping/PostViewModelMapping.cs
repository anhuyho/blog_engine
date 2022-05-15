using AutoMapper;
using BlogEngine.DataTransferObject;
using BlogEnginer.API.Entites;

namespace BlogEngine.API.Mapping
{
    public class PostViewModelMapping: Profile
    {
        public PostViewModelMapping()
        {
            CreateMap<Post, PostViewModel>();
        }
    }
}
