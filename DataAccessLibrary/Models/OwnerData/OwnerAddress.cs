using DataAccessLibrary.Models.SharedData;

namespace DataAccessLibrary.Models.OwnerData
{
    public class OwnerAddress
    {
        public int idowner_address { get; set; }
        public int owner_idowner { get; set; }
        public string address_line1_number { get; set; }
        public string address_line1_street_name { get; set; }
        public string address_line2 { get; set; }
        public City city { get; set; }
        public County county { get; set; }
        public State state { get; set; }
        public Zipcode zipcode { get; set; }
    }
}
