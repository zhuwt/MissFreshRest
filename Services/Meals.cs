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
    public class Meals
    {
        static public ReturnJasonConstruct<IList<DTO.Meals>> GetAllMeals()
        {
            ReturnJasonConstruct<IList<DTO.Meals>> list = new ReturnJasonConstruct<IList<DTO.Meals>>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var mealsList = db.Meals.ToList();
                list.SetDTOObject(mealsList.ToDTOs());
                return list;
            }
            catch (Exception ex)
            {
                list.SetFailedInformation(ex.Message, null);
                return list;
            }
        }

        static public ReturnJasonConstruct<DTO.Meals> GetEntireMeals(Guid mealsId)
        {
            ReturnJasonConstruct<DTO.Meals> list = new ReturnJasonConstruct<DTO.Meals>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var meals = db.Meals.SingleOrDefault(p => p.id == mealsId);
                var goodsListQuery = from r in db.MealsDetails
                                     join g in db.Goods
                                     on r.goodsId equals g.id
                                     select g;
                var goodsList = goodsListQuery.ToList();
                var DTOobject = meals.ToDTO();
                DTOobject.goodsList = goodsList.ToDTOs();
                list.SetDTOObject(DTOobject);
                return list;
            }
            catch(Exception ex)
            {
                list.SetFailedInformation(ex.Message, null);
                return list;
            }
        }
    }
}
