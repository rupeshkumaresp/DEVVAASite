using Elmah;
using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VAA.BusinessComponents;
using VAA.DataAccess;

namespace VAA.UI
{
    /// <summary>
    /// Generate Audit reprot Excel for the Menu(s)
    /// </summary>
    public partial class GenerateAudit : Page
    {
        readonly CycleManagement _cycleManagement = new CycleManagement();
        readonly MenuManagement _menuManagement = new MenuManagement();
        readonly RouteManagement _routeManagement = new RouteManagement();
        MenuProcessor _menuProcessor = new MenuProcessor();

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
                    BindFlightFrom();
                    BindFlightTo();
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        public void GenerateAuditBtnClicked(object sender, EventArgs e)
        {
            try
            {
                int menuTypeId = -1;

                if (ddlMenuType.SelectedValue != "All")
                    menuTypeId = Convert.ToInt32(ddlMenuType.SelectedValue);

                AuditTrailGeneration audit = new AuditTrailGeneration();
                audit.GenerateAuditTrail(Convert.ToInt64(ddlCycle.SelectedValue), Convert.ToInt32(ddlClass.SelectedValue), menuTypeId, ddlFlightFrom.SelectedItem.Text, ddlFlightTo.SelectedItem.Text);

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "MessageAuditGenerationComplete", "AuditGenerationComplete();", true);
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        public void DownloadAuditBtnClicked(object sender, EventArgs e)
        {
            try
            {
                var path = GetAuditFileName();

                FileInfo auditfile = new FileInfo(path);

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
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "MessageFileNotExist", "AlertFileNotExist();", true);
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private string GetAuditFileName()
        {
            var path = "";

            try
            {
                int menuTypeId = -1;

                long cycleId = Convert.ToInt64(ddlCycle.SelectedValue);

                var cycle = _cycleManagement.GetCycle(cycleId);
                var className = _menuManagement.GetClassShortName(Convert.ToInt32(ddlClass.SelectedValue));

                if (ddlMenuType.SelectedValue != "All")
                    menuTypeId = Convert.ToInt32(ddlMenuType.SelectedValue);

                var arrival = ddlFlightTo.SelectedItem.Text;
                var departure = ddlFlightFrom.SelectedItem.Text;


                var directory = Directory.GetParent(HttpRuntime.AppDomainAppPath);
                var parentDir = directory.Parent;

                if (menuTypeId == -1)
                {
                    if (arrival == "All" && departure == "All")
                        path = parentDir.FullName + @"\Audit\" + cycle.CycleName + "_" + className + "_Audit_AllRoutes.xlsx";

                    else
                    {
                        if (arrival != "All" && departure != "All")

                            path = parentDir.FullName + @"\Audit\" + cycle.CycleName + "_" + className + "_" + departure + "_" + arrival + "_Audit.xlsx";
                    }
                }
                else
                {
                    if (arrival == "All" && departure == "All")
                    {
                        var menuType = _menuManagement.GetMenuTypeById(menuTypeId);
                        path = parentDir.FullName + @"\Audit\" + cycle.CycleName + "_" + className + "_" + menuType.DisplayName + "_Audit_AllRoutes.xlsx";
                    }
                    else
                    {
                        if (arrival != "All" && departure != "All")
                        {
                            var menuType = _menuManagement.GetMenuTypeById(menuTypeId);
                            path = parentDir.FullName + @"\Audit\" + cycle.CycleName + "_" + className + "_" + menuType.DisplayName + "_" + departure + "_" + arrival + "_Audit.xlsx";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
            return path;
        }


        #region DropDowns

        private void BindCycle()
        {
            var cycles = _cycleManagement.GetCycles();

            ddlCycle.Items.Clear();
            foreach (var cycle in cycles)
            {
                ddlCycle.Items.Add(new ListItem(cycle.CycleName, Convert.ToString(cycle.Id)));
            }
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

        private void BindMenuType()
        {
            ddlMenuType.Items.Clear();

            ddlMenuType.Items.Add(new ListItem("All", "All"));

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


        private void BindFlightTo()
        {
            ddlFlightTo.Items.Clear();

            var fromFlight = ddlFlightFrom.SelectedValue;

            if (fromFlight == "All")
                ddlFlightTo.Items.Add(new ListItem("All", "All"));

            if (!string.IsNullOrEmpty(fromFlight) && fromFlight != "All")
            {
                var locations = _routeManagement.GetArrivalsForDeparture(Convert.ToInt16(fromFlight));

                foreach (var location in locations)
                {
                    ddlFlightTo.Items.Add(new ListItem(location.AirportCode, Convert.ToString(location.LocationID)));
                }
            }
        }

        private void BindFlightFrom()
        {
            var locations = _routeManagement.GetAllLocations();

            ddlFlightFrom.Items.Clear();
            ddlFlightFrom.Items.Add(new ListItem("All", "All"));

            foreach (var location in locations)
            {
                ddlFlightFrom.Items.Add(new ListItem(location.AirportCode, Convert.ToString(location.LocationID)));
            }
        }
        protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMenuType();
        }

        protected void ddlFlightFrom_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindFlightTo();
        }
        #endregion

    }
}