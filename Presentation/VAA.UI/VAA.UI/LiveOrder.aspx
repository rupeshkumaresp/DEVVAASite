<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LiveOrder.aspx.cs" Inherits="VAA.UI.LiveOrder" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .ruFileWrap {
            width: 235px !important;
        }

        .ruButton {
            background: none !important; /*#e3e3e3 linear-gradient(#fefefe, #e3e3e3) repeat-x scroll 0 0;*/
            border: 1px solid #AA2029 !important;
            border-radius: 3px !important;
            background-color: #AA2029 !important;
            color: #FFFFFF !important;
        }
    </style>
    <script type="text/javascript">
        function ShowOrderCreated() {
            bootbox.alert("Order is created successfully.");
        }

        function GetConfirm() {
            var myControl = document.getElementById('<%=btnOrderNow.ClientID%>');
            myControl.click();
            return;

            bootbox.dialog({
                title: "ORDER CONFIRMATION",
                message: '<div class="row">' +
                    '<div class="col-md-12"><span class="help-block" style="font-size:16px !important;">By clicking Order Now, you accept these menu(s) for printing and agree with the terms and conditions.</span></div>' +
                    '<div class="col-md-12"><span class="help-block" style="font-size:16px !important;">Please confirm you want to Order Now?</span></div> ' +
                    ' </div>',
                buttons: {
                    success: {
                        label: "OK",
                        className: "btn-success",
                        callback: function () {
                            var myControl = document.getElementById('<%=btnOrderNow.ClientID%>');
                            myControl.click();
                        }
                    },
                    danger: {
                        label: "Cancel",
                        className: "btn-danger",
                        callback: function () {
                        }
                    }
                }
            });
        }

        function ConfirmRemoveFromLiveOrder(menucode) {
            var mcode = String(menucode);
            var myControl = document.getElementById('<%=btnRemoveFormLiveOrder.ClientID%>');
            myControl.click();
            return;

            bootbox.dialog({
                title: "REMOVE FROM LIVE ORDER",
                message: '<div class="row">' +
                    '<div class="col-md-12"><span class="help-block" style="font-size:16px !important;">This operation will remove menu  "' + mcode + '" from Live Order and place back to Print Status page.</span></div>' +
                    '<div class="col-md-12"><span class="help-block" style="font-size:16px !important;">Please confirm you want to remove this menu from Live Order?</span></div> ' +
                    ' </div>',
                buttons: {
                    success: {
                        label: "OK",
                        className: "btn-success",
                        callback: function () {
                            var myControl = document.getElementById('<%=btnRemoveFormLiveOrder.ClientID%>');
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
    </script>
    <asp:UpdatePanel ID="updpnlOrder" runat="server">
        <ContentTemplate>
            <h3 style="text-decoration: underline">Orders Ready</h3>
            <br />

            <asp:Label ID="lblNoLiveOrder" runat="server" Text="No live order found!" Visible="false"></asp:Label>
            <asp:Button ID="btnRecalculateQuqntity" runat="server" Text="Recalculate" Visible="false" OnClick="btnRecalculateQuqntity_Click" />
            <asp:Panel ID="pnlMain" runat="server">
                <div>
                    <table style="width: 650px;">
                        <%--<tr>
                            <td style="text-align: right;">
                                <asp:Label ID="lblLiveOrderID" runat="server" Text="Live Order ID :"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:Label ID="lblLiveOrderIDValue" runat="server"></asp:Label>
                            </td>
                        </tr>--%>
                        <tr>
                            <td style="text-align: right;">
                                <asp:Label ID="lblLotNumber" runat="server" Text="LOT Number :"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:Label ID="lblLotNumberValue" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <%-- <tr>
                            <td style="text-align: right;">
                                <asp:Label ID="lblOrderNumber" runat="server" Text="Order Name :"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:Label ID="lblOrderNumberValue" runat="server" Text="2015-Spring-LHR"></asp:Label>
                            </td>
                        </tr>--%>
                        <tr>
                            <td style="text-align: right;">
                                <asp:Label ID="lblCycle" runat="server" Text="Cycle :"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:Label ID="lblCycleNameValue" runat="server" Text="Spring Cycle"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right;">
                                <asp:Label ID="lblOrderName" Text="Order Friendly Name:" runat="server"></asp:Label></td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txOrderName" runat="server" width="350px"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvtxOrderName" runat="server" Display="Dynamic" ControlToValidate="txOrderName"
                                    SetFocusOnError="true" ErrorMessage="* Required"
                                    CssClass="validationErrorMsg"></asp:RequiredFieldValidator></td>

                        </tr>
                        <tr>
                            <td style="text-align: right;">
                                <asp:Label ID="Label1" runat="server" Text="Select Week :"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:DropDownList ID="ddlWeekAndDates" runat="server" Class="dropdown_small"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td style="padding: 10px 150px 0px 0;">
                                <asp:Button ID="btnUpdateQuantity" runat="server" Text="Update Quantity" Class="button" OnClick="BtnUpdateQuantityClick" />
                            </td>
                        </tr>
                    </table>
                    <hr />
                </div>
                <div class="row" style="text-align: left;">
                    <br />
                    <h3 style="text-decoration: underline">Menus included for the above order :</h3>
                    <br />
                </div>
                <div style="padding: 0px; height: 8px;"></div>
                <div class="row">
                    <telerik:RadGrid ID="radGridLiveOrder" runat="server" AutoGenerateColumns="false" Skin="Silk" ShowHeader="true" PageSize="500"
                        PagerStyle-AlwaysVisible="true" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center" PagerStyle-BorderStyle="None"
                        AlternatingItemStyle-BackColor="#eeeded" AllowSorting="true" AllowPaging="true" AllowMultiRowSelection="false" AllowMultiRowEdit="false" GridLines="Both"
                        AllowFilteringByColumn="true" EnableViewState="true" ViewStateMode="Enabled" HeaderStyle-BackColor="#aa2029" HeaderStyle-ForeColor="#FFFFFF"
                        OnNeedDataSource="radGridLiveOrder_NeedDataSource" OnItemCommand="radGridLiveOrder_ItemCommand" ShowGroupPanel="true" OnDetailTableDataBind="radGridLiveOrder_DetailTableDataBind"
                        RetainExpandStateOnRebind="true">
                        <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
                        <GroupingSettings CaseSensitive="false" />
                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" AllowKeyboardNavigation="true">
                            <Selecting AllowRowSelect="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                            <Resizing EnableRealTimeResize="false" AllowColumnResize="false" />
                        </ClientSettings>
                        <MasterTableView TableLayout="Fixed" AutoGenerateColumns="false" CommandItemDisplay="Top" CssClass="rdheaderCss" FilterItemStyle-HorizontalAlign="Center" DataKeyNames="MenuId">
                            <CommandItemSettings />
                            <SortExpressions>
                                <telerik:GridSortExpression FieldName="MenuId" SortOrder="Descending" />
                            </SortExpressions>
                            <CommandItemTemplate>
                                <asp:LinkButton ID="lnkbtnClearAllFilter" runat="server" CommandName="ClearFilter" Text="Clear all filter" TabIndex="13" CssClass="FilterGridCommand"></asp:LinkButton>
                                <asp:LinkButton ID="lnkbtnRefreshGrid" runat="server" CommandName="Refresh" Text="Refresh" TabIndex="14" CssClass="FilterGridCommand marginLeft"></asp:LinkButton>
                            </CommandItemTemplate>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Live order details Id" AllowFiltering="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Display="false" AllowSorting="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGvLiveOrderDetailsId" runat="server" Text='<%#Eval("MenuId") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Menu Code" UniqueName="MenuCode" DataField="MenuCode" AllowFiltering="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="MenuCode" FilterControlWidth="70%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGvMenuCode" runat="server" Text='<%#Eval("MenuCode") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Class Name" UniqueName="ClassName" DataField="ClassName" AllowFiltering="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="ClassName" FilterControlWidth="70%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGvClassName" runat="server" Text='<%#Eval("ClassName") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Menu Type" UniqueName="MenuType" DataField="MenuType" AllowFiltering="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="MenuType" FilterControlWidth="70%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGvMenuType" runat="server" Text='<%#Eval("MenuType") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Language Name" UniqueName="LanguageName" DataField="LanguageName" AllowFiltering="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="LanguageName" FilterControlWidth="70%">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGvLanguageName" runat="server" Text='<%#Eval("LanguageName") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Quantity" UniqueName="Quantity" DataField="Quantity" AllowFiltering="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="Quantity">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGvQuantityTop" runat="server" Text='<%#Eval("Quantity") %>'></asp:Label>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action" AllowFiltering="false" AllowSorting="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvlnkbtnRemoveFromLiveOrder" runat="server" Text="Remove from Live Order" CommandName="REMOVEFROMLIVEORDER" CommandArgument='<%#Eval("MenuId") %>' CausesValidation="false" ToolTip="Remove form Live Order" ForeColor="#B72B3C" Font-Bold="false" Font-Underline="true"></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <DetailTables>
                                <telerik:GridTableView DataKeyNames="MenuId" Name="LiveOrderDetails" Width="100%" AllowSorting="False" AllowPaging="False" AllowFilteringByColumn="False" HeaderStyle-BackColor="#808080" EditMode="InPlace" RetainExpandStateOnRebind="true">
                                    <ParentTableRelation>
                                        <telerik:GridRelationFields DetailKeyField="MenuId" MasterKeyField="MenuId" />
                                    </ParentTableRelation>
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Live order details Id" AllowFiltering="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Display="false" AllowSorting="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvLiveOrderDetailsId" runat="server" Text='<%#Eval("LiveOrderId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblGvLiveOrderDetailsIdEdit" runat="server" Text='<%#Eval("LiveOrderId") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Menu Id" AllowFiltering="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Display="false" AllowSorting="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvMenuIdDetail" runat="server" Text='<%#Eval("LiveOrderId") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblGvMenuIdDetailEdit" runat="server" Text='<%#Eval("LiveOrderId") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Route" UniqueName="Route" DataField="Route" AllowFiltering="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="Route">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvRoute" runat="server" Text='<%#Eval("Route") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblGvRouteEdit" runat="server" Text='<%#Eval("Route") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Flight No." UniqueName="FlightNo" DataField="FlightNo" AllowFiltering="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="FlightNo">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvFlightNo" runat="server" Text='<%#Eval("FlightNo") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Label ID="lblGvFlightNoEdit" runat="server" Text='<%#Eval("FlightNo") %>'></asp:Label>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>

                                        <telerik:GridTemplateColumn HeaderText="Quantity" UniqueName="Quantity" DataField="Quantity" AllowFiltering="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" SortExpression="Quantity" HeaderStyle-Width="250px" ItemStyle-Width="250px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGvQuantity" runat="server" Text='<%#Eval("Quantity") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <div>
                                                    <asp:TextBox ID="txtGvQuantity" runat="server" Text='<%#Eval("Quantity") %>' Width="200px"></asp:TextBox>
                                                </div>
                                                <div>
                                                    <asp:RequiredFieldValidator ID="rfvQuantity" runat="server" CssClass="validationErrorMsg" Display="Dynamic" ErrorMessage="Please enter quantity."
                                                        SetFocusOnError="true" ControlToValidate="txtGvQuantity" ValidationGroup="GRIDEDITVALID"></asp:RequiredFieldValidator>
                                                </div>
                                                <div>
                                                    <asp:RegularExpressionValidator ID="revQuantity" runat="server" CssClass="validationErrorMsg" Display="Dynamic" ErrorMessage="Number  only."
                                                        SetFocusOnError="true" ControlToValidate="txtGvQuantity" ValidationExpression="^\d+$" ValidationGroup="GRIDEDITVALID"></asp:RegularExpressionValidator>
                                                </div>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Edit" AllowFiltering="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" AllowSorting="false">
                                            <ItemTemplate>
                                                <telerik:RadButton ID="radbtnEdit" runat="server" Text="Edit" CommandName="EDIT" CommandArgument='<%#Eval("LiveOrderId") %>' CausesValidation="false"></telerik:RadButton>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <telerik:RadButton ID="radbtnUpdate" runat="server" Text="Update" CommandName="UPDATE" CommandArgument='<%#Eval("LiveOrderId") %>' ValidationGroup="GRIDEDITVALID"></telerik:RadButton>
                                                <telerik:RadButton ID="radbtnCancel" runat="server" Text="Cancel" CommandName="CANCEL" CommandArgument='<%#Eval("LiveOrderId") %>' CausesValidation="false"></telerik:RadButton>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </telerik:GridTableView>
                            </DetailTables>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <%--<telerik:RadAjaxLoadingPanel ID="gridLoadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>--%>
                    <telerik:RadAjaxManager ID="radAjaxManager" runat="server">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="radAjaxManager">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="radGridLiveOrder" LoadingPanelID="gridLoadingPanel"></telerik:AjaxUpdatedControl>
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                            <telerik:AjaxSetting AjaxControlID="radGridLiveOrder">
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="radGridLiveOrder" LoadingPanelID="gridLoadingPanel"></telerik:AjaxUpdatedControl>
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                </div>
                <br />
                <div style="text-align: center;">
                    <input type="button" value="Order Now" class="button" onclick="GetConfirm()" />
                    <asp:Button ID="btnOrderNow" runat="server" Text="Order Now" CssClass="button" OnClick="OrderNowBtnClicked" Style="display: none!important;" />
                </div>
                <asp:Button ID="btnRemoveFormLiveOrder" runat="server" Style="display: none" OnClick="btnRemoveFormLiveOrder_Click" />
                <asp:Button ID="btnClearSession" runat="server" Style="display: none" OnClick="btnClearSession_Click" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="updpnlOrder" runat="server" DisplayAfter="0">
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
</asp:Content>
