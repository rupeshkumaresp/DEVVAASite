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
    
    public partial class tFlightSchedule
    {
        public long ID { get; set; }
        public Nullable<long> RouteID { get; set; }
        public string FlightNo { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivalTime { get; set; }
        public Nullable<bool> Monday { get; set; }
        public Nullable<bool> Tuesday { get; set; }
        public Nullable<bool> Wednesday { get; set; }
        public Nullable<bool> Thursday { get; set; }
        public Nullable<bool> Friday { get; set; }
        public Nullable<bool> Saturday { get; set; }
        public Nullable<bool> Sunday { get; set; }
        public string EquipmentType { get; set; }
        public Nullable<System.DateTime> Effective { get; set; }
        public Nullable<System.DateTime> Discontinued { get; set; }
        public string Bound { get; set; }
    
        public virtual tRouteDetails tRouteDetails { get; set; }
    }
}