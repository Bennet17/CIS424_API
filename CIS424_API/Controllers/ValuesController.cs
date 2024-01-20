using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using CIS424_API.Models;

namespace CIS424_API.Controllers
{
    [RoutePrefix("SVSU_CIS424")]
    public class CIS424Controller : ApiController
    {
        //POST SVSU_CIS424/CreateUser
        [HttpPost]
        [Route("CreateUser")]
        public IHttpActionResult Post([FromBody] User user)
        {
            string connectionString = "Data Source=DESKTOP-A0KOPHR;Initial Catalog=capstone_db_01;Integrated Security=True;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_CreateUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters for the stored procedure.
                        command.Parameters.AddWithValue("@username", user.username);
                        command.Parameters.AddWithValue("@role", user.role);
                        command.Parameters.AddWithValue("@location", user.location);
                        // Hash the password
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.password);
                        command.Parameters.AddWithValue("@password", hashedPassword);


                        // Add output parameter
                        SqlParameter resultMessageParam = new SqlParameter("@ResultMessage", SqlDbType.VarChar, 255);
                        resultMessageParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(resultMessageParam);

                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        // Retrieve the result message
                        string resultMessage = resultMessageParam.Value.ToString();

                        
                        // Return the response as JSON object.
                        return Ok(new { response = resultMessage });
                         
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during database operations.
                return InternalServerError(ex);
            }
        }
        [HttpPost]
        [Route("AuthenticateUser")]
        public IHttpActionResult AuthenticateUser([FromBody] User user)
        {
            string connectionString = "Data Source=DESKTOP-A0KOPHR;Initial Catalog=capstone_db_01;Integrated Security=True;";

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
                            return Ok(new { IsValid = "User Not Found" }); ; // No matching record found.
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
