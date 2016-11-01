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
    public class AddressController : ApiController
    {
        [HttpGet]
        [Route("Address")]
        public ReturnJasonConstruct<IList<DTO.Address>> Get()
        {
            return Services.Address.GetAllAddress();
        }

        [HttpGet]
        [Route("Address/{id}")]
        public ReturnJasonConstruct<DTO.Address> Get(Guid id)
        {
            return Services.Address.GetDefaultAddress(id);
        }

        [HttpPost]
        [Route("Address/")]
        public ReturnJasonConstruct<DTO.Address> Post([FromBody]DTO.Address address)
        {
            return Services.Address.Create(address);
        }

        [HttpPut]
        [Route("Address")]
        public ReturnJasonConstruct<DTO.Address> Put([FromBody]DTO.Address address)
        {
            return Services.Address.Update(address);
        }

        [HttpDelete]
        [Route("Address")]
        public ReturnJasonConstruct<bool> delete([FromBody]Guid id)
        {
            return Services.Address.Delete(id);
        }
    }
}
