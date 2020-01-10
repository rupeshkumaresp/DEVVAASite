using Elmah;
using System;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VAA.DataAccess;

namespace VAA.UI
{
    /// <summary>
    /// Orders page - Shows current and past orders, ESPDelivery can change the order status
    /// </summary>
    public partial class Orders : System.Web.UI.Page
    {
        OrderManagement _orderManagement = new OrderManagement();
        AccountManagement _accountManagement = new AccountManagement();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(Session["USERID"])))
                    Response.Redirect("~/Account/Login.aspx");

                string useridstr = Convert.ToString(Session["USERID"]);
                if (!string.IsNullOrEmpty(useridstr))
                {
                    if (!IsPostBack)
                    {
                        Session["IsDeliveryUser"] = "FALSE";
                        int userid = 0;
                        Int32.TryParse(useridstr, out userid);
                        if (userid > 0)
                        {
                            var userdata = _accountManagement.GetUserById(userid);
                            string conUserName = (System.Configuration.ConfigurationManager.AppSettings["AllowedUpdateOrderStatus"]);
                            if (userdata.Username.ToUpper().Trim().Equals(conUserName.ToUpper().Trim()))
                            {
                                Session["IsDeliveryUser"] = "TRUE";
                            }
                        }
                        BindGridCurrentOrder();
                        BindGridPreviousOrder();
                    }
                }
                else
                {
                    Response.Redirect("~/Account/Login.aspx");
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        //Current Order
        private void BindGridCurrentOrder()
        {
            try
            {
                var currentOrderList = _orderManagement.GetAllCurrentOrders();
                radGridCurrentOrder.DataSource = currentOrderList;
                radGridCurrentOrder.DataBind();
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void radGridCurrentOrder_DetailTableDataBind(object source, Telerik.Web.UI.GridDetailTableDataBindEventArgs e)
        {
            try
            {
                GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
                string orderId = dataItem.GetDataKeyValue("OrderId").ToString();
                e.DetailTableView.DataSource = _orderManagement.GetOrderDetailsbyOrderId(Convert.ToInt32(orderId));
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void radGridCurrentOrder_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (IsPostBack)
                {
                    var currentOrderList = _orderManagement.GetAllCurrentOrders();
                    radGridCurrentOrder.DataSource = currentOrderList;
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void radGridCurrentOrder_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
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
                else if (e.CommandName.ToUpper().Trim().Equals("UPDATE"))
                {
                    string isDeliveryuser = Convert.ToString(Session["IsDeliveryUser"]);
                    if (isDeliveryuser.ToUpper().Trim() == "TRUE")
                    {
                        DropDownList ddlStaus = e.Item.FindControl("ddlGVOrderStatus") as DropDownList;
                        int statusId = Convert.ToInt32(ddlStaus.SelectedItem.Value);
                        Int64 OrderRowId = Convert.ToInt64(e.CommandArgument);
                        if (statusId > 0 && OrderRowId > 0)
                        {
                            _orderManagement.UpdateOrderStatus(OrderRowId, statusId);

                            if (statusId == 4)
                            {
                                BindGridPreviousOrder();
                            }
                        }
                    }
                    RebindPage();
                }
                else if (e.CommandName.Equals("ClearFilter"))
                {
                    foreach (GridColumn column in radGridCurrentOrder.MasterTableView.Columns)
                    {
                        column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                        column.CurrentFilterValue = string.Empty;
                    }
                    radGridCurrentOrder.MasterTableView.FilterExpression = string.Empty;
                    radGridCurrentOrder.MasterTableView.Rebind();
                }
                else if (e.CommandName.Equals("Refresh"))
                {
                    foreach (GridColumn column in radGridCurrentOrder.MasterTableView.Columns)
                    {
                        column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                        column.CurrentFilterValue = string.Empty;
                    }
                    radGridCurrentOrder.MasterTableView.FilterExpression = string.Empty;
                    radGridCurrentOrder.DataSource = null;
                    BindGridCurrentOrder();
                }
                else if (e.CommandName.Equals("REORDER"))
                {
                    GridDataItem dataItem = (GridDataItem)e.Item;
                    string rowId = dataItem["OrderId"].Text;
                    RadButton editLink = (RadButton)dataItem.FindControl("btnCurrReorder");
                    editLink.Attributes["href"] = "javascript:void(0);";
                    //editLink.Attributes["onclick"] = String.Format("return ShowCurrReorderForm('{0}','{1}');", rowId, e.Item.ItemIndex);
                    var url = "ReOrder.aspx?ID=" + rowId;

                    Response.Redirect(url);
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void CollapseAllRows()
        {
            foreach (GridItem item in radGridCurrentOrder.MasterTableView.Items)
            {
                item.Expanded = false;
            }
        }
        protected void radGridCurrentOrder_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    GridEditableItem item = e.Item as GridEditableItem;
                    DropDownList list = item.FindControl("ddlGVOrderStatus") as DropDownList;
                    list.Items.Clear();
                    list.Items.Add(new ListItem("--Please select--", "0"));

                    string isDeliveryuser = Convert.ToString(Session["IsDeliveryUser"]);
                    if (isDeliveryuser.ToUpper().Trim() == "TRUE")
                    {
                        Label OrderStatus = item.FindControl("lblOrderStatusEID") as Label;
                        var orderstatuslist = _orderManagement.GetAllOrderStatus();
                        foreach (var ost in orderstatuslist)
                        {
                            list.Items.Add(new ListItem(ost.Status, Convert.ToString(ost.StatusId)));
                        }
                        list.SelectedValue = OrderStatus.Text;
                        radGridCurrentOrder.MasterTableView.GetColumn("EditControl").Display = true;
                    }
                    else
                    {
                        radGridCurrentOrder.MasterTableView.GetColumn("EditControl").Display = false;
                    }


                }
                else if (e.Item is GridDataItem)
                {
                    string isDeliveryuser = Convert.ToString(Session["IsDeliveryUser"]);
                    if (isDeliveryuser.ToUpper().Trim() == "TRUE")
                    {
                        radGridCurrentOrder.MasterTableView.GetColumn("EditControl").Display = true;
                    }
                    else
                    {
                        radGridCurrentOrder.MasterTableView.GetColumn("EditControl").Display = false;
                    }
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }


        //Previous Order
        private void BindGridPreviousOrder()
        {
            try
            {
                var previousOrderList = _orderManagement.GetAllPreviousOrders();
                radGridPreviousOrder.DataSource = previousOrderList;
                radGridPreviousOrder.DataBind();
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void radGridPreviousOrder_DetailTableDataBind(object source, Telerik.Web.UI.GridDetailTableDataBindEventArgs e)
        {
            try
            {
                GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
                string orderId = dataItem.GetDataKeyValue("OrderId").ToString();
                e.DetailTableView.DataSource = _orderManagement.GetOrderDetailsbyOrderId(Convert.ToInt32(orderId));
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void radGridPreviousOrder_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (IsPostBack)
                {
                    var previousOrderList = _orderManagement.GetAllPreviousOrders();
                    radGridPreviousOrder.DataSource = previousOrderList;
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void radGridPreviousOrder_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "RowClick" || e.CommandName == "ExpandCollapse")
                {
                    bool lastState = e.Item.Expanded;

                    if (e.CommandName == "ExpandCollapse")
                    {
                        lastState = !lastState;
                    }

                    PrivousGridCollapseAllRows();
                    e.Item.Expanded = !lastState;
                }

                else if (e.CommandName.Equals("ClearFilter"))
                {
                    foreach (GridColumn column in radGridPreviousOrder.MasterTableView.Columns)
                    {
                        column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                        column.CurrentFilterValue = string.Empty;
                    }
                    radGridPreviousOrder.MasterTableView.FilterExpression = string.Empty;
                    radGridPreviousOrder.MasterTableView.Rebind();
                }
                else if (e.CommandName.Equals("Refresh"))
                {
                    foreach (GridColumn column in radGridPreviousOrder.MasterTableView.Columns)
                    {
                        column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                        column.CurrentFilterValue = string.Empty;
                    }
                    radGridPreviousOrder.MasterTableView.FilterExpression = string.Empty;
                    radGridPreviousOrder.DataSource = null;
                    BindGridPreviousOrder();
                }
                else if (e.CommandName.Equals("REORDER"))
                {
                    GridDataItem dataItem = (GridDataItem)e.Item;
                    string rowId = dataItem["OrderId"].Text;
                    RadButton editLink = (RadButton)dataItem.FindControl("btnReorder");
                    editLink.Attributes["href"] = "javascript:void(0);";
                    //editLink.Attributes["onclick"] = String.Format("return ShowReorderForm('{0}','{1}');", rowId, e.Item.ItemIndex);

                    var url = "ReOrder.aspx?ID=" + rowId;

                    Response.Redirect(url);
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        private void PrivousGridCollapseAllRows()
        {
            foreach (GridItem item in radGridPreviousOrder.MasterTableView.Items)
            {
                item.Expanded = false;
            }
        }
        protected void radGridPreviousOrder_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {

            }

        }
        private void RebindPage()
        {
            Page.Response.Redirect(Page.Request.Url.ToString(), true);
        }
        public void radGridMenu_PageSizeChange(object sender, GridPageSizeChangedEventArgs e)
        {
            try
            {
                radGridCurrentOrder.MasterTableView.Rebind();
                radGridPreviousOrder.MasterTableView.Rebind();
                BindGridCurrentOrder();
                BindGridPreviousOrder();
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void radAjaxManager_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {

        }

    }
}