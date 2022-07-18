using DatabaseConnection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace To_Do_API.Controllers
{
    [ApiController]
    public class AuthController : Controller
    {
        private readonly ILogger<ToDoController> _logger;
        private readonly IConfiguration _config;
        private readonly AuthSqlServerConnection _connection;
        public AuthController(ILogger<ToDoController> logger, IConfiguration config, AuthSqlServerConnection authenticationConnection)
        {
            _logger = logger;
            _config = config;
            _connection = authenticationConnection;
        }
        [AllowAnonymous]
        [HttpPost(nameof(Auth))]
        public IActionResult Auth([FromHeader] string username, [FromHeader] string password)
        {
            if (username.Equals("test") && password.Equals("test"))
            {
                var tokenString = GenerateJWTToken(username);
                return Ok(new { Token = tokenString, Message = "Success" });
            }
            return BadRequest("Invalid username and password");
        }
        private string GenerateJWTToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Auth:JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", username) }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _config["Auth:JWT:Issuer"],
                Audience = _config["Auth:JWT:Issuer"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
