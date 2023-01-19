using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NSE.Core.Messages.Integration;
using NSE.Identidade.API.Models;
using NSE.MessageBus;
using NSE.WebAPI.Core.Controllers;
using NSE.WebAPI.Core.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NSE.Identidade.API.Controllers
{
    [Route("api/identity")]
    public class AuthController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppSettings _appSettings;
        private readonly IMessageBus _bus;
        //private IBus _bus;

        public AuthController(UserManager<IdentityUser> userManager,
                                SignInManager<IdentityUser> signInManager,
                                IOptions<AppSettings> appSettings, IMessageBus bus)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
            _bus = bus;
        }

        [HttpPost("new")]
        public async Task<ActionResult> Register(UserRegistry userRegistry)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = userRegistry.Email,
                Email = userRegistry.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, userRegistry.Password);

            if (result.Succeeded)
            {
                var resultCustomer = await RegisterCustomer(userRegistry);

                if (!resultCustomer.ValidationResult.IsValid)
                {
                    await _userManager.DeleteAsync(user);
                    return CustomResponse(resultCustomer.ValidationResult);
                }

                return CustomResponse(await JWTGenerate(userRegistry.Email));
            }

            foreach (var error in result.Errors)
            {
                AddProcessingError(error.Description);
            }

            return CustomResponse();
        }

        [HttpPost("authenticate")]
        public async Task<ActionResult> Login(UserLogin userLogin)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var restult = await _signInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password,
                isPersistent: false, lockoutOnFailure: true);

            if (restult.Succeeded)
            {
                return CustomResponse(await JWTGenerate(userLogin.Email));
            }

            if (restult.IsLockedOut)
            {
                AddProcessingError("Usuário temporariamente bloqueado por tentativas inválidas");
                return CustomResponse();
            }

            AddProcessingError("Usuário ou Senha incorretos");
            return CustomResponse();
        }

        private async Task<UserResponseLogin> JWTGenerate(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);

            var identityClaims = await GetUserClaims(claims, user);
            var encodedToken = EncodeToken(identityClaims);

            return GetResponseToken(encodedToken, user, claims);
        }

        private async Task<ClaimsIdentity> GetUserClaims(ICollection<Claim> claims, IdentityUser user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim("role", userRole));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            return identityClaims;
        }

        private string EncodeToken(ClaimsIdentity identityClaims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _appSettings.Issuer,
                Audience = _appSettings.ValidIn,
                Subject = identityClaims,
                Expires = DateTime.UtcNow.AddHours(_appSettings.ExpirationHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return tokenHandler.WriteToken(token);
        }

        private UserResponseLogin GetResponseToken(string encodedToken, IdentityUser user, IList<Claim> claims)
        {
            var response = new UserResponseLogin
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.ExpirationHours).TotalSeconds,
                UserToken = new UserToken
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(c => new UserClaim { Type = c.Type, Value = c.Value })
                }
            };

            return response;
        }

        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        private async Task<ResponseMessage> RegisterCustomer(UserRegistry userRegistry)
        {
            var user = await _userManager.FindByEmailAsync(userRegistry.Email);

            var userRegistered = new UserRegistredIntegrationEvent(Guid.Parse(user.Id), userRegistry.Name, userRegistry.Email, userRegistry.Cpf);

            //_bus = RabbitHutch.CreateBus(connectionString: "host=localhost:5672");
            //var success = await _bus.Rpc.RequestAsync<UserRegistredIntegrationEvent, ResponseMessage>(userRegistered);

            try
            {
                return await _bus.RequestAsync<UserRegistredIntegrationEvent, ResponseMessage>(userRegistered);
            }
            catch
            {
                await _userManager.DeleteAsync(user);
                throw;
            }
        }
    }
}




