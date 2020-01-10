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
    /// Live Order and Live Order Details
    /// Calculate the Live Order Quantity
    /// </summary>
    public partial class LiveOrder : System.Web.UI.Page
    {
        readonly OrderManagement _orderManagement = new OrderManagement();
        readonly CycleManagement _cycleManagement = new CycleManagement();

        MenuProcessor _menuProcessor = new MenuProcessor();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(Session["USERID"])))
                    Response.Redirect("~/Account/Login.aspx");

                string useridstr = Convert.ToString(Session["USERID"]);
                if (!string.IsNullOrEmpty(useridstr))
                {
                    if (!Page.IsPostBack)
                    {
                        var liveOrder = _orderManagement.GetLiveOrder();
                        if (liveOrder == null)
                        {
                            pnlMain.Visible = false;
                            lblNoLiveOrder.Visible = true;
                        }
                        else
                        {
                            BindPage();
                        }

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

        private void BindPage()
        {
            try
            {
                var liveOrder = _orderManagement.GetLiveOrder();
                if (liveOrder != null)
                {
                    //lblLiveOrderIDValue.Text = Convert.ToString(liveOrder.LiveOrderId);
                    lblLotNumberValue.Text = Convert.ToString(liveOrder.LotNo);
                    lblCycleNameValue.Text = _cycleManagement.GetCycle(Convert.ToInt64(liveOrder.CycleId)).CycleName;

                    ddlWeekAndDates.Items.Clear();

                    var weeks = _cycleManagement.GetCycleWeek(Convert.ToInt64(liveOrder.CycleId));

                    foreach (var week in weeks)
                    {
                        ddlWeekAndDates.Items.Add(new ListItem(week, week));
                    }

                    string orderName = "";

                    try
                    {
                        orderName = _orderManagement.GetOrderFriendlyNameFromLiveOrderId(liveOrder.LiveOrderId);
                    }
                    catch { }
                    txOrderName.Text = orderName;
                }
                BindGrid();

            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void BindGrid()
        {
            try
            {
                var data = _orderManagement.GetAllApprovedMenuOnly();//GetAllApprovedMenu();
                radGridLiveOrder.DataSource = data;
                radGridLiveOrder.DataBind();
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        /// <summary>
        /// Order the live orders
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OrderNowBtnClicked(object sender, EventArgs e)
        {
            try
            {
                var value = ddlWeekAndDates.SelectedValue;

                var weekPart = value.Split(new char[] { '(' });

                var dates = weekPart[1].Trim().Replace(")", "");

                var datePart = dates.Split(new char[] { '-' });

                _orderManagement.CreateOrderNow(Convert.ToDateTime(datePart[0]), Convert.ToDateTime(datePart[1]), txOrderName.Text);
                BindPage();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "ShowOrderCreated();", true);
                pnlMain.Visible = false;
                lblNoLiveOrder.Visible = true;
                lblNoLiveOrder.Text = "Order created successfully, please refer to Orders page to see the orders.";
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        /// <summary>
        /// Update the quantity for the live orders
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BtnUpdateQuantityClick(object sender, EventArgs e)
        {
            //now update the quantity for each menu in the live order details table




            try
            {
                var liveOrder = _orderManagement.GetLiveOrder();
                if (liveOrder != null)
                {
                    var value = ddlWeekAndDates.SelectedValue;

                    var weekPart = value.Split(new char[] { '(' });

                    var dates = weekPart[1].Trim().Replace(")", "");

                    var datePart = dates.Split(new char[] { '-' });

                    //validation added to check if flight schedule is present or not?
                    var flightScheuldeValid = true;

                    try
                    {
                        flightScheuldeValid = _menuProcessor.FlightScheduleValidityCheck(liveOrder.LiveOrderId, Convert.ToDateTime(datePart[0]), Convert.ToDateTime(datePart[1]));
                    }
                    catch { }

                    if (flightScheuldeValid)
                    {
                        _menuProcessor.CalculateQuantity(liveOrder.LiveOrderId, Convert.ToDateTime(datePart[0]), Convert.ToDateTime(datePart[1]));
                        BindGrid();

                    }
                    else
                    {
                        //send mail alert

                        var scheduleMailTemplate = EmailHelper.FlightScheduleMailTemplate;

                        string notificationEmails = (System.Configuration.ConfigurationManager.AppSettings["NotificationEmails"]);

                        var emails = notificationEmails.Split(new char[] { ';' });

                        foreach (var email in emails)
                        {
                            if (!string.IsNullOrEmpty(email))
                                EmailHelper.SendMail(email, "ESPAdmin@espcolour.co.uk", "EMMA- Flight Schedule Issue", scheduleMailTemplate);
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void radGridLiveOrder_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (IsPostBack)
            {
                var data = _orderManagement.GetAllApprovedMenu();
                radGridLiveOrder.DataSource = data;
            }
        }

        protected void radGridLiveOrder_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToUpper().Trim().Equals("EDIT"))
                {

                }
                else if (e.CommandName.ToUpper().Trim().Equals("UPDATE"))
                {
                    string id = Convert.ToString(e.CommandArgument);
                    TextBox qtybox = (TextBox)e.Item.FindControl("txtGvQuantity");
                    Label lblMenuId = (Label)e.Item.FindControl("lblGvMenuIdDetailEdit");
                    Int64 menuId = Convert.ToInt64(lblMenuId.Text);
                    string qty = qtybox.Text;
                    if ((!string.IsNullOrEmpty(id)) && (!string.IsNullOrEmpty(qty)))
                    {
                        long longid = 0;
                        bool idconv = long.TryParse(id, out longid);
                        int intqty = 0;
                        bool qconv = Int32.TryParse(qty, out intqty);
                        if (idconv && qconv)
                            _orderManagement.UpdateQuantity(Convert.ToInt64(longid), Convert.ToInt32(intqty));
                    }
                    e.Item.Edit = false;
                    e.Item.OwnerTableView.Rebind();
                    BindGrid();
                }
                else if (e.CommandName.ToUpper().Trim().Equals("CANCEL"))
                {
                }
                else if (e.CommandName == "RowClick" || e.CommandName == "ExpandCollapse")
                {
                    bool lastState = e.Item.Expanded;

                    if (e.CommandName == "ExpandCollapse")
                    {
                        lastState = !lastState;
                    }

                    LiveOrderGridCollapseAllRows();
                    e.Item.Expanded = !lastState;
                }
                else if (e.CommandName.Equals("ClearFilter"))
                {
                    foreach (GridColumn column in radGridLiveOrder.MasterTableView.Columns)
                    {
                        column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                        column.CurrentFilterValue = string.Empty;
                    }
                    radGridLiveOrder.MasterTableView.FilterExpression = string.Empty;
                    radGridLiveOrder.DataSource = null;
                    //radGridLiveOrder.MasterTableView.Rebind();
                    BindGrid();
                }
                else if (e.CommandName.Equals("Refresh"))
                {
                    radGridLiveOrder.DataSource = null;
                    BindGrid();
                }
                else if (e.CommandName.ToUpper().Trim().Equals("REMOVEFROMLIVEORDER"))
                {
                    long menuId = 0;
                    long.TryParse(Convert.ToString(e.CommandArgument), out menuId);
                    if (menuId > 0)
                    {
                        Session["REMOVEFROMLIVEORDERMENUID"] = menuId;
                        string menuCode = "UNDEFINED";

                        Label lblMcode = (Label)e.Item.FindControl("lblGvMenuCode");
                        menuCode = lblMcode.Text;
                        menuCode = "\"" + menuCode + "\"";
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "REMOVELIVEORDER", "ConfirmRemoveFromLiveOrder(" + menuCode + ")", true);
                    }
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void LiveOrderGridCollapseAllRows()
        {
            foreach (GridItem item in radGridLiveOrder.MasterTableView.Items)
            {
                item.Expanded = false;
            }
        }

        protected void radGridLiveOrder_DetailTableDataBind(object sender, GridDetailTableDataBindEventArgs e)
        {
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
            string menuId = dataItem.GetDataKeyValue("MenuId").ToString();
            e.DetailTableView.DataSource = _orderManagement.GetAllApprovedMenuDetails(Convert.ToInt64(menuId));
        }

        protected void btnRemoveFormLiveOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(Session["REMOVEFROMLIVEORDERMENUID"])))
                {
                    long menuId = 0;
                    string strmenuid = Convert.ToString(Session["REMOVEFROMLIVEORDERMENUID"]);
                    long.TryParse(strmenuid, out menuId);
                    if (menuId > 0)
                    {
                        _orderManagement.RemoveMenuFromLiveOrder(menuId);


                        Session["REMOVEFROMLIVEORDERMENUID"] = string.Empty;
                        radGridLiveOrder.DataSource = null;
                        BindGrid();

                        var liveOrder = _orderManagement.GetLiveOrder();
                        if (liveOrder == null)
                        {
                            pnlMain.Visible = false;
                            lblNoLiveOrder.Visible = true;
                        }


                    }
                    else
                    {
                        Session["REMOVEFROMLIVEORDERMENUID"] = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void btnClearSession_Click(object sender, EventArgs e)
        {
            Session["REMOVEFROMLIVEORDERMENUID"] = string.Empty;
        }

        protected void btnRecalculateQuqntity_Click(object sender, EventArgs e)
        {

            var _menuManagement = new MenuManagement();

            _menuProcessor.CalculateQuantity(10207, Convert.ToDateTime("09/11/2016"), Convert.ToDateTime("15/11/2016"));


            //var liveOrderDetails = _orderManagement.GetLiveOrderDetails(10201);

            //foreach (var data in liveOrderDetails)
            //{
            //    var menuid = Convert.ToString(data.MenuId);

            //    //TODO: upadte the LOT NUM variable
            //    _menuProcessor.UpdateLotNoChiliVariable(Convert.ToInt64(menuid));

            //}
            //var boxTicketData = _orderManagement.GetBoxTicketData(10089);

            //foreach (var boxTicket in boxTicketData)
            //{

            //    if (boxTicket.ClassId == 1)
            //    {
            //        //breakfast - menu code
            //        var J_BRKMenuCode = _menuManagement.GetBreakfastMenuCodeFromMainMenuCode(boxTicket.MenuCode, boxTicket.RouteId, boxTicket.FlightNo);

            //        //tea - menu code
            //        var J_TEAMenuCode = _menuManagement.GetTeaMenuCodeFromMainMenuCode(boxTicket.MenuCode, boxTicket.RouteId, boxTicket.FlightNo);

            //        if (!string.IsNullOrEmpty(J_TEAMenuCode))
            //        {
            //            _menuManagement.updateTeaBoxTicketData(boxTicket.ID, J_TEAMenuCode);
            //        }

            //        if (!string.IsNullOrEmpty(J_BRKMenuCode))
            //        {
            //            _menuManagement.updateBRKBoxTicketData(boxTicket.ID, J_BRKMenuCode);
            //        }
            //    }

            //}


        }

    }
}