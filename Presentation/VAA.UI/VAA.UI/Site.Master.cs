using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VAA.DataAccess;

namespace VAA.UI
{
    public partial class SiteMaster : MasterPage
    {
        readonly AccountManagement _accountManagement = new AccountManagement();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                WelcomeUser();
            }
        }

        private void WelcomeUser()
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["USERID"])))
            {
                var userId = Convert.ToInt32(Session["USERID"]);
                var user = _accountManagement.GetUserById(userId);
                if (user != null)
                {
                    lblUsername.InnerText = user.FirstName + "!";
                    if (user.UserType == "Caterer" || user.UserType == "Translator")
                    {
                        liOrderprocess.Visible = false;
                        liUpload.Visible = false;
                        liGenerate.Visible = false;
                        liManage.Visible = false;
                        liScheduler.Visible = false;
                        liReports.Visible = false;
                    }
                    if (user.UserType == "Caterer" || user.UserType == "Translator" || user.UserType == "Virgin")
                    {
                        liManagecycles.Visible = false;
                        liDeleteMenu.Visible = false;
                        liMenuTranslation.Visible = false;
                        //liManageBoxTicket.Visible = false;
                        liManagePrintRun.Visible = false;
                    }
                }
            }
        }

        protected void lnkbtnLogOut_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Account/Login.aspx");
        }
    }
}