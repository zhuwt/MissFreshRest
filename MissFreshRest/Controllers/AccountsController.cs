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
        [Route("account/Check/{telNo}")]
        public ReturnJasonConstruct<DTO.Account> Get(string telNo)
        {
            DTO.Account ac = new DTO.Account();
            ReturnJasonConstruct<DTO.Account> obj = new ReturnJasonConstruct<DTO.Account>();
            switch (Services.Account.CanSendCheckCode(telNo))
            {
                case 0:
                    {
                        bool result;
                        int code = SMS.SendMessage(telNo, out result);
                        if (result == false)
                        {
                            obj.status = (int)executeStatus.warning;
                            obj.information = "请勿频繁发送短信，请稍后再次尝试.";
                            return obj;
                        }
                        Console.WriteLine("******Code is:" + code.ToString());
                        if (!Services.Account.Exist(telNo))
                            return Services.Account.Create(telNo, code);
                        else
                        {
                            return Services.Account.Update(telNo, code);
                        }
                    }
                case 1:
                    {
                        //account exist cannot send check code，please change to another telephone number.
                        obj.status = (int)executeStatus.warning;
                        obj.information = "账户已经存在,请确认手机号填写是否正确.";
                        return obj;
                    }
                case 2:
                    {
                        obj.status = (int)executeStatus.warning;
                        obj.information = "验证码每一分钟只能发送一次，请稍后再试.";
                        return obj;
                    }
                default:
                    {
                        obj.status = (int)executeStatus.warning;
                        obj.information = "获取验证码出现未知错误，请联系客服.";
                        return obj;
                    }
            }
        }

        //[HttpGet]
        //[Route("account\exist")]
        //public bool Get(string telNo)
        //{
        //    return Services.Account.Exist(telNo);
        //}

        [HttpGet]
        [Route("account")]
        public ReturnJasonConstruct<Guid> Get(string telNo, string password)
        {
            return Services.Account.GetAccountId(telNo, password);
        }

        [HttpPut]
        [Route("account")]
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
