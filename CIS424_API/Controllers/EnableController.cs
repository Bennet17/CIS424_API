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
    public class EnableController : BaseApiController
    {
        // POST SVSU_CIS424/EnableUser
        // Enables a user in the database
        [HttpPost]
        [Route("EnableUser")]

        public IHttpActionResult EnableUser([FromBody] User User)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.SQL_Conn))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_EnableUser", connection))
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

        // POST SVSU_CIS424/EnableStore
        // Enables a store in the database
        [HttpPost]
        [Route("EnableStore")]

        public IHttpActionResult EnableStore([FromBody] Store Store)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.SQL_Conn))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_EnableStore", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ID", Store.ID);

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

        // POST SVSU_CIS424/EnableRegister
        // Returns a list of all stores in the database
        [HttpPost]
        [Route("EnableRegister")]

        public IHttpActionResult EnableRegister([FromBody] Register Register)
        {

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString.SQL_Conn))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand("sp_EnableRegister", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@ID", Register.ID);

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