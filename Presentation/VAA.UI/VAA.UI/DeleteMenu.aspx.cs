using Elmah;
using System;
using System.Linq;
using System.Web.UI;
using VAA.BusinessComponents;
using VAA.DataAccess;

namespace VAA.UI
{
    /// <summary>
    /// Handle Menu delete, delete one or more menu items
    /// </summary>
    public partial class DeleteMenu : Page
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
        /// Delete handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDeleteMenu_Click(object sender, EventArgs e)
        {
            try
            {
                var menuCode = txtMenucode.Text;

                if (string.IsNullOrEmpty(menuCode))
                    return;

                var codes = menuCode.Split(new char[] { ',' });

                for (int i = 0; i < codes.Length; i++)
                {
                    if (string.IsNullOrEmpty(codes[i]))
                        continue;

                    _menuManagement.DeleteMenu(codes[i]);
                }

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DeleteCompleted", "DeleteCompleted();", true);
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

    }
}