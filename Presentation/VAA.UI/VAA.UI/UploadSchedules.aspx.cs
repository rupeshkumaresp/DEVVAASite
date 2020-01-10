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
    public partial class UploadSchedules : System.Web.UI.Page
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
        public void UploadScheduleBtnClicked(object sender, EventArgs e)
        {
            if (RadAsyncUploadSchedule.UploadedFiles.Count == 0)
                return;

            UploadedFile attachment = RadAsyncUploadSchedule.UploadedFiles[0];

            bool validInput = InvalidUploadInput();

            if (!validInput)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "InValidUploadFile", "InValidUploadFile();", true);
                return;
            }

            int userId = Convert.ToInt32(Session["USERID"]);
            var user = _accountManagement.GetUserById(userId);

            FlightScheduleEngine scheduleEngine = new FlightScheduleEngine();

            string notificationEmails = (System.Configuration.ConfigurationManager.AppSettings["NotificationEmails"]);

            var emails = notificationEmails.Split(new char[] { ';' });

            //validate before upload
            bool valid = ValidateSchedule(attachment, scheduleEngine, user, emails);

            if (!valid)
                return;

            //valid schedule, upload it
            scheduleEngine.UploadFlightSchedule(attachment.InputStream, chkClearFlightSchedule.Checked);

            var defaultMessage = EmailHelper.FlightScheduleUploadTemplate;

            EmailHelper.SendMail(user.Username, "ESPAdmin@espcolour.co.uk", "EMMA- Flight Schedule Upload - " + attachment.FileName, defaultMessage);

            foreach (var email in emails)
            {
                if (!string.IsNullOrEmpty(email))
                    EmailHelper.SendMail(email, "ESPAdmin@espcolour.co.uk", "EMMA- Flight Schedule Upload - " + attachment.FileName, defaultMessage);
            }

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "UploadCompleted", "UploadCompleted();", true);
        }

        private bool ValidateSchedule(UploadedFile attachment, FlightScheduleEngine scheduleEngine, User user, string[] emails)
        {
            bool valid = true;
            //validate before upload
            var missingEquipmentCode = scheduleEngine.ValidateSchedulePlan(attachment.InputStream);

            if (missingEquipmentCode.Count > 0)
            {
                //Loop with comma separated value
                var missingCode = string.Empty;
                for (int j = 0; j < missingEquipmentCode.Count; j++)
                {
                    missingCode += missingEquipmentCode[j] + ",";
                }

                //send email
                var misingEquipmentMessage = EmailHelper.MissingEquipmentEmailTemplate;
                misingEquipmentMessage = EmailHelper.ConvertMail2(misingEquipmentMessage, missingCode, "\\[EQUIPMENTS\\]");

                EmailHelper.SendMail(user.Username, "ESPAdmin@espcolour.co.uk", "EMMA- Flight Schedule Validation - " + attachment.FileName, misingEquipmentMessage);

                foreach (var email in emails)
                {
                    if (!string.IsNullOrEmpty(email))
                        EmailHelper.SendMail(email, "ESPAdmin@espcolour.co.uk", "EMMA- Flight Schedule Validation - " + attachment.FileName, misingEquipmentMessage);
                }

                valid = false;
            }
            return valid;
        }

        private bool InvalidUploadInput()
        {
            bool validInput = true;

            for (int i = 0; i < RadAsyncUploadSchedule.UploadedFiles.Count; i++)
            {
                UploadedFile attachment = RadAsyncUploadSchedule.UploadedFiles[i];

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