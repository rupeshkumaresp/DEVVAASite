using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using VAA.BusinessComponents;
using VAA.DataAccess;

namespace VAA.UI
{
    /// <summary>
    /// Rebuild the menu after any change in base item
    /// </summary>
    public partial class RebuildMenu : Page
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
        /// Call rebuild menu for one or more menu codes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRebuildMenu_Click(object sender, EventArgs e)
        {
            try
            {
                var menuCode = txtMenucode.Text;

                var codes = menuCode.Split(new char[] { ',' });

                for (int i = 0; i < codes.Length; i++)
                {
                    if (string.IsNullOrEmpty(codes[i]))
                        continue;

                    //get menu Id
                    var menudata = _menuManagement.GetMenuByMenuCode(codes[i].Trim());

                    if (!string.IsNullOrEmpty(txtTemplateId.Text))
                    {
                        var templateId = _menuManagement.GetTemplateIdByChiliDocumentId(txtTemplateId.Text);
                        var languageId = _menuManagement.GetLanguageIdByChiliDocumentId(txtTemplateId.Text);

                        if (templateId > 0)
                            _menuManagement.UpdateMenuTemplate(menudata.Id, templateId, "");

                        if (languageId > 0)
                            _menuManagement.UpdateMenuLanguage(menudata.Id, languageId);
                    }

                    var menuId = menudata.Id;
                    _menuProcessor.RebuildChiliDocumentForMenu(menuId);

                    var userId = Convert.ToInt32(Session["USERID"]);

                    _menuProcessor.UpdateMenuHistory(new List<long> { menuId }, userId, "Chili template Rebuilt for menu");
                }

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "RebuildCompleted", "RebuildCompleted();", true);
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

    }
}