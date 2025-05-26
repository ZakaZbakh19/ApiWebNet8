using GestionApi.Dtos.Orders;
using GestionApi.Dtos.Users;
using GestionApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GestionApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IConfiguration _configuration;

        public UsersController(UserManager<User> userManager,
            IConfiguration configuration,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDto credentialsDto)
        {
            var user = new User
            {
                UserName = credentialsDto.FullName,
                Email = credentialsDto.Email
            };

            var result = await _userManager.CreateAsync(user, credentialsDto.Password);

            if (result.Succeeded)
            {
                if (credentialsDto.Roles != null && credentialsDto.Roles.Any())
                {
                    var roleResult = await _userManager.AddToRolesAsync(user, credentialsDto.Roles);
                    if (!roleResult.Succeeded)
                        return BadRequest(roleResult.Errors);
                }

                return Ok(await GenerateToken(credentialsDto.Email));
            }

            return BadRequest("Invalid credentials");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CredentialsUsersDto credentialsDto)
        {
            var user = await _userManager.FindByEmailAsync(credentialsDto.Email);

            if (user is null)
            {
                return BadRequest("Invalid credentials");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, credentialsDto.Password, false);

            if (result.Succeeded)
            {
                return Ok(await GenerateToken(credentialsDto.Email));
            }

            return BadRequest("Invalid credentials");
        }

        private async Task<RequestAuthDto> GenerateToken(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            //Cambiarlo por desgranar el token para contener la info
            claims.AddRange((await _userManager.GetRolesAsync(user)).Select(x => new Claim(type: ClaimTypes.Role, x)));

            var claimsDB = await _userManager.GetClaimsAsync(user);

            claims.AddRange(claimsDB);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var experiation = DateTime.UtcNow.AddHours(3);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: experiation,
                signingCredentials: creds
            );

            var tokenHandler = new JwtSecurityTokenHandler().WriteToken(token);

            return new RequestAuthDto
            {
                Token = tokenHandler,
                ExpiredDate = experiation
            };
        }
    }
}
