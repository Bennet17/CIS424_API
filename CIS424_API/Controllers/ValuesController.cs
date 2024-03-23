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

namespace CIS424_API.Controllers
{/*
    [RoutePrefix("SVSU_CIS424")]
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class CIS424Controller : ApiController
    {
        //POST SVSU_CIS424/CreateUser
        [HttpPost]
        [Route("CreateUser")]
        public IHttpActionResult Post([FromBody] User user)
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
                        command.Parameters.AddWithValue("@username", user.username);
                        command.Parameters.AddWithValue("@name", user.username);
                        command.Parameters.AddWithValue("@position", user.position);
                        command.Parameters.AddWithValue("@storeID", user.storeID);
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
    */
}
