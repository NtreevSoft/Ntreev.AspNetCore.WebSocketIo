using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Ntreev.AspNetCore.WebSocketIo;
using WebSocketIo_Web.Models;

namespace WebSocketIo_Web.Controllers.Api
{
    [Route("/api/jwt")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + "," + WebSocketIoDefaults.AuthenticationScheme)]
    public class JwtController : WebSocketIoController
    {
        private readonly IWebSocketIo _webSocketIo;
        private readonly IConfiguration _configuration;

        public JwtController(IWebSocketIo webSocketIo,
            IConfiguration configuration) : base(webSocketIo)
        {
            _webSocketIo = webSocketIo;
            _configuration = configuration;
        }

        [Route("login")]
        [AllowAnonymous]
        public IActionResult Login(LoginModel model)
        {
            return Ok(new
            {
                AccessToken = BuildToken()
            });
        }

        [Route("me")]
        public IActionResult Me()
        {
            return Ok(new
            {
                Name = User.FindFirstValue(ClaimTypes.NameIdentifier),
                Email = User.FindFirstValue(ClaimTypes.Email)
            });
        }

        private string BuildToken()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "admin"),
                new Claim(JwtRegisteredClaimNames.Email, "admin@ntreev.com"),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["authentication:key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["authentication:issuer"],
                _configuration["authentication:issuer"],
                claims: claims,
                expires: DateTime.Now.AddYears(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
