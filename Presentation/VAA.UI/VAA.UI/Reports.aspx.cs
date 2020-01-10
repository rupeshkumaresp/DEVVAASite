using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace VAA.UI
{
    public partial class Reports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["USERID"])))
                Response.Redirect("~/Account/Login.aspx");

            if (!Page.IsPostBack)
            {
                if (ddlReportType.SelectedValue == "0")
                    this.ReportViewerEmma.Visible = false;
                else
                    this.ReportViewerEmma.Visible = true;
            }

        }

        protected void ddlReportType_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlReportType.SelectedValue == "0")
                this.ReportViewerEmma.Visible = false;
            else
                this.ReportViewerEmma.Visible = true;

            if (ddlReportType.SelectedValue == "1")
            {
                var typeReportSource = new Telerik.Reporting.TypeReportSource();
                typeReportSource.TypeName = "ReportLibrary.SeatConfiguration, ReportLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                this.ReportViewerEmma.ReportSource = typeReportSource;
            }

            if (ddlReportType.SelectedValue == "2")
            {
                var typeReportSource = new Telerik.Reporting.TypeReportSource();
                typeReportSource.TypeName = "ReportLibrary.Region, ReportLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                this.ReportViewerEmma.ReportSource = typeReportSource;
            }

            if (ddlReportType.SelectedValue == "3")
            {
                var typeReportSource = new Telerik.Reporting.TypeReportSource();
                typeReportSource.TypeName = "ReportLibrary.Locations, ReportLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                this.ReportViewerEmma.ReportSource = typeReportSource;
            }

            if (ddlReportType.SelectedValue == "4")
            {
                var typeReportSource = new Telerik.Reporting.TypeReportSource();
                typeReportSource.TypeName = "ReportLibrary.RouteDetails, ReportLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                this.ReportViewerEmma.ReportSource = typeReportSource;
            }

            if (ddlReportType.SelectedValue == "5")
            {
                var typeReportSource = new Telerik.Reporting.TypeReportSource();
                typeReportSource.TypeName = "ReportLibrary.ScheduleofFlight, ReportLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                this.ReportViewerEmma.ReportSource = typeReportSource;
            }
            if (ddlReportType.SelectedValue == "6")
            {
                var typeReportSource = new Telerik.Reporting.TypeReportSource();
                typeReportSource.TypeName = "ReportLibrary.Approver, ReportLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                this.ReportViewerEmma.ReportSource = typeReportSource;
            }
            if (ddlReportType.SelectedValue == "7")
            {
                var typeReportSource = new Telerik.Reporting.TypeReportSource();
                typeReportSource.TypeName = "ReportLibrary.ChiliTemplate, ReportLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                this.ReportViewerEmma.ReportSource = typeReportSource;
            }
            if (ddlReportType.SelectedValue == "8")
            {
                var typeReportSource = new Telerik.Reporting.TypeReportSource();
                typeReportSource.TypeName = "ReportLibrary.Notifications, ReportLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                this.ReportViewerEmma.ReportSource = typeReportSource;
            }
            if (ddlReportType.SelectedValue == "9")
            {
                var typeReportSource = new Telerik.Reporting.TypeReportSource();
                typeReportSource.TypeName = "ReportLibrary.MenuItemCategory, ReportLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                this.ReportViewerEmma.ReportSource = typeReportSource;
            }

            if (ddlReportType.SelectedValue == "10")
            {
                var typeReportSource = new Telerik.Reporting.TypeReportSource();
                typeReportSource.TypeName = "ReportLibrary.BaseItemReport, ReportLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                this.ReportViewerEmma.ReportSource = typeReportSource;
            }
        }

    }
}