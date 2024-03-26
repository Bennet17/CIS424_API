using System;
using System.Data.SqlClient;
using System.Data;
using System.Web.Http;
using System.Web.Http.Cors;
using CIS424_API.Models;

// CreateStore Controller route
namespace CIS424_API.Controllers
{
    [RoutePrefix("SVSU_CIS424")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CreateStoreController : BaseApiController
    {
        // POST SVSU_CIS424/CreateStore
        // Creates a store location in the database
        [HttpPost]
        [Route("CreateStore")]
        public IHttpActionResult CreateStore([FromBody] Store store)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.SQL_Conn))
                {
                    connection.Open();

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_CreateStore", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters for the stored procedure.
                        command.Parameters.AddWithValue("@location", store.location);
                        command.Parameters.AddWithValue("@hundredRegisterMax", store.hundredRegisterMax);
                        command.Parameters.AddWithValue("@fiftyRegisterMax", store.fiftyRegisterMax);
                        command.Parameters.AddWithValue("@twentyRegisterMax", store.twentyRegisterMax);
                        command.Parameters.AddWithValue("@hundredMax", store.hundredMax);
                        command.Parameters.AddWithValue("@fiftyMax", store.fiftyMax);
                        command.Parameters.AddWithValue("@twentyMax", store.twentyMax);
                        command.Parameters.AddWithValue("@tenMax", store.tenMax);
                        command.Parameters.AddWithValue("@fiveMax", store.fiveMax);
                        command.Parameters.AddWithValue("@twoMax", store.twoMax);
                        command.Parameters.AddWithValue("@oneMax", store.oneMax);
                        command.Parameters.AddWithValue("@quarterRollMax", store.quarterRollMax);
                        command.Parameters.AddWithValue("@dimeRollMax", store.dimeRollMax);
                        command.Parameters.AddWithValue("@nickelRollMax", store.nickelRollMax);
                        command.Parameters.AddWithValue("@pennyRollMax", store.pennyRollMax);

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