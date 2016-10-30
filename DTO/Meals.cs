using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Meals
    {
        public System.Guid id { get; set; }
        public string name { get; set; }
        public string imangeName { get; set; }
        public decimal totalPrice { get; set; }
        public Nullable<byte> evaluate { get; set; }
        public IEnumerable<DTO.Goods> goodsList { get; set; }
    }
}
