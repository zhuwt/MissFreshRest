//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrderDetail
    {
        public System.Guid id { get; set; }
        public System.Guid orderId { get; set; }
        public System.Guid goodsId { get; set; }
        public int count { get; set; }
        public decimal price { get; set; }
        public Nullable<byte> evaluate { get; set; }
    
        public virtual Good Good { get; set; }
        public virtual Order Order { get; set; }
    }
}
