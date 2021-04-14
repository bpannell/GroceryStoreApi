using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStoreAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class JwtController : ControllerBase
    {
        private IConfiguration _configuration;
        private readonly ILogger _logger;

        public JwtController(IConfiguration configuration, ILogger<JwtController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Jwt()
        {
            _logger.LogDebug("Request Made To Get A JWT Token");
            if (!Request.Headers.ContainsKey("ApiKey"))
            {
                _logger.LogWarning("Call For JWT token did not have an ApiKey in the request");
                return Unauthorized();
            }
            if (!Request.Headers["ApiKey"].Equals(_configuration.GetValue<string>("Jwt:PublicKey")))
            {
                _logger.LogWarning($"The ApiKey {Request.Headers["ApiKey"]} is invalid for a JWT token request");
                return Forbid();
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Issuer"],
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            _logger.LogDebug("Jwt Token Created For Request");
            return Ok(tokenHandler.WriteToken(token));
        }
    }
}
