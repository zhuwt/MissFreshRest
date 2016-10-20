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
            MissFreshEntities db = new MissFreshEntities();
            List<Models.Category> list = db.Categories.ToList();
            return list.ToDTOs();
        }
    }
}
