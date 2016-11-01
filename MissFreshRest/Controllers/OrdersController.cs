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
        [Route("Orders")]
        public ReturnJasonConstruct<IList<DTO.Order>> Get()
        {
            return Services.Orders.GetAllOrders();
        }

        [HttpGet]
        [Route("Orders/{id}")]
        public ReturnJasonConstruct<DTO.Order> Get(Guid id)
        {
            return Services.Orders.GetEntireOrderInformation(id);
        }

        [HttpPost]
        [Route("Orders/")]
        public ReturnJasonConstruct<DTO.Order> Post([FromBody]DTO.Order order)
        {
            return Services.Orders.Create(order);
        }

        [HttpPut]
        [Route("Orders/updateStatus")]
        public ReturnJasonConstruct<DTO.Order> Put([FromBody]Guid id)
        {
            return Services.Orders.UpdateOrderStatus(id);
        }

        [HttpPut]
        [Route("Orders/close")]
        public ReturnJasonConstruct<DTO.Order> close([FromBody]Guid id)
        {
            return Services.Orders.CloseOrder(id);
        }
    }
}
