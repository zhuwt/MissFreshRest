using DTO;
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
    public class AccountsController : ApiController
    {
        [HttpGet]
        [Route("Account/Check")]
        public ReturnJasonConstruct<DTO.Account> Get(string telNo)
        {
            DTO.Account ac = new DTO.Account();
            ReturnJasonConstruct<DTO.Account> obj = new ReturnJasonConstruct<DTO.Account>();
            if (Services.Account.CanSendCheckCode(telNo))
            {
                int code = SMS.SendMessage(telNo);
                Console.WriteLine("******Code is:" + code.ToString());
                if (!Services.Account.Exist(telNo))
                    return Services.Account.Create(telNo, code);
                else
                {
                    return Services.Account.Update(telNo, code);
                }
            }
            else
            {
                obj.status = (int)executeStatus.warning;
                obj.information = "验证码每一分钟只能发送一次，请稍后再试.";
                return obj;
                //account exist cannot send check code，please change to another telephone number.
            }
        }

        [HttpGet]
        [Route("Account")]
        public bool Get(string telNo,string password)
        {
            return Services.Account.Exist(telNo, password);
        }

        [HttpPut]
        [Route("Account")]
        public ReturnJasonConstruct<DTO.Account> Put([FromBody]DTO.Account dto)
        {
            ReturnJasonConstruct<DTO.Account> obj = new ReturnJasonConstruct<DTO.Account>();
            if (!Services.Account.Exist(dto.customer.telNo))
            {
                obj.SetWarningInformation("新建用户失败.");
                return obj;
            }
            else
            {
                return Services.Account.Update(dto);
            }
        }
    }
}
