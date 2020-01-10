using System;
using System.Linq;
using System.Web.UI;
using Elmah;
using VAA.DataAccess;

namespace VAA.UI
{
    public partial class ViewApprover : System.Web.UI.Page
    {
        /// <summary>
        /// Approvers for system - View
        /// </summary>
        AccountManagement _accountManagement = new AccountManagement();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(Session["USERID"])))
                    Response.Redirect("~/Account/Login.aspx");

                if (!IsPostBack)
                {
                    string userId = Request.QueryString["ID"];
                    if (!string.IsNullOrEmpty(userId))
                    {
                        int Id = 0;
                        bool RID = Int32.TryParse(userId, out Id);
                        if (RID)
                        {
                            Session["CURRENTUSERID"] = Id;
                            if (Id > 0)
                            {
                                //Get All User data 
                                var userData = _accountManagement.GetUserById(Id);
                                if (userData != null)
                                {
                                    lblName.Text = userData.FirstName + " " + userData.LastName;
                                    lblDesignation.Text = userData.Designation;
                                    lblDepartment.Text = userData.Department;
                                    lblTelephone.Text = userData.Telephone;
                                    lblMobile.Text = userData.Mobile;
                                    lblUserName.Text = userData.Username;
                                }
                            }

                            else
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "mykey", "CancelEdit();", true);
                        }
                        else
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mykey", "CancelEdit();", true);
                    }
                    else
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "mykey", "CancelEdit();", true);
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        public void MailtoButton_clicked(object sender, EventArgs e)
        {
            var toEmail = lblUserName.Text;
            Response.Redirect("mailto:" + toEmail);
        }
    }
}