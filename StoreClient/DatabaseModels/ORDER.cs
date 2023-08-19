using StoreClient.DatabaseModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoceClient.DatabaseModels
{
    public class ORDER
    {
        [Required]
        public int? Id { get; set; }
        [StringLength(maximumLength: 100)]
        public string Comment { get; set; }

        public IEnumerable<ORDERDETAILS> Details { get; set; }
    }
}
