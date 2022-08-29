using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models.ClientData
{
    public class ClientPhone
    {
        public int IdClientPhone { get; set; }

        public int ClientIdClient { get; set; }

        public string AreaCode { get; set; }

        public string PhoneNumber { get; set; }

    }
}
