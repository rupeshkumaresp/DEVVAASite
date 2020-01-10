<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReOrder.aspx.cs" Inherits="VAA.UI.ReOrder" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <head id="Head1">
        <title>Re Order</title>
        <link href="Content/Site.css" rel="stylesheet" />
        <script src="Scripts/bootbox.js"></script>
        <script type="text/javascript">
            function CloseAndRebind(args) {
                GetRadWindow().BrowserWindow.refreshGrid(args);
                GetRadWindow().close();
            }

            function GetRadWindow() {
                var oWindow = null;
                if (window.radWindow) oWindow = window.radWindow;
                else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                return oWindow;
            }

            function CancelRoroder() {
                GetRadWindow().close();
            }

            function IsPageValid() {
                if (Page_ClientValidate("ManageUser")) {
                    alert(Page_ClientValidate("ManageUser"));
                    return false;
                }
                else { return false; }
            }
            function MsgMovedToOrder() {
                alert("The Order has been moved to Current Orders. Please check the Current Orders List.");
            }
            function MsgMovedToLiveOrder() {
                alert("The Order has been moved to Live Orders. Please check the LiveOrders page.");
            }
            function MsgHasLiveOrders() {
                alert("Cannot be moved! There is an existing Live Order, please process this live order and then re-order.");
            }
            function MsgMoveFailed() {
                alert("The Order cannot be moved. Please contact technical team.");
            }
            function MsgSelectOption() {
                alert("Please select an option to reorder!");
            }

            function MsgMenuUpdate() {
                alert("ReOrder with menu update is in progress, you will be notified by email shortly once the menu creation is complete!");
            }


            function MsgOrderReCreted(moved, recreated) {
                alert("Number of menus moved to LiveOrder = " + moved +
                    "\n Number of menus recreated for ReOrder = " + recreated);
            }
        </script>
        <style type="text/css">
            td {
                padding: 2px;
            }
        </style>
    </head>
    <div>

        <h3 style="text-decoration: underline">Re-Order Page:</h3>

        <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" Skin="Vista" DecoratedControls="All" />
        <br />
        <div align="center">
            <h3 style="text-decoration: underline">
                <asp:Label ID="lblUserActionType" runat="server"></asp:Label>
            </h3>
        </div>
        <br />

        <div align="center" id="divProcessdone" runat="server" visible="false">
            <asp:Label ID="lblProcesscomplete" Text="Re-Order process is complete!" runat="server"></asp:Label>
        </div>
        <div align="center" id="divReorderOptions" runat="server">
            <table style="width: 650px !important; text-align: left; border-collapse: separate; border-spacing: 8px;">
                <tr>
                    <td align="center">
                        <table style="width: 650px !important;">
                            <tr>
                                <td style="width: 250px !important;">Selected OrderID :
                                </td>
                                <td>
                                    <asp:Label ID="lblCurrOrderId" runat="server" Text="" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 250px !important;">Order Friendly Name:
                                </td>
                                <td>
                                    <asp:TextBox ID="txOrderName" runat="server" Width="300px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvtxOrderName" runat="server" Display="Dynamic" ControlToValidate="txOrderName"
                                        SetFocusOnError="true" ErrorMessage="* Required"
                                        CssClass="validationErrorMsg"></asp:RequiredFieldValidator>
                                </td>

                            </tr>
                            <tr>
                                <td style="padding-bottom: 10px; vertical-align: top;">Push the Order to :</td>
                                <td>
                                    <telerik:RadButton ID="btnStraightReprint" runat="server" ToggleType="Radio" ButtonType="ToggleButton" GroupName="StandardButton" Skin="Outlook" AutoPostBack="False" OnClientCheckedChanged="ChangeHFopt1" Style="">
                                        <ToggleStates>
                                            <telerik:RadButtonToggleState Text="Straight Reprint" PrimaryIconCssClass="rbToggleRadioChecked" />
                                            <telerik:RadButtonToggleState Text="Straight Reprint" PrimaryIconCssClass="rbToggleRadio" />
                                        </ToggleStates>
                                    </telerik:RadButton>
                                    <br />
                                    <telerik:RadButton ID="btnScheduleUpdate" runat="server" ToggleType="Radio" ButtonType="ToggleButton" GroupName="StandardButton" Skin="Outlook" AutoPostBack="False" OnClientCheckedChanged="ChangeHFopt2">
                                        <ToggleStates>
                                            <telerik:RadButtonToggleState Text="Re-order with Schedule update" PrimaryIconCssClass="rbToggleRadioChecked" />
                                            <telerik:RadButtonToggleState Text="Re-order with Schedule update" PrimaryIconCssClass="rbToggleRadio" />
                                        </ToggleStates>
                                    </telerik:RadButton>
                                    <br />
                                    <telerik:RadButton ID="btnMenuUpdate" runat="server" ToggleType="Radio" ButtonType="ToggleButton" GroupName="StandardButton" Skin="Outlook" AutoPostBack="False" OnClientCheckedChanged="ChangeHFopt3">
                                        <ToggleStates>
                                            <telerik:RadButtonToggleState Text="Re-order with Menu update" PrimaryIconCssClass="rbToggleRadioChecked" />
                                            <telerik:RadButtonToggleState Text="Re-order with Menu update" PrimaryIconCssClass="rbToggleRadio" />
                                        </ToggleStates>
                                    </telerik:RadButton>
                                    <asp:HiddenField ID="hfchangeoption" runat="server" />
                                    <asp:Button ID="btngetCurrCycle" runat="server" OnClick="btngetCurrCycle_Click" CssClass="button" Style="display: none; background-image: none !important;" Text="" />
                                    <asp:Button ID="btnMoveMenuPnl" runat="server" OnClick="btnMoveMenuPnl_Click" CssClass="button" Style="display: none; background-image: none !important;" Text="" />
                                    <asp:Button ID="btnHidePanels" runat="server" OnClick="btnHidePanels_Click" CssClass="button" Style="display: none; background-image: none !important;" Text="" />

                                </td>
                            </tr>
                            <asp:Panel ID="StraightReprintCol" runat="server" Visible="false" Style="visibility: hidden;">
                                <tr>
                                    <td style="padding-bottom: 10px; width: 250px !important;">Current Cycle :</td>
                                    <td>
                                        <asp:Label ID="lblCurrCycleName" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="padding-bottom: 10px; width: 250px !important;">Choose the week and dates :</td>
                                    <td>
                                        <asp:DropDownList ID="ddlWeekAndDates" runat="server" Class="dropdown_small"></asp:DropDownList>
                                    </td>
                                </tr>
                            </asp:Panel>
                            <asp:Panel ID="NoActiveCycle" runat="server">
                                <tr>
                                    <td style="padding-bottom: 10px;"></td>
                                    <td>
                                        <asp:Label ID="lblNoActiveCycle" runat="server" Text="" Style="font-style: italic; color: #aa2029;"></asp:Label>
                                    </td>
                                </tr>
                            </asp:Panel>

                            <tr>
                                <td colspan="2" style="text-align: center; padding: 10px;">
                                    <asp:Button ID="btnReOrder" runat="server" Text="Move Order" CssClass="button" ValidationGroup="AddBaseItem" CausesValidation="true" OnClick="btnReOrder_Click" />
                                    <asp:Button ID="btlCancel" runat="server" Text="Cancel" CssClass="button" ValidationGroup="AddBaseItem" CausesValidation="false" OnClick="btlCancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>

    </div>
    <script type="text/javascript">
        function ChangeHFopt1(sender, args) {
            if (args.get_checked()) {
                document.getElementById("<%=hfchangeoption.ClientID%>").value = 1;
                document.getElementById("<%=btngetCurrCycle.ClientID%>").click();
            }
            else {
                document.getElementById("<%=hfchangeoption.ClientID%>").value = 0;
            }
        }
        function ChangeHFopt2(sender, args) {
            if (args.get_checked()) {
                document.getElementById("<%=hfchangeoption.ClientID%>").value = 2;
                document.getElementById("<%=btnHidePanels.ClientID%>").click();
            }
            else {
                document.getElementById("<%=hfchangeoption.ClientID%>").value = 0;
            }
        }
        function ChangeHFopt3(sender, args) {
            if (args.get_checked()) {
                document.getElementById("<%=hfchangeoption.ClientID%>").value = 3;
                document.getElementById("<%=btnMoveMenuPnl.ClientID%>").click();
            }
            else {
                document.getElementById("<%=hfchangeoption.ClientID%>").value = 0;
            }
        }

    </script>
</asp:Content>

