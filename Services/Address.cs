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
        static public ReturnJasonConstruct<IList<DTO.Address>> GetAllAddress()
        {
            ReturnJasonConstruct<IList<DTO.Address>> obj = new ReturnJasonConstruct<IList<DTO.Address>>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var list = db.Addresses.ToList();
                obj.SetDTOObject(list.ToDTOs());
                return obj;
            }
            catch(Exception ex)
            {
                obj.SetFailedInformation(ex.Message, null);
                return obj;
            }
        }

        static public ReturnJasonConstruct<DTO.Address> GetDefaultAddress(Guid accountId)
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

        static public ReturnJasonConstruct<DTO.Address> Create(DTO.Address dto)
        {
            ReturnJasonConstruct<DTO.Address> obj = new ReturnJasonConstruct<DTO.Address>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
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

        static public ReturnJasonConstruct<DTO.Address> Update(DTO.Address dto)
        {
            ReturnJasonConstruct<DTO.Address> obj = new ReturnJasonConstruct<DTO.Address>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var model = db.Addresses.Single(p => p.id == dto.id);
                model.area = dto.area;
                model.building = dto.building;
                model.floor = dto.floor;
                model.number = dto.number;
                model.tel = dto.tel;
                model.name = dto.name;
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

        static public ReturnJasonConstruct<bool> Delete(Guid id)
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
    }
}
