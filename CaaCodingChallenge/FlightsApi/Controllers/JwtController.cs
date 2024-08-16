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
        private readonly string _jwtIssuer;
        private readonly string _jwtKey;

        public JwtController(IConfiguration config)
        {
            Guard.Against.Null(config);
#pragma warning disable CS8601 // Possible null reference assignment. We just checked this.
            _jwtIssuer = config["Jwt:Issuer"];
            _jwtKey = config["Jwt:Key"];
#pragma warning restore CS8601 // Possible null reference assignment.
            Guard.Against.Null(_jwtIssuer);
            Guard.Against.Null(_jwtKey);
        }

        [HttpGet]
        public JsonResult Get()
        {
            var token = GenerateJwt();
            return new JsonResult(token);
        }

        private string GenerateJwt()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, "user_name"),
                new Claim(JwtRegisteredClaimNames.Email, "user_email"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_jwtIssuer,
                _jwtIssuer,
                claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
