using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Cors;
using CIS424_API.Models;

// AuthenticateUser Controller route
namespace CIS424_API.Controllers
{
    [RoutePrefix("SVSU_CIS424")]
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class AuthenticateUserController : ApiController
    {
        // POST SVSU_CIS424/AuthenticateUser
        [HttpPost]
        [Route("AuthenticateUser")]
        public IHttpActionResult AuthenticateUser([FromBody] User user)
        {

            string connectionString = "Server=tcp:capsstone-server-01.database.windows.net,1433;Initial Catalog=capstone_db_01;Persist Security Info=False;User ID=SA_Admin;Password=Capstone424!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_GetPasswordByUsername", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameter for the stored procedure.
                        command.Parameters.AddWithValue("@username", user.username);

                        // Execute the stored procedure and retrieve the stored hashed password.
                        var storedHashedPassword = command.ExecuteScalar() as string;

                        if (storedHashedPassword != null)
                        {
                            // Use BCrypt to verify the entered password against the stored hashed password.
                            bool passwordMatch = BCrypt.Net.BCrypt.Verify(user.password, storedHashedPassword);

                            // Return a JSON object with the key "IsValid" and the corresponding boolean value.
                            return Ok(new { IsValid = passwordMatch });
                        }
                        else
                        {
                            return Ok(new { IsValid = "false" }); ; // No matching record found.
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during database operations.
                return InternalServerError(ex);
            }
        }
    }
}