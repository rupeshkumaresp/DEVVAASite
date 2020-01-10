using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Elmah;
using Telerik.Web.UI;
using VAA.DataAccess;

namespace VAA.UI
{
    public partial class Home : Page
    {
        readonly CycleManagement _cycleManagement = new CycleManagement();
        readonly MenuManagement _menuManagement = new MenuManagement();
        readonly AccountManagement _accountManagement = new AccountManagement();


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(Session["USERID"])))
                    Response.Redirect("~/Account/Login.aspx");

                if (!Page.IsPostBack)
                {
                    BindCycle();
                    BindMenuClass();
                    BindMenuType();
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        public void BtnViewMenuClick(object sender, EventArgs e)
        {
            MenuGridDiv.Visible = true;
            GridDataSource();
        }

        #region bind Dropdowns

        private void BindMenuClass()
        {
            ddlMenuClass.Items.Clear();

            var classes = _menuManagement.GetAllClass();

            foreach (var menuclass in classes)
            {
                ddlMenuClass.Items.Add(new ListItem(menuclass.FlightClass, Convert.ToString(menuclass.ID)));
            }
        }
        private void BindMenuType()
        {
            ddlMenuType.Items.Clear();

            var selectedClassId = ddlMenuClass.SelectedValue;

            if (!string.IsNullOrEmpty(selectedClassId))
            {
                var menuTypes = _menuManagement.GetMenuTypeByClass(Convert.ToInt32(selectedClassId));

                foreach (var types in menuTypes)
                {
                    ddlMenuType.Items.Add(new ListItem(types.DisplayName, Convert.ToString(types.ID)));
                }
            }
        }
        private void BindCycle()
        {
            var cycles = _cycleManagement.GetCycles();

            ddlCycle.Items.Clear();
            foreach (var cycle in cycles)
            {
                ddlCycle.Items.Add(new ListItem(cycle.CycleName, Convert.ToString(cycle.Id)));
            }
        }
        protected void ddlMenuClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMenuType();
        }
        #endregion


        #region grid events

        private void GridDataSource()
        {
            try
            {
                var cycle = ddlCycle.SelectedValue;
                var menuclass = ddlMenuClass.SelectedValue;
                var menutype = ddlMenuType.SelectedValue;
                var userId = Convert.ToInt32(Session["USERID"]);
                var userdata = _accountManagement.GetUserTypeByUserid(userId);
                //if (userdata == "ESP")
                //{
                var menuDataSource = _menuManagement.GetMenuByCycleClassAndMenutype(Convert.ToInt32(cycle), Convert.ToInt16(menuclass), Convert.ToInt16(menutype));
                radGridMenu.DataSource = menuDataSource;
                radGridMenu.DataBind();
                //}
                //else
                //{
                //    var menuDataSource = _menuManagement.GetMenuByCycleClassMenutypeAndUserid(userId, Convert.ToInt32(cycle), Convert.ToInt16(menuclass), Convert.ToInt16(menutype));
                //    radGridMenu.DataSource = menuDataSource;
                //    radGridMenu.DataBind();
                //}
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

        }

        protected void radGridMenu_DetailTableDataBind(object source, Telerik.Web.UI.GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
            string menuId = dataItem.GetDataKeyValue("ID").ToString();
            e.DetailTableView.DataSource = _menuManagement.GetRoutesByMenu(Convert.ToInt32(menuId));
        }
        protected void radGridMenu_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (IsPostBack)
            {
                var cycle = ddlCycle.SelectedValue;
                var menuclass = ddlMenuClass.SelectedValue;
                var menutype = ddlMenuType.SelectedValue;
                var userId = Convert.ToInt32(Session["USERID"]);
                var userdata = _accountManagement.GetUserTypeByUserid(userId);
                //if (userdata == "ESP")
                //{
                var menuDataSource = _menuManagement.GetMenuByCycleClassAndMenutype(Convert.ToInt32(cycle), Convert.ToInt16(menuclass), Convert.ToInt16(menutype));
                radGridMenu.DataSource = menuDataSource;
                //radGridMenu.DataBind();
                //}
                //else
                //{
                //    var menuDataSource = _menuManagement.GetMenuByCycleClassMenutypeAndUserid(userId, Convert.ToInt32(cycle), Convert.ToInt16(menuclass), Convert.ToInt16(menutype));
                //    radGridMenu.DataSource = menuDataSource;
                //    //radGridMenu.DataBind();
                //}
            }
        }

        protected void radGridMenu_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "RowClick" || e.CommandName == "ExpandCollapse")
            {
                bool lastState = e.Item.Expanded;

                if (e.CommandName == "ExpandCollapse")
                {
                    lastState = !lastState;
                }

                CollapseAllRows();
                e.Item.Expanded = !lastState;
            }
            else if (e.CommandName.Equals("ClearFilter"))
            {
                foreach (GridColumn column in radGridMenu.MasterTableView.Columns)
                {
                    column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.CurrentFilterValue = string.Empty;
                }
                radGridMenu.MasterTableView.FilterExpression = string.Empty;
                radGridMenu.MasterTableView.Rebind();
                radGridMenu.DataSource = null;
                GridDataSource();
            }
            else if (e.CommandName.Equals("Refresh"))
            {
                radGridMenu.DataSource = null;
                GridDataSource();
            }
            else if (e.CommandName == RadGrid.FilterCommandName)
            {
                MenuGridDiv.Visible = true;
                GridDataSource();
            }

        }
        private void CollapseAllRows()
        {
            foreach (GridItem item in radGridMenu.MasterTableView.Items)
            {
                item.Expanded = false;
            }
        }

        #endregion
        public void radGridMenu_PageSizeChange(object sender, GridPageSizeChangedEventArgs e)
        {
            radGridMenu.MasterTableView.Rebind();
            GridDataSource();
        }

        protected void radAjaxManager_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                radGridMenu.MasterTableView.SortExpressions.Clear();
                radGridMenu.MasterTableView.GroupByExpressions.Clear();
                radGridMenu.Rebind();
            }
            else if (e.Argument == "RebindAndNavigate")
            {
                radGridMenu.MasterTableView.SortExpressions.Clear();
                radGridMenu.MasterTableView.GroupByExpressions.Clear();
                radGridMenu.MasterTableView.CurrentPageIndex = radGridMenu.MasterTableView.PageCount - 1;
                radGridMenu.Rebind();
            }
        }

        protected void radGridMenu_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = (GridDataItem)e.Item;
                try
                {
                    string rowId = dataItem["Id"].Text;
                    RadButton editLink = (RadButton)dataItem.FindControl("ViewMenuLink");
                    editLink.Attributes["href"] = "javascript:void(0);";
                    editLink.Attributes["onclick"] = String.Format("return ShowEditForm('{0}','{1}');", rowId, e.Item.ItemIndex);

                    if (dataItem["MenuName"].Text.Length > 50)
                    {
                        string strValue = dataItem["MenuName"].Text;
                        int strLen = dataItem["MenuName"].Text.Length;
                        int intSection = (strLen - (strLen % 50)) / 50;
                        while (intSection > 0)
                        {
                            strValue = strValue.Substring(0, intSection * 50) + "<br/>" + strValue.Substring(intSection * 50);
                            intSection -= 1;

                        }
                    }
                }
                catch
                {
                }

                try
                {
                    string rowId = dataItem["Id"].Text;
                    RadButton editLink = (RadButton)dataItem.FindControl("ViewPDFLink");
                    editLink.Attributes["href"] = "javascript:void(0);";
                    editLink.Attributes["onclick"] = String.Format("return ShowMenuPDFForm('{0}','{1}');", rowId, e.Item.ItemIndex);

                    if (dataItem["MenuName"].Text.Length > 50)
                    {
                        string strValue = dataItem["MenuName"].Text;
                        int strLen = dataItem["MenuName"].Text.Length;
                        int intSection = (strLen - (strLen % 50)) / 50;
                        while (intSection > 0)
                        {
                            strValue = strValue.Substring(0, intSection * 50) + "<br/>" + strValue.Substring(intSection * 50);
                            intSection -= 1;

                        }
                    }
                }
                catch
                {
                }
            }
        }

    }
}