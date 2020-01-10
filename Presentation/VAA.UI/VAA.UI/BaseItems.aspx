<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="BaseItems.aspx.cs" Inherits="VAA.UI.ManageBaseItems" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3 style="text-decoration: underline">View Base Menu Items</h3>
    <br />
    <div>
        <table style="width: 650px;">
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="lblClass" runat="server" Text="Menu Class :"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlClass" runat="server" Class="dropdown" AutoPostBack="True" OnSelectedIndexChanged="ddlClass_SelectedIndexChanged"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvClass" runat="server" ControlToValidate="ddlClass" InitialValue="0" Display="Dynamic" SetFocusOnError="true" ErrorMessage=">> Please select a class." ValidationGroup="ManageMenuItem" CssClass="validation-error"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="lblMenuType" runat="server" Text="Menu Type :"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlMenuType" runat="server" Class="dropdown"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvMenuType" runat="server" ControlToValidate="ddlMenuType" InitialValue="0" Display="Dynamic" SetFocusOnError="true" ErrorMessage=">> Please select a Menu Type." ValidationGroup="ManageMenuItem" CssClass="validation-error"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="lblLanguage" runat="server" Text="Language :"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlLanguage" runat="server" Class="dropdown"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvLanguage" runat="server" ControlToValidate="ddlLanguage" InitialValue="0" Display="Dynamic" SetFocusOnError="true" ErrorMessage=">> Please select a language." ValidationGroup="ManageMenuItem" CssClass="validation-error"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td style="text-align: left;"></td>
                <td style="padding: 30px 20px 50px 0;">
                    <asp:Button ID="btnViewMenu" runat="server" Text="View Menu List" Class="button" OnClick="btnViewMenu_Click" ValidationGroup="ManageMenuItem" />
                </td>
            </tr>
        </table>      
    </div>
    <div id="ListViewDiv" visible="false" runat="server">
        <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecoratedControls="All" DecorationZoneID="demo-container" EnableRoundedCorners="false" />
        <div class="demo-container size-wide" id="demo-container">
            <telerik:RadAjaxManager ID="RadAjaxManagerBaseItem" runat="server" OnAjaxRequest="RadAjaxManagerBaseItem_AjaxRequest">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="ListViewPanelBaseItems">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="ListViewPanelBaseItems" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
                <ClientEvents OnRequestStart="RequestStart"></ClientEvents>
            </telerik:RadAjaxManager>
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Sunset"></telerik:RadAjaxLoadingPanel>
            <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                <script type="text/javascript">
                    function RequestStart(sender, eventArgs) {
                        //disable ajax on update/insert operation to upload the image
                        if ((eventArgs.get_eventTarget().indexOf("Update") > -1) || (eventArgs.get_eventTarget().indexOf("PerformInsert") > -1)) {
                            eventArgs.set_enableAjax(false);
                        }
                        else {
                            eventArgs.set_enableAjax(true);
                        }
                    }

                    function ShowInsertForm(cid, mtid, lanid) {
                        window.radopen("Add_BaseItem.aspx?cid=" + cid + "&mtid=" + mtid + "&lanid=" + lanid, "UserListDialog");
                        return false;
                    }

                    function ShowEditForm(id) {
                        window.radopen("Add_BaseItem.aspx?ID=" + id, "UserListDialog");
                        return false;
                    }
                    function confirmCallBackUserFn(args) {
                        if (args) {
                            var currentURL = window.location.href + "/DeleteBaseItem";
                            $.ajax({
                                type: "POST",
                                url: currentURL,
                                data: "",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                success: function (msg) {
                                    var listView = $find("<%= RadListViewBaseItems.ClientID %>");
                                    listView.rebind();
                                },
                                error: function (e) { }
                            });
                            }
                        }

                        function RefreshListView(arg) {
                            if (!arg) {
                                var listView = $find("<%= RadListViewBaseItems.ClientID %>");
                                listView.rebind();
                            }
                        }
                </script>
            </telerik:RadCodeBlock>
            <telerik:RadWindowManager ID="radWindowManager" runat="server" EnableShadow="true">
                <Windows>
                    <telerik:RadWindow ID="UserListDialog" runat="server" AutoSize="false" Behaviors="Close" VisibleStatusbar="false" Title="Editing record" Height="620px" Width="675px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true"></telerik:RadWindow>
                </Windows>
            </telerik:RadWindowManager>
            <table>
                <tr>
                    <td>
                        <asp:Label id="lblsearchBaseItem" Text="Search Item Code:" runat="server"></asp:Label>
                        <telerik:RadSearchBox ID ="searchBaseItem" runat="server" Skin="Simple" Width="300"                        
                                OnSearch="searchBaseItem_clicked">
                            <DropDownSettings Height="400" Width="300" />
                        </telerik:RadSearchBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="ListViewPanelBaseItems" runat="server">
                            <telerik:RadListView ID="RadListViewBaseItems" runat="server" ItemPlaceholderID="BaseItemContainer" DataKeyNames="BaseItemId"
                                AllowPaging="True" OnNeedDataSource="RadListView1_NeedDataSource" OnItemCommand="RadListViewBaseItems_ItemCommand"
                                OnItemDataBound="RadListViewBaseItems_ItemDataBound">
                                <LayoutTemplate>
                                    <fieldset id="FiledSet1" class="mainFieldset">
                                        <div style="padding: 5px 10px 5px 10px; border: 1px solid #828282; margin-bottom: 4px; background-color: #EEEEEE;" align="right">
                                            <telerik:RadButton ID="radbtnRefresh" runat="server" CommandName="Refresh" Text="Refresh"></telerik:RadButton>
                                            <%--<telerik:RadButton ID="radbtnAddBaseItem" runat="server" CommandName="Insert" Text="Add new base item"></telerik:RadButton>--%>
                                        </div>
                                        <div class="clearFix"></div>
                                        <legend style="color: #696969; font-weight: 500; text-decoration: underline; font-size: 18px; text-align: left;">Existing Base Items List :</legend>
                                        <table cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td>
                                                    <telerik:RadDataPager ID="RadDataPager1" runat="server" PagedControlID="RadListViewBaseItems" PageSize="4" Style="padding: 10px 0px 10px 0px;">
                                                        <Fields>
                                                            <telerik:RadDataPagerButtonField FieldType="FirstPrev"></telerik:RadDataPagerButtonField>
                                                            <telerik:RadDataPagerButtonField FieldType="Numeric" PageButtonCount="6"></telerik:RadDataPagerButtonField>
                                                            <telerik:RadDataPagerButtonField FieldType="NextLast"></telerik:RadDataPagerButtonField>
                                                            <telerik:RadDataPagerPageSizeField PageSizeComboWidth="60" PageSizeText="Page size: "></telerik:RadDataPagerPageSizeField>
                                                            <telerik:RadDataPagerGoToPageField CurrentPageText="Page: " TotalPageText="of" SubmitButtonText="Go"
                                                                TextBoxWidth="25"></telerik:RadDataPagerGoToPageField>
                                                        </Fields>
                                                    </telerik:RadDataPager>
                                                </td>
                                            </tr>
                                        </table>
                                        <div class="RadListView">
                                            <asp:PlaceHolder ID="BaseItemContainer" runat="server"></asp:PlaceHolder>
                                        </div>
                                        <div class="clearFix"></div>
                                        <table class="commandTable">
                                            <tr>
                                                <td class="sortCell" style="background-color: #E6E6E6; padding: 10px; border: 1px solid #828282">
                                                    <telerik:RadButton ID="radbtnAddBaseItem" runat="server" CommandName="Insert" Text="Add new base item" Style="float: left; color: #aa2029;"></telerik:RadButton>
                                                    <asp:Label ID="lblBaseitemSort" runat="server" AssociatedControlID="ddListBaseitemSort" Text="Sort by: " CssClass="sortLabel"></asp:Label>
                                                    <telerik:RadComboBox ID="ddListBaseitemSort" Width="185px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddListBaseitemSort_SelectedIndexChanged">
                                                        <Items>
                                                            <telerik:RadComboBoxItem Text="-Select field to sort-" Value=""></telerik:RadComboBoxItem>
                                                            <telerik:RadComboBoxItem Text="BaseItem ID" Value="BaseItemId"></telerik:RadComboBoxItem>
                                                            <telerik:RadComboBoxItem Text="BaseItem Code" Value="BaseItemCode"></telerik:RadComboBoxItem>
                                                            <telerik:RadComboBoxItem Text="Category Name" Value="CategoryName"></telerik:RadComboBoxItem>
                                                            <telerik:RadComboBoxItem Text="Clear sort" Value="ItemTitle"></telerik:RadComboBoxItem>
                                                        </Items>
                                                    </telerik:RadComboBox>
                                                    <asp:RadioButtonList RepeatLayout="Flow" RepeatDirection="Horizontal" ID="rblBaseitemSort"
                                                        runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblBaseitemSort_SelectedIndexChanged">
                                                        <asp:ListItem Text="Ascending" Value="ASC" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="Descending" Value="DESC"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <fieldset class="fieldset" style="text-align: left;">
                                        <legend>
                                            <%# Eval("BaseItemTitle") %></legend>
                                        <table class="dataTable">
                                            <tr class="rlvI">
                                                <td>
                                                    <table class="itemTable">
                                                        <tr>
                                                            <td>
                                                                <table class="innerItemTable">
                                                                    <tr>
                                                                        <td hidden class="itemCellLabel">BaseItem Id:
                                                                        </td>
                                                                        <td hidden class="itemCellInfo">
                                                                            <asp:Label ID="lblBaseitemId" runat="server" Text='<%#Eval("BaseItemID")%>'></asp:Label></td>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="itemCellLabel">Category:
                                                            </td>
                                                            <td class="itemCellInfo">
                                                                <%#Eval("CategoryName")%>
                                                            </td>
                                                        </tr>
                                                         <tr>
                                                            <td class="itemCellLabel">Category Code:
                                                            </td>
                                                            <td class="itemCellInfo">
                                                                <%#Eval("CategoryCode")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="itemCellLabel">Item Code:
                                                            </td>
                                                            <td>
                                                                <%#Eval("BaseItemCode")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Title Description:
                                                            </td>
                                                            <td>
                                                                <%#Eval("BaseItemTitleDescription")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Description:
                                                            </td>
                                                            <td>
                                                                <div style="width: 100%; height: 80px; resize: none; overflow-y: scroll; overflow-x: hidden; border: 1px solid #F1F1F1;">
                                                                    <%# Eval("BaseItemDescription")%>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Sub Description:
                                                            </td>
                                                            <td>
                                                                <%# Eval("BaseItemSubDescription")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>Attributes:
                                                            </td>
                                                            <td>
                                                                <%#Eval("BaseItemAttributes")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <telerik:RadButton ID="btnEditBaseItem" CssClass="btnEdit" runat="server" Text="Edit" Width="70px"></telerik:RadButton>
                                                    <telerik:RadButton ID="btnDeleteBaseItem" CssClass="btnDelete" runat="server" Text="Delete" CommandName="Delete" Width="70px" CommandArgument='<%#Eval("BaseItemID")%>'></telerik:RadButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </fieldset>
                                </ItemTemplate>
                                <EmptyDataTemplate>
                                    <fieldset id="FiledSet2" class="mainFieldset">
                                        <div style="padding: 5px 10px 5px 10px; border: 1px solid #828282; margin-bottom: 4px; background-color: #EEEEEE;" align="right">
                                            <telerik:RadButton ID="radbtnAddBaseItem" runat="server" CommandName="Insert" Text="Add new base item"></telerik:RadButton>
                                        </div>
                                        <fieldset style="width: 100%!important;" class="noRecordsFieldset">
                                            No records for base item available.
                                        </fieldset>
                                    </fieldset>
                                </EmptyDataTemplate>
                            </telerik:RadListView>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>
        <br />
    </div>
</asp:Content>

