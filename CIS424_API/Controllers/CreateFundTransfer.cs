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
            
            decimal total = 0.0m;
            total = fundTransfer.hundred * 100 +
                fundTransfer.fifty * 50 +
                fundTransfer.twenty * 20 +
                fundTransfer.ten * 10 +
                fundTransfer.five * 5 +
                fundTransfer.two * 2 +
                fundTransfer.one +
                fundTransfer.dollarCoin +
                fundTransfer.halfDollar * 0.5m +
                fundTransfer.quarter * 0.25m +
                fundTransfer.dime * 0.1m +
                fundTransfer.nickel * 0.05m +
                fundTransfer.penny * 0.01m +
                fundTransfer.quarterRoll * 10 +
                fundTransfer.dimeRoll * 5 +
                fundTransfer.nickelRoll * 2 +
                fundTransfer.pennyRoll * 0.5m;

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

                            GenerateTransferCommand(command, fundTransfer, total);

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

                         GenerateTransferCommand(command, fundTransfer, total);

                         SqlParameter resultMessageParam = new SqlParameter("@ResultMessage", SqlDbType.VarChar, 255);
                         resultMessageParam.Direction = ParameterDirection.Output;
                         command.Parameters.Add(resultMessageParam);

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
                            totals.total -= checkNegative(total);
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

                        command.Parameters.AddWithValue("@storeID", totals.storeID);
                        command.Parameters.AddWithValue("@total", totals.total);
                        command.Parameters.AddWithValue("@hundred", totals.hundred);
                        command.Parameters.AddWithValue("@fifty", totals.fifty);
                        command.Parameters.AddWithValue("@twenty", totals.twenty);
                        command.Parameters.AddWithValue("@ten", totals.ten);
                        command.Parameters.AddWithValue("@five", totals.five);
                        command.Parameters.AddWithValue("@two", totals.two);
                        command.Parameters.AddWithValue("@one", totals.one);
                        command.Parameters.AddWithValue("@dollarCoin", totals.dollarCoin);
                        command.Parameters.AddWithValue("@halfDollar", totals.halfDollar);
                        command.Parameters.AddWithValue("@quarter", totals.quarter);
                        command.Parameters.AddWithValue("@dime", totals.dime);
                        command.Parameters.AddWithValue("@nickel", totals.nickel);
                        command.Parameters.AddWithValue("@penny", totals.penny);
                        command.Parameters.AddWithValue("@quarterRoll", totals.quarterRoll);
                        command.Parameters.AddWithValue("@dimeRoll", totals.dimeRoll);
                        command.Parameters.AddWithValue("@nickelRoll", totals.nickelRoll);
                        command.Parameters.AddWithValue("@pennyRoll", totals.pennyRoll);

                        SqlParameter resultMessageParam = new SqlParameter("@ResultMessage", SqlDbType.VarChar, 255);
                        resultMessageParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(resultMessageParam);

                        command.ExecuteNonQuery();
                         // Retrieve the result message
                        string resultMessage = resultMessageParam.Value.ToString();

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

        private int checkNegative(int total)
        {
            if (total < 0)
            {
                return 0;
            }
            else
            {
                return total;
            }
        }

        private decimal checkNegative(decimal total)
        {
            if (total < 0.00m)
            {
                return 0.00m;
            }
            else
            {
                return total;
            }
        }

        private void GenerateTransferCommand(SqlCommand command, CreateFundTransfer fundTransfer, decimal total)
        {
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@usrID", fundTransfer.usrID);
            command.Parameters.AddWithValue("@origin", fundTransfer.origin);
            command.Parameters.AddWithValue("@destination", fundTransfer.destination);
            command.Parameters.AddWithValue("@total", total);
            command.Parameters.AddWithValue("@hundred", fundTransfer.hundred);
            command.Parameters.AddWithValue("@fifty", fundTransfer.fifty);
            command.Parameters.AddWithValue("@twenty", fundTransfer.twenty);
            command.Parameters.AddWithValue("@ten", fundTransfer.ten);
            command.Parameters.AddWithValue("@five", fundTransfer.five);
            command.Parameters.AddWithValue("@two", fundTransfer.two);
            command.Parameters.AddWithValue("@one", fundTransfer.one);
            command.Parameters.AddWithValue("@dollarCoin", fundTransfer.dollarCoin);
            command.Parameters.AddWithValue("@halfDollar", fundTransfer.halfDollar);
            command.Parameters.AddWithValue("@quarter", fundTransfer.quarter);
            command.Parameters.AddWithValue("@dime", fundTransfer.dime);
            command.Parameters.AddWithValue("@nickel", fundTransfer.nickel);
            command.Parameters.AddWithValue("@penny", fundTransfer.penny);
            command.Parameters.AddWithValue("@quarterRoll", fundTransfer.quarterRoll);
            command.Parameters.AddWithValue("@dimeRoll", fundTransfer.dimeRoll);
            command.Parameters.AddWithValue("@nickelRoll", fundTransfer.nickelRoll);
            command.Parameters.AddWithValue("@pennyRoll", fundTransfer.pennyRoll);
        }
    }
} 