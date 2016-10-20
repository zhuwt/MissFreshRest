using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;

namespace Services
{
    public class Account
    {
        static Random seed = new Random();
        static public bool Exist(string telNo)
        {
            MissFreshEntities db = new MissFreshEntities();
            var count = db.Customers.Count(p => p.telNo == telNo && p.password != "");
            return count > 0;
        }

        //public void Create(DTO.)
        //{
        //    MissFreshEntities db = new MissFreshEntities();
        //    Models.Customer ct = new Models.Customer();
        //    ct.id = Guid.NewGuid();
        //    ct.


        //    Models.Account ac = new Models.Account();
        //    ac.id = Guid.NewGuid();
        //    ac.UserId = ct.id;
        //    List<Models.Category> list = db.Categories.ToList();
        //    return list.ToDTOs();
        //}

        public static void SendMessage(string telNo)
        {
            string param = @"{name:'小鲜来了',number:'4575'}";
            ITopClient client = new DefaultTopClient("https://eco.taobao.com/router/rest", "23482739", "3c5568a65d3b8ff55c68a9cbc42ab898");
            AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
            req.Extend = "";
            req.SmsType = "normal";
            req.SmsFreeSignName = "邻客网络";
            req.SmsParam = param;
            req.RecNum = telNo;
            req.SmsTemplateCode = "SMS_18285183";
            AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
            Console.WriteLine(rsp.Body);
        }
    }
}
