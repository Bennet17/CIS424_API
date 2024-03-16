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
    public class CashCountController : ApiController
    {

        // POST SVSU_CIS424/CreateCashCount
        //Creates a Cash Count entry in the database
        //The stored procedure for this route has not been adjusted to include a registerID
        //So the model has not been changed to reflect that for consistency sake.
        [HttpPost]
        [Route("CreateCashCount")]
        public IHttpActionResult CreateCashCount([FromBody] CreateCashCount createCashCount)
        {
            string connectionString = "Server=tcp:capsstone-server-01.database.windows.net,1433;Initial Catalog=capstone_db_01;Persist Security Info=False;User ID=SA_Admin;Password=Capstone424!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_CreateCashCount", connection))
                    {
                        if (createCashCount.itemCounted == "SAFE") 
                        {
                            if (createCashCount.type == "CLOSE")
                            {
                                //needs to check for bank deposit
                            }
                            else
                            {
                                GenerateCommand(command, createCashCount);
                            }
                        } 
                        else 
                        {
                            if (createCashCount.type == "CLOSE")
                            {
                                //needs to check for safe deposit
                            }
                            else
                            {
                                GenerateCommand(command, createCashCount);
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



        private void GenerateCommand(SqlCommand command, CreateCashCount createCashCount)
        {
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@storeID", createCashCount.storeID);
            command.Parameters.AddWithValue("@usrID", createCashCount.usrID);
            command.Parameters.AddWithValue("@total", createCashCount.total);
            command.Parameters.AddWithValue("@type", createCashCount.type);
            command.Parameters.AddWithValue("@itemCounted", createCashCount.itemCounted);
            command.Parameters.AddWithValue("@amountExpected", createCashCount.amountExpected);
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