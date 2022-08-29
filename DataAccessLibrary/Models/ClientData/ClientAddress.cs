using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models.ClientData
{
    public class ClientAddress
    {
        public int IdClientAddress { get; set; }

        public int ClientIdClient { get; set; }

        public string AddressLine1Number { get; set; }

        public string AddressLine1StreetName { get; set; }

        public string AddressLine2 { get; set; }

        public int CityCodeIdCityCode { get; set; }

        public int CityCodeCityIdCity { get; set; }

        public int Zipcode_IdZipcode { get; set; }
    }
}
