using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;
using System.Web.Http.Cors;
using CIS424_API.Models;
using BCrypt.Net;
using System.Net;

namespace CIS424_API.Controllers
{
    [RoutePrefix("SVSU_CIS424")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AuthenticateUserController : BaseApiController
    {
        // SVSU_CIS424/AuthenticateUser
        [HttpPost]
        [Route("AuthenticateUser")]
        public IHttpActionResult AuthenticateUser([FromBody] User user)
        {
             if (!AuthenticateRequest(Request))
             {
                 // Return unauthorized response with custom message
                 return Content(HttpStatusCode.Unauthorized, "Unauthorized: Invalid or missing API key.");
             }
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


        [HttpPost]
        [Route("GetQuestionByUsername")]

        public IHttpActionResult GetQuestionAndAnswerByUsername([FromBody] User user)
        {
            if (!AuthenticateRequest(Request))
            {
                // Return unauthorized response with custom message
                return Content(HttpStatusCode.Unauthorized, "Unauthorized: Invalid or missing API key.");
            }
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.SQL_Conn))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_GetQuestionByUsername", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters for the stored procedure.
                        command.Parameters.AddWithValue("@username", user.username);

                        // Output parameters
                        command.Parameters.Add("@question", SqlDbType.VarChar, -1).Direction = ParameterDirection.Output;
                        command.Parameters.Add("@ResultMessage", SqlDbType.VarChar, 255).Direction = ParameterDirection.Output;

                        // Execute the command
                        command.ExecuteNonQuery();

                        // Retrieve output parameters
                        string question = command.Parameters["@question"].Value.ToString();
                        string resultMessage = command.Parameters["@ResultMessage"].Value.ToString();

                        if (!string.IsNullOrEmpty(resultMessage))
                        {
                            // Username not found
                            var response = new
                            {
                                Response = resultMessage
                            };
                            return Content(HttpStatusCode.NotFound, response);
                        }
                        else
                        {
                            // Success, return question
                            var response = new
                            {
                                Question = question
                            };
                            return Ok(response);
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
        [Route("AuthenticateQuestion")]
        public IHttpActionResult AuthenticateQuestion([FromBody] User user)
        {
            if (!AuthenticateRequest(Request))
            {
                // Return unauthorized response with custom message
                return Content(HttpStatusCode.Unauthorized, "Unauthorized: Invalid or missing API key.");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.SQL_Conn))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_GetAnswerByUsername", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@username", user.username);

                        // Output parameters
                        SqlParameter answerParam = command.Parameters.Add("@answer", SqlDbType.VarChar, -1);
                        answerParam.Direction = ParameterDirection.Output;
                        SqlParameter resultMessageParam = command.Parameters.Add("@ResultMessage", SqlDbType.VarChar, 255);
                        resultMessageParam.Direction = ParameterDirection.Output;

                        command.ExecuteNonQuery(); // Execute the stored procedure

                        string answer = answerParam.Value?.ToString();
                        string resultMessage = resultMessageParam.Value?.ToString();

                        if (!string.IsNullOrEmpty(answer)) // If answer is not null or empty, the username exists
                        {
                            if (BCrypt.Net.BCrypt.Verify(user.answer, answer))
                            {
                                var response = new
                                {
                                    IsValid = true
                                };
                                return Ok(response);
                            }
                            else // Incorrect answer
                            {
                                var errorResponse = new
                                {
                                    IsValid = false,
                                    Message = "Incorrect answer."
                                };
                                return Ok(errorResponse);
                            }
                        }
                        else // Username not found
                        {
                            var errorResponse = new
                            {
                                IsValid = false,
                                Message = resultMessage ?? "Username not found."
                            };
                            return Ok(errorResponse);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

    }
}
