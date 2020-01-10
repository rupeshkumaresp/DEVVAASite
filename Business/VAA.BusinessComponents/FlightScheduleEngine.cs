using Elmah;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VAA.BusinessComponents.Interfaces;
using VAA.CommonComponents;
using VAA.DataAccess;
using VAA.Entities.VAAEntity;

namespace VAA.BusinessComponents
{
    /// <summary>
    /// Flight Schedule handling
    /// </summary>
    public class FlightScheduleEngine : IFlightSchedule
    {
        readonly RouteManagement _routeManagement = new RouteManagement();

        public void UploadFlightSchedule(Stream stream, bool clearSchedule)
        {
            ExcelRecordImporter importer = new ExcelRecordImporter(stream);

            int worksheet = 0;
            foreach (var dataSetName in importer.GetDataSetNames())
            {
                if (worksheet == 0)
                {
                    var importedRows = importer.Import(dataSetName);

                    if (importedRows != null && importedRows.Any())
                    {
                        //clear all the flight schedules

                        if (clearSchedule)
                            _routeManagement.ClearFlightSchedules();

                        //Import flight schedules
                        foreach (var importedRow in importedRows)
                            try
                            {
                                ImportFlightSchedule(importedRow);
                            }
                            catch (Exception e)
                            {
                                //write to Elma
                                ErrorSignal.FromCurrentContext().Raise(e);
                            }
                    }
                }
                worksheet++;

            }
        }

        /// <summary>
        /// Import FLight Schedule
        /// </summary>
        /// <param name="data"></param>
        public void ImportFlightSchedule(Dictionary<string, string> data)
        {
            try
            {
                var FltNo = data["flt no"];

                if (string.IsNullOrEmpty(FltNo))
                    return;

                if (FltNo.ToLower().EndsWith("p"))
                    return;

                if (FltNo.ToLower().EndsWith("t"))
                    return;

                var flightnumber = "VS" + FltNo.ToString().PadLeft(3, '0');

                var origin = data["origin"];
                var dest = data["dest"];

                var departure = data["dept"];
                var arrival = data["arrival"];

                if (string.IsNullOrEmpty(origin) || string.IsNullOrEmpty(dest))
                    return;

                departure = CleanTime(departure);
                arrival = CleanTime(arrival);

                var equipmentype = data["equipment type"];
                var effective = data["effective"];
                var discontinue = data["discontinue"];
                var bound = "";

                if (!(origin.Contains("MAN") || origin.Contains("BFS") || origin.Contains("GLA") || origin.Contains("LGW") || origin.Contains("LHR") || dest.Contains("MAN") || dest.Contains("BFS") || dest.Contains("GLA") || dest.Contains("LGW") || dest.Contains("LHR")))
                    return;

                if (origin.Contains("MAN") || origin.Contains("BFS") || origin.Contains("GLA") || origin.Contains("LGW") || origin.Contains("LHR"))
                    bound = "Outbound";

                if (dest.Contains("MAN") || dest.Contains("BFS") || dest.Contains("GLA") || dest.Contains("LGW") || dest.Contains("LHR"))
                    bound = "Inbound";

                if (string.IsNullOrEmpty(bound))
                    return;

                //if (_routeManagement.IsDomesticRoute(origin, dest))
                //    return;

                var routeId = _routeManagement.GetRouteId(origin, dest);

                tFlightSchedule flightSchedule = new tFlightSchedule
                {
                    FlightNo = flightnumber,
                    ArrivalTime = arrival,
                    DepartureTime = departure,
                    RouteID = routeId,
                    Effective = Convert.ToDateTime(effective),
                    Discontinued = Convert.ToDateTime(discontinue),
                    Bound = bound,
                    EquipmentType = equipmentype,
                    Monday = !string.IsNullOrEmpty(data["m"].Trim()),
                    Tuesday = !string.IsNullOrEmpty(data["tu"].Trim()),
                    Wednesday = !string.IsNullOrEmpty(data["w"].Trim()),
                    Thursday = !string.IsNullOrEmpty(data["th"].Trim()),
                    Friday = !string.IsNullOrEmpty(data["f"].Trim()),
                    Saturday = !string.IsNullOrEmpty(data["sa"].Trim()),
                    Sunday = !string.IsNullOrEmpty(data["su"].Trim())
                };

                _routeManagement.AddSchedule(flightSchedule);
            }
            catch (Exception e)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(e);
            }
        }

        public List<string> ValidateSchedulePlan(Stream stream)
        {
            List<string> missingEquipmentCode = new List<string>();

            try
            {
                ExcelRecordImporter importer = new ExcelRecordImporter(stream);

                int worksheet = 0;
                foreach (var dataSetName in importer.GetDataSetNames())
                {
                    if (worksheet == 0)
                    {
                        var importedRows = importer.Import(dataSetName);

                        if (importedRows != null && importedRows.Any())
                        {
                            foreach (var row in importedRows)
                            {
                                var equipmentCode = Convert.ToString(row["equipment type"]);

                                if (!string.IsNullOrEmpty(equipmentCode))
                                {
                                    var found = _routeManagement.CheckEquipmentType(equipmentCode);

                                    if (!found)
                                        missingEquipmentCode.Add(equipmentCode);
                                }

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return missingEquipmentCode;
        }
        private static string CleanTime(string time)
        {
            time = time.Trim();
            var temp = time.Split(new char[] { ' ' });

            time = temp.Length == 2 ? temp[1] : temp[0];
            return time;
        }
    }
}
