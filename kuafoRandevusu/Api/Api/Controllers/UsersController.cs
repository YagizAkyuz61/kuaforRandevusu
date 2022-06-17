using Api.Data;
using Api.Model;
using AuthenticationPlugin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private krDbContext _krDbContext;

        public UsersController(krDbContext krDbContext)
        {
            _krDbContext = krDbContext;
        }
        
        public IEnumerable<UserClass> Get()
        {
            string conString = @"Data Source =(localdb)\MSSQLLocalDB; Initial Catalog = krDB; Integrated Security = True";
            using (var connection = new SqlConnection(conString))
            {
                connection.Open();
                SqlDependency.Start(conString);
                using (SqlCommand command = new SqlCommand("Select * From UserTable", connection))
                {
                    command.Notification = null;

                    SqlDependency dependency = new SqlDependency(command);
                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    var listCus = reader.Cast<IDataRecord>()
                            .Select(x => new
                            {
                                Id = x.GetInt32(0),
                                Name = x.GetString(1),
                                Email = x.GetString(2),
                                Nickname = x.GetString(3),
                                Password = x.GetString(4),
                                PhoneNumber = x.GetString(5),
                                Address = x.GetString(6),
                                City = x.GetString(7),
                                Ilce = x.GetString(8),
                                Gender = x.GetString(9),
                                Date = x.GetDateTime(10)
                            }).ToList();

                    return _krDbContext.UserTable;
                }
            }
        }
        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            UserHub.Show();
        }

        // GET: api/<UsersController>
        /*[HttpGet]
        public IEnumerable<UserClass> Get()
        {
            return _krDbContext.UserTable;
        }
        */
        // GET api/<UsersController>/5
        [HttpGet("{id}", Name = "Get")]
        public UserClass Get(int id)
        {
            var user = _krDbContext.UserTable.Find(id);
            return user;
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserClass user)
        {
            var users = _krDbContext.UserTable.Find(id);
            if (users == null)
            {
                return NotFound("Hata");
            }
            else
            {
                users.Name = user.Name;
                users.Nickname = user.Nickname;
                users.PhoneNumber = user.PhoneNumber;
                users.Address = user.Address;
                users.City = user.City;
                users.Ilce = user.Ilce;
                users.Gender = user.Gender;
                _krDbContext.SaveChanges();
                return Ok("Güncelleme başarılı.");
            }
        }
    }
}
