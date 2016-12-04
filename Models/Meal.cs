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
    
    public partial class Meal
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Meal()
        {
            this.MealsDetails = new HashSet<MealsDetail>();
            this.MealsOrderDetails = new HashSet<MealsOrderDetail>();
        }
    
        public System.Guid id { get; set; }
        public string name { get; set; }
        public string imangeName { get; set; }
        public decimal totalPrice { get; set; }
        public int sellCount { get; set; }
        public int stock { get; set; }
        public Nullable<byte> evaluate { get; set; }
        public System.DateTime createTime { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MealsDetail> MealsDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MealsOrderDetail> MealsOrderDetails { get; set; }
    }
}
