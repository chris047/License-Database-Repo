using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models.SharedData
{
    public class County
    {
        public int idcounty { get; set; }
        public int state_idstate { get; set; }
        public string county_name { get; set; }
        public string county_region { get; set; }
    }
}
