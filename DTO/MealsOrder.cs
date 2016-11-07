using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class MealsOrder
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

        public List<DTO.MealsOrderDetail> orderDetail { get; set; }
    }

    public class MealsOrderDetail
    {
        public System.Guid id { get; set; }
        public System.Guid mealsOrderId { get; set; }
        public System.Guid mealsId { get; set; }
        public int count { get; set; }
        public decimal price { get; set; }
        public Nullable<byte> evaluate { get; set; }
    }
}
