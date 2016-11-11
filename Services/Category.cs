using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;

namespace Services
{
    public class Category
    {
        public static IEnumerable<DTO.Category> GetAllCategory()
        {
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                List<Models.Category> list = db.Categorys.ToList();
                return list.ToDTOs();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
