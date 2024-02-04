using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using CIS424_API.Models;

namespace CIS424_API.Controllers
{
    [RoutePrefix("SVSU_CIS424")]
    public class CashCountController : ApiController
    {

        //POST SVSU_CIS424/CreateCashCount
        [HttpPost]
        [Route("CreateCashCount")]
        public IHttpActionResult Post([FromBody] CreateCashCount createCashCount)
        {
            string connectionString = "Server=tcp:capsstone-server-01.database.windows.net,1433;Initial Catalog=capstone_db_01;Persist Security Info=False;User ID=SA_Admin;Password=Capstone424!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            decimal total = (createCashCount.hundred ?? 0) * 100 +
                (createCashCount.fifty ?? 0) * 50 +
                (createCashCount.twenty ?? 0) * 20 +
                (createCashCount.ten ?? 0) * 10 +
                (createCashCount.five ?? 0) * 5 +
                (createCashCount.two ?? 0) * 2 +
                (createCashCount.one ?? 0) +
                (createCashCount.dollarCoin ?? 0) +
                (createCashCount.halfDollar ?? 0) * 0.5m +
                (createCashCount.quarter ?? 0) * 0.25m +
                (createCashCount.dime ?? 0) * 0.1m +
                (createCashCount.nickel ?? 0) * 0.05m +
                (createCashCount.penny ?? 0) * 0.01m +
                (createCashCount.quarterRoll ?? 0) * 10 +
                (createCashCount.dimeRoll ?? 0) * 5 +
                (createCashCount.nickelRoll ?? 0) * 2 +
                (createCashCount.pennyRoll ?? 0) * 0.5m;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_CreateCashCount", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters for the stored procedure.
                        command.Parameters.AddWithValue("@usrID", createCashCount.usrID);
                        command.Parameters.AddWithValue("@total", total);
                        command.Parameters.AddWithValue("@hundred", createCashCount.hundred ?? 0);
                        command.Parameters.AddWithValue("@fifty", createCashCount.fifty ?? 0);
                        command.Parameters.AddWithValue("@twenty", createCashCount.twenty ?? 0);
                        command.Parameters.AddWithValue("@ten", createCashCount.ten ?? 0);
                        command.Parameters.AddWithValue("@five", createCashCount.five ?? 0);
                        command.Parameters.AddWithValue("@two", createCashCount.two ?? 0);
                        command.Parameters.AddWithValue("@one", createCashCount.one ?? 0);
                        command.Parameters.AddWithValue("@dollarCoin", createCashCount.dollarCoin ?? 0);
                        command.Parameters.AddWithValue("@halfDollar", createCashCount.halfDollar ?? 0);
                        command.Parameters.AddWithValue("@quarter", createCashCount.quarter ?? 0);
                        command.Parameters.AddWithValue("@dime", createCashCount.dime ?? 0);
                        command.Parameters.AddWithValue("@nickel", createCashCount.nickel ?? 0);
                        command.Parameters.AddWithValue("@penny", createCashCount.penny ?? 0);
                        command.Parameters.AddWithValue("@quarterRoll", createCashCount.quarterRoll ?? 0);
                        command.Parameters.AddWithValue("@dimeRoll", createCashCount.dimeRoll ?? 0);
                        command.Parameters.AddWithValue("@nickelRoll", createCashCount.nickelRoll ?? 0);
                        command.Parameters.AddWithValue("@pennyRoll", createCashCount.pennyRoll ?? 0);

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
    }
}