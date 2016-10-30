using DTO;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;

namespace Services
{
    public class Orders
    {
        enum orderStatus
        {
            Created = 0,    //创建
            payed = 1,      //已支付
            deal = 2,       //正在拣货
            trasfer = 3,    //正在派送
            complete = 4,   //已经完成
            close = 5       //关闭
        }
        public ReturnJasonConstruct<IList<DTO.Order>> GetAllOrders()
        {
            ReturnJasonConstruct<IList<DTO.Order>> list = new ReturnJasonConstruct<IList<DTO.Order>>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var orderList = db.Orders.ToList();
                list.SetDTOObject(orderList.ToDTOs());
                return list;
            }
            catch (Exception ex)
            {
                list.SetFailedInformation(ex.Message, null);
                return list;
            }
        }

        public ReturnJasonConstruct<DTO.Order> Create(DTO.Order order)
        {
            ReturnJasonConstruct<DTO.Order> DTOObject = new ReturnJasonConstruct<DTO.Order>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var model = order.ToModel();
                db.Orders.Add(model);
                db.SaveChanges();
                DTOObject.SetDTOObject(model.ToDTO());
                return DTOObject;
            }
            catch (Exception ex)
            {
                DTOObject.SetFailedInformation(ex.Message, null);
                return DTOObject;
            }
        }

        public ReturnJasonConstruct<DTO.Order> UpdateOrderStatus(Guid id)
        {
            ReturnJasonConstruct<DTO.Order> DTOObject = new ReturnJasonConstruct<DTO.Order>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var order = db.Orders.SingleOrDefault(p => p.id == id);
                if (order.orderState == (int)orderStatus.Created)
                {
                    DTOObject.SetWarningInformation("当前订单没有支付，无法改变状态.");
                    return DTOObject;
                }
                if (order.orderState == (int)orderStatus.complete || order.orderState == (int)orderStatus.close)
                {
                    DTOObject.SetWarningInformation("当前订单为结束或者关闭状态，无法改变状态.");
                    return DTOObject;
                }
                order.orderState = order.orderState++;
                db.SaveChanges();
                DTOObject.SetDTOObject(order.ToDTO());
                return DTOObject;
            }
            catch (Exception ex)
            {
                DTOObject.SetFailedInformation(ex.Message, null);
                return DTOObject;
            }
        }

        public ReturnJasonConstruct<DTO.Order> CloseOrder(Guid id)
        {
            ReturnJasonConstruct<DTO.Order> DTOObject = new ReturnJasonConstruct<DTO.Order>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var order = db.Orders.SingleOrDefault(p => p.id == id);
                if (order.orderState != (int)orderStatus.Created)
                {
                    DTOObject.SetWarningInformation("只有创建后未支付的订单可以关闭.");
                    return DTOObject;
                }
                order.orderState = (int)orderStatus.close;
                db.SaveChanges();
                DTOObject.SetDTOObject(order.ToDTO());
                return DTOObject;
            }
            catch (Exception ex)
            {
                DTOObject.SetFailedInformation(ex.Message, null);
                return DTOObject;
            }
        }
    }
}
