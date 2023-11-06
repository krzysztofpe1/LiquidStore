namespace StoreServer.DatabaseModels
{
    public class ORDERDETAILS
    {
        public int? Id { get; set; }
        public string? Brand { get; set; }
        public string? Name { get; set; }
        public int Volume { get; set; }
        public double Concentration { get; set; }
        public int? Status { get; set; }
        public int OrderId { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public DateTime? SettledDate { get; set; }
    }
}
