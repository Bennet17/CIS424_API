﻿using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;
using System.Web.Http.Cors;
using CIS424_API.Models;
using BCrypt.Net;

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
    }
}
