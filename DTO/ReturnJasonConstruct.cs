using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public enum executeStatus
    {
        success = 0,
        faild = 1,
        warning = 2
    }
    //0，成功
    //1，失败
    //2，执行成功，业务失败带有返回信息
    public class ReturnJasonConstruct<T>
    {
        public int status;
        public string information;
        public T DTOObject;
    }

    public static class ReturnJasonExternMethod
    {
        public static void SetFailedInformation<T>(this ReturnJasonConstruct<T> obj, string failedMessage, T DTOObject)
        {
            obj.status = (int)executeStatus.success;
            obj.information = failedMessage;
            obj.DTOObject = DTOObject;
        }
        public static void SetWarningInformation<T>(this ReturnJasonConstruct<T> obj, string warningMessage)
        {
            obj.status = (int)executeStatus.warning;
            obj.information = warningMessage;
        }

        public static void SetDTOObject<T>(this ReturnJasonConstruct<T> obj, T DTOObject)
        {
            obj.status = (int)executeStatus.success;
            obj.DTOObject = DTOObject;
        }
    }
}
