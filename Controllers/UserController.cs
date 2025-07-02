using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SimpleProde.DTOs;
using SimpleProde.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SimpleProde.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;

        public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        [HttpPost("SignIn")]
        public async Task<ActionResult<ResponseAuthenticationDTO>> Registrar(UserCredentialDTO userCredentialDTO)
        {
            var user = new IdentityUser
            {
                Email = userCredentialDTO.Email,
                UserName = userCredentialDTO.Email
            };

            var result = await userManager.CreateAsync(user, userCredentialDTO.Password);

            if (result.Succeeded)
            {
                return await BuildToken(user);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<ResponseAuthenticationDTO>> Login(UserCredentialDTO userCredentialDTO)
        {
            var user = await userManager.FindByEmailAsync(userCredentialDTO.Email);

            if (user is null)
            {
                var errores = BuildLoginIncorrect();
                return BadRequest(errores);
            }

            var resultado = await signInManager.CheckPasswordSignInAsync(user, userCredentialDTO.Password, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return await BuildToken(user);
            }
            else
            {
                var errores = BuildLoginIncorrect();
                return BadRequest(errores);
            }
        }

        private IEnumerable<IdentityError> BuildLoginIncorrect()
        {
            var identityError = new IdentityError() { Description = "Incorrect Login" };
            var errors = new List<IdentityError>();
            errors.Add(identityError);
            return errors;
        }

        private async Task<ResponseAuthenticationDTO> BuildToken(IdentityUser identityUser)
        {
            var claims = new List<Claim>
            {
                new Claim("email", identityUser.Email!),
                new Claim("lo que yo quiera", "cualquier valor")

            };

            var claimsDB = await userManager.GetClaimsAsync(identityUser);
            claims.AddRange(claimsDB);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["llavejwt"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddYears(1);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: creds);
            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return new ResponseAuthenticationDTO
            {
                Token = token,
                ExpirationDate = expiration
            };
        }
    }
}
