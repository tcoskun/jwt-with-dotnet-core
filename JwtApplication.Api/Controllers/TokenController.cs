using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtApplication.Models;
using JwtApplication.Services;

namespace JwtApplication.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<TokenController> _logger;
        private readonly ISecurityService _securityService;
        private readonly IConfiguration _configuration;

        public TokenController(ILogger<TokenController> logger, ISecurityService securityService, IConfiguration configuration)
        {
            _logger = logger;
            _securityService = securityService;
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Post([FromBody]LoginRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request", "Request is null!");
            }
            var userId = _securityService.Login(request.Username, request.Password);

            if (userId == default)
            {
                return Unauthorized();
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var authOptions = _configuration.GetSection("AuthOptions").Get<AuthOptions>();

            var token = new JwtSecurityToken
            (
                authOptions.Issuer,
                authOptions.Audience,
                claims,
                expires: DateTime.UtcNow.AddDays(30),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authOptions.SecureKey)),
                    SecurityAlgorithms.HmacSha256)
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
    }
}
