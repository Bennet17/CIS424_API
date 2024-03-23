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
    public class MoveController : ApiController
    {

        // GET SVSU_CIS424/ViewStores
        // Returns a list of all stores in the database
        // Move Register from one store to another using ID
        [HttpPost]
        [Route("MoveRegister")]

        public IHttpActionResult MoveRegister([FromBody] Register Register)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.SQL_Conn))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_MoveRegister", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@RegID", Register.ID);
                        command.Parameters.AddWithValue("@LocID", Register.storeID);

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



        // GET SVSU_CIS424/ViewStores
        // Returns a list of all stores in the database
        // MoveUser from one store to another using ID
        [HttpPost]
        [Route("MoveUser")]

        public IHttpActionResult MoveUser([FromBody] User User)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.SQL_Conn))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_MoveUser", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@EmpID", User.ID);
                        command.Parameters.AddWithValue("@LocID", User.storeCSV);

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