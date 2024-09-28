using System;
using System.Collections.Generic;
using System.Text;

namespace CarModel.DataAccessLayer.Model
{
    public class CommissionReport
    {
        public string Salesman { get; set; }
        public decimal TotalCommission { get; set; }
        public List<BrandCommissionDetail> BrandCommissionDetails { get; set; }
    }

    public class BrandCommissionDetail
    {
        public string Brand { get; set; }
        public int AclassSold { get; set; }
        public int BclassSold { get; set; }
        public int CclassSold { get; set; }
        public decimal Commission { get; set; }
    }

}
