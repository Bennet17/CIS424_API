using System;
using System.Data.SqlClient;
using System.Data;
using System.Web.Http;
using System.Web.Http.Cors;
using CIS424_API.Models;

// AuthenticateUser Controller route
namespace CIS424_API.Controllers
{
    [RoutePrefix("SVSU_CIS424")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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

                        // Execute the stored procedure and retrieve the result set.
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Retrieve the hashed password from the database.
                                string storedHashedPassword = reader["hashPassword"].ToString();

                                // Use BCrypt to verify the entered password against the stored hashed password.
                                bool passwordMatch = BCrypt.Net.BCrypt.Verify(user.password, storedHashedPassword);

                                // Populate the AuthenticateUserResponse object with values from the result set.
                                AuthenticateUserResponse response = new AuthenticateUserResponse
                                {
                                    ID = (int)reader["ID"],
                                    username = reader["username"].ToString(),
                                    name = reader["name"].ToString(),
                                    position = reader["position"].ToString(),
                                    storeID = (int)reader["storeID"],
                                    IsValid = passwordMatch ? "true" : "false"
                                };

                                // Return the AuthenticateUserResponse object as JSON.
                                return Ok(response);
                            }
                            else
                            {
                                // If no matching record found, return IsValid as false
                                AuthenticateUserResponse response = new AuthenticateUserResponse
                                {
                                    IsValid = "false"
                                };
                                return Ok(response);
                            }
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
