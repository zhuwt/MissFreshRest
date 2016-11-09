using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Address
    {
        public System.Guid id { get; set; }
        public System.Guid accountId { get; set; }
        public string Address1 { get; set; }
        public string tel { get; set; }
        public string name { get; set; }
        public Nullable<bool> defaultAddress { get; set; }
    }
}
