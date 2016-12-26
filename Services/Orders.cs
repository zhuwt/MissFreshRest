using DTO;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace Services
{
    //public enum orderType
    //{
    //    goods = 0,      //create
    //    meals = 1       //payed
    //}

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
                string str = @"select o.id as orderId,o.orderNo as orderNo,o.orderState as orderState,
                                o.totalPrice as totalPrice,o.totalCount as totalCount,
                                o.receiveAddress as receiveAddress,o.tel as tel,o.receivePerson as receivePerson,
                                g.id as goodsId,g.name as name,g.imageName as imageName,od.price as price,
                                od.count as count,od.evaluate as evaluate
                                from Orders o,OrderDetails od,Goods g where o.id = od.orderId and od.goodsId = g.id 
                                and o.accountId='{0}'";
                str = string.Format(str, id);
                DataTable dt = ExecuteSql(db.Database.Connection.ConnectionString, str);
                List<DTO.Order> ls = new List<DTO.Order>();
                Guid orderId = new Guid();
                DTO.Order obj = null;
                //for (int n = 0; n < dt.Rows.Count; n++)
                //{
                //    var item = dt.Rows[n];
                //    if (orderId != (Guid)item["orderId"])
                //    {
                //        obj = new DTO.Order();
                //        ls.Add(obj);

                //    }
                //}
                foreach (DataRow item in dt.Rows)
                {
                    if (orderId != (Guid)item["orderId"])
                    {
                        orderId = (Guid)item["orderId"];
                        obj = new DTO.Order();
                        ls.Add(obj);
                        obj.id = (Guid)item["orderId"];
                        obj.orderNo = (long)item["orderNo"];
                        obj.orderState = (int)item["orderState"];
                        obj.totalPrice = (decimal)item["totalPrice"];
                        obj.totalCount = (int)item["totalCount"];
                        obj.receiveAddress = item["receiveAddress"].ToString();
                        obj.tel = item["tel"].ToString();
                        obj.receivePerson = item["receivePerson"].ToString();
                        obj.orderDetailList = new List<DTO.OrderDetail>();
                    }
                    var subObj = new DTO.OrderDetail();
                    obj.orderDetailList.Add(subObj);
                    subObj.id = (Guid)item["goodsId"];
                    subObj.name = item["name"].ToString();
                    subObj.imageName = item["imageName"].ToString();
                    subObj.price = (decimal)item["price"];
                    subObj.count = (int)item["count"];
                    subObj.evaluate = (byte)item["evaluate"];
                }

                list.SetDTOObject(ls);
                //string str = string.Format(@"select * from orders o,goods g,orderdetail od from ")
                //var orderList = db.Orders.Where(p=>p.accountId == id).ToList();
                //list.SetDTOObject(orderList.ToDTOs());
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
                foreach (var item in list.DTOObject.orderDetailList)
                {
                    item.name = db.Goods.Single(p => p.id == item.id).name;
                }
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
                model.createTime = DateTime.Now;
                var temp = string.Format("{0:0000}{1:00}{2:00}{3:00}{4:00}{5:00}{6:000}", 
                                            model.createTime.Year, model.createTime.Month, 
                                            model.createTime.Day, model.createTime.Hour, 
                                            model.createTime.Minute, model.createTime.Second, 
                                            model.createTime.Millisecond);

                model.orderNo = long.Parse(temp);
                if (order.orderDetailList.Count == 0)
                {
                    DTOObject.SetWarningInformation("订单信息为空.请选择需要购买的菜品.");
                    return DTOObject;
                }

                //set the first goodsimage for order image
                Guid goodsId = order.orderDetailList[0].id;
                var goods = db.Goods.SingleOrDefault(p => p.id == goodsId);
                model.imangeName = goods.imageName;
                foreach (var item in order.orderDetailList)
                {
                    var goodsInfo = db.Goods.Single(p => p.id == item.id);
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
