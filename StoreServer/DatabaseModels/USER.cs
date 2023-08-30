namespace StoreServer.DatabaseModels
{
    public class USER
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        [Required]
        public int? Privileges { get; set; }
        [StringLength(maximumLength: 72)]
        public string Password { get; set; }
    }
}
