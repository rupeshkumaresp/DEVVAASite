using System;
using System.Linq;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using VAA.BusinessComponents;
using VAA.DataAccess;
using VAA.DataAccess.Model;

namespace VAA.CrewMemberSite
{
    /// <summary>
    /// Menu Viewer - Lanuch the chili proof viewer page - show flash and html based editor
    /// </summary>
    public partial class MenuViewer : System.Web.UI.Page
    {
        readonly CycleManagement _cycleManagement = new CycleManagement();
        readonly MenuManagement _menuManagement = new MenuManagement();
        readonly AccountManagement _accountManagement = new AccountManagement();
        ChiliProcessor chiliProcessor = new ChiliProcessor();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(Session["USERID"])))
                    Response.Redirect("~/Account/Login.aspx");

                if (!IsPostBack)
                {
                    Page.Title = "";
                    string rowId = Convert.ToString(Request.QueryString["ID"]);
                    if (!string.IsNullOrEmpty(rowId))
                    {
                        long id = 0;
                        bool rid = long.TryParse(rowId, out id);
                        if (rid)
                        {
                            Session["CURRENTID"] = id;
                        }
                    }
                    LoadChiliDocument(true);
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                //ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        /// <summary>
        /// Load chili document
        /// </summary>
        /// <param name="isFlashEditor"></param>
        private void LoadChiliDocument(bool isFlashEditor)
        {
            try
            {
                var getEditorUrlResponse = new XmlDocument();

                ChiliProcessor chili = new ChiliProcessor();

                //get the workspace id from database
                var workspaceID = "2ec4fc0a-cc3b-4f57-973a-8cfe5c0c8e4b";

                //get the viewpreference from database
                var viewPrefsID = "";// "f67b53b6-6ad4-4cc9-a1dd-bd1ac8acdd8d";

                long menuId = Convert.ToInt64(Session["CURRENTID"]);

                Session["MENUIDFORNOTIFICATION"] = menuId;

                var menuTemplate = _menuManagement.GetMenuTemplate(menuId);
                var menu = _menuManagement.GetMenuById(menuId);

                //if special meal or allergen guide, donot show chili edit view
                if (menu.MenuTypeId == 6 || menu.MenuTypeId == 7)
                    return;

                workspaceID = WorkspaceAssignmentBasedOnRoles(workspaceID, menu);

                if (menuTemplate != null && !string.IsNullOrEmpty(menuTemplate.ChiliDocumentID))
                {
                    if (!isFlashEditor)
                        getEditorUrlResponse.LoadXml(chili.WebService.DocumentGetHTMLEditorURL(chili.ApiKey, menuTemplate.ChiliDocumentID, workspaceID, viewPrefsID, "", true, true));
                    else
                        getEditorUrlResponse.LoadXml(chili.WebService.DocumentGetEditorURL(chili.ApiKey, menuTemplate.ChiliDocumentID, workspaceID, viewPrefsID, "", true, true));

                    //create url and assign source
                    var editorUrl = getEditorUrlResponse.DocumentElement.GetAttribute("url") + "&d=approve4print.co.uk";

                    if (!isFlashEditor)
                        editorUrl = getEditorUrlResponse.DocumentElement.GetAttribute("url") + "&d=approve4print.co.uk &fullWs=true";

                    //set the iframesource as editor url
                    HtmlControl iframe = (HtmlControl)this.FindControl("iframeChiliProof");

                    if (iframe != null)
                        iframe.Attributes["src"] = editorUrl;
                }

            }
            catch (Exception ex)
            {
                //write to Elma
                //ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private string WorkspaceAssignmentBasedOnRoles(string workspaceID, MenuData menu)
        {
            //Afternoon Tea Card, Breakfast Card, Wine List , ECO                                  
            if (menu.MenuTypeId == 2 || menu.MenuTypeId == 3 || menu.MenuTypeId == 4 || menu.MenuTypeId == 13)
                workspaceID = "b47ad112-a9b3-4931-9b4f-4528deb5f187";

            //food guide
            if (menu.MenuTypeId == 5)
                workspaceID = "35aa3617-6834-4063-bbe7-24eeac8baf20";

            if (menu.MenuTypeId == 1 || menu.MenuTypeId == 10)
                workspaceID = "7e76fa17-4c65-4aa1-845e-ec304cfd5b35";

            //if translator show different workspace ID
            bool isTranslator = true;
            var userId = Convert.ToInt32(Session["USERID"]);

            var userdata = _accountManagement.GetUserTypeByUserid(userId);
            isTranslator = (userdata == "Translator");

            if (isTranslator && menu.LanguageId != 1)
                workspaceID = "61e46bc2-406f-4750-a19d-785c85228904";
            return workspaceID;
        }

        protected void btnFlashEditor_Click(object sender, EventArgs e)
        {
            LoadChiliDocument(true);
        }

        protected void btnHTML5Editor_Click(object sender, EventArgs e)
        {
            LoadChiliDocument(false);
        }



    }
}