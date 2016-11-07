using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class EntireMealsOrder
    {
        public System.Guid id { get; set; }
        public long orderNo { get; set; }
        public int orderState { get; set; }
        public decimal totalPrice { get; set; }
        public int totalCount { get; set; }
        public System.Guid accountId { get; set; }
        public string receiveAddress { get; set; }
        public string tel { get; set; }
        public string receivePerson { get; set; }
        public string imangeName { get; set; }
        public System.DateTime createTime { get; set; }

        public List<DTO.Meals> orderDetail { get; set; }
    }
}
