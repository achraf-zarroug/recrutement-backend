using backen_dotnet.Interfaces;
using backen_dotnet.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace backen_dotnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class adminController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public adminController(UserManager<AppUser> userManager,
                                      ITokenService tokenService,
                                      SignInManager<AppUser> signInManager,
                                      IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet("user")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetUser()
        {
            return Ok("Welcome, Admin!");


        }

    }
}
