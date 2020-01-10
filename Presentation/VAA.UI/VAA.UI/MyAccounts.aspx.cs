using Elmah;
using System;
using System.Web.UI;
using Telerik.Web.UI;
using VAA.CommonComponents;
using VAA.DataAccess;

namespace VAA.UI
{
    /// <summary>
    /// My account - Edit profile, uplaod photo, change password
    /// </summary>
    public partial class MyAccounts : Page
    {
        readonly AccountManagement _accountManagement = new AccountManagement();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(Session["USERID"])))
                    Response.Redirect("~/Account/Login.aspx");

                if (!string.IsNullOrEmpty(Convert.ToString(Session["USERID"])))
                {
                    if (!Page.IsPostBack)
                    {
                        PopulateUserDetails();
                    }
                }
                else
                {
                    Response.Redirect("~/Account/Login.aspx");
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        public void ChangePasswordClicked(object sender, EventArgs e)
        {
            lblPassword.Visible = false;
            lblPasswordTitle.Visible = false;
            btnChangePassword.Visible = false;
            plnChangePassword.Visible = true;
        }

        /// <summary>
        /// Upload profile picture
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void UploadBtnClicked(object sender, EventArgs e)
        {
            try
            {
                UploadedFile attachment = radUploadUserImage.UploadedFiles[0];
                // create byte array
                byte[] attachmentBytes = new byte[attachment.InputStream.Length];
                // read attachment into byte array 
                attachment.InputStream.Read(attachmentBytes, 0, attachmentBytes.Length);
                var userId = Convert.ToInt32(Session["USERID"]);
                bool IsUploaded = _accountManagement.UploadUserImage(userId, attachmentBytes);

                if (IsUploaded)
                {
                    radBinaryUserImage.DataValue = attachmentBytes;
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }



        public void CancelBtnClicked(object sender, EventArgs e)
        {
            Response.Redirect("MyAccounts.aspx");
        }
        public void UpdateBtnClicked(object sender, EventArgs e)
        {
            try
            {
                UpdateUserDetails();
                lblPassword.Visible = true;
                lblPasswordTitle.Visible = true;
                btnChangePassword.Visible = true;
                plnChangePassword.Visible = false;
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        /// <summary>
        /// Update user details
        /// </summary>
        private void UpdateUserDetails()
        {
            try
            {
                var userId = Convert.ToInt32(Session["USERID"]);
                var user = _accountManagement.GetUserById(userId);
                user.FirstName = txtFirstName.Text;
                user.LastName = txtLastName.Text;
                user.Department = txtDepartment.Text;
                user.Designation = txtDesignation.Text;
                user.Address1 = txtAddress1.Text;
                user.Address2 = txtAddress2.Text;
                user.Address3 = txtAddress3.Text;
                user.City = txtCity.Text;
                user.County = txtCounty.Text;
                user.Country = txtCountry.Text;
                user.Postcode = txtPostcode.Text;
                user.Mobile = txtMobileNumber.Text;
                user.Telephone = txtTelephoneNumber.Text;
                user.Hash = EncryptionHelper.Encrypt(txtBoxNewPassword.Text);
                bool IsUpdated = _accountManagement.UpdateMyAccount(user);
                if (!string.IsNullOrEmpty(txtBoxNewPassword.Text))
                    if (Request.Cookies["vaapassword"] != null)
                        Response.Cookies["vaapassword"].Expires = DateTime.Now.AddDays(-1);
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        /// <summary>
        /// Display user details
        /// </summary>
        private void PopulateUserDetails()
        {
            try
            {
                var userId = Convert.ToInt32(Session["USERID"]);
                var user = _accountManagement.GetUserById(userId);

                lblUserID.Text = Convert.ToString(user.Id);
                lblUserName.Text = user.Username;

                txtFirstName.Text = user.FirstName;
                txtLastName.Text = user.LastName;
                txtDepartment.Text = user.Department;
                txtDesignation.Text = user.Designation;
                txtAddress1.Text = user.Address1;
                txtAddress2.Text = user.Address2;
                txtAddress3.Text = user.Address3;
                txtCity.Text = user.City;
                txtCounty.Text = user.County;
                txtCountry.Text = user.Country;
                txtPostcode.Text = user.Postcode;
                txtMobileNumber.Text = user.Mobile;
                txtTelephoneNumber.Text = user.Telephone;
                radBinaryUserImage.DataValue = user.ProfileImage;
                //DataValue='<%# Eval("UserImage") == DBNull.Value? new System.Byte[0]: Eval("UserImage") %>'
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
    }
}