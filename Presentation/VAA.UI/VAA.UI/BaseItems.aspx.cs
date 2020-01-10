using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VAA.DataAccess;
using VAA.DataAccess.Model;

namespace VAA.UI
{
    /// <summary>
    /// Base items functionality -Add, Edit, Delete base items for Menu
    /// </summary>
    public partial class ManageBaseItems : System.Web.UI.Page
    {
        readonly BaseItemManagement _baseItemManagement = new BaseItemManagement();
        readonly MenuManagement _menuManagement = new MenuManagement();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["USERID"])))
                Response.Redirect("~/Account/Login.aspx");

            if (!IsPostBack)
            {
                BindMenuClass();
                BindMenuType();
                BindLanguage();
            }
            searchBaseItem.DataSource = GetBaseItemsDataSource();
            searchBaseItem.DataTextField = "BaseItemCode";
            searchBaseItem.DataValueField = "BaseItemId";
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

        protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMenuType();
        }

        private void BindMenuType()
        {
            ddlMenuType.Items.Clear();

            var selectedClassId = ddlClass.SelectedValue;

            if (!string.IsNullOrEmpty(selectedClassId))
            {
                var menuTypes = _menuManagement.GetMenuTypeByClass(Convert.ToInt32(selectedClassId));

                foreach (var types in menuTypes)
                {
                    ddlMenuType.Items.Add(new ListItem(types.DisplayName, Convert.ToString(types.ID)));
                }
            }
        }

        private void BindLanguage()
        {
            ddlLanguage.Items.Clear();

            var languages = _baseItemManagement.GetAllLanguages();
            foreach (var language in languages)
            {
                ddlLanguage.Items.Add(new ListItem(language.LanguageCode, Convert.ToString(language.ID)));
            }
        }

        private void GridDataSource()
        {
            var menuclass = ddlClass.SelectedIndex;
            var menutype = ddlMenuType.SelectedIndex;
            var language = ddlLanguage.SelectedIndex;

            var baseItemsDataSource = _baseItemManagement.GetBaseItems(menuclass, menutype, language);
            RadListViewBaseItems.DataSource = baseItemsDataSource;
            RadListViewBaseItems.DataBind();

        }

        public void btnViewMenu_Click(object sender, EventArgs e)
        {
            ListViewDiv.Visible = true;
            RadListViewBaseItems.DataSource = GetBaseItemsDataSource();
            RadListViewBaseItems.DataBind();
        }

        public void searchBaseItem_clicked(object sender, SearchBoxEventArgs e)
        {
            string baseItemCode = searchBaseItem.Text;
            //check for not null
            if (baseItemCode != "")
            {
                var baseItemsDataSource = _baseItemManagement.GetBaseItemList(baseItemCode);
                RadListViewBaseItems.DataSource = baseItemsDataSource;
                RadListViewBaseItems.DataBind();
            }
            else
            {
                RadListViewBaseItems.DataSource = GetBaseItemsDataSource();
                RadListViewBaseItems.DataBind();
            }

        }

        private List<BaseItem> GetBaseItemsDataSource()
        {
            var menuclass = ddlClass.SelectedValue;
            var menutype = ddlMenuType.SelectedValue;
            var language = ddlLanguage.SelectedValue;

            var baseItemsDataSource = _baseItemManagement.GetBaseItems(Convert.ToInt32(menuclass), Convert.ToInt32(menutype), Convert.ToInt32(language));
            return baseItemsDataSource;
        }

        protected void RadListView1_NeedDataSource(object sender, RadListViewNeedDataSourceEventArgs e)
        {
            if (IsPostBack)
            {
                RadListViewBaseItems.DataSource = GetBaseItemsDataSource();
            }
        }

        [WebMethod]
        public static bool DeleteBaseItem()
        {
            ManageBaseItems baseItemObj = new ManageBaseItems();
            return baseItemObj.DeleteBaseItemRow();
        }

        public bool DeleteBaseItemRow()
        {
            try
            {
                string baseItemId = Convert.ToString(Session["DELBASEITEM"]);
                if (!string.IsNullOrEmpty(baseItemId))
                {
                    long id = 0;
                    bool isBaseItemId = long.TryParse(baseItemId, out id);
                    if (isBaseItemId)
                    {
                        bool baseItemDeleted = _baseItemManagement.DeleteBaseItem(id);
                    }
                    Session["DELBASEITEM"] = string.Empty;
                    return true;
                }
                else
                {
                    Session["DELBASEITEM"] = string.Empty;
                    return false;
                }

            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
                return false;
            }
        }

        protected void rblBaseitemSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList rbl = sender as RadioButtonList;
            RadComboBox combo = RadListViewBaseItems.FindControl("ddListBaseitemSort") as RadComboBox;
            if (combo != null && (combo.SelectedItem.Value != String.Empty && combo.SelectedItem.Value != "ClearSort"))
            {
                switch (rbl.SelectedValue)
                {
                    case "ASC":
                        RadListViewBaseItems.Items[0].FireCommandEvent(RadListView.SortCommandName, combo.SelectedValue + " ASC");
                        break;
                    case "DESC":
                        RadListViewBaseItems.Items[0].FireCommandEvent(RadListView.SortCommandName, combo.SelectedValue + " DESC");
                        break;
                    default:
                        break;
                }
            }
        }

        protected void ddListBaseitemSort_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadioButtonList rbl = RadListViewBaseItems.FindControl("rblBaseitemSort") as RadioButtonList;
            switch (e.Value)
            {
                case "BaseItemId":
                    RadListViewBaseItems.Items[0].FireCommandEvent(RadListView.SortCommandName, "BaseItemId");
                    rbl.SelectedIndex = 0;
                    break;
                case "BaseItemCode":
                    RadListViewBaseItems.Items[0].FireCommandEvent(RadListView.SortCommandName, "BaseItemCode");
                    rbl.SelectedIndex = 0;
                    break;
                case "CategoryName":
                    RadListViewBaseItems.Items[0].FireCommandEvent(RadListView.SortCommandName, "CategoryName");
                    rbl.SelectedIndex = 0;
                    break;
                case "ClearSort":
                    RadListViewBaseItems.SortExpressions.Clear();
                    RadListViewBaseItems.Rebind();
                    rbl.SelectedIndex = 0;
                    break;
                default:
                    break;
            }
        }
        protected void RadListViewBaseItems_ItemCommand(object sender, RadListViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToUpper() == "UPDATE")
                {
                    TextBox Title = e.ListViewItem.FindControl("txtBoxItemTitle") as TextBox;
                    DropDownList CategoryName = e.ListViewItem.FindControl("ddlCategory") as DropDownList;
                    TextBox BaseitemCode = e.ListViewItem.FindControl("txtBaseItemCode") as TextBox;
                    TextBox TitleDescription = e.ListViewItem.FindControl("txtBoxTitleDesc") as TextBox;
                    TextBox Description = e.ListViewItem.FindControl("txtBoxDesc") as TextBox;
                    TextBox SubDescription = e.ListViewItem.FindControl("txtBoxSubDesc") as TextBox;
                    TextBox ItemAttributes = e.ListViewItem.FindControl("txtBoxAttributes") as TextBox;

                    BaseItem baseItem = new BaseItem()
                    {
                        BaseItemId = Convert.ToInt64(e.CommandArgument),
                        BaseItemCode = BaseitemCode.Text,
                        CategoryId = Convert.ToInt64(CategoryName.SelectedItem.Value),
                        CategoryName = CategoryName.SelectedItem.Text,
                        BaseItemTitle = Title.Text,
                        BaseItemTitleDescription = TitleDescription.Text,
                        BaseItemDescription = Description.Text,
                        BaseItemSubDescription = SubDescription.Text,
                        BaseItemAttributes = ItemAttributes.Text
                    };

                    bool IsSaved = _baseItemManagement.UpdateBaseItem(baseItem);
                    RadListViewBaseItems.DataSource = GetBaseItemsDataSource();
                    RadListViewBaseItems.DataBind();
                }
                else if (e.CommandName.ToUpper().Equals("DELETE"))
                {
                    string baseItemId = Convert.ToString(e.CommandArgument);
                    Session["DELBASEITEM"] = baseItemId;
                    radWindowManager.RadConfirm("Are you sure you want to delete this base item, please confirm?", "confirmCallBackUserFn", 300, 150, null, "Delete Confirmation", baseItemId);
                }
                else if (e.CommandName.ToUpper().Equals("REFRESH"))
                {
                    RadListViewBaseItems.DataSource = null;
                    RadListViewBaseItems.DataSource = GetBaseItemsDataSource();
                    RadListViewBaseItems.DataBind();
                }
                else if (e.CommandName.ToUpper().Equals("INSERT"))
                {
                    string classId = ddlClass.SelectedItem.Value;
                    string menuTypeId = ddlClass.SelectedItem.Value;
                    string languageId = ddlLanguage.SelectedItem.Value;

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "KEY", "ShowInsertForm(" + classId + "," + menuTypeId + "," + languageId + ");", true);
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void RadListViewBaseItems_ItemDataBound(object sender, RadListViewItemEventArgs e)
        {
            try
            {
                if (e.Item is RadListViewEditableItem && e.Item.IsInEditMode)
                {
                    RadListViewEditableItem item = e.Item as RadListViewEditableItem;
                    DropDownList list = item.FindControl("ddlCategory") as DropDownList;
                    var categorylist = _menuManagement.GetAllMenuItemCategory();
                    list.Items.Clear();
                    list.Items.Add(new ListItem("--Please select--", "0"));
                    foreach (var cate in categorylist)
                    {
                        list.Items.Add(new ListItem(cate.CategoryName, Convert.ToString(cate.ID)));
                    }
                    Label strcatid = e.Item.FindControl("lblBoxCategoryId") as Label;
                    list.SelectedValue = strcatid.Text;
                }
                if (e.Item is RadListViewDataItem)
                {
                    var dataItem = ((RadListViewDataItem)e.Item).DataItem;
                    Label lblId = e.Item.FindControl("lblBaseitemId") as Label;

                    RadButton editLink = (RadButton)e.Item.FindControl("btnEditBaseItem");
                    editLink.Attributes["href"] = "javascript:void(0);";
                    editLink.Attributes["onclick"] = String.Format("return ShowEditForm('{0}');", lblId.Text);
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void RadAjaxManagerBaseItem_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                RadListViewBaseItems.Rebind();
            }
        }
    }
}