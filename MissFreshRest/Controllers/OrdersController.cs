using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace MissFreshRest.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class OrdersController : ApiController
    {
        [HttpGet]
        [Route("ordersList/{accountId}")]
        public ReturnJasonConstruct<IList<DTO.Order>> GetOrderList(Guid accountId)
        {
            return Services.Orders.GetAllOrders(accountId);
        }

        [HttpGet]
        [Route("orders/{id}")]
        public ReturnJasonConstruct<DTO.Order> GetEntireOrder(Guid id)
        {
            return Services.Orders.GetEntireOrderInformation(id);
        }

        [HttpPost]
        [Route("orders/")]
        public ReturnJasonConstruct<DTO.Order> Post([FromBody]DTO.Order order)
        {
            order.createTime = DateTime.Now;
            order.id = Guid.NewGuid();
            var temp = string.Format("{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}{6:000}", order.createTime.Year, order.createTime.Month, order.createTime.Day, order.createTime.Hour, order.createTime.Minute, order.createTime.Second, order.createTime.Millisecond);////, , , , 
            order.orderNo = long.Parse(temp);
            foreach(var item in order.orderDetailList)
            {
                item.id = Guid.NewGuid();
            }

            return Services.Orders.Create(order);
        }

        [HttpPut]
        [Route("orders/updateStatus")]
        public ReturnJasonConstruct<DTO.Order> Put(Guid id)
        {
            return Services.Orders.UpdateOrderStatus(id);
        }

        [HttpPut]
        [Route("orders/close")]
        public ReturnJasonConstruct<DTO.Order> close(Guid id)
        {
            return Services.Orders.CloseOrder(id);
        }
    }
}
