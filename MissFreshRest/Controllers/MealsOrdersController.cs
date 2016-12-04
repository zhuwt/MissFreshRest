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
    public class MealsOrdersController : ApiController
    {
        [HttpGet]
        [Route("mealsOrderList/{id}")]
        public ReturnJasonConstruct<IList<DTO.MealsOrder>> GetMealList(Guid id)
        {
            return Services.MealsOrder.GetAllOrder(id);
        }

        [HttpGet]
        [Route("mealsOrders/{id}/{accId}")]
        public ReturnJasonConstruct<DTO.EntireMealsOrder> Get(Guid id,Guid accId)
        {
            return Services.MealsOrder.GetEntireOrderInformation(id, accId);
        }

        [HttpPost]
        [Route("mealsOrders/")]
        public ReturnJasonConstruct<DTO.MealsOrder> Post([FromBody]DTO.MealsOrder order)
        {
            order.createTime = DateTime.Now;
            order.id = Guid.NewGuid();
            var temp = string.Format("{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}{6:000}", order.createTime.Year, order.createTime.Month, order.createTime.Day, order.createTime.Hour, order.createTime.Minute, order.createTime.Second, order.createTime.Millisecond);////, , , , 
            order.orderNo = long.Parse(temp);
            foreach (var item in order.orderDetail)
            {
                item.id = Guid.NewGuid();
            }

            return Services.MealsOrder.Create(order);
        }

        [HttpPut]
        [Route("mealsOrders/updateStatus")]
        public ReturnJasonConstruct<DTO.MealsOrder> Put(Guid id)
        {
            return Services.MealsOrder.UpdateOrderStatus(id);
        }

        [HttpPut]
        [Route("mealsOrders/close")]
        public ReturnJasonConstruct<DTO.MealsOrder> close(Guid id)
        {
            return Services.MealsOrder.CloseOrder(id);
        }
    }
}
