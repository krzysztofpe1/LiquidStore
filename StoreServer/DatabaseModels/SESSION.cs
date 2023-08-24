using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreServer.DatabaseModels
{
    public class SESSION
    {
        public int Id { get; set; }
        public string SessionToken { get; set; }
        public string AccessKey { get; set; }
        public USER User { get; set; }
        [Required]
        public DateTime ExpirationDate { get; set; }
    }
}
