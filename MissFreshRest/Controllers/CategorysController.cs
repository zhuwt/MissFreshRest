using Services;
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
    public class CategorysController : ApiController
    {
        [HttpGet]
        [Route("category")]
        public IEnumerable<DTO.Category> Get()
        {
            return Category.GetAllCategory();
        }
    }
}
