using Elmah;
using System;
using System.Web.UI;
using VAA.CommonComponents;
using VAA.DataAccess;

namespace VAA.UI.Account
{
    /// <summary>
    /// Forgot password page for sending temp password to user
    /// </summary>
    public partial class ForgotPwd : Page
    {
        readonly AccountManagement _accountManagement = new AccountManagement();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// create a new password and send to user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void BtnForgotPssswordClick(object sender, EventArgs e)
        {

            lblPwdSentMessage.Text = "";

            try
            {
                var user = _accountManagement.GetUserByUsername(txtEmail.Text);

                if (user != null)
                {
                    var newpassword =
                        EmailHelper.ForgotPasswordNotification(user.FirstName + " " + user.LastName, txtEmail.Text);

                    _accountManagement.UpdatePassword(txtEmail.Text, newpassword);

                    lblPwdSentMessage.Text = "Email with your login details has been sent.";
                    lblPwdSentMessage.Visible = true;
                }
                else
                {
                    lblPwdSentMessage.Visible = true;
                    lblPwdSentMessage.Text = "No such user found, please try again!";

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