using System;
using System.Linq;
using System.Web.UI;
using Telerik.Web.UI;
using VAA.BusinessComponents;
using VAA.CommonComponents;
using VAA.DataAccess;

namespace VAA.UI
{
    /// <summary>
    /// Validate service plan for any missing items before upload
    /// </summary>
    public partial class ServicePlanValidator : Page
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

        protected void btnUpload_Click(object sender, EventArgs e)
        {

            if (RadAsyncValidateSP.UploadedFiles.Count == 0)
                return;

            UploadedFile attachment = RadAsyncValidateSP.UploadedFiles[0];


            bool validInput = InvalidUploadInput();

            if (!validInput)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "InValidUploadFile", "InValidUploadFile();", true);
                return;
            }

            var missingServiceCode = _menuProcessor.ValidateServicePlanMenu(attachment.InputStream);


            if (missingServiceCode.Count > 0)
            {
                //Loop with comma separated value
                var missingCode = string.Empty;
                for (int i = 0; i < missingServiceCode.Count; i++)
                {
                    missingCode += missingServiceCode[i] + ",";
                }

                var defaultMessage = EmailHelper.MissingBaseItemEmailTemplate;
                defaultMessage = EmailHelper.ConvertMail2(defaultMessage, missingCode, "\\[BASEITEMS\\]");

                string currentUserId = Convert.ToString(Session["USERID"]);

                if (!string.IsNullOrEmpty(currentUserId))
                {
                    int userid = 0;
                    Int32.TryParse(currentUserId, out userid);
                    if (userid > 0)
                    {
                        var user = _accountManagement.GetUserById(userid);
                        EmailHelper.SendMail(user.Username, "ESPAdmin@espcolour.co.uk", "EMMA- Service Plan Validation", defaultMessage);
                        EmailHelper.SendMail("R.Kumar@espcolour.co.uk", "ESPAdmin@espcolour.co.uk", "EMMA- Service Plan Validation", defaultMessage);
                        EmailHelper.SendMail("S.Subramanian@espweb2print.co.uk", "ESPAdmin@espcolour.co.uk", "EMMA- Service Plan Validation", defaultMessage);

                        //EmailHelper.SendMail("C.Corbett@espweb2print.co.uk", "ESPAdmin@espcolour.co.uk", "EMMA- Service Plan Validation", defaultMessage);
                        //EmailHelper.SendMail("k.mcnerlin@espweb2print.co.uk", "ESPAdmin@espcolour.co.uk", "EMMA- Service Plan Validation", defaultMessage);


                    }
                }
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NotValid", "NotValid();", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "ValidateCompleted", "ValidateCompleted();", true);
            }

        }


        private bool InvalidUploadInput()
        {
            bool validInput = true;

            for (int i = 0; i < RadAsyncValidateSP.UploadedFiles.Count; i++)
            {
                UploadedFile attachment = RadAsyncValidateSP.UploadedFiles[i];

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