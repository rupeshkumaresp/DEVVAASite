using System;
using System.IO;
using System.Linq;
using VAA.BusinessComponents;
using VAA.DataAccess;

namespace VAA.CrewMemberSite
{
    /// <summary>
    /// Menu Viewer - Lanuch the chili proof viewer page - show flash and html based editor
    /// </summary>
    public partial class PDFViewer : System.Web.UI.Page
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

                var pdfFileName = "";

                if (menuId == 11111111111)
                {
                    pdfFileName = "Allergen_UC.pdf";
                }

                else
                {
                    if (menuId == 22222222222)
                    {
                        pdfFileName = "Allergen_PE_ECO.pdf";
                    }
                    else
                    {
                        if (menuId == 33333333333)
                        {
                            pdfFileName = "Allergen_PE_ECO.pdf";
                        }
                        else
                        {
                            if (menuId == 77777777777)
                                pdfFileName = "SPML.pdf";
                            else
                            {
                                var menu = _menuManagement.GetMenuById(menuId);

                                pdfFileName = _menuProcessor.GeneratePdfFileNameForDownloadByMenuId(menuId);
                            }

                        }

                    }
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