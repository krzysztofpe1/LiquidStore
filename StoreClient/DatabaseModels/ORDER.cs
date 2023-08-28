using StoreClient.DatabaseModels;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Windows.Controls;
using System.Windows.Data;

namespace StoceClient.DatabaseModels
{
    public class ORDER
    {
        [Required]
        public int? Id { get; set; }
        [StringLength(maximumLength: 100)]
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

