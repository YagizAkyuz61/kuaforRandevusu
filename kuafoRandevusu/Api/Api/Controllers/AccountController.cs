using Api.Data;
using Api.Model;
using AuthenticationPlugin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private krDbContext _krDbContext;
        private IConfiguration _configuration;
        private readonly AuthService _auth;

        public AccountController(IConfiguration configuration, krDbContext krDbContext)
        {
            _krDbContext = krDbContext;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }

        [HttpPost]
        public IActionResult Register([FromBody] UserClass user)
        {
            var userWithSameNickname = _krDbContext.UserTable.Where(u => u.Nickname == user.Nickname).SingleOrDefault();
            if (userWithSameNickname != null)
            {
                return BadRequest("User with same nickname already exists");
            }

            var userObj = new UserClass()
            {
                Name = user.Name,
                Email = user.Email,
                Nickname = user.Nickname,
                Password = SecurePasswordHasherHelper.Hash(user.Password),
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                City = user.City,
                Ilce = user.Ilce,
                Gender = user.Gender,
                Date = user.Date
            };
            _krDbContext.UserTable.Add(userObj);
            _krDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost]
        public IActionResult Login([FromBody] UserClass user)
        {
            var userNickName = _krDbContext.UserTable.FirstOrDefault(u => u.Nickname == user.Nickname);
            if (userNickName == null)
            {
                return NotFound();
            }
            if (!SecurePasswordHasherHelper.Verify(user.Password, userNickName.Password))
            {
                return Unauthorized();
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Nickname),
                new Claim(ClaimTypes.Email, user.Nickname),
            };
            var token = _auth.GenerateAccessToken(claims);
            return new ObjectResult(new
            {
                access_token = token.AccessToken,
                expires_in = token.ExpiresIn,
                token_type = token.TokenType,
                creation_Time = token.ValidFrom,
                expiration_Time = token.ValidTo,
                user_id = userNickName.Id,
                user_Name = userNickName.Name,
                user_Mail = userNickName.Email,
                user_NickName = userNickName.Nickname,
                user_PhoneNumber = userNickName.PhoneNumber,
                user_Address = userNickName.Address
            });
        }
    }
}
