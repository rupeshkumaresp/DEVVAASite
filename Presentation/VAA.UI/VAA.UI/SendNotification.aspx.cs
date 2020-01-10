using Elmah;
using System;
using System.Linq;
using VAA.CommonComponents;
using VAA.DataAccess;

namespace VAA.UI
{
    /// <summary>
    /// Add, edit or Delete Users - Virgin, ESP, Caterer Or Translator
    /// </summary>
    public partial class SendNotification : System.Web.UI.Page
    {
        AccountManagement _accountManagement = new AccountManagement();
        MenuManagement _menuManagement = new MenuManagement();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["USERID"])))
                Response.Redirect("~/Account/Login.aspx");

            if (Session["MENUIDFORNOTIFICATION"] != null)
            {
                long menuId = Convert.ToInt64(Session["MENUIDFORNOTIFICATION"]);
                var menu = _menuManagement.GetMenuById(menuId);

                txtMenuCode.Text = menu.MenuCode;
            }

            if (!Page.IsPostBack)
                lblSendMailStatus.Visible = false;

        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMessage.Text))
                return;

            try
            {
                long menuId = Convert.ToInt64(Session["MENUIDFORNOTIFICATION"]);
                var menu = _menuManagement.GetMenuById(menuId);

                int userId = Convert.ToInt32(Session["USERID"]);

                var user = _accountManagement.GetUserById(userId);


                //send email

                string notificationEmails = (System.Configuration.ConfigurationManager.AppSettings["MenuChangeNotificationEmails"]);

                var emails = notificationEmails.Split(new char[] { ';' });

                var allEmail = "";
                foreach (var email in emails)
                {
                    if (!string.IsNullOrEmpty(email))
                    {
                        allEmail += email +",";
                        EmailHelper.SendMenuChangeNotification(email, "", menu.MenuCode, menu.MenuName, user.FirstName + " " + user.LastName, txtMessage.Text);
                        lblSendMailStatus.Visible = true;

                    }
                }

                //update in database
                _menuManagement.UpdateMenuChangeNotification(user.FirstName + " " + user.LastName, "ESP", menu.MenuCode, menu.MenuName, txtMessage.Text);
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            txtMessage.Text = "";
            lblSendMailStatus.Visible = false;
        }
    }
}