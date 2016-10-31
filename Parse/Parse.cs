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
            //var account = model.ParseObject<Models.Account, DTO.Account>();
            var customer = model.Customer.ToDTO();
            var account = new DTO.Account
            {
                id = model.id,
                userId = model.Customer.id,
                code = model.code,
                codeTime = model.codeTime,
                customer = customer
            };
            //account.customer = customer;
            return account;
        }
        public static Models.Account ToModel(this DTO.Account DTOObject)
        {
            //var account = DTOObject.ParseObject<DTO.Account, Models.Account>();
            var customer = DTOObject.customer.ToModel();
            var account = new Models.Account
            {
                id = DTOObject.id,
                userId = customer.id,
                code = DTOObject.code,
                codeTime = DTOObject.codeTime,
                Customer = customer
            };
            //account.Customer = customer;
            return account;
        }
        #endregion

        #region Goods
        public static DTO.Goods ToDTO(this Models.Good model)
        {
            var goods = new DTO.Goods
            {
                id = model.id,
                name = model.name,
                detailName = model.detailName,
                unit = model.Unit1.unit1,
                category = model.Category1.category1,
                price = model.price,
                sellCount = model.sellCount,
                limited = model.limited,
                imageName = model.imageName,
                goodsStatus = model.goodsStatus,
                evaluate = model.evaluate,
                goodsDetail = null
            };
            goods.goodsDetail = model.GoodsDetail.ToDTO();
            return goods;
        }
        public static Models.Good ToModel(this DTO.Goods DTOObject)
        {
            return DTOObject.ParseObject<DTO.Goods, Models.Good>();
        }
        public static IList<DTO.Goods> ToDTOs(this IList<Models.Good> models)
        {
            Mapper.Initialize(p => p.CreateMap<Models.GoodsDetail, DTO.GoodsDetail>());
            List<DTO.Goods> list = new List<DTO.Goods>();
            foreach (var item in models)
            {
                var goods = new DTO.Goods
                {
                    id = item.id,
                    name = item.name,
                    detailName = item.detailName,
                    unit = item.Unit1.unit1,
                    category = item.Category1.category1,
                    price = item.price,
                    sellCount = item.sellCount,
                    limited = item.limited,
                    imageName = item.imageName,
                    goodsStatus = item.goodsStatus,
                    evaluate = item.evaluate,
                    goodsDetail = null
                };
                goods.goodsDetail = item.GoodsDetail.ToDTO();
                list.Add(goods);
            }
            return list;
        }

        public static IList<Models.Good> ToModels(this IList<DTO.Goods> DTOObjects)
        {
            List<Models.Good> list = new List<Models.Good>();
            foreach (var item in DTOObjects)
            {
                list.Add(item.ToModel());
            }
            return list;
        }
        ////////////GoodsDetail/////////////////////
        public static DTO.GoodsDetail ToDTO(this Models.GoodsDetail model)
        {
            return model.ParseObject<Models.GoodsDetail, DTO.GoodsDetail>();
        }
        public static Models.GoodsDetail ToModel(this DTO.GoodsDetail DTOObject)
        {
            return DTOObject.ParseObject<DTO.GoodsDetail, Models.GoodsDetail>();
        }
        #endregion
        #region address
        public static DTO.Address ToDTO(this Models.Address model)
        {
            return model.ParseObject<Models.Address, DTO.Address>();
        }
        public static Models.Address ToModel(this DTO.Address DTOObject)
        {
            return DTOObject.ParseObject<DTO.Address, Models.Address>();
        }
        public static IList<DTO.Address> ToDTOs(this IList<Models.Address> models)
        {
            List<DTO.Address> list = new List<DTO.Address>();
            foreach (var item in models)
            {
                list.Add(item.ToDTO());
            }
            return list;
        }
        public static IList<Models.Address> ToModels(this IList<DTO.Address> DTOObjects)
        {
            List<Models.Address> list = new List<Models.Address>();
            foreach (var item in DTOObjects)
            {
                list.Add(item.ToModel());
            }
            return list;
        }
        #endregion
        #region Meals
        public static DTO.Meals ToDTO(this Models.Meal model)
        {
            return model.ParseObject<Models.Meal, DTO.Meals>();
        }
        public static Models.Meal ToModel(this DTO.Meals DTOObject)
        {
            return DTOObject.ParseObject<DTO.Meals, Models.Meal>();
        }
        public static IList<DTO.Meals> ToDTOs(this IList<Models.Meal> models)
        {
            List<DTO.Meals> list = new List<DTO.Meals>();
            foreach (var item in models)
            {
                list.Add(item.ToDTO());
            }
            return list;
        }
        public static IList<Models.Meal> ToModels(this IList<DTO.Meals> DTOObjects)
        {
            List<Models.Meal> list = new List<Models.Meal>();
            foreach (var item in DTOObjects)
            {
                list.Add(item.ToModel());
            }
            return list;
        }
        #endregion
        #region Order
        public static DTO.Order ToDTO(this Models.Order model)
        {
            return model.ParseObject<Models.Order, DTO.Order>();
        }
        public static Models.Order ToModel(this DTO.Order DTOObject)
        {
            return DTOObject.ParseObject<DTO.Order, Models.Order>();
        }
        public static IList<DTO.Order> ToDTOs(this IList<Models.Order> models)
        {
            List<DTO.Order> list = new List<DTO.Order>();
            foreach (var item in models)
            {
                list.Add(item.ToDTO());
            }
            return list;
        }
        public static IList<Models.Order> ToModels(this IList<DTO.Order> DTOObjects)
        {
            List<Models.Order> list = new List<Models.Order>();
            foreach (var item in DTOObjects)
            {
                list.Add(item.ToModel());
            }
            return list;
        }
        #endregion
    }
}
