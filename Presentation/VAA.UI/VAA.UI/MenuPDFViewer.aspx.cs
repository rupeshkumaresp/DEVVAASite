using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VAA.BusinessComponents;
using VAA.DataAccess;

namespace VAA.UI
{
    public partial class MenuPDFViewer : System.Web.UI.Page
    {
        MenuProcessor _menuProcessor = new MenuProcessor();
        MenuManagement _menuManagement = new MenuManagement();


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (string.IsNullOrEmpty(Convert.ToString(Session["USERID"])))
                    Response.Redirect("~/Account/Login.aspx");

                if (!IsPostBack)
                {
                    Page.Title = "";
                    string rowId = Convert.ToString(Request.QueryString["ID"]);
                    if (!string.IsNullOrEmpty(rowId))
                    {
                        long id = 0;
                        bool rid = long.TryParse(rowId, out id);
                        if (rid)
                        {
                            Session["CURRENTMENUID"] = id;
                            LoadPDF(id);

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                //ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }


        /// <summary>
        /// Load chili document
        /// </summary>
        /// <param name="isFlashEditor"></param>
        private void LoadPDF(long menuId)
        {
            try
            {
                //get the file file based on selected critieria
                var pdfFileName = _menuProcessor.GeneratePdfFileNameForDownloadByMenuId(menuId);

                var menu = _menuManagement.GetMenuById(menuId);

                if (menu.MenuTypeId == 6 || menu.MenuTypeId == 7)
                {
                    //if special meal or allergen guide, take name as menucode.pdf

                    var menuClassId = _menuManagement.GetMenuClass(Convert.ToInt32(menu.MenuTypeId));

                    if (menu.MenuTypeId == 6 && menuClassId == 1)
                        pdfFileName = "Allergen_UC.pdf";

                    if (menu.MenuTypeId == 6 && (menuClassId == 2 || menuClassId == 3))
                        pdfFileName = "Allergen_PE_ECO.pdf";

                    if (menu.MenuTypeId == 7)
                        pdfFileName = "SPML.pdf";
                }
                else
                {
                    try
                    {
                        string EmmaPDFPathFolder = (System.Configuration.ConfigurationManager.AppSettings["EmmaPDFPathFolder"]) + @"\\MENU PDFS\";

                        if (!System.IO.File.Exists(EmmaPDFPathFolder + pdfFileName))
                        {
                            var menutemplate = _menuManagement.GetMenuTemplate(menuId);
                            _menuProcessor.Generate(Convert.ToInt64(menu.CycleId), 1, menuId, Convert.ToInt32(menutemplate.TemplateID), Convert.ToInt32(menu.LanguageId));
                        }
                    }
                    catch { }
                }

               


                iframePDF.Attributes["src"] = "http://emmamenupdf.approve4print.co.uk/" + pdfFileName;
            }
            catch (Exception ex)
            {
                //write to Elma
                //ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

    }
}