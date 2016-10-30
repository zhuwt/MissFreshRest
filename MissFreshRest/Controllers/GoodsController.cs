using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MissFreshRest.Controllers
{
    public class GoodsController : ApiController
    {
        // GET: api/Goods
        [HttpGet]
        [Route("goods")]
        public ReturnJasonConstruct<IList<DTO.Goods>> Get()
        {
            return Services.Goods.GetAllGoods();
        }

        // GET: api/Goods/5
        [HttpGet]
        [Route("goods")]
        public ReturnJasonConstruct<DTO.Goods> Get(Guid id)
        {
            return Services.Goods.GetEntireGoodsInformation(id);
        }

        // POST: api/Goods
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Goods/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Goods/5
        public void Delete(int id)
        {
        }
    }
}
