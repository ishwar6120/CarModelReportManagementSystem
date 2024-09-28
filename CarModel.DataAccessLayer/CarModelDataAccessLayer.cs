using CarModel.DataAccessLayer.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static CarModel.DataAccessLayer.Model.EnumsConstant.enumConstant;

namespace CarModel.DataAccessLayer
{
    public class CarModelDataAccessLayer : ICarModelDataAccessLayer
    {
        private readonly string _connectionString;

        public CarModelDataAccessLayer( IConfiguration configuration )
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<int> AddCarAsync(CarModels car)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var imagePaths = string.Join(",", car.Images);
                    var command = new SqlCommand(
                        "INSERT INTO CarModels (Brand, Class, ModelName, ModelCode, Description, Features, Price, DateOfManufacturing, Active, SortOrder ,ImagePaths ) OUTPUT INSERTED.Id VALUES (@Brand, @Class, @ModelName, @ModelCode, @Description, @Features, @Price, @DateOfManufacturing, @Active, @SortOrder,@ImagePaths )",
                        connection);
                    command.Parameters.AddWithValue("@Brand", car.Brand.ToString());
                    command.Parameters.AddWithValue("@Class", car.Class.ToString());
                    command.Parameters.AddWithValue("@ModelName", car.ModelName);
                    command.Parameters.AddWithValue("@ModelCode", car.ModelCode);
                    command.Parameters.AddWithValue("@Description", car.Description);
                    command.Parameters.AddWithValue("@Features", car.Features);
                    command.Parameters.AddWithValue("@Price", car.Price);
                    command.Parameters.AddWithValue("@DateOfManufacturing", car.DateOfManufacturing);
                    command.Parameters.AddWithValue("@Active", car.Active);
                    command.Parameters.AddWithValue("@SortOrder", car.SortOrder);
                    command.Parameters.AddWithValue("@ImagePaths", imagePaths);

                    var carId = (int)await command.ExecuteScalarAsync();
                    return carId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching car price: {ex.Message}");
                throw new ApplicationException("An error occurred while fetching the car price.", ex);
            }
        }

      
        public async Task<List<CarModels>> GetAllCarListAsync()
        {
            var cars = new List<CarModels>();
            try {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var query = "SELECT * FROM CarModels ORDER BY  DateOfManufacturing DESC";   

                    using (var command = new SqlCommand(query, connection))
                    {
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var car = new CarModels
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    Brand = (Brand)Enum.Parse(typeof(Brand), reader.GetString(reader.GetOrdinal("Brand"))),
                                    Class = (CarClass)Enum.Parse(typeof(CarClass), reader.GetString(reader.GetOrdinal("Class"))),
                                    ModelName = reader.GetString(reader.GetOrdinal("ModelName")),
                                    ModelCode = reader.GetString(reader.GetOrdinal("ModelCode")),
                                    Description = reader.GetString(reader.GetOrdinal("Description")),
                                    Features = reader.GetString(reader.GetOrdinal("Features")),
                                    Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                    DateOfManufacturing = reader.GetDateTime(reader.GetOrdinal("DateOfManufacturing")),
                                    Active = reader.GetBoolean(reader.GetOrdinal("Active")),
                                    SortOrder = reader.GetInt32(reader.GetOrdinal("SortOrder")),
                                    Images = new List<string>() // Populate images as needed
                                };
                                // Retrieve and split the image paths
                                var imagePaths = reader.GetString(reader.GetOrdinal("ImagePaths"));
                                if (!string.IsNullOrEmpty(imagePaths))
                                {
                                    car.Images = imagePaths.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList(); // Split and populate the list
                                }

                                cars.Add(car);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching car price: {ex.Message}");
                throw new ApplicationException("An error occurred while fetching the car price.", ex);
            }

            return cars;
        }

        public async Task<List<CarModels>> SearchCarAsync(string modelName, string modelCode)
        {
            var cars = new List<CarModels>();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var command = new SqlCommand("SELECT * FROM CarModels WHERE ModelName LIKE @ModelName OR ModelCode LIKE @ModelCode", connection);
                    command.Parameters.AddWithValue("@ModelName", $"%{modelName}%");
                    command.Parameters.AddWithValue("@ModelCode", $"%{modelCode}%");

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var car = new CarModels
                            {
                                Id = reader.GetInt32(0),
                                Brand = (Brand)Enum.Parse(typeof(Brand), reader.GetString(1)),
                                Class = (CarClass)Enum.Parse(typeof(CarClass), reader.GetString(2)),
                                ModelName = reader.GetString(3),
                                ModelCode = reader.GetString(4),
                                Description = reader.GetString(5),
                                Features = reader.GetString(6),
                                Price = reader.GetDecimal(7),
                                DateOfManufacturing = reader.GetDateTime(8),
                                Active = reader.GetBoolean(9),
                                SortOrder = reader.GetInt32(10),
                                Images = new List<string>() // Initialize the list for images
                            };

                            // Retrieve and split the image paths
                            var imagePaths = reader.GetString(reader.GetOrdinal("ImagePaths"));
                            if (!string.IsNullOrEmpty(imagePaths))
                            {
                                car.Images = imagePaths.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList(); // Split and populate the list
                            }

                            cars.Add(car); // Add the car to the list
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching car price: {ex.Message}");
                throw new ApplicationException("An error occurred while fetching the car price.", ex);
            }
            return cars;
        }

    }
}
