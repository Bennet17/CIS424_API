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
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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
                                store.enabled = Convert.ToBoolean(reader["enabled"]);
                                store.opened = Convert.ToBoolean(reader["opened"]);
                                store.hundredRegisterMax = Convert.ToInt32(reader["hundredRegisterMax"]);
                                store.fiftyRegisterMax = Convert.ToInt32(reader["fiftyRegisterMax"]);
                                store.twentyRegisterMax = Convert.ToInt32(reader["twentyRegisterMax"]);
                                store.hundredMax = Convert.ToInt32(reader["hundredMax"]);
                                store.fiftyMax = Convert.ToInt32(reader["fiftyMax"]);
                                store.twentyMax = Convert.ToInt32(reader["twentyMax"]);
                                store.tenMax = Convert.ToInt32(reader["tenMax"]);
                                store.fiveMax = Convert.ToInt32(reader["fiveMax"]);
                                store.twoMax = Convert.ToInt32(reader["twoMax"]);
                                store.oneMax = Convert.ToInt32(reader["oneMax"]);
                                store.quarterRollMax = Convert.ToInt32(reader["quarterRollMax"]);
                                store.dimeRollMax = Convert.ToInt32(reader["dimeRollMax"]);
                                store.nickelRollMax = Convert.ToInt32(reader["nickelRollMax"]);
                                store.pennyRollMax = Convert.ToInt32(reader["pennyRollMax"]);
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

        [HttpGet]
        [Route("ViewStoreThresholds")]
        public IHttpActionResult ViewStoreThresholds([FromUri] int storeID)
        {
            string connectionString = "Server=tcp:capsstone-server-01.database.windows.net,1433;Initial Catalog=capstone_db_01;Persist Security Info=False;User ID=SA_Admin;Password=Capstone424!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_GetStoreThresholds", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameter for the stored procedure.
                        command.Parameters.AddWithValue("@storeID", storeID);
                        var maxThreshold = new MaxThreshold();
                        maxThreshold.storeID = storeID;

                        // Create a SqlDataReader object
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                maxThreshold.hundredRegisterMax = (int)reader["hundredMax"];
                                maxThreshold.fiftyRegisterMax = (int)reader["fiftyMax"];
                                maxThreshold.twentyRegisterMax = (int)reader["twentyMax"];
                                maxThreshold.hundredMax = (int)reader["hundred"];
                                maxThreshold.fiftyMax = (int)reader["fifty"];
                                maxThreshold.twentyMax = (int)reader["twenty"];
                                maxThreshold.tenMax = (int)reader["ten"];
                                maxThreshold.fiveMax = (int)reader["five"];
                                maxThreshold.twoMax = (int)reader["two"];
                                maxThreshold.oneMax = (int)reader["one"];
                                maxThreshold.quarterRollMax = (int)reader["quarterRoll"];
                                maxThreshold.dimeRollMax = (int)reader["dimeRoll"];
                                maxThreshold.nickelRollMax = (int)reader["nickelRoll"];
                                maxThreshold.pennyRollMax = (int)reader["pennyRoll"];
                            }
                        }

                        return Ok(maxThreshold); // Return the list of user data

                    }
                }
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e);
            }
        }
        
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
                                obj.regID = Convert.ToInt32(reader["regID"]);      
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
            public int regID { get; set; }
            public string name { get; set; }
            public bool opened { get; set; }
        }
    }
}
