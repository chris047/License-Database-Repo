namespace DataAccessLibrary.Models.BusinessData
{
    public class BusinessLicense
    {
        public int idbusiness_license { get; set; }
        public int business_idbusiness { get; set; }
        public string license_number { get; set; }
        public string establishment { get; set; }
        public string entity { get; set; }
        public bool active { get; set; }
        public List<BusinessActivityCode> business_activity_codes { get; set; }
        public string activity_date { get; set; }
    }
}
