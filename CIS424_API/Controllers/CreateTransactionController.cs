using System;
using System.Data.SqlClient;
using System.Data;
using System.Web.Http;
using System.Web.Http.Cors;
using CIS424_API.Models;

namespace CIS424_API.Controllers
{
    [RoutePrefix("SVSU_CIS424")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CashCountController : BaseApiController
    {

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