using System;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data;
using System.Web.Http.Cors;
using CIS424_API.Models;
using System.Collections.Generic;
using System.Net;

namespace CIS424_API.Controllers
{
    [RoutePrefix("SVSU_CIS424")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MoneyController : BaseApiController
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

        // POST SVSU_CIS424/GetOpenCount
        [HttpGet]
        [Route("GetOpenCount")]
        //For get requests using ASP.NET Framework 4.8, simple datatypes are implicitly read from the URI
        public IHttpActionResult GetOpenCount(int storeID, int registerID)
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

                    using (SqlCommand command = new SqlCommand("sp_GetOpenExpectedAmount", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@storeID", storeID);
                        command.Parameters.AddWithValue("@registerID", registerID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                decimal expectedAmount = reader.GetDecimal(0); // Assuming the result is a decimal value
                                return Ok(expectedAmount);
                            }
                            else
                            {
                                // No rows returned
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

        [HttpGet]
        [Route("GetCloseCount")]
        public IHttpActionResult GetCloseCount([FromUri] int storeID)
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

                    using (SqlCommand command = new SqlCommand("sp_GetCloseExpectedAmount", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@storeID", storeID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Totals expectedAmount = new Totals{
                                    total = Convert.ToDecimal(reader["total"]),
                                    hundred = Convert.ToInt32(reader["hundred"]),
                                    fifty = Convert.ToInt32(reader["fifty"]),
                                    twenty = Convert.ToInt32(reader["twenty"]),
                                    ten = Convert.ToInt32(reader["ten"]),
                                    five = Convert.ToInt32(reader["five"]),
                                    two = Convert.ToInt32(reader["two"]),
                                    one = Convert.ToInt32(reader["one"]),
                                    dollarCoin = Convert.ToInt32(reader["dollarCoin"]),
                                    halfDollar = Convert.ToInt32(reader["halfDollar"]),
                                    quarter = Convert.ToInt32(reader["quarter"]),
                                    dime = Convert.ToInt32(reader["dime"]),
                                    nickel = Convert.ToInt32(reader["nickel"]),
                                    penny = Convert.ToInt32(reader["penny"]),
                                    quarterRoll = Convert.ToInt32(reader["quarterRoll"]),
                                    dimeRoll = Convert.ToInt32(reader["dimeRoll"]),
                                    nickelRoll = Convert.ToInt32(reader["nickelRoll"]),
                                    pennyRoll = Convert.ToInt32(reader["pennyRoll"])
                                };
                                return Ok(expectedAmount);
                            }
                            else
                            {
                                // No rows returned
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

        // POST SVSU_CIS424/CreateCashCount
        //Creates a Cash Count entry in the database
        //The stored procedure for this route has not been adjusted to include a registerID
        //So the model has not been changed to reflect that for consistency sake.
        [HttpPost]
        [Route("CreateCashCount")]
        public IHttpActionResult CreateCashCount([FromBody] CreateCashCount createCashCount)
        {

                         //if (!AuthenticateRequest(Request))
           // {
                // Return unauthorized response with custom message
           //     return Content(HttpStatusCode.Unauthorized, "Unauthorized: Invalid or missing API key.");
           // }

            //string connectionString = "Server=tcp:capsstone-server-01.database.windows.net,1433;Initial Catalog=capstone_db_01;Persist Security Info=False;User ID=SA_Admin;Password=Capstone424!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.SQL_Conn))
                {
                    connection.Open();

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_CreateCashCount", connection))
                    {
                        if (createCashCount.itemCounted == "SAFE") 
                        {
                            if (createCashCount.type == "CLOSE")
                            {
                                if (createCashCount.cashToBankTotal > 0) 
                                {
                                    GenerateSafeCloseCommand(command, createCashCount);
                                }
                                else
                                {
                                    GenerateCountCommand(command, createCashCount);
                                }
                            }
                            else
                            {
                                GenerateCountCommand(command, createCashCount);
                            }
                        } 
                        else 
                        {
                            if (createCashCount.type == "CLOSE")
                            {
                               GenerateRegCloseCommand(command, createCashCount);
                            }
                            else
                            {
                                GenerateCountCommand(command, createCashCount);
                            }
                        }
                        
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

         // POST SVSU_CIS424/CreateFundTransfer
        //Create a fund transfer and store it in the database
        [HttpPost]
        [Route("CreateFundTransfer")]
        public IHttpActionResult CreateFundTransfer([FromBody] CreateFundTransfer fundTransfer)
        {
            
            Totals totals = new Totals();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.SQL_Conn))
                {
                    connection.Open();

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_createFundTransfer", connection))
                    {

                        GenerateTransferCommand(command, fundTransfer);

                        SqlParameter resultMessageParam = new SqlParameter("@ResultMessage", SqlDbType.VarChar, 255);
                        resultMessageParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(resultMessageParam);

                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        // Retrieve the result message
                        string resultMessage = resultMessageParam.Value.ToString();

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

        private void GenerateTransferCommand(SqlCommand command, CreateFundTransfer fundTransfer)
        {
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@usrID", fundTransfer.usrID);
            command.Parameters.AddWithValue("@storeID", fundTransfer.storeID);
            command.Parameters.AddWithValue("@origin", fundTransfer.origin);
            command.Parameters.AddWithValue("@destination", fundTransfer.destination);
            command.Parameters.AddWithValue("@total", fundTransfer.total);
            command.Parameters.AddWithValue("@hundred", fundTransfer.hundred);
            command.Parameters.AddWithValue("@fifty", fundTransfer.fifty);
            command.Parameters.AddWithValue("@twenty", fundTransfer.twenty);
            command.Parameters.AddWithValue("@ten", fundTransfer.ten);
            command.Parameters.AddWithValue("@five", fundTransfer.five);
            command.Parameters.AddWithValue("@two", fundTransfer.two);
            command.Parameters.AddWithValue("@one", fundTransfer.one);
            command.Parameters.AddWithValue("@dollarCoin", fundTransfer.dollarCoin);
            command.Parameters.AddWithValue("@halfDollar", fundTransfer.halfDollar);
            command.Parameters.AddWithValue("@quarter", fundTransfer.quarter);
            command.Parameters.AddWithValue("@dime", fundTransfer.dime);
            command.Parameters.AddWithValue("@nickel", fundTransfer.nickel);
            command.Parameters.AddWithValue("@penny", fundTransfer.penny);
            command.Parameters.AddWithValue("@quarterRoll", fundTransfer.quarterRoll);
            command.Parameters.AddWithValue("@dimeRoll", fundTransfer.dimeRoll);
            command.Parameters.AddWithValue("@nickelRoll", fundTransfer.nickelRoll);
            command.Parameters.AddWithValue("@pennyRoll", fundTransfer.pennyRoll);
        }

        private void GenerateCountCommand(SqlCommand command, CreateCashCount createCashCount)
        {
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@storeID", createCashCount.storeID);
            command.Parameters.AddWithValue("@usrID", createCashCount.usrID);
            command.Parameters.AddWithValue("@type", createCashCount.type);
            command.Parameters.AddWithValue("@itemCounted", createCashCount.itemCounted);
            command.Parameters.AddWithValue("@amountExpected", createCashCount.amountExpected);
            command.Parameters.AddWithValue("@total", createCashCount.total);
            command.Parameters.AddWithValue("@hundred", createCashCount.hundred);
            command.Parameters.AddWithValue("@fifty", createCashCount.fifty);
            command.Parameters.AddWithValue("@twenty", createCashCount.twenty);
            command.Parameters.AddWithValue("@ten", createCashCount.ten);
            command.Parameters.AddWithValue("@five", createCashCount.five);
            command.Parameters.AddWithValue("@two", createCashCount.two);
            command.Parameters.AddWithValue("@one", createCashCount.one);
            command.Parameters.AddWithValue("@dollarCoin", createCashCount.dollarCoin);
            command.Parameters.AddWithValue("@halfDollar", createCashCount.halfDollar);
            command.Parameters.AddWithValue("@quarter", createCashCount.quarter);
            command.Parameters.AddWithValue("@dime", createCashCount.dime);
            command.Parameters.AddWithValue("@nickel", createCashCount.nickel);
            command.Parameters.AddWithValue("@penny", createCashCount.penny);
            command.Parameters.AddWithValue("@quarterRoll", createCashCount.quarterRoll);
            command.Parameters.AddWithValue("@dimeRoll", createCashCount.dimeRoll);
            command.Parameters.AddWithValue("@nickelRoll", createCashCount.nickelRoll);
            command.Parameters.AddWithValue("@pennyRoll", createCashCount.pennyRoll);
        }

        private void GenerateRegCloseCommand(SqlCommand command, CreateCashCount createCashCount)
        {
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@storeID", createCashCount.storeID);
            command.Parameters.AddWithValue("@usrID", createCashCount.usrID);
            command.Parameters.AddWithValue("@type", createCashCount.type);
            command.Parameters.AddWithValue("@itemCounted", createCashCount.itemCounted);
            command.Parameters.AddWithValue("@amountExpected", createCashCount.amountExpected);

            command.Parameters.AddWithValue("@creditExpected", createCashCount.creditExpected);
            command.Parameters.AddWithValue("@creditActual", createCashCount.creditActual);
            command.Parameters.AddWithValue("@cashToSafeTotal", createCashCount.cashToSafeTotal);
            command.Parameters.AddWithValue("@hundredToSafe", createCashCount.hundredToSafe);
            command.Parameters.AddWithValue("@fiftyToSafe", createCashCount.fiftyToSafe);
            command.Parameters.AddWithValue("@twentyToSafe", createCashCount.twentyToSafe);

            command.Parameters.AddWithValue("@total", createCashCount.total);
            command.Parameters.AddWithValue("@hundred", createCashCount.hundred);
            command.Parameters.AddWithValue("@fifty", createCashCount.fifty);
            command.Parameters.AddWithValue("@twenty", createCashCount.twenty);
            command.Parameters.AddWithValue("@ten", createCashCount.ten);
            command.Parameters.AddWithValue("@five", createCashCount.five);
            command.Parameters.AddWithValue("@two", createCashCount.two);
            command.Parameters.AddWithValue("@one", createCashCount.one);
            command.Parameters.AddWithValue("@dollarCoin", createCashCount.dollarCoin);
            command.Parameters.AddWithValue("@halfDollar", createCashCount.halfDollar);
            command.Parameters.AddWithValue("@quarter", createCashCount.quarter);
            command.Parameters.AddWithValue("@dime", createCashCount.dime);
            command.Parameters.AddWithValue("@nickel", createCashCount.nickel);
            command.Parameters.AddWithValue("@penny", createCashCount.penny);
            command.Parameters.AddWithValue("@quarterRoll", createCashCount.quarterRoll);
            command.Parameters.AddWithValue("@dimeRoll", createCashCount.dimeRoll);
            command.Parameters.AddWithValue("@nickelRoll", createCashCount.nickelRoll);
            command.Parameters.AddWithValue("@pennyRoll", createCashCount.pennyRoll);
        }

        private void GenerateSafeCloseCommand(SqlCommand command, CreateCashCount createCashCount)
        {
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@storeID", createCashCount.storeID);
            command.Parameters.AddWithValue("@usrID", createCashCount.usrID);
            command.Parameters.AddWithValue("@type", createCashCount.type);
            command.Parameters.AddWithValue("@itemCounted", createCashCount.itemCounted);
            command.Parameters.AddWithValue("@amountExpected", createCashCount.amountExpected);

            command.Parameters.AddWithValue("@cashToBankTotal", createCashCount.cashToBankTotal);
            command.Parameters.AddWithValue("@hundredToBank", createCashCount.hundredToBank);
            command.Parameters.AddWithValue("@fiftyToBank", createCashCount.fiftyToBank);
            command.Parameters.AddWithValue("@twentyToBank", createCashCount.twentyToBank);
            command.Parameters.AddWithValue("@tenToBank", createCashCount.tenToBank);
            command.Parameters.AddWithValue("@fiveToBank", createCashCount.fiveToBank);
            command.Parameters.AddWithValue("@twoToBank", createCashCount.twoToBank);
            command.Parameters.AddWithValue("@oneToBank", createCashCount.oneToBank);
            command.Parameters.AddWithValue("@quarterRollToBank", createCashCount.quarterRollToBank);
            command.Parameters.AddWithValue("@dimeRollToBank", createCashCount.dimeRollToBank);
            command.Parameters.AddWithValue("@nickelRollToBank", createCashCount.nickelRollToBank);
            command.Parameters.AddWithValue("@pennyRollToBank", createCashCount.pennyRollToBank);

            command.Parameters.AddWithValue("@total", createCashCount.total);
            command.Parameters.AddWithValue("@hundred", createCashCount.hundred);
            command.Parameters.AddWithValue("@fifty", createCashCount.fifty);
            command.Parameters.AddWithValue("@twenty", createCashCount.twenty);
            command.Parameters.AddWithValue("@ten", createCashCount.ten);
            command.Parameters.AddWithValue("@five", createCashCount.five);
            command.Parameters.AddWithValue("@two", createCashCount.two);
            command.Parameters.AddWithValue("@one", createCashCount.one);
            command.Parameters.AddWithValue("@dollarCoin", createCashCount.dollarCoin);
            command.Parameters.AddWithValue("@halfDollar", createCashCount.halfDollar);
            command.Parameters.AddWithValue("@quarter", createCashCount.quarter);
            command.Parameters.AddWithValue("@dime", createCashCount.dime);
            command.Parameters.AddWithValue("@nickel", createCashCount.nickel);
            command.Parameters.AddWithValue("@penny", createCashCount.penny);
            command.Parameters.AddWithValue("@quarterRoll", createCashCount.quarterRoll);
            command.Parameters.AddWithValue("@dimeRoll", createCashCount.dimeRoll);
            command.Parameters.AddWithValue("@nickelRoll", createCashCount.nickelRoll);
            command.Parameters.AddWithValue("@pennyRoll", createCashCount.pennyRoll);
        }

    }
}