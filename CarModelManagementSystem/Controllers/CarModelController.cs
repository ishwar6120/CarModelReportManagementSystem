
using CarModel.BusinessLayer;
using CarModel.DataAccessLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CarModelManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarModelController : ControllerBase
    {
        private readonly ICarModelBusinessLayer _carModelBusinessLayer;

        public CarModelController(ICarModelBusinessLayer carModelBusinessLayer)
        {
            _carModelBusinessLayer = carModelBusinessLayer;
        }

       [HttpPost]
        public async Task<ActionResult<CarModels>> InsertCarDetails([FromForm] CarModels car, List<IFormFile> images)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var imagePaths = new List<string>();
                foreach (var file in images)
                {
                    if (file.Length > 5 * 1024 * 1024) // 5 MB
                    {
                        return BadRequest("Image size must not exceed 5 MB.");
                    }

                    var filePath = Path.Combine("uploads", file.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    imagePaths.Add(filePath);
                }

                car.Images = imagePaths;
                var carId = await _carModelBusinessLayer.AddCarAsync(car);
                return new OkObjectResult(carId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating commission report: {ex.Message}");

                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<CarModels>>> Search(string modelName=null, string modelCode=null)
        {
            try
            {
                var vehicles = await _carModelBusinessLayer.SearchCarAsync(modelName, modelCode);
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating commission report: {ex.Message}");

                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<CarModels>>> GetAllCarListAsync()
        {
            try
            {
                var cars = await _carModelBusinessLayer.GetAllCarListAsync();

                if (cars == null || !cars.Any())
                {
                    return NotFound("No cars found.");
                }

                return Ok(cars);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error generating commission report: {ex.Message}");

                return StatusCode(500, "Internal server error. Please try again later.");
            }
        }

    }

}
