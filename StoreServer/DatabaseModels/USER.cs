using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreServer.DatabaseModels
{
    public class USER
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        [Required]
        public int? Privileges { get; set; }
        public string Password { get; set; }
    }
}
