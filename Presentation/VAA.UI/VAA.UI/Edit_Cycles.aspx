<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit_Cycles.aspx.cs" Inherits="VAA.UI.Edit_Cycles" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Cycle</title>
    <link href="../Content/Site.css" rel="stylesheet" />
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="Scripts/jquery-2.1.4.js"></script>
    <script src="Scripts/bootstrap.js"></script>
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

        function CancelEdit() {
            GetRadWindow().close();
        }

        function OnDateSelectingWednesday(sender, args) {
            var date = new Date(args.get_renderDay().DomElement.title);
            if (date.getDay() != 3) {
                args.set_cancel(true);
                bootbox.alert('Cycle must start on "Wednesday" Only!');
            }
            else {
            }
        }

        function OnDateSelectingTuesday(sender, args) {
            var date = new Date(args.get_renderDay().DomElement.title);
            if (date.getDay() != 2) {
                args.set_cancel(true);
                bootbox.alert('Cycle must end on "Tuesday" Only!');
            }
            else {

            }
        }
    </script>
    <style>
        .riTextBox {
            width: 372px !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="rdmgr" runat="server"></telerik:RadScriptManager>
        <asp:UpdatePanel ID="updpnlEditCycle" runat="server">
            <ContentTemplate>
                <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" Skin="Vista" DecoratedControls="All" />
                <br />
                <div align="center">
                    <h3 style="text-decoration: underline">Update Cycle</h3>
                </div>
                <br />
                <div align="center">
                    <table style="text-align: left; border-collapse: separate; border-spacing: 8px;">
                        <tr>
                            <td style="text-align: left;">
                                <asp:Label ID="lblYear" runat="server" Text="Year:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <div>
                                    <asp:DropDownList ID="ddlYearUpdate" runat="server" Class="dropdown"></asp:DropDownList>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvYearUpdate" runat="server" ControlToValidate="ddlYearUpdate" InitialValue="0" Display="Dynamic" SetFocusOnError="true" ErrorMessage=">> Please select year." ValidationGroup="ManageCycleUpdate" Style="color: #be3e16; font-weight: bold; font-size: 1.1em;"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Label ID="lblCycleName" runat="server" Text="Cycle Name :"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <div>
                                    <asp:TextBox ID="txtCycleNameUpdate" runat="server" CssClass="textBox"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvCycleNameUpdate" runat="server" ControlToValidate="txtCycleNameUpdate" Display="Dynamic" SetFocusOnError="true" ErrorMessage=">> Please enter cycle name." ValidationGroup="ManageCycleUpdate" Style="color: #be3e16; font-weight: bold; font-size: 1.1em;"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Label ID="lblShortName" runat="server" Text="Short Name :"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <div>
                                    <asp:TextBox ID="txtShortNameUpdate" runat="server" CssClass="textBox"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvShortNameUpdate" runat="server" ControlToValidate="txtShortNameUpdate" Display="Dynamic" SetFocusOnError="true" ErrorMessage=">> Please enter short name." ValidationGroup="ManageCycleUpdate" Style="color: #be3e16; font-weight: bold; font-size: 1.1em;"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Label ID="lblStartDate" runat="server" Text="Start Date :"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <div>
                                    <div style="width: 400px; height: 30px; border: 1px solid #000;">
                                        <telerik:RadDatePicker ID="radDatePickerStartDateUpdate" runat="server" Font-Italic="true" DateInput-ReadOnly="true" Skin="Simple" DateInput-DateFormat="dd/MM/yyyy" DateInput-DisplayDateFormat="dd/MM/yyyy" Calendar-ClientEvents-OnDateSelecting="OnDateSelectingWednesday" DatePopupButton-ImageUrl="~/Images/Calender.png" DatePopupButton-Height="24" DatePopupButton-Width="20" DatePopupButton-ToolTip="Select start date" DatePopupButton-HoverImageUrl="~/Images/Calender.png" Style="padding: 0px!important;" Calendar-ShowRowHeaders="false"></telerik:RadDatePicker>
                                    </div>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvStartDateUpdate" runat="server" ControlToValidate="radDatePickerStartDateUpdate" Display="Dynamic" SetFocusOnError="true" ErrorMessage=">> Please select start date." ValidationGroup="ManageCycleUpdate" Style="color: #be3e16; font-weight: bold; font-size: 1.1em;"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;">
                                <asp:Label ID="lblEndDate" runat="server" Text="End Date :"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <div>
                                    <div style="width: 400px; height: 30px; border: 1px solid #000;">
                                        <telerik:RadDatePicker ID="radDatePickerEndDateUpdate" runat="server" Font-Italic="true" DateInput-ReadOnly="true" Skin="Simple" DateInput-DateFormat="dd/MM/yyyy" DateInput-DisplayDateFormat="dd/MM/yyyy" Calendar-ClientEvents-OnDateSelecting="OnDateSelectingTuesday" DatePopupButton-ImageUrl="~/Images/Calender.png" DatePopupButton-Height="24" DatePopupButton-Width="20" DatePopupButton-ToolTip="Select end date" DatePopupButton-HoverImageUrl="~/Images/Calender.png" Style="padding: 0px!important;" Calendar-ShowRowHeaders="false"></telerik:RadDatePicker>
                                    </div>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvEndDateUpdate" runat="server" ControlToValidate="radDatePickerEndDateUpdate" Display="Dynamic" SetFocusOnError="true" ErrorMessage=">> Please select end date." ValidationGroup="ManageCycleUpdate" Style="color: #be3e16; font-weight: bold; font-size: 1.1em;"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                         <tr>
                            <td style="text-align: right;">
                                <asp:Label ID="lblCurrentCycle" runat="server" Text="Make this cycle as current cycle? :"></asp:Label>
                            </td>
                            <td style="text-align: left; padding-left: 25px;">
                                <telerik:RadButton ID="btnCurrentCycle" runat="server" ToggleType="Radio" ButtonType="ToggleButton" GroupName="StandardButton" Skin="Outlook" AutoPostBack="False" OnClientCheckedChanged="ShowConfirmWindow">
                                    <ToggleStates>
                                        <telerik:RadButtonToggleState Text="Yes" PrimaryIconCssClass="rbToggleRadioChecked" />
                                        <telerik:RadButtonToggleState Text="Yes" PrimaryIconCssClass="rbToggleRadio" />
                                    </ToggleStates>
                                </telerik:RadButton>

                                <telerik:RadButton ID="btnNotCurrentCycle" runat="server" ToggleType="Radio" Checked="true" GroupName="StandardButton" ButtonType="ToggleButton" Skin="Outlook" AutoPostBack="False" OnClientCheckedChanged="NotShowConfirmWindow">
                                    <ToggleStates>
                                        <telerik:RadButtonToggleState Text="No" PrimaryIconCssClass="rbToggleRadioChecked"></telerik:RadButtonToggleState>
                                        <telerik:RadButtonToggleState Text="No" PrimaryIconCssClass="rbToggleRadio"></telerik:RadButtonToggleState>
                                    </ToggleStates>
                                </telerik:RadButton>
                                <asp:Label hidden="true" runat="server" ID="lblCurrCycleName" />
                                <asp:Button ID="btnchangecurrcycle" runat="server" Visible="false" OnClick="btnChangeCurrCycle_Click" />
                                <asp:HiddenField ID="hfchangeCurrCycle" runat="server" />
                                <asp:Label hidden="true" runat="server" ID="lblChangeCurrCycle" />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left;"></td>
                            <td style="text-align: left; padding-bottom: 30px;">
                                <asp:Button ID="btnUpdateCycle" runat="server" Text="Update Cycle" Class="button" ValidationGroup="ManageCycleUpdate" OnClick="btnUpdateCycle_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="updpnlEditCycle" runat="server" DisplayAfter="0">
            <ProgressTemplate>
                <div align="center" class="updateprogressdiv1">
                </div>
                <div id="divmiddle" align="center" class="updateprogressdiv2">
                    <div align="center" id="divdeepInner" class="deepInnerdiv">
                        <div align="center" style="border: none; background-color: #FFFFFF; border: 1px solid #000000; -webkit-border-radius: 10px; -moz-border-radius: 10px; padding-bottom: 10px;">
                            <img src="../Images/gear.gif" alt="Loading" height="100" width="100" style="border: none;" />
                            <br />
                            <span style="font-family: sans-serif; font-size: 14px; font-weight: 700; color: #BC1527;">Please wait...</span>
                        </div>
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </form>
    <script>
        function ConfirmCurrentCycle(currcyclename, newcyclename) {
            var currCycleName = String(currcyclename);
            var newCycleName = String(newcyclename);
            bootbox.dialog({
                title: "CHANGE CURRENT CYCLE",
                message: '<div class="row">' +
                    '<div class="col-md-12"><span class="help-block" style="font-size:16px !important;">The current active cycle is: <b>' + currCycleName + '</b></span></div>' +
                    '<div class="col-md-12"><span class="help-block" style="font-size:16px !important;">Are you sure want to change your active cycle to the new cycle: <b>' + newCycleName + '</b>?</span></div> ' +
                    ' </div>',
                buttons: {
                    success: {
                        label: "Change",
                        className: "btn-success",
                        callback: function () {
                            document.getElementById("<%=hfchangeCurrCycle.ClientID%>").value = 1;
                        }
                    },
                    danger: {
                        label: "Cancel",
                        className: "btn-danger",
                        callback: function () {
                            document.getElementById("<%=hfchangeCurrCycle.ClientID%>").value = 0;
                            var btn1 = $find("<%=btnCurrentCycle.ClientID%>");
                            var btn2 = $find("<%=btnNotCurrentCycle.ClientID%>");
                            btn1.set_checked(false);
                            btn2.set_checked(true);
                        }
                    }
                }
            });
        }
        function ShowConfirmWindow(sender, args) {
            if (args.get_checked()) {
                var currcyclename = document.getElementById("<%=lblCurrCycleName.ClientID%>").innerHTML;
                 var newcyclename = document.getElementById("<%=txtCycleNameUpdate.ClientID%>").value;
                 ConfirmCurrentCycle(currcyclename, newcyclename);
             }
             else {
                 document.getElementById("<%=hfchangeCurrCycle.ClientID%>").value = 0;
             }
         }
         function NotShowConfirmWindow(sender, args) {
             if (args.get_checked()) {
                 document.getElementById("<%=hfchangeCurrCycle.ClientID%>").value = 0;
            }
            else {
                var currcyclename = document.getElementById('lblCurrCycleName').innerText;
                var newcyclename = document.getElementById('txtCycleName').innerText;
                ConfirmCurrentCycle(currcyclename, newcyclename);
            }
        }
        function MsgHasLiveOrders() {
            bootbox.alert("Cannot change make the new cycle as active! <br/> Reason : The current cycle has pending Live orders to be processed.<br/> Please process the live orders before making a new active cycle.");
        }       
        function MsgChangeActiveOrderFalse() {
            bootbox.alert("Cannot change make the new cycle as active! <br/> Please contact technical team for assistance.");
        }
    </script>
</body>
</html>
