using DataAccessLibrary.Models.ClientData;

namespace DataAccessLibrary.Models.BusinessData
{
    public class BusinessInteraction
    {
        public int idbusiness_interaction { get; set; }
        public int business_idbusiness { get; set; }
        public int user_iduser { get; set; }
        public string worked_date { get; set; }
        public string completed_date { get; set; }
        public List<Client> clients { get; set; }
    }
}
