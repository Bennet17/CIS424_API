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
    public class GetExpectedCountController : BaseApiController
    {
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
                                var expectedAmount = new {
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
                                    pennyRoll = Convert.ToInt32(reader["pennyRoll"]),
                                    first = Convert.ToBoolean(reader["first"])
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
    }
}