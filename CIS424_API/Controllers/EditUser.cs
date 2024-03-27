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
    public class EditUserController : BaseApiController
    {
        // POST SVSU_CIS424/EditUser
        [HttpPost]
        [Route("EditUser")]
        public IHttpActionResult EditUser([FromBody] User user)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.SQL_Conn))
                {
                    connection.Open();

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_EditUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters for the stored procedure.
                        command.Parameters.AddWithValue("@name", user.name);
                        command.Parameters.AddWithValue("@ID",user.ID);
                        command.Parameters.AddWithValue("@username",user.username);
                        command.Parameters.AddWithValue("@position",user.position);
                        command.Parameters.AddWithValue("@storeCSV",user.storeCSV);


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
        [HttpPost]
        [Route("UpdateMaximums")]
        public IHttpActionResult UpdateStoreAndTotals([FromBody] MaximumDenominations data)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.SQL_Conn))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_UpdateStoreAndTotals", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@storeID", data.storeId);
                        command.Parameters.AddWithValue("@enabled", data.enabled);
                        command.Parameters.AddWithValue("@opened", data.opened);
                        command.Parameters.AddWithValue("@hundredMax", data.Hundred_Register);
                        command.Parameters.AddWithValue("@fiftyMax", data.Fifty_Register);
                        command.Parameters.AddWithValue("@twentyMax", data.Twenty_Register);
                        command.Parameters.AddWithValue("@hundred", data.Hundred);
                        command.Parameters.AddWithValue("@fifty", data.Fifty);
                        command.Parameters.AddWithValue("@twenty", data.Twenty);
                        command.Parameters.AddWithValue("@ten", data.Ten);
                        command.Parameters.AddWithValue("@five", data.Five);
                        command.Parameters.AddWithValue("@two", data.Two);
                        command.Parameters.AddWithValue("@one", data.One);
                        command.Parameters.AddWithValue("@quarterRoll", data.QuarterRoll);
                        command.Parameters.AddWithValue("@dimeRoll", data.DimeRoll);
                        command.Parameters.AddWithValue("@nickelRoll", data.NickelRoll);
                        command.Parameters.AddWithValue("@pennyRoll", data.PennyRoll);

                        command.ExecuteNonQuery();
                    }
                }

                var response = new
                {
                    Message = "Store and Totals updated successfully."
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}