using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Models.BusinessData;
using DataAccessLibrary.Models.ClientData;
using DataAccessLibrary.Models.OwnerData;

namespace DataAccessLibrary.Models
{
    public class BusinessFile
    {
        public int idbusiness { get; set; }
        public string dba { get; set; }
        public List<BusinessFormerly> business_formerlies { get; set; } //= new List<BusinessFormerly>();
        public int idbusiness_address { get; set; }
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
        public int idbusiness_phone { get; set; }
        public string area_code { get; set; }
        public string phone_number { get; set; }
        public int idbusiness_license { get; set; }
        public string license_number { get; set; }
        public string establishment { get; set; }
        public string entity { get; set; }
        public bool? active { get; set; } // This must be explicilty nullable to prevent issue w/ this being passed into mysql as a 0 by default
        public List<BusinessActivityCode> business_activity_codes { get; set; } //= new List<BusinessActivityCode>();
        public string activity_date { get; set; }
        public int idbusiness_interaction { get; set; }
        public int user_iduser { get; set; }
        public string worked_date { get; set; }
        public string completed_date { get; set; }
        public List<Client> clients { get; set; } //= new List<Client>();
        public int idbusiness_memo { get; set; }
        public string memo_text { get; set; }

    }
}
