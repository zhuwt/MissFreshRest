using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;
using Parse;
using DTO;

namespace Services
{
    public class Account
    {
        /// <summary>
        /// Send message to cellphone by indicate
        /// </summary>
        /// <param name="telNo">cellphone number</param>
        /// <returns></returns>
        static public bool CanSendCheckCode(string telNo)
        {
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var result = db.Accounts.AsQueryable().Where(p => p.Customer.telNo == telNo);
                if (result == null || result.Count() == 0)
                {
                    return true;
                }
                else
                {
                    var obj = result.ToList()[0];
                    if (obj.codeTime == null || (DateTime.Now - obj.codeTime).Value.Minutes > 1)
                        return true;
                    else
                        return false;//This branch means user send SMS interval less than 1min.
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Wether an account exist
        /// </summary>
        /// <param name="telNo">cell phone number</param>
        /// <returns></returns>
        static public bool Exist(string telNo)
        {
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var count = db.Customers.Count(p => p.telNo == telNo);
                return count > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Wether an account 
        /// </summary>
        /// <param name="telNo"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        static public bool Exist(string telNo,string password)
        {
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var count = db.Customers.Count(p => p.telNo == telNo && p.password == password);
                return count > 0;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save check code result for create user
        /// </summary>
        /// <param name=""></param>
        static public ReturnJasonConstruct<DTO.Account> Create(string telNo,int code)
        {
            ReturnJasonConstruct<DTO.Account> obj = new ReturnJasonConstruct<DTO.Account>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();

                Models.Customer ct = new Models.Customer();
                ct.id = Guid.NewGuid();
                ct.telNo = telNo;
                ct.createTime = DateTime.Now;

                Models.Account ac = new Models.Account();
                ac.id = Guid.NewGuid();
                ac.code = code.ToString();
                ac.userId = ct.id;

                db.Customers.Add(ct);
                db.Accounts.Add(ac);
                db.SaveChanges();

                obj.status = (int)executeStatus.success;
                obj.DTOObject = ac.ToDTO();

                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update account information
        /// </summary>
        /// <param name="telNo"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static ReturnJasonConstruct<DTO.Account> Update(string telNo, int code)
        {
            ReturnJasonConstruct<DTO.Account> obj = new ReturnJasonConstruct<DTO.Account>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var model = db.Accounts.SingleOrDefault(p => p.Customer.telNo == telNo);
                if (model == null)
                {
                    obj.status = (int)executeStatus.warning;
                    obj.information = "创建用户失败.";
                    return obj;
                }
                else
                {
                    model.code = code.ToString();
                    db.SaveChanges();

                    obj.SetDTOObject(model.ToDTO());
                    obj.status = (int)executeStatus.success;
                    obj.DTOObject = model.ToDTO();
                    return obj;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static ReturnJasonConstruct<DTO.Account> Update(DTO.Account account)
        {
            ReturnJasonConstruct<DTO.Account> obj = new ReturnJasonConstruct<DTO.Account>();
            try
            {
                Models.Account acc = account.ToModel();
                MissFreshEntities db = new MissFreshEntities();
                var model = db.Accounts.SingleOrDefault(p =>p.Customer.telNo == acc.Customer.telNo);
                if (model == null)
                {
                    obj.SetWarningInformation("查找用户失败.");
                    return obj;
                }
                else
                {
                    if (model.code == acc.code)
                    {
                        model.code = SMS.GetRandomCode().ToString();
                        model.Customer.firstName = acc.Customer.firstName;
                        model.Customer.lastName = acc.Customer.lastName;
                        model.Customer.email = acc.Customer.email;
                        model.Customer.password = acc.Customer.password;
                        db.SaveChanges();
                        obj.SetDTOObject(model.ToDTO());
                    }
                    else
                    {
                        obj.SetWarningInformation("验证码输入错误,请重新输入.");
                    }
                    return obj;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
