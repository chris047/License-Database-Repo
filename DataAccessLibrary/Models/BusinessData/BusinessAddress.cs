using DataAccessLibrary.Models.SharedData;

namespace DataAccessLibrary.Models.BusinessData
{
    public class BusinessAddress
    {
        public int idbusiness_address { get; set; }
        public int business_idbusiness { get; set; }
        public string address_line1_number { get; set; }
        public string address_line1_street_name { get; set; }
        public string address_line2 { get; set; }
        public City city { get; set; }
        public County county { get; set; }
        public Zipcode zipcode { get; set; }
        public State state { get; set; }

    }
}
