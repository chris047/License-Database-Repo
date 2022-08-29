namespace DataAccessLibrary.Models.OwnerData
{
    public class OwnerPhone
    {
        public int idowner_phone { get; set; }
        public int owner_idowner { get; set; }
        public string area_code { get; set; }
        public string phone_number { get; set; }
    }
}
