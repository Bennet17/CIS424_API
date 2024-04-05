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
    public class VarianceController : BaseApiController
    {
        // POST SVSU_CIS424/RegisterVariance
        [HttpGet]
        [Route("RegisterVariance")]
        public IHttpActionResult RegisterVariance([FromUri] int registerID, [FromUri] int storeID, [FromUri] String startDate, [FromUri] String endDate)
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
                    using (SqlCommand command = new SqlCommand("sp_GetRegisterVariance", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameter for the stored procedure.
                        command.Parameters.AddWithValue("@registerID", registerID);
                        command.Parameters.AddWithValue("@storeID", storeID);
                        command.Parameters.AddWithValue("@startDate", startDate);
                        command.Parameters.AddWithValue("@endDate", endDate);

                        // Modify your response object to hold a list of VarianceResponse objects
                        List<VarianceResponse> responseList = new List<VarianceResponse>();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Populate the VarianceResponse object for each row in the result set.
                                VarianceResponse response = new VarianceResponse
                                {
                                    Date = Convert.ToDateTime(reader["Date"]),
                                    POSName = reader["POSName"].ToString(),
                                    OpenerName = reader["OpenerName"].ToString(),
                                    OpenExpected = Convert.ToDecimal(reader["OpenExpected"]),
                                    OpenActual = Convert.ToDecimal(reader["OpenActual"]),
                                    CloserName = reader["CloserName"].ToString(),
                                    CloseExpected = Convert.ToDecimal(reader["CloseExpected"]),
                                    CloseActual = Convert.ToDecimal(reader["CloseActual"]),
                                    CashToSafe = Convert.ToDecimal(reader["CashToSafe"]),
                                    CloseCreditActual = Convert.ToDecimal(reader["CloseCreditActual"]),
                                    CloseCreditExpected = Convert.ToDecimal(reader["CloseCreditExpected"]),
                                    OpenVariance = Convert.ToDecimal(reader["OpenVariance"]),
                                    CloseVariance = Convert.ToDecimal(reader["CloseVariance"]),
                                    TotalCashVariance = Convert.ToDecimal(reader["TotalCashVariance"]),
                                    CreditVariance = Convert.ToDecimal(reader["CreditVariance"]),
                                    TotalVariance = Convert.ToDecimal(reader["TotalVariance"])
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

        // GET SVSU_CIS424/GeneralVariance
        [HttpGet]
        [Route("GeneralVariance")]
        public IHttpActionResult GeneralVariance([FromUri] int storeID, [FromUri] String startDate, [FromUri] String endDate)
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
                    using (SqlCommand command = new SqlCommand("sp_GetGeneralVariance", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameter for the stored procedure.
                        command.Parameters.AddWithValue("@storeID", storeID);
                        command.Parameters.AddWithValue("@startDate", startDate);
                        command.Parameters.AddWithValue("@endDate", endDate);

                        // Modify your response object to hold a list of VarianceResponse objects
                        List<GeneralVarianceResponse> responseList = new List<GeneralVarianceResponse>();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Populate the VarianceResponse object for each row in the result set.
                                GeneralVarianceResponse response = new GeneralVarianceResponse
                                {
                                    variance = Convert.ToDecimal(reader["variance"]),
                                    date = Convert.ToDateTime(reader["date"])
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
                                 return Ok(responseList);
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
        [Route("VarianceAudit")]
        public IHttpActionResult RunVarianceAudit([FromBody] VarianceAudit varianceAudit)
        {
                         //if (!AuthenticateRequest(Request))
           // {
                // Return unauthorized response with custom message
           //     return Content(HttpStatusCode.Unauthorized, "Unauthorized: Invalid or missing API key.");
           // }
            decimal cashRecord = varianceAudit.cashTendered - (varianceAudit.cashBuys + varianceAudit.pettyCash);
            decimal creditRecord = varianceAudit.mastercard + varianceAudit.visa + varianceAudit.americanExpress 
            + varianceAudit.discover + varianceAudit.debit + varianceAudit.other;
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.SQL_Conn))
                {
                    connection.Open();

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_GetVarianceAudit", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@storeID", varianceAudit.storeID);
                        command.Parameters.AddWithValue("@cashRecordDRS", cashRecord);
                        command.Parameters.AddWithValue("@creditRecordDRS", creditRecord);
                        command.Parameters.AddWithValue("@startDate", varianceAudit.startDate);
                        command.Parameters.AddWithValue("@endDate", varianceAudit.endDate);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            VarianceAuditResponse response = new VarianceAuditResponse();
                            while (reader.Read())
                            {
                                // Populate the VarianceResponse object for each row in the result set.
                                response = new VarianceAuditResponse
                                {
                                    cashVariance = Convert.ToDecimal(reader["cashvariance"]),
                                    creditVariance = Convert.ToDecimal(reader["creditVariance"]),
                                    totalVariance = Convert.ToDecimal(reader["totalVariance"])
                                };
                            }
                            return Ok(response);
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
        private class GeneralVarianceResponse
        {
            public decimal variance { get; set; }
            public DateTime date { get; set; }
        }

        private class VarianceAuditResponse
        {
            public decimal cashVariance { get; set; }
            public decimal creditVariance { get; set; }
            public decimal totalVariance { get; set; }
        }
    }
}