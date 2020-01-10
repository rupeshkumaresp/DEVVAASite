using System.Collections.Generic;
using VAA.DataAccess.Model;
using VAA.Entities.VAAEntity;

namespace VAA.DataAccess.Interfaces
{
    /// <summary>
    /// Menu related operations
    /// </summary>
    public interface IMenu
    {
        List<MenuData> GetAllMenu();
        MenuData GetMenuById(long menuId);
        bool DeleteMenu(string menuCode);
        MenuData GetMenuByMenuCode(string menuCode);
        List<MenuData> GetMenuByCycle(long cycleId);
        List<MenuData> GetMenuByCycleAndMenuType(long cycleId, int menuTypeId);
        List<MenuData> GetMenuByCycleClassAndMenutype(long cycleId, int classId, int menuTypeId);
        List<MenuData> GetMenuByCycleClassMenutypeAndUserid(int userid, long cycleId, int classId, int menuTypeId);
        List<MenuData> GetRoutesByMenu(long menuId);
        
        long AddMenu(string menuName, string menuCode, int menuTypeID, int createdBy, long cycleId, int languageId);
        void AddRouteForMenu(long menuId, long routeId, string flightNo);


        //Menu Items
        List<BaseItem> GetAllMenuItems(long menuId);
        bool AddMenuItem(long menuId, string baseItemCode);

        //approvals
        List<MenuData> GetAllStatuses();
        bool UpdateStatus(MenuData menu, int UserId);

        List<tMenuHistory> GetMenuHistory(long menuId);
        void UpdateMenuHistory(long menuId, int userId, string action);

        //Class
        int GetMenuClass(int menuTypeId);
        int GetClassIdbyName(string className);
        List<tClass> GetAllClass();
        List<tMenuType> GetMenuTypeByClass(int classId);
        tMenuType GetMenuTypeById(int menuTypeId);

        int GetMenuTypeByMenuName(string menuName);

        string GetClassShortName(int classId);
        string GetMenuTypeName(int menuTypeId);

        tMenuTemplates GetMenuTemplate(long menuId);
        bool CreateMenuTemplate(long menuId, int templateId);
        bool UpdateMenuTemplate(long menuId, int templateId, string chilidoc);
        bool UpdateMenuTemplate(long menuId, int templateId);

        tTemplates GetTemplate(int menuTypeId, int languageId);
        tTemplates GetTemplate(int templateId);

        string GetLanguage(int languageId);

        void UpdateMenuNameBasedOnRoute(long menuId);
        long GetMenuIdByMenuCode(string menuCode);

        List<tMenuItemCategory> GetAllMenuItemCategory();
        tMenuItemCategory GetMenuItemCategory(string categoryCode, int language);

        tBoxTicketTemplate GetBoxTicketTemplate(long BoxTicketId);
        void UpdateMenuChangeNotification(string fromUser, string toUser, string menuCode, string menuName, string message);
    }
}

