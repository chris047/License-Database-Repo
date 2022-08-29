using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class ClientFile
    {
        public int idclient { get; set; }
        public int client_idclient { get; set; }
        public string business_name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public int idclient_address { get; set; }
        public string address_line1_number { get; set; }
        public string address_line1_street_name { get; set; }
        public string address_line2 { get; set; }
        public int idclient_phone { get; set; }
        public string area_code { get; set; }
        public string phone_number { get; set; }
        public int idcity { get; set; }
        public int county_idcounty { get; set; }
        public string city_name { get; set; }
        public string city_code { get; set; }
        public int idcounty { get; set; }
        public int state_idstate { get; set; }
        public string county_name { get; set; }
        public string county_region { get; set; }
        public int idstate { get; set; }
        public string state_code { get; set; }
        public int idzipcode { get; set; }
        public string zip { get; set; }

    }
}
