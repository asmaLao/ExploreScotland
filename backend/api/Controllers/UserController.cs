using api.Data;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req, [FromServices] TokenGenerator tokenGen)
        {
            var user = await _context.User.SingleOrDefaultAsync(u => u.Email == req.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.Password))
                return Unauthorized("Invalid email or password.");

            var token = tokenGen.GenerateToken(user.Email);

            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req, TokenGenerator tokenGen)
        {
            if (await _context.User.AnyAsync(u => u.Email == req.Email))
                return Conflict("User already exists.");

            var hashedPw = BCrypt.Net.BCrypt.HashPassword(req.Password);

            var user = new User
            {
                Email = req.Email,
                Password = hashedPw
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            var token = tokenGen.GenerateToken(user.Email);

            return Ok(new { Token = token });
        }
    }
}
