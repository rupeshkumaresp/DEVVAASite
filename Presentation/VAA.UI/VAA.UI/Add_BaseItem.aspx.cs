using Elmah;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using VAA.DataAccess;
using VAA.DataAccess.Model;

namespace VAA.UI
{
    /// <summary>
    /// Base items functionality -Add, Edit, Delete base items for Menu
    /// </summary>
    public partial class Add_BaseItem : System.Web.UI.Page
    {
        readonly MenuManagement _menuManagement = new MenuManagement();
        readonly BaseItemManagement _baseItemManagement = new BaseItemManagement();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["USERID"])))
                Response.Redirect("~/Account/Login.aspx");

            if (!IsPostBack)
            {

                try
                {//bind dropdown
                    ddlCategory.Items.Clear();
                    ddlCategory.Items.Add(new ListItem("--Please select--", "0"));
                    var categorylist = _menuManagement.GetAllMenuItemCategory();
                    foreach (var cate in categorylist)
                    {
                        ddlCategory.Items.Add(new ListItem(cate.CategoryName, Convert.ToString(cate.ID)));
                    }

                    int BASEITEMID = 0;

                    string baseitemId = Request.QueryString["ID"];

                    Int32.TryParse(baseitemId, out BASEITEMID);
                    if (BASEITEMID > 0)
                    {
                        btnUpdateBaseItem.Visible = true;
                        lblBaseItemActionType.Text = "Update Base Item";

                        var baseitemData = _baseItemManagement.GetBaseItem(BASEITEMID);
                        if (baseitemData != null)
                        {
                            txtItemTitle.Text = baseitemData.BaseItemTitle;
                            ddlCategory.SelectedIndex = Convert.ToUInt16(baseitemData.CategoryId);
                            txtBaseItemCode.Text = baseitemData.BaseItemCode;
                            txtTitleDesc.Text = baseitemData.BaseItemTitleDescription;
                            txtDesc.Text = baseitemData.BaseItemDescription;
                            txtSubDesc.Text = baseitemData.BaseItemSubDescription;
                            txtAttributes.Text = baseitemData.BaseItemAttributes;

                            txtCategoryCode.Text = _menuManagement.GetMenuItemCategoryCode(Convert.ToUInt16(baseitemData.CategoryId));


                        }
                    }
                    else if (BASEITEMID == 0)
                    {
                        btnAddBaseItem.Visible = true;
                        lblBaseItemActionType.Text = "Add Base Item";
                    }
                    else
                    {
                        btnAddBaseItem.Visible = false;
                        btnUpdateBaseItem.Visible = false;
                        lblBaseItemActionType.Text = "";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "mykey", "CancelEdit();", true);
                    }
                }
                catch (Exception ex)
                {
                    //write to Elma
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }
        }

        /// <summary>
        /// add base item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddBaseItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    int CID = 0;
                    int MTID = 0;
                    int LANID = 0;

                    string classId = Request.QueryString["cid"];
                    string menuTypeId = Request.QueryString["mtid"];
                    string languageId = Request.QueryString["lanid"];

                    if ((!string.IsNullOrEmpty(classId)) && (!string.IsNullOrEmpty(menuTypeId)) && (!string.IsNullOrEmpty(languageId)))
                    {
                        Int32.TryParse(classId, out CID);
                        Int32.TryParse(menuTypeId, out MTID);
                        Int32.TryParse(languageId, out LANID);
                    }
                    if (CID > 0 && MTID > 0 && LANID > 0)
                    {
                        var baseitemCode = txtBaseItemCode.Text;
                        bool checkBaseitemCode = _baseItemManagement.ExistingBaseItem(baseitemCode);
                        if (checkBaseitemCode == true)
                        {
                            Response.Write("<span style='color:#aa2029; text-align:center;'>Base Item Code already in use! Please search and edit the existing item.</span>");
                        }
                        else
                        {
                            BaseItem baseItem = new BaseItem()
                              {
                                  BaseItemCode = txtBaseItemCode.Text,
                                  ClassId = CID,
                                  MenuTypeId = MTID,
                                  LanguageId = LANID,
                                  CategoryId = Convert.ToInt64(ddlCategory.SelectedItem.Value),
                                  BaseItemTitle = txtItemTitle.Text,
                                  BaseItemTitleDescription = txtTitleDesc.Text,
                                  BaseItemDescription = txtDesc.Text,
                                  BaseItemSubDescription = txtSubDesc.Text,
                                  BaseItemAttributes = txtAttributes.Text,
                              };
                            long id = _baseItemManagement.CreateNewBaseItem(baseItem);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "KEY", "CloseAndRebind();", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "KEY", "CancelEdit();", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "KEY", "CancelEdit();", true);
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        /// <summary>
        /// Edit base item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void btnUpdateBaseItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    string baseitemId = Request.QueryString["ID"];
                    int BASEITEMID = 0;

                    if (!string.IsNullOrEmpty(baseitemId))
                    {
                        Int32.TryParse(baseitemId, out BASEITEMID);
                        if (BASEITEMID > 0)
                        {
                            btnUpdateBaseItem.Visible = true;
                            lblBaseItemActionType.Text = "Update Base Item";
                            BaseItem newBaseItem = new BaseItem()
                            {
                                BaseItemId = BASEITEMID,
                                BaseItemCode = txtBaseItemCode.Text,
                                CategoryId = Convert.ToInt64(ddlCategory.SelectedItem.Value),
                                CategoryName = ddlCategory.SelectedItem.Text,
                                BaseItemTitle = txtItemTitle.Text,
                                BaseItemTitleDescription = txtTitleDesc.Text,
                                BaseItemDescription = txtDesc.Text,
                                BaseItemSubDescription = txtSubDesc.Text,
                                BaseItemAttributes = txtAttributes.Text
                            };

                            bool IsSaved = _baseItemManagement.UpdateBaseItem(newBaseItem);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "KEY", "CloseAndRebind();", true);
                        }
                        else
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "userKey", "CloseAndRebind();", true);
                    }
                    else
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "userKey", "CloseAndRebind();", true);
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void ddlCategory_TextChanged(object sender, EventArgs e)
        {
            if (ddlCategory.SelectedItem.Text != "--Please select--")
                txtCategoryCode.Text = _menuManagement.GetMenuItemCategoryCode(Convert.ToUInt16(ddlCategory.SelectedItem.Value));
            else
                txtCategoryCode.Text = "";
        }
    }
}