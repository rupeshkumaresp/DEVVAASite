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
    
    public partial class tRouteDetails
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tRouteDetails()
        {
            this.tFlightSchedule = new HashSet<tFlightSchedule>();
            this.tLiveOrderDetails = new HashSet<tLiveOrderDetails>();
            this.tMenuForRoute = new HashSet<tMenuForRoute>();
        }
    
        public long RouteID { get; set; }
        public int DepartureID { get; set; }
        public int ArrivalID { get; set; }
        public Nullable<int> RegionID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tFlightSchedule> tFlightSchedule { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tLiveOrderDetails> tLiveOrderDetails { get; set; }
        public virtual tLocation tLocation { get; set; }
        public virtual tLocation tLocation1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tMenuForRoute> tMenuForRoute { get; set; }
        public virtual tRegion tRegion { get; set; }
    }
}