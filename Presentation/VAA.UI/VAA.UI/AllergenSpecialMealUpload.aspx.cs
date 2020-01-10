using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.UI;
using VAA.BusinessComponents;
using VAA.DataAccess;

namespace VAA.UI
{
    /// <summary>
    /// Rebuild the menu after any change in base item
    /// </summary>
    public partial class AllergenSpecialMealUpload : Page
    {
        readonly CycleManagement _cycleManagement = new CycleManagement();
        readonly MenuManagement _menuManagement = new MenuManagement();
        readonly RouteManagement _routeManagement = new RouteManagement();
        readonly AccountManagement _accountManagement = new AccountManagement();

        MenuProcessor _menuProcessor = new MenuProcessor();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["USERID"])))
                Response.Redirect("~/Account/Login.aspx");

            if (!Page.IsPostBack)
            {
            }
        }

    }
}