namespace StoreServer.DatabaseModels
{
    internal class ORDERDETAILS
    {
        public int Id { get; set; }
        public string? Brand { get; set; }
        public string? Name { get; set; }
        public int Volume { get; set; }
        public double Concentration { get; set; }
        public int Status { get; set; }

        public ORDER Order { get; set; } = null!;
    }
}
