using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models.BusinessData
{
    public class BusinessInteractionHasClient
    {
        public int business_interaction_idbusiness_interaction { get; set; }
        public int client_idclient { get; set; }
    }
}   
