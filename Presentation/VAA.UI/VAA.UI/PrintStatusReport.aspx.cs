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
    /// Generate Print Status Report - show all menu with current status
    /// </summary>
    public partial class PrintStatusReport : Page
    {
        readonly CycleManagement _cycleManagement = new CycleManagement();
        readonly MenuManagement _menuManagement = new MenuManagement();
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
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        /// <summary>
        /// generate and download the status report
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GenerateAuditBtnClicked(object sender, EventArgs e)
        {
            try
            {
                int menuTypeId = -1;

                if (ddlMenuType.SelectedValue != "All")
                    menuTypeId = Convert.ToInt32(ddlMenuType.SelectedValue);

                AuditTrailGeneration audit = new AuditTrailGeneration();

                audit.GeneratePrintStausAudit(Convert.ToInt64(ddlCycle.SelectedValue), Convert.ToInt32(ddlClass.SelectedValue), menuTypeId);

                var path = GetPrintStatusReportFileName();

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

            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        /// <summary>
        /// get the status reprot file name
        /// </summary>
        /// <returns></returns>

        private string GetPrintStatusReportFileName()
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


                var directory = Directory.GetParent(HttpRuntime.AppDomainAppPath);
                var parentDir = directory.Parent;

                if (menuTypeId == -1)
                {
                    path = parentDir.FullName + @"\PrintStatusReport\" + cycle.CycleName + "_" + className + "_PrintStatus.xlsx";


                }
                else
                {
                    var menuType = _menuManagement.GetMenuTypeById(menuTypeId);
                    path = parentDir.FullName + @"\PrintStatusReport\" + cycle.CycleName + "_" + className + "_" + menuType.DisplayName + "_PrintStatus.xlsx";

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

        protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMenuType();
        }


        #endregion

    }
}