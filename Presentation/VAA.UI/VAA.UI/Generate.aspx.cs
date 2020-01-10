using Elmah;
using ICSharpCode.SharpZipLib.Zip;
using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VAA.BusinessComponents;
using VAA.CommonComponents;
using VAA.DataAccess;

namespace VAA.UI
{
    /// <summary>
    /// Generate and Download Menu PDF
    /// Single PDF can be downloaded, multi PDF has to be viewed at FTP
    /// </summary>
    public partial class Generate : Page
    {
        readonly CycleManagement _cycleManagement = new CycleManagement();
        readonly MenuManagement _menuManagement = new MenuManagement();
        readonly RouteManagement _routeManagement = new RouteManagement();
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

        public void DownloadPdfBtnClicked(object sender, EventArgs e)
        {

            try
            {
                string EmmaPDFPathFolder = (System.Configuration.ConfigurationManager.AppSettings["EmmaPDFPathFolder"]);

                long routeId = 0;
                var directory = Directory.GetParent(HttpRuntime.AppDomainAppPath);
                var parentDir = directory.Parent;

                if (ddlMenuType.SelectedItem.Text == "All" || ddlFlightFrom.SelectedItem.Text == "All" || ddlFlightTo.SelectedItem.Text == "All")
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "CheckPDFOnFTP", "AlertCheckPDFOnFTP();", true);
                    return;
                }

                int menuTypeId = -1;

                if (ddlMenuType.SelectedValue != "All")
                    menuTypeId = Convert.ToInt32(ddlMenuType.SelectedValue);

                if (ddlFlightFrom.SelectedItem.Text != "All" && ddlFlightTo.SelectedItem.Text != "All")
                    routeId = _routeManagement.GetRouteId(ddlFlightFrom.SelectedItem.Text, ddlFlightTo.SelectedItem.Text);

                var fileNames = _menuProcessor.GeneratePdfFileNameForDownload(Convert.ToInt64(ddlCycle.SelectedValue), Convert.ToInt32(ddlClass.SelectedValue), menuTypeId, routeId);

                if (fileNames.Count == 0 && (ddlMenuType.SelectedValue != "All" && ddlFlightFrom.SelectedItem.Text != "All" && ddlFlightTo.SelectedItem.Text != "All"))
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "NoPDFOnFTP", "AlertNoPDFOnFTP();", true);
                else
                {
                    if (fileNames.Count == 1)
                    {
                        FileInfo pdffile = new FileInfo(EmmaPDFPathFolder + @"\MENU PDFS\" + fileNames[0]);

                        if (pdffile.Exists)
                        {
                            string fname = pdffile.Name;
                            fname = fname.Replace(" ", "_");
                            Response.ClearContent();
                            Response.Buffer = true;
                            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", fname));
                            Response.ContentType = "application/ms-excel";
                            Response.TransmitFile(pdffile.FullName);
                            Response.End();
                        }
                    }
                    else
                    {
                        Response.AddHeader("Content-Disposition", "attachment; filename=" + "MenuPDF" + ".zip");
                        Response.ContentType = "application/zip";

                        using (var zipStream = new ZipOutputStream(Response.OutputStream))
                        {
                            foreach (string filePath in fileNames)
                            {
                                var path = EmmaPDFPathFolder + @"\MENU PDFS\" + filePath;


                                byte[] fileBytes = System.IO.File.ReadAllBytes(path);

                                var fileEntry = new ZipEntry(Path.GetFileName(path))
                                {
                                    Size = fileBytes.Length
                                };

                                zipStream.PutNextEntry(fileEntry);
                                zipStream.Write(fileBytes, 0, fileBytes.Length);
                            }

                            zipStream.Flush();
                            zipStream.Close();
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

        public void GeneratePdfBtnClicked(object sender, EventArgs e)
        {
            try
            {
                int menuTypeId = -1;

                if (ddlMenuType.SelectedValue != "All")
                    menuTypeId = Convert.ToInt32(ddlMenuType.SelectedValue);

                long routeId = 0;

                if (ddlFlightFrom.SelectedItem.Text != "All" || ddlFlightTo.SelectedItem.Text != "All")
                    routeId = _routeManagement.GetRouteId(ddlFlightFrom.SelectedItem.Text, ddlFlightTo.SelectedItem.Text);

                _menuProcessor.GeneratePdf(Convert.ToInt64(ddlCycle.SelectedValue), Convert.ToInt32(ddlClass.SelectedValue), menuTypeId, routeId);


                //send email
                int userId = Convert.ToInt32(Session["USERID"]);
                var user = _accountManagement.GetUserById(userId);

                var PDFMessage = EmailHelper.PdfGenerationCompleteForCritetiaEmailTemplate;

                EmailHelper.SendMail(user.Username, "ESPAdmin@espcolour.co.uk", "EMMA- PDF Generation - " + ddlClass.SelectedItem.Text + " - " + ddlMenuType.SelectedItem.Text, PDFMessage);

                string notificationEmails = (System.Configuration.ConfigurationManager.AppSettings["NotificationEmails"]);

                var emails = notificationEmails.Split(new char[] { ';' });

                foreach (var email in emails)
                {
                    if (!string.IsNullOrEmpty(email))
                        EmailHelper.SendMail(email, "ESPAdmin@espcolour.co.uk", "EMMA- PDF Generation - " + ddlClass.SelectedItem.Text + "-" + ddlMenuType.SelectedItem.Text, PDFMessage);
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
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

            //ddlMenuType.Items.Add(new ListItem("All", "All"));

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