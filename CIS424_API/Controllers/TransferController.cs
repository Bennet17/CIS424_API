using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data;
using System.Web.Http.Cors;
using CIS424_API.Models;

namespace CIS424_API.Controllers
{
    [RoutePrefix("SVSU_CIS424")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TransferController : BaseApiController
    {
        [HttpGet]
        [Route("GetFundTransfersForStore")]
        //Route
        //GET GetTransferForStore
        public IHttpActionResult GetFundTransfersForStore([FromUri] int storeID, [FromUri] String startDate, [FromUri] String endDate)
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
                    using (SqlCommand command = new SqlCommand("sp_GetFundTransfersForStore", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameter for the stored procedure.
                        command.Parameters.AddWithValue("@storeID", storeID);
                        command.Parameters.AddWithValue("@startDate", startDate);
                        command.Parameters.AddWithValue("@endDate", endDate);

                        // Modify your response object to hold a list of Funds Transfer objects
                        List<FundTransfer> responseList = new List<FundTransfer>();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Populate the VarianceResponse object for each row in the result set.
                                FundTransfer response = new FundTransfer
                                {
                                    fID = Convert.ToInt16(reader["fundTransferID"]),
                                    name = Convert.ToString(reader["name"]),
                                    date = Convert.ToDateTime(reader["date"]),
                                    origin = Convert.ToString(reader["origin"]),
                                    destination = Convert.ToString(reader["destination"]),
                                    status = Convert.ToString(reader["status"]),
                                    total = Convert.ToDecimal(reader["total"])
                                };

                                // Add the response object to the list
                                responseList.Add(response);
                            }

                            if (responseList.Count > 0)
                            {
                                // Return the list of response objects as JSON.
                                return Ok(responseList);
                            }
                            else if (responseList.Count == 0)
                            {
                                //If the request was valid but no data was found, return a custom message.
                                return Ok("No transfers or deposits were found between " + startDate + " and " + endDate);
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

        [HttpPost]
        [Route("VerifyDeposit")]
        //Route
        //GET GetTransferForStore
        public IHttpActionResult VerifyDeposit([FromBody] FundTransfer fundTransfer)
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
                    using (SqlCommand command = new SqlCommand("sp_VerifyDeposit", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameter for the stored procedure.
                        command.Parameters.AddWithValue("@fID", fundTransfer.fID);
                        command.Parameters.AddWithValue("@vID", fundTransfer.verifiedBy);

                        FundTransfer response = new FundTransfer();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                //Only populate the necessary information instead of making a whole new object
                                response.fID = Convert.ToInt16(reader["fundTransferID"]);
                                response.name = Convert.ToString(reader["name"]);
                                response.date = Convert.ToDateTime(reader["date"]);
                                response.origin = Convert.ToString(reader["origin"]);
                                response.destination = Convert.ToString(reader["destination"]);
                                response.status = Convert.ToString(reader["status"]);
                                response.total = Convert.ToDecimal(reader["total"]);
                                response.verifiedBy = Convert.ToInt16(reader["verifiedBy"]);
                                response.verifiedOn = Convert.ToDateTime(reader["verifiedOn"]);
                            }
                        }

                        if (response != null)
                        {
                            // Return the list of response objects as JSON.
                            return Ok(response);
                        }
                        else if (response == null)
                        {
                            //If the request was valid but no data was found, return a custom message.
                            return Ok("Deposit status could not be updated");
                        }
                        else
                        {
                            // Return 404 if the request fails for any other reason.
                            return NotFound();
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

        [HttpPost]
        [Route("AbortDeposit")]
        //Route
        //GET GetTransferForStore
        public IHttpActionResult AbortDeposit([FromBody] FundTransfer fundTransfer)
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
                    using (SqlCommand command = new SqlCommand("sp_AbortDeposit", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameter for the stored procedure.
                        command.Parameters.AddWithValue("@fID", fundTransfer.fID);

                        FundTransfer response = new FundTransfer();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //Only populate the necessary information instead of making a whole new object
                                response.fID = Convert.ToInt16(reader["fundTransferID"]);
                                response.name = Convert.ToString(reader["name"]);
                                response.date = Convert.ToDateTime(reader["date"]);
                                response.origin = Convert.ToString(reader["origin"]);
                                response.destination = Convert.ToString(reader["destination"]);
                                response.status = Convert.ToString(reader["status"]);
                                response.total = Convert.ToDecimal(reader["total"]);

                            }
                        }

                        if (response != null)
                        {
                            // Return the list of response objects as JSON.
                            return Ok(response);
                        }
                        else if (response == null)
                        {
                            //If the request was valid but no data was found, return a custom message.
                            return Ok("Deposit status could not be updated");
                        }
                        else
                        {
                            // Return 404 if the request fails for any other reason.
                            return NotFound();
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