using Elmah;
using System;
using System.Data.SqlTypes;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VAA.DataAccess;
using VAA.DataAccess.Model;


namespace VAA.UI
{
    /// <summary>
    /// Manage Cycle - Add, Edit, Delete
    /// </summary>
    public partial class ManageCycles : Page
    {
        readonly CycleManagement _cycleManagement = new CycleManagement();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(Session["USERID"])))
                    Response.Redirect("~/Account/Login.aspx");

                if (!IsPostBack)
                {
                    var activeCycle = _cycleManagement.GetActiveCycle();
                    if (activeCycle != null)
                    {
                        lblCurrCycleName.Text = activeCycle.CycleName;
                    }
                    int currentYear = DateTime.Now.Year;
                    int endYear = currentYear + 10;
                    ddlYear.Items.Clear();
                    ddlYear.Items.Add(new ListItem("-- Please select --", "0"));
                    ddlYear.SelectedIndex = 0;
                    for (int i = currentYear; i <= endYear; i++)
                    {
                        ddlYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    }
                    GridDataSource();
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void GridDataSource()
        {
            try
            {
                var cycleDataSource = _cycleManagement.GetCycles();
                radGridCycles.DataSource = cycleDataSource;
                radGridCycles.DataBind();
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        public void btnChangeCurrCycle_Click(object sender, EventArgs e)
        {
            lblChangeCurrCycle.Text = "1";
        }
        /// <summary>
        /// Add a new Cycle - Cycle state date - starts on Wednesday and Ends on Tuesday
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddCycle_Click(object sender, EventArgs e)
        {
            try
            {
                var changeCurrCycle = hfchangeCurrCycle.Value;

                DateTime? startDate = radDatePickerStartDate.SelectedDate;
                DateTime? endDate = radDatePickerEndDate.SelectedDate;

                if (startDate == null)
                {
                    startDate = (DateTime)SqlDateTime.Null;
                }

                if (endDate == null)
                {
                    endDate = (DateTime)SqlDateTime.Null;
                }
                if (changeCurrCycle == "1")
                {
                    // get active cycle
                    var activeCycle = _cycleManagement.GetActiveCycle();


                    if (activeCycle != null)// check if there is an active cycle
                    {
                        var activeCycleID = activeCycle.Id;
                        // check if the active cycle has live orders
                        bool isCyclehasliveorders = _cycleManagement.IsCycleHasLiveOrders(activeCycleID);

                        if (isCyclehasliveorders == true)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "MsgHasLiveOrders", "MsgHasLiveOrders();", true);
                        }
                        else
                        {
                            //change curr active cycle to inactive
                            bool inactivateCurrCycle = _cycleManagement.UpdateCyclesActiveState(activeCycleID);

                            if (inactivateCurrCycle == true)
                            {
                                //update new active cycle
                                Cycle cycle = new Cycle()
                                {
                                    InstanceId = 0,
                                    CycleName = txtCycleName.Text,
                                    ShortName = txtShortName.Text,
                                    Year = ddlYear.SelectedItem.Text,
                                    CreatedDatetime = DateTime.Now,
                                    RecordTitlePattern = "",
                                    Active = true,
                                    IsLocked = false,
                                    StartDate = startDate,
                                    EndDate = endDate
                                };

                                var cycledata = _cycleManagement.CreateNewCycle(cycle);
                                ddlYear.SelectedIndex = 0;
                                txtCycleName.Text = "";
                                txtShortName.Text = "";
                                radDatePickerStartDate.Clear();
                                radDatePickerEndDate.Clear();
                                btnCurrentCycle.Checked = false;
                                btnNotCurrentCycle.Checked = true;
                                GridDataSource();
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "MsgChangeActiveOrderFalse", "MsgChangeActiveOrderFalse();", true);
                            }

                        }
                    }
                    else //no active cycle found, Create new active cycle
                    {
                        Cycle cycle = new Cycle()
                        {
                            InstanceId = 0,
                            CycleName = txtCycleName.Text,
                            ShortName = txtShortName.Text,
                            Year = ddlYear.SelectedItem.Text,
                            CreatedDatetime = DateTime.Now,
                            RecordTitlePattern = "",
                            Active = true,
                            IsLocked = false,
                            StartDate = startDate,
                            EndDate = endDate
                        };

                        var cycledata = _cycleManagement.CreateNewCycle(cycle);
                        ddlYear.SelectedIndex = 0;
                        txtCycleName.Text = "";
                        txtShortName.Text = "";
                        radDatePickerStartDate.Clear();
                        radDatePickerEndDate.Clear();
                        btnCurrentCycle.Checked = false;
                        btnNotCurrentCycle.Checked = true;
                        GridDataSource();
                    }
                }
                else
                {
                    Cycle cycle = new Cycle()
                    {
                        InstanceId = 0,
                        CycleName = txtCycleName.Text,
                        ShortName = txtShortName.Text,
                        Year = ddlYear.SelectedItem.Text,
                        CreatedDatetime = DateTime.Now,
                        RecordTitlePattern = "",
                        Active = false,
                        IsLocked = false,
                        StartDate = startDate,
                        EndDate = endDate
                    };

                    var cycledata = _cycleManagement.CreateNewCycle(cycle);
                    ddlYear.SelectedIndex = 0;
                    txtCycleName.Text = "";
                    txtShortName.Text = "";
                    radDatePickerStartDate.Clear();
                    radDatePickerEndDate.Clear();
                    btnCurrentCycle.Checked = false;
                    btnNotCurrentCycle.Checked = true;
                    GridDataSource();
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void radGridCycles_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (IsPostBack)
            {
                var cycleDataSource = _cycleManagement.GetCycles();
                radGridCycles.DataSource = cycleDataSource;
            }
        }

        protected void radGridCycles_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                //e.Item.Selected = true;            
                if (e.CommandName.Equals("RowDelete"))
                {
                    GridDataItem item = e.Item as GridDataItem;
                    if (item != null)
                    {
                        string idRowDelete = item["Id"].Text;
                        Session["DELCYCLEITEM"] = idRowDelete;
                        bool isCyclehasliveorders = _cycleManagement.IsCycleHasLiveOrders(Convert.ToInt64(idRowDelete));
                        if (isCyclehasliveorders == true)
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "MsgCannotDelete", "MsgCannotDelete();", true);
                        }
                        else
                        {
                            string cyclename = item["CycleName"].Text;
                            cyclename = "\"" + cyclename + "\"";
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "confirm-dialog", "ConfirmBootToDeleteCycle(" + cyclename + ");", true);
                        }
                    }
                }
                else if (e.CommandName.Equals("ClearFilter"))
                {
                    foreach (GridColumn column in radGridCycles.MasterTableView.Columns)
                    {
                        column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                        column.CurrentFilterValue = string.Empty;
                    }
                    radGridCycles.MasterTableView.FilterExpression = string.Empty;
                    radGridCycles.MasterTableView.Rebind();
                }
                else if (e.CommandName.Equals("Refresh"))
                {
                    radGridCycles.DataSource = null;
                    GridDataSource();
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void radGridCycles_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = (GridDataItem)e.Item;
                    string rowId = dataItem["Id"].Text;
                    HyperLink editLink = (HyperLink)e.Item.FindControl("EditLink");
                    editLink.Attributes["href"] = "javascript:void(0);";
                    editLink.Attributes["onclick"] = String.Format("return ShowEditForm('{0}','{1}');", rowId, e.Item.ItemIndex);

                    var activeCycle = _cycleManagement.GetActiveCycle();
                    if (activeCycle != null)
                    {
                        var activecycleid = Convert.ToString(activeCycle.Id);
                        if (rowId == activecycleid)
                        {
                            dataItem.BackColor = System.Drawing.Color.LightSalmon;
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

        protected void radAjaxManager_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                radGridCycles.MasterTableView.SortExpressions.Clear();
                radGridCycles.MasterTableView.GroupByExpressions.Clear();
                radGridCycles.Rebind();
            }
            else if (e.Argument == "RebindAndNavigate")
            {
                radGridCycles.MasterTableView.SortExpressions.Clear();
                radGridCycles.MasterTableView.GroupByExpressions.Clear();
                radGridCycles.MasterTableView.CurrentPageIndex = radGridCycles.MasterTableView.PageCount - 1;
                radGridCycles.Rebind();
            }
        }
        protected void btndelcycle_Click(object sender, EventArgs e)
        {
            try
            {
                string cycleId = Convert.ToString(Session["DELCYCLEITEM"]);
                if (!string.IsNullOrEmpty(cycleId))
                {
                    long id = 0;
                    bool isCycleId = long.TryParse(cycleId, out id);
                    if (isCycleId)
                    {
                        bool cycleDeleted = _cycleManagement.DeleteCycle(id);
                    }
                    Session["DELCYCLEITEM"] = string.Empty;
                }
                else
                {
                    Session["DELCYCLEITEM"] = string.Empty;
                }
                radGridCycles.Rebind();
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void btnClearSession_Click(object sender, EventArgs e)
        {
            Session["DELCYCLEITEM"] = string.Empty;
        }

    }
}