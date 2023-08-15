namespace StoreServer.DatabaseModels
{
    internal class ORDER
    {
        public int Id { get; set; }
        [StringLength(maximumLength: 100)]
        public string Comment { get; set; } = null!;

        public IEnumerable<ORDERDETAILS> Details { get; set; } = null!;
    }
}
