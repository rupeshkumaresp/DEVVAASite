using System;

namespace VAA.DataAccess.Model
{
    public class Menu
    {
        public long Id { get; set; }
        public string MenuName { get; set; }
        public string MenuCode { get; set; }
        public string ChiliPdfName { get; set; }
        public bool? IsDeleted { get; set; }
        
        //Language
        public int LanguageId { get; set; }
        public string LanguageName { get; set; }
        
        //Cycle
        public Int64 CycleId { get; set; }        
        public string CycleName { get; set; }
        public string CycleShortName { get; set; }
        public string CycleYear { get; set; }
        public DateTime? CycleStartDate { get; set; }
        public DateTime? CycleEndDate { get; set; }

        //class
        public int ClassId { get; set; }
        public string ClassName { get; set; }

        //Route
        public long? RouteId { get; set; }
        public string RegionName { get; set; }
        public int DepartureId { get; set; }
        public int DepartureRegionId { get; set; }
        public string DepartureRegionName { get; set; }
        public string DepartureAirportName { get; set; }
        public string DepartureAirportCode { get; set; }
        public int ArrivalId { get; set; }
        public int ArrivalRegionId { get; set; }
        public string ArrivalRegionName { get; set; }
        public string ArrivalAirportName { get; set; }
        public string ArrivalAirportCode { get; set; }
        public string DepartureTime { get; set; }
        public string ArrivialTime { get; set; }

        //Flight info
        public string FlightNo { get; set; }
        public int SeatUpperClass { get; set; }
        public int SeatPremiumEconomy { get; set; }
        public int SeatEconomy { get; set; }

        //Menu Type
        public int MenuTypeId { get; set; }
        public string MenuTypeName { get; set; }
        public string MenuTypeDisplayName { get; set; }

        //Approval Status
        public int ApprovalStatusId { get; set; }
        public string ApprovalStatusName { get; set; }

        

    }
}
