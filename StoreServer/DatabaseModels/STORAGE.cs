using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreServer.DatabaseModels
{
    internal class STORAGE
    {
        public int ID { get; set; }
        public string? BRAND { get; set; }
        public string? NAME { get; set; }
        [Required]
        public int VOLUME { get; set; }
        [Required]
        public int COST { get; set; }
        [Required]
        public int REMAINING { get; set; }
    }
}
