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
    public class DisableController : BaseApiController
    {
        // POST SVSU_CIS424/DisableUser
        // Disables a user in the database
        [HttpPost]
        [Route("DisableUser")]
        public IHttpActionResult DisableUser([FromBody] User User)
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

                    using (SqlCommand command = new SqlCommand("sp_DisableUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ID", User.ID);

                        SqlParameter resultMessageParam = new SqlParameter("@ResultMessage", SqlDbType.VarChar, 255);
                        resultMessageParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(resultMessageParam);

                        command.ExecuteNonQuery();

                        string resultMessage = resultMessageParam.Value.ToString();

                        return Ok(new { response = resultMessage });
                    }

                }
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

        }
    }
}
