using Elmah;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VAA.BusinessComponents;
using VAA.CommonComponents;
using VAA.DataAccess;

namespace VAA.UI
{
    /// <summary>
    /// Upload service plan and create menu
    /// </summary>
    public partial class UploadMenu : Page
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
                BindCycle();
                BindMenuClass();
                //BindMenuType();
            }
        }


        #region Bind DropDowns

        private void BindCycle()
        {
            var cycles = _cycleManagement.GetCycles();

            ddlCycle.Items.Clear();
            foreach (var cycle in cycles)
            {
                ddlCycle.Items.Add(new ListItem(cycle.CycleName, Convert.ToString(cycle.Id)));
            }
        }

        private void BindMenuClass()
        {
            ddlClass.Items.Clear();

            var classes = _menuManagement.GetAllClass();

            foreach (var menuclass in classes)
            {
                ddlClass.Items.Add(new ListItem(menuclass.FlightClass, Convert.ToString(menuclass.ID)));
            }
        }

        //private void BindMenuType()
        //{
        //    ddlMenuType.Items.Clear();

        //    var selectedClassId = ddlClass.SelectedValue;

        //    if (!string.IsNullOrEmpty(selectedClassId))
        //    {
        //        var menuTypes = _menuManagement.GetMenuTypeByClass(Convert.ToInt32(selectedClassId));

        //        foreach (var types in menuTypes)
        //        {
        //            ddlMenuType.Items.Add(new ListItem(types.DisplayName, Convert.ToString(types.ID)));
        //        }
        //    }
        //}

        protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            //BindMenuType();
        }

        #endregion

        /// <summary>
        /// Uplaod service plan
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (RadAsyncUploadMenu.UploadedFiles.Count == 0)
                    return;

                int userId = Convert.ToInt32(Session["USERID"]);
                var user = _accountManagement.GetUserById(userId);

                var servicePlanNames = "";

                bool validInput = InvalidUploadInput();

                if (!validInput)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "InValidUploadFile", "InValidUploadFile();", true);
                    return;
                }

                string notificationEmails = (System.Configuration.ConfigurationManager.AppSettings["NotificationEmails"]);

                var emails = notificationEmails.Split(new char[] { ';' });

                for (int i = 0; i < RadAsyncUploadMenu.UploadedFiles.Count; i++)
                {
                    UploadedFile attachment = RadAsyncUploadMenu.UploadedFiles[i];

                    servicePlanNames += attachment.FileName + ",";

                    //validate before upload
                    var missingServiceCode = _menuProcessor.ValidateServicePlanMenu(attachment.InputStream);

                    if (missingServiceCode.Count > 0)
                    {
                        //Loop with comma separated value
                        var missingCode = string.Empty;
                        for (int j = 0; j < missingServiceCode.Count; j++)
                        {
                            missingCode += missingServiceCode[j] + ",";
                        }

                        //send email
                        var defaultMessage = EmailHelper.MissingBaseItemEmailTemplate;
                        defaultMessage = EmailHelper.ConvertMail2(defaultMessage, missingCode, "\\[BASEITEMS\\]");

                        EmailHelper.SendMail(user.Username, "ESPAdmin@espcolour.co.uk", "EMMA- Service Plan Validation - " + attachment.FileName, defaultMessage);

                        
                        foreach (var email in emails)
                        {
                            if (!string.IsNullOrEmpty(email))
                                EmailHelper.SendMail(email, "ESPAdmin@espcolour.co.uk", "EMMA- Service Plan Validation - " + attachment.FileName, defaultMessage);
                        }


                        if (RadAsyncUploadMenu.UploadedFiles.Count == 1)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NotValid", "NotValid();", true);
                            return;
                        }
                        continue;
                    }


                    var classId = Convert.ToInt32(ddlClass.SelectedValue);

                    var menuTypeId = 1;

                    //TODO: build this for all menu types for the class, call the ImportMenu in loop for all menu types

                    var menuTypes = _menuManagement.GetMenuTypeByClass(classId);

                    foreach (var menuType in menuTypes)
                    {
                        //TODO: uncomment below while testing
                        //if (menuType.ID == 1)
                        _menuProcessor.ImportMenu(Convert.ToInt64(ddlCycle.SelectedValue), Convert.ToInt32(ddlClass.SelectedValue), menuType.ID, attachment.InputStream, userId);
                    }
                    //send email 
                    var uploadMessage = EmailHelper.UploadCompleteEmailTemplate;
                    uploadMessage = EmailHelper.ConvertMail2(uploadMessage, attachment.FileName, "\\[SERVICEPLANNAME\\]");

                    EmailHelper.SendMail(user.Username, "ESPAdmin@espcolour.co.uk", "EMMA- Service Plan Upload", uploadMessage);

                    foreach (var email in emails)
                    {
                        if (!string.IsNullOrEmpty(email))
                            EmailHelper.SendMail(email, "ESPAdmin@espcolour.co.uk", "EMMA- Service Plan Validation", uploadMessage);
                    }

                }

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "UploadCompleted", "UploadCompleted();", true);
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private bool InvalidUploadInput()
        {
            bool validInput = true;
            try
            {
                for (int i = 0; i < RadAsyncUploadMenu.UploadedFiles.Count; i++)
                {
                    UploadedFile attachment = RadAsyncUploadMenu.UploadedFiles[i];

                    if (!Helper.IsValidExcelFormat(attachment.InputStream))
                    {
                        validInput = false;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return validInput;
        }

    }
}