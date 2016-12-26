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
            order.id = Guid.NewGuid();
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
