using System;
using System.Data.SqlClient;
using System.Data;
using System.Web.Http;
using System.Web.Http.Cors;
using CIS424_API.Models;

// CreateUser Controller route
namespace CIS424_API.Controllers
{
    [RoutePrefix("SVSU_CIS424")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CreateUserController : BaseApiController
    {
        // POST SVSU_CIS424/CreateUser
        // Creates a user in the database
        [HttpPost]
        [Route("CreateUser")]
        public IHttpActionResult CreateStore([FromBody] User user)
        {
           
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.SQL_Conn))
                {
                    connection.Open();

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_CreateUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters for the stored procedure.
                        command.Parameters.AddWithValue("@storeCSV", user.storeCSV);
                        command.Parameters.AddWithValue("@username", user.username);
                        command.Parameters.AddWithValue("@name", user.name);
                        command.Parameters.AddWithValue("@position", user.position);
                        // Hash the password
                        Console.WriteLine(user);
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.password);
                        command.Parameters.AddWithValue("@hashPassword", hashedPassword);


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
    }
}