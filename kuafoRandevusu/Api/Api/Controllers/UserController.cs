using Api.Data;
using Api.Model;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private krDbContext _krDbContext;
        public UserController(krDbContext krDbContext)
        {
            _krDbContext = krDbContext;
        }

        // GET api/user?name=fatma&city=Trabzon&ilce=Ortahisar&gender=Kadın
        [HttpGet]
         public IQueryable<UserClass> GetUser(string name, string city, string ilce, string gender)
         {
             return _krDbContext.UserTable.Where(user =>
             user.Name.Contains(name)
             || user.City.Contains(city)
             & user.Ilce.Contains(ilce)
             & user.Gender.Contains(gender));
         }
    }
}