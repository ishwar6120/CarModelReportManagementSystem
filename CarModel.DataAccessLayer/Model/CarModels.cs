using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static CarModel.DataAccessLayer.Model.EnumsConstant.enumConstant;
namespace CarModel.DataAccessLayer.Model
{
    public class CarModels
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Brand is required.")]
        public Brand Brand { get; set; }

        [Required(ErrorMessage = "Class is required.")]
        public CarClass Class { get; set; }

        [Required(ErrorMessage = "Model Name is required.")]
        public string ModelName { get; set; }

        [Required(ErrorMessage = "Model Code is required.")]
        [StringLength(10, ErrorMessage = "Model Code must be 10 characters long.")]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "Model Code must be alphanumeric.")]
        public string ModelCode { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Features are required.")]
        public string Features { get; set; }

        [Required(ErrorMessage = "Price is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Date of Manufacturing is required.")]
        public DateTime DateOfManufacturing { get; set; }

        public bool Active { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Sort Order must be a positive number.")]
        public int SortOrder { get; set; }

        [Required(ErrorMessage = "Images are required.")]
        public List<string> Images { get; set; }
    }
}
