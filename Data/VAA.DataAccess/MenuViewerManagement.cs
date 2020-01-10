using System;
using System.Collections.Generic;
using System.Linq;
using VAA.DataAccess.Interfaces;
using VAA.DataAccess.Model;
using VAA.Entities.VAAEntity;

namespace VAA.DataAccess
{
    /// <summary>
    /// MenuViewer management class - Handles all the grid functionalities in the menuviewer/cabincrew site
    /// </summary>
    public class MenuViewerManagement : IMenuViewer
    {
        readonly VAAEntities _context = new VAAEntities();
        OrderManagement _orderManagement = new OrderManagement();

        public List<MenuData> GetListForMenuViewer(long cycle, int menuclass, int menutype, int departure, int arrival, string flightno)
        {
            var origMenuType = menutype;

            if (origMenuType == 100 || (menuclass == 1 && menutype == 6) || (menuclass == 1 && menutype == 7))
            {
                List<MenuViewerData> menuViewerData = new List<MenuViewerData>();

                var CycleName = (from cy in _context.tCycle
                                 where cy.CycleID == cycle
                                 select cy.CycleName).FirstOrDefault();


                var mData = new MenuViewerData();

                if (menuclass == 1 && menutype == 6)
                {
                    mData.ClassName = "Upper Class";
                    mData.MenuId = 11111111111;
                    mData.MenuTypeName = "Allergen guides";
                }

                if (menuclass == 1 && menutype == 7)
                {
                    mData.ClassName = "Upper Class";
                    mData.MenuId = 77777777777;
                    mData.MenuTypeName = "Special Meals";
                }


                if (menuclass == 2)
                {
                    mData.ClassName = "Premium Economy";
                    mData.MenuTypeName = "Allergen guides";
                    mData.MenuId = 22222222222;

                }

                if (menuclass == 3)
                {
                    mData.ClassName = "Economy";
                    mData.MenuTypeName = "Allergen guides";
                    mData.MenuId = 33333333333;
                }

                mData.CycleName = CycleName;

                mData.FlightNo = "All";
                mData.MenuCode = "-";
              
                mData.FromRoute = "All";
                mData.ToRoute = "All";
                menuViewerData.Add(mData);



                return (from x in menuViewerData
                        select new MenuData
                        {
                            FlightNo = x.FlightNo,
                            CycleName = x.CycleName,
                            ClassName = x.ClassName,
                            MenuTypeName = x.MenuTypeName,
                            Route = x.FromRoute + "-" + x.ToRoute,
                            Id = x.MenuId,
                            MenuCode = x.MenuCode
                        }).ToList();
            }

            try
            {
                if (flightno != null)
                {
                    var data = (from m in _context.tMenu
                                join cmtm in _context.tClassMenuTypeMap on m.MenuTypeID equals cmtm.MenuTypeID
                                join mfr in _context.tMenuForRoute on m.ID equals mfr.MenuID
                                join rd in _context.tRouteDetails on mfr.RouteID equals rd.RouteID
                                join cyc in _context.tCycle on m.CycleID equals cyc.CycleID
                                where cyc.Archived == false && (m.CycleID == cycle || cycle == 0) && (cmtm.FlightClassID == menuclass || menuclass == 0) && (m.MenuTypeID == menutype || menutype == 0) && (rd.DepartureID == departure || departure == 0) && (rd.ArrivalID == arrival || arrival == 0) && m.MenuName.Contains(flightno) && mfr.Flights.Contains(flightno)
                                 &&
                                (
                                 (m.IsMovedToLiveOrder == true && (m.MenuTypeID == 1 || m.MenuTypeID == 3 || m.MenuTypeID == 5 || m.MenuTypeID == 10 || m.MenuTypeID == 13))
                                || (m.IsMovedToLiveOrder != true && (m.MenuTypeID == 4 ))
                                || m.MenuTypeID == 2
                                )
                                select new
                                {
                                    FlightNo = mfr.Flights,
                                    CycleName = (from cy in _context.tCycle
                                                 where cy.CycleID == m.CycleID
                                                 select cy.CycleName).FirstOrDefault(),
                                    ClassName = (from cl in _context.tClass
                                                 join cmtmp in _context.tClassMenuTypeMap on cl.ID equals cmtmp.FlightClassID
                                                 where cmtmp.MenuTypeID == m.MenuTypeID
                                                 select cl.FlightClass).FirstOrDefault(),
                                    MenuTypeName = (from mt in _context.tMenuType
                                                    where mt.ID == m.MenuTypeID
                                                    select mt.DisplayName).FirstOrDefault(),
                                    MenuTypeId = m.MenuTypeID,
                                    FromRoute = (from l in _context.tLocation
                                                 where l.LocationID == rd.DepartureID
                                                 select l.AirportCode).FirstOrDefault(),
                                    ToRoute = (from l in _context.tLocation
                                               where l.LocationID == rd.ArrivalID
                                               select l.AirportCode).FirstOrDefault(),
                                    MenuId = m.ID,
                                    MenuCode = m.MenuCode
                                }).ToList();

                    //filter menus based on data before return

                    List<MenuViewerData> menuViewerData = new List<MenuViewerData>();
                    var today = System.DateTime.Now;

                    foreach (var d in data)
                    {
                        var mData = new MenuViewerData();

                        if (d.MenuTypeId == 4 )
                        {
                            mData.ClassName = d.ClassName;
                            mData.CycleName = d.CycleName;
                            mData.FlightNo = d.FlightNo;
                            mData.MenuCode = d.MenuCode;
                            mData.MenuId = d.MenuId;
                            mData.MenuTypeName = d.MenuTypeName;
                            mData.FromRoute = d.FromRoute;
                            mData.ToRoute = d.ToRoute;
                            menuViewerData.Add(mData);
                            continue;
                        }

                        var menuid = d.MenuId;

                        //get live order id for menuid

                        var liveorder = _context.tLiveOrderDetails.Where(lo => lo.MenuId == d.MenuId).FirstOrDefault();

                        if (liveorder == null) continue;

                        var liveorderId = Convert.ToInt64(liveorder.LiveOrderId);

                        //get orderid from live order id

                        var orderId = _orderManagement.GetOrderIdFromLiveOrderId(liveorderId);

                        if (orderId == 0) continue;

                        //get dates
                        var OrderCycleStartDate = _orderManagement.GetOrderCycleStartDate(orderId);

                        if (today >= OrderCycleStartDate.AddDays(-1))
                        {
                            mData.ClassName = d.ClassName;
                            mData.CycleName = d.CycleName;
                            mData.FlightNo = d.FlightNo;
                            mData.MenuCode = d.MenuCode;
                            mData.MenuId = d.MenuId;
                            mData.MenuTypeName = d.MenuTypeName;
                            mData.FromRoute = d.FromRoute;
                            mData.ToRoute = d.ToRoute;

                            menuViewerData.Add(mData);
                        }
                        else
                            continue;
                    }

                    return (from x in menuViewerData
                            select new MenuData
                            {
                                FlightNo = x.FlightNo,
                                CycleName = x.CycleName,
                                ClassName = x.ClassName,
                                MenuTypeName = x.MenuTypeName,
                                Route = x.FromRoute + "-" + x.ToRoute,
                                Id = x.MenuId,
                                MenuCode = x.MenuCode
                            }).ToList();
                }
                else
                {
                    var data = (from m in _context.tMenu
                                join cmtm in _context.tClassMenuTypeMap on m.MenuTypeID equals cmtm.MenuTypeID
                                join mfr in _context.tMenuForRoute on m.ID equals mfr.MenuID
                                join rd in _context.tRouteDetails on mfr.RouteID equals rd.RouteID
                                join cyc in _context.tCycle on m.CycleID equals cyc.CycleID
                                where cyc.Archived == false &&
                                 (m.CycleID == cycle || cycle == 0) && (cmtm.FlightClassID == menuclass || menuclass == 0) && (m.MenuTypeID == menutype || menutype == 0) && (rd.DepartureID == departure || departure == 0) && (rd.ArrivalID == arrival || arrival == 0)
                                 &&
                                (
                                 (m.IsMovedToLiveOrder == true && (m.MenuTypeID == 1 || m.MenuTypeID == 3 || m.MenuTypeID == 5 || m.MenuTypeID == 10 || m.MenuTypeID == 13))
                                || (m.IsMovedToLiveOrder != true && (m.MenuTypeID == 4 ))
                                || m.MenuTypeID == 2
                                )
                                select new
                                {
                                    FlightNo = mfr.Flights,
                                    CycleName = (from cy in _context.tCycle
                                                 where cy.CycleID == m.CycleID
                                                 select cy.CycleName).FirstOrDefault(),
                                    ClassName = (from cl in _context.tClass
                                                 join cmtmp in _context.tClassMenuTypeMap on cl.ID equals cmtmp.FlightClassID
                                                 where cmtmp.MenuTypeID == m.MenuTypeID
                                                 select cl.FlightClass).FirstOrDefault(),
                                    MenuTypeName = (from mt in _context.tMenuType
                                                    where mt.ID == m.MenuTypeID
                                                    select mt.DisplayName).FirstOrDefault(),
                                    MenuTypeId = m.MenuTypeID,
                                    FromRoute = (from l in _context.tLocation
                                                 where l.LocationID == rd.DepartureID
                                                 select l.AirportCode).FirstOrDefault(),
                                    ToRoute = (from l in _context.tLocation
                                               where l.LocationID == rd.ArrivalID
                                               select l.AirportCode).FirstOrDefault(),
                                    MenuId = m.ID,
                                    MenuCode = m.MenuCode
                                }).ToList();


                    //filter menus based on data before return

                    List<MenuViewerData> menuViewerData = new List<MenuViewerData>();
                    var today = System.DateTime.Now;

                    foreach (var d in data)
                    {
                        var mData = new MenuViewerData();

                        if (d.MenuTypeId == 4 )
                        {
                            mData.ClassName = d.ClassName;
                            mData.CycleName = d.CycleName;
                            mData.FlightNo = d.FlightNo;
                            mData.MenuCode = d.MenuCode;
                            mData.MenuId = d.MenuId;
                            mData.MenuTypeName = d.MenuTypeName;
                            mData.FromRoute = d.FromRoute;
                            mData.ToRoute = d.ToRoute;
                            menuViewerData.Add(mData);
                            continue;
                        }

                        var menuid = d.MenuId;

                        //get live order id for menuid

                        var liveorder = _context.tLiveOrderDetails.Where(lo => lo.MenuId == d.MenuId).FirstOrDefault();

                        if (liveorder == null) continue;

                        var liveorderId = Convert.ToInt64(liveorder.LiveOrderId);

                        //get orderid from live order id

                        var orderId = _orderManagement.GetOrderIdFromLiveOrderId(liveorderId);

                        if (orderId == 0) continue;

                        var OrderCycleStartDate = _orderManagement.GetOrderCycleStartDate(orderId);

                        //if beyond current date then add in list

                        if (today >= OrderCycleStartDate.AddDays(-1))
                        {
                            mData.ClassName = d.ClassName;
                            mData.CycleName = d.CycleName;
                            mData.FlightNo = d.FlightNo;
                            mData.MenuCode = d.MenuCode;
                            mData.MenuId = d.MenuId;
                            mData.MenuTypeName = d.MenuTypeName;
                            mData.FromRoute = d.FromRoute;
                            mData.ToRoute = d.ToRoute;
                            menuViewerData.Add(mData);
                        }
                        else
                            continue;
                    }

                    return (from x in menuViewerData
                            select new MenuData
                            {
                                FlightNo = x.FlightNo,
                                CycleName = x.CycleName,
                                ClassName = x.ClassName,
                                MenuTypeName = x.MenuTypeName,
                                Route = x.FromRoute + "-" + x.ToRoute,
                                Id = x.MenuId,
                                MenuCode = x.MenuCode
                            }).ToList();

                }
                return new List<MenuData>();
            }
            catch (Exception e)
            {
                //write to Elma
                // ErrorSignal.FromCurrentContext().Raise(e);

                return new List<MenuData>();
            }
        }

    }

    public class MenuViewerData
    {
        public string FlightNo;
        public string CycleName;
        public string ClassName;
        public string MenuTypeName;
        public string FromRoute;
        public string ToRoute;
        public long MenuId;
        public string MenuCode;
    }
}
