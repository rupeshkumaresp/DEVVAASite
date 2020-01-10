using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Elmah;
using VAA.DataAccess;
using VAA.BusinessComponents;
using Telerik.Web.UI;
using VAA.CommonComponents;
using System.Threading;

namespace VAA.UI
{
    /// <summary>
    /// Reorder menu - Handling three workflow Straight Reprint , Re-order with Schedule update , Re-order with Menu update
    /// </summary>
    public partial class ReOrder : System.Web.UI.Page
    {
        readonly CycleManagement _cycleManagement = new CycleManagement();
        readonly OrderManagement _orderManagement = new OrderManagement();
        readonly MenuManagement _menuManagement = new MenuManagement();
        readonly AccountManagement _accountManagement = new AccountManagement();

        MenuProcessor _menuProcessor = new MenuProcessor();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(Session["USERID"])))
                    Response.Redirect("~/Account/Login.aspx");

                if (!Page.IsPostBack)
                {
                    int ORDERID = 0;

                    string orderId = Request.QueryString["ID"];

                    Int32.TryParse(orderId, out ORDERID);

                    if (ORDERID > 0)
                    {
                        lblCurrOrderId.Text = Convert.ToString(ORDERID);
                        hfchangeoption.Value = "0";
                        StraightReprintCol.Visible = false;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "mykey", "CancelRoroder();", true);
                    }
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        public void btngetCurrCycle_Click(object sender, EventArgs e)
        {
            try
            {
                StraightReprintCol.Visible = true;
                ddlWeekAndDates.Items.Clear();
                var hiddenfieldOption = hfchangeoption.Value;

                if (hiddenfieldOption == "1")
                {
                    // get active cycle
                    var activeCycle = _cycleManagement.GetActiveCycle();
                    if (activeCycle != null)// check if there is an active cycle
                    {
                        lblCurrCycleName.Text = activeCycle.CycleName;
                        var activeCycleID = activeCycle.Id;
                        var weeks = _cycleManagement.GetCycleWeek(Convert.ToInt64(activeCycleID));

                        foreach (var week in weeks)
                        {
                            ddlWeekAndDates.Items.Add(new ListItem(week, week));
                        }
                    }
                    else
                    {
                        lblNoActiveCycle.Text = "There is no active Cycle found. Please create a new active cyle to proceed.";
                    }
                }
                else
                {
                    StraightReprintCol.Visible = false;
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        public void btnMoveMenuPnl_Click(object sender, EventArgs e)
        {
           
        }
        public void btnHidePanels_Click(object sender, EventArgs e)
        {
            StraightReprintCol.Visible = false;
        }

        public List<string> GetSelectedMenus(RadListBox menuList)
        {
            List<string> checkedMenulist = new List<string>();

            IList<RadListBoxItem> collection = menuList.CheckedItems;
            foreach (RadListBoxItem item in collection)
            {
                checkedMenulist.Add(item.DataKey.ToString());
            }
            return checkedMenulist;
        }
        public List<string> GetUnselectedMenus(RadListBox menuList)
        {
            List<string> uncheckedMenulist = new List<string>();

            IList<RadListBoxItem> collection = menuList.Items;
            IList<RadListBoxItem> checkedlist = menuList.CheckedItems;
            foreach (RadListBoxItem item in collection)
            {
                foreach (RadListBoxItem checkeditem in checkedlist)
                {
                    if (item != checkeditem)
                    {
                        uncheckedMenulist.Add(item.DataKey.ToString());
                    }
                }
            }
            return uncheckedMenulist.Distinct().ToList();

        }
        public void btnReOrder_Click(object sender, EventArgs e)
        {
            try
            {
                var orderName = txOrderName.Text;

                if (string.IsNullOrEmpty(orderName))
                    return;

                var hiddenfieldOption = hfchangeoption.Value;

                if (hiddenfieldOption == "1")//straight reprint
                {
                    var currOrderId = lblCurrOrderId.Text;

                    var newliveorderdetails = _orderManagement.CreateReorderFromLiveOrder(Convert.ToInt64(currOrderId));

                    if (newliveorderdetails != null)
                    {
                        // calculate quantity for the new live order
                        var value = ddlWeekAndDates.SelectedValue;

                        var weekPart = value.Split(new char[] { '(' });

                        var dates = weekPart[1].Trim().Replace(")", "");

                        var datePart = dates.Split(new char[] { '-' });

                        _menuProcessor.CalculateQuantity(newliveorderdetails.LiveOrderId, Convert.ToDateTime(datePart[0]), Convert.ToDateTime(datePart[1]));
                        // set the is converted to order column to true in live orders table 
                        _orderManagement.CreateReOrderNow(Convert.ToInt64(newliveorderdetails.LiveOrderId), Convert.ToDateTime(datePart[0]), Convert.ToDateTime(datePart[1]), orderName);

                        //update reordercount
                        List<string> reorderedMenuIds = new List<string>();
                        var liveOrderDetails = _orderManagement.GetLiveOrderDetails(newliveorderdetails.LiveOrderId);
                        foreach (var data in liveOrderDetails)
                        {
                            var menuid = Convert.ToString(data.MenuId);

                            //TODO: upadte the LOT NUM variable
                            _menuProcessor.UpdateLotNoChiliVariable(Convert.ToInt64(menuid));
                            if (!reorderedMenuIds.Contains(menuid))
                            {
                                reorderedMenuIds.Add(menuid);
                            }
                        }
                        _orderManagement.UpdateReOrderCount(reorderedMenuIds);
                    }

                    divProcessdone.Visible = true;
                    divReorderOptions.Visible = false;

                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "MsgMovedToOrder", "MsgMovedToOrder();", true);

                    var userId = Convert.ToInt32(Session["USERID"]);
                    var user = _accountManagement.GetUserById(userId);

                    var reorderMessage = EmailHelper.ReorderOption1EmailTemplate;

                    EmailHelper.SendMail(user.Username, "ESPAdmin@espcolour.co.uk", "EMMA- Re-Order - Straight Reprint", reorderMessage);

                    string notificationEmails = (System.Configuration.ConfigurationManager.AppSettings["NotificationEmails"]);

                    var emails = notificationEmails.Split(new char[] { ';' });

                    foreach (var email in emails)
                    {
                        if (!string.IsNullOrEmpty(email))
                            EmailHelper.SendMail(email, "ESPAdmin@espcolour.co.uk", "EMMA- Re-Order- Straight Reprint", reorderMessage);
                    }



                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "KEY", "CloseAndRebind();", true);
                }
                else if (hiddenfieldOption == "2")//schedule update
                {
                    // see if there is a current live order
                    var currLiveorder = _orderManagement.GetLiveOrder();
                    if (currLiveorder != null)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "MsgHasLiveOrders", "MsgHasLiveOrders();", true);
                    }
                    else
                    {
                        var currOrderId = lblCurrOrderId.Text;
                        var newliveorderdetails = _orderManagement.CreateReorderFromLiveOrder(Convert.ToInt64(currOrderId));
                        if (newliveorderdetails != null)
                        {
                            //update reordercount
                            List<string> reorderedMenuIds = new List<string>();
                            var liveOrderDetails = _orderManagement.GetLiveOrderDetails(newliveorderdetails.LiveOrderId);
                            foreach (var data in liveOrderDetails)
                            {
                                var menuid = Convert.ToString(data.MenuId);
                                //TODO: upadte the LOT NUM variable
                                _menuProcessor.UpdateLotNoChiliVariable(Convert.ToInt64(menuid));
                                if (!reorderedMenuIds.Contains(menuid))
                                {
                                    reorderedMenuIds.Add(menuid);
                                }
                            }
                            _orderManagement.UpdateReOrderCount(reorderedMenuIds);

                            divProcessdone.Visible = true;
                            divReorderOptions.Visible = false;

                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "MsgMovedToLiveOrder", "MsgMovedToLiveOrder();", true);


                            //send email
                            var userId = Convert.ToInt32(Session["USERID"]);
                            var user = _accountManagement.GetUserById(userId);

                            var reorderMessage = EmailHelper.ReorderOption2EmailTemplate;


                            EmailHelper.SendMail(user.Username, "ESPAdmin@espcolour.co.uk", "EMMA- Re-Order with schedule update", reorderMessage);

                            string notificationEmails = (System.Configuration.ConfigurationManager.AppSettings["NotificationEmails"]);

                            var emails = notificationEmails.Split(new char[] { ';' });

                            foreach (var email in emails)
                            {
                                if (!string.IsNullOrEmpty(email))
                                    EmailHelper.SendMail(email, "ESPAdmin@espcolour.co.uk", "EMMA- Re-Order with schedule update", reorderMessage);
                            }


                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "MsgMoveFailed", "MsgMoveFailed();", true);
                        }
                    }
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "KEY", "CloseAndRebind();", true);
                }
                else if (hiddenfieldOption == "3")//Menu Update
                {
                    var currLiveorder = _orderManagement.GetLiveOrder();
                    if (currLiveorder != null)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "MsgHasLiveOrders", "MsgHasLiveOrders();", true);
                    }
                    else
                    {

                        Thread newThread = new Thread(new ThreadStart(ReorderAllMenuWithMenuUpdate));
                        newThread.Start();

                        btnReOrder.Enabled = false;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "MsgMenuUpdate", "MsgMenuUpdate();", true);


                    }
                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "KEY", "CloseAndRebind();", true);
                }
                else if (hiddenfieldOption == "0")// select option message
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "MsgSelectOption", "MsgSelectOption();", true);
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void ReorderAllMenuWithMenuUpdate()
        {
            var movedtoliveorderCount = 0;

            var currOrderId = lblCurrOrderId.Text;
            var orderdetails = _orderManagement.GetMenuDetailsbyOrderId(Convert.ToInt32(currOrderId));

            //Selected Menus - recreate old menu
            var reCreatedMenuCount = 0;

            var userId = Convert.ToInt32(Session["USERID"]);
            List<long> newMenuIds = new List<long>();

            foreach (var order in orderdetails)
            {

                var createNewMenu = _orderManagement.CreateReorderMenuFromMenuid(order.MenuId, userId);

                _menuManagement.UpdateMenuHistory(order.MenuId, userId, "ReOrder Duplicate Menu Created - with same chili document as old menu");

                _menuProcessor.CreateChiliDocumentForReOrderMenuBymenuid(order.MenuId);

                _menuProcessor.UpdateLotNoChiliVariable(order.MenuId);

                _menuManagement.UpdateMenuHistory(order.MenuId, userId, "ReOrder Duplicate Menu Created - New Chili document Id created");

                newMenuIds.Add(createNewMenu);
                if (createNewMenu != 0)
                {
                    reCreatedMenuCount = reCreatedMenuCount + 1;
                }

            }

            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "MsgOrderReCreted", "MsgOrderReCreted(" + movedtoliveorderCount + "," + reCreatedMenuCount + ");", true);

            divProcessdone.Visible = true;
            divReorderOptions.Visible = false;


            //send email
            var user = _accountManagement.GetUserById(userId);

            var reorderMessage = EmailHelper.ReorderOption3EmailTemplate;

            reorderMessage = EmailHelper.ConvertMail2(reorderMessage, Convert.ToString(movedtoliveorderCount), "\\[MOVEDTOLIVEORDER\\]");
            reorderMessage = EmailHelper.ConvertMail2(reorderMessage, Convert.ToString(reCreatedMenuCount), "\\[RECREAREDFORREORDER\\]");

            EmailHelper.SendMail(user.Username, "ESPAdmin@espcolour.co.uk", "EMMA- Re-Order with menu update", reorderMessage);

            string notificationEmails = (System.Configuration.ConfigurationManager.AppSettings["NotificationEmails"]);

            var emails = notificationEmails.Split(new char[] { ';' });

            foreach (var email in emails)
            {
                if (!string.IsNullOrEmpty(email))
                    EmailHelper.SendMail(email, "ESPAdmin@espcolour.co.uk", "EMMA- Re-Order with menu update", reorderMessage);
            }
        }

        protected void btlCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("Orders.aspx");
        }

    }
}