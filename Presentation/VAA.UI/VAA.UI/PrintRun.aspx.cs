using System.Web;
using OutputSpreadsheetWriterLibrary;
using System;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VAA.BusinessComponents;
using VAA.CommonComponents;
using VAA.DataAccess;
using Elmah;

namespace VAA.UI
{
    /// <summary>
    /// Displays the current print run info, also creates the order PDF for this print run
    /// </summary>
    public partial class PrintRun : System.Web.UI.Page
    {
        OrderManagement _orderManagement = new OrderManagement();
        AccountManagement _accountManagement = new AccountManagement();
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
                    if (!IsPostBack)
                    {
                        BindGridCurrentOrder();
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
                var currentOrderList = _orderManagement.GetAllOrders();
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
                    var currentOrderList = _orderManagement.GetAllOrders();
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
                else if (e.CommandName.Equals("DownloadPrintRun"))
                {
                    Label lblOrderId = (Label)e.Item.FindControl("lblOrderIdV");
                    Int64 orderId = Convert.ToInt64(lblOrderId.Text);
                    DownloadPrintRun(orderId);
                }
                else if (e.CommandName.Equals("DownloadPrintPDF"))
                {
                    Label lblOrderId = (Label)e.Item.FindControl("lblOrderIdV");
                    Int64 orderId = Convert.ToInt64(lblOrderId.Text);

                    var liveOrderId = _orderManagement.GetLiveOrderIdFromOrderId(orderId);

                    var cycleId = _orderManagement.GetCycleIdOfLiveOrder(liveOrderId);

                    //all upper class
                    _menuProcessor.GeneratePdfForOrder(cycleId, 1, orderId);
                    //all PE class
                    _menuProcessor.GeneratePdfForOrder(cycleId, 2, orderId);
                    //all Eco class
                    _menuProcessor.GeneratePdfForOrder(cycleId, 3, orderId);


                    //send email
                    int userId = Convert.ToInt32(Session["USERID"]);
                    var user = _accountManagement.GetUserById(userId);

                    var PDFMessage = EmailHelper.OrderPdfGenerationCompleteEmailTemplate;
                    PDFMessage = EmailHelper.ConvertMail2(PDFMessage, Convert.ToString(orderId), "\\[ORDERID\\]");

                    EmailHelper.SendMail(user.Username, "ESPAdmin@espcolour.co.uk", "EMMA- PDF Generation for Order", PDFMessage);

                    string notificationEmails = (System.Configuration.ConfigurationManager.AppSettings["NotificationEmails"]);

                    var emails = notificationEmails.Split(new char[] { ';' });

                    foreach (var email in emails)
                    {
                        if (!string.IsNullOrEmpty(email))
                            EmailHelper.SendMail(email, "ESPAdmin@espcolour.co.uk", "EMMA- PDF Generation for Order", PDFMessage);


                    }
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
            if (e.Item is GridEditableItem && e.Item.IsInEditMode)
            {
                GridEditableItem item = e.Item as GridEditableItem;
                DropDownList list = item.FindControl("ddlGVOrderStatus") as DropDownList;
                list.Items.Clear();
                list.Items.Add(new ListItem("--Please select--", "0"));
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
                BindGridCurrentOrder();
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

        private void DownloadPrintRun(long orderId)
        {
            try
            {
                //get the order Id of current row
                PackingTicketProcessor packingTicket = new PackingTicketProcessor();
                var printRunBundleCountDict = packingTicket.CalculatePrintRunData(orderId);

                var lotNo = _orderManagement.GetLotNoFromOrderId(orderId);

                var directory = Directory.GetParent(HttpRuntime.AppDomainAppPath);
                var parentDir = directory.Parent;

                var printRunPath = parentDir.FullName + @"\PrintRunReport\";

                GenerateOutputSpreadsheet.CreatePrintRunSpreadSheet(printRunBundleCountDict, orderId, lotNo, printRunPath);

                FileInfo auditfile = new FileInfo(printRunPath + "PrintRun_" + orderId + ".xlsx");

                if (auditfile.Exists)
                {
                    string fname = auditfile.Name;
                    fname = fname.Replace(" ", "_");
                    Response.ClearContent();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fname));
                    Response.ContentType = "application/ms-excel";
                    Response.TransmitFile(auditfile.FullName);
                    Response.End();
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