using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Customer
    {
        public System.Guid id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string telNo { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public System.DateTime createTime { get; set; }
    }
}
