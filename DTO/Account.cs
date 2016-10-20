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
        public Nullable<System.Guid> UserId { get; set; }
        public DTO.Customer customer;
    }
}
