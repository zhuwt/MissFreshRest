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
    public class AccountController : ApiController
    {
        // GET: api/Account
        [HttpGet]
        [Route("Account/Check")]
        public void Get(string telNo)
        {
            if (!Services.Account.Exist(telNo))
            {
                Services.Account.SendMessage(telNo);
            }
        }

        // GET: api/Account/5
        public string Get(int id)
        {
            return "Get";
        }

        // POST: api/Account
        public string Post([FromBody]string value)
        {
            return "post";
        }

        // PUT: api/Account/5
        public string Put(int id, [FromBody]string value)
        {
            return "put";
        }

        // DELETE: api/Account/5
        public string Delete(int id)
        {
            return "delete";
        }
    }
}
