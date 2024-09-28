using CarModel.DataAccessLayer.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarModel.BusinessLayer
{
    public interface ICarModelBusinessLayer
    {
        Task<int> AddCarAsync(CarModels car);
        Task<IEnumerable<CarModels>> SearchCarAsync(string modelName, string modelCode);

        Task<List<CarModels>> GetAllCarListAsync();
    }
}
