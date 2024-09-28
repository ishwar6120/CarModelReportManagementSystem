using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace CarModel.DataAccessLayer
{
    public class CommissionDataAccessLayer : ICommissionDataAccessLayer
    {
        private readonly string _connectionString;

        public CommissionDataAccessLayer(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<decimal?> GetPreviousYearSalesAsync(string salesman)
        {
            decimal? LastYearTotalSaleAmount = null;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var command = new SqlCommand("SELECT LastYearTotalSaleAmount FROM PreviousYearSales WHERE Salesman = @salesman", connection);
                    command.Parameters.AddWithValue("@salesman", salesman);

                    await connection.OpenAsync();
                    var result = await command.ExecuteScalarAsync();
                    if (result != null)
                    {
                        LastYearTotalSaleAmount = (decimal)result;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching previous year sales: {ex.Message}");
            }

            return LastYearTotalSaleAmount;
        }

        public async Task<decimal> GetCarPriceAsync(string brand, string carClass)
        {
            decimal price = 0;

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = "SELECT Price FROM CarModels WHERE Brand = @Brand AND Class = @Class";

                    using (var command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Brand", brand);
                        command.Parameters.AddWithValue("@Class", carClass);

                        var result = await command.ExecuteScalarAsync();

                        if (result != null && result != DBNull.Value)
                        {
                            price = Convert.ToDecimal(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching car price: {ex.Message}");
                throw new ApplicationException("An error occurred while fetching the car price.", ex);
            }

            return price;
        }

    }
}