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
    public class GetExpectedCountController : ApiController
    {
        // POST SVSU_CIS424/GetOpenCount
        [HttpGet]
        [Route("GetOpenCount")]
        //For get requests using ASP.NET Framework 4.8, simple datatypes are implicitly read from the URI
        public IHttpActionResult GetOpenCount(int storeID, int registerID)
        {

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
    }
}