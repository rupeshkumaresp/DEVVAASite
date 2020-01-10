using System;
using System.Linq;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VAA.DataAccess;

namespace VAA.UI
{
    public partial class Users : Page
    {
        /// <summary>
        /// Manage users for system - Add/Edit/Delete
        /// </summary>
        readonly AccountManagement _accountManagement = new AccountManagement();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(Convert.ToString(Session["USERID"])))
                Response.Redirect("~/Account/Login.aspx");

            if (!IsPostBack)
            {
                BindUserList();
            }
        }

        private void BindUserList()
        {
            var userData = _accountManagement.GetAllUsers();
            var userDataSource = (from x in userData
                                  where x.IsBlocked == false
                                  select new
                                  {
                                      Name = x.FirstName.ToUpperInvariant() + " " + x.LastName.ToUpperInvariant(),
                                      UserId = x.Id,
                                      UserName = x.Username,
                                      UserType = x.UserType,
                                      City = x.City,
                                      Country = x.Country,
                                      Phone = x.Mobile,
                                      UserImage = x.ProfileImage
                                  }).ToList();
            radListViewUsers.DataSource = userDataSource;
            radListViewUsers.DataBind();
        }

        protected void rblSortUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            RadioButtonList rbl = sender as RadioButtonList;
            RadComboBox combo = radListViewUsers.FindControl("ddListUserSort") as RadComboBox;
            if (combo != null && (combo.SelectedItem.Value != String.Empty && combo.SelectedItem.Value != "ClearSort"))
            {
                switch (rbl.SelectedValue)
                {
                    case "ASC":
                        radListViewUsers.Items[0].FireCommandEvent(RadListView.SortCommandName, combo.SelectedValue + " ASC");
                        break;
                    case "DESC":
                        radListViewUsers.Items[0].FireCommandEvent(RadListView.SortCommandName, combo.SelectedValue + " DESC");
                        break;
                    default:
                        break;
                }
            }
        }

        protected void ddListUserSort_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadioButtonList rbl = radListViewUsers.FindControl("rblSortUser") as RadioButtonList;
            switch (e.Value)
            {
                case "UserName":
                    radListViewUsers.Items[0].FireCommandEvent(RadListView.SortCommandName, "UserName");
                    rbl.SelectedIndex = 0;
                    break;
                case "Name":
                    radListViewUsers.Items[0].FireCommandEvent(RadListView.SortCommandName, "Name");
                    rbl.SelectedIndex = 0;
                    break;
                case "UserType":
                    radListViewUsers.Items[0].FireCommandEvent(RadListView.SortCommandName, "UserType");
                    rbl.SelectedIndex = 0;
                    break;
                case "City":
                    radListViewUsers.Items[0].FireCommandEvent(RadListView.SortCommandName, "City");
                    rbl.SelectedIndex = 0;
                    break;
                case "Country":
                    radListViewUsers.Items[0].FireCommandEvent(RadListView.SortCommandName, "Country");
                    rbl.SelectedIndex = 0;
                    break;
                case "ClearSort":
                    radListViewUsers.SortExpressions.Clear();
                    radListViewUsers.Rebind();
                    rbl.SelectedIndex = 0;
                    break;
                default:
                    break;
            }
        }

        protected void radListViewUsers_ItemCommand(object sender, RadListViewCommandEventArgs e)
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string userId = Convert.ToString(e.CommandArgument);
                Session["DELUSERID"] = userId;
                radWindowManager.RadConfirm("Are you sure you want to delete this user, please confirm?", "confirmCallBackUserFn", 300, 150, null, "Delete Confirmation", userId);
            }
            else if (e.CommandName.ToUpper().Equals("REFRESH"))
            {
                radListViewUsers.DataSource = null;
                BindUserList();
            }
            else if (e.CommandName.ToUpper().Equals("INSERT"))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "KEY", "ShowInsertForm();", true);
            }
        }

        protected void radListViewUsers_ItemDataBound(object sender, RadListViewItemEventArgs e)
        {
            if (e.Item is RadListViewDataItem)
            {
                var dataItem = ((RadListViewDataItem)e.Item).DataItem;
                Label lblId = e.Item.FindControl("lblListUserId") as Label;

                RadButton editLink = (RadButton)e.Item.FindControl("EditLink");
                editLink.Attributes["href"] = "javascript:void(0);";
                editLink.Attributes["onclick"] = String.Format("return ShowEditForm('{0}');", lblId.Text);
            }
        }

        protected void radListViewUsers_NeedDataSource(object sender, RadListViewNeedDataSourceEventArgs e)
        {
            if (IsPostBack)
            {
                var userData = _accountManagement.GetAllUsers();
                var userDataSource = (from x in userData
                                      where x.IsBlocked == false
                                      select new
                                      {
                                          Name = x.FirstName.ToUpperInvariant() + " " + x.LastName.ToUpperInvariant(),
                                          UserId = x.Id,
                                          UserName = x.Username,
                                          UserType = x.UserType,
                                          City = x.City,
                                          Country = x.Country,
                                          Phone = x.Mobile,
                                          UserImage = x.ProfileImage
                                      }).ToList();
                radListViewUsers.DataSource = userDataSource;
            }
        }

        protected void radAjaxManagerUser_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                radListViewUsers.Rebind();
            }
        }

        [WebMethod]
        public static bool DeleteUser()
        {
            Users cycleObj = new Users();
            return cycleObj.DeleteUserListData();
        }

        public bool DeleteUserListData()
        {
            string userId = Convert.ToString(Session["DELUSERID"]);
            if (!string.IsNullOrEmpty(userId))
            {
                int Id = 0;
                bool isUserId = Int32.TryParse(userId, out Id);
                if (isUserId)
                {
                    bool cycleDeleted = _accountManagement.DeleteUser(Id);
                }
                Session["DELUSERID"] = string.Empty;
                return true;
            }
            else
            {
                Session["DELUSERID"] = string.Empty;
                return false;
            }
        }
    }
}