namespace DataAccessLibrary.Models.OwnerData
{
    public class OwnerTimeline
    {
        public int idowner_timeline { get; set; }
        public int owner_idowner { get; set; }
        public string stock { get; set; }
        public string from_date { get; set; }
        public string to_date { get; set; }
    }
}
