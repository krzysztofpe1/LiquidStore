using StoreClient.DatabaseModels;
using System.Collections.Generic;

namespace StoceClient.DatabaseModels
{
    public class ORDER
    {
        public int? Id { get; set; }
        public string Comment { get; set; }
        public List<ORDERDETAILS> Details { get; set; }
        public bool Equals(ORDER other)
        {
            if(Id!=other.Id) return false;
            if(Comment!=other.Comment) return false;
            if(Details.Count!=other.Details.Count)return false;
            return true;
        }
        public int OrderId;
    }
}

