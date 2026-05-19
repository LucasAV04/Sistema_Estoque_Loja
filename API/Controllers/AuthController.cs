using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace API.Controllers
{
        [ApiController]
        [Route("[controller]")]
        public class AuthController : ControllerBase
        {
            private readonly IConfiguration _config;

            public AuthController(IConfiguration config)
            {
                _config = config;
            }

            [HttpPost("Login")]
            public IActionResult Login([FromBody] LoginRequest request)
            {
                string? role = null;

                if (request.Usuario == "Lucas" && request.Senha == "3214")
                    role = "Admin";
                else if (request.Usuario == "Usuario" && request.Senha == "1234")
                    role = "User";
                else
                    return Unauthorized("Usuário ou senha inválidos");

                var claims = new[]
                {
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.Name, request.Usuario)
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
                var token = new JwtSecurityToken(
                    expires: DateTime.UtcNow.AddHours(8),
                    claims: claims,
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );

                return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }
        }

        public record LoginRequest(string Usuario, string Senha);
    }
}
