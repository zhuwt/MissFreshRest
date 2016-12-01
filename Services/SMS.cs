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
        static Random rd = new Random(3752);
        static int minCode = 100000;
        static int maxCode = 999999;

        static string appKey = "23482739";
        static string secret = "3c5568a65d3b8ff55c68a9cbc42ab898";
        public static int SendMessage(string telNo, out bool result)
        {
            result = true;
            return GetRandomCode();
            int code = GetRandomCode();
            string param = string.Format(@"{{code:'{0}'}}", code);
            ITopClient client = new DefaultTopClient("https://eco.taobao.com/router/rest", SMS.appKey, SMS.secret);
            AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
            req.Extend = "";
            req.SmsType = "normal";
            req.SmsFreeSignName = "小鲜来了 ";
            req.SmsParam = param;
            req.RecNum = telNo;
            req.SmsTemplateCode = "SMS_25676012";
            AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
            result = rsp.Result.Success;
            Console.WriteLine(rsp.Body);
            return code;
        }

        public static int GetRandomCode()
        {
            return rd.Next(minCode, maxCode);
        }
    }
}
