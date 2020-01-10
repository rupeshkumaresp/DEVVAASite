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
    public partial class MenuCodeUpdate : Page
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
        /// <summary>
        /// Call rebuild menu for one or more menu codes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRebuildMenu_Click(object sender, EventArgs e)
        {
            try
            {
                var reorderdate = txtMenuReorderDate.Text;

                if (string.IsNullOrEmpty(reorderdate))
                    return;

                Thread thread = new Thread(() => RefreshMenuCodes());
                thread.Start();


                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "RebuildCompleted", "RebuildCompleted();", true);
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void RefreshMenuCodes()
        {
            var reorderdate = txtMenuReorderDate.Text;

            var menuCreatedDate = Convert.ToDateTime(reorderdate);

            //get menu Id
            var menudata = _menuManagement.GetMenuByCreatedDate(menuCreatedDate);

            foreach (var menuId in menudata)
            {
                _menuProcessor.UpdateLotNoChiliVariable(menuId);

            }


        }

    }
}