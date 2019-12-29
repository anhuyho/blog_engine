using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using BlogEngine.API.Entities;
using BlogEngine.DataTransferObject;
using BlogEnginer.API.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogEnginer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private AppDbContext _context = null;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task Post(UserViewModel vm)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var idToken = await HttpContext.GetTokenAsync("id_token");
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            var claims = User.Claims.ToList();
            var _accessToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            var sub = _accessToken?.Claims?.Where(c => c.Type == "sub").FirstOrDefault();

            var identityServerId = sub?.Value;
            var user = _context.Users.FirstOrDefault(u => u.IdentityId == identityServerId);

            if (user == null)
            {
                
                var newUser = new User
                {
                    IdentityId = identityServerId,
                    Email = vm.Email,
                    Username = vm.Username
                };
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();
            }
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<UserViewModel> Get(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.IdentityId == id);
            var vm = new UserViewModel
            {
                Email = user?.Email,
                Username = user?.Username
            };
            return vm;
        }
        
    }
}