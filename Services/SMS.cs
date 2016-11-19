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
    public class SMS
    {
        static Random rd = new Random(7752);
        static int minCode = 100000;
        static int maxCode = 999999;
        public static int SendMessage(string telNo)
        {
            return GetRandomCode();
            int code = GetRandomCode();
            string param = string.Format("{number:'{0}'}", code);
            //string param = @"{name:'小鲜来了',number:'4575'}";
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
            return code;
        }

        public static int GetRandomCode()
        {
            return rd.Next(minCode, maxCode);
        }
    }
}
