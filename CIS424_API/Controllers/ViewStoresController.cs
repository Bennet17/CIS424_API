using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using CIS424_API.Models;

namespace CIS424_API.Controllers
{
    [RoutePrefix("SVSU_CIS424")]
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class ViewStoresController : ApiController
    {
        // GET SVSU_CIS424/ViewStores
        // Returns a list of all stores in the database
        [HttpGet]
        [Route("ViewStores")]
        public IHttpActionResult ViewStores()
        {
            string connectionString = "Server=tcp:capsstone-server-01.database.windows.net,1433;Initial Catalog=capstone_db_01;Persist Security Info=False;User ID=SA_Admin;Password=Capstone424!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_ViewStores", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Create a SqlDataReader object
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            List<Store> stores = new List<Store>();
                            while (reader.Read())
                            {
                                Store store = new Store();
                                store.ID = Convert.ToInt32(reader["ID"]);
                                store.location = reader["location"].ToString();
                                // Add more properties as needed
                                stores.Add(store);
                            }
                            return Ok(stores);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Return InternalServerError status code along with the exception message
                return Content(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
