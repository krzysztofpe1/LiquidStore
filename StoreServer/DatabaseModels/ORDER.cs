using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreServer.DatabaseModels
{
    internal class ORDER
    {
        public int ID { get; set; }
        [Required]
        [StringLength(maximumLength: 100)]
        public string COMMENT { get; set; }
    }
}
