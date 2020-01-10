using Elmah;
using System;
using System.Linq;
using System.Web.UI;
using Telerik.Web.UI;
using VAA.BusinessComponents;
using VAA.DataAccess;

namespace VAA.UI
{
    /// <summary>
    /// Hindi and Chinese language is not supported in chili so upload the manually translated PDFs
    /// </summary>
    public partial class MenuTranslation : Page
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
        /// Uplaod the translation PDF - rename to proper PDF name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnUpload_Click(object sender, EventArgs e)
        {
            // TODO: Implement this method

            //get the PDF file and save it with proper renamed name as of that menu

            bool error = false;
            try
            {
                if (RadAsyncUploadTranslation.UploadedFiles.Count == 0)
                    return;

                if (!string.IsNullOrEmpty(txtMenucode.Text))
                {
                    var menu = _menuManagement.GetMenuByMenuCode(txtMenucode.Text);

                    if (menu != null)
                    {
                        string EmmaPDFPathFolder = (System.Configuration.ConfigurationManager.AppSettings["EmmaPDFPathFolder"]);

                        UploadedFile attachment = RadAsyncUploadTranslation.UploadedFiles[0];

                        var menuTemplate = _menuManagement.GetMenuTemplate(menu.Id);
                        var outfilePath = _menuProcessor.GetPdfFileName(Convert.ToInt64(menu.CycleId), menu.Id, menuTemplate.TemplateID, Convert.ToInt32(menu.LanguageId));

                        attachment.SaveAs(EmmaPDFPathFolder + @"\MENU PDFS\" + outfilePath, true);
                    }
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
                error = true;
            }

            if (!error)
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "TranslationFileSaved", "TranslationFileSaved();", true);

            else
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "TranslationFileSaveFailed", "TranslationFileSaveFailed();", true);

        }

        protected void btnGetChiliTemplate_Click(object sender, EventArgs e)
        {
            lblChiliDoc.Visible = true;
            txtChiliDocId.Visible = true;
            txtChiliDocId.Text = "";
            var menuCode = txtMenuCodeForTempalte.Text;

            try
            {
                if (!string.IsNullOrEmpty(menuCode))
                {
                    var menu = _menuManagement.GetMenuByMenuCode(menuCode);

                    if (menu != null)
                    {
                        var menuTemplate = _menuManagement.GetMenuTemplate(menu.Id);

                        if (menuTemplate != null)
                            txtChiliDocId.Text = menuTemplate.ChiliDocumentID;
                    }
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

        }

    }
}