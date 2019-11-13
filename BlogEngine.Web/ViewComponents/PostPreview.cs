using BlogEngine.DataTransferObject;
using Microsoft.AspNetCore.Mvc;

namespace BlogEngine.ViewComponents
{
    public class PostPreview: ViewComponent
    {
        public IViewComponentResult Invoke(PostViewModel post)
        {
            return View(post);
        }
    }
}
