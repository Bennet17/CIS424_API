using System;
using System.Data.SqlClient;
using System.Data;
using System.Web.Http;
using System.Web.Http.Cors;
using CIS424_API.Models;

// EditUser Controller route
namespace CIS424_API.Controllers
{
    [RoutePrefix("SVSU_CIS424")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class GetOpenExpectedAmount : ApiController
    {
        // POST SVSU_CIS424/GetOpenExpectedAmount
        [HttpPost]
        [Route("GetOpenExpectedAmount")]
        public IHttpActionResult GetOpenExpectedAmount([FromBody] CreateCashCount cashCount)
        {
            string connectionString = "Server=tcp:capsstone-server-01.database.windows.net,1433;Initial Catalog=capstone_db_01;Persist Security Info=False;User ID=SA_Admin;Password=Capstone424!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_GetOpenExpectedAmount", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters for the stored procedure.
                        command.Parameters.AddWithValue("@usrID", cashCount.usrID;
                        command.Parameters.AddWithValue("@itemCounted",cashCount.itemCounted);

                        // Add output parameter
                        object result = command.ExecuteScalar();

                        decimal expectedAmount = Convert.ToDecimal(result);

                        // Return the expected amount in the response
                        return Ok(expectedAmount);
                    }
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