using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using VAA.DataAccess;

namespace VAA.UI
{
    public class BindControls
    {
        readonly CycleManagement _cycleManagement = new CycleManagement();
        readonly MenuManagement _menuManagement = new MenuManagement();
        readonly RouteManagement _routeManagement = new RouteManagement();
     

        private void BindMenuClass(DropDownList ddlClass)
        {
            ddlClass.Items.Clear();

            var classes = _menuManagement.GetAllClass();

            foreach (var menuclass in classes)
            {
                ddlClass.Items.Add(new ListItem(menuclass.FlightClass, Convert.ToString(menuclass.ID)));
            }
        }
    }
}