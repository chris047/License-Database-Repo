using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary.Models.BusinessData;
using DataAccessLibrary.Models.OwnerData;

namespace DataAccessLibrary.Models
{

    // Owner, Address, Streetname, Zip Code, DBA, License
    public class PreviewGeneralFile
    {
        public int IdBusiness { get; set; }
        public int IdOwner { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AddressLine1Number { get; set; }
        public string AddressLine1StreetName { get; set; }
        public string ZipCode { get; set; }
        public string Dba { get; set; }
        public string LicenseNumber { get; set; }
    }
}
