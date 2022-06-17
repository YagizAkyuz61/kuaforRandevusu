using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Api.Model;
using Microsoft.Data.SqlClient;

namespace Api.Respositories
{
    public class UserRespository
    {
        string conString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CNEdb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public IEnumerable<UserClass> GetData()
        {
            using (var connection = new SqlConnection(conString))
            {
                //  [ID],[Name],[LastExecutionDate],[Status]
                connection.Open();
                SqlDependency.Start(conString);
                using (SqlCommand command = new SqlCommand(@"SELECT * FROM [dbo].[UserTable]", connection))
                {
                    // Make sure the command object does not already have
                    // a notification object associated with it.
                    command.Notification = null;

                    SqlDependency dependency = new SqlDependency(command);

                    dependency.OnChange += new OnChangeEventHandler(dependency_OnChange);

                    if (connection.State == ConnectionState.Closed)
                        connection.Open();

                    using (var reader = command.ExecuteReader())
                        return reader.Cast<IDataRecord>()
                            .Select(x => new UserClass()
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
                }
            }
        }
        private void dependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            UserHub.Show();
        }
    }
}
