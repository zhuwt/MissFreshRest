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
    public enum layer
    {
        zone = 0
        , building = 1
        , floor = 2
        , number = 3
    }

    public class AddressConfig
    {
        public static ReturnJasonConstruct<IList<DTO.AddressConfig>> GetAllZone()
        {
            ReturnJasonConstruct<IList<DTO.AddressConfig>> obj = new ReturnJasonConstruct<IList<DTO.AddressConfig>>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var list = db.AddressConfigs.Where(p => p.layer == ((int)layer.zone)).ToList();
                obj.SetDTOObject(list.ToDTOs());
                return obj;
            }
            catch (Exception ex)
            {
                obj.SetFailedInformation(ex.Message, null);
                return obj;
            }
        }

        public static ReturnJasonConstruct<IList<DTO.AddressConfig>> GetAllBuildings(Guid parentId)
        {
            ReturnJasonConstruct<IList<DTO.AddressConfig>> obj = new ReturnJasonConstruct<IList<DTO.AddressConfig>>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var list = db.AddressConfigs.Where(p => p.layer == ((int)layer.building) && p.parentId == parentId).ToList();
                obj.SetDTOObject(list.ToDTOs());
                return obj;
            }
            catch (Exception ex)
            {
                obj.SetFailedInformation(ex.Message, null);
                return obj;
            }
        }

        public static ReturnJasonConstruct<IList<DTO.AddressConfig>> GetAllFloors(Guid parentId)
        {
            ReturnJasonConstruct<IList<DTO.AddressConfig>> obj = new ReturnJasonConstruct<IList<DTO.AddressConfig>>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var list = db.AddressConfigs.Where(p => p.layer == ((int)layer.floor) && p.parentId == parentId).ToList();
                obj.SetDTOObject(list.ToDTOs());
                return obj;
            }
            catch (Exception ex)
            {
                obj.SetFailedInformation(ex.Message, null);
                return obj;
            }
        }

        public static ReturnJasonConstruct<IList<DTO.AddressConfig>> GetAllNumber(Guid parentId)
        {
            ReturnJasonConstruct<IList<DTO.AddressConfig>> obj = new ReturnJasonConstruct<IList<DTO.AddressConfig>>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var list = db.AddressConfigs.Where(p => p.layer == ((int)layer.number) && p.parentId == parentId).ToList();
                obj.SetDTOObject(list.ToDTOs());
                return obj;
            }
            catch (Exception ex)
            {
                obj.SetFailedInformation(ex.Message, null);
                return obj;
            }
        }

        public static ReturnJasonConstruct<DTO.AddressConfig> CreateAddressConfig(DTO.AddressConfig dto)
        {
            ReturnJasonConstruct<DTO.AddressConfig> obj = new ReturnJasonConstruct<DTO.AddressConfig>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var model = dto.ToModel();
                model.id = Guid.NewGuid();
                db.AddressConfigs.Add(model);
                db.SaveChanges();
                obj.SetDTOObject(model.ToDTO());
                return obj;
            }
            catch (Exception ex)
            {
                obj.SetFailedInformation(ex.Message, null);
                return obj;
            }
        }

        public static ReturnJasonConstruct<bool> DeleteAddressConfig(Guid id)
        {
            ReturnJasonConstruct<bool> obj = new ReturnJasonConstruct<bool>();
            try
            {
                MissFreshEntities db = new MissFreshEntities();
                var addressConfig = db.AddressConfigs.SingleOrDefault(p => p.id == id);
                if (addressConfig == null)
                {
                    obj.SetWarningInformation("无法查找到对应的地址信息.");
                    return obj;
                }
                db.AddressConfigs.Remove(addressConfig);
                db.SaveChanges();
                obj.SetDTOObject(true);

                return obj;
            }
            catch (Exception ex)
            {
                obj.SetFailedInformation(ex.Message, false);
                return obj;
            }
        }
    }
}
