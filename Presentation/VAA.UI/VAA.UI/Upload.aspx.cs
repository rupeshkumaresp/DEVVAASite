using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VAA.BusinessComponents;
using VAA.DataAccess;

namespace VAA.UI
{
    public partial class Upload : Page
    {
        readonly CycleManagement _cycleManagement = new CycleManagement();
        readonly MenuManagement _menuManagement = new MenuManagement();
        readonly RouteManagement _routeManagement = new RouteManagement();

        MenuProcessor _menuProcessor = new MenuProcessor();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindCycle();
                BindMenuClass();
                //BindMenuType();
            }
        }


        #region Bind DropDowns

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

        //private void BindMenuType()
        //{
        //    ddlMenuType.Items.Clear();

        //    var selectedClassId = ddlClass.SelectedValue;

        //    if (!string.IsNullOrEmpty(selectedClassId))
        //    {
        //        var menuTypes = _menuManagement.GetMenuTypeByClass(Convert.ToInt32(selectedClassId));

        //        foreach (var types in menuTypes)
        //        {
        //            ddlMenuType.Items.Add(new ListItem(types.DisplayName, Convert.ToString(types.ID)));
        //        }
        //    }
        //}

        protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            //BindMenuType();
        }

        #endregion

        protected void btnUpload_Click(object sender, EventArgs e)
        {

            if (RadAsyncUploadMenu.UploadedFiles.Count == 0)
                return;

            UploadedFile attachment = RadAsyncUploadMenu.UploadedFiles[0];

            int userId = Convert.ToInt32(Session["USERID"]);

            var classId = Convert.ToInt32(ddlClass.SelectedValue);

            var menuTypeId = 1;


            //TODO: build this for all menu types for the class, call the ImportMenu in loop for all menu types

            var menuTypes = _menuManagement.GetMenuTypeByClass(classId);

            foreach (var menuType in menuTypes)
            {
                _menuProcessor.ImportMenu(Convert.ToInt64(ddlCycle.SelectedValue), Convert.ToInt32(ddlClass.SelectedValue), menuType.ID, attachment.InputStream, userId);
            }

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "UploadCompleted", "UploadCompleted();", true);

            //if (classId == 1)
            //    menuTypeId = 1;
            //else if (classId == 2)
            //    menuTypeId = 10;
            //else
            //    menuTypeId = 13;

            //_menuProcessor.ImportMenu(Convert.ToInt64(ddlCycle.SelectedValue), Convert.ToInt32(ddlClass.SelectedValue), menuTypeId, attachment.InputStream, userId);

        }

    }
}