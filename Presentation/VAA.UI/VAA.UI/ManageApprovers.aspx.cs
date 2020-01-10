using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Elmah;
using VAA.DataAccess;

namespace VAA.UI
{
    /// <summary>
    /// Set the approvers, Translators, Caterers based on route - Origin
    /// </summary>
    public partial class ManageApprovers : System.Web.UI.Page
    {
        readonly MenuManagement _menuManagement = new MenuManagement();
        readonly RouteManagement _routeManagement = new RouteManagement();
        AccountManagement _accountManagement = new AccountManagement();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(Session["USERID"])))
                    Response.Redirect("~/Account/Login.aspx");

                if (!Page.IsPostBack)
                {
                    BindOrigin();
                    BindMenuClass();
                    plnChangeVirginApprover.Visible = true;
                    plnChangeCatererApp.Visible = true;
                    plnChangeTranslationApp.Visible = true;
                    BindVirginApprover();
                    BindCaterer();
                    BindTranslator();
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void BindOrigin()
        {
            try
            {
                ddlOrigin.Items.Clear();

                var locations = _routeManagement.GetAllLocations();

                foreach (var origin in locations)
                {
                    var location = origin.AirportCode + " - " + origin.AirportName;
                    ddlOrigin.Items.Add(new ListItem(location, Convert.ToString(origin.LocationID)));
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        private void BindMenuClass()
        {
            ddlClass.Items.Clear();
            plnVirginApprover.Visible = false;
            plnCatererApp.Visible = false;
            plnTranslationApp.Visible = false;
            plnChangeVirginApprover.Visible = true;
            plnChangeCatererApp.Visible = true;
            plnChangeTranslationApp.Visible = true;
            BindVirginApprover();
            BindCaterer();
            BindTranslator();
            btnChangeApprover.Visible = false;

            var classes = _menuManagement.GetAllClass();
            ddlClass.Items.Add(new ListItem("- Please Select -", "0"));
            foreach (var menuclass in classes)
            {
                ddlClass.Items.Add(new ListItem(menuclass.FlightClass, Convert.ToString(menuclass.ID)));
            }
        }

        private void BindApprovers()
        {
            try
            {
                var selectedOrigin = ddlOrigin.SelectedValue;
                var selectedClass = ddlClass.SelectedValue;

                if ((!string.IsNullOrEmpty(selectedOrigin)) && (!string.IsNullOrEmpty(selectedClass)))
                {
                    var approversList = _accountManagement.GetApproversByOrginAndClass(Convert.ToInt32(selectedOrigin), Convert.ToInt32(selectedClass));

                    if (approversList.Count > 0)
                    {
                        foreach (var approver in approversList)
                        {
                            if (approver.VirginApproverID != null)
                            {
                                plnVirginApprover.Visible = true;
                                plnChangeVirginApprover.Visible = false;
                                var name = GetApproverbyId(Convert.ToInt32(approver.VirginApproverID));
                                lblCurrVirginApp.Text = name;
                                btnChangeApprover.Visible = true;
                            }
                            else
                            {
                                plnVirginApprover.Visible = false;
                                plnChangeVirginApprover.Visible = true;
                                BindVirginApprover();
                            }
                            if (approver.CatererID != null)
                            {
                                plnChangeCatererApp.Visible = false;
                                plnCatererApp.Visible = true;
                                var name = GetApproverbyId(Convert.ToInt32(approver.CatererID));
                                lblCurrCatererApp.Text = name;
                                btnChangeApprover.Visible = true;
                            }
                            else
                            {
                                plnCatererApp.Visible = false;
                                plnChangeCatererApp.Visible = true;
                                BindCaterer();
                            }
                            if (approver.TranslatorID != null)
                            {
                                plnChangeTranslationApp.Visible = false;
                                plnTranslationApp.Visible = true;
                                var name = GetApproverbyId(Convert.ToInt32(approver.TranslatorID));
                                lblCurrTranslationApp.Text = name;
                                btnChangeApprover.Visible = true;
                            }
                            else
                            {
                                plnTranslationApp.Visible = false;
                                plnChangeTranslationApp.Visible = true;
                                BindTranslator();
                            }
                        }
                    }
                    else
                    {
                        plnVirginApprover.Visible = false;
                        plnCatererApp.Visible = false;
                        plnTranslationApp.Visible = false;
                        plnChangeVirginApprover.Visible = true;
                        plnChangeCatererApp.Visible = true;
                        plnChangeTranslationApp.Visible = true;
                        BindVirginApprover();
                        BindCaterer();
                        BindTranslator();
                        btnChangeApprover.Visible = false;
                    }

                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        protected void ddlOrigin_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMenuClass();
        }
        protected void ddlClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindApprovers();
        }

        private string GetApproverbyId(int id)
        {
            var virginApp = _accountManagement.GetUserById(Convert.ToInt32(id));
            var approverName = virginApp.FirstName + " " + virginApp.LastName;
            return approverName;
        }

        private void BindVirginApprover()
        {
            try
            {
                ddlVirginApprover.Items.Clear();

                var userType = "Virgin";
                ddlVirginApprover.Items.Add(new ListItem("- Please Select -", "0"));
                var users = _accountManagement.GetAllUsersByUserType(userType);
                foreach (var user in users)
                {
                    var virginUser = user.FirstName + " " + user.LastName;
                    ddlVirginApprover.Items.Add(new ListItem(virginUser, Convert.ToString(user.Id)));
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        private void BindCaterer()
        {
            try
            {
                ddlCatererApprover.Items.Clear();

                var userType = "Caterer";
                ddlCatererApprover.Items.Add(new ListItem("- Please Select -", "0"));
                var users = _accountManagement.GetAllUsersByUserType(userType);
                foreach (var user in users)
                {
                    var caterer = user.FirstName + " " + user.LastName;
                    ddlCatererApprover.Items.Add(new ListItem(caterer, Convert.ToString(user.Id)));
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        private void BindTranslator()
        {
            try
            {
                ddlTranslationApprover.Items.Clear();

                var userType = "Translator";
                ddlTranslationApprover.Items.Add(new ListItem("- Please Select -", "0"));
                var users = _accountManagement.GetAllUsersByUserType(userType);
                foreach (var user in users)
                {
                    var translator = user.FirstName + " " + user.LastName;
                    ddlTranslationApprover.Items.Add(new ListItem(translator, Convert.ToString(user.Id)));
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        public void ChangeApproverClicked(object sender, EventArgs e)
        {
            try
            {
                plnVirginApprover.Visible = false;
                plnCatererApp.Visible = false;
                plnTranslationApp.Visible = false;
                plnChangeVirginApprover.Visible = true;
                plnChangeCatererApp.Visible = true;
                plnChangeTranslationApp.Visible = true;

                var selectedOrigin = ddlOrigin.SelectedValue;
                var selectedClass = ddlClass.SelectedValue;

                if ((!string.IsNullOrEmpty(selectedOrigin)) && (!string.IsNullOrEmpty(selectedClass)))
                {
                    var approversList = _accountManagement.GetApproversByOrginAndClass(Convert.ToInt32(selectedOrigin), Convert.ToInt32(selectedClass));

                    foreach (var approver in approversList)
                    {
                        if (approver.VirginApproverID != null)
                        {
                            ddlVirginApprover.Text = Convert.ToString(approver.VirginApproverID);
                        }
                        else
                        {
                            BindVirginApprover();
                        }
                        if (approver.CatererID != null)
                        {
                            ddlCatererApprover.Text = Convert.ToString(approver.CatererID);
                        }
                        else
                        {
                            BindCaterer();
                        }
                        if (approver.TranslatorID != null)
                        {
                            ddlTranslationApprover.Text = Convert.ToString(approver.TranslatorID);
                        }
                        else
                        {
                            BindTranslator();
                        }
                    }
                }
                btnChangeApprover.Visible = false;
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        public void btnUpdateApprover_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    var originLocationId = Convert.ToInt32(ddlOrigin.SelectedItem.Value);
                    var classID = Convert.ToInt32(ddlClass.SelectedItem.Value);
                    var virginApproverID = Convert.ToInt32(ddlVirginApprover.SelectedItem.Value);
                    var catererID = Convert.ToInt32(ddlCatererApprover.SelectedItem.Value);
                    var translatorID = Convert.ToInt32(ddlTranslationApprover.SelectedItem.Value);
                    if (translatorID != 0)
                    {
                        bool isAdded = _accountManagement.AddUpdateApprovers(originLocationId, classID, virginApproverID, catererID, translatorID);
                        if (isAdded == true)
                        {
                            //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Approvers Added!');", true);

                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "ApproverAdded();", true);

                            plnVirginApprover.Visible = true;
                            plnCatererApp.Visible = true;
                            plnTranslationApp.Visible = true;
                            plnChangeVirginApprover.Visible = false;
                            plnChangeCatererApp.Visible = false;
                            plnChangeTranslationApp.Visible = false;
                            BindApprovers();
                            //lblmessage.Visible = true;
                            //lblmessage.Text = "Approvers Added!";                           
                        }
                        else
                        {
                            //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Cannot add approver!');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "ApproverError();", true);
                            //lblmessage.Visible = true;
                            //lblmessage.Text = "Sorry cannot add!";
                        }
                    }
                    else
                    {
                        bool isAdded = _accountManagement.AddUpdateApprovers(originLocationId, classID, virginApproverID, catererID);
                        if (isAdded == true)
                        {
                            //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Approvers Added!');", true); 
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "ApproverAdded();", true);

                            plnVirginApprover.Visible = true;
                            plnCatererApp.Visible = true;
                            plnTranslationApp.Visible = false;
                            plnChangeVirginApprover.Visible = false;
                            plnChangeCatererApp.Visible = false;
                            plnChangeTranslationApp.Visible = true;
                            BindApprovers();
                            //lblmessage.Visible = true;
                            //lblmessage.Text = "Approvers Added!";                       
                        }
                        else
                        {
                            //ClientScript.RegisterStartupScript(this.GetType(), "myalert", "alert('Cannot add approver!');", true);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ALERT", "ApproverError();", true);
                            //lblmessage.Visible = true;
                            //lblmessage.Text = "Sorry cannot add!";
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
    }
}