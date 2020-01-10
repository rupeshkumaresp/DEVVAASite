﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PrintRun.aspx.cs" Inherits="VAA.UI.PrintRun" MaintainScrollPositionOnPostback="true" %>

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
     <script type="text/javascript">
        
         function GeneratePrintNotification() {
             alert("PDF generation is in progress, you will be notified by email once the PDF generation is complete!");
         }


        </script>

    <div class="row" style="text-align: left; padding-left: 20px; padding-right: 20px;">
        <h3 style="text-decoration: underline">Weekly Print Run Summary</h3>
        <br />
    </div>
    <div class="row" style="text-align: left; padding-left: 20px; padding-right: 20px;">
        <telerik:RadGrid ID="radGridCurrentOrder" runat="server" AutoGenerateColumns="false" Skin="Silk" ShowHeader="true" PageSize="10"
            PagerStyle-AlwaysVisible="true" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center" PagerStyle-BorderStyle="None" MasterTableView-EditMode="InPlace" AlternatingItemStyle-BackColor="#eeeded" AllowSorting="true" AllowPaging="true" AllowMultiRowSelection="false" AllowMultiRowEdit="false" GridLines="Both" MasterTableView-ExpandCollapseColumn-ItemStyle-Width="200px" MasterTableView-ExpandCollapseColumn-HeaderStyle-Width="200px" AllowFilteringByColumn="true" EnableViewState="true" ViewStateMode="Enabled" HeaderStyle-BackColor="#aa2029" HeaderStyle-ForeColor="#FFFFFF" ShowGroupPanel="true" OnNeedDataSource="radGridCurrentOrder_NeedDataSource" OnItemCommand="radGridCurrentOrder_ItemCommand" OnItemDataBound="radGridCurrentOrder_ItemDataBound" OnDetailTableDataBind="radGridCurrentOrder_DetailTableDataBind">
            <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
            <GroupingSettings CaseSensitive="false" />
            <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" AllowKeyboardNavigation="true">
                <Selecting AllowRowSelect="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="false" AllowColumnResize="false" />
            </ClientSettings>
            <MasterTableView TableLayout="Fixed" AutoGenerateColumns="false" DataKeyNames="OrderId" Name="OrderDetails" CommandItemDisplay="Top" CssClass="rdheaderCss" FilterItemStyle-HorizontalAlign="Center" ExpandCollapseColumn-HeaderText="Details" ExpandCollapseColumn-HeaderStyle-Width="200px">
                <CommandItemSettings />
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

                    <telerik:GridTemplateColumn HeaderText="Order Id" DataType="System.Int32" UniqueName="OrderId" DataField="OrderId" AllowFiltering="true" AllowSorting="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FilterControlWidth="80%" SortExpression="OrderId">
                        <ItemTemplate>
                            <asp:Label ID="lblOrderIdV" runat="server" Text='<%#Eval("OrderId") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lblOrderIdE" runat="server" Text='<%#Eval("OrderId") %>'></asp:Label>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <%--                            <telerik:GridTemplateColumn HeaderText="Order Date" DataType="System.DateTime" UniqueName="OrderDate" DataField="OrderDate" AllowFiltering="true" AllowSorting="true" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" FilterControlWidth="80%" SortExpression="OrderDate">
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderDateV" runat="server" Text='<%#Eval("OrderDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblOrderDateE" runat="server" Text='<%#Eval("OrderDate","{0:dd/MM/yyyy}") %>'></asp:Label>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>--%>

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

                    <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" AllowSorting="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkbtnGVDownloadPrintRun" runat="server" Text="Download Print Run (Excel)" CommandName="DownloadPrintRun" CommandArgument="" CausesValidation="false" ForeColor="#AA2029" Font-Underline="true" Font-Bold="true"></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                <%--    <telerik:GridTemplateColumn AllowFiltering="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" AllowSorting="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkbtnGVDownloadPrintReadyPDF" runat="server" Text="Generate Print (PDF)" OnClientClick="GeneratePrintNotification();" CommandName="DownloadPrintPDF" CommandArgument="" CausesValidation="false" ForeColor="#AA2029" Font-Underline="true" Font-Bold="true"></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>--%>


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

        <br />
        <b>* Please note that Generate Print (PDF) will create PDF(s) for this order at path: H:\Emma\MENU PDFS\Order_Id</b>
    </div>


    <telerik:RadAjaxManager ID="radAjaxManager" runat="server" ClientEvents-OnRequestStart="OnRequestStart">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="radAjaxManager">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radGridCurrentOrder" LoadingPanelID="gridLoadingPanel"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="radGridCurrentOrder">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="radGridCurrentOrder" LoadingPanelID="gridLoadingPanel"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>

        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadCodeBlock ID="radCodeBlock" runat="server">
        <script type="text/javascript">
            function OnRequestStart(sender, args) {
                if (args.get_eventTarget().indexOf("lnkbtnGVDownloadPrintRun") >= 0)
                    args.set_enableAjax(false);
            }
           

        </script>
    </telerik:RadCodeBlock>

</asp:Content>
