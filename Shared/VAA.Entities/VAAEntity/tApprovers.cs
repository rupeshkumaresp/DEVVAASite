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
    
    public partial class tApprovers
    {
        public long ID { get; set; }
        public Nullable<int> OriginLocationID { get; set; }
        public Nullable<int> ClassID { get; set; }
        public Nullable<int> VirginApproverID { get; set; }
        public Nullable<int> CatererID { get; set; }
        public Nullable<int> TranslatorID { get; set; }
    
        public virtual tClass tClass { get; set; }
        public virtual tLocation tLocation { get; set; }
        public virtual tUsers tUsers { get; set; }
        public virtual tUsers tUsers1 { get; set; }
        public virtual tUsers tUsers2 { get; set; }
    }
}