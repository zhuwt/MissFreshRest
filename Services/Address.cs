using DTO;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parse;

namespace Services
{
    public class Address
    {
        const int maxAddressCount = 6;
        public static ReturnJasonConstruct<IList<DTO.Address>> GetAllAddress(Guid accountId)
        {
            ReturnJasonConstruct<IList<DTO.Address>> obj = new ReturnJasonConstruct<IList<DTO.Address>>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var list = db.Addresses.Where(p=>p.accountId == accountId).ToList();
                obj.SetDTOObject(list.ToDTOs());
                return obj;
            }
            catch(Exception ex)
            {
                obj.SetFailedInformation(ex.Message, null);
                return obj;
            }
        }

        public static ReturnJasonConstruct<DTO.Address> GetDefaultAddress(Guid accountId)
        {
            ReturnJasonConstruct<DTO.Address> obj = new ReturnJasonConstruct<DTO.Address>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var address = db.Addresses.SingleOrDefault(p => p.accountId == accountId && p.defaultAddress == true);
                obj.SetDTOObject(address.ToDTO());
                return obj;
            }
            catch (Exception ex)
            {
                obj.SetFailedInformation(ex.Message, null);
                return obj;
            }
        }

        public static ReturnJasonConstruct<DTO.Address> Create(DTO.Address dto)
        {
            ReturnJasonConstruct<DTO.Address> obj = new ReturnJasonConstruct<DTO.Address>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                if (db.Addresses.Count(p => p.accountId == dto.accountId) == maxAddressCount)
                {
                    obj.SetWarningInformation("您的地址数不能超过6个，请删除其他地址来添加地址信息");
                    return obj;
                }
                if (dto.defaultAddress == true)//Need set other address to undefault
                    StaticSetAddressUndefault(ref db, dto.accountId);

                Models.Address model = dto.ToModel();
                model.id = Guid.NewGuid();
                db.Addresses.Add(model);
                db.SaveChanges();
                obj.SetDTOObject(model.ToDTO());
            }
            catch(Exception ex)
            {
                obj.SetFailedInformation(ex.Message, null);
                return obj;
            }
            return obj;
        }

        public static ReturnJasonConstruct<DTO.Address> Update(DTO.Address dto)
        {
            ReturnJasonConstruct<DTO.Address> obj = new ReturnJasonConstruct<DTO.Address>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                if (dto.defaultAddress == true)//Need set other address to undefault
                    StaticSetAddressUndefault(ref db, dto.accountId);

                var model = db.Addresses.Single(p => p.id == dto.id);
                model.Address1 = dto.Address1;
                model.tel = dto.tel;
                model.name = dto.name;
                model.defaultAddress = dto.defaultAddress;
                db.SaveChanges();

                obj.SetDTOObject(model.ToDTO());
            }
            catch (Exception ex)
            {
                obj.SetFailedInformation(ex.Message, null);
                return obj;
            }
            return obj;
        }

        public static ReturnJasonConstruct<bool> Delete(Guid id)
        {
            ReturnJasonConstruct<bool> obj = new ReturnJasonConstruct<bool>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var addressObj = db.Addresses.SingleOrDefault(p => p.id == id);
                db.Addresses.Remove(addressObj);
                db.SaveChanges();
                obj.SetDTOObject(true);
            }
            catch (Exception ex)
            {
                obj.SetFailedInformation(ex.Message, false);
                return obj;
            }
            return obj;
        }

        private static void StaticSetAddressUndefault(ref MissFreshEntities db, Guid userId)
        {
            var addressList = db.Addresses.Where(p => p.accountId == userId).ToList();
            foreach (var item in addressList)
            {
                item.defaultAddress = false;
            }
        }
    }
}
