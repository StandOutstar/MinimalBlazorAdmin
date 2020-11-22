using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MinimalBlazorAdmin.Shared.Models;

namespace MinimalBlazorAdmin.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly SignInManager<IdentityUser> _signInManager;

        public LoginController(IConfiguration configuration,
            SignInManager<IdentityUser> signInManager)
        {
            _configuration = configuration;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var result = await _signInManager.PasswordSignInAsync(loginDto.Name, loginDto.Password, false, false);

            if (!result.Succeeded) return BadRequest(new LoginResult { Successful = false, Error = "Username and password are invalid." });

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, loginDto.Name)
            };

            var tokenSource = _configuration.GetSection("Security:Token");

            // var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSource["JwtSecurityKey"]));
            // var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // var expiry = DateTime.Now.AddDays(Convert.ToInt32(tokenSource["JwtExpiryInDays"]));
            //
            // var token = new JwtSecurityToken(
            //     tokenSource["JwtIssuer"],
            //     tokenSource["JwtAudience"],
            //     claims,
            //     expires: expiry,
            //     signingCredentials: creds
            // );
            //
            // return Ok(new LoginResult { Successful = true, Token = new JwtSecurityTokenHandler().WriteToken(token) });
            //
            var tokenSection = _configuration.GetSection("Security:Token");
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSection["Key"]));  // 长度必须超过 16 位
            var signCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddDays(Convert.ToInt32(tokenSection["ExpireInDays"]));
            
            var jwtToken = new JwtSecurityToken(
                issuer: tokenSection["Issuer"],
                audience: tokenSection["Audience"],
                claims: claims,
                expires: expiry,
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