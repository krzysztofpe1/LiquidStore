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
        public bool Equals(STORAGE item)
        {
            if(Id!=item.Id) return false;
            if(Brand!=item.Brand) return false;
            if(Name!=item.Name) return false;
            if(Volume!=item.Volume) return false;
            if(Cost!=item.Cost) return false;
            if(Remaining!=item.Remaining) return false;
            return true;
        }
    }
}
