using System;
using System.Data.SqlClient;
using System.Data;
using System.Web.Http;
using System.Web.Http.Cors;
using CIS424_API.Models;

namespace CIS424_API.Controllers 
{
    [RoutePrefix("SVSU_CIS424")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : BaseApiController
    {


        // AuthenticateUserController.cs is here now
         // SVSU_CIS424/AuthenticateUser
        [HttpPost]
        [Route("AuthenticateUser")]
        public IHttpActionResult AuthenticateUser([FromBody] User user)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.SQL_Conn))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_GetPasswordByUsername", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@username", user.username);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedHashedPassword = reader["hashPassword"].ToString();
                                if (BCrypt.Net.BCrypt.Verify(user.password, storedHashedPassword))
                                {
                                    string storedPosition = reader["position"].ToString();
                                    string storeID_CSV = reader["StoreID_CSV"].ToString();

                                    // Split CSV string into an array
                                    string[] storeIDs = storeID_CSV.Split(',');

                                    var userData = new
                                    {
                                        ID = (int)reader["ID"],
                                        username = reader["username"].ToString(),
                                        name = reader["name"].ToString(),
                                        position = storedPosition,
                                        enabled = Convert.ToBoolean(reader["enabled"]),
                                        storeID_CSV = storeIDs
                                    };

                                    var response = new
                                    {
                                        IsValid = true,
                                        user = userData
                                    };

                                    return Ok(response);
                                }
                            }
                        }

                        // If the username doesn't exist or password doesn't match
                        var errorResponse = new
                        {
                            IsValid = false
                        };
                        return Ok(errorResponse);
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        // CreateUserController.cs
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


        // EditUser.cs

         // POST SVSU_CIS424/EditUser
        [HttpPost]
        [Route("EditUser")]
        public IHttpActionResult EditUser([FromBody] User user)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.SQL_Conn))
                {
                    connection.Open();

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_EditUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters for the stored procedure.
                        command.Parameters.AddWithValue("@name", user.name);
                        command.Parameters.AddWithValue("@ID",user.ID);
                        command.Parameters.AddWithValue("@username",user.username);
                        command.Parameters.AddWithValue("@position",user.position);
                        command.Parameters.AddWithValue("@storeCSV",user.storeCSV);


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
        [Route("UpdateMaximums")]
        public IHttpActionResult UpdateStoreAndTotals([FromBody] MaximumDenominations data)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.SQL_Conn))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_UpdateStoreAndTotals", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@storeID", data.storeId);
                        command.Parameters.AddWithValue("@enabled", data.enabled);
                        command.Parameters.AddWithValue("@opened", data.opened);
                        command.Parameters.AddWithValue("@hundredMax", data.Hundred_Register);
                        command.Parameters.AddWithValue("@fiftyMax", data.Fifty_Register);
                        command.Parameters.AddWithValue("@twentyMax", data.Twenty_Register);
                        command.Parameters.AddWithValue("@hundred", data.Hundred);
                        command.Parameters.AddWithValue("@fifty", data.Fifty);
                        command.Parameters.AddWithValue("@twenty", data.Twenty);
                        command.Parameters.AddWithValue("@ten", data.Ten);
                        command.Parameters.AddWithValue("@five", data.Five);
                        command.Parameters.AddWithValue("@two", data.Two);
                        command.Parameters.AddWithValue("@one", data.One);
                        command.Parameters.AddWithValue("@quarterRoll", data.QuarterRoll);
                        command.Parameters.AddWithValue("@dimeRoll", data.DimeRoll);
                        command.Parameters.AddWithValue("@nickelRoll", data.NickelRoll);
                        command.Parameters.AddWithValue("@pennyRoll", data.PennyRoll);

                        command.ExecuteNonQuery();
                    }
                }

                var response = new
                {
                    Message = "Store and Totals updated successfully."
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }


        // MoveUser.cs is in the MoveController.cs


        // ViewUsersController.cs is here now

         // GET SVSU_CIS424/ViewUsers
        // Returns a list of all users in the database
        [HttpGet]
        [Route("ViewUsers")]
        public IHttpActionResult ViewUsers()
        {
             //if (!AuthenticateRequest(Request))
           // {
                // Return unauthorized response with custom message
           //     return Content(HttpStatusCode.Unauthorized, "Unauthorized: Invalid or missing API key.");
           // }

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.SQL_Conn))
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
            //if (!AuthenticateRequest(Request))
           // {
                // Return unauthorized response with custom message
           //     return Content(HttpStatusCode.Unauthorized, "Unauthorized: Invalid or missing API key.");
           // }
            
                try
                {
                    using (SqlConnection connection = new SqlConnection(ConnectionString.SQL_Conn))
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
                                    string storeID_CSV = reader["StoreID_CSV"].ToString();
                                    string[] storeIDs = storeID_CSV.Split(',');

                                    var userData = new
                                    {
                                        ID = (int)reader["ID"],
                                        username = reader["username"].ToString(),
                                        name = reader["name"].ToString(),
                                        position = reader["position"].ToString(),
                                        enabled = Convert.ToBoolean(reader["enabled"]),
                                        storeID_CSV = storeIDs
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
             //if (!AuthenticateRequest(Request))
           // {
                // Return unauthorized response with custom message
           //     return Content(HttpStatusCode.Unauthorized, "Unauthorized: Invalid or missing API key.");
           // }

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.SQL_Conn))
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
             //if (!AuthenticateRequest(Request))
           // {
                // Return unauthorized response with custom message
           //     return Content(HttpStatusCode.Unauthorized, "Unauthorized: Invalid or missing API key.");
           // }
            
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.SQL_Conn))
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

        [HttpGet]
        [Route("Test")]
        public IHttpActionResult GetHelloWorld()
        {
            if (!AuthenticateRequest(Request))
             {
             //Return unauthorized response with custom message
                 return Content(HttpStatusCode.Unauthorized, "Unauthorized: Invalid or missing API key.");
             }
            return Ok("Hello, FrontEnd Team!");
        }

        [HttpPost]
        [Route("Test")]
        public IHttpActionResult PostHelloWorld()
        {
            if (!AuthenticateRequest(Request))
            {
                //Return unauthorized response with custom message
                return Content(HttpStatusCode.Unauthorized, "Unauthorized: Invalid or missing API key.");
            }
            return Ok("Hello, FrontEnd Team!");
        }











    }
}
