using System;
using System.Collections.Generic;
using System.Text;

namespace CarModel.DataAccessLayer.Model
{
    public class CommissionRequest
    {
        public string Salesman { get; set; }
        public Dictionary<string, SalesClass> SalesData { get; set; }
    }

    public class SalesClass
    {
        public int Aclass { get; set; }
        public int Bclass { get; set; }
        public int Cclass { get; set; }
    }
}
