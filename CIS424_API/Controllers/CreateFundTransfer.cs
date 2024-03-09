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
    public class FundTransferController : ApiController
    {

        // POST SVSU_CIS424/CreateFundTransfer
        //Create a fund transfer and store it in the database
        [HttpPost]
        [Route("CreateFundTransfer")]
        public IHttpActionResult CreateFundTransfer([FromBody] CreateFundTransfer fundTransfer)
        {
            string connectionString = "Server=tcp:capsstone-server-01.database.windows.net,1433;Initial Catalog=capstone_db_01;Persist Security Info=False;User ID=SA_Admin;Password=Capstone424!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
            
            decimal total = 0.0;
            total = (fundTransfer.hundred ?? 0) * 100 +
                (fundTransfer.fifty ?? 0) * 50 +
                (fundTransfer.twenty ?? 0) * 20 +
                (fundTransfer.ten ?? 0) * 10 +
                (fundTransfer.five ?? 0) * 5 +
                (fundTransfer.two ?? 0) * 2 +
                (fundTransfer.one ?? 0) +
                (fundTransfer.dollarCoin ?? 0) +
                (fundTransfer.halfDollar ?? 0) * 0.5m +
                (fundTransfer.quarter ?? 0) * 0.25m +
                (fundTransfer.dime ?? 0) * 0.1m +
                (fundTransfer.nickel ?? 0) * 0.05m +
                (fundTransfer.penny ?? 0) * 0.01m +
                (fundTransfer.quarterRoll ?? 0) * 10 +
                (fundTransfer.dimeRoll ?? 0) * 5 +
                (fundTransfer.nickelRoll ?? 0) * 2 +
                (fundTransfer.pennyRoll ?? 0) * 0.5m;

            Totals totals = new Totals();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_createFundTransfer", connection))
                    {
                        // Check if origin or destination is 'SAFE'
                        if (fundTransfer.origin == "SAFE" || fundTransfer.destination == "SAFE")
                        {

                            GenerateTransferCommand(command, fundTransfer);

                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    // Populate the Totals object with values from the result set
                                    totals.storeID = (int)reader["storeID"];
                                    totals.total = (decimal)reader["total"];
                                    totals.hundred = (int)reader["hundred"];
                                    totals.fifty = (int)reader["fifty"];
                                    totals.twenty = (int)reader["twenty"];
                                    totals.ten = (int)reader["ten"];
                                    totals.five = (int)reader["five"];
                                    totals.two = (int)reader["two"];
                                    totals.one = (int)reader["one"];
                                    totals.dollarCoin = (int)reader["dollarCoin"];
                                    totals.halfDollar = (int)reader["halfDollar"];
                                    totals.quarter = (int)reader["quarter"];
                                    totals.dime = (int)reader["dime"];
                                    totals.nickel = (int)reader["nickel"];
                                    totals.penny = (int)reader["penny"];
                                    totals.quarterRoll = (int)reader["quarterRoll"];
                                    totals.dimeRoll = (int)reader["dimeRoll"];
                                    totals.nickelRoll = (int)reader["nickelRoll"];
                                    totals.pennyRoll = (int)reader["pennyRoll"];
                                }
                            }
                        }
                        else
                        {

                         GenerateTransferCommand(command, fundTransfer);

                         // Execute the stored procedure
                         command.ExecuteNonQuery();

                         // Retrieve the result message
                         string resultMessage = resultMessageParam.Value.ToString();

                         return Ok(new { response = resultMessage });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during database operations.
                return InternalServerError(ex);
            }
            
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a SqlCommand object for the stored procedure.
                    using (SqlCommand command = new SqlCommand("sp_updateSafeTotal", connection))
                    {
                        // Check if origin or destination is 'SAFE'
                        if (fundTransfer.origin == "SAFE")
                        {
                            totals.total -= checkNegative(tota);
                            totals.hundred -= checkNegative(fundTransfer.hundred);
                            totals.fifty -= checkNegative(fundTransfer.fifty);
                            totals.twenty -= checkNegative(fundTransfer.twenty);
                            totals.ten -= checkNegative(fundTransfer.ten);
                            totals.five -= checkNegative(fundTransfer.five);
                            totals.two -= checkNegative(fundTransfer.two);
                            totals.one -= checkNegative(fundTransfer.one);
                            totals.dollarCoin -= checkNegative(fundTransfer.dollarCoin);
                            totals.halfDollar -= checkNegative(fundTransfer.halfDollar);
                            totals.quarter -= checkNegative(fundTransfer.quarter);
                            totals.dime -= checkNegative(fundTransfer.dime);
                            totals.nickel -= checkNegative(fundTransfer.nickel);
                            totals.penny -= checkNegative(fundTransfer.penny);
                            totals.quarterRoll -= checkNegative(fundTransfer.quarterRoll);
                            totals.dimeRoll -= checkNegative(fundTransfer.dimeRoll);
                            totals.nickelRoll -= checkNegative(fundTransfer.nickelRoll);
                            totals.pennyRoll -= checkNegative(fundTransfer.pennyRoll);
                        } 
                        if (fundTransfer.destination == "SAFE")
                        { 
                            totals.total += total;
                            totals.hundred += fundTransfer.hundred;
                            totals.fifty += fundTransfer.fifty;
                            totals.twenty += fundTransfer.twenty;
                            totals.ten += fundTransfer.ten;
                            totals.five += fundTransfer.five;
                            totals.two += fundTransfer.two;
                            totals.one += fundTransfer.one;
                            totals.dollarCoin += fundTransfer.dollarCoin;
                            totals.halfDollar += fundTransfer.halfDollar;
                            totals.quarter += fundTransfer.quarter;
                            totals.dime += fundTransfer.dime;
                            totals.nickel += fundTransfer.nickel;
                            totals.penny += fundTransfer.penny;
                            totals.quarterRoll += fundTransfer.quarterRoll;
                            totals.dimeRoll += fundTransfer.dimeRoll;
                            totals.nickelRoll += fundTransfer.nickelRoll;
                            totals.pennyRoll += fundTransfer.pennyRoll;
                        }

                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@storeID", total.storeID);
                        command.Parameters.AddWithValue("@total", total.total);
                        command.Parameters.AddWithValue("@hundred", total.hundred);
                        command.Parameters.AddWithValue("@fifty", total.fifty);
                        command.Parameters.AddWithValue("@twenty", total.twenty);
                        command.Parameters.AddWithValue("@ten", total.ten);
                        command.Parameters.AddWithValue("@five", total.five);
                        command.Parameters.AddWithValue("@two", total.two);
                        command.Parameters.AddWithValue("@one", total.one);
                        command.Parameters.AddWithValue("@dollarCoin", total.dollarCoin);
                        command.Parameters.AddWithValue("@halfDollar", total.halfDollar);
                        command.Parameters.AddWithValue("@quarter", total.quarter);
                        command.Parameters.AddWithValue("@dime", total.dime);
                        command.Parameters.AddWithValue("@nickel", total.nickel);
                        command.Parameters.AddWithValue("@penny", total.penny);
                        command.Parameters.AddWithValue("@quarterRoll", total.quarterRoll);
                        command.Parameters.AddWithValue("@dimeRoll", total.dimeRoll);
                        command.Parameters.AddWithValue("@nickelRoll", total.nickelRoll);
                        command.Parameters.AddWithValue("@pennyRoll", total.pennyRoll);

                        SqlParameter resultMessageParam = new SqlParameter("@ResultMessage", SqlDbType.VarChar, 255);
                        resultMessageParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(resultMessageParam);
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during database operations.
                return InternalServerError(ex);
            }
        }

        private int checkNegative(int total)
        {
            if (total < 0)
            {
                return 0;
            }
            return total;
        }

        private int checkNegative(decimal total)
        {
            if (total < 0.0)
            {
                return 0.0;
            }
            return total;
        }

        private void GenerateTransferCommand(SqlCommand command, CreateFundTransfer fundTransfer)
        {
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@usrID", fundTransfer.usrID);
            command.Parameters.AddWithValue("@origin", fundTransfer.origin);
            command.Parameters.AddWithValue("@destination", fundTransfer.destination);
            command.Parameters.AddWithValue("@total", total);
            command.Parameters.AddWithValue("@hundred", fundTransfer.hundred ?? 0);
            command.Parameters.AddWithValue("@fifty", fundTransfer.fifty ?? 0);
            command.Parameters.AddWithValue("@twenty", fundTransfer.twenty ?? 0);
            command.Parameters.AddWithValue("@ten", fundTransfer.ten ?? 0);
            command.Parameters.AddWithValue("@five", fundTransfer.five ?? 0);
            command.Parameters.AddWithValue("@two", fundTransfer.two ?? 0);
            command.Parameters.AddWithValue("@one", fundTransfer.one ?? 0);
            command.Parameters.AddWithValue("@dollarCoin", fundTransfer.dollarCoin ?? 0);
            command.Parameters.AddWithValue("@halfDollar", fundTransfer.halfDollar ?? 0);
            command.Parameters.AddWithValue("@quarter", fundTransfer.quarter ?? 0);
            command.Parameters.AddWithValue("@dime", fundTransfer.dime ?? 0);
            command.Parameters.AddWithValue("@nickel", fundTransfer.nickel ?? 0);
            command.Parameters.AddWithValue("@penny", fundTransfer.penny ?? 0);
            command.Parameters.AddWithValue("@quarterRoll", fundTransfer.quarterRoll ?? 0);
            command.Parameters.AddWithValue("@dimeRoll", fundTransfer.dimeRoll ?? 0);
            command.Parameters.AddWithValue("@nickelRoll", fundTransfer.nickelRoll ?? 0);
            command.Parameters.AddWithValue("@pennyRoll", fundTransfer.pennyRoll ?? 0);

            if (fundTransfer.origin != "SAFE" && fundTransfer.destination != "SAFE")
            {
                SqlParameter resultMessageParam = new SqlParameter("@ResultMessage", SqlDbType.VarChar, 255);
                resultMessageParam.Direction = ParameterDirection.Output;
                command.Parameters.Add(resultMessageParam);
            }
        }
    }
} 