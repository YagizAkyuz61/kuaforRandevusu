using Api.Data;
using Api.Model;
using AuthenticationPlugin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private krDbContext _krDbContext;
        private IConfiguration _configuration;
        private readonly AuthService _auth;

        public CustomerController(IConfiguration configuration, krDbContext krDbContext)
        {
            _krDbContext = krDbContext;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }

        [HttpPost]
        public IActionResult CuReg([FromBody] CustomerClass customer)
        {
            var userWithSameNickname = _krDbContext.CustomerTable.Where(u => u.NickName == customer.NickName).SingleOrDefault();
            if (userWithSameNickname != null)
            {
                return BadRequest("User with same nickname already exists");
            }

            var customerObj = new CustomerClass()
            {
                UserName = customer.UserName,
                NickName = customer.NickName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Password = SecurePasswordHasherHelper.Hash(customer.Password),
                Date = customer.Date
            };
            _krDbContext.CustomerTable.Add(customerObj);
            _krDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost]
        public IActionResult CuLog([FromBody] CustomerClass customer)
        {
            var userNickName = _krDbContext.CustomerTable.FirstOrDefault(u => u.NickName == customer.NickName);
            if (userNickName == null)
            {
                return NotFound();
            }
            if (!SecurePasswordHasherHelper.Verify(customer.Password, userNickName.Password))
            {
                return Unauthorized();
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, customer.NickName),
                new Claim(ClaimTypes.Email, customer.NickName),
            };
            var token = _auth.GenerateAccessToken(claims);
            return new ObjectResult(new
            {
                access_token = token.AccessToken,
                expires_in = token.ExpiresIn,
                token_type = token.TokenType,
                creation_Time = token.ValidFrom,
                expiration_Time = token.ValidTo,
                customer_id = userNickName.Id,
                customer_Name = userNickName.UserName,
                customer_NickName = userNickName.NickName,
                customer_PhoneNumber = userNickName.PhoneNumber,
                customer_Email = userNickName.Email
            });
        }

        /*[HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] CustomerClass customer)
        {
            var customers = _krDbContext.CustomerTable.Find(id);
            if (customers == null)
            {
                return NotFound("Hata");
            }
            else
            {
                customers.UserName = customer.UserName;
                customers.NickName = customer.NickName;
                customers.Email = customer.Email;
                customers.PhoneNumber = customer.PhoneNumber;
                customers.Password = customer.Password;
                _krDbContext.SaveChanges();
                return Ok("Güncelleme başarılı.");
            }
        }*/
    }
}
