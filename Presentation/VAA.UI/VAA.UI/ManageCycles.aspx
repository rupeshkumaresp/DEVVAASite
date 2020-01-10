<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageCycles.aspx.cs" Inherits="VAA.UI.ManageCycles" MaintainScrollPositionOnPostback="true" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .riTextBox {
            width: 372px !important;
        }
    </style>
    <script src="Scripts/bootbox.js"></script>


    <asp:UpdatePanel ID="updpnlManageCycle" runat="server">
        <ContentTemplate>
            <div class="container-fluid">
                <div class="row">
                    <h3 style="text-decoration: underline">Add a New Cycle</h3>
                </div>
                <br />

                <div>
                    <table style="width: 665px;">
                        <tr>
                            <td style="text-align: right;">
                                <asp:Label ID="lblYear" runat="server" Text="Year:"></asp:Label>
                            </td>
                            <td>
                                <div>
                                    <asp:DropDownList ID="ddlYear" runat="server" Class="dropdown"></asp:DropDownList>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvYear" runat="server" ControlToValidate="ddlYear" InitialValue="0" Display="Dynamic" SetFocusOnError="true" ErrorMessage=">> Please select year." ValidationGroup="ManageCycle" CssClass="validationErrorMsg"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                                <asp:Label ID="lblCycleName" runat="server" Text="Cycle Name :"></asp:Label>
                            </td>
                            <td>
                                <div>
                                    <asp:TextBox ID="txtCycleName" runat="server" CssClass="textBox"></asp:TextBox><br />
                                    <asp:Label ID="lblNameformat" runat="server" Text="(CycleName YYYY. Eg:March 2015)" Style="font-size: 12px; font-style: italic; float: left; padding-left: 10px;"></asp:Label>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvCycleName" runat="server" ControlToValidate="txtCycleName" Display="Dynamic" SetFocusOnError="true" ErrorMessage=">> Please enter cycle name." ValidationGroup="ManageCycle" CssClass="validationErrorMsg"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                                <asp:Label ID="lblShortName" runat="server" Text="Short Name :"></asp:Label>
                            </td>
                            <td>
                                <div>
                                    <asp:TextBox ID="txtShortName" runat="server" CssClass="textBox"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvShortName" runat="server" ControlToValidate="txtShortName" Display="Dynamic" SetFocusOnError="true" ErrorMessage=">> Please enter short name." ValidationGroup="ManageCycle" CssClass="validationErrorMsg"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                                <asp:Label ID="lblStartDate" runat="server" Text="Start Date :"></asp:Label>
                            </td>
                            <td style="text-align: left; padding-left: 20px;">
                                <div>
                                    <div style="width: 400px; height: 32px; border: 1px solid #000;">
                                        <telerik:RadDatePicker ID="radDatePickerStartDate" runat="server" Font-Italic="true" DateInput-ReadOnly="true" Skin="Simple" DateInput-DateFormat="dd/MM/yyyy" Calendar-ClientEvents-OnDateSelecting="OnDateSelectingWednesday" DateInput-DisplayDateFormat="dd/MM/yyyy" DatePopupButton-ImageUrl="~/Images/Calender.png" DatePopupButton-Height="24" DatePopupButton-ToolTip="Select start date" DatePopupButton-HoverImageUrl="~/Images/Calender.png" DatePopupButton-Width="20" Style="padding: 0px!important;" Calendar-ShowRowHeaders="false">
                                        </telerik:RadDatePicker>
                                    </div>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="radDatePickerStartDate" Display="Dynamic" SetFocusOnError="true" ErrorMessage=">> Please select start date." ValidationGroup="ManageCycle" CssClass="validationErrorMsg"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                                <asp:Label ID="lblEndDate" runat="server" Text="End Date :"></asp:Label>
                            </td>
                            <td style="text-align: left; padding-left: 20px;">
                                <div>
                                    <div style="width: 400px; height: 32px; border: 1px solid #000;">
                                        <telerik:RadDatePicker ID="radDatePickerEndDate" runat="server" Font-Italic="true" DateInput-ReadOnly="true" Skin="Simple" DateInput-DateFormat="dd/MM/yyyy" Calendar-ClientEvents-OnDateSelecting="OnDateSelectingTuesday" DateInput-DisplayDateFormat="dd/MM/yyyy" DatePopupButton-ImageUrl="~/Images/Calender.png" DatePopupButton-Height="24" DatePopupButton-Width="20" DatePopupButton-ToolTip="Select end date" DatePopupButton-HoverImageUrl="~/Images/Calender.png" Style="padding: 0px!important;" Calendar-ShowRowHeaders="false"></telerik:RadDatePicker>
                                    </div>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvEndDate" runat="server" ControlToValidate="radDatePickerEndDate" Display="Dynamic" SetFocusOnError="true" ErrorMessage=">> Please select end date." ValidationGroup="ManageCycle" CssClass="validationErrorMsg"></asp:RequiredFieldValidator>
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
                            <td style="text-align: left; padding: 10px 0 0px 70px;">
                                <asp:Button ID="btnAddCycle" runat="server" Text="Add Cycle" Class="button" ValidationGroup="ManageCycle" OnClick="btnAddCycle_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <hr />

                <div class="row">
                    <h3 style="text-decoration: underline; text-align: left;">Existing Cycles:</h3>
                    <br />
                </div>

                <div class="row">
                    <telerik:RadGrid ID="radGridCycles" runat="server" AutoGenerateColumns="false" Skin="Silk" ShowHeader="true" PageSize="25"
                        PagerStyle-AlwaysVisible="true" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center" PagerStyle-BorderStyle="None" AlternatingItemStyle-BackColor="#eeeded"
                        AllowSorting="true" AllowPaging="true" AllowMultiRowSelection="false" AllowMultiRowEdit="false" GridLines="Both" AllowFilteringByColumn="true"
                        EnableViewState="true" ViewStateMode="Enabled" HeaderStyle-BackColor="#aa2029" HeaderStyle-ForeColor="#FFFFFF"
                        OnNeedDataSource="radGridCycles_NeedDataSource" OnItemCommand="radGridCycles_ItemCommand" OnItemDataBound="radGridCycles_ItemDataBound">
                        <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
                        <GroupingSettings CaseSensitive="false" />
                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" AllowKeyboardNavigation="true">
                            <Selecting AllowRowSelect="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                            <Resizing EnableRealTimeResize="false" AllowColumnResize="false" />
                        </ClientSettings>
                        <MasterTableView TableLayout="Fixed" AutoGenerateColumns="false" CommandItemDisplay="Top" CssClass="rdheaderCss" FilterItemStyle-HorizontalAlign="Center">
                            <CommandItemSettings />
                            <SortExpressions>
                                <telerik:GridSortExpression FieldName="Id" SortOrder="Descending" />
                            </SortExpressions>
                            <CommandItemTemplate>
                                <asp:LinkButton ID="lnkbtnClearAllFilter" runat="server" CommandName="ClearFilter" Text="Clear all filter" TabIndex="13" CssClass="FilterGridCommand"></asp:LinkButton>
                                <asp:LinkButton ID="lnkbtnRefreshGrid" runat="server" CommandName="Refresh" Text="Refresh" TabIndex="14" CssClass="FilterGridCommand marginLeft"></asp:LinkButton>
                            </CommandItemTemplate>
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="Cycle Id" DataType="System.Int64" UniqueName="Id" DataField="Id" Display="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn HeaderText="Year" DataType="System.String" UniqueName="Year" DataField="Year" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FilterControlWidth="70%"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn HeaderText="Cycle Name" DataType="System.String" UniqueName="CycleName" DataField="CycleName" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FilterControlWidth="70%"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn HeaderText="Short Name" DataType="System.String" UniqueName="ShortName" DataField="ShortName" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FilterControlWidth="70%"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn HeaderText="Start Date" DataType="System.DateTime" UniqueName="StartDate" DataField="StartDate" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FilterControlWidth="70%"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn HeaderText="End Date" DataType="System.DateTime" UniqueName="EndDate" DataField="EndDate" DataFormatString="{0:dd/MM/yyyy}" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FilterControlWidth="70%"></telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Edit" AllowFiltering="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="75" ItemStyle-Width="75">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="EditLink" runat="server" ImageUrl="~/Images/Edit.png" ImageHeight="20" ImageWidth="24" BorderStyle="None"></asp:HyperLink>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Delete" AllowFiltering="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="75" ItemStyle-Width="75">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="telimgbtnDelete" runat="server" CommandName="RowDelete" CausesValidation="false" ImageUrl="~/Images/Delete.png" BorderStyle="None" Height="20" Width="20" ToolTip="Delete" CssClass="Grdbtn" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <telerik:RadAjaxManager ID="radAjaxManager" runat="server" OnAjaxRequest="radAjaxManager_AjaxRequest">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="radAjaxManager">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="radGridCycles" LoadingPanelID="gridLoadingPanel"></telerik:AjaxUpdatedControl>
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="radGridCycles">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="radGridCycles" LoadingPanelID="gridLoadingPanel"></telerik:AjaxUpdatedControl>
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                </div>
            </div>
            <telerik:RadWindowManager ID="radWindowManager" runat="server" EnableShadow="true">
                <Windows>
                    <telerik:RadWindow ID="UserListDialog" runat="server" AutoSize="false" Behaviors="Close" VisibleStatusbar="false" Title="Editing record" Height="680px" Width="650px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
                    </telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>
            <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                <script type="text/javascript">
                    function ShowEditForm(id, rowIndex) {
                        var grid = $find("<%= radGridCycles.ClientID %>");
                        var rowControl = grid.get_masterTableView().get_dataItems()[rowIndex].get_element();
                        grid.get_masterTableView().selectItem(rowControl, true);
                        window.radopen("Edit_Cycles.aspx?ID=" + id, "UserListDialog");
                        return false;
                    }

                    function refreshGrid(arg) {
                        if (!arg) { $find("<%= radAjaxManager.ClientID %>").ajaxRequest("Rebind"); }
                        else { $find("<%= radAjaxManager.ClientID %>").ajaxRequest("RebindAndNavigate"); }
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
            </telerik:RadCodeBlock>
            <asp:Button ID="btndelcycle" runat="server" OnClick="btndelcycle_Click" Style="display: none" />
            <asp:Button ID="btnClearSession" runat="server" OnClick="btnClearSession_Click" Style="display: none" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="updpnlManageCycle" runat="server" DisplayAfter="0">
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
    <script type="text/javascript">

        function ConfirmBootToDeleteCycle(cname) {
            var cyclename = String(cname);
            bootbox.dialog({
                title: "DELETE CYCLE",
                message: '<div class="row">' +
                    '<div class="col-md-12"><span class="help-block" style="font-size:16px !important;">Delete cycle will delete <b>' + cyclename + '</b> cycle and all the related records.</span></div>' +
                    '<div class="col-md-12"><span class="help-block" style="font-size:16px !important;">Are you sure want to delete this cycle?</span></div> ' +
                    ' </div>',
                buttons: {
                    success: {
                        label: "OK",
                        className: "btn-success",
                        callback: function () {
                            var myControl = document.getElementById('<%=btndelcycle.ClientID%>');
                            myControl.click();
                        }
                    },
                    danger: {
                        label: "Cancel",
                        className: "btn-danger",
                        callback: function () {
                            var myControl = document.getElementById('<%=btnClearSession.ClientID%>');
                            myControl.click();
                        }
                    }
                }
            });
        }
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
                var newcyclename = document.getElementById("<%=txtCycleName.ClientID%>").value;
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
         function MsgCannotDelete() {
             bootbox.alert("Cannot delete this cycle! <br/> Reason : The current cycle has pending Live orders to be processed.");
         }
         function MsgChangeActiveOrderFalse() {
             bootbox.alert("Cannot change make the new cycle as active! <br/> Please contact technical team for assistance.");
         }
    </script>
</asp:Content>
