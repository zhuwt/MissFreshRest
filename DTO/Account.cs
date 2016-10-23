using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Account
    {
        public System.Guid id { get; set; }
        public System.Guid userId { get; set; }
        public string code { get; set; }
        public Nullable<System.DateTime> codeTime { get; set; }
        public DTO.Customer customer;
    }
}
