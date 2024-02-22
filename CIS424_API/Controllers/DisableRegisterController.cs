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
    public class DisableRegisterController : ApiController
    {
        // POST SVSU_CIS424/DisableRegister
        // Disables a register in the database
        [HttpPost]
        [Route("DisableRegister")]

        public IHttpActionResult DisableRegister([FromBody] Register Register)
        {
            string connectionString = "Server=tcp:capsstone-server-01.database.windows.net,1433;Initial Catalog=capstone_db_01;Persist Security Info=False;User ID=SA_Admin;Password=Capstone424!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

            try 
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_DisableRegister", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ID", Regsiter.ID);

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
