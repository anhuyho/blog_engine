using BlogEngine.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BlogEngine.ViewComponents
{
    public class PostPreview: ViewComponent
    {
        public IViewComponentResult Invoke(Post post)
        {
            return View(post);
        }
    }
}
