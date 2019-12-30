using System.Threading.Tasks;
using BlogEngine.DataTransferObject;
using Microsoft.AspNetCore.Mvc;

namespace BlogEngine.Controllers
{
    public class ProfileController : Controller
    {
        //private SignInManager<IdentityUser> _signInManager;
        //IConfiguration _config;
        Profile profile = new Profile
        {
            Content = @"<p>A son, a hushband, a father and a software developer. 
                            <br>
                            <hr>
                            Experienced Software Engineer with a demonstrated history of working in the software outsourcing and software development industry.
</p><p>                            <br>
                            Able to work well from front end to back end
                            <br>
                            </p><p>Skilled in </p><p>&nbsp;&nbsp;&nbsp;&nbsp;<strong>C#, ASP.NET (Core 3.x), Entity Framework (Core), Identity Server</strong></p><p>&nbsp; &nbsp; <b>Azure, Docker (Compose)</b><strong><br></strong></p><p>&nbsp;&nbsp;&nbsp;&nbsp;<strong>Javascript, jQuery, Angular JS, Vue JS, React JS</strong></p><p>
                            <br>Strong engineering professional with a Bachelor's degree focused in Computer Software Engineering from University of Foreign Language &amp; Information Technology. </p>",
            UserName = "Huy Ho",
            Facebook = "#",
            Youtube = "https://www.youtube.com/channel/UCvLAedWEOD_35UqPl7jV7pQ?view_as=subscriber",
            LinkedIn = "https://www.linkedin.com/in/huy-ho/",
            Twitter = "https://twitter.com/HuyHo51839379",
            ImageUrl = "/content/images/about.jpg"
        };
        public ProfileController()
        {
            //_signInManager = signInManager;
            //_config = config;
        }
        [HttpGet]
        public async Task<IActionResult> About()
        {
            
            return View(profile);
        }

        public async Task<IActionResult> Edit(int Id)
        {
            return View(profile);
        }
        [HttpPost]
        public async Task<IActionResult> About(Profile vm)
        {
            profile = vm;
            return View(vm);
        }
        
    }
}