using System;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data;
using System.Web.Http.Cors;
using CIS424_API.Models;
using System.Collections.Generic;
using System.Net;

namespace CIS424_API.Controllers
{
    [RoutePrefix("SVSU_CIS424")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RegisterController : BaseApiController
    {

        // POST SVSU_CIS424/CreateRegister
        // Creates a new register for a store via it's storeID and an alias if requested
        [HttpPost]
        [Route("CreateRegister")]
        public IHttpActionResult CreateRegister([FromBody] Register register)
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

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_CreateRegister", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters for the stored procedure.
                        command.Parameters.AddWithValue("@storeID", register.storeID);

                        //If the value is null, convert to dbnull and pass that in for the alias
                        if (register.alias == null)
                        {
                            command.Parameters.AddWithValue("@alias", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@alias", register.alias);
                        }

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

        // POST SVSU_CIS424/EnableRegister
        // Takes a register ID and enables it in the database
        [HttpPost]
        [Route("EnableRegister")]

        public IHttpActionResult EnableRegister([FromBody] Register Register)
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

        // POST SVSU_CIS424/DisableRegister
        // Disables a register in the database
        [HttpPost]
        [Route("DisableRegister")]
        public IHttpActionResult DisableRegister([FromBody] Register Register)
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

                    using (SqlCommand command = new SqlCommand("sp_DisableRegister", connection))
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

        // GET SVSU_CIS424/ViewRegistersByStoreID
        // Returns a list of all users in the database for a store by the storeID
        [HttpGet]
        [Route("ViewRegistersByStoreID")]
        public IHttpActionResult ViewRegistersByStoreID([FromUri] int storeID)
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

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_ViewRegistersByStoreID", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameter for the stored procedure.
                        command.Parameters.AddWithValue("@storeID", storeID);

                        // Create a SqlDataReader object
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            List<Register> registers = new List<Register>();
                            while (reader.Read())
                            {
                                Register register = new Register();
                                register.ID = Convert.ToInt32(reader["ID"]);
                                register.storeID = Convert.ToInt32(reader["storeID"]);
                                register.name = reader["name"].ToString();
                                register.opened = Convert.ToBoolean(reader["opened"]);
                                register.enabled = Convert.ToBoolean(reader["enabled"]);
                                register.alias = Convert.ToString(reader["alias"]);
                                registers.Add(register);
                            }
                            return Ok(registers);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return Content(HttpStatusCode.InternalServerError, e);
            }
        }

        // GET SVSU_CIS424/UpdateRegisterAlias
        // Updates the alias of a register by taking in that registers ID and a new alias
        [HttpPost]
        [Route("UpdateRegisterAlias")]
        public IHttpActionResult UpdateregisterAlias([FromBody] Register register)
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

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_UpdateRegisterAlias", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameter for the stored procedure.
                        command.Parameters.AddWithValue("@registerID", register.ID);
                        command.Parameters.AddWithValue("@alias", register.alias);

                        // A simple update does not require a response object
                        // Return a generic 200 OK instead for success.
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                //Return generic okay
                                return Ok();
                            }
                            else
                            {
                                // Return 404 Not Found if no data is found in the database.
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