using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Goods
    {
        public System.Guid id { get; set; }
        public string name { get; set; }
        public string detailName { get; set; }
        public string unit { get; set; }
        public string category { get; set; }
        public decimal price { get; set; }
        public Nullable<int> sellCount { get; set; }
        public Nullable<int> limited { get; set; }
        public string imageName { get; set; }
        public Nullable<int> goodsStatus { get; set; }
        public Nullable<byte> evaluate { get; set; }
        public GoodsDetail goodsDetail { get; set; }
    }
}
