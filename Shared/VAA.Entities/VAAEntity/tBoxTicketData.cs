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
    
    public partial class tBoxTicketData
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tBoxTicketData()
        {
            this.tBoxTicketTemplate = new HashSet<tBoxTicketTemplate>();
            this.tPDFGenerationTasksPackingTicket = new HashSet<tPDFGenerationTasksPackingTicket>();
        }
    
        public long ID { get; set; }
        public long OrderId { get; set; }
        public string ClassName { get; set; }
        public string ShipTo { get; set; }
        public Nullable<int> ClassId { get; set; }
        public string FlightNo { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Time { get; set; }
        public string Route { get; set; }
        public Nullable<int> Count { get; set; }
        public string Bound { get; set; }
        public string MenuCode { get; set; }
        public string LoadingFlight { get; set; }
        public Nullable<long> RouteId { get; set; }
        public string BRKMenuCode { get; set; }
        public string TEAMenuCode { get; set; }
    
        public virtual tOrders tOrders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tBoxTicketTemplate> tBoxTicketTemplate { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tPDFGenerationTasksPackingTicket> tPDFGenerationTasksPackingTicket { get; set; }
    }
}
