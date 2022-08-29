using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models.BusinessData
{
    public class BusinessHasOwner
    {
        public int business_idbusiness { get; set; }
        public int owner_idowner { get; set; }
    }
}
