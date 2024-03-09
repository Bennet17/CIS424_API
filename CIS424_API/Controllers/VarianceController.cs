using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data;
using System.Web.Helpers;
using System.Web.Http.Cors;
using CIS424_API.Models;

namespace CIS424_API.Controllers
{
    [RoutePrefix("SVSU_CIS424")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VarianceController : ApiController
    {
        // POST SVSU_CIS424/RegisterVariance
        [HttpPost]
        [Route("RegisterVariance")]
        public IHttpActionResult RegisterVariance([FromBody] RegisterVarianceRequest request)
        {
            string connectionString = "Server=tcp:capsstone-server-01.database.windows.net,1433;Initial Catalog=capstone_db_01;Persist Security Info=False;User ID=SA_Admin;Password=Capstone424!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_GetRegister7DayVariance", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameter for the stored procedure.
                        command.Parameters.AddWithValue("@registerID", request.registerID);
                        command.Parameters.AddWithValue("@startDate", request.startDate);
                        command.Parameters.AddWithValue("@endDate", request.endDate);

                        // Modify your response object to hold a list of VarianceResponse objects
                        List<VarianceResponse> responseList = new List<VarianceResponse>();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Populate the VarianceResponse object for each row in the result set.
                                VarianceResponse response = new VarianceResponse
                                {
                                    amountExpected = Convert.ToSingle(reader["amountExpected"]),
                                    total = Convert.ToSingle(reader["total"]),
                                    Variance = Convert.ToSingle(reader["Variance"]),
                                    Date = Convert.ToDateTime(reader["Date"])
                                };

                                // Add the response object to the list
                                responseList.Add(response);
                            }

                            if (responseList.Count > 0)
                            {
                                // Return the list of response objects as JSON.
                                return Ok(responseList);
                            }
                            else
                            {
                                // Return 404 Not Found if no data is found in the database.
                                return NotFound();
                            }
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

    [RoutePrefix("SVSU_CIS424")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class GeneralVarianceController : ApiController
    {
        // POST SVSU_CIS424/GeneralVariance
        [HttpPost]
        [Route("GeneralVariance")]
        public IHttpActionResult GeneralVariance([FromBody] SafeVarianceRequest request)
        {
            string connectionString = "Server=tcp:capsstone-server-01.database.windows.net,1433;Initial Catalog=capstone_db_01;Persist Security Info=False;User ID=SA_Admin;Password=Capstone424!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_GetGeneralVariance", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameter for the stored procedure.
                        command.Parameters.AddWithValue("@storeID", request.storeID);
                        command.Parameters.AddWithValue("@startDate", request.startDate);
                        command.Parameters.AddWithValue("@endDate", request.endDate);

                        // Modify your response object to hold a list of VarianceResponse objects
                        List<VarianceResponse> responseList = new List<VarianceResponse>();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Populate the VarianceResponse object for each row in the result set.
                                VarianceResponse response = new VarianceResponse
                                {
                                    amountExpected = Convert.ToSingle(reader["amountExpected"]),
                                    total = Convert.ToSingle(reader["total"]),
                                    Variance = Convert.ToSingle(reader["Variance"]),
                                    Date = Convert.ToDateTime(reader["Date"])
                                };

                                // Add the response object to the list
                                responseList.Add(response);
                            }

                            if (responseList.Count > 0)
                            {
                                // Return the list of response objects as JSON.
                                return Ok(responseList);
                            }
                            else
                            {
                                // Return 404 Not Found if no data is found in the database.
                                return NotFound();
                            }
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
}
