using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using CIS424_API.Models;
using Microsoft.Win32;

namespace CIS424_API.Controllers
{
    [RoutePrefix("SVSU_CIS424")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ViewUsersController : ApiController
    {
        // GET SVSU_CIS424/ViewUsers
        // Returns a list of all users in the database
        [HttpGet]
        [Route("ViewUsers")]
        public IHttpActionResult ViewUsers()
        {
            string connectionString = "Server=tcp:capsstone-server-01.database.windows.net,1433;Initial Catalog=capstone_db_01;Persist Security Info=False;User ID=SA_Admin;Password=Capstone424!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_ViewUsers", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Create a SqlDataReader object
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            List<User> users = new List<User>();
                            while (reader.Read())
                            {
                                User user = new User();
                                users.Add(user);
                                user.ID = Convert.ToInt32(reader["ID"]);
                                //user.storeID = Convert.ToInt32(reader["storeID"]);
                                user.username = reader["username"].ToString();
                                user.name = reader["name"].ToString();
                                user.position = reader["position"].ToString();
                                user.enabled = Convert.ToBoolean(reader["enabled"]);
                            }
                            return Ok(users);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e);
            }
        }
        // GET SVSU_CIS424/ViewUsersByStoreID
        // Returns a list of all users in the database for a store by the storeID
        [HttpGet]
        [Route("ViewUsersByStoreID")]
        public IHttpActionResult ViewUsersByStoreID([FromUri] int storeID)
        {
            string connectionString = "Server=tcp:capsstone-server-01.database.windows.net,1433;Initial Catalog=capstone_db_01;Persist Security Info=False;User ID=SA_Admin;Password=Capstone424!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_ViewUsersByStoreID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameter for the stored procedure.
                        command.Parameters.AddWithValue("@storeID", storeID);

                        // List to store user data
                        List<object> userDataList = new List<object>(); 
                        // Create a SqlDataReader object
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                               //string storeID_CSV = reader["StoreID_CSV"].ToString();
                               // string[] storeIDs = storeID_CSV.Split(',');

                                var userData = new
                                {
                                    ID = (int)reader["ID"],
                                    username = reader["username"].ToString(),
                                    name = reader["name"].ToString(),
                                    position = reader["position"].ToString(),
                                    enabled = Convert.ToBoolean(reader["enabled"]),
                                //storeID_CSV = storeIDs
                                };

                                userDataList.Add(userData); // Add user data to the list
                            }
                        }

                        return Ok(userDataList); // Return the list of user data

                    }
                }
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e);
            }
        }
        // GET SVSU_CIS424/ViewRegistersByStoreID
        // Returns a list of all users in the database for a store by the storeID
        [HttpGet]
        [Route("ViewRegistersByStoreID")]
        public IHttpActionResult ViewRegistersByStoreID([FromUri] int storeID)
        {
            string connectionString = "Server=tcp:capsstone-server-01.database.windows.net,1433;Initial Catalog=capstone_db_01;Persist Security Info=False;User ID=SA_Admin;Password=Capstone424!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_ViewRegistersByStoreID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameter for the stored procedure.
                        command.Parameters.AddWithValue("@storeID", storeID);

                        // Create a SqlDataReader object
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            List<Register> registers = new List<Register>();
                            while (reader.Read())
                            {
                                Register register = new Register();
                                register.ID = Convert.ToInt32(reader["ID"]);
                                register.storeID = Convert.ToInt32(reader["storeID"]);                 
                                register.name = reader["name"].ToString();
                                register.opened = Convert.ToBoolean(reader["opened"]);
                                register.enabled = Convert.ToBoolean(reader["enabled"]);
                                registers.Add(register);
                            }
                            return Ok(registers);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e);
            }
        }

        [HttpPost]
        [Route("UpdateUserPassword")]
        public IHttpActionResult UpdateUserPassword([FromBody] User user)
        {
            string connectionString = "Server=tcp:capsstone-server-01.database.windows.net,1433;Initial Catalog=capstone_db_01;Persist Security Info=False;User ID=SA_Admin;Password=Capstone424!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.password);

                    using (SqlCommand command = new SqlCommand("sp_UpdateUserPassword", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@username", user.username);
                        command.Parameters.AddWithValue("@newHashPassword", hashedPassword);

                        // Add output parameter
                        SqlParameter resultMessageParam = new SqlParameter("@outputMessage", SqlDbType.VarChar, 255);
                        resultMessageParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(resultMessageParam);

                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        // Retrieve the result message and return
                        string outputMessage = resultMessageParam.Value.ToString();
                        var response = new { Message = outputMessage }; // Creating anonymous object
                        return Ok(response); // Serialize to JSON and return
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

// GET SVSU_CIS424/ViewRegistersByStoreID
        // Returns a list of all users in the database for a store by the storeID
        [HttpGet]
        [Route("ViewStoreObjects")]
        public IHttpActionResult ViewStoreObjects([FromUri] int storeID)
        {
            string connectionString = "Server=tcp:capsstone-server-01.database.windows.net,1433;Initial Catalog=capstone_db_01;Persist Security Info=False;User ID=SA_Admin;Password=Capstone424!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_getStoreObjects", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameter for the stored procedure.
                        command.Parameters.AddWithValue("@storeID", storeID);

                        // Create a SqlDataReader object
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            List<StoreObject> objects = new List<StoreObject>();
                            while (reader.Read())
                            {
                                StoreObject obj = new StoreObject();       
                                obj.name = reader["name"].ToString();
                                obj.opened = Convert.ToBoolean(reader["opened"]);
                                objects.Add(obj);
                            }
                            return Ok(objects);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e);
            }
        }
        private class StoreObject
        {
            public string name { get; set; }
            public bool opened { get; set; }
        }
    }
}

