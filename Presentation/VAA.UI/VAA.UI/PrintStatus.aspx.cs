using Elmah;
using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using VAA.DataAccess;
using VAA.DataAccess.Model;

namespace VAA.UI
{
    /// <summary>
    /// Print Status - approval status workflow handling
    /// </summary>
    public partial class PrintStatus : System.Web.UI.Page
    {
        readonly CycleManagement _cycleManagement = new CycleManagement();
        readonly MenuManagement _menuManagement = new MenuManagement();
        readonly OrderManagement _orderManagement = new OrderManagement();


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(Session["USERID"])))
                    Response.Redirect("~/Account/Login.aspx");

                var userId = Convert.ToString(Session["USERID"]);
                if (!string.IsNullOrEmpty(userId))
                {
                    if (!Page.IsPostBack)
                    {
                        BindCycle();
                        BindMenuClass();
                        BindMenuType();
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

        protected void BtnViewStatusClick(object sender, EventArgs e)
        {
            try
            {
                PrintstatusGridDiv.Visible = true;
                GridDataSource();
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        #region bind dropdowns
        private void BindMenuClass()
        {
            ddlMenuClass.Items.Clear();

            var classes = _menuManagement.GetAllClass();

            foreach (var menuclass in classes)
            {
                ddlMenuClass.Items.Add(new ListItem(menuclass.FlightClass, Convert.ToString(menuclass.ID)));
            }
        }

        private void BindMenuType()
        {
            ddlMenuType.Items.Clear();

            var selectedClassId = ddlMenuClass.SelectedValue;

            if (!string.IsNullOrEmpty(selectedClassId))
            {
                var menuTypes = _menuManagement.GetMenuTypeByClass(Convert.ToInt32(selectedClassId));

                foreach (var types in menuTypes)
                {
                    ddlMenuType.Items.Add(new ListItem(types.DisplayName, Convert.ToString(types.ID)));
                }
            }
        }

        private void BindCycle()
        {
            var cycles = _cycleManagement.GetCycles();

            ddlCycle.Items.Clear();
            foreach (var cycle in cycles)
            {
                ddlCycle.Items.Add(new ListItem(cycle.CycleName, Convert.ToString(cycle.Id)));
            }
        }

        protected void ddlMenuClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindMenuType();
        }
        #endregion


        private void GridDataSource()
        {
            try
            {
                var cycle = ddlCycle.SelectedValue;
                var menuclass = ddlMenuClass.SelectedValue;
                var menutype = ddlMenuType.SelectedValue;
                var userId = Convert.ToInt32(Session["USERID"]);

                var data = _orderManagement.GetMenuStatusAndApprovers(Convert.ToInt32(cycle), Convert.ToInt16(menuclass), Convert.ToInt16(menutype), userId);
                var approvalstatus = _orderManagement.GetAllApprovalStatus();

                string Proof1 = (from x in approvalstatus where x.ApprovalStatusId == 1 select x.ApprovalStatusName).FirstOrDefault();
                string Proof2 = (from x in approvalstatus where x.ApprovalStatusId == 2 select x.ApprovalStatusName).FirstOrDefault();
                string Proof3 = (from x in approvalstatus where x.ApprovalStatusId == 3 select x.ApprovalStatusName).FirstOrDefault();
                string Proof4 = (from x in approvalstatus where x.ApprovalStatusId == 4 select x.ApprovalStatusName).FirstOrDefault();
                string Proof5 = (from x in approvalstatus where x.ApprovalStatusId == 5 select x.ApprovalStatusName).FirstOrDefault();
                var DataToBindInGrid = (from menudata in data
                                        where menudata.IsMovedToLiveOrder != true
                                        select new
                                        {
                                            MenuId = menudata.Id,
                                            MenuCode = menudata.MenuCode,
                                            MenuName = menudata.MenuName,
                                            PrintOrderStatus = menudata.ApprovalStatusId,
                                            LanguageID = menudata.LanguageId,
                                            LanguageName = menudata.LanguageName,
                                            Proof1 = Proof1,
                                            Proof2 = Proof2,
                                            Proof3 = Proof3,
                                            Proof4 = Proof4,
                                            Proof5 = Proof5,
                                            VirginApproverId = menudata.VirginApproverId,
                                            VirginApprover = menudata.VirginAppFirstName + " " + menudata.VirginAppLastName,
                                            CatererApproverId = menudata.CatererApproverId,
                                            CatererApprover = menudata.CatererAppFirstName + " " + menudata.CatererAppLastName,
                                            TranslatorApproverId = menudata.TranslatorApproverId,
                                            TranslatorApprover = menudata.TranslatorAppFirstName + " " + menudata.TranslatorAppLastName
                                        }).ToList();
                radGridPrintStatus.DataSource = DataToBindInGrid;
                radGridPrintStatus.DataBind();


                var allApproved = true;
                foreach (var d in DataToBindInGrid)
                {
                    if (d.PrintOrderStatus != 6)
                    {
                        allApproved = false;
                        break;
                    }
                }

                if (DataToBindInGrid == null || DataToBindInGrid.Count == 0)
                {
                    btnApproveAll.Visible = false;
                    btnMoveAlltoOrder.Visible = false;
                }
                else
                {
                    btnApproveAll.Visible = true;
                    btnMoveAlltoOrder.Visible = true;
                }

                if (allApproved && DataToBindInGrid.Count > 0)
                    btnApproveAll.Visible = false;

            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void radGridPrintStatus_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (IsPostBack)
                {
                    var cycle = ddlCycle.SelectedValue;
                    var menuclass = ddlMenuClass.SelectedValue;
                    var menutype = ddlMenuType.SelectedValue;
                    var userId = Convert.ToInt32(Session["USERID"]);
                    var data = _orderManagement.GetMenuStatusAndApprovers(Convert.ToInt32(cycle), Convert.ToInt16(menuclass), Convert.ToInt16(menutype), userId);
                    var approvalstatus = _orderManagement.GetAllApprovalStatus();

                    string Proof1 = (from x in approvalstatus where x.ApprovalStatusId == 1 select x.ApprovalStatusName).FirstOrDefault();
                    string Proof2 = (from x in approvalstatus where x.ApprovalStatusId == 2 select x.ApprovalStatusName).FirstOrDefault();
                    string Proof3 = (from x in approvalstatus where x.ApprovalStatusId == 3 select x.ApprovalStatusName).FirstOrDefault();
                    string Proof4 = (from x in approvalstatus where x.ApprovalStatusId == 4 select x.ApprovalStatusName).FirstOrDefault();
                    string Proof5 = (from x in approvalstatus where x.ApprovalStatusId == 5 select x.ApprovalStatusName).FirstOrDefault();
                    var DataToBindInGrid = (from menudata in data
                                            where menudata.IsMovedToLiveOrder != true
                                            select new
                                            {
                                                MenuId = menudata.Id,
                                                MenuCode = menudata.MenuCode,
                                                MenuName = menudata.MenuName,
                                                PrintOrderStatus = menudata.ApprovalStatusId,
                                                LanguageID = menudata.LanguageId,
                                                LanguageName = menudata.LanguageName,
                                                Proof1 = Proof1,
                                                Proof2 = Proof2,
                                                Proof3 = Proof3,
                                                Proof4 = Proof4,
                                                Proof5 = Proof5,
                                                VirginApproverId = menudata.VirginApproverId,
                                                VirginApprover = menudata.VirginAppFirstName + " " + menudata.VirginAppLastName,
                                                CatererApproverId = menudata.CatererApproverId,
                                                CatererApprover = menudata.CatererAppFirstName + " " + menudata.CatererAppLastName,
                                                TranslatorApproverId = menudata.TranslatorApproverId,
                                                TranslatorApprover = menudata.TranslatorAppFirstName + " " + menudata.TranslatorAppLastName
                                            }).ToList();
                    radGridPrintStatus.DataSource = DataToBindInGrid;
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void radGridPrintStatus_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    string rowId = item["MenuId"].Text;
                    LinkButton editLink = (LinkButton)item.FindControl("gvlnkbtnViewMenu");
                    editLink.Attributes["href"] = "javascript:void(0);";
                    editLink.Attributes["onclick"] = String.Format("return ShowEditForm('{0}','{1}');", rowId, e.Item.ItemIndex);
                }

                if (e.Item.ItemType == GridItemType.NestedView)
                {
                    GridNestedViewItem NestedView = (GridNestedViewItem)e.Item;
                    if (NestedView != null)
                    {

                        Label lblVirginAppId = e.Item.FindControl("lblVirginAppId") as Label;
                        string virginUserid = lblVirginAppId.Text;
                        LinkButton link3 = e.Item.FindControl("LinkButton3") as LinkButton;
                        link3.Attributes["href"] = "javascript:void(0);";
                        link3.Attributes["onclick"] = String.Format("return ShowViewApprover('{0}');", virginUserid);

                        Label lblVirginAppId1 = e.Item.FindControl("lblVirginAppId") as Label;
                        string virginUserid1 = lblVirginAppId1.Text;
                        LinkButton link5 = e.Item.FindControl("LinkButton5") as LinkButton;
                        link5.Attributes["href"] = "javascript:void(0);";
                        link5.Attributes["onclick"] = String.Format("return ShowViewApprover('{0}');", virginUserid1);

                        Label lblVirginAppId2 = e.Item.FindControl("lblVirginAppId") as Label;
                        string virginUserid2 = lblVirginAppId2.Text;
                        LinkButton link7 = e.Item.FindControl("LinkButton7") as LinkButton;
                        link7.Attributes["href"] = "javascript:void(0);";
                        link7.Attributes["onclick"] = String.Format("return ShowViewApprover('{0}');", virginUserid2);

                        Label lblVirginAppId3 = e.Item.FindControl("lblVirginAppId2") as Label;
                        string virginUserid3 = lblVirginAppId3.Text;
                        LinkButton link = e.Item.FindControl("LinkButton") as LinkButton;
                        link.Attributes["href"] = "javascript:void(0);";
                        link.Attributes["onclick"] = String.Format("return ShowViewApprover('{0}');", virginUserid3);

                        Label lblVirginAppId4 = e.Item.FindControl("lblVirginAppId2") as Label;
                        string virginUserid4 = lblVirginAppId4.Text;
                        LinkButton link2 = e.Item.FindControl("LinkButton2") as LinkButton;
                        link2.Attributes["href"] = "javascript:void(0);";
                        link2.Attributes["onclick"] = String.Format("return ShowViewApprover('{0}');", virginUserid4);

                        Label lblCatererAppId = e.Item.FindControl("lblCatererAppId") as Label;
                        string catererUserid = lblCatererAppId.Text;
                        LinkButton link4 = e.Item.FindControl("LinkButton4") as LinkButton;
                        link4.Attributes["href"] = "javascript:void(0);";
                        link4.Attributes["onclick"] = String.Format("return ShowViewApprover('{0}');", catererUserid);

                        Label lblCatererAppId2 = e.Item.FindControl("lblCatererAppId2") as Label;
                        string catererUserid2 = lblCatererAppId2.Text;
                        LinkButton link1 = e.Item.FindControl("LinkButton1") as LinkButton;
                        link1.Attributes["href"] = "javascript:void(0);";
                        link1.Attributes["onclick"] = String.Format("return ShowViewApprover('{0}');", catererUserid2);

                        Label lblTranslatorAppId = e.Item.FindControl("lblTranslatorAppId") as Label;
                        string translatorUserid = lblTranslatorAppId.Text;
                        LinkButton link6 = e.Item.FindControl("LinkButton6") as LinkButton;
                        link6.Attributes["href"] = "javascript:void(0);";
                        link6.Attributes["onclick"] = String.Format("return ShowViewApprover('{0}');", translatorUserid);


                    }
                }


                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = (GridDataItem)e.Item;

                    Label printStatus = e.Item.FindControl("gvlblPrintOrderStatus") as Label;
                    string printStatusID = printStatus.Text;
                    Label language = e.Item.FindControl("gvlblLanguageId") as Label;
                    string languageID = language.Text;
                    Image imgStatus = e.Item.FindControl("gvimgPrintOrderStatus") as Image;
                    LinkButton lnkbtnordernow = e.Item.FindControl("gvlnkbtnOrderNow") as LinkButton;
                    lnkbtnordernow.ForeColor = System.Drawing.ColorTranslator.FromHtml("#818181");
                    lnkbtnordernow.Enabled = false;
                    if (languageID == "1")
                    {
                        if (Convert.ToInt32(printStatusID) == 1)
                        {
                            imgStatus.ImageUrl = "~/Images/ApprovalStatus/EngStatus1InProgress.png";
                        }
                        else if (Convert.ToInt32(printStatusID) == 2)
                        {
                            imgStatus.ImageUrl = "~/Images/ApprovalStatus/EngStatus2InProgress.png";
                        }
                        else if (Convert.ToInt32(printStatusID) == 3)
                        {
                            imgStatus.ImageUrl = "~/Images/ApprovalStatus/EngStatus3InProgress.png";
                        }
                        else if (Convert.ToInt32(printStatusID) == 6)
                        {
                            var activeCycle = _cycleManagement.GetActiveCycle();
                            var selectedCycle = Convert.ToInt64(ddlCycle.SelectedValue);
                            if (activeCycle.Id == selectedCycle)
                            {
                                lnkbtnordernow.ForeColor = System.Drawing.ColorTranslator.FromHtml("#B72B3C");
                                lnkbtnordernow.Enabled = true;
                            }
                            imgStatus.ImageUrl = "~/Images/ApprovalStatus/EngStatusApproved.png";
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(printStatusID) == 1)
                        {
                            imgStatus.ImageUrl = "~/Images/ApprovalStatus/OthStatus1InProgress.png";
                        }
                        else if (Convert.ToInt32(printStatusID) == 2)
                        {
                            imgStatus.ImageUrl = "~/Images/ApprovalStatus/OthStatus2InProgress.png";
                        }
                        else if (Convert.ToInt32(printStatusID) == 3)
                        {
                            imgStatus.ImageUrl = "~/Images/ApprovalStatus/OthStatus3InProgress.png";
                        }
                        else if (Convert.ToInt32(printStatusID) == 4)
                        {
                            imgStatus.ImageUrl = "~/Images/ApprovalStatus/OthStatus4InProgress.png";
                        }
                        else if (Convert.ToInt32(printStatusID) == 5)
                        {
                            imgStatus.ImageUrl = "~/Images/ApprovalStatus/OthStatus5InProgress.png";
                        }
                        else if (Convert.ToInt32(printStatusID) == 6)
                        {
                            var activeCycle = _cycleManagement.GetActiveCycle();
                            var selectedCycle = Convert.ToInt64(ddlCycle.SelectedValue);
                            if (activeCycle.Id == selectedCycle)
                            {
                                lnkbtnordernow.ForeColor = System.Drawing.ColorTranslator.FromHtml("#B72B3C");
                                lnkbtnordernow.Enabled = true;
                            }
                            imgStatus.ImageUrl = "~/Images/ApprovalStatus/OthStatusApproved.png";
                        }
                    }
                }

                if (e.Item.ItemType == GridItemType.NestedView)
                {
                    GridNestedViewItem NestedView = (GridNestedViewItem)e.Item;
                    if (NestedView != null)
                    {
                        Label printStatus = e.Item.FindControl("gvlblNestPrintOrderStatus") as Label;
                        string printStatusID = printStatus.Text;
                        Label language = e.Item.FindControl("gvlblNestLanguageId") as Label;
                        string languageID = language.Text;
                        Panel pnlForFive = e.Item.FindControl("pnlLanguageForFive") as Panel;
                        Panel pnlForThree = e.Item.FindControl("pnlLanguageForThree") as Panel;
                        Label lblProof33 = e.Item.FindControl("gvlblProof33") as Label;
                        if (languageID == "1")
                        {
                            pnlForFive.Visible = false;
                            pnlForThree.Visible = true;

                            Image imgApproveStatus1 = e.Item.FindControl("gvimgProof31") as Image;
                            Image imgApproveStatus2 = e.Item.FindControl("gvimgProof32") as Image;
                            Image imgApproveStatus3 = e.Item.FindControl("gvimgProof33") as Image;
                            if (Convert.ToInt32(printStatusID) == 1)
                            {
                                lblProof33.Text = "Final Proof";
                                imgApproveStatus1.ImageUrl = "~/Images/ApprovalStatus/EngStatus1Incomplete.png";
                                imgApproveStatus2.ImageUrl = "~/Images/ApprovalStatus/EngStatus2Incomplete.png";
                                imgApproveStatus3.ImageUrl = "~/Images/ApprovalStatus/EngStatus3Incomplete.png";
                            }
                            else if (Convert.ToInt32(printStatusID) == 2)
                            {
                                lblProof33.Text = "Final Proof";
                                imgApproveStatus1.ImageUrl = "~/Images/ApprovalStatus/EngStatus1Complete.png";
                                imgApproveStatus2.ImageUrl = "~/Images/ApprovalStatus/EngStatus2Incomplete.png";
                                imgApproveStatus3.ImageUrl = "~/Images/ApprovalStatus/EngStatus3Incomplete.png";
                            }
                            else if (Convert.ToInt32(printStatusID) == 3)
                            {
                                lblProof33.Text = "Final Proof";
                                imgApproveStatus1.ImageUrl = "~/Images/ApprovalStatus/EngStatus1Complete.png";
                                imgApproveStatus2.ImageUrl = "~/Images/ApprovalStatus/EngStatus2Complete.png";
                                imgApproveStatus3.ImageUrl = "~/Images/ApprovalStatus/EngStatus3Incomplete.png";
                            }
                            else if (Convert.ToInt32(printStatusID) == 6)
                            {
                                lblProof33.Text = "Approved";
                                imgApproveStatus1.ImageUrl = "~/Images/ApprovalStatus/EngStatus1Complete.png";
                                imgApproveStatus2.ImageUrl = "~/Images/ApprovalStatus/EngStatus2Complete.png";
                                imgApproveStatus3.ImageUrl = "~/Images/ApprovalStatus/EngStatus3Complete.png";
                            }
                        }
                        else
                        {
                            pnlForFive.Visible = true;
                            pnlForThree.Visible = false;
                            Image imgApproveStatus1 = e.Item.FindControl("gvimgProof51") as Image;
                            Image imgApproveStatus2 = e.Item.FindControl("gvimgProof52") as Image;
                            Image imgApproveStatus3 = e.Item.FindControl("gvimgProof53") as Image;
                            Image imgApproveStatus4 = e.Item.FindControl("gvimgProof54") as Image;
                            Image imgApproveStatus5 = e.Item.FindControl("gvimgProof55") as Image;
                            if (Convert.ToInt32(printStatusID) == 1)
                            {
                                imgApproveStatus1.ImageUrl = "~/Images/ApprovalStatus/OthStatus1Incomplete.png";
                                imgApproveStatus2.ImageUrl = "~/Images/ApprovalStatus/OthStatus2Incomplete.png";
                                imgApproveStatus3.ImageUrl = "~/Images/ApprovalStatus/OthStatus3Incomplete.png";
                                imgApproveStatus4.ImageUrl = "~/Images/ApprovalStatus/OthStatus4Incomplete.png";
                                imgApproveStatus5.ImageUrl = "~/Images/ApprovalStatus/OthStatus5Incomplete.png";
                            }
                            else if (Convert.ToInt32(printStatusID) == 2)
                            {
                                imgApproveStatus1.ImageUrl = "~/Images/ApprovalStatus/OthStatus1Complete.png";
                                imgApproveStatus2.ImageUrl = "~/Images/ApprovalStatus/OthStatus2Incomplete.png";
                                imgApproveStatus3.ImageUrl = "~/Images/ApprovalStatus/OthStatus3Incomplete.png";
                                imgApproveStatus4.ImageUrl = "~/Images/ApprovalStatus/OthStatus4Incomplete.png";
                                imgApproveStatus5.ImageUrl = "~/Images/ApprovalStatus/OthStatus5Incomplete.png";
                            }
                            else if (Convert.ToInt32(printStatusID) == 3)
                            {
                                imgApproveStatus1.ImageUrl = "~/Images/ApprovalStatus/OthStatus1Complete.png";
                                imgApproveStatus2.ImageUrl = "~/Images/ApprovalStatus/OthStatus2Complete.png";
                                imgApproveStatus3.ImageUrl = "~/Images/ApprovalStatus/OthStatus3Incomplete.png";
                                imgApproveStatus4.ImageUrl = "~/Images/ApprovalStatus/OthStatus4Incomplete.png";
                                imgApproveStatus5.ImageUrl = "~/Images/ApprovalStatus/OthStatus5Incomplete.png";
                            }
                            else if (Convert.ToInt32(printStatusID) == 4)
                            {
                                imgApproveStatus1.ImageUrl = "~/Images/ApprovalStatus/OthStatus1Complete.png";
                                imgApproveStatus2.ImageUrl = "~/Images/ApprovalStatus/OthStatus2Complete.png";
                                imgApproveStatus3.ImageUrl = "~/Images/ApprovalStatus/OthStatus3Complete.png";
                                imgApproveStatus4.ImageUrl = "~/Images/ApprovalStatus/OthStatus4Incomplete.png";
                                imgApproveStatus5.ImageUrl = "~/Images/ApprovalStatus/OthStatus5Incomplete.png";
                            }
                            else if (Convert.ToInt32(printStatusID) == 5)
                            {
                                imgApproveStatus1.ImageUrl = "~/Images/ApprovalStatus/OthStatus1Complete.png";
                                imgApproveStatus2.ImageUrl = "~/Images/ApprovalStatus/OthStatus2Complete.png";
                                imgApproveStatus3.ImageUrl = "~/Images/ApprovalStatus/OthStatus3Complete.png";
                                imgApproveStatus4.ImageUrl = "~/Images/ApprovalStatus/OthStatus4Complete.png";
                                imgApproveStatus5.ImageUrl = "~/Images/ApprovalStatus/OthStatus5Incomplete.png";
                            }
                            else if (Convert.ToInt32(printStatusID) == 6)
                            {
                                Label lblProof55 = e.Item.FindControl("gvlblProof55") as Label;
                                lblProof55.Text = "Approved";
                                imgApproveStatus1.ImageUrl = "~/Images/ApprovalStatus/OthStatus1Complete.png";
                                imgApproveStatus2.ImageUrl = "~/Images/ApprovalStatus/OthStatus2Complete.png";
                                imgApproveStatus3.ImageUrl = "~/Images/ApprovalStatus/OthStatus3Complete.png";
                                imgApproveStatus4.ImageUrl = "~/Images/ApprovalStatus/OthStatus4Complete.png";
                                imgApproveStatus5.ImageUrl = "~/Images/ApprovalStatus/OthStatus5Complete.png";
                            }
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

        protected void radGridPrintStatus_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.ToUpper().Equals("ORDERNOW"))
                {
                    //get the menuID on which order now button is clicked
                    long menuId = 0;
                    string strmenuid = Convert.ToString(e.CommandArgument);
                    long.TryParse(strmenuid, out menuId);
                    string menucode = "UNDEFINED";
                    GridDataItem item = e.Item as GridDataItem;
                    if (item != null)
                    {
                        menucode = item["MenuCode"].Text;
                    }
                    menucode = "\"" + menucode + "\"";
                    Session["MOVEMENU"] = menuId;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "MoveMenuConf", "ConfirmMoveMenu(" + menucode + ")", true);

                    //var menu = _menuManagement.GetMenuById(menuId);

                    //var cycleIdofLiveOrder = _orderManagement.GetLiveOrderCycleId();

                    //if (cycleIdofLiveOrder != menu.CycleId && cycleIdofLiveOrder != 0)
                    //{
                    //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "CannotMove()", true);
                    //}
                    //else
                    //{
                    //    //move order to Live Order
                    //    _orderManagement.CreateLiveOrderNow(menuId);

                    //    //refresh the grid as the menu item won't be available now
                    //    GridDataSource();
                    //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "ShowMoved()", true);
                    //}
                }
                else if (e.CommandName == RadGrid.ExpandCollapseCommandName)
                {
                    foreach (GridItem item in e.Item.OwnerTableView.Items)
                    {
                        if (item.Expanded && item != e.Item)
                        {
                            item.Expanded = false;
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

        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "Rebind")
            {
                radGridPrintStatus.Rebind();
            }
        }

        protected void btnMoveMenuData_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(Session["MOVEMENU"])))
                {
                    long menuId = 0;
                    string strmenuid = Convert.ToString(Session["MOVEMENU"]);
                    long.TryParse(strmenuid, out menuId);
                    if (menuId > 0)
                    {
                        var menu = _menuManagement.GetMenuById(menuId);

                        var cycleIdofLiveOrder = _orderManagement.GetLiveOrderCycleId();

                        if (cycleIdofLiveOrder != menu.CycleId && cycleIdofLiveOrder != 0)
                        {
                            Session["MOVEMENU"] = string.Empty;
                            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "CannotMove()", true);
                        }
                        else
                        {
                            //move order to Live Order
                            _orderManagement.CreateLiveOrderNow(menuId);

                            //refresh the grid as the menu item won't be available now
                            GridDataSource();
                            Session["MOVEMENU"] = string.Empty;
                            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "ShowMoved()", true);
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

        protected void btnclearsession_Click(object sender, EventArgs e)
        {
            Session["MOVEMENU"] = string.Empty;
        }

        protected void btnApproveAll_Click(object sender, EventArgs e)
        {
            var cycle = ddlCycle.SelectedValue;
            var menuclass = ddlMenuClass.SelectedValue;
            var menutype = ddlMenuType.SelectedValue;
            var userId = Convert.ToInt32(Session["USERID"]);

            var data = _orderManagement.GetMenuStatusAndApprovers(Convert.ToInt32(cycle), Convert.ToInt16(menuclass), Convert.ToInt16(menutype), userId);

            var DataToBindInGrid = (from menudata in data
                                    where menudata.IsMovedToLiveOrder != true
                                    select new
                                    {
                                        MenuId = menudata.Id,
                                        MenuCode = menudata.MenuCode,
                                        MenuName = menudata.MenuName,
                                        PrintOrderStatus = menudata.ApprovalStatusId,
                                        LanguageID = menudata.LanguageId,
                                        LanguageName = menudata.LanguageName,
                                    }).ToList();

            foreach (var d in DataToBindInGrid)
            {
                MenuData newmenu = new MenuData()
                {
                    Id = d.MenuId,
                    ApprovalStatusId = 6
                };
                _menuManagement.UpdateStatus(newmenu, Convert.ToInt32(Session["USERID"]));
            }

            //refresh the grid as the menu item won't be available now
            GridDataSource();
        }

        protected void btnMoveAlltoOrder_Click(object sender, EventArgs e)
        {
            var cycle = ddlCycle.SelectedValue;
            var menuclass = ddlMenuClass.SelectedValue;
            var menutype = ddlMenuType.SelectedValue;
            var userId = Convert.ToInt32(Session["USERID"]);

            var data = _orderManagement.GetMenuStatusAndApprovers(Convert.ToInt32(cycle), Convert.ToInt16(menuclass), Convert.ToInt16(menutype), userId);

            var DataToBindInGrid = (from menudata in data
                                    where menudata.IsMovedToLiveOrder != true
                                    select new
                                    {
                                        MenuId = menudata.Id,
                                        MenuCode = menudata.MenuCode,
                                        MenuName = menudata.MenuName,
                                        PrintOrderStatus = menudata.ApprovalStatusId,
                                        LanguageID = menudata.LanguageId,
                                        LanguageName = menudata.LanguageName,
                                    }).ToList();


            bool cantMove = false;

            foreach (var d in DataToBindInGrid)
            {

                var menu = _menuManagement.GetMenuById(d.MenuId);

                var cycleIdofLiveOrder = _orderManagement.GetLiveOrderCycleId();

                if (cycleIdofLiveOrder != menu.CycleId && cycleIdofLiveOrder != 0)
                {
                    cantMove = true;
                    break;
                }
            }

            if (cantMove)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "Message", "CannotMove()", true);
                return;
            }


            foreach (var d in DataToBindInGrid)
            {
                //move order to Live Order
                _orderManagement.CreateLiveOrderNow(d.MenuId);
            }

            //refresh the grid as the menu item won't be available now
            GridDataSource();
        }
    }
}