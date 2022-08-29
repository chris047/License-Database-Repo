using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class OwnerFile
    {
        public int idowner { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string socsec { get; set; }
        public int idowner_address { get; set; }
        public string address_line1_number { get; set; }
        public string address_line1_street_name { get; set; }
        public string address_line2 { get; set; }
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
        public int idowner_phone { get; set; }
        public string area_code { get; set; }
        public string phone_number { get; set; }
        public int idowner_position { get; set; }
        public string title { get; set; }
        public int idowner_timeline { get; set; }
        public string stock { get; set; }
        public string from_date { get; set; }
        public string to_date { get; set; }
        public int idowner_blind { get; set; }
        public string blind_text { get; set; }
    }
}
