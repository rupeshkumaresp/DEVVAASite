//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VAA.Entities.VAAEntity
{
    using System;
    using System.Collections.Generic;
    
    public partial class tLiveOrders
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tLiveOrders()
        {
            this.tLiveOrderDetails = new HashSet<tLiveOrderDetails>();
            this.tOrders = new HashSet<tOrders>();
        }
    
        public long LiveOrderId { get; set; }
        public string LotNo { get; set; }
        public Nullable<long> CycleId { get; set; }
        public Nullable<bool> IsConvertedToOrder { get; set; }
    
        public virtual tCycle tCycle { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tLiveOrderDetails> tLiveOrderDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tOrders> tOrders { get; set; }
    }
}