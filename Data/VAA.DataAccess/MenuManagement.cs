using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using VAA.DataAccess.Interfaces;
using VAA.DataAccess.Model;
using VAA.Entities.VAAEntity;

namespace VAA.DataAccess
{
    /// <summary>
    /// Menu management class - Handles Menu get, add, edit, delete of menu various menu related entities
    /// </summary>
    public class MenuManagement : IMenu
    {
        private readonly VAAEntities _context = new VAAEntities();

        public MenuManagement()
        {
        }



        public List<MenuData> GetAllMenu()
        {
            var menus = (from i in _context.tMenu
                         select new MenuData
                         {
                             Id = i.ID,
                             MenuName = i.MenuName,
                             MenuCode = i.MenuCode,
                             IsDeleted = i.IsDeleted
                         }).ToList();
            return menus;
        }

        public MenuData GetMenuById(long menuId)
        {
            return (from m in _context.tMenu
                    join a in _context.tApprovalStatuses
                    on m.CurrentApprovalStatusID equals a.ApprovalStatusID
                    where m.ID == menuId
                    select new MenuData
                    {
                        Id = m.ID,
                        MenuName = m.MenuName,
                        MenuCode = m.MenuCode,
                        ApprovalStatusName = a.Status,
                        MenuTypeId = m.MenuTypeID,
                        LanguageId = m.LanguageID,
                        CycleId = m.CycleID,
                    }).FirstOrDefault();

        }

        public bool DeleteMenu(string menuCode)
        {
            bool deleted = true;

            try
            {
                var menu = GetMenuByMenuCode(menuCode);

                var allHistory = _context.tMenuHistory.Where(h => h.MenuID == menu.Id).ToList();

                foreach (var history in allHistory)
                {
                    if (history != null)
                        _context.tMenuHistory.Remove(history);
                }

                var menutemplate = _context.tMenuTemplates.Where(t => t.MenuID == menu.Id).FirstOrDefault();
                if (menutemplate != null)
                    _context.tMenuTemplates.Remove(menutemplate);

                var allItems = _context.tMenuItem.Where(h => h.MenuID == menu.Id).ToList();

                foreach (var item in allItems)
                {
                    if (item != null)
                        _context.tMenuItem.Remove(item);
                }

                var allRoutes = _context.tMenuForRoute.Where(h => h.MenuID == menu.Id).ToList();

                foreach (var route in allRoutes)
                {
                    if (route != null)
                        _context.tMenuForRoute.Remove(route);
                }

                var allLiveOrdersDetails = _context.tLiveOrderDetails.Where(h => h.MenuId == menu.Id).ToList();

                foreach (var liveOrderdetails in allLiveOrdersDetails)
                {
                    if (liveOrderdetails != null)
                        _context.tLiveOrderDetails.Remove(liveOrderdetails);
                }

                var allapprovalStage = _context.tMenuApprovalStage.Where(h => h.MenuID == menu.Id).ToList();

                foreach (var approvalStage in allapprovalStage)
                {
                    if (approvalStage != null)
                        _context.tMenuApprovalStage.Remove(approvalStage);
                }

                var pdfTasks = _context.tPDFGenerationTasks.Where(h => h.MenuID == menu.Id).ToList();

                foreach (var pdfTask in pdfTasks)
                {
                    if (pdfTask != null)
                        _context.tPDFGenerationTasks.Remove(pdfTask);
                }

                var tmenu = _context.tMenu.Where(m => m.ID == menu.Id).FirstOrDefault();

                if (tmenu != null)
                    _context.tMenu.Remove(tmenu);

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                deleted = false;
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return deleted;
        }

        public MenuData GetMenuByMenuCode(string menuCode)
        {
            return (from m in _context.tMenu
                    join a in _context.tApprovalStatuses
                    on m.CurrentApprovalStatusID equals a.ApprovalStatusID
                    where m.MenuCode == menuCode
                    select new MenuData
                    {
                        Id = m.ID,
                        MenuName = m.MenuName,
                        MenuCode = m.MenuCode,
                        ApprovalStatusName = a.Status,
                        MenuTypeId = m.MenuTypeID,
                        LanguageId = m.LanguageID,
                        CycleId = m.CycleID,
                    }).FirstOrDefault();

        }
        public List<MenuData> GetAllStatuses()
        {
            return (from s in _context.tApprovalStatuses
                    select new MenuData
                    {
                        ApprovalStatusId = s.ApprovalStatusID,
                        ApprovalStatusName = s.Status
                    }).ToList();
        }
        public bool UpdateStatus(MenuData menu, int UserId)
        {
            try
            {
                var statusUpdate = (from m in _context.tMenu where m.ID == menu.Id select m).FirstOrDefault();
                if (statusUpdate != null)
                {
                    //get current status, if status is same then do not update the status
                    if (statusUpdate.CurrentApprovalStatusID == menu.ApprovalStatusId)
                        return true;

                    statusUpdate.CurrentApprovalStatusID = menu.ApprovalStatusId;
                    _context.SaveChanges();

                    var status = (from s in _context.tApprovalStatuses where s.ApprovalStatusID == menu.ApprovalStatusId select s).FirstOrDefault();

                    var menuHistory = new tMenuHistory();
                    menuHistory.ActionByUserID = UserId;
                    menuHistory.MenuID = menu.Id;
                    menuHistory.ModifiedAt = DateTime.Now;
                    menuHistory.ActionTaken = "Status Changed To: " + status.Status;

                    _context.tMenuHistory.Add(menuHistory);
                    _context.SaveChanges();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(e);
                return false;
            }
        }
        public List<MenuData> GetMenuByCycle(long cycleId)
        {
            var menus = (from i in _context.tMenu
                         where i.CycleID == cycleId
                         select new MenuData
                         {
                             Id = i.ID,
                             MenuName = i.MenuName,
                             MenuCode = i.MenuCode,
                             IsDeleted = i.IsDeleted
                         }).ToList();
            return menus;
        }


        public List<MenuData> GetMenuByCycleAndMenuType(long cycleId, int menuTypeId)
        {
            var menus = (from i in _context.tMenu
                         where i.CycleID == cycleId && i.MenuTypeID == menuTypeId
                         select new MenuData
                         {
                             Id = i.ID,
                             MenuName = i.MenuName,
                             MenuCode = i.MenuCode
                         }).ToList();
            return menus;
        }


        public int GetMenuClass(int menuTypeId)
        {
            var menuClass = (from c in _context.tClassMenuTypeMap where c.MenuTypeID == menuTypeId select c).FirstOrDefault();

            if (menuClass != null)
                return menuClass.FlightClassID;

            return 0;

        }

        public int GetClassIdbyName(string className)
        {
            var menuClass = (from c in _context.tClass where c.FlightClass == className select c).FirstOrDefault();

            if (menuClass != null)
                return menuClass.ID;

            return 0;
        }

        public List<MenuData> GetMenuByCycleClassAndMenutype(long cycleId, int classId, int menuTypeId)
        {
            try
            {
                return (from m in _context.tMenu
                        join cmtm in _context.tClassMenuTypeMap on m.MenuTypeID equals cmtm.MenuTypeID
                        join lang in _context.tMenuLanguage on m.LanguageID equals lang.ID
                        join status in _context.tApprovalStatuses on m.CurrentApprovalStatusID equals status.ApprovalStatusID
                        where m.CycleID == cycleId && cmtm.FlightClassID == classId && m.MenuTypeID == menuTypeId
                        select new MenuData
                        {
                            Id = m.ID,
                            MenuName = m.MenuName,
                            MenuCode = m.MenuCode,
                            ApprovalStatus = status.Status,
                            LanguageId = m.LanguageID,
                            LanguageName = lang.LanguageCode
                        }).ToList();
            }
            catch (Exception e)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(e);

                return new List<MenuData>();
            }
        }
        public List<MenuData> GetMenuByCycleClassMenutypeAndUserid(int userId, long cycleId, int classId, int menuTypeId)
        {
            try
            {
                return (from m in _context.sp_GetMenuByUserId(userId, cycleId, classId, menuTypeId)
                        select new MenuData
                        {
                            Id = m.ID,
                            MenuName = m.MenuName,
                            MenuCode = m.MenuCode,
                            ApprovalStatus = m.ApprovalStatus,
                            LanguageId = m.LanguageID,
                            LanguageName = m.LanguageCode
                        }).Distinct().ToList();
            }
            catch (Exception e)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(e);
                return new List<MenuData>();
            }
        }

        public List<MenuData> GetRoutesByMenu(long menuId)
        {
            try
            {
                return (from mfr in _context.tMenuForRoute
                        join rd in _context.tRouteDetails on mfr.RouteID equals rd.RouteID
                        join r in _context.tRegion on rd.RegionID equals r.ID
                        join dl in _context.tLocation on rd.DepartureID equals dl.LocationID
                        join al in _context.tLocation on rd.ArrivalID equals al.LocationID
                        where mfr.MenuID == menuId
                        select new MenuData
                        {
                            RouteId = mfr.RouteID,
                            RegionName = r.RegionName,
                            DepartureAirportName = dl.AirportName,
                            DepartureAirportCode = dl.AirportCode,
                            ArrivalAirportName = al.AirportName,
                            ArrivalAirportCode = al.AirportCode,
                            FlightNo = mfr.Flights
                        }).ToList();

            }
            catch (Exception e)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(e);
                return new List<MenuData>();
            }
        }


        public List<BaseItem> GetAllMenuItems(long menuId)
        {
            try
            {
                List<BaseItem> baseItemCollection = new List<BaseItem>();

                var menu = GetMenuById(menuId);

                var menuItemCollection = (from mi in _context.tMenuItem where mi.MenuID == menuId select mi).ToList();

                foreach (var menuItem in menuItemCollection)
                {
                    var baseItemData = (from baseItem in _context.tBaseItems where baseItem.BaseItemCode == menuItem.BaseItemCode select baseItem).FirstOrDefault();

                    if (baseItemData != null)
                    {
                        var categoryId = baseItemData.CategoryID;

                        var category =
                            (from c in _context.tMenuItemCategory where c.ID == categoryId select c).FirstOrDefault();

                        if (category != null)
                        {
                            BaseItem baseItem = new BaseItem()
                            {
                                BaseItemId = baseItemData.ID,
                                BaseItemCode = baseItemData.BaseItemCode,
                                ClassId = baseItemData.ClassID,
                                MenuTypeId = baseItemData.MenuTypeID,
                                CategoryName = category.CategoryName,
                                CategoryCode = category.CategoryCode,
                                BaseItemTitle = baseItemData.BaseItemTitle,
                                BaseItemTitleDescription = baseItemData.BaseItemTitleDescription,
                                BaseItemDescription = baseItemData.BaseItemDescription,
                                BaseItemSubDescription = baseItemData.BaseItemSubDescription,
                                BaseItemAttributes = baseItemData.BaseItemAttributes
                            };

                            baseItemCollection.Add(baseItem);
                        }
                    }
                }
                return baseItemCollection;
            }
            catch (Exception e)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(e);
                return null;
            }
        }

        public bool AddMenuItem(long menuId, string baseItemCode)
        {
            var item = new tMenuItem();
            item.BaseItemCode = baseItemCode;
            item.MenuID = menuId;
            //item.Sequence = sequence;

            _context.tMenuItem.Add(item);
            _context.SaveChanges();

            return true;
        }


        public List<tMenuHistory> GetMenuHistory(long menuId)
        {
            return _context.tMenuHistory.Where(h => h.MenuID == menuId).ToList();
        }

        public List<tClass> GetAllClass()
        {
            return _context.tClass.ToList();
        }

        public List<tMenuType> GetMenuTypeByClass(int classId)
        {
            List<tMenuType> menuTypeList = new List<tMenuType>();

            //var menuTypeMap = _context.tClassMenuTypeMap.Where(mt => mt.FlightClassID == classId).ToList();

            var menuTypeMap =
                (from mtp in _context.tClassMenuTypeMap where mtp.FlightClassID == classId select mtp).ToList();


            foreach (var mtm in menuTypeMap)
            {
                var menuType = _context.tMenuType.FirstOrDefault(mt => mt.ID == mtm.MenuTypeID);

                if (menuType != null)
                    menuTypeList.Add(menuType);
            }

            return menuTypeList;
        }

        public List<tMenuItemCategory> GetAllMenuItemCategory()
        {
            return _context.tMenuItemCategory.ToList();
        }


        public tMenuItemCategory GetMenuItemCategory(string categoryCode, int language)
        {
            var menuItemCategory = (from mc in _context.tMenuItemCategory where mc.CategoryCode == categoryCode && mc.LanguageId == language select mc).FirstOrDefault();

            if (menuItemCategory != null)
            {
                return menuItemCategory;
            }

            return null;

        }


        public string GetMenuItemCategoryCode(int categoryId)
        {
            var menuItemCategory = (from mc in _context.tMenuItemCategory where mc.ID == categoryId select mc).FirstOrDefault();

            if (menuItemCategory != null)
            {
                return menuItemCategory.CategoryCode;
            }

            return "";

        }



        public long AddMenu(string menuName, string menuCode, int menuTypeID, int createdBy, long cycleId, int languageId)
        {
            tMenu menu = new tMenu();

            menu.MenuName = menuName;
            menu.MenuCode = menuCode;
            menu.MenuTypeID = menuTypeID;
            menu.CreatedBy = createdBy;
            menu.CycleID = cycleId;
            menu.LanguageID = languageId;
            menu.CreatedAt = DateTime.Now;
            menu.IsDeleted = false;
            menu.CurrentApprovalStatusID = 1;


            _context.tMenu.Add(menu);
            _context.SaveChanges();

            return menu.ID;

        }


        public void AddRouteForMenu(long menuId, long routeId, string flightNo)
        {
            var routeForMenu = (from rm in _context.tMenuForRoute where rm.MenuID == menuId && rm.RouteID == routeId select rm).FirstOrDefault();

            if (routeForMenu == null)
            {
                routeForMenu = new tMenuForRoute();

                routeForMenu.MenuID = menuId;
                routeForMenu.RouteID = routeId;
                routeForMenu.Flights = flightNo;
                _context.tMenuForRoute.Add(routeForMenu);
                _context.SaveChanges();
            }
            else
            {
                routeForMenu.Flights = routeForMenu.Flights + "," + flightNo;
            }
        }


        public string GetClassShortName(int classId)
        {

            var flightClass = (from c in _context.tClass where c.ID == classId select c).FirstOrDefault();

            if (flightClass != null)
            {
                return flightClass.ShortName;
            }

            return "";
        }


        public string GetMenuTypeName(int menuTypeId)
        {
            var menuType = (from mt in _context.tMenuType where mt.ID == menuTypeId select mt).FirstOrDefault();

            if (menuType != null)
            {
                return menuType.DisplayName;
            }

            return "";
        }

        public tMenuTemplates GetMenuTemplate(long menuId)
        {
            var menuTemplate = _context.tMenuTemplates.Where(mt => mt.MenuID == menuId).FirstOrDefault();

            if (menuTemplate != null)
                return menuTemplate;

            return null;

        }

        public bool CreateMenuTemplate(long menuId, int templateId)
        {
            var menutemplate = _context.tMenuTemplates.Where(mt => mt.MenuID == menuId).FirstOrDefault();

            if (menutemplate == null)
            {
                menutemplate = new tMenuTemplates();
                menutemplate.MenuID = menuId;
                menutemplate.TemplateID = templateId;

                _context.tMenuTemplates.Add(menutemplate);
                _context.SaveChanges();
            }
            return true;
        }

        public bool UpdateMenuTemplate(long menuId, int templateId, string chilidoc)
        {
            var menutemplate = _context.tMenuTemplates.Where(mt => mt.MenuID == menuId).FirstOrDefault();

            if (menutemplate != null)
            {
                menutemplate.ChiliDocumentID = chilidoc;
                menutemplate.TemplateID = templateId;
                _context.SaveChanges();
            }
            else
            {
                menutemplate = new tMenuTemplates();
                menutemplate.MenuID = menuId;
                menutemplate.TemplateID = templateId;
                menutemplate.ChiliDocumentID = chilidoc;

                _context.tMenuTemplates.Add(menutemplate);
                _context.SaveChanges();
            }
            return true;
        }

        public bool UpdateMenuTemplate(long menuId, int templateId)
        {
            var menutemplate = _context.tMenuTemplates.Where(mt => mt.MenuID == menuId).FirstOrDefault();

            if (menutemplate != null)
            {
                menutemplate.TemplateID = templateId;
                _context.SaveChanges();
            }
            else
            {
                menutemplate = new tMenuTemplates();
                menutemplate.MenuID = menuId;
                menutemplate.TemplateID = templateId;

                _context.tMenuTemplates.Add(menutemplate);
                _context.SaveChanges();
            }
            return true;
        }


        public tTemplates GetTemplate(int menuTypeId, int languageId)
        {
            var template = (from t in _context.tTemplates where t.MenuTypeID == menuTypeId && t.LanguageId == languageId select t).FirstOrDefault();

            if (template != null)
                return template;
            return null;
        }


        public tTemplates GetTemplate(int templateId)
        {
            var template = (from t in _context.tTemplates where t.TemplateID == templateId select t).FirstOrDefault();

            if (template != null)
                return template;
            return null;
        }


        public string GetLanguage(int languageId)
        {
            var language = _context.tMenuLanguage.Where(l => l.ID == languageId).FirstOrDefault();

            if (language != null)
                return language.LanguageCode;

            return "";
        }

        public void UpdateMenuNameBasedOnRoute(long menuId)
        {
            var routes = (from r in _context.tMenuForRoute where r.MenuID == menuId select r).ToList();

            string flightdetails = "";

            foreach (var route in routes)
            {
                var flight = route.Flights;

                if (route.Flights.Contains(","))
                    flight = flight.Replace(",", "/");

                flightdetails += "/" + flight;
            }

            var menu = (from m in _context.tMenu where m.ID == menuId select m).FirstOrDefault();

            menu.MenuName += flightdetails;

            _context.SaveChanges();
        }

        public void UpdateMenuHistory(long menuId, int userId, string action)
        {
            tMenuHistory menuHistory = new tMenuHistory();
            menuHistory.ActionByUserID = userId;
            menuHistory.ActionTaken = action;
            menuHistory.ModifiedAt = System.DateTime.Now;
            menuHistory.MenuID = menuId;
            _context.tMenuHistory.Add(menuHistory);
            _context.SaveChanges();
        }
        public tMenuType GetMenuTypeById(int menuTypeId)
        {
            var menuType = _context.tMenuType.FirstOrDefault(mt => mt.ID == menuTypeId);

            return menuType;
        }

        public int GetMenuTypeByMenuName(string menuName)
        {
            var menuType = _context.tMenuType.FirstOrDefault(mt => mt.MenuType == menuName);

            if (menuType != null)
                return menuType.ID;
            else
                return 0;

        }

        public long GetMenuIdByMenuCode(string menuCode)
        {
            var menuid = (from m in _context.tMenu where m.MenuCode == menuCode select m.ID).FirstOrDefault();
            if (menuid != null)
            {
                return menuid;
            }
            else
            {
                return 0;
            }
        }

        public tBoxTicketTemplate GetBoxTicketTemplate(long BoxTicketId)
        {
            return _context.tBoxTicketTemplate.Where(t => t.BoxTicketID == BoxTicketId).FirstOrDefault();
        }

        public void UpdateMenuChangeNotification(string fromUser, string toUser, string menuCode, string menuName, string message)
        {
            tNotification notification = new tNotification();

            notification.FromUser = fromUser;
            notification.MenuCode = menuCode;
            notification.MenuName = menuName;
            notification.NotificationMessage = message;
            notification.SentToUser = toUser;
            notification.CreatedAt = System.DateTime.Now;

            _context.tNotification.Add(notification);

            _context.SaveChanges();

        }


        public int GetTemplateIdByChiliDocumentId(string chiliDocumentId)
        {
            var template = _context.tTemplates.Where(t => t.ChiliDocumentID == chiliDocumentId).FirstOrDefault();

            if (template != null)
                return template.TemplateID;

            return 0;
        }

        public int GetLanguageIdByChiliDocumentId(string chiliDocumentId)
        {
            var template = _context.tTemplates.Where(t => t.ChiliDocumentID == chiliDocumentId).FirstOrDefault();

            if (template != null)
                return Convert.ToInt32(template.LanguageId);

            return 0;
        }

        public void UpdateMenuLanguage(long menuId, int languageId)
        {
            var menu = _context.tMenu.Where(m => m.ID == menuId).FirstOrDefault();

            if (menu != null)
            {
                menu.LanguageID = languageId;
                _context.SaveChanges();
            }
        }

        public string GetBreakfastMenuCodeFromMainMenuCode(string menuCode, long? routeId, string FlightNo, long liveOrderId)
        {
            // TODO: Implement this method
            var mainMenu = GetMenuByMenuCode(menuCode);

            var cycleId = mainMenu.CycleId;
            var liveOrderDetails = (from lod in _context.tLiveOrderDetails where lod.RouteId == routeId && lod.FlightNo == FlightNo && lod.LiveOrderId == liveOrderId select lod).ToList();

            foreach (var lo in liveOrderDetails)
            {
                var menuId = lo.MenuId;

                var menuObj = GetMenuById(Convert.ToInt64(menuId));

                if (menuObj.MenuTypeId == 3 && menuObj.CycleId == cycleId)
                {
                    return menuObj.MenuCode;
                }


            }
            return "";


        }

        public string GetTeaMenuCodeFromMainMenuCode(string menuCode, long? routeId, string FlightNo, long liveOrderId)
        {
            // TODO: Implement this method
            var mainMenu = GetMenuByMenuCode(menuCode);

            var cycleId = mainMenu.CycleId;
            var liveOrderDetails = (from lod in _context.tLiveOrderDetails where lod.RouteId == routeId && lod.FlightNo == FlightNo && lod.LiveOrderId == liveOrderId select lod).ToList();

            foreach (var lo in liveOrderDetails)
            {
                var menuId = lo.MenuId;

                var menuObj = GetMenuById(Convert.ToInt64(menuId));

                if (menuObj.MenuTypeId == 2 && menuObj.CycleId == cycleId)
                {
                    return menuObj.MenuCode;
                }


            }
            return "";
        }

        public string GetWineCardMenuCodeFromMainMenuCode(string menuCode, long? routeId, string FlightNo)
        {
            // TODO: Implement this method
            var mainMenu = GetMenuByMenuCode(menuCode);

            var cycleId = mainMenu.CycleId;
            var liveOrderDetails = (from lod in _context.tLiveOrderDetails where lod.RouteId == routeId && lod.FlightNo == FlightNo select lod).ToList();

            foreach (var lo in liveOrderDetails)
            {
                var menuId = lo.MenuId;

                var menuObj = GetMenuById(Convert.ToInt64(menuId));

                if (menuObj.MenuTypeId == 4 && menuObj.CycleId == cycleId)
                {
                    return menuObj.MenuCode;
                }


            }
            return "";
        }

        public void updateTeaBoxTicketData(long ticketid, string teaCode)
        {
            var ticket = _context.tBoxTicketData.Where(t => t.ID == ticketid).FirstOrDefault();
            ticket.TEAMenuCode = teaCode;
            _context.SaveChanges();
        }

        public void updateBRKBoxTicketData(long ticketid, string BRKCode)
        {
            var ticket = _context.tBoxTicketData.Where(t => t.ID == ticketid).FirstOrDefault();
            ticket.BRKMenuCode = BRKCode;
            _context.SaveChanges();
        }




        public List<long> GetMenuByCreatedDate(DateTime menuCreatedDate)
        {
            var dateEnd = menuCreatedDate.AddDays(1);
            var menuIds = (from m in _context.tMenu where m.CreatedAt >= menuCreatedDate && m.CreatedAt <= dateEnd select m.ID).ToList();

            return menuIds;
        }
    }
}
