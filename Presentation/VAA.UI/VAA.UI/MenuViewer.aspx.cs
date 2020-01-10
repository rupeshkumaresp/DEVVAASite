using System;
using System.Linq;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using Elmah;
using VAA.BusinessComponents;
using VAA.DataAccess;
using VAA.DataAccess.Model;

namespace VAA.UI
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

                //btnSendChangeNotification.Attributes["onclick"] = "return ShowNotificationForm();";

                if (!IsPostBack)
                {
                    Page.Title = "";
                    StatusHandling();
                    LoadChiliDocument(true);
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
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

                workspaceID = WorkspaceAssignmentBasedOnRoles(workspaceID, menu);

                var userId = Convert.ToInt32(Session["USERID"]);
                var userdata = _accountManagement.GetUserTypeByUserid(userId);

                if (menuTemplate != null && !string.IsNullOrEmpty(menuTemplate.ChiliDocumentID))
                {
                    if (!isFlashEditor)
                        getEditorUrlResponse.LoadXml(chili.WebService.DocumentGetHTMLEditorURL(chili.ApiKey, menuTemplate.ChiliDocumentID, workspaceID, viewPrefsID, "", false, false));
                    else
                        getEditorUrlResponse.LoadXml(chili.WebService.DocumentGetEditorURL(chili.ApiKey, menuTemplate.ChiliDocumentID, workspaceID, viewPrefsID, "", false, false));

                    if (menu.ApprovalStatusName == "Approved" /*&& userdata != "ESP"*/)
                    {
                        if (!isFlashEditor)
                            getEditorUrlResponse.LoadXml(chili.WebService.DocumentGetHTMLEditorURL(chili.ApiKey, menuTemplate.ChiliDocumentID, workspaceID, viewPrefsID, "", true, true));
                        else
                            getEditorUrlResponse.LoadXml(chili.WebService.DocumentGetEditorURL(chili.ApiKey, menuTemplate.ChiliDocumentID, workspaceID, viewPrefsID, "", true, true));

                    }

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
                ErrorSignal.FromCurrentContext().Raise(ex);
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


        #region StatusHandling

        private void StatusHandling()
        {
            try
            {
                Page.Title = "";
                //Bind Data
                string rowId = Convert.ToString(Request.QueryString["ID"]);
                if (!string.IsNullOrEmpty(rowId))
                {
                    long id = 0;
                    bool rid = long.TryParse(rowId, out id);
                    if (rid)
                    {
                        Session["CURRENTID"] = id;
                        var userId = Convert.ToInt32(Session["USERID"]);
                        var userdata = _accountManagement.GetUserTypeByUserid(userId);
                        if (id > 0)
                        {
                            var menudata = _menuManagement.GetMenuById(id);
                            if (menudata != null)
                            {
                                Page.Title = "Menu Code : " + Convert.ToString(menudata.MenuCode);
                                lblCurrApprovalStatus.Text = menudata.ApprovalStatusName;
                            }
                            if (userdata == "Caterer")
                            {
                                if (menudata.ApprovalStatusName == "Caterer proof")
                                {
                                    changestatusDiv.Visible = true;
                                    ddlChangeStatus.Items.Clear();
                                    ddlChangeStatus.Items.Add(new ListItem("--Please select--", "0"));
                                    var allStatus = _menuManagement.GetAllStatuses();
                                    foreach (var item in allStatus)
                                    {
                                        if (item.ApprovalStatusId == 1 || item.ApprovalStatusId == 4 || item.ApprovalStatusId == 5 || item.ApprovalStatusId == 6)
                                            continue;
                                        ddlChangeStatus.Items.Add(new ListItem(item.ApprovalStatusName, Convert.ToString(item.ApprovalStatusId)));
                                    }
                                }
                                else
                                {
                                    changestatusDiv.Visible = false;
                                    if (menudata.ApprovalStatusName == "Approved")
                                        btnSendChangeNotification.Visible = false;
                                }
                            }
                            else if (userdata == "Translator")
                            {
                                if (menudata.ApprovalStatusName == "Translator proof")
                                {
                                    changestatusDiv.Visible = true;
                                    ddlChangeStatus.Items.Clear();
                                    ddlChangeStatus.Items.Add(new ListItem("--Please select--", "0"));
                                    var allStatus = _menuManagement.GetAllStatuses();
                                    foreach (var item in allStatus)
                                    {
                                        if (item.ApprovalStatusId == 1 || item.ApprovalStatusId == 2 || item.ApprovalStatusId == 3 || item.ApprovalStatusId == 6)
                                            continue;
                                        ddlChangeStatus.Items.Add(new ListItem(item.ApprovalStatusName, Convert.ToString(item.ApprovalStatusId)));
                                    }
                                }
                                else
                                {
                                    changestatusDiv.Visible = false;
                                    if (menudata.ApprovalStatusName == "Approved")
                                        btnSendChangeNotification.Visible = false;
                                }
                            }
                            else
                            {
                                if (menudata.ApprovalStatusName != "Approved")
                                    changestatusDiv.Visible = true;
                                else
                                {
                                    changestatusDiv.Visible = false;
                                    btnSendChangeNotification.Visible = false;
                                }
                                ddlChangeStatus.Items.Clear();
                                ddlChangeStatus.Items.Add(new ListItem("--Please select--", "0"));
                                var allStatus = _menuManagement.GetAllStatuses();
                                foreach (var item in allStatus)
                                {
                                    if (menudata.LanguageId == 1)
                                    {
                                        if (item.ApprovalStatusId == 4 || item.ApprovalStatusId == 5)
                                            continue;
                                    }
                                    ddlChangeStatus.Items.Add(new ListItem(item.ApprovalStatusName, Convert.ToString(item.ApprovalStatusId)));
                                }
                            }

                            if (userdata == "ESP")
                            {
                                changestatusDiv.Visible = true;
                                btnSendChangeNotification.Visible = true;
                            }
                        }
                        else
                        {
                            Page.Title = "";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mykey", "CancelEdit();", true);
                        }
                    }
                    else
                    {
                        Page.Title = "";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "mykey", "CancelEdit();", true);
                    }
                }
                else
                {
                    Page.Title = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mykey", "CancelEdit();", true);
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        //protected void btnUpdateStatus_Click(object sender, EventArgs e)
        //{

        //}

        //[WebMethod]
        //public static bool AjaxUpdateStatus(int ApprovalStatusId, string qid)
        //{
        //    MenuViewer mview = new MenuViewer();
        //    return mview.UpdateStatus(ApprovalStatusId, qid);
        //}

        //public bool UpdateStatus(int ApprovalStatusId, string qid)
        //{
        //    try
        //    {
        //        string sid = Convert.ToString(Session["CURRENTID"]);
        //        if (sid == qid)
        //        {
        //            long id = 0;
        //            bool rid = long.TryParse(sid, out id);
        //            if (rid && id > 0)
        //            {
        //                MenuData newmenu = new MenuData()
        //                {
        //                    Id = id,
        //                    ApprovalStatusId = ApprovalStatusId
        //                };

        //                bool isUpdated = _menuManagement.UpdateStatus(newmenu, Convert.ToInt32(Session["USERID"]));
        //                if (isUpdated)
        //                {
        //                    //var menudata = _menuManagement.GetMenuById(id);                        
        //                    return true;
        //                }
        //                else
        //                {
        //                    Page.Title = "";
        //                    return false;
        //                }
        //            }
        //            else
        //            {
        //                Page.Title = "";
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            Page.Title = "";
        //            return false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //write to Elma
        //        ErrorSignal.FromCurrentContext().Raise(ex);
        //        return false;
        //    }
        //}

        #endregion

        //protected void btnUpdateStatus_Click1(object sender, EventArgs e)
        //{

        //}

        protected void btnUpdateApprovalStatus_Click(object sender, EventArgs e)
        {
            try
            {
                string sid = Convert.ToString(Session["CURRENTID"]);
                string qid = Request.QueryString["ID"];
                if (sid == qid)
                {
                    long id = 0;
                    bool rid = long.TryParse(sid, out id);
                    if (rid && id > 0)
                    {
                        MenuData newmenu = new MenuData()
                        {
                            Id = id,
                            ApprovalStatusId = Convert.ToInt32(ddlChangeStatus.SelectedItem.Value)
                        };
                        bool isUpdated = _menuManagement.UpdateStatus(newmenu, Convert.ToInt32(Session["USERID"]));

                        LoadChiliDocument(true);
                        if (isUpdated)
                        {
                            var menudata = _menuManagement.GetMenuById(id);
                            if (menudata != null)
                            {
                                lblCurrApprovalStatus.Text = menudata.ApprovalStatusName;

                                if (menudata.ApprovalStatusName != "Approved")
                                    changestatusDiv.Visible = true;
                                else
                                {
                                    changestatusDiv.Visible = false;
                                }
                            }
                        }
                        else
                        {
                            Page.Title = "";
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mykey", "CloseAndRebind();", true);
                        }
                    }
                    else
                    {
                        Page.Title = "";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "mykey", "CloseAndRebind();", true);
                    }

                    StatusHandling();
                }
                else
                {
                    Page.Title = "";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "mykey", "CloseAndRebind();", true);
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