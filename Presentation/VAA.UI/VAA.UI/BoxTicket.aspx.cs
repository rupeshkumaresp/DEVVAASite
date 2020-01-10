using System.Web;
using System.Web.UI;
using OutputSpreadsheetWriterLibrary;
using System;
using System.IO;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VAA.BusinessComponents;
using VAA.DataAccess;
using VAA.CommonComponents;
using Elmah;
using System.Threading;

namespace VAA.UI
{
    /// <summary>
    /// Create the Box Ticket spreadsheet for the Order, Generate Box ticket PDFs
    /// </summary>
    public partial class BoxTicket : System.Web.UI.Page
    {
        OrderManagement _orderManagement = new OrderManagement();
        AccountManagement _accountManagement = new AccountManagement();
        MenuProcessor _menuProcessor = new MenuProcessor();

        protected void Page_Load(object sender, EventArgs e)
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
            GridDataItem dataItem = (GridDataItem)e.DetailTableView.ParentItem;
            string orderId = dataItem.GetDataKeyValue("OrderId").ToString();
            e.DetailTableView.DataSource = _orderManagement.GetOrderDetailsbyOrderId(Convert.ToInt32(orderId));
        }

        protected void radGridCurrentOrder_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (IsPostBack)
            {
                var currentOrderList = _orderManagement.GetAllOrders();
                radGridCurrentOrder.DataSource = currentOrderList;
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


                else if (e.CommandName.Equals("GenerateBoxTicket"))
                {
                    Label lblOrderId = (Label)e.Item.FindControl("lblOrderIdV");
                    Int64 orderId = Convert.ToInt64(lblOrderId.Text);

                    Thread thread = new Thread(() => GenerateBoxTicket(orderId));
                    thread.Start();


                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "GeneratePackingPDFNotification", "GeneratePackingPDFNotification();", true);
                }
                else if (e.CommandName.Equals("DownloadBoxTicket"))
                {
                    Label lblOrderId = (Label)e.Item.FindControl("lblOrderIdV");
                    Int64 orderId = Convert.ToInt64(lblOrderId.Text);
                    DownloadBoxTicket(orderId);
                }
                else if (e.CommandName.Equals("GeneratePackingPDF"))
                {
                    Label lblOrderId = (Label)e.Item.FindControl("lblOrderIdV");
                    Int64 orderId = Convert.ToInt64(lblOrderId.Text);
                    GeneratePackingPdfs(orderId);
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
            radGridCurrentOrder.MasterTableView.Rebind();
            BindGridCurrentOrder();
        }

        protected void radAjaxManager_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
        }
        /// <summary>
        /// Generate Box ticket PDF
        /// </summary>
        /// <param name="orderId"></param>
        private void GeneratePackingPdfs(long orderId)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "GeneratePDFInProgress", "GeneratePDFInProgress();", true);

                PackingTicketProcessor packingTicket = new PackingTicketProcessor();
                packingTicket.CreateBoxTicketProofsAndTicketPDF(orderId);

                //send email
                int userId = Convert.ToInt32(Session["USERID"]);
                var user = _accountManagement.GetUserById(userId);

                var packingPDFMessage = EmailHelper.PackingTicketPdfGenerationCompleteEmailTemplate;
                packingPDFMessage = EmailHelper.ConvertMail2(packingPDFMessage, Convert.ToString(orderId), "\\[ORDERID\\]");

                EmailHelper.SendMail(user.Username, "ESPAdmin@espcolour.co.uk", "EMMA- Packing Ticket PDF Generation", packingPDFMessage);


                string notificationEmails = (System.Configuration.ConfigurationManager.AppSettings["NotificationEmails"]);

                var emails = notificationEmails.Split(new char[] { ';' });

                foreach (var email in emails)
                {
                    if (!string.IsNullOrEmpty(email))
                        EmailHelper.SendMail(email, "ESPAdmin@espcolour.co.uk", "EMMA- Packing Ticket PDF Generation", packingPDFMessage);
                }

            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        /// <summary>
        /// Downlaod Box Ticket excel
        /// </summary>
        /// <param name="orderId"></param>
        private void DownloadBoxTicket(long orderId)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "DownloadInProgress", "DownloadInProgress();", true);

                //get the order Id of current row
                var lotNo = _orderManagement.GetLotNoFromOrderId(orderId);

                var directory = Directory.GetParent(HttpRuntime.AppDomainAppPath);
                var parentDir = directory.Parent;

                var boxTicketPath = parentDir.FullName + @"\BoxTicketReport\";

                var boxTicketData = _orderManagement.GetBoxTicketData(orderId);
                GenerateOutputSpreadsheet.CreateBoxTicketSpreadSheet(boxTicketData, orderId, lotNo, boxTicketPath);

                FileInfo auditfile = new FileInfo(boxTicketPath + "BoxReport_" + orderId + ".xlsx");

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


        private void GenerateBoxTicket(long orderId)
        {
            try
            {
                //get the order Id of current row
                PackingTicketProcessor packingTicket = new PackingTicketProcessor();
                packingTicket.CalculateBoxTicketData(orderId);

                //send email
                int userId = Convert.ToInt32(Session["USERID"]);
                var user = _accountManagement.GetUserById(userId);

                var packingPDFMessage = EmailHelper.PackingTicketExcelGenerationCompleteEmailTemplate;
                packingPDFMessage = EmailHelper.ConvertMail2(packingPDFMessage, Convert.ToString(orderId), "\\[ORDERID\\]");

                EmailHelper.SendMail(user.Username, "ESPAdmin@espcolour.co.uk", "EMMA- Packing Ticket Excel Data Generation", packingPDFMessage);

            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
    }
}