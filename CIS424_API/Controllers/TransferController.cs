﻿using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Data.SqlClient;
using System.Data;
using System.Web.Http.Cors;
using CIS424_API.Models;

namespace CIS424_API.Controllers
{
    [RoutePrefix("SVSU_CIS424")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class TransferController : ApiController
    {
        [HttpGet]
        [Route("GetFundTransfersForStore")]
        //Route
        //GET GetTransferForStore
        public IHttpActionResult GetFundTransfersForStore([FromUri] int storeID, [FromUri] String startDate, [FromUri] String endDate)
        {
            string connectionString = "Server=tcp:capsstone-server-01.database.windows.net,1433;Initial Catalog=capstone_db_01;Persist Security Info=False;User ID=SA_Admin;Password=Capstone424!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_GetFundTransfersForStore", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameter for the stored procedure.
                        command.Parameters.AddWithValue("@storeID", storeID);
                        command.Parameters.AddWithValue("@startDate", startDate);
                        command.Parameters.AddWithValue("@endDate", endDate);

                        // Modify your response object to hold a list of Funds Transfer objects
                        List<FundsTransferResponse> responseList = new List<FundsTransferResponse>();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Populate the VarianceResponse object for each row in the result set.
                                FundsTransferResponse response = new FundsTransferResponse
                                {
                                    name = Convert.ToString(reader["name"]),
                                    date = Convert.ToDateTime(reader["date"]),
                                    origin = Convert.ToString(reader["origin"]),
                                    destination = Convert.ToString(reader["destination"]),
                                    total = Convert.ToSingle(reader["total"]),
                                    hundred = Convert.ToInt16(reader["hundred"]),
                                    fifty = Convert.ToInt16(reader["fifty"]),
                                    twenty = Convert.ToInt16(reader["twenty"]),
                                    ten = Convert.ToInt16(reader["ten"]),
                                    five = Convert.ToInt16(reader["five"]),
                                    two = Convert.ToInt16(reader["two"]),
                                    one = Convert.ToInt16(reader["one"]),
                                    dollarCoin = Convert.ToInt16(reader["dollarCoin"]),
                                    halfDollar = Convert.ToInt16(reader["halfDollar"]),
                                    quarter = Convert.ToInt16(reader["quarter"]),
                                    dime = Convert.ToInt16(reader["dime"]),
                                    nickel = Convert.ToInt16(reader["nickel"]),
                                    penny = Convert.ToInt16(reader["penny"]),
                                    quarterRoll = Convert.ToInt16(reader["quarterRoll"]),
                                    dimeRoll = Convert.ToInt16(reader["dimeRoll"]),
                                    nickelRoll = Convert.ToInt16(reader["nickelRoll"]),
                                    pennyRoll = Convert.ToInt16(reader["pennyRoll"])
                                };

                                // Add the response object to the list
                                responseList.Add(response);
                            }

                            if (responseList.Count > 0)
                            {
                                // Return the list of response objects as JSON.
                                return Ok(responseList);
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