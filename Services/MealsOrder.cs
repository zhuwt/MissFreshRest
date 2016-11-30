using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;
using DTO;

namespace Services
{
    public class MealsOrder
    {
        public static ReturnJasonConstruct<IList<DTO.MealsOrder>> GetAllOrder()
        {
            ReturnJasonConstruct<IList<DTO.MealsOrder>> list = new ReturnJasonConstruct<IList<DTO.MealsOrder>>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var orderList = db.MealsOrders.ToList();
                list.SetDTOObject(orderList.ToDTOs());
                return list;
            }
            catch (Exception ex)
            {
                list.SetFailedInformation(ex.Message, null);
                return list;
            }
        }

        public static ReturnJasonConstruct<DTO.EntireMealsOrder> GetEntireOrderInformation(Guid id)
        {
            ReturnJasonConstruct<DTO.EntireMealsOrder> list = new ReturnJasonConstruct<DTO.EntireMealsOrder>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var order = db.MealsOrders.SingleOrDefault(p => p.id == id);
                var dto = order.ToEntireMealsOrder();
                var temp = from r in db.MealsOrderDetails.Where(p => p.mealsOrderId == id)
                           join m in db.Meals.AsQueryable<Models.Meal>()
                           on r.mealsId equals m.id
                           select m;
                foreach (var item in temp.ToList())
                {
                    dto.orderDetail.Add(item.ToDTO());
                }
                list.SetDTOObject(dto);
                return list;
            }
            catch (Exception ex)
            {
                list.SetFailedInformation(ex.Message, null);
                return list;
            }
        }

        public static ReturnJasonConstruct<DTO.MealsOrder> Create(DTO.MealsOrder order)
        {
            ReturnJasonConstruct<DTO.MealsOrder> DTOObject = new ReturnJasonConstruct<DTO.MealsOrder>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var model = order.ToModel();
                if (model.MealsOrderDetails.Count == 0)
                {
                    DTOObject.SetWarningInformation("订单信息为空.请选择需要购买的套餐.");
                    return DTOObject;
                }
                //set the first goodsimage for order image
                Guid goodsId = order.orderDetail[0].mealsId;
                var mealsObj = db.Meals.SingleOrDefault(p => p.id == goodsId);
                model.imangeName = mealsObj.imangeName;
                foreach (var item in model.MealsOrderDetails)
                {
                    var mealsInfo = db.Meals.Single(p => p.id == item.mealsId);
                    mealsInfo.stock -= item.count;
                    if (mealsInfo.stock < 0)
                    {
                        DTOObject.SetWarningInformation("很抱歉，所选菜品中库存不足.");
                        return DTOObject;
                    }
                }

                db.MealsOrders.Add(model);
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

        public static ReturnJasonConstruct<DTO.MealsOrder> UpdateOrderStatus(Guid id)
        {
            ReturnJasonConstruct<DTO.MealsOrder> DTOObject = new ReturnJasonConstruct<DTO.MealsOrder>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var order = db.MealsOrders.SingleOrDefault(p => p.id == id);
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

        public static ReturnJasonConstruct<DTO.MealsOrder> CloseOrder(Guid id)
        {
            ReturnJasonConstruct<DTO.MealsOrder> DTOObject = new ReturnJasonConstruct<DTO.MealsOrder>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var order = db.MealsOrders.SingleOrDefault(p => p.id == id);
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
