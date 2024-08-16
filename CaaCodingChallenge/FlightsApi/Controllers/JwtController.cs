using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Ardalis.GuardClauses;

namespace FlightsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtController : ControllerBase
    {
        private IConfiguration _config;

        public JwtController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet]
        public JsonResult Get()
        {
            var token = GenerateJwt();
            return new JsonResult(token);
        }

        private string GenerateJwt()
        {
            var jwtIssuer = _config["Jwt:Issuer"];
            var jwtKey = _config["Jwt:Key"];
            Guard.Against.Null(jwtIssuer);
            Guard.Against.Null(jwtKey);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, "user_name"),
                new Claim(JwtRegisteredClaimNames.Email, "user_email"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(jwtIssuer,
                jwtIssuer,
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
