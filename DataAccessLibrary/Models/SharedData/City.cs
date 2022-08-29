using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models.SharedData
{
    public class City
    {
        public int idcity { get; set; }
        public int county_idcounty { get; set; }
        public string city_name { get; set; }
        public string city_code { get; set; }
    }
}
