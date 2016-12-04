//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Good
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Good()
        {
            this.MealsDetails = new HashSet<MealsDetail>();
            this.OrderDetails = new HashSet<OrderDetail>();
        }
    
        public System.Guid id { get; set; }
        public string name { get; set; }
        public string detailName { get; set; }
        public int unit { get; set; }
        public int category { get; set; }
        public decimal price { get; set; }
        public int sellCount { get; set; }
        public int stock { get; set; }
        public int limited { get; set; }
        public string imageName { get; set; }
        public int goodsStatus { get; set; }
        public Nullable<byte> evaluate { get; set; }
        public System.DateTime createTime { get; set; }
    
        public virtual Category Category1 { get; set; }
        public virtual GoodsDetail GoodsDetail { get; set; }
        public virtual Unit Unit1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MealsDetail> MealsDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
