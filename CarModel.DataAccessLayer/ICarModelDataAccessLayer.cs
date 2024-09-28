using CarModel.DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarModel.DataAccessLayer
{
    public interface ICarModelDataAccessLayer
    {
        Task<int> AddCarAsync(CarModels car);
        Task<List<CarModels>> SearchCarAsync(string modelName, string modelCode);

        Task<List<CarModels>> GetAllCarListAsync();
    }
}
