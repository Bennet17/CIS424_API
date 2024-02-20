using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Cors;
using CIS424_API.Models;

namespace CIS424_API.Controllers
{
    [RoutePrefix("SVSU_CIS424")]
    [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    public class FundTransferController : ApiController
    {

        //POST SVSU_CIS424/fundTransfer
        //Create a fund transfer and store it in the database
        [HttpPost]
        [Route("CreateFundTransfer")]
        public IHttpActionResult CreateFundTransfer([FromBody] CreateFundTransfer fundTransfer)
        {
            string connectionString = "Server=tcp:capsstone-server-01.database.windows.net,1433;Initial Catalog=capstone_db_01;Persist Security Info=False;User ID=SA_Admin;Password=Capstone424!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            decimal total = (fundTransfer.hundred ?? 0) * 100 +
                (fundTransfer.fifty ?? 0) * 50 +
                (fundTransfer.twenty ?? 0) * 20 +
                (fundTransfer.ten ?? 0) * 10 +
                (fundTransfer.five ?? 0) * 5 +
                (fundTransfer.two ?? 0) * 2 +
                (fundTransfer.one ?? 0) +
                (fundTransfer.dollarCoin ?? 0) +
                (fundTransfer.halfDollar ?? 0) * 0.5m +
                (fundTransfer.quarter ?? 0) * 0.25m +
                (fundTransfer.dime ?? 0) * 0.1m +
                (fundTransfer.nickel ?? 0) * 0.05m +
                (fundTransfer.penny ?? 0) * 0.01m +
                (fundTransfer.quarterRoll ?? 0) * 10 +
                (fundTransfer.dimeRoll ?? 0) * 5 +
                (fundTransfer.nickelRoll ?? 0) * 2 +
                (fundTransfer.pennyRoll ?? 0) * 0.5m;

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
                        command.Parameters.AddWithValue("@usrID", fundTransfer.usrID);
                        command.Parameters.AddWithValue("@origin", fundTransfer.origin);
                        command.Parameters.AddWithValue("@destination", fundTransfer.destination);
                        command.Parameters.AddWithValue("@total", total);
                        command.Parameters.AddWithValue("@hundred", fundTransfer.hundred ?? 0);
                        command.Parameters.AddWithValue("@fifty", fundTransfer.fifty ?? 0);
                        command.Parameters.AddWithValue("@twenty", fundTransfer.twenty ?? 0);
                        command.Parameters.AddWithValue("@ten", fundTransfer.ten ?? 0);
                        command.Parameters.AddWithValue("@five", fundTransfer.five ?? 0);
                        command.Parameters.AddWithValue("@two", fundTransfer.two ?? 0);
                        command.Parameters.AddWithValue("@one", fundTransfer.one ?? 0);
                        command.Parameters.AddWithValue("@dollarCoin", fundTransfer.dollarCoin ?? 0);
                        command.Parameters.AddWithValue("@halfDollar", fundTransfer.halfDollar ?? 0);
                        command.Parameters.AddWithValue("@quarter", fundTransfer.quarter ?? 0);
                        command.Parameters.AddWithValue("@dime", fundTransfer.dime ?? 0);
                        command.Parameters.AddWithValue("@nickel", fundTransfer.nickel ?? 0);
                        command.Parameters.AddWithValue("@penny", fundTransfer.penny ?? 0);
                        command.Parameters.AddWithValue("@quarterRoll", fundTransfer.quarterRoll ?? 0);
                        command.Parameters.AddWithValue("@dimeRoll", fundTransfer.dimeRoll ?? 0);
                        command.Parameters.AddWithValue("@nickelRoll", fundTransfer.nickelRoll ?? 0);
                        command.Parameters.AddWithValue("@pennyRoll", fundTransfer.pennyRoll ?? 0);

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