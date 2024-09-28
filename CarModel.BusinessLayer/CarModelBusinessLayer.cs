using CarModel.DataAccessLayer;
using CarModel.DataAccessLayer.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarModel.BusinessLayer
{
    public class CarModelBusinessLayer : ICarModelBusinessLayer
    {
        private readonly ICarModelDataAccessLayer _dataAccessLayer;
        public CarModelBusinessLayer(ICarModelDataAccessLayer dataAccessLayer)
        {
            _dataAccessLayer = dataAccessLayer;
        }
        public async Task<int> AddCarAsync(CarModels car)
        {
            try
            {
                return await _dataAccessLayer.AddCarAsync(car);
            }
            catch
            {
                throw;
            }
        }
         
        public async Task<IEnumerable<CarModels>> SearchCarAsync(string modelName, string modelCode)
        {
            try
            {
                return await _dataAccessLayer.SearchCarAsync(modelName, modelCode);
            }
            catch
            {
                throw;
            }
        }

        Task<List<CarModels>> ICarModelBusinessLayer.GetAllCarListAsync()
        {
            try
            {
                return _dataAccessLayer.GetAllCarListAsync();
            }
            catch
            {
                throw;
            }
        }
    }
}
 