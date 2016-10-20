using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parse
{
    public static class Parse
    {
        public static DestT ParseObject<SourceT,DestT>(this SourceT sourceObj)
        {
            Mapper.Initialize(p => p.CreateMap<SourceT, DestT>());
            return Mapper.Map<DestT>(sourceObj);
        }
        public static IList<DestT> ParseObjectList<SourceT, DestT>(this IList<SourceT> models)
        {
            List<DestT> list = new List<DestT>();
            foreach (var item in models)
            {
                list.Add(item.ParseObject<SourceT, DestT>());
            }
            return list;
        }
        #region category
        public static DTO.Category ToDTO(this Models.Category model)
        {
            Mapper.Initialize(p => p.CreateMap<Models.Category, DTO.Category>());
            return Mapper.Map<DTO.Category>(model);
        }

        public static Models.Category ToModel(this DTO.Category DTOObject)
        {
            Mapper.Initialize(p => p.CreateMap<DTO.Category, Models.Category>());
            return Mapper.Map<Models.Category>(DTOObject);
        }

        public static IList<DTO.Category> ToDTOs(this IList<Models.Category> models)
        {
            List<DTO.Category> list = new List<DTO.Category>();
            foreach (var item in models)
            {
                list.Add(item.ToDTO());
            }
            return list;
        }

        public static IList<Models.Category> ToModels(this IList<DTO.Category> DTOObjects)
        {
            List<Models.Category> list = new List<Models.Category>();
            foreach (var item in DTOObjects)
            {
                list.Add(item.ToModel());
            }
            return list;
        }
        #endregion
        #region account
        public static DTO.Customer ToDTO(this Models.Customer model)
        {
            return model.ParseObject<Models.Customer, DTO.Customer>();
        }
        public static Models.Customer ToModel(this DTO.Customer DTOObject)
        {
            return DTOObject.ParseObject<DTO.Customer, Models.Customer>();
        }
        public static DTO.Account ToDTO(this Models.Account model)
        {
            var account = model.ParseObject<Models.Account, DTO.Account>();
            var customer = model.Customer.ToDTO();
            account.customer = customer;
            return account;
        }
        public static Models.Account ToModel(this DTO.Account DTOObject)
        {
            var account = DTOObject.ParseObject<DTO.Account, Models.Account>();
            var customer = DTOObject.customer.ToModel();
            account.Customer = customer;
            return account;
        }
        #endregion
    }
}
