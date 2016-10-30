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
    public class CategoryController : ApiController
    {
        // GET: api/Category
        [HttpGet]
        [Route("category")]
        public IEnumerable<DTO.Category> Get()
        {
            return Category.GetAllCategory();
        }

        //// GET: api/Category/5
        //[HttpGet]
        //[Route("user")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST: api/Category
        //[HttpPost]
        //[Route("user")]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Category/5
        //[HttpPut]
        //[Route("user")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Category/5
        //[HttpDelete]
        //[Route("user")]
        //public void Delete(int id)
        //{
        //}
    }
}
