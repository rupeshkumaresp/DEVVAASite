using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using VAA.DataAccess;
using VAA.DataAccess.Model;

namespace VAA.UI
{
    /// <summary>
    /// Add, edit or Delete Users - Virgin, ESP, Caterer Or Translator
    /// </summary>
    public partial class AddEdit_User : System.Web.UI.Page
    {
        AccountManagement _accountManagement = new AccountManagement();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["USERID"])))
                Response.Redirect("~/Account/Login.aspx");

            if (!IsPostBack)
            {
                try
                {
                    //Bind Dropdown
                    ddlUsertype.Items.Clear();
                    ddlUsertype.Items.Add(new ListItem("--Please select--", "NA"));
                    List<string> userTypes = new List<string>() { "ESP", "Virgin", "Caterer", "Translator", "VirginCrew" };
                    foreach (string usertype in userTypes)
                    {
                        ddlUsertype.Items.Add(usertype);
                    }

                    string rowId = Request.QueryString["ID"];
                    if (!string.IsNullOrEmpty(rowId))
                    {
                        int Id = 0;
                        bool RID = Int32.TryParse(rowId, out Id);
                        if (RID)
                        {
                            Session["CURRENTUSERID"] = Id;
                            if (Id > 0)
                            {
                                btnUpdateUser.Visible = true;
                                lblUserActionType.Text = "Update User";

                                //Get All User data 
                                var userData = _accountManagement.GetUserById(Id);
                                if (userData != null)
                                {
                                    txtUserName.Text = userData.Username;
                                    txtPassword.Text = CommonComponents.EncryptionHelper.Decrypt(userData.Hash);
                                    txtReTypePassword.Text = CommonComponents.EncryptionHelper.Decrypt(userData.Hash);
                                    txtFirstName.Text = userData.FirstName;
                                    txtLastName.Text = userData.LastName;
                                    ddlUsertype.Text = userData.UserType;
                                    txtDepartment.Text = userData.Department;
                                    txtDesignation.Text = userData.Designation;
                                    txtAddress1.Text = userData.Address1;
                                    txtAddress2.Text = userData.Address2;
                                    txtAddress3.Text = userData.Address3;
                                    txtCity.Text = userData.City;
                                    txtPostcode.Text = userData.Postcode;
                                    txtCounty.Text = userData.County;
                                    txtCountry.Text = userData.Country;
                                    txtMobile.Text = userData.Mobile;
                                    txtTelephone.Text = userData.Telephone;

                                    //Set Permission
                                    rblIsSuper.ClearSelection();
                                    if (userData.IsSuper) rblIsSuper.SelectedIndex = 0;
                                    else rblIsSuper.SelectedIndex = 1;

                                    rblCanImport.ClearSelection();
                                    if (userData.CanImport) rblCanImport.SelectedIndex = 0;
                                    else rblCanImport.SelectedIndex = 1;

                                    rblCanExport.ClearSelection();
                                    if (userData.CanExport) rblCanExport.SelectedIndex = 0;
                                    else rblCanExport.SelectedIndex = 1;

                                    rblIsBlocked.ClearSelection();
                                    if (Convert.ToBoolean(userData.IsBlocked)) rblIsBlocked.SelectedIndex = 0;
                                    else rblIsBlocked.SelectedIndex = 1;
                                }
                            }
                            else if (Id == 0)
                            {
                                btnAddNewUser.Visible = true;
                                lblUserActionType.Text = "Add New User";
                            }
                            else
                            {
                                btnAddNewUser.Visible = false;
                                btnUpdateUser.Visible = false;
                                lblUserActionType.Text = "";
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "mykey", "CancelEdit();", true);
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "mykey", "CancelEdit();", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "mykey", "CancelEdit();", true);
                    }
                }
                catch (Exception ex)
                {
                    //write to Elma
                    ErrorSignal.FromCurrentContext().Raise(ex);
                }
            }
            txtPassword.Attributes["type"] = "password";
            txtReTypePassword.Attributes["type"] = "password";
        }

        /// <summary>
        /// Add new user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAddNewUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    User newuser = new User()
                    {
                        Username = txtUserName.Text,
                        Hash = txtPassword.Text,
                        FirstName = txtFirstName.Text,
                        LastName = txtLastName.Text,
                        UserType = ddlUsertype.SelectedItem.Value,
                        Department = txtDepartment.Text,
                        Designation = txtDesignation.Text,
                        Address1 = txtAddress1.Text,
                        Address2 = txtAddress2.Text,
                        Address3 = txtAddress3.Text,
                        City = txtCity.Text,
                        Postcode = txtPostcode.Text,
                        County = txtCounty.Text,
                        Country = txtCountry.Text,
                        Mobile = txtMobile.Text,
                        Telephone = txtTelephone.Text,
                        IsSuper = rblIsSuper.SelectedItem.Value.ToUpper().Trim() == "YES" ? true : false,
                        IsBlocked = rblIsBlocked.SelectedItem.Value.ToUpper().Trim() == "YES" ? true : false,
                        CanImport = rblCanImport.SelectedItem.Value.ToUpper().Trim() == "YES" ? true : false,
                        CanExport = rblCanExport.SelectedItem.Value.ToUpper().Trim() == "YES" ? true : false,
                    };

                    string currentUserId = Convert.ToString(Session["USERID"]);
                    if (!string.IsNullOrEmpty(currentUserId))
                    {
                        int userid = 0;
                        Int32.TryParse(currentUserId, out userid);
                        if (userid > 0)
                        {
                            bool isCreated = _accountManagement.CreateUser(newuser, userid);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "userKey", "CloseAndRebind();", true);
                        }
                    }
                    else
                    {
                        Response.Redirect("~/Account/Login.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        /// <summary>
        /// Edit User
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpdateUser_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {

                    string rowId = Request.QueryString["ID"];
                    if (!string.IsNullOrEmpty(rowId))
                    {
                        int Id = 0;
                        bool RID = Int32.TryParse(rowId, out Id);
                        if (RID)
                        {
                            Session["CURRENTUSERID"] = Id;
                            if (Id > 0)
                            {
                                User newuser = new User()
                                {
                                    Id = Id,
                                    Username = txtUserName.Text,
                                    Hash = txtPassword.Text,
                                    FirstName = txtFirstName.Text,
                                    LastName = txtLastName.Text,
                                    UserType = ddlUsertype.SelectedItem.Value,
                                    Department = txtDepartment.Text,
                                    Designation = txtDesignation.Text,
                                    Address1 = txtAddress1.Text,
                                    Address2 = txtAddress2.Text,
                                    Address3 = txtAddress3.Text,
                                    City = txtCity.Text,
                                    Postcode = txtPostcode.Text,
                                    County = txtCounty.Text,
                                    Country = txtCountry.Text,
                                    Mobile = txtMobile.Text,
                                    Telephone = txtTelephone.Text,
                                    IsSuper = rblIsSuper.SelectedItem.Value.ToUpper().Trim() == "YES" ? true : false,
                                    IsBlocked = rblIsBlocked.SelectedItem.Value.ToUpper().Trim() == "YES" ? true : false,
                                    CanImport = rblCanImport.SelectedItem.Value.ToUpper().Trim() == "YES" ? true : false,
                                    CanExport = rblCanExport.SelectedItem.Value.ToUpper().Trim() == "YES" ? true : false,
                                };

                                string currentUserId = Convert.ToString(Session["USERID"]);
                                if (!string.IsNullOrEmpty(currentUserId))
                                {
                                    int userid = 0;
                                    Int32.TryParse(currentUserId, out userid);
                                    if (userid > 0)
                                    {
                                        bool IsUpdated = _accountManagement.UpdateUser(newuser, userid);
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "userKey", "CloseAndRebind();", true);
                                    }
                                }
                                else
                                    Response.Redirect("~/Account/Login.aspx");
                            }
                            else
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "userKey", "CloseAndRebind();", true);
                        }
                        else
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "userKey", "CloseAndRebind();", true);
                    }
                    else
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "userKey", "CloseAndRebind();", true);
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