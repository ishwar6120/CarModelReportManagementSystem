using CarModel.DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarModel.BusinessLayer
{
    public interface ICommissionBusinessLayer
    { 
        Task<CommissionReport> GenerateCommissionReportAsync(CommissionRequest request);
      //  Task<decimal> GetCarPriceAsync(string brand, string carClass);
    }
}
