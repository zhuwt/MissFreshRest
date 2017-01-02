using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;
using DTO;
using System.Data.SqlClient;
using System.Data;

namespace Services
{
    public class MealsOrder
    {
        public static ReturnJasonConstruct<IList<DTO.MealsOrder>> GetAllOrder(Guid id)
        {
            ReturnJasonConstruct<IList<DTO.MealsOrder>> list = new ReturnJasonConstruct<IList<DTO.MealsOrder>>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                string str = @"select mo.id as orderId,mo.orderNo as orderNo,mo.orderState as orderState,
                                mo.totalPrice as totalPrice,mo.totalCount as totalCount,mo.receiveAddress as receiveAddress,
                                mo.tel as tel,mo.receivePerson as receivePerson,m.name as mealsName,g.id as goodsId,g.name as name,
                                g.detailName as detailName,g.evaluate as evaluate,g.imageName as imageName 
                                from MealsOrders mo,MealsOrderDetails mod,Meals m,MealsDetails md,Goods g 
                                where mo.id=mod.mealsOrderId and mod.mealsId=m.id and m.id=md.mealsId 
                                                            and md.goodsId=g.id and accountId = '{0}'";
                str = string.Format(str, id);
                DataTable dt = ExecuteSql(db.Database.Connection.ConnectionString, str);

                Guid orderId = default(Guid);
                DTO.MealsOrder obj = null;
                List<DTO.MealsOrder> ls = new List<DTO.MealsOrder>();
                foreach (DataRow dr in dt.Rows)
                {
                    if (orderId != (Guid)dr["orderId"])
                    {
                        orderId = (Guid)dr["orderId"];
                        obj = new DTO.MealsOrder();
                        ls.Add(obj);
                        obj.id = (Guid)dr["orderId"];
                        obj.orderNo = (long)dr["orderNo"];
                        obj.orderState = (int)dr["orderState"];
                        obj.totalPrice = (decimal)dr["totalPrice"];
                        obj.totalCount = (int)dr["totalCount"];
                        obj.mealsName = dr["mealsName"].ToString();
                        obj.receiveAddress = dr["receiveAddress"].ToString();
                        obj.tel = dr["tel"].ToString();
                        obj.receivePerson = dr["receivePerson"].ToString();
                        obj.orderDetail = new List<DTO.MealsOrderDetail>();
                    }
                    var subObj = new DTO.MealsOrderDetail();
                    obj.orderDetail.Add(subObj);
                    subObj.id = (Guid)dr["goodsId"];
                    subObj.name = dr["name"].ToString();
                    subObj.detailName = dr["detailName"].ToString();
                    subObj.evaluate = (byte)dr["evaluate"];
                    subObj.imageName = dr["imageName"].ToString();
                }
                list.SetDTOObject(ls);
                return list;
            }
            catch (Exception ex)
            {
                list.SetFailedInformation(ex.Message, null);
                return list;
            }
        }

        public static ReturnJasonConstruct<DTO.EntireMealsOrder> GetEntireOrderInformation(Guid id, Guid accId)
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
                           select r;
                foreach (var item in temp.ToList())
                {
                    var subdto = item.ToDTO();
                    subdto.name = item.Meal.name;
                    dto.orderDetail.Add(subdto);
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
                
                //Guid goodsId = order.orderDetail[0].mealsId;
                var mealsObj = db.Meals.SingleOrDefault(p => p.name == order.mealsName);
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
                model.id = Guid.NewGuid();
                model.createTime = DateTime.Now;
                //order.createTime = DateTime.Now;
                order.id = Guid.NewGuid();
                var temp = string.Format("{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}{6:000}", model.createTime.Year, model.createTime.Month, model.createTime.Day, model.createTime.Hour, model.createTime.Minute, model.createTime.Second, model.createTime.Millisecond);////, , , , 
                order.orderNo = long.Parse(temp);

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

        private static DataTable ExecuteSql(string connect, string sqlSentence)
        {
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = connect;
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sqlSentence;

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable table = new DataTable();
            adapter.Fill(table);

            conn.Close();//连接需要关闭  
            conn.Dispose();
            return table;
        }
    }
}
