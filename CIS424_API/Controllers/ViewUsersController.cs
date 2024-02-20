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
{
    [RoutePrefix("SVSU_CIS424")]
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class ViewUsersController : ApiController
    {
        // GET SVSU_CIS424/ViewUsers
        // Returns a list of all users in the database
        [HttpGet]
        [Route("ViewUsers")]
        public IHttpActionResult Get()
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
                                user.username = reader["username"].ToString();
                                user.name = reader["name"].ToString();
                                user.position = reader["position"].ToString();
                                user.storeID = Convert.ToInt32(reader["storeID"]);
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
        public IHttpActionResult GetUsersByStoreID([FromUri] int storeID)
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

                        // Create a SqlDataReader object
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            List<User> users = new List<User>();
                            while (reader.Read())
                            {
                                User user = new User();
                                users.Add(user);
                                user.username = reader["username"].ToString();
                                user.ID = Convert.ToInt32(reader["ID"]);
                                user.name = reader["name"].ToString();
                                user.position = reader["position"].ToString();
                                user.storeID = Convert.ToInt32(reader["storeID"]);
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
    }
}