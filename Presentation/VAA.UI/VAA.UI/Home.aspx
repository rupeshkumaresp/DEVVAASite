<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="VAA.UI.Home" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <telerik:RadAjaxPanel ID="RadAjaxPanelHomePage" runat="server">
        <style type="text/css">
            /*.rwLoading {
                background-image: none !important;
            }*/

            #RadWindowWrapper_ctl00_MainContent_UserListDialog {
                top: 10px!important;
                left: 35px!important;
            }          
        </style>    
        <div>
		
		        
			
            <h3 style="text-decoration: underline">Choose Menu Version</h3>
            <br />
            <table style="width: 600px;">
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="lblCycle" runat="server" Text="Cycle :"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlCycle" runat="server" Class="dropdown_small"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="lblMenuClass" runat="server" Text="Menu Class :"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMenuClass" runat="server" Class="dropdown_small" AutoPostBack="True" OnSelectedIndexChanged="ddlMenuClass_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="lblMenuType" runat="server" Text="Menu Type :"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMenuType" runat="server" Class="dropdown_small" AutoPostBack="True"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td style="padding-top: 10px; padding-bottom: 5px; padding-right: 110px;">
                        <asp:Button ID="btnViewMenu" runat="server" Text="View Menu" Class="button" OnClick="BtnViewMenuClick" />
                    </td>
                </tr>
            </table>
        </div>
        <script type="text/javascript">
            function OnClientBeforeClose(sender, args) {
                args.set_cancel(true);
                function confirmCallback(arg) {
                    if (arg) {
                        sender.remove_beforeClose(OnClientBeforeClose);
                        sender.close();
                        sender.add_beforeClose(OnClientBeforeClose);
                    }
                }
                
                radconfirm("You must save any changes before closing this window. Any unsaved changes will be lost. Are you sure you want to close this window?", confirmCallback, 600, 200, null, "Close Confirmation?")
            }

            function RowMouseOver(sender, eventArgs) {
                $get(eventArgs.get_id()).className = "RowMouseOver";
            }
            function RowMouseOut(sender, eventArgs) {
                $get(eventArgs.get_id()).className = "RowMouseOut";
            }
            //function FilterMenuOnMouseOver(sender, eventArgs) {
            //    $get(eventArgs.get_id()).className = "FilterMenuOnMouseOver";
            //} 
        </script>
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">

                var $ = $telerik.$;
                function ShowEditForm(id, rowIndex) {
                    var grid = $find("<%= radGridMenu.ClientID %>");
                    var rowControl = grid.get_masterTableView().get_dataItems()[rowIndex].get_element();
                    grid.get_masterTableView().selectItem(rowControl, true);
                    var rwin = $find("<%= UserListDialog.ClientID %>");
                    rwin.set_title("");
                    window.radopen("MenuViewer.aspx?ID=" + id, "UserListDialog");
                    return false;
                }

                function ShowMenuPDFForm(id, rowIndex) {
                    var grid = $find("<%= radGridMenu.ClientID %>");
                    var rowControl = grid.get_masterTableView().get_dataItems()[rowIndex].get_element();
                    grid.get_masterTableView().selectItem(rowControl, true);
                    var rwin = $find("<%= UserListDialog.ClientID %>");
                    rwin.set_title("");
                    window.radopen("MenuPDFViewer.aspx?ID=" + id, "PDFRadDialog");
                    return false;
                }

                function refreshGrid(arg) {
                    if (!arg) { $find("<%= radAjaxManager.ClientID %>").ajaxRequest("Rebind"); }
                    else { $find("<%= radAjaxManager.ClientID %>").ajaxRequest("RebindAndNavigate"); }
                }


                $(window).resize(function () {
                    var grid = $find("<%= UserListDialog.ClientID %>");
                    if (grid.isVisible()) {
                        setWindowsize();
                    }

                    var grid2 = $find("<%= PDFRadDialog.ClientID %>");
                    if (grid2.isVisible()) {
                        setWindowsizePDFRadDialog();
                    }

                });

                function setWindowsize() {
                    var grid = $find("<%= UserListDialog.ClientID %>");
                    var viewportWidth = $(window).width();
                    var viewportHeight = $(window).height();
                    grid.setSize(Math.ceil(viewportWidth * 95 / 100), Math.ceil(viewportHeight * 95 / 100));
                    grid.center();
                }

                function setWindowsizePDFRadDialog() {
                    var grid = $find("<%= PDFRadDialog.ClientID %>");
                     var viewportWidth = $(window).width();
                     var viewportHeight = $(window).height();
                     grid.setSize(Math.ceil(viewportWidth * 95 / 100), Math.ceil(viewportHeight * 95 / 100));
                     grid.center();
                 }
                //Limit the Filter control options
                var column = null;
                function MenuShowing(sender, args) {
                    if (column === null)
                        return;
                    var menu = sender; var items = menu.get_items();
                    if (column.get_dataType() === "System.String") {
                        var i = 0;
                        while (i < items.get_count()) {
                            if (!(items.getItem(i).get_value() in { 'NoFilter': '', 'Contains': '', 'DoesNotContain': '', 'StartsWith': '', 'EndsWith': '', 'EqualTo': '' })) {
                                var item = items.getItem(i);
                                if (item !== null)
                                    item.set_visible(false);
                            }
                            else {
                                var item = items.getItem(i);
                                if (item !== null)
                                    item.set_visible(true);
                            } i++;
                        }
                    }
                    if (column.get_dataType() === "System.Int64") {
                        var j = 0; while (j < items.get_count()) {
                            if (!(items.getItem(j).get_value() in { 'NoFilter': '', 'Contains': '', 'DoesNotContain': '', 'StartsWith': '', 'EndsWith': '', 'EqualTo': '' })) {
                                var item = items.getItem(j); if (item !== null)
                                    item.set_visible(false);
                            }
                            else { var item = items.getItem(j); if (item !== null) item.set_visible(true); } j++;
                        }
                    }
                    column = null;
                    menu.repaint();
                }
                function filterMenuShowing(sender, eventArgs) {
                    column = eventArgs.get_column();
                }

            </script>
        </telerik:RadCodeBlock>
        <div>
            <telerik:RadAjaxManager ID="radAjaxManager" runat="server">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="radAjaxManager">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="radGridMenu" LoadingPanelID="gridLoadingPanel"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="radGridMenu">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="radGridMenu" LoadingPanelID="gridLoadingPanel"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
            <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
                <Windows>
                    <telerik:RadWindow ID="UserListDialog" OnClientBeforeClose="OnClientBeforeClose" runat="server" Modal="true" Behaviors="Close" OnClientShow="setWindowsize" ReloadOnShow="true"
                        VisibleOnPageLoad="false" VisibleStatusbar="false" Title="" ShowContentDuringLoad="false">
                    </telerik:RadWindow>

                    <telerik:RadWindow ID="PDFRadDialog" runat="server" Modal="true" Behaviors="Close" OnClientShow="setWindowsizePDFRadDialog" ReloadOnShow="true"
                        VisibleOnPageLoad="false" VisibleStatusbar="false" Title="" ShowContentDuringLoad="false">
                    </telerik:RadWindow>                
                </Windows>
            </telerik:RadWindowManager>
          
            <div id="MenuGridDiv" runat="server" visible="False">
                <div>
                    <h3 style="text-decoration: underline; text-align: left;">List of Menus:</h3>
                    <br />
                </div>
                <telerik:RadGrid ID="radGridMenu" runat="server" AutoGenerateColumns="False" Skin="Silk" ShowHeader="true" PageSize="10" PagerStyle-AlwaysVisible="true" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center" PagerStyle-BorderStyle="None" AllowSorting="true" AllowPaging="true" AllowMultiRowSelection="false" AllowMultiRowEdit="false" GridLines="Both" AllowFilteringByColumn="true" EnableViewState="true" ViewStateMode="Enabled" HeaderStyle-BackColor="#aa2029" HeaderStyle-ForeColor="#FFFFFF" GroupingSettings-IgnorePagingForGroupAggregates="False" MasterTableView-ExpandCollapseColumn-ItemStyle-Width="200px" ItemStyle-BorderColor="#999999" FilterMenu-BackColor="#FF6600" OnPageSizeChanged="radGridMenu_PageSizeChange" OnNeedDataSource="radGridMenu_NeedDataSource" OnItemDataBound="radGridMenu_ItemDataBound" OnItemCommand="radGridMenu_ItemCommand" OnDetailTableDataBind="radGridMenu_DetailTableDataBind">
                    <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
                    <GroupingSettings CaseSensitive="false" />
                    <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" AllowKeyboardNavigation="true" EnableRowHoverStyle="true">
                        <Selecting AllowRowSelect="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                        <Resizing EnableRealTimeResize="false" AllowColumnResize="false" />
                        <ClientEvents OnRowMouseOver="RowMouseOver" OnRowMouseOut="RowMouseOut" OnFilterMenuShowing="filterMenuShowing" />
                    </ClientSettings>
                    <FilterMenu OnClientShowing="MenuShowing" />
                    <SelectedItemStyle CssClass="SelectedItem"></SelectedItemStyle>
                    <MasterTableView TableLayout="Auto" AutoGenerateColumns="false" DataKeyNames="ID" Name="MenuDetails" CommandItemDisplay="Top" CssClass="rdheaderCss" FilterItemStyle-HorizontalAlign="Center" ExpandCollapseColumn-HeaderText="Routes" ExpandCollapseColumn-HeaderStyle-Width="200px" CellPadding="1" CellSpacing="1">
                    
                        <SortExpressions>
                            <telerik:GridSortExpression FieldName="Id" SortOrder="Descending" />
                        </SortExpressions>
                        <CommandItemTemplate>
                            <asp:LinkButton ID="lnkbtnClearAllFilter" runat="server" CommandName="ClearFilter" Text="Clear all filter" TabIndex="13" CssClass="FilterGridCommand"></asp:LinkButton>
                            <asp:LinkButton ID="lnkbtnRefreshGrid" runat="server" CommandName="Refresh" Text="Refresh" TabIndex="14" CssClass="FilterGridCommand marginLeft"></asp:LinkButton>
                        </CommandItemTemplate>
                        <Columns>
                            <telerik:GridBoundColumn HeaderText="Menu Id" DataType="System.Int64" UniqueName="Id" DataField="Id" Display="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Menu Code" DataType="System.String" UniqueName="MenuCode" DataField="MenuCode" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FilterControlWidth="70%" HeaderStyle-Width="30%"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Menu Name" DataType="System.String" UniqueName="MenuName" DataField="MenuName" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FilterControlWidth="70%" HeaderStyle-Width="30%">
                                <ItemStyle Wrap="true" Width="350px" />
                            </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn HeaderText="Approval Status" DataType="System.String" UniqueName="ApprovalStatus" DataField="ApprovalStatus" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FilterControlWidth="70%" HeaderStyle-Width="30%">
                              <FilterTemplate>
                                    <telerik:RadComboBox ID="StatusCombo" SelectedValue='<%#((GridItem)Container).OwnerTableView.GetColumn("ApprovalStatus").CurrentFilterValue %>'
                                        runat="server" OnClientSelectedIndexChanged="StatusComboIndexChanged">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="All" Value="" />
                                            <telerik:RadComboBoxItem Text="Virgin first proof" Value="Virgin first proof" />
                                            <telerik:RadComboBoxItem Text="Caterer proof" Value="Caterer proof" />
                                            <telerik:RadComboBoxItem Text="Virgin second proof" Value="Virgin second proof" />
                                            <telerik:RadComboBoxItem Text="Translator proof" Value="Translator proof" />
                                            <telerik:RadComboBoxItem Text="Final proof" Value="Final proof" />
                                            <telerik:RadComboBoxItem Text="Approved" Value="Approved" />

                                        </Items>
                                    </telerik:RadComboBox>
                                    <telerik:RadScriptBlock ID="RadScriptBlockStatus" runat="server">
                                        <script type="text/javascript">
                                            function StatusComboIndexChanged(sender, args) {
                                                var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                tableView.filter("ApprovalStatus", args.get_item().get_value(), "EqualTo");
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                                </telerik:GridBoundColumn>

                            <telerik:GridBoundColumn HeaderText="LanguageId" DataType="System.String" UniqueName="LanguageId" DataField="LanguageId" Visible="false"></telerik:GridBoundColumn>
                            <telerik:GridBoundColumn HeaderText="Language" DataType="System.String" UniqueName="LanguageName" DataField="LanguageName" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FilterControlWidth="70%" HeaderStyle-Width="20%">
                                <FilterTemplate>
                                    <telerik:RadComboBox ID="LanguageCombo" SelectedValue='<%#((GridItem)Container).OwnerTableView.GetColumn("LanguageName").CurrentFilterValue %>'
                                        runat="server" OnClientSelectedIndexChanged="LanguageNameComboIndexChanged">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="All" Value="" />
                                            <telerik:RadComboBoxItem Text="English" Value="English" />
                                            <telerik:RadComboBoxItem Text="Hindi" Value="Hindi" />
                                            <telerik:RadComboBoxItem Text="Simplified Chinese" Value="Simplified Chinese" />
                                            <telerik:RadComboBoxItem Text="Traditional Chinese" Value="Traditional Chinese" />
                                        </Items>
                                    </telerik:RadComboBox>
                                    <telerik:RadScriptBlock ID="RadScriptBlock2" runat="server">
                                        <script type="text/javascript">
                                            function LanguageNameComboIndexChanged(sender, args) {
                                                var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                tableView.filter("LanguageName", args.get_item().get_value(), "EqualTo");
                                            }
                                        </script>
                                    </telerik:RadScriptBlock>
                                </FilterTemplate>
                            </telerik:GridBoundColumn>
                            <telerik:GridTemplateColumn UniqueName="TemplateEditColumn" HeaderText="View" AllowFiltering="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="14%">

                                <ItemTemplate>
                                    <%-- <asp:LinkButton ID="ViewMenuLink" runat="server" CommandName="ViewMenu">LinkButton</asp:LinkButton>--%>
                                    <telerik:RadButton ID="ViewMenuLink" runat="server" CommandName="ViewMenu" Text="View Menu" CssClass="hyperLink"></telerik:RadButton>
                                    <telerik:RadButton ID="ViewPDFLink" runat="server" CommandName="ViewPDFMenu" Text="View PDF" CssClass="hyperLink" Visible="false"></telerik:RadButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <DetailTables>
                            <telerik:GridTableView DataKeyNames="RouteId" Name="RouteDetails" Width="80%" AllowSorting="False" AllowPaging="False" AllowFilteringByColumn="False" HeaderStyle-BackColor="#808080">
                                <Columns>
                                    <telerik:GridBoundColumn HeaderText="Route ID" DataType="System.Int64" UniqueName="RouteId" DataField="RouteId" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Region" DataType="System.String" UniqueName="RegionName" DataField="RegionName" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Departure Aiport Name" DataType="System.String" UniqueName="DepartureAirportName" DataField="DepartureAirportName" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Departure Airport Code" DataType="System.String" UniqueName="DepartureAirportCode" DataField="DepartureAirportCode" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Arrival Aiport Name" DataType="System.String" UniqueName="ArrivalAirportName" DataField="ArrivalAirportName" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Arrival Airport Code" DataType="System.String" UniqueName="ArrivalAirportCode" DataField="ArrivalAirportCode" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Flights" DataType="System.String" UniqueName="FlightNo" DataField="FlightNo" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                </Columns>
                            </telerik:GridTableView>
                        </DetailTables>
                        <NoRecordsTemplate>
                            <fieldset style="width: 100%!important;" class="noRecordsFieldset">
                                <legend>Menu items</legend>No records available.
                            </fieldset>
                        </NoRecordsTemplate>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="false" EnablePostBackOnRowClick="false">
                        <Selecting AllowRowSelect="false" EnableDragToSelectRows="false" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
        </div>
    </telerik:RadAjaxPanel>

</asp:Content>
