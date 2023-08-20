using StoreClient.DatabaseModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    }
}

