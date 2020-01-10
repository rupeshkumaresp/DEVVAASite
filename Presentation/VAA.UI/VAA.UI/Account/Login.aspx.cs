using Elmah;
using System;
using System.Web.UI;
using VAA.CommonComponents;
using VAA.DataAccess;

namespace VAA.UI.Account
{
    /// <summary>
    /// Account login related functionality, set the credentails in cookie
    /// </summary>
    public partial class Login : Page
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

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                SetCookie();

                int userId = _accountManagement.LogIn(txtEmail.Text, EncryptionHelper.Encrypt(txtPassword.Text));
                if (userId > 0)
                {
                    pnlErrorMessage.Visible = false;
                    Session["USERID"] = userId;
                    Response.Redirect("~/Home.aspx");
                }
                else
                {
                    pnlErrorMessage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
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