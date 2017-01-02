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
    public class AddressConfigsController : ApiController
    {
        [HttpGet]
        [Route("AddressConfigs/cityzones")]
        public ReturnJasonConstruct<IList<DTO.AddressConfig>> GetCityZones()
        {
            return Services.AddressConfig.GetAllCityZone();
        }

        [HttpGet]
        [Route("AddressConfigs/zones/{parentId}")]
        public ReturnJasonConstruct<IList<DTO.AddressConfig>> GetZones(Guid parentId)
        {
            return Services.AddressConfig.GetAllZone(parentId);
        }

        [HttpGet]
        [Route("AddressConfigs/buildings/{parentId}")]
        public ReturnJasonConstruct<IList<DTO.AddressConfig>> GetBuildings(Guid parentId)
        {
            return Services.AddressConfig.GetAllBuildings(parentId);
        }

        [HttpGet]
        [Route("AddressConfigs/floors/{parentId}")]
        public ReturnJasonConstruct<IList<DTO.AddressConfig>> GetFloor(Guid parentId)
        {
            return Services.AddressConfig.GetAllFloors(parentId);
        }

        [HttpGet]
        [Route("AddressConfigs/number/{parentId}")]
        public ReturnJasonConstruct<IList<DTO.AddressConfig>> GetNumber(Guid parentId)
        {
            return Services.AddressConfig.GetAllNumber(parentId);
        }

        [HttpPost]
        [Route("AddressConfigs")]
        public ReturnJasonConstruct<DTO.AddressConfig> Create(DTO.AddressConfig DTO)
        {
            return Services.AddressConfig.CreateAddressConfig(DTO);
        }

        [HttpDelete]
        [Route("AddressConfigs/{id}")]
        public ReturnJasonConstruct<bool> Delete(Guid id)
        {
            return Services.AddressConfig.DeleteAddressConfig(id);
        }
    }
}
