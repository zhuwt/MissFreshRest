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
    public class GoodsController : ApiController
    {
        [HttpGet]
        [Route("goods")]
        public ReturnJasonConstruct<IList<DTO.Goods>> Get()
        {
            return Services.Goods.GetAllGoods();
        }

        [HttpGet]
        [Route("goods/{id}")]
        public ReturnJasonConstruct<DTO.Goods> Get(Guid id)
        {
            return Services.Goods.GetEntireGoodsInformation(id);
        }

        public void Post([FromBody]string value)
        {
        }

        public void Put(int id, [FromBody]string value)
        {
        }

        public void Delete(int id)
        {
        }
    }
}
