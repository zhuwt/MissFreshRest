using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class AddressConfig
    {
        public System.Guid id { get; set; }
        public System.Guid parentId { get; set; }
        public string Content { get; set; }
        public int layer { get; set; }
    }
}
