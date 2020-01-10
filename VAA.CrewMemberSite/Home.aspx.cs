using System;
using System.Linq;
using System.Web.UI;
using Elmah;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using VAA.DataAccess;
using System.Web.Caching;

namespace VAA.CrewMemberSite
{
    public partial class _Default : Page
    {
        readonly CycleManagement _cycleManagement = new CycleManagement();
        readonly MenuManagement _menuManagement = new MenuManagement();
        readonly AccountManagement _accountManagement = new AccountManagement();
        readonly RouteManagement _routeManagement = new RouteManagement();
        readonly MenuViewerManagement _menuviewerManagement = new MenuViewerManagement();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(Session["USERID"])))
                    Response.Redirect("~/Default.aspx");

                if (!IsPostBack)
                {
                    var userId = Convert.ToInt32(Session["USERID"]);
                    var userdata = _accountManagement.GetUserTypeByUserid(userId);
                    if (userdata == "ESP" || userdata == "Virgin")
                    {
                        BindCycle();
                        BindMenuClass();
                        BindMenuType();
                        BindDeparture();
                        BindArrival();
                        BindFlightNo();
                        //GridDataSource();
                    }
                    else if (userdata == "VirginCrew")
                    {
                        cycleDiv.Visible = false;
                        BindMenuClass();
                        BindMenuType();
                        BindDeparture();
                        BindArrival();
                        BindFlightNo();
                        //GridDataSource();
                    }
                }

            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

        }
        #region bind dropdowns
        private void BindCycle()
        {
            var cycles = _cycleManagement.GetCycles();

            ddlCycle.Items.Clear();
            ddlCycle.Items.Add(new ListItem("All", "All"));

            foreach (var cycle in cycles)
            {
                ddlCycle.Items.Add(new ListItem(cycle.CycleName, Convert.ToString(cycle.Id)));
            }
        }
        private void BindMenuClass()
        {
            ddlClass.Items.Clear();
            //ddlClass.Items.Add(new ListItem("All", "All"));

            var classes = _menuManagement.GetAllClass();

            foreach (var menuclass in classes)
            {
                ddlClass.Items.Add(new ListItem(menuclass.FlightClass, Convert.ToString(menuclass.ID)));
            }
        }
        private void BindMenuType()
        {
            ddlMenutype.Items.Clear();

            var selectedClassId = ddlClass.SelectedValue;

            if (selectedClassId == "All")
            {
                ddlMenutype.Items.Add(new ListItem("All", "All"));
            }

            if (!string.IsNullOrEmpty(selectedClassId) && selectedClassId != "All")
            {
                if (selectedClassId == "1")
                {
                    ddlMenutype.Items.Add(new ListItem("All", "All"));
                    var menuTypes = _menuManagement.GetMenuTypeByClass(Convert.ToInt32(selectedClassId));

                    foreach (var types in menuTypes)
                    {
                        ddlMenutype.Items.Add(new ListItem(types.DisplayName, Convert.ToString(types.ID)));
                    }
                }
                else
                {
                    var menuTypes = _menuManagement.GetMenuTypeByClass(Convert.ToInt32(selectedClassId));

                    foreach (var types in menuTypes)
                    {
                        ddlMenutype.Items.Add(new ListItem(types.DisplayName, Convert.ToString(types.ID)));
                    }

                    ddlMenutype.Items.Add(new ListItem("Allergen guides", Convert.ToString(100)));

                }
            }
        }
        private void BindFlightNo()
        {
            ddlFlight.Items.Clear();
            ddlFlight.Items.Add(new ListItem("All", "All"));

            var departure = ddlDeparture.SelectedValue;
            var arrival = ddlArrival.SelectedValue;

            if (departure == "All" && arrival == "All")
            {
                var flights = _routeManagement.GetAllFlightNo();

                foreach (var flight in flights)
                {
                    ddlFlight.Items.Add(new ListItem(flight.FlightNo, Convert.ToString(flight.FlightNo)));
                }
            }
            else if (departure == "All" && !string.IsNullOrEmpty(arrival) && arrival != "All")
            {
                var flights = _routeManagement.GetFlightNoByArrival(Convert.ToInt16(arrival));

                foreach (var flight in flights)
                {
                    ddlFlight.Items.Add(new ListItem(flight.FlightNo, Convert.ToString(flight.FlightNo)));
                }
            }
            else if (!string.IsNullOrEmpty(departure) && departure != "All" && arrival == "All")
            {
                var flights = _routeManagement.GetFlightNoByDeparture(Convert.ToInt16(departure));

                foreach (var flight in flights)
                {
                    ddlFlight.Items.Add(new ListItem(flight.FlightNo, Convert.ToString(flight.FlightNo)));
                }
            }
            else if (!string.IsNullOrEmpty(departure) && departure != "All" && !string.IsNullOrEmpty(arrival) && arrival != "All")
            {
                var flights = _routeManagement.GetFlightNoByRoute(Convert.ToInt16(departure), Convert.ToInt16(arrival));

                foreach (var flight in flights)
                {
                    ddlFlight.Items.Add(new ListItem(flight.FlightNo, Convert.ToString(flight.FlightNo)));
                }
            }
        }

        private void BindDeparture()
        {
            ddlDeparture.Items.Clear();
            ddlDeparture.Items.Add(new ListItem("All", "All"));

            var arrival = ddlArrival.SelectedValue;
            var alllocations = _routeManagement.GetAllLocations();

            if (arrival == "All")
            {
                foreach (var location in alllocations)
                {
                    ddlDeparture.Items.Add(new ListItem(location.AirportCode, Convert.ToString(location.LocationID)));
                }
            }

            if (!string.IsNullOrEmpty(arrival) && arrival != "All")
            {
                var locations = _routeManagement.GetArrivalsForDeparture(Convert.ToInt16(arrival));

                foreach (var location in locations)
                {
                    ddlDeparture.Items.Add(new ListItem(location.AirportCode, Convert.ToString(location.LocationID)));
                }
            }
            else
            {
                foreach (var location in alllocations)
                {
                    ddlDeparture.Items.Add(new ListItem(location.AirportCode, Convert.ToString(location.LocationID)));
                }
            }

        }

        private void BindArrival()
        {
            ddlArrival.Items.Clear();
            ddlArrival.Items.Add(new ListItem("All", "All"));

            var departure = ddlDeparture.SelectedValue;
            var alllocations = _routeManagement.GetAllLocations();

            if (departure == "All")
            {
                foreach (var location in alllocations)
                {
                    ddlArrival.Items.Add(new ListItem(location.AirportCode, Convert.ToString(location.LocationID)));
                }
            }
            else if (!string.IsNullOrEmpty(departure) && departure != "All")
            {
                var locations = _routeManagement.GetArrivalsForDeparture(Convert.ToInt16(departure));

                foreach (var location in locations)
                {
                    ddlArrival.Items.Add(new ListItem(location.AirportCode, Convert.ToString(location.LocationID)));
                }
            }
        }
        protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMenuType();
        }

        protected void ddlDeparture_SelectedIndexChanged(object sender, EventArgs e)
        {
            var departure = ddlDeparture.SelectedValue;
            var arrival = ddlArrival.SelectedValue;

            if (departure != "All" && arrival == "All")
            {
                BindArrival();
                BindFlightNo();
                ddlDeparture.SelectedValue = departure;
            }
            else if (departure == "All" && arrival != "All")
            {
                BindDeparture();
                BindFlightNo();
                ddlArrival.SelectedValue = arrival;
            }
            else if (departure != "All" && arrival != "All")
            {
                BindFlightNo();
                ddlArrival.SelectedValue = arrival;
                ddlDeparture.SelectedValue = departure;
            }
            else
            {
                BindArrival();
                BindDeparture();
                BindFlightNo();
            }
        }
        protected void ddlArrival_SelectedIndexChanged(object sender, EventArgs e)
        {
            var arrival = ddlArrival.SelectedValue;
            var departure = ddlDeparture.SelectedValue;

            if (departure != "All" && arrival == "All")
            {
                BindArrival();
                BindFlightNo();
                ddlDeparture.SelectedValue = departure;
            }
            else if (departure == "All" && arrival != "All")
            {
                BindDeparture();
                BindFlightNo();
                ddlArrival.SelectedValue = arrival;
            }
            else if (departure != "All" && arrival != "All")
            {
                BindFlightNo();
                ddlArrival.SelectedValue = arrival;
                ddlDeparture.SelectedValue = departure;
            }
            else
            {
                BindArrival();
                BindDeparture();
                BindFlightNo();
            }
        }
        protected void ddlFlightNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            var flight = ddlFlight.SelectedValue;
            ddlFlight.SelectedValue = flight;
        }

        #endregion

        private void GridDataSource()
        {
            try
            {
                var cycle = ddlCycle.SelectedValue;
                if (cycle == "All")
                {
                    cycle = "0";
                }
                var menuclass = ddlClass.SelectedValue;
                if (menuclass == "All")
                {
                    menuclass = "0";
                }
                var menutype = ddlMenutype.SelectedValue;
                if (menutype == "All")
                {
                    menutype = "0";
                }
                var departure = ddlDeparture.SelectedValue;
                if (departure == "All")
                {
                    departure = "0";
                }
                var arrival = ddlArrival.SelectedValue;
                if (arrival == "All")
                {
                    arrival = "0";
                }
                var flightno = ddlFlight.SelectedValue;
                if (flightno == "All")
                {
                    flightno = null;
                }

                var userId = Convert.ToInt32(Session["USERID"]);
                var userdata = _accountManagement.GetUserTypeByUserid(userId);
                if (userdata == "ESP" || userdata == "Virgin")
                {
                    var menuDataSource = _menuviewerManagement.GetListForMenuViewer(Convert.ToInt64(cycle), Convert.ToInt16(menuclass), Convert.ToInt16(menutype), Convert.ToInt16(departure), Convert.ToInt16(arrival), Convert.ToString(flightno));

                    Cache["DataSource"] = menuDataSource;

                    radGridMenu.DataSource = Cache["DataSource"];
                    radGridMenu.DataBind();

                }
                else if (userdata == "VirginCrew")
                {
                    //get current cycle and pass it
                    var currCycle = _cycleManagement.GetActiveCycle();
                    var currcycleId = currCycle.Id;

                    var menuDataSource = _menuviewerManagement.GetListForMenuViewer(Convert.ToInt64(currcycleId), Convert.ToInt16(menuclass), Convert.ToInt16(menutype), Convert.ToInt16(departure), Convert.ToInt16(arrival), Convert.ToString(flightno));
                    Cache["DataSource"] = menuDataSource;
                    radGridMenu.DataSource = Cache["DataSource"];
                    radGridMenu.DataBind();
                }
                else
                {
                    //out
                }
            }
            catch (Exception ex)
            {
                //    //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

        }

        public void btnGetMenuList_Clicked(object sender, EventArgs e)
        {
            radGridMenu.Visible = true;
            GridDataSource();
        }
        public void radGridMenu_PreRender(object sender, EventArgs e)
        {
            var userId = Convert.ToInt32(Session["USERID"]);
            var userdata = _accountManagement.GetUserTypeByUserid(userId);
            if (userdata == "VirginCrew")
            {
                radGridMenu.MasterTableView.GetColumn("CycleName").Display = false;
            }
        }
        public void radGridMenu_PageSizeChange(object sender, GridPageSizeChangedEventArgs e)
        {
            radGridMenu.MasterTableView.Rebind();
            GridDataSource();
        }
        protected void radGridMenu_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            radGridMenu.DataSource = Cache["DataSource"];
            return;
            //if (IsPostBack)
            {
                var cycle = ddlCycle.SelectedValue;
                if (cycle == "All")
                {
                    cycle = "0";
                }
                var menuclass = ddlClass.SelectedValue;
                if (menuclass == "All")
                {
                    menuclass = "0";
                }
                var menutype = ddlMenutype.SelectedValue;
                if (menutype == "All")
                {
                    menutype = "0";
                }
                var departure = ddlDeparture.SelectedValue;
                if (departure == "All")
                {
                    departure = "0";
                }
                var arrival = ddlArrival.SelectedValue;
                if (arrival == "All")
                {
                    arrival = "0";
                }
                var flightno = ddlFlight.SelectedValue;
                if (flightno == "All")
                {
                    flightno = null;
                }

                var userId = Convert.ToInt32(Session["USERID"]);
                var userdata = _accountManagement.GetUserTypeByUserid(userId);
                if (userdata == "ESP" || userdata == "Virgin")
                {
                    var menuDataSource = _menuviewerManagement.GetListForMenuViewer(Convert.ToInt64(cycle), Convert.ToInt16(menuclass), Convert.ToInt16(menutype), Convert.ToInt16(departure), Convert.ToInt16(arrival), Convert.ToString(flightno));
                    radGridMenu.DataSource = menuDataSource;
                }
                else if (userdata == "VirginCrew")
                {
                    //get current cycle and pass it
                    var currCycle = _cycleManagement.GetActiveCycle();
                    var currcycleId = currCycle.Id;

                    var menuDataSource = _menuviewerManagement.GetListForMenuViewer(Convert.ToInt64(currcycleId), Convert.ToInt16(menuclass), Convert.ToInt16(menutype), Convert.ToInt16(departure), Convert.ToInt16(arrival), Convert.ToString(flightno));
                    radGridMenu.DataSource = menuDataSource;
                }
            }
        }
        protected void radGridMenu_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {

                if (e.CommandName.Equals("ClearFilter"))
                {
                    foreach (GridColumn column in radGridMenu.MasterTableView.Columns)
                    {
                        column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                        column.CurrentFilterValue = string.Empty;
                    }
                    radGridMenu.MasterTableView.FilterExpression = string.Empty;
                    radGridMenu.MasterTableView.Rebind();
                }
                else if (e.CommandName.Equals("Refresh"))
                {
                    radGridMenu.DataSource = null;
                    GridDataSource();
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }

        }
        protected void radGridMenu_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    try
                    {
                        GridDataItem dataItem = (GridDataItem)e.Item;
                        string rowId = dataItem["Id"].Text;
                        ImageButton editLink = (ImageButton)e.Item.FindControl("ViewBookLink");
                        editLink.Attributes["href"] = "javascript:void(0);";
                        editLink.Attributes["onclick"] = String.Format("return ShowEditBook('{0}','{1}');", rowId, e.Item.ItemIndex);

                    }

                    catch { }
                    try
                    {
                        GridDataItem dataItem = (GridDataItem)e.Item;
                        string rowId = dataItem["Id"].Text;
                        ImageButton editLink = (ImageButton)e.Item.FindControl("ViewPDFLink");
                        editLink.Attributes["href"] = "javascript:void(0);";
                        editLink.Attributes["onclick"] = String.Format("return ShowEditPDF('{0}','{1}');", rowId, e.Item.ItemIndex);

                    }

                    catch { }

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