using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarModel.DataAccessLayer
{
    public interface ICommissionDataAccessLayer
    {
        Task<decimal?> GetPreviousYearSalesAsync(string salesman);
        Task<decimal> GetCarPriceAsync(string brand, string carClass);
    }
}
