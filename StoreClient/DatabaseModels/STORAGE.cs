using System.ComponentModel.DataAnnotations;

namespace StoreClient.DatabaseModels
{
    public class STORAGE
    {
        public int? Id { get; set; }
        public string Brand { get; set; }
        public string Name { get; set; }
        public int Volume { get; set; }
        public double Cost { get; set; }
        public int Remaining { get; set; }
    }
}
