﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class VAAEntities : DbContext
    {
        public VAAEntities()
            : base("name=VAAEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ELMAH_Error> ELMAH_Error { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<tApprovalStatuses> tApprovalStatuses { get; set; }
        public virtual DbSet<tApprovers> tApprovers { get; set; }
        public virtual DbSet<tBaseItems> tBaseItems { get; set; }
        public virtual DbSet<tBoxTicketData> tBoxTicketData { get; set; }
        public virtual DbSet<tBoxTicketTemplate> tBoxTicketTemplate { get; set; }
        public virtual DbSet<tClass> tClass { get; set; }
        public virtual DbSet<tClassMenuTypeMap> tClassMenuTypeMap { get; set; }
        public virtual DbSet<tCycle> tCycle { get; set; }
        public virtual DbSet<tCycleWeek> tCycleWeek { get; set; }
        public virtual DbSet<tFlightSchedule> tFlightSchedule { get; set; }
        public virtual DbSet<tInstances> tInstances { get; set; }
        public virtual DbSet<tLiveOrderDetails> tLiveOrderDetails { get; set; }
        public virtual DbSet<tLiveOrders> tLiveOrders { get; set; }
        public virtual DbSet<tLocation> tLocation { get; set; }
        public virtual DbSet<tMenu> tMenu { get; set; }
        public virtual DbSet<tMenuApprovalStage> tMenuApprovalStage { get; set; }
        public virtual DbSet<tMenuForRoute> tMenuForRoute { get; set; }
        public virtual DbSet<tMenuHistory> tMenuHistory { get; set; }
        public virtual DbSet<tMenuItem> tMenuItem { get; set; }
        public virtual DbSet<tMenuItemCategory> tMenuItemCategory { get; set; }
        public virtual DbSet<tMenuLanguage> tMenuLanguage { get; set; }
        public virtual DbSet<tMenuTemplates> tMenuTemplates { get; set; }
        public virtual DbSet<tMenuType> tMenuType { get; set; }
        public virtual DbSet<tNotification> tNotification { get; set; }
        public virtual DbSet<tOrders> tOrders { get; set; }
        public virtual DbSet<tOrderStatus> tOrderStatus { get; set; }
        public virtual DbSet<tPDFGenerationJobs> tPDFGenerationJobs { get; set; }
        public virtual DbSet<tPDFGenerationTasks> tPDFGenerationTasks { get; set; }
        public virtual DbSet<tPDFGenerationTasksPackingTicket> tPDFGenerationTasksPackingTicket { get; set; }
        public virtual DbSet<tRegion> tRegion { get; set; }
        public virtual DbSet<tRouteDetails> tRouteDetails { get; set; }
        public virtual DbSet<tSchedulerColors> tSchedulerColors { get; set; }
        public virtual DbSet<tSchedules> tSchedules { get; set; }
        public virtual DbSet<tSeatConfiguration> tSeatConfiguration { get; set; }
        public virtual DbSet<tTemplates> tTemplates { get; set; }
        public virtual DbSet<tTemplatesPacking> tTemplatesPacking { get; set; }
        public virtual DbSet<tUserPermissions> tUserPermissions { get; set; }
        public virtual DbSet<tUsers> tUsers { get; set; }
    
        public virtual ObjectResult<string> ELMAH_GetErrorsXml(string application, Nullable<int> pageIndex, Nullable<int> pageSize, ObjectParameter totalCount)
        {
            var applicationParameter = application != null ?
                new ObjectParameter("Application", application) :
                new ObjectParameter("Application", typeof(string));
    
            var pageIndexParameter = pageIndex.HasValue ?
                new ObjectParameter("PageIndex", pageIndex) :
                new ObjectParameter("PageIndex", typeof(int));
    
            var pageSizeParameter = pageSize.HasValue ?
                new ObjectParameter("PageSize", pageSize) :
                new ObjectParameter("PageSize", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("ELMAH_GetErrorsXml", applicationParameter, pageIndexParameter, pageSizeParameter, totalCount);
        }
    
        public virtual ObjectResult<string> ELMAH_GetErrorXml(string application, Nullable<System.Guid> errorId)
        {
            var applicationParameter = application != null ?
                new ObjectParameter("Application", application) :
                new ObjectParameter("Application", typeof(string));
    
            var errorIdParameter = errorId.HasValue ?
                new ObjectParameter("ErrorId", errorId) :
                new ObjectParameter("ErrorId", typeof(System.Guid));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("ELMAH_GetErrorXml", applicationParameter, errorIdParameter);
        }
    
        public virtual int ELMAH_LogError(Nullable<System.Guid> errorId, string application, string host, string type, string source, string message, string user, string allXml, Nullable<int> statusCode, Nullable<System.DateTime> timeUtc)
        {
            var errorIdParameter = errorId.HasValue ?
                new ObjectParameter("ErrorId", errorId) :
                new ObjectParameter("ErrorId", typeof(System.Guid));
    
            var applicationParameter = application != null ?
                new ObjectParameter("Application", application) :
                new ObjectParameter("Application", typeof(string));
    
            var hostParameter = host != null ?
                new ObjectParameter("Host", host) :
                new ObjectParameter("Host", typeof(string));
    
            var typeParameter = type != null ?
                new ObjectParameter("Type", type) :
                new ObjectParameter("Type", typeof(string));
    
            var sourceParameter = source != null ?
                new ObjectParameter("Source", source) :
                new ObjectParameter("Source", typeof(string));
    
            var messageParameter = message != null ?
                new ObjectParameter("Message", message) :
                new ObjectParameter("Message", typeof(string));
    
            var userParameter = user != null ?
                new ObjectParameter("User", user) :
                new ObjectParameter("User", typeof(string));
    
            var allXmlParameter = allXml != null ?
                new ObjectParameter("AllXml", allXml) :
                new ObjectParameter("AllXml", typeof(string));
    
            var statusCodeParameter = statusCode.HasValue ?
                new ObjectParameter("StatusCode", statusCode) :
                new ObjectParameter("StatusCode", typeof(int));
    
            var timeUtcParameter = timeUtc.HasValue ?
                new ObjectParameter("TimeUtc", timeUtc) :
                new ObjectParameter("TimeUtc", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ELMAH_LogError", errorIdParameter, applicationParameter, hostParameter, typeParameter, sourceParameter, messageParameter, userParameter, allXmlParameter, statusCodeParameter, timeUtcParameter);
        }
    
        public virtual ObjectResult<sp_GetAll_tBaseItems_Result> sp_GetAll_tBaseItems(Nullable<int> classId, Nullable<int> menutypeId, Nullable<int> languageId)
        {
            var classIdParameter = classId.HasValue ?
                new ObjectParameter("classId", classId) :
                new ObjectParameter("classId", typeof(int));
    
            var menutypeIdParameter = menutypeId.HasValue ?
                new ObjectParameter("menutypeId", menutypeId) :
                new ObjectParameter("menutypeId", typeof(int));
    
            var languageIdParameter = languageId.HasValue ?
                new ObjectParameter("languageId", languageId) :
                new ObjectParameter("languageId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetAll_tBaseItems_Result>("sp_GetAll_tBaseItems", classIdParameter, menutypeIdParameter, languageIdParameter);
        }
    
        public virtual ObjectResult<sp_GetAll_tCycle_Result> sp_GetAll_tCycle()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetAll_tCycle_Result>("sp_GetAll_tCycle");
        }
    
        public virtual int sp_GetMenu(Nullable<long> cycleid, Nullable<int> classid, Nullable<int> menutypeid)
        {
            var cycleidParameter = cycleid.HasValue ?
                new ObjectParameter("cycleid", cycleid) :
                new ObjectParameter("cycleid", typeof(long));
    
            var classidParameter = classid.HasValue ?
                new ObjectParameter("classid", classid) :
                new ObjectParameter("classid", typeof(int));
    
            var menutypeidParameter = menutypeid.HasValue ?
                new ObjectParameter("menutypeid", menutypeid) :
                new ObjectParameter("menutypeid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_GetMenu", cycleidParameter, classidParameter, menutypeidParameter);
        }
    
        public virtual ObjectResult<sp_GetMenuAndApprovers_Result> sp_GetMenuAndApprovers(Nullable<long> menuId, Nullable<int> classid)
        {
            var menuIdParameter = menuId.HasValue ?
                new ObjectParameter("menuId", menuId) :
                new ObjectParameter("menuId", typeof(long));
    
            var classidParameter = classid.HasValue ?
                new ObjectParameter("classid", classid) :
                new ObjectParameter("classid", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetMenuAndApprovers_Result>("sp_GetMenuAndApprovers", menuIdParameter, classidParameter);
        }
    
        public virtual ObjectResult<sp_GetMenuAndRoute_Result> sp_GetMenuAndRoute(Nullable<long> cycleId, Nullable<int> classId, Nullable<int> menutypeId)
        {
            var cycleIdParameter = cycleId.HasValue ?
                new ObjectParameter("cycleId", cycleId) :
                new ObjectParameter("cycleId", typeof(long));
    
            var classIdParameter = classId.HasValue ?
                new ObjectParameter("classId", classId) :
                new ObjectParameter("classId", typeof(int));
    
            var menutypeIdParameter = menutypeId.HasValue ?
                new ObjectParameter("menutypeId", menutypeId) :
                new ObjectParameter("menutypeId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetMenuAndRoute_Result>("sp_GetMenuAndRoute", cycleIdParameter, classIdParameter, menutypeIdParameter);
        }
    
        public virtual ObjectResult<sp_GetMenuByUserId_Result> sp_GetMenuByUserId(Nullable<int> userId, Nullable<long> cycleId, Nullable<int> classId, Nullable<int> menutypeId)
        {
            var userIdParameter = userId.HasValue ?
                new ObjectParameter("userId", userId) :
                new ObjectParameter("userId", typeof(int));
    
            var cycleIdParameter = cycleId.HasValue ?
                new ObjectParameter("cycleId", cycleId) :
                new ObjectParameter("cycleId", typeof(long));
    
            var classIdParameter = classId.HasValue ?
                new ObjectParameter("classId", classId) :
                new ObjectParameter("classId", typeof(int));
    
            var menutypeIdParameter = menutypeId.HasValue ?
                new ObjectParameter("menutypeId", menutypeId) :
                new ObjectParameter("menutypeId", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetMenuByUserId_Result>("sp_GetMenuByUserId", userIdParameter, cycleIdParameter, classIdParameter, menutypeIdParameter);
        }
    }
}
