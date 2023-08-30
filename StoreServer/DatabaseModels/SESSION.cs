namespace StoreServer.DatabaseModels
{
    public class SESSION
    {
        public int? Id { get; set; }
        public string SessionToken { get; set; }
        public string AccessKey { get; set; }
        public USER User { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
    }
}
