<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="VAA.UI.Orders" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        /*.rgRow td, .rgAltRow td, .rgHeader td,*/
        .rgFilterRow td {
            padding: 0px !important;
            vertical-align: middle;
        }

        .widthgvDDL {
            width: 80% !important;
        }
    </style>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            var $ = $telerik.$;
            function ShowReorderForm(id, rowIndex) {
                var grid = $find("<%= radGridPreviousOrder.ClientID %>");
                var rowControl = grid.get_masterTableView().get_dataItems()[rowIndex].get_element();
                grid.get_masterTableView().selectItem(rowControl, true);
                var rwin = $find("<%= UserListDialog.ClientID %>");
                rwin.set_title("");
                window.radopen("ReOrder.aspx?ID=" + id, "UserListDialog");
                return false;
            }

            function ShowCurrReorderForm(id, rowIndex) {
                var grid = $find("<%= radGridCurrentOrder.ClientID %>");
                var rowControl = grid.get_masterTableView().get_dataItems()[rowIndex].get_element();
                grid.get_masterTableView().selectItem(rowControl, true);
                var rwin = $find("<%= UserListDialog.ClientID %>");
                rwin.set_title("");
                window.radopen("ReOrder.aspx?ID=" + id, "UserListDialog");
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
            });

            function setWindowsize() {
                var grid = $find("<%= UserListDialog.ClientID %>");
                    var viewportWidth = $(window).width();
                    var viewportHeight = $(window).height();
                    grid.setSize(Math.ceil(viewportWidth * 60 / 100), Math.ceil(viewportHeight * 50 / 100));
                    grid.center();
                }
                function RefreshParentPage() {
                    document.location.reload();
                }

        </script>
    </telerik:RadCodeBlock>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        <Windows>
            <telerik:RadWindow ID="UserListDialog" runat="server" Modal="true" Behaviors="Close" ReloadOnShow="true"
                VisibleOnPageLoad="false" VisibleStatusbar="false" Title="" ShowContentDuringLoad="false" MinHeight="400px" Width="730px" OnClientClose="RefreshParentPage">
            </telerik:RadWindow>
        </Windows>
    </telerik:RadWindowManager>
    <asp:UpdatePanel ID="updpnlOrder" runat="server">
        <ContentTemplate>
            <div class="row" style="text-align: left; padding-left: 20px; padding-right: 20px;">
                <h3 style="text-decoration: underline">Current Orders</h3>
                <br />
            </div>
            <div class="row" style="text-align: left; padding-left: 20px; padding-right: 20px;">
                <telerik:RadGrid ID="radGridCurrentOrder" runat="server" AutoGenerateColumns="false" Skin="Silk" ShowHeader="true" PageSize="10"
                    PagerStyle-AlwaysVisible="true" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center" PagerStyle-BorderStyle="None" MasterTableView-EditMode="InPlace" AlternatingItemStyle-BackColor="#eeeded" AllowSorting="true" AllowPaging="true" AllowMultiRowSelection="false" AllowMultiRowEdit="false" GridLines="Both" ExpandCollapseColumn-ItemStyle-Width="200px" AllowFilteringByColumn="true" EnableViewState="true" ViewStateMode="Enabled" HeaderStyle-BackColor="#aa2029" HeaderStyle-ForeColor="#FFFFFF" ShowGroupPanel="true" OnNeedDataSource="radGridCurrentOrder_NeedDataSource" OnItemCommand="radGridCurrentOrder_ItemCommand" OnItemDataBound="radGridCurrentOrder_ItemDataBound" OnDetailTableDataBind="radGridCurrentOrder_DetailTableDataBind">
                    <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
                    <GroupingSettings CaseSensitive="false" />
                    <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" AllowKeyboardNavigation="true">
                        <Selecting AllowRowSelect="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                        <Resizing EnableRealTimeResize="false" AllowColumnResize="false" />
                    </ClientSettings>
                    <MasterTableView TableLayout="Fixed" AutoGenerateColumns="false" DataKeyNames="OrderId" Name="OrderDetails" CommandItemDisplay="Top" CssClass="rdheaderCss" FilterItemStyle-HorizontalAlign="Center" ExpandCollapseColumn-HeaderText="Details" ExpandCollapseColumn-HeaderStyle-Width="200px">
                        <CommandItemSettings />
                        <SortExpressions>
                            <telerik:GridSortExpression FieldName="OrderId" SortOrder="Descending" />
                        </SortExpressions>
                        <CommandItemTemplate>
                            <asp:LinkButton ID="lnkbtnClearAllFilter" runat="server" CommandName="ClearFilter" Text="Clear all filter" TabIndex="13" CssClass="FilterGridCommand" CausesValidation="false"></asp:LinkButton>
                            <asp:LinkButton ID="lnkbtnRefreshGrid" runat="server" CommandName="Refresh" Text="Refresh" TabIndex="14" CssClass="FilterGridCommand marginLeft" CausesValidation="false"></asp:LinkButton>
                        </CommandItemTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Id" DataType="System.Int64" UniqueName="OrderRowId" DataField="OrderRowId" AllowFiltering="false" AllowSorting="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FilterControlWidth="80%" Display="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderRowIdV" runat="server" Text='<%#Eval("OrderRowId") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblOrderRowIdE" runat="server" Text='<%#Eval("OrderRowId") %>'></asp:Label>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="Order Id" DataType="System.Int32" UniqueName="OrderId" DataField="OrderId" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" AllowFiltering="true" AllowSorting="true" FilterControlWidth="80%" SortExpression="OrderId"></telerik:GridBoundColumn>
                            <%--<telerik:GridTemplateColumn HeaderText="Order Id" DataType="System.Int32" UniqueName="OrderId" DataField="OrderId" AllowFiltering="true" AllowSorting="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FilterControlWidth="80%" SortExpression="OrderId">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderIdV" runat="server" Text='<%#Eval("OrderId") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblOrderIdE" runat="server" Text='<%#Eval("OrderId") %>'></asp:Label>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>--%>

                            <telerik:GridTemplateColumn HeaderText="Order Date" DataType="System.DateTime" UniqueName="OrderDate" DataField="OrderDate" AllowFiltering="true" AllowSorting="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FilterControlWidth="80%" SortExpression="OrderDate">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderDateV" runat="server" Text='<%#Eval("OrderDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblOrderDateE" runat="server" Text='<%#Eval("OrderDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Friendly Name" DataType="System.String" UniqueName="FriendlyName" DataField="FriendlyName" AllowFiltering="true" AllowSorting="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FilterControlWidth="80%" SortExpression="FriendlyName">
                                <ItemTemplate>
                                    <asp:Label ID="lblFriendlyNameV" runat="server" Text='<%#Eval("FriendlyName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblFriendlyNameE" runat="server" Text='<%#Eval("FriendlyName") %>'></asp:Label>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="LOT Number" DataType="System.String" UniqueName="LOTNumber" DataField="LOTNumber" AllowFiltering="true" AllowSorting="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FilterControlWidth="80%" SortExpression="LOTNumber">
                                <ItemTemplate>
                                    <asp:Label ID="lblLOTNumberV" runat="server" Text='<%#Eval("LOTNumber") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblLOTNumberE" runat="server" Text='<%#Eval("LOTNumber") %>'></asp:Label>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Cycle" DataType="System.String" UniqueName="Cycle" DataField="Cycle" AllowFiltering="true" AllowSorting="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FilterControlWidth="80%" SortExpression="Cycle">
                                <ItemTemplate>
                                    <asp:Label ID="lblCycleV" runat="server" Text='<%#Eval("Cycle") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblCycleE" runat="server" Text='<%#Eval("Cycle") %>'></asp:Label>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Order Status" DataType="System.String" UniqueName="OrderStatus" DataField="OrderStatus" AllowFiltering="true" AllowSorting="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FilterControlWidth="80%" SortExpression="OrderStatus">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderStatusV" runat="server" Text='<%#Eval("OrderStatus") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblOrderStatusEID" runat="server" Text='<%#Eval("OrderStatusId") %>' Visible="false"></asp:Label>
                                    <div>
                                        <asp:DropDownList ID="ddlGVOrderStatus" runat="server" Style="width: 100%"></asp:DropDownList>
                                    </div>
                                    <div>
                                        <asp:RequiredFieldValidator ID="rfvGVOrderStatus" runat="server" ControlToValidate="ddlGVOrderStatus" SetFocusOnError="true" Display="Dynamic"
                                            CssClass="validationErrorMsg" InitialValue="0" ValidationGroup="ORSTATUS" ErrorMessage="Please select order status."></asp:RequiredFieldValidator>
                                    </div>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Edit" AllowFiltering="false" UniqueName="EditControl" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" AllowSorting="false">
                                <ItemTemplate>
                                    <telerik:RadButton ID="radbtnEdit" runat="server" Text="Edit" CommandName="EDIT" CommandArgument='<%#Eval("OrderRowId") %>' CausesValidation="false"></telerik:RadButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadButton ID="radbtnUpdate" runat="server" Text="Update" CommandName="UPDATE" CommandArgument='<%#Eval("OrderRowId") %>' ValidationGroup="ORSTATUS"></telerik:RadButton>
                                    <telerik:RadButton ID="radbtnCancel" runat="server" Text="Cancel" CommandName="CANCEL" CommandArgument='<%#Eval("OrderRowId") %>' CausesValidation="false"></telerik:RadButton>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" AllowSorting="false">
                                <ItemTemplate>
                                    <telerik:RadButton ID="btnCurrReorder" runat="server" CommandName="REORDER" Text="Reorder" CssClass="hyperLink"></telerik:RadButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <DetailTables>
                            <telerik:GridTableView DataKeyNames="LiveOrderId" Name="LiveOrderDetails" Width="80%" AllowSorting="False" AllowPaging="False" AllowFilteringByColumn="False" HeaderStyle-BackColor="#808080">
                                <Columns>
                                    <telerik:GridBoundColumn HeaderText="LiveOrderId" DataType="System.Int64" UniqueName="LiveOrderId" DataField="LiveOrderId" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Visible="false"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Menu Code" DataType="System.String" UniqueName="MenuCode" DataField="MenuCode" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Language" DataType="System.String" UniqueName="LanguageName" DataField="LanguageName" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Route" DataType="System.String" UniqueName="Route" DataField="Route" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="ClassName" DataType="System.String" UniqueName="ClassName" DataField="ClassName" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="MenuType" DataType="System.String" UniqueName="MenuType" DataField="MenuType" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="FlightNo" DataType="System.String" UniqueName="FlightNo" DataField="FlightNo" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Quantity" DataType="System.String" UniqueName="Quantity" DataField="Quantity" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                </Columns>
                            </telerik:GridTableView>
                        </DetailTables>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="false" EnablePostBackOnRowClick="false">
                        <Selecting AllowRowSelect="false" EnableDragToSelectRows="false" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
            <br />
            <div class="row" style="text-align: left; padding-left: 20px; padding-right: 20px;">
                <br />
                <hr />
                <h3 style="text-decoration: underline">Previous Orders</h3>
                <br />
            </div>
            <div class="row" style="text-align: left; padding-left: 20px; padding-right: 20px;">
                <telerik:RadGrid ID="radGridPreviousOrder" runat="server" AutoGenerateColumns="false" Skin="Silk" ShowHeader="true" PageSize="10"
                    PagerStyle-AlwaysVisible="true" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center" PagerStyle-BorderStyle="None" MasterTableView-EditMode="InPlace"
                    AlternatingItemStyle-BackColor="#eeeded" AllowSorting="true" AllowPaging="true" AllowMultiRowSelection="false" AllowMultiRowEdit="false" GridLines="Both"
                    AllowFilteringByColumn="true" EnableViewState="true" ViewStateMode="Enabled" HeaderStyle-BackColor="#aa2029" HeaderStyle-ForeColor="#FFFFFF" ShowGroupPanel="true" ExpandCollapseColumn-ItemStyle-Width="200px" OnNeedDataSource="radGridPreviousOrder_NeedDataSource" OnItemCommand="radGridPreviousOrder_ItemCommand" OnDetailTableDataBind="radGridPreviousOrder_DetailTableDataBind" OnItemDataBound="radGridPreviousOrder_ItemDataBound">
                    <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
                    <GroupingSettings CaseSensitive="false" />
                    <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" AllowKeyboardNavigation="true">
                        <Selecting AllowRowSelect="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                        <Resizing EnableRealTimeResize="false" AllowColumnResize="false" />
                    </ClientSettings>
                    <MasterTableView TableLayout="Fixed" AutoGenerateColumns="false" DataKeyNames="OrderId" Name="PreviousOrder" CommandItemDisplay="Top" CssClass="rdheaderCss" FilterItemStyle-HorizontalAlign="Center" ExpandCollapseColumn-HeaderText="Details" ExpandCollapseColumn-HeaderStyle-Width="200px">
                        <CommandItemSettings />
                        <SortExpressions>
                            <telerik:GridSortExpression FieldName="OrderId" SortOrder="Descending" />
                        </SortExpressions>
                        <CommandItemTemplate>
                            <asp:LinkButton ID="lnkbtnClearAllFilter" runat="server" CommandName="ClearFilter" Text="Clear all filter" TabIndex="13" CssClass="FilterGridCommand" CausesValidation="false"></asp:LinkButton>
                            <asp:LinkButton ID="lnkbtnRefreshGrid" runat="server" CommandName="Refresh" Text="Refresh" TabIndex="14" CssClass="FilterGridCommand marginLeft" CausesValidation="false"></asp:LinkButton>
                        </CommandItemTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Id" DataType="System.Int64" UniqueName="OrderRowId" DataField="OrderRowId" AllowFiltering="false" AllowSorting="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FilterControlWidth="80%" Display="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblPOrderRowIdV" runat="server" Text='<%#Eval("OrderRowId") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridBoundColumn HeaderText="Order Id" DataType="System.Int32" UniqueName="OrderId" DataField="OrderId" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" AllowFiltering="true" AllowSorting="true" FilterControlWidth="80%" SortExpression="OrderId"></telerik:GridBoundColumn>
                            <%--<telerik:GridTemplateColumn HeaderText="Order Id" DataType="System.Int32" UniqueName="OrderId" DataField="OrderId" AllowFiltering="true" AllowSorting="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FilterControlWidth="80%" SortExpression="OrderId">
                                <ItemTemplate>
                                    <asp:Label ID="lblPOrderIdV" runat="server" Text='<%#Eval("OrderId") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>--%>

                            <telerik:GridTemplateColumn HeaderText="Order Date" DataType="System.DateTime" UniqueName="OrderDate" DataField="OrderDate" AllowFiltering="true" AllowSorting="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FilterControlWidth="80%" SortExpression="OrderDate">
                                <ItemTemplate>
                                    <asp:Label ID="lblPOrderDateV" runat="server" Text='<%#Eval("OrderDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Friendly Name" DataType="System.String" UniqueName="FriendlyName" DataField="FriendlyName" AllowFiltering="true" AllowSorting="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FilterControlWidth="80%" SortExpression="FriendlyName">
                                <ItemTemplate>
                                    <asp:Label ID="lblFriendlyNameV" runat="server" Text='<%#Eval("FriendlyName") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblFriendlyNameE" runat="server" Text='<%#Eval("FriendlyName") %>'></asp:Label>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="LOT Number" DataType="System.String" UniqueName="LOTNumber" DataField="LOTNumber" AllowFiltering="true" AllowSorting="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FilterControlWidth="80%" SortExpression="LOTNumber">
                                <ItemTemplate>
                                    <asp:Label ID="lblPLOTNumberV" runat="server" Text='<%#Eval("LOTNumber") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Cycle" DataType="System.String" UniqueName="Cycle" DataField="Cycle" AllowFiltering="true" AllowSorting="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FilterControlWidth="80%" SortExpression="Cycle">
                                <ItemTemplate>
                                    <asp:Label ID="lblPCycleV" runat="server" Text='<%#Eval("Cycle") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Order Status" DataType="System.String" UniqueName="OrderStatus" DataField="OrderStatus" AllowFiltering="true" AllowSorting="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FilterControlWidth="80%" SortExpression="OrderStatus">
                                <ItemTemplate>
                                    <asp:Label ID="lblPOrderStatusV" runat="server" Text='<%#Eval("OrderStatus") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Order Completed Date" DataType="System.DateTime" UniqueName="CompletedDate" DataField="CompletedDate" AllowFiltering="true" AllowSorting="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FilterControlWidth="80%" SortExpression="CompletedDate">
                                <ItemTemplate>
                                    <asp:Label ID="lblPOrderDatecompleted" runat="server" Text='<%#Eval("CompletedDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" AllowSorting="false">
                                <ItemTemplate>
                                    <%-- <asp:LinkButton ID="lnkbtnGVReorder" runat="server" Text="Reorder" CommandName="REORDER" CommandArgument="" CausesValidation="false" ForeColor="#AA2029" Font-Underline="true" Font-Bold="true"></asp:LinkButton>--%>
                                    <telerik:RadButton ID="btnReorder" runat="server" CommandName="REORDER" Text="Reorder" CssClass="hyperLink"></telerik:RadButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <DetailTables>
                            <telerik:GridTableView DataKeyNames="LiveOrderId" Name="PreviousOrderDetails" Width="80%" AllowSorting="False" AllowPaging="False" AllowFilteringByColumn="False" HeaderStyle-BackColor="#808080">
                                <Columns>
                                    <telerik:GridBoundColumn HeaderText="LiveOrderId" DataType="System.Int64" UniqueName="LiveOrderId" DataField="LiveOrderId" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Visible="false"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Menu Code" DataType="System.String" UniqueName="MenuCode" DataField="MenuCode" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Language" DataType="System.String" UniqueName="LanguageName" DataField="LanguageName" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Route" DataType="System.String" UniqueName="Route" DataField="Route" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="ClassName" DataType="System.String" UniqueName="ClassName" DataField="ClassName" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="MenuType" DataType="System.String" UniqueName="MenuType" DataField="MenuType" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="FlightNo" DataType="System.String" UniqueName="FlightNo" DataField="FlightNo" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn HeaderText="Quantity" DataType="System.String" UniqueName="Quantity" DataField="Quantity" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                </Columns>
                            </telerik:GridTableView>
                        </DetailTables>
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="false" EnablePostBackOnRowClick="false">
                        <Selecting AllowRowSelect="false" EnableDragToSelectRows="false" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
            <%--<telerik:RadAjaxLoadingPanel ID="gridLoadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>--%>
            <telerik:RadAjaxManager ID="radAjaxManager" runat="server">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="radAjaxManager">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="radGridCurrentOrder" LoadingPanelID="gridLoadingPanel"></telerik:AjaxUpdatedControl>
                            <telerik:AjaxUpdatedControl ControlID="radGridPreviousOrder" LoadingPanelID="gridLoadingPanel"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="radGridCurrentOrder">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="radGridCurrentOrder" LoadingPanelID="gridLoadingPanel"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                    <telerik:AjaxSetting AjaxControlID="radGridPreviousOrder">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="radGridPreviousOrder" LoadingPanelID="gridLoadingPanel"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
            <telerik:RadCodeBlock ID="radCodeBlock" runat="server">
                <script type="text/javascript">
                    function OnRequestStart(sender, args) {
                        args.set_enableAjax(false);
                    }

                    function RefreshPreviousOrder() {
                        var preGrid = $find("<%= radGridPreviousOrder.ClientID %>");
                        preGrid.rebind();
                    }
                </script>
            </telerik:RadCodeBlock>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--<asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="updpnlOrder" runat="server" DisplayAfter="0">
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
        </asp:UpdateProgress>--%>
</asp:Content>
