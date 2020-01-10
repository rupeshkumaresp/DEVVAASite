<%@ Page Title="User" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="VAA.UI.Users" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%--<asp:UpdatePanel ID="updpnlUsers" runat="server">
        <ContentTemplate>--%>
    <style type="text/css">
        #MainContentPanel {
            padding: 20px 20px 20px 20px !important;
        }

        html .RadSearchBox .rsbInner {
            height: 28px;
            font-size: 15px;
            border: 1px solid #000;
            border-radius: 0px !important;
            font-style: normal !important;
            padding: 0px !important;
            margin: 0px !important;
        }

        html .RadSearchBox .rsbInput {
            height: 25px;
            font-size: 15px;
        }

        .RadSearchBox .rsbButtonSearch {
            height: 30px;
            background-color: #AA2029;
        }

        .rsbButtonSearch .rsbIcon {
            background-image: url('../Images/search-13-16.png') !important;
            background-position: 0 !important;
        }

        .riTextBox {
            width: 100% !important;
            height: 100% !important;
        }
    </style>
    <%-- <div>
                <telerik:RadSearchBox ID="RadSearchBox1" runat="server" EnableAutoComplete="false" EmptyMessage="Search Users" Width="400px" Skin="Silk" CssClass="floatRight"></telerik:RadSearchBox>
            </div>--%>
    <div class="spacerDiv"></div>
    <br />
    <div style="padding: 30px 30px 30px 30px!important; background-color: #F3F3F3!important;" align="right">
        <div>
            <telerik:RadFormDecorator ID="radFormDecoratorUser" runat="server" DecoratedControls="All" DecorationZoneID="user-container" EnableRoundedCorners="false" />
            <div class="demo-container size-wide" id="user-container">
                <telerik:RadAjaxManager ID="radAjaxManagerUser" runat="server" OnAjaxRequest="radAjaxManagerUser_AjaxRequest">
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="lstViewPanelUser">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="lstViewPanelUser" LoadingPanelID="radAjaxLoadPnlUser"></telerik:AjaxUpdatedControl>
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                        <telerik:AjaxSetting AjaxControlID="radWindowManager">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="radWindowManager" LoadingPanelID="radAjaxLoadPnlUser"></telerik:AjaxUpdatedControl>
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                    </AjaxSettings>
                    <ClientEvents OnRequestStart="RequestStart"></ClientEvents>
                </telerik:RadAjaxManager>
                <telerik:RadAjaxLoadingPanel ID="radAjaxLoadPnlUser" runat="server" Skin="Sunset"></telerik:RadAjaxLoadingPanel>
                <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                    <script type="text/javascript">
                        function RequestStart(sender, eventArgs) {
                            //disable ajax on update/insert operation to upload the image
                            //if ((eventArgs.get_eventTarget().indexOf("Update") > -1) || (eventArgs.get_eventTarget().indexOf("PerformInsert") > -1)) {
                            //    eventArgs.set_enableAjax(false);
                            //}
                        }

                        function ShowEditForm(id) {
                            window.radopen("AddEdit_User.aspx?ID=" + id, "UserListDialog");
                            return false;
                        }

                        function ShowInsertForm() {
                            window.radopen("AddEdit_User.aspx?ID=0", "UserListDialog");
                        }

                        function confirmCallBackUserFn(args) {
                            //alert(args);

                            if (args) {
                                var currentURL = window.location.href + "/DeleteUser";
                                $.ajax({
                                    type: "POST",
                                    url: currentURL,
                                    data: "",
                                    contentType: "application/json; charset=utf-8",
                                    dataType: "json",
                                    success: function (msg) {
                                        var listView = $find("<%= radListViewUsers.ClientID %>");
                                                listView.rebind();
                                            },
                                            error: function (e) {
                                            }
                                        });
                                        }
                                    }

                                    function RefreshListView(arg) {
                                        if (!arg) {
                                            var listView = $find("<%= radListViewUsers.ClientID %>");
                                            listView.rebind();
                                        }
                                    }
                    </script>
                </telerik:RadCodeBlock>
                <telerik:RadWindowManager ID="radWindowManager" runat="server" EnableShadow="true">
                    <Windows>
                        <telerik:RadWindow ID="UserListDialog" runat="server" AutoSize="false" Behaviors="Close" VisibleStatusbar="false" Title="Editing record" Height="600px" Width="800px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
                        </telerik:RadWindow>
                    </Windows>
                </telerik:RadWindowManager>
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <asp:Panel ID="lstViewPanelUser" runat="server">
                                <telerik:RadListView ID="radListViewUsers" runat="server" Skin="Silk" ItemPlaceholderID="UsersContainer" AllowPaging="true"
                                    OnItemCommand="radListViewUsers_ItemCommand" OnItemDataBound="radListViewUsers_ItemDataBound"
                                    OnNeedDataSource="radListViewUsers_NeedDataSource">
                                    <LayoutTemplate>
                                        <fieldset id="FiledSet1" class="mainFieldset">
                                            <div style="padding: 5px 10px 5px 10px; border: 1px solid #828282; margin-bottom: 4px; background-color: #EEEEEE;" align="right">
                                                <telerik:RadButton ID="radbtnRefresh" runat="server" CommandName="Refresh" Text="Refresh"></telerik:RadButton>
                                                <telerik:RadButton ID="radbtnCreateUser" runat="server" CommandName="Insert" Text="Add new user"></telerik:RadButton>
                                            </div>
                                            <div class="clearFix"></div>
                                            <legend>Users</legend>
                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td>
                                                        <telerik:RadDataPager ID="radDataPagerUser" runat="server" PagedControlID="radListViewUsers" PageSize="4" CssClass="pagerStyle" Style="padding: 10px 0px 10px 0px;">
                                                            <Fields>
                                                                <telerik:RadDataPagerButtonField FieldType="FirstPrev"></telerik:RadDataPagerButtonField>
                                                                <telerik:RadDataPagerButtonField FieldType="Numeric" PageButtonCount="6"></telerik:RadDataPagerButtonField>
                                                                <telerik:RadDataPagerButtonField FieldType="NextLast"></telerik:RadDataPagerButtonField>
                                                                <telerik:RadDataPagerPageSizeField PageSizeComboWidth="60" PageSizeText="Page size: "></telerik:RadDataPagerPageSizeField>
                                                                <telerik:RadDataPagerGoToPageField CurrentPageText="Page: " TotalPageText="of" SubmitButtonText="Go" TextBoxWidth="25"></telerik:RadDataPagerGoToPageField>
                                                            </Fields>
                                                        </telerik:RadDataPager>
                                                    </td>
                                                </tr>
                                            </table>
                                            <div class="RadListView RadListView_<%# Container.Skin %>">
                                                <asp:PlaceHolder ID="UsersContainer" runat="server"></asp:PlaceHolder>
                                            </div>
                                            <div class="clearFix">
                                            </div>
                                            <table class="commandTable" style="width: 100%!important;">
                                                <tr>
                                                    <td class="sortCell" style="background-color: #E6E6E6; padding: 10px; border: 1px solid #828282">
                                                        <asp:Label ID="lblUserSort" runat="server" AssociatedControlID="ddListUserSort" Text="Sort by:" CssClass="sortLabel"></asp:Label>
                                                        <telerik:RadComboBox ID="ddListUserSort" Width="185px" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddListUserSort_SelectedIndexChanged">
                                                            <Items>
                                                                <telerik:RadComboBoxItem Text="-Select field to sort-" Value=""></telerik:RadComboBoxItem>
                                                                <telerik:RadComboBoxItem Text="Name" Value="Name"></telerik:RadComboBoxItem>
                                                                <telerik:RadComboBoxItem Text="User name" Value="UserName"></telerik:RadComboBoxItem>
                                                                <telerik:RadComboBoxItem Text="UserType" Value="UserType"></telerik:RadComboBoxItem>
                                                                <telerik:RadComboBoxItem Text="City" Value="City"></telerik:RadComboBoxItem>
                                                                <telerik:RadComboBoxItem Text="Country" Value="Country"></telerik:RadComboBoxItem>
                                                                <telerik:RadComboBoxItem Text="Clear sort" Value="ClearSort"></telerik:RadComboBoxItem>
                                                            </Items>
                                                        </telerik:RadComboBox>
                                                        <asp:RadioButtonList RepeatLayout="Flow" RepeatDirection="Horizontal" ID="rblSortUser" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblSortUser_SelectedIndexChanged">
                                                            <asp:ListItem Text="Ascending" Value="ASC" Selected="True"></asp:ListItem>
                                                            <asp:ListItem Text="Descending" Value="DESC"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <div align="center" style="width: 50%!important; float: left;">
                                            <fieldset class="fieldset" style="width: 98%!important; padding: 8px!important; text-align: left; margin-left: 4px; margin-right: 4px;">
                                                <legend><%# Eval("Name") %></legend>
                                                <table class="dataTable tablewidth">
                                                    <tr class="rlvI">
                                                        <td>
                                                            <table class="itemTable tablewidth">
                                                                <tr>
                                                                    <td valign="top">
                                                                        <table class="innerItemTable tablewidth">
                                                                            <tr style="display: none;">
                                                                                <td>ID:</td>
                                                                                <td>
                                                                                    <asp:Label ID="lblListUserId" runat="server" Text='<%#Eval("UserId")%>'></asp:Label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td valign="top">User ID:</td>
                                                                                <td valign="top">
                                                                                    <asp:Label ID="lblEmail" runat="server" CssClass="long-email-address-css" Text='<%#Eval("UserName")%>'></asp:Label></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>UserType:</td>
                                                                                <td><%#Eval("UserType")%></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>City:</td>
                                                                                <td><%#Eval("City")%></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>Country:</td>
                                                                                <td><%#Eval("Country")%></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>Phone:</td>
                                                                                <td><%#Eval("Phone")%></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td class="image" valign="top" align="right">
                                                                        <telerik:RadBinaryImage ID="radBinaryUserImage" runat="server" AlternateText="User Image" ToolTip="User Image" ImageUrl="~/Images/Noavatar.png" Width="90px" Height="110px" Style="height: 110px!important; width: 90px!important; border: 1px solid #F1F1F1!important;" ResizeMode="Fit" DataValue='<%# Eval("UserImage") == DBNull.Value? new System.Byte[0]: Eval("UserImage") %>'></telerik:RadBinaryImage>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <telerik:RadButton ID="EditLink" CssClass="btnEdit" runat="server" Text="Edit" Width="70px"></telerik:RadButton>
                                                            <telerik:RadButton ID="btnDelete" CssClass="btnDelete" runat="server" Text="Delete" CommandName="Delete" Width="70px" CommandArgument='<%#Eval("UserId")%>'></telerik:RadButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </fieldset>
                                        </div>
                                    </ItemTemplate>
                                    <EmptyDataTemplate>
                                        <fieldset id="FiledSet2" class="mainFieldset">
                                            <div style="padding: 5px 10px 5px 10px; border: 1px solid #828282; margin-bottom: 4px; background-color: #EEEEEE;" align="right">
                                                <telerik:RadButton ID="radbtnCreateUser" runat="server" CommandName="Insert" Text="Add new user"></telerik:RadButton>
                                            </div>
                                            <fieldset class="noRecordsFieldset">
                                                No existing users available to view.
                                            </fieldset>
                                        </fieldset>
                                    </EmptyDataTemplate>
                                </telerik:RadListView>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
