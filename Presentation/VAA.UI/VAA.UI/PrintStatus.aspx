<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PrintStatus.aspx.cs" Inherits="VAA.UI.PrintStatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style type="text/css">
        .gvHeaderStyle {
            background-color: #B72B3C !important;
            color: #FFFFFF !important;
        }

        .rgHeader a {
            color: #FFFFFF !important;
        }
    </style>

    <asp:UpdatePanel ID="updpnlManagePrintStatus" runat="server">
        <ContentTemplate>
            <h3 style="text-decoration: underline">Proof Status</h3>
            <br />
            <div>
                <table style="width: 650px;">
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblCycle" runat="server" Text="Cycle :"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCycle" runat="server" Class="dropdown"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblMenuClass" runat="server" Text="Menu Class :"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlMenuClass" runat="server" Class="dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlMenuClass_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblMenuType" runat="server" Text="Menu Type :"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlMenuType" runat="server" Class="dropdown"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="padding: 10px 150px 5px 0;">
                            <asp:Button ID="btnViewStatus" runat="server" Text="View Status" Class="button" OnClick="BtnViewStatusClick" />
                        </td>
                    </tr>
                </table>
            </div>
            <br />
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

                function ConfirmMoveMenu(menucode) {
                    var mcode = String(menucode);
                    var myControl = document.getElementById('<%=btnMoveMenuData.ClientID%>');
                    myControl.click();
                    return;


                    bootbox.dialog({
                        title: "MOVE TO LIVE ORDER",
                        message: '<div class="row">' +
                            '<div class="col-md-12"><span class="help-block" style="font-size:16px !important;">This operation will place this menu to Live Order. Please confirm if you want to move menu "' + mcode + '" to live order?.</span></div>' +
                            ' </div>',
                        buttons: {
                            success: {
                                label: "OK",
                                className: "btn-success",
                                callback: function () {
                                    var myControl = document.getElementById('<%=btnMoveMenuData.ClientID%>');
                                    myControl.click();
                                }
                            },
                            danger: {
                                label: "Cancel",
                                className: "btn-danger",
                                callback: function () {
                                    var myControl = document.getElementById('<%=btnclearsession.ClientID%>');
                                    myControl.click();
                                }
                            }
                        }
                    });
                }
            </script>
            <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                <script type="text/javascript">
                    function ShowEditForm(id, rowIndex) {
                        var grid = $find("<%= radGridPrintStatus.ClientID %>");
                        var rowControl = grid.get_masterTableView().get_dataItems()[rowIndex].get_element();
                        grid.get_masterTableView().selectItem(rowControl, true);
                        window.radopen("MenuViewer.aspx?ID=" + id, "UserListDialog");
                        return false;
                    }

                    function ShowViewApprover(id) {
                        window.radopen("ViewApprover.aspx?ID=" + id, "ApproverViewWindow");
                        return false;
                    }

                    function refreshGrid(arg) {
                        var masterTable = $find("<%= radGridPrintStatus.ClientID %>").get_masterTableView();
                        masterTable.rebind();
                    }

                    function clientcloseReloadGrid() {
                        $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("Rebind");
            }

            function setWindowsize() {
                var grid = $find("<%= UserListDialog.ClientID %>");
                var viewportWidth = $(window).width();
                var viewportHeight = $(window).height();
                grid.setSize(Math.ceil(viewportWidth * 95 / 100), Math.ceil(viewportHeight * 95 / 100));
                grid.center();
            }

            function ShowMoved() {
                bootbox.alert("Item moved to Live Order successfully!");
            }

            function CannotMove() {
                bootbox.alert("This menu doesnot belong to live cycle and cannot be moved to live orders!");
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

            $(window).resize(function () {
                var grid = $find("<%= UserListDialog.ClientID %>");
                        if (grid.isVisible()) {
                            setWindowsize();
                        }
                    });


                </script>
            </telerik:RadCodeBlock>
            <div>
                <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="radGridPrintStatus">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="radGridPrintStatus" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                    </AjaxSettings>
                </telerik:RadAjaxManager>
                <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
                    <Windows>
                        <telerik:RadWindow ID="UserListDialog" OnClientBeforeClose="OnClientBeforeClose" OnClientClose="clientcloseReloadGrid" runat="server" Modal="true" OnClientShow="setWindowsize" VisibleStatusbar="false" Title="" Behaviors="Close">
                        </telerik:RadWindow>
                        <telerik:RadWindow ID="ApproverViewWindow" runat="server" Modal="true" Height="400px" Width="450px" VisibleStatusbar="false" Title="" Behaviors="Close">
                        </telerik:RadWindow>
                    </Windows>
                </telerik:RadWindowManager>
                <div id="PrintstatusGridDiv" runat="server" visible="False">

                    <div style="float: right" >
                        <asp:Button ID="btnApproveAll" runat="server" Text="Approve All Menus" Class="button" OnClick="btnApproveAll_Click" />
                        <asp:Button ID="btnMoveAlltoOrder" runat="server" Text="Move All to Order" Class="button" OnClick="btnMoveAlltoOrder_Click" />
                    </div>

                    <telerik:RadGrid ID="radGridPrintStatus" Skin="Silk" ShowStatusBar="true" runat="server" Width="100%" AutoGenerateColumns="False" AllowSorting="True"
                        AllowFilteringByColumn="true" AllowMultiRowSelection="False" AllowPaging="True" PagerStyle-AlwaysVisible="true" GridLines="Both" PageSize="10"
                        HeaderStyle-CssClass="gvHeaderStyle" ItemStyle-BackColor="#FFFFFF" AlternatingItemStyle-BackColor="#EDEDED" ShowHeader="true" HeaderStyle-VerticalAlign="Middle"
                        HeaderStyle-HorizontalAlign="Center" PagerStyle-BorderStyle="None" AllowMultiRowEdit="false" EnableViewState="true" ViewStateMode="Enabled"
                        OnNeedDataSource="radGridPrintStatus_NeedDataSource" OnItemDataBound="radGridPrintStatus_ItemDataBound" OnItemCommand="radGridPrintStatus_ItemCommand">
                        <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
                        <ClientSettings>
                            <Scrolling AllowScroll="false" />
                            <ClientEvents OnFilterMenuShowing="filterMenuShowing" />
                        </ClientSettings>
                        <FilterMenu OnClientShowing="MenuShowing" />
                        <MasterTableView DataKeyNames="MenuId" Name="MenuDetails" Width="100%" AllowMultiColumnSorting="True" ExpandCollapseColumn-HeaderText="View Status" ExpandCollapseColumn-HeaderStyle-Width="10%">
                            <SortExpressions>
                                <telerik:GridSortExpression FieldName="MenuId" SortOrder="Descending" />
                            </SortExpressions>
                            <NestedViewTemplate>
                                <asp:Label ID="gvlblNestLanguageId" runat="server" Visible="false" Text='<%#Eval("LanguageID") %>'></asp:Label>
                                <asp:Label ID="gvlblNestPrintOrderStatus" runat="server" Visible="false" Text='<%#Eval("PrintOrderStatus") %>'></asp:Label>
                                <asp:Panel ID="pnlLanguageForFive" runat="server" Visible="false">
                                    <table style="text-align: center;">
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>
                                                <asp:Image ID="gvimgProof51" runat="server" BorderStyle="None" Height="100" Width="100" />
                                            </td>
                                            <td>
                                                <asp:Image ID="gvimgProof52" runat="server" BorderStyle="None" Height="100" Width="100" />
                                            </td>
                                            <td>
                                                <asp:Image ID="gvimgProof53" runat="server" BorderStyle="None" Height="100" Width="100" />
                                            </td>
                                            <td>
                                                <asp:Image ID="gvimgProof54" runat="server" BorderStyle="None" Height="100" Width="100" />
                                            </td>
                                            <td>
                                                <asp:Image ID="gvimgProof55" runat="server" BorderStyle="None" Height="100" Width="100" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>
                                                <asp:Label ID="gvlblProof51" runat="server" Text='<%#Eval("Proof1") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="gvlblProof52" runat="server" Text='<%#Eval("Proof2") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="gvlblProof53" runat="server" Text='<%#Eval("Proof3") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="gvlblProof54" runat="server" Text='<%#Eval("Proof4") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="gvlblProof55" runat="server" Text='<%#Eval("Proof5") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label9" runat="server" Text="Key Person :"></asp:Label>
                                                <asp:Label ID="lblVirginAppId" runat="server" Visible="false" Text='<%#Eval("VirginApproverId") %>'></asp:Label>
                                                <asp:Label ID="lblCatererAppId" runat="server" Visible="false" Text='<%#Eval("CatererApproverId") %>'></asp:Label>
                                                <asp:Label ID="lblTranslatorAppId" runat="server" Visible="false" Text='<%#Eval("TranslatorApproverId") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" Text='<%#Eval("VirginApprover") %>' ForeColor="#B72B3C" SingleClick="true" CommandName="VIEWVIRGINAPP1"></asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" Text='<%#Eval("CatererApprover") %>' ForeColor="#B72B3C" SingleClick="true" CommandName="VIEWCATERERAPP1"></asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton5" runat="server" Text='<%#Eval("VirginApprover") %>' ForeColor="#B72B3C" SingleClick="true" CommandName="VIEWVIRGINAPP2"></asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton6" runat="server" Text='<%#Eval("TranslatorApprover") %>' ForeColor="#B72B3C" SingleClick="true" CommandName="VIEWTRANSLATORAPP"></asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton7" runat="server" Text='<%#Eval("VirginApprover") %>' ForeColor="#B72B3C" SingleClick="true" CommandName="VIEWVIRGINAPP3"></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="pnlLanguageForThree" runat="server" Visible="false">
                                    <table style="text-align: center;">
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>
                                                <asp:Image ID="gvimgProof31" runat="server" BorderStyle="None" Height="100" Width="100" />
                                            </td>
                                            <td>
                                                <asp:Image ID="gvimgProof32" runat="server" BorderStyle="None" Height="100" Width="100" />
                                            </td>
                                            <td>
                                                <asp:Image ID="gvimgProof33" runat="server" BorderStyle="None" Height="100" Width="100" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>
                                                <asp:Label ID="gvlblProof31" runat="server" Text='<%#Eval("Proof1") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="gvlblProof32" runat="server" Text='<%#Eval("Proof2") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="gvlblProof33" runat="server" Text='<%#Eval("Proof3") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label10" runat="server" Text="Key Person :"></asp:Label>
                                                <asp:Label ID="lblVirginAppId2" runat="server" Visible="false" Text='<%#Eval("VirginApproverId") %>'></asp:Label>
                                                <asp:Label ID="lblCatererAppId2" runat="server" Visible="false" Text='<%#Eval("CatererApproverId") %>'></asp:Label>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton" runat="server" Text='<%#Eval("VirginApprover") %>' ForeColor="#B72B3C" SingleClick="true" CommandName="VIEWVIRGINAPP4"></asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton1" runat="server" Text='<%#Eval("CatererApprover") %>' ForeColor="#B72B3C" SingleClick="true" CommandName="VIEWCATERERAPP2"></asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" Text='<%#Eval("VirginApprover") %>' ForeColor="#B72B3C" SingleClick="true" CommandName="VIEWVIRGINAPP5"></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </NestedViewTemplate>
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="Menu Id" DataType="System.Int64" UniqueName="MenuId" DataField="MenuId" Display="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn HeaderText="Meny Class Id" DataType="System.Int32" UniqueName="MenuClassId" DataField="MenuClassId" Display="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn HeaderText="Menu Type Id" DataType="System.Int32" UniqueName="MenuTypeId" DataField="MenuTypeId" Display="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn HeaderText="Menu Code" DataType="System.String" UniqueName="MenuCode" DataField="MenuCode" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" AllowFiltering="true" HeaderStyle-Width="10%" FilterControlWidth="60%"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn HeaderText="Menu Name" DataType="System.String" UniqueName="MenuName" DataField="MenuName" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" AllowFiltering="true" HeaderStyle-Width="36%" FilterControlWidth="70%"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn HeaderText="Language" DataType="System.String" UniqueName="LanguageName" DataField="LanguageName" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" AllowFiltering="true" HeaderStyle-Width="8%" FilterControlWidth="50%"></telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn HeaderText="Print Order Status" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" AllowFiltering="false" HeaderStyle-Width="20%">
                                    <ItemTemplate>
                                        <asp:Label ID="gvlblLanguageId" runat="server" Visible="false" Text='<%#Eval("LanguageID") %>'></asp:Label>
                                        <asp:Label ID="gvlblPrintOrderStatus" runat="server" Visible="false" Text='<%#Eval("PrintOrderStatus") %>'></asp:Label>
                                        <asp:Image ID="gvimgPrintOrderStatus" runat="server" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="View Menu" AllowFiltering="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvlnkbtnViewMenu" runat="server" Text="View Menu" CommandName="VIEWMENU" CausesValidation="false" ToolTip="Open Chili Doc" ForeColor="#B72B3C" Font-Bold="false" SingleClick="true" Font-Underline="true"></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action" AllowFiltering="false" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%" ItemStyle-Width="10%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="gvlnkbtnOrderNow" runat="server" Text="Move to Order" CommandName="ORDERNOW" CommandArgument='<%#Eval("MenuId") %>' CausesValidation="false" ToolTip="Order now" ForeColor="#B72B3C" Font-Bold="false" Font-Underline="true"></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                    </telerik:RadGrid>
                    <asp:Button ID="btnMoveMenuData" runat="server" Style="display: none" OnClick="btnMoveMenuData_Click" />
                    <asp:Button ID="btnclearsession" runat="server" Style="display: none" OnClick="btnclearsession_Click" />
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="updpnlManagePrintStatus" runat="server" DisplayAfter="0">
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
