using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MinimalBlazorAdmin.Shared.Models;

namespace MinimalBlazorAdmin.Server.Controllers
{
    [Authorize]
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public IConfiguration Configuration { get; }

        public AuthController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        [AllowAnonymous]
        [HttpPost("login", Name = nameof(Login))]
        public IActionResult Login(LoginDto loginUser)
        {
            if (loginUser.Name != "demouser" || loginUser.Password != "demopassword")
            {
                return Unauthorized(new LoginResult{ Successful = false, Error = "用户名或密码不正确"});
            }
            
            // 生成 token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginUser.Name)
            };

            var tokenSection = Configuration.GetSection("Security:Token");
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSection["Key"]));  // 长度必须超过 16 位
            var signCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
            var jwtToken = new JwtSecurityToken(
                issuer: tokenSection["Issuer"],
                audience: tokenSection["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(3),
                signingCredentials: signCredential
            );

            return Ok(new LoginResult
            {
                Successful = true,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken)
            });
        }

    }
}