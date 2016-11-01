using DTO;
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
    public class MealsController : ApiController
    {
        [HttpGet]
        [Route("Meals")]
        public ReturnJasonConstruct<IList<DTO.Meals>> Get()
        {
            return Services.Meals.GetAllMeals();
        }

        [HttpGet]
        [Route("Meals/{id}")]
        public ReturnJasonConstruct<DTO.Meals> Get(Guid id)
        {
            return Services.Meals.GetEntireMeals(id);
        }
    }
}
