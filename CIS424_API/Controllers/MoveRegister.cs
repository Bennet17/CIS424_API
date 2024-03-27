// using System;
// using System.Collections.Generic;
// using System.Data.SqlClient;
// using System.Data;
// using System.Linq;
// using System.Net;
// using System.Net.Http;
// using System.Web.Helpers;
// using System.Web.Http;
// using System.Web.Http.Cors;
// using CIS424_API.Models;

// namespace CIS424_API.Controllers
// {
//     [RoutePrefix("SVSU_CIS424")]
//     [EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
//     public class MoveRegisterController : ApiController
//     {
//         // GET SVSU_CIS424/ViewStores
//         // Returns a list of all stores in the database
//         [HttpPost]
//         [Route("MoveRegister")]

//         public IHttpActionResult MoveRegister([FromBody] Register Register)
//         {

//             try
//             {
//                 using (SqlConnection connection = new SqlConnection(ConnectionString.SQL_Conn))
//                 {
//                     connection.Open();

//                     using (SqlCommand command = new SqlCommand("sp_MoveRegister", connection))
//                     {
//                         command.CommandType = CommandType.StoredProcedure;

//                         command.Parameters.AddWithValue("@RegID", Register.ID);
//                         command.Parameters.AddWithValue("@LocID", Register.storeID);

//                         SqlParameter resultMessageParam = new SqlParameter("@ResultMessage", SqlDbType.VarChar, 255);
//                         resultMessageParam.Direction = ParameterDirection.Output;
//                         command.Parameters.Add(resultMessageParam);

//                         command.ExecuteNonQuery();

//                         string resultMessage = resultMessageParam.Value.ToString();

//                         return Ok(new { response = resultMessage });
//                     }

//                 }
//             }
//             catch (Exception ex)
//             {
//                 return InternalServerError(ex);
//             }

//         }
//     }
// }
