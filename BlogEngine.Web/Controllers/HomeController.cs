using BlogEngine.DataTransferObject;
using BlogEngine.Web.Helpers;
using Microsoft.AspNetCore.Mvc;


namespace BlogEngine.Controllers
{
    public class HomeController : Controller
    {
        private readonly IControllerHelpers _controllerHelpers = null;
        private readonly ILogger<HomeController> _logger = null;

        public HomeController(IControllerHelpers controllerHelpers, ILogger<HomeController> logger)
        {
            _controllerHelpers = controllerHelpers;
            _logger = logger;
        }
        // GET: Posts
        public async Task<IActionResult> Index(int? pageNumber)
        {
            var pageSize = 2;
            var posts = new List<PostViewModel>();
            try
            {
                var response = await _controllerHelpers.GetAsync("Posts");
                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    var p = await System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<PostViewModel>>(responseStream);
                    posts = p.ToList();
                }
                else
                {
                    posts = Array.Empty<PostViewModel>().ToList();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(pageNumber.Value, ex, $"can not get page {pageNumber}");
            }
            return View(PaginatedList<PostViewModel>.CreateAsync(posts, pageNumber ?? 1, pageSize));
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            PostViewModel post;
            try
            {
                var response = await _controllerHelpers.GetAsync("Posts/"+id);
                if (response.IsSuccessStatusCode)
                {
                    using var responseStream = await response.Content.ReadAsStreamAsync();
                    post = await System.Text.Json.JsonSerializer.DeserializeAsync<PostViewModel>(responseStream);
                }
                throw new Exception();
            }
            catch (Exception ex)
            {
                _logger.LogError(id.Value, ex, $"can not get post with id {id.Value}");
                post = new PostViewModel();
            }
            return View(post);
        }

        public async Task<IActionResult> About()
        {
            var profile = new UserProfile
            {
                Content = "A son, a hushband, a father and a software developer",
                UserName = "Huy Ho",
                Facebook = "#",
                Youtube = "https://www.youtube.com/channel/UCvLAedWEOD_35UqPl7jV7pQ?view_as=subscriber",
                LinkedIn = "https://www.linkedin.com/in/huy-ho/",
                Twitter = "",
                ImageUrl = "/content/images/about.jpg"
            };
            return View(profile);
        }
        [HttpPost]
        public async Task<IActionResult> About(UserProfile profile)
        {
            return View(profile);
        }

    }
}