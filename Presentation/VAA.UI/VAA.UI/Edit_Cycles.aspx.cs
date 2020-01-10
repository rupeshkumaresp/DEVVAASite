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
    /// Cycle Edit
    /// </summary>
    public partial class Edit_Cycles : System.Web.UI.Page
    {
        CycleManagement _cycleManagement = new CycleManagement();

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
                    ddlYearUpdate.Items.Clear();
                    ddlYearUpdate.Items.Add(new ListItem("-- Please select --", "0"));
                    ddlYearUpdate.SelectedIndex = 0;
                    for (int i = currentYear; i <= endYear; i++)
                    {
                        ddlYearUpdate.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    }

                    string rowId = Convert.ToString(Request.QueryString["ID"]);
                    if (!string.IsNullOrEmpty(rowId))
                    {
                        long Id = 0;
                        bool RID = long.TryParse(rowId, out Id);
                        if (RID)
                        {
                            Session["CURRENTID"] = Id;
                            var cycledata = _cycleManagement.GetCycle(Id);
                            if (cycledata.Id > 0)
                            {
                                int index = 0;
                                index = ddlYearUpdate.Items.IndexOf(ddlYearUpdate.Items.FindByValue(Convert.ToString(cycledata.Year.Trim())));
                                if (index > -1)
                                {
                                    ddlYearUpdate.SelectedIndex = index;
                                }
                                else
                                {
                                    ddlYearUpdate.SelectedIndex = 0;
                                }
                                txtCycleNameUpdate.Text = cycledata.CycleName;
                                txtShortNameUpdate.Text = cycledata.ShortName;
                                radDatePickerStartDateUpdate.SelectedDate = cycledata.StartDate;
                                radDatePickerEndDateUpdate.SelectedDate = cycledata.EndDate;
                            }
                            else
                            {
                                ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CancelEdit();", true);
                            }
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CancelEdit();", true);
                        }
                    }
                    else
                    {
                        ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CancelEdit();", true);
                    }

                }
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

        protected void btnUpdateCycle_Click(object sender, EventArgs e)
        {
            try
            {
                var changeCurrCycle = hfchangeCurrCycle.Value;

                string SID = Convert.ToString(Session["CURRENTID"]);
                string QID = Request.QueryString["ID"];
                if (SID == QID)
                {
                    long Id = 0;
                    bool RID = long.TryParse(SID, out Id);
                    if (RID && Id > 0)
                    {
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
                                        Cycle cycle = new Cycle()
                                        {
                                            Id = Id,
                                            Year = ddlYearUpdate.SelectedItem.Value,
                                            CycleName = txtCycleNameUpdate.Text,
                                            ShortName = txtShortNameUpdate.Text,
                                            StartDate = radDatePickerStartDateUpdate.SelectedDate,
                                            EndDate = radDatePickerEndDateUpdate.SelectedDate,
                                            Active = true
                                        };
                                        bool IsUpdated = _cycleManagement.UpdateCycle(cycle);
                                        if (IsUpdated)
                                        {
                                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "key", "CloseAndRebind();", true);
                                        }
                                        else
                                        {
                                            ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CloseAndRebind();", true);
                                        }
                                    }
                                    else
                                    {
                                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "MsgChangeActiveOrderFalse", "MsgChangeActiveOrderFalse();", true);
                                    }
                                }
                            }
                            else
                            {
                                Cycle cycle = new Cycle()
                                {
                                    Id = Id,
                                    Year = ddlYearUpdate.SelectedItem.Value,
                                    CycleName = txtCycleNameUpdate.Text,
                                    ShortName = txtShortNameUpdate.Text,
                                    StartDate = radDatePickerStartDateUpdate.SelectedDate,
                                    EndDate = radDatePickerEndDateUpdate.SelectedDate,
                                    Active = true
                                };
                                bool IsUpdated = _cycleManagement.UpdateCycle(cycle);
                                if (IsUpdated)
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "key", "CloseAndRebind();", true);
                                }
                                else
                                {
                                    ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CloseAndRebind();", true);
                                }
                            }
                        }
                        else
                        {
                            Cycle cycle = new Cycle()
                            {
                                Id = Id,
                                Year = ddlYearUpdate.SelectedItem.Value,
                                CycleName = txtCycleNameUpdate.Text,
                                ShortName = txtShortNameUpdate.Text,
                                StartDate = radDatePickerStartDateUpdate.SelectedDate,
                                EndDate = radDatePickerEndDateUpdate.SelectedDate,
                                Active = false
                            };
                            bool IsUpdated = _cycleManagement.UpdateCycle(cycle);

                            if (IsUpdated)
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "key", "CloseAndRebind();", true);
                            else
                                ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CloseAndRebind();", true);
                        }
                    }

                    else
                        ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CloseAndRebind();", true);
                }
                else
                    ClientScript.RegisterStartupScript(Page.GetType(), "mykey", "CloseAndRebind();", true);

            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
    }
}