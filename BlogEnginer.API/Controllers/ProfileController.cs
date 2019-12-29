using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using BlogEngine.API.Entities;
using BlogEngine.DataTransferObject;
using BlogEnginer.API.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogEnginer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly AppDbContext _context = null;

        public ProfileController(AppDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
       
        public async Task<UserViewModel> Get()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            var idToken = await HttpContext.GetTokenAsync("id_token");
            var refreshToken = await HttpContext.GetTokenAsync("refresh_token");

            var claims = User.Claims.ToList();
            var _accessToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);
            //var _idToken = new JwtSecurityTokenHandler().ReadJwtToken(idToken);
            //var sub = claims?.Where(c => c.Type == "sub").FirstOrDefault();
            var sub = _accessToken?.Claims?.Where(c => c.Type == "sub").FirstOrDefault();

            var identityServerId = sub?.Value;
            var user = _context.Users.FirstOrDefault(u => u.IdentityId == identityServerId);
            var vm = new UserViewModel
            {
                Id = user.Id,
                Email = user?.Email,
                Username = user?.Username,
                IdentityId = identityServerId,
                SiteName = user.SiteName
            };
            return vm;
        }
        [HttpPut]
        //[Route("{id}")]
        public async Task<IActionResult> Put(UserViewModel vm)
        {
            //var id = post.Id;
            var p = new User
            {
                Id = vm.Id,
                Email = vm.Email,
                SiteName = vm.SiteName,
                IdentityId = vm.IdentityId,
                Username = vm.Username
            };
            _context.Entry(p).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();
        }
    }
}