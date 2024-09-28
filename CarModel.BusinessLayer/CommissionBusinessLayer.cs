using CarModel.DataAccessLayer;
using CarModel.DataAccessLayer.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CarModel.BusinessLayer
{
    public class CommissionBusinessLayer : ICommissionBusinessLayer
    {
       
    private readonly ICommissionDataAccessLayer _commissionDataAccessLayer;

        public CommissionBusinessLayer(ICommissionDataAccessLayer commissionDataAccessLayer)
        {
            _commissionDataAccessLayer = commissionDataAccessLayer;
        }
        public async Task<CommissionReport> GenerateCommissionReportAsync(CommissionRequest request)
        {
            try
            {
                // Validate the input request
                if (request == null || request.SalesData == null || string.IsNullOrEmpty(request.Salesman))
                {
                    return null;
                }

                // Retrieve previous year's sales for the salesman
                var previousYearSales = await _commissionDataAccessLayer.GetPreviousYearSalesAsync(request.Salesman);
                if (previousYearSales == null)
                {
                    return null;
                }

                decimal totalCommission = 0;
                var brandCommissionDetails = new List<BrandCommissionDetail>();

                // Loop through each brand in the sales data
                foreach (var brand in request.SalesData.Keys)
                {
                    var salesData = request.SalesData[brand];
                    var aclass = salesData.Aclass;
                    var bclass = salesData.Bclass;
                    var cclass = salesData.Cclass;

                    // Get the commission rates and price threshold for the brand
                    var (fixedCommission, classACommission, classBCommission, classCCommission, priceThreshold) = GetCommissionRates(brand);

                    decimal brandCommission = 0;

                    // Fetch car prices from the database
                    var aclassPrice = await _commissionDataAccessLayer.GetCarPriceAsync(brand, "Aclass");
                    var bclassPrice = await _commissionDataAccessLayer.GetCarPriceAsync(brand, "Bclass");
                    var cclassPrice = await _commissionDataAccessLayer.GetCarPriceAsync(brand, "Cclass");

                    // Add fixed commission for Class A if price threshold is met
                    if (aclassPrice > priceThreshold)
                    {
                        brandCommission += aclass * fixedCommission;
                    }

                    // Calculate additional commissions based on class and model price
                    brandCommission += aclass * classACommission * aclassPrice;
                    brandCommission += bclass * classBCommission * bclassPrice;
                    brandCommission += cclass * classCCommission * cclassPrice;

                    // Additional 2% commission for sales over $500,000 last year, applicable only for Class A cars
                    if (previousYearSales > 500000)
                    {
                        brandCommission += aclass * (0.02m * aclassPrice);
                    }

                    // Add the brand commission to the total commission
                    totalCommission += brandCommission;

                    // Add the brand commission details to the list
                    brandCommissionDetails.Add(new BrandCommissionDetail
                    {
                        Brand = brand,
                        AclassSold = aclass,
                        BclassSold = bclass,
                        CclassSold = cclass,
                        Commission = brandCommission
                    });
                }

                // Return the commission report for the salesman
                return new CommissionReport
                {
                    Salesman = request.Salesman,
                    TotalCommission = totalCommission,
                    BrandCommissionDetails = brandCommissionDetails
                };
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while generating the commission report.", ex);
            }
        }



        private (decimal fixedCommission, decimal classA, decimal classB, decimal classC, decimal priceThreshold) GetCommissionRates(string brand)
        {
            // Define a dictionary to store commission rates and price thresholds for each brand
            var brandCommissionRates = new Dictionary<string, (decimal fixedCommission, decimal classA, decimal classB, decimal classC, decimal priceThreshold)>
             {
                // Each entry contains the fixed commission, class A, B, C commission rates, and the price threshold for that brand
                { "Audi", (800, 0.08m, 0.06m, 0.04m, 25000) },
                  { "Jaguar", (750, 0.06m, 0.05m, 0.03m, 35000) },
                 { "Land Rover", (850, 0.07m, 0.05m, 0.04m, 30000) },
                 { "Renault", (400, 0.05m, 0.03m, 0.02m, 20000) }
             };

            // Check if the brand exists in the dictionary and return the corresponding tuple
            // If the brand doesn't exist, return a tuple with default values (0, 0, 0, 0, 0)
            return brandCommissionRates.ContainsKey(brand) ? brandCommissionRates[brand] : (0, 0, 0, 0, 0);
        }


    }
}
