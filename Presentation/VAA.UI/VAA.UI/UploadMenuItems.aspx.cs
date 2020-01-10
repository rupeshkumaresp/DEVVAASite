using System;
using System.Linq;
using System.Web.UI;
using Telerik.Web.UI;
using VAA.BusinessComponents;
using VAA.CommonComponents;
using VAA.DataAccess;
using VAA.DataAccess.Model;

namespace VAA.UI
{
    /// <summary>
    /// Upload flight schedule- all quantity calculation is based on flight schedule
    /// </summary>
    public partial class UploadMenuItems : System.Web.UI.Page
    {
        MenuProcessor _menuProcessor = new MenuProcessor();
        readonly AccountManagement _accountManagement = new AccountManagement();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["USERID"])))
                Response.Redirect("~/Account/Login.aspx");

        }
        /// <summary>
        /// Upload excel for flight schedule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UploadMenuItemsBtnClicked(object sender, EventArgs e)
        {
            if (RadAsyncUploadBaseItem.UploadedFiles.Count == 0)
                return;

            UploadedFile attachment = RadAsyncUploadBaseItem.UploadedFiles[0];

            bool validInput = InvalidUploadInput();

            if (!validInput)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "InValidUploadFile", "InValidUploadFile();", true);
                return;
            }

            int userId = Convert.ToInt32(Session["USERID"]);
            var user = _accountManagement.GetUserById(userId);

            string notificationEmails = (System.Configuration.ConfigurationManager.AppSettings["NotificationEmails"]);

            var emails = notificationEmails.Split(new char[] { ';' });

            //valid schedule, upload it
            _menuProcessor.ImportBaseMenuItems(attachment.InputStream);

            var defaultMessage = EmailHelper.BaseMenuItemsUploadTemplate;

            EmailHelper.SendMail(user.Username, "ESPAdmin@espcolour.co.uk", "EMMA- Base Menu Items Upload - " + attachment.FileName, defaultMessage);

            foreach (var email in emails)
            {
                if (!string.IsNullOrEmpty(email))
                    EmailHelper.SendMail(email, "ESPAdmin@espcolour.co.uk", "EMMA- Base Menu Items Upload - " + attachment.FileName, defaultMessage);
            }

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "UploadCompleted", "UploadCompleted();", true);
        }

       
        private bool InvalidUploadInput()
        {
            bool validInput = true;

            for (int i = 0; i < RadAsyncUploadBaseItem.UploadedFiles.Count; i++)
            {
                UploadedFile attachment = RadAsyncUploadBaseItem.UploadedFiles[i];

                if (!Helper.IsValidExcelFormat(attachment.InputStream))
                {
                    validInput = false;
                    break;
                }
            }
            return validInput;
        }
    }
}