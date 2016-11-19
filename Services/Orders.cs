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
    public enum orderType
    {
        goods = 0,      //create
        meals = 1       //payed
    }

    public enum orderStatus
    {
        Created = 0,    //create
        payed = 1,      //payed
        deal = 2,       //clean up
        trasfer = 3,    //sending
        complete = 4,   //complete
        close = 5       //close
    }

    public class Orders
    {
        public static ReturnJasonConstruct<IList<DTO.Order>> GetAllOrders(Guid id)
        {
            ReturnJasonConstruct<IList<DTO.Order>> list = new ReturnJasonConstruct<IList<DTO.Order>>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var orderList = db.Orders.Where(p=>p.accountId == id).ToList();
                list.SetDTOObject(orderList.ToDTOs());
                return list;
            }
            catch (Exception ex)
            {
                list.SetFailedInformation(ex.Message, null);
                return list;
            }
        }

        public static ReturnJasonConstruct<DTO.Order> GetEntireOrderInformation(Guid id)
        {
            ReturnJasonConstruct<DTO.Order> list = new ReturnJasonConstruct<DTO.Order>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var order = db.Orders.SingleOrDefault(p => p.id == id);
                list.SetDTOObject(order.ToDTO());
                return list;
            }
            catch (Exception ex)
            {
                list.SetFailedInformation(ex.Message, null);
                return list;
            }
        }

        public static ReturnJasonConstruct<DTO.Order> Create(DTO.Order order)
        {
            ReturnJasonConstruct<DTO.Order> DTOObject = new ReturnJasonConstruct<DTO.Order>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var model = order.ToModel();
                if (order.orderDetailList.Count == 0)
                {
                    DTOObject.SetWarningInformation("订单信息为空.请选择需要购买的菜品.");
                    return DTOObject;
                }

                //set the first goodsimage for order image
                Guid goodsId = order.orderDetailList[0].goodsId;
                var goods = db.Goods.SingleOrDefault(p => p.id == goodsId);
                model.imangeName = goods.imageName;
                foreach (var item in order.orderDetailList)
                {
                    var goodsInfo = db.Goods.Single(p => p.id == item.goodsId);
                    goodsInfo.stock -= item.count;
                    if (goodsInfo.stock < 0)
                    {
                        DTOObject.SetWarningInformation("很抱歉，所选菜品中库存不足.");
                        return DTOObject;
                    }
                }
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

        public static ReturnJasonConstruct<DTO.Order> UpdateOrderStatus(Guid id)
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
                order.orderState += 1;
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

        public static ReturnJasonConstruct<DTO.Order> CloseOrder(Guid id)
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
