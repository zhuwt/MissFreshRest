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
        static public bool CanSendCheckCode(string telNo)
        {
            MissFreshEntities db = new MissFreshEntities();
            var customer = db.Customers.SingleOrDefault(p => p.telNo == telNo);
            if (customer == null)
            {
                return true;
            }
            else
            {   
                if (string.IsNullOrEmpty(customer.password))
                {
                    if (customer.codeTime == null || (DateTime.Now - customer.codeTime).Minutes > 1)
                        return true;
                    else
                        return false;//This branch means user send SMS interval less than 1min.
                }
                else
                {   //This branch means the record is legal
                    //So we cannot send the message and we need notify user change telephone number
                    return false;
                }
            }
        }

        static public bool Exist(string telNo)
        {
            MissFreshEntities db = new MissFreshEntities();
            var count = db.Customers.Count(p => p.telNo == telNo);
            return count > 0;
        }

        /// <summary>
        /// Save check code result for create user
        /// </summary>
        /// <param name=""></param>
        public ReturnJasonConstruct<DTO.Account> Create(string telNo)
        {
            ReturnJasonConstruct<DTO.Account> obj = new ReturnJasonConstruct<DTO.Account>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();

                Models.Customer ct = new Models.Customer();
                ct.id = Guid.NewGuid();
                ct.telNo = telNo;
                ct.createTime = DateTime.Now;
                ct.codeTime = DateTime.Now;

                Models.Account ac = new Models.Account();
                ac.id = Guid.NewGuid();
                ac.UserId = ct.id;

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

        public ReturnJasonConstruct<DTO.Account> Update(DTO.Account account)
        {
            ReturnJasonConstruct<DTO.Account> obj = new ReturnJasonConstruct<DTO.Account>();
            try
            {
                Models.Account acc = account.ToModel();
                MissFreshEntities db = new MissFreshEntities();
                var model = db.Accounts.SingleOrDefault(p => p.id == acc.id && p.Customer.id == acc.Customer.id && p.Customer.telNo == acc.Customer.telNo);
                if (model == null)
                {
                    obj.status = (int)executeStatus.warning;
                    obj.information = "没有用户信息可以更新.";
                    return obj;
                }
                else
                {
                    model.Customer.firstName = acc.Customer.firstName;
                    model.Customer.lastName = acc.Customer.lastName;
                    model.Customer.email = acc.Customer.email;
                    model.Customer.password = acc.Customer.password;
                    model.Customer.codeTime = acc.Customer.codeTime;
                    db.SaveChanges();

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
    }
}
