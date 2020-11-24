using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MinimalBlazorAdmin.Shared.Models;

namespace MinimalBlazorAdmin.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountsController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegisterDto registerDto)
        {
            var newUser = new IdentityUser { UserName = registerDto.Name };

            var result = await _userManager.CreateAsync(newUser, registerDto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(x => x.Description);

                return Ok(new RegisterResult { Successful = false, Errors = errors });

            }

            var role = await _roleManager.FindByNameAsync(registerDto.Role);
            if (role != null)
            {
                var addToRoleresult = await _userManager.AddToRoleAsync(newUser, registerDto.Role);
                if (!addToRoleresult.Succeeded)
                {
                    var errors = addToRoleresult.Errors.Select(x => x.Description);

                    return Ok(new RegisterResult { Successful = false, Errors = errors });
                }
            }
            else
            {
                return Ok(new RegisterResult { Successful = false, Errors = new[]{$"Role {registerDto.Role} not exist."}});
            }

            return Ok(new RegisterResult { Successful = true });
        }
    }

}