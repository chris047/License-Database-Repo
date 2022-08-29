using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class GeneralFile
    {
        public int idbusiness { get; set; }
        public List<int> owner_idowner { get; set; }
        public List<int> client_idclient { get; set; }
    }
}
