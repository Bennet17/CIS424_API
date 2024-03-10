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

                                if (passwordMatch)
                                {
                                    if (user.position == "Manager" || user.position == "Owner")
                                    {
                                    var userData = new
                                    {
                                        ID = (int)reader["ID"],
                                        username = reader["username"].ToString(),
                                        name = reader["name"].ToString(),
                                        position = reader["position"].ToString(),
                                        storeID = (int)reader["storeID"],
                                        managerCSV = reader["managerCSV"].ToString()
                                    };

                                    // Construct the response object with nested user object and set IsValid to true.
                                    var response = new
                                    {
                                        IsValid = true,
                                        user = userData
                                    };

                                    // Return the response object as JSON.
                                    return Ok(response);
                                    }
                                    else
                                    {
                                    var userData = new
                                    {
                                        ID = (int)reader["ID"],
                                        username = reader["username"].ToString(),
                                        name = reader["name"].ToString(),
                                        position = reader["position"].ToString(),
                                        storeID = (int)reader["storeID"]
                                    };

                                    // Construct the response object with nested user object and set IsValid to true.
                                    var response = new
                                    {
                                        IsValid = true,
                                        user = userData
                                    };

                                    // Return the response object as JSON.
                                    return Ok(response);
                                    }
                                    // Populate the user object with values from the result set.
                                }
                                else
                                {
                                    // If password doesn't match, return IsValid as false
                                    var response = new
                                    {
                                        IsValid = false
                                    };
                                    return Ok(response);
                                }
                            }
                            else
                            {
                                // If no matching record found, return IsValid as false
                                var response = new
                                {
                                    IsValid = false
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
