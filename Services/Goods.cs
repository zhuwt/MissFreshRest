using DTO;
using Models;
using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class Goods
    {
        public static ReturnJasonConstruct<IList<DTO.Goods>> GetAllGoods()
        {
            List<DTO.Goods> aa = new List<DTO.Goods>();
            ReturnJasonConstruct <IList<DTO.Goods>> list = new ReturnJasonConstruct<IList<DTO.Goods>>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                List<Models.Good> goodsList = db.Goods.ToList();
                list.SetDTOObject(goodsList.ToDTOs());
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public static ReturnJasonConstruct<DTO.Goods> GetEntireGoodsInformation(Guid goodsId)
        {
            ReturnJasonConstruct<DTO.Goods> obj = new ReturnJasonConstruct<DTO.Goods>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var goods = db.Goods.SingleOrDefault(p => p.id == goodsId);
                obj.SetDTOObject(goods.ToDTO());
                return obj;
            }
            catch (Exception ex)
            {
                obj.SetFailedInformation(ex.Message, null);
                return obj;
            }
        }
    }
}
