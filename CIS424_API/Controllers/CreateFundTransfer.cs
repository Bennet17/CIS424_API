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
    public class FundTransferController : ApiController
    {

        //POST SVSU_CIS424/createFundTransfer
        [HttpPost]
        [Route("CreateFundTransfer")]
        public IHttpActionResult Post([FromBody] CreateFundTransfer createFundTransfer)
        {
            string connectionString = "Data Source=DESKTOP-OR5B156;Initial Catalog=capstone_db_01;Integrated Security=True;";

            decimal total = (createFundTransfer.hundred ?? 0) * 100 +
                (createFundTransfer.fifty ?? 0) * 50 +
                (createFundTransfer.twenty ?? 0) * 20 +
                (createFundTransfer.ten ?? 0) * 10 +
                (createFundTransfer.five ?? 0) * 5 +
                (createFundTransfer.two ?? 0) * 2 +
                (createFundTransfer.one ?? 0) +
                (createFundTransfer.dollarCoin ?? 0) +
                (createFundTransfer.halfDollar ?? 0) * 0.5m +
                (createFundTransfer.quarter ?? 0) * 0.25m +
                (createFundTransfer.dime ?? 0) * 0.1m +
                (createFundTransfer.nickel ?? 0) * 0.05m +
                (createFundTransfer.penny ?? 0) * 0.01m +
                (createFundTransfer.quarterRoll ?? 0) * 10 +
                (createFundTransfer.dimeRoll ?? 0) * 5 +
                (createFundTransfer.nickelRoll ?? 0) * 2 +
                (createFundTransfer.pennyRoll ?? 0) * 0.5m;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_createFundTransfer", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters for the stored procedure.
                        command.Parameters.AddWithValue("@usrID", createFundTransfer.usrID);
                        command.Parameters.AddWithValue("@origin", createFundTransfer.origin);
                        command.Parameters.AddWithValue("@destination", createFundTransfer.destination);
                        command.Parameters.AddWithValue("@total", total);
                        command.Parameters.AddWithValue("@hundred", createFundTransfer.hundred ?? 0);
                        command.Parameters.AddWithValue("@fifty", createFundTransfer.fifty ?? 0);
                        command.Parameters.AddWithValue("@twenty", createFundTransfer.twenty ?? 0);
                        command.Parameters.AddWithValue("@ten", createFundTransfer.ten ?? 0);
                        command.Parameters.AddWithValue("@five", createFundTransfer.five ?? 0);
                        command.Parameters.AddWithValue("@two", createFundTransfer.two ?? 0);
                        command.Parameters.AddWithValue("@one", createFundTransfer.one ?? 0);
                        command.Parameters.AddWithValue("@dollarCoin", createFundTransfer.dollarCoin ?? 0);
                        command.Parameters.AddWithValue("@halfDollar", createFundTransfer.halfDollar ?? 0);
                        command.Parameters.AddWithValue("@quarter", createFundTransfer.quarter ?? 0);
                        command.Parameters.AddWithValue("@dime", createFundTransfer.dime ?? 0);
                        command.Parameters.AddWithValue("@nickel", createFundTransfer.nickel ?? 0);
                        command.Parameters.AddWithValue("@penny", createFundTransfer.penny ?? 0);
                        command.Parameters.AddWithValue("@quarterRoll", createFundTransfer.quarterRoll ?? 0);
                        command.Parameters.AddWithValue("@dimeRoll", createFundTransfer.dimeRoll ?? 0);
                        command.Parameters.AddWithValue("@nickelRoll", createFundTransfer.nickelRoll ?? 0);
                        command.Parameters.AddWithValue("@pennyRoll", createFundTransfer.pennyRoll ?? 0);

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