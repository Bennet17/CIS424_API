﻿using System;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using CIS424_API.Models;

namespace CIS424_API.Controllers
{
    [RoutePrefix("SVSU_CIS424")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AuthenticateUserController : ApiController
    {
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

                    using (SqlCommand command = new SqlCommand("sp_GetPasswordByUsername", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@username", user.username);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedHashedPassword = reader["hashPassword"].ToString();
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
                            else
                            {
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
                return InternalServerError(ex);
            }
        }
    }
}
