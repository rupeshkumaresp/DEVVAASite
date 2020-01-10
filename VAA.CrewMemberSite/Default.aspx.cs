using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using VAA.CommonComponents;
using VAA.DataAccess;

namespace VAA.CrewMemberSite
{
    public partial class Default : System.Web.UI.Page
    {
        readonly AccountManagement _accountManagement = new AccountManagement();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReadFromCookie();
            }
            txtPassword.Attributes["type"] = "password";
        }
        public void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtEmail.Text.ToLower() == "virgin" || txtEmail.Text.ToLower() == "virgincrew")
            {
                int userId = _accountManagement.LogIn(txtEmail.Text, EncryptionHelper.Encrypt(txtPassword.Text));
                if (userId > 0)
                {
                    pnlErrorMessage.Visible = false;
                    Session["USERID"] = userId;
                    SetCookie();
                    Response.Redirect("~/Home.aspx");
                }
                else
                {
                    pnlErrorMessage.Visible = true;
                }
            }
            else
            {
                pnlErrorMessage.Visible = true;
                return;
            }
        }
        private void ReadFromCookie()
        {
            if (Request.Cookies["vaaemail"] != null)
                txtEmail.Text = Request.Cookies["vaaemail"].Value;

            if (Request.Cookies["vaapassword"] != null)
                txtPassword.Text = EncryptionHelper.Decrypt(Request.Cookies["vaapassword"].Value);
        }
        /// <summary>
        /// Read and write to Cookie
        /// </summary>
        private void SetCookie()
        {
            if (cbRememberMe.Checked)
            {
                Response.Cookies["vaaemail"].Value = txtEmail.Text;
                Response.Cookies["vaapassword"].Value = EncryptionHelper.Encrypt(txtPassword.Text);
                Response.Cookies["vaaemail"].Expires = DateTime.Now.AddDays(30);
                Response.Cookies["vaapassword"].Expires = DateTime.Now.AddDays(30);
            }
            else
            {
                if (Request.Cookies["vaaemail"] != null)
                    Response.Cookies["vaaemail"].Expires = DateTime.Now.AddDays(-1);

                if (Request.Cookies["vaapassword"] != null)
                    Response.Cookies["vaapassword"].Expires = DateTime.Now.AddDays(-1);
            }
        }
    }
}