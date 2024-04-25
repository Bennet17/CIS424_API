using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Web.Http;
using System.Web.Http.Cors;
using CIS424_API.Models;

namespace CIS424_API.Controllers
{
    [RoutePrefix("SVSU_CIS424")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StoreController : BaseApiController
    {

        // POST SVSU_CIS424/CreateStore
        // Creates a store location in the database
        [HttpPost]
        [Route("CreateStore")]
        public IHttpActionResult CreateStore([FromBody] Store store)
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

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_CreateStore", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters for the stored procedure.
                        command.Parameters.AddWithValue("@location", store.location);
                        command.Parameters.AddWithValue("@hundredRegisterMax", store.hundredRegisterMax);
                        command.Parameters.AddWithValue("@fiftyRegisterMax", store.fiftyRegisterMax);
                        command.Parameters.AddWithValue("@twentyRegisterMax", store.twentyRegisterMax);
                        command.Parameters.AddWithValue("@hundredMax", store.hundredMax);
                        command.Parameters.AddWithValue("@fiftyMax", store.fiftyMax);
                        command.Parameters.AddWithValue("@twentyMax", store.twentyMax);
                        command.Parameters.AddWithValue("@tenMax", store.tenMax);
                        command.Parameters.AddWithValue("@fiveMax", store.fiveMax);
                        command.Parameters.AddWithValue("@twoMax", store.twoMax);
                        command.Parameters.AddWithValue("@oneMax", store.oneMax);
                        command.Parameters.AddWithValue("@quarterRollMax", store.quarterRollMax);
                        command.Parameters.AddWithValue("@dimeRollMax", store.dimeRollMax);
                        command.Parameters.AddWithValue("@nickelRollMax", store.nickelRollMax);
                        command.Parameters.AddWithValue("@pennyRollMax", store.pennyRollMax);

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

        // POST SVSU_CIS424/EnableStore
        // Enables a store in the database
        [HttpPost]
        [Route("EnableStore")]
        public IHttpActionResult EnableStore([FromBody] Store Store)
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
                    //open connection
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_EnableStore", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        //add param
                        command.Parameters.AddWithValue("@ID", Store.ID);

                        SqlParameter resultMessageParam = new SqlParameter("@ResultMessage", SqlDbType.VarChar, 255);
                        resultMessageParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(resultMessageParam);
                        //execute query
                        command.ExecuteNonQuery();

                        string resultMessage = resultMessageParam.Value.ToString();

                        return Ok(new { response = resultMessage });
                    }

                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        // POST SVSU_CIS424/DisableStore
        // Disables a store in the database
        [HttpPost]
        [Route("DisableStore")]
        public IHttpActionResult DisableStore([FromBody] Store Store)
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
                    //open connection
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_DisableStore", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        //add param
                        command.Parameters.AddWithValue("@ID", Store.ID);


                        SqlParameter resultMessageParam = new SqlParameter("@ResultMessage", SqlDbType.VarChar, 255);
                        resultMessageParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(resultMessageParam);
                        //execute query
                        command.ExecuteNonQuery();

                        string resultMessage = resultMessageParam.Value.ToString();

                        return Ok(new { response = resultMessage });
                    }

                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }

        // GET SVSU_CIS424/ViewStores
        // Returns a list of all stores in the database
        [HttpGet]
        [Route("ViewStores")]
        public IHttpActionResult ViewStores()
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

        // GET SVSU_CIS424/ViewStoreThresholds
        // Returns a list of all thresholds
        [HttpGet]
        [Route("ViewStoreThresholds")]
        public IHttpActionResult ViewStoreThresholds(int storeID)
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

        // Set store maximums
        // Updates the maximums for a store and safe
        [HttpPost]
        [Route("UpdateMaximums")]
        public IHttpActionResult UpdateStoreAndTotals([FromBody] MaximumDenominations data)
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

                    using (SqlCommand command = new SqlCommand("sp_UpdateStoreAndTotals", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@storeID", data.storeId);
                        command.Parameters.AddWithValue("@enabled", data.enabled);
                        command.Parameters.AddWithValue("@location", data.location);
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

        // GET SVSU_CIS424/ViewStoreObjects
        // Returns a list of a store, if it's open, all that stores registers, and if they are open
        [HttpGet]
        [Route("ViewStoreObjects")]
        public IHttpActionResult ViewStoreObjects([FromUri] int storeID)
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
