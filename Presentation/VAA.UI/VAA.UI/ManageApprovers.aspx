<%@ Page Title="Manage Approvers" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageApprovers.aspx.cs" Inherits="VAA.UI.ManageApprovers" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <telerik:RadAjaxPanel ID="RadAjaxPanelApproverPage" runat="server">

        <script type="text/javascript">
            function ApproverAdded() {
                bootbox.alert("Approver Added successfully.");
            }

            function ApproverError() {
                bootbox.alert("Cannot add approver, please review the options selected!");
            }

        </script>
        <h3 style="text-decoration: underline">Manage Approvers</h3>
        <br />
        <div>
            <table style="width: 650px;">
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="lblCycle" runat="server" Text="Origin Airport :"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlOrigin" runat="server" Class="dropdown" AutoPostBack="True" OnSelectedIndexChanged="ddlOrigin_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="lblClass" runat="server" Text="Menu Class :"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlClass" runat="server" Class="dropdown" AutoPostBack="True" OnSelectedIndexChanged="ddlClass_SelectedIndexChanged"></asp:DropDownList>
                    </td>
                </tr>
                <asp:Panel ID="plnVirginApprover" runat="server" Visible="False">
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblVirginApp" runat="server" Text="Virgin Approver :"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:Label ID="lblCurrVirginApp" runat="server" Style="text-align: left; padding-left: 40px;" />
                        </td>
                    </tr>
                </asp:Panel>
                <asp:Panel ID="plnChangeVirginApprover" runat="server" Visible="False">
                    <tr>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Change Virgin Approver :"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlVirginApprover" runat="server" Class="dropdown" AutoPostBack="True"></asp:DropDownList>
                        </td>
                    </tr>
                </asp:Panel>
                <asp:Panel ID="plnCatererApp" runat="server" Visible="False">
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblCatererApp" runat="server" Text="Caterer Approver :"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:Label ID="lblCurrCatererApp" runat="server" Style="text-align: left; padding-left: 40px;" />
                        </td>
                    </tr>
                </asp:Panel>
                <asp:Panel ID="plnChangeCatererApp" runat="server" Visible="False">
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Change Caterer Approver :"></asp:Label></td>
                        <td>
                            <asp:DropDownList ID="ddlCatererApprover" runat="server" Class="dropdown" AutoPostBack="True"></asp:DropDownList>
                        </td>
                    </tr>
                </asp:Panel>
                <asp:Panel ID="plnTranslationApp" runat="server" Visible="False">
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblTranslationApp" runat="server" Text="Translation Approver :"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:Label ID="lblCurrTranslationApp" runat="server" Style="text-align: left; padding-left: 40px;" />
                        </td>
                    </tr>
                </asp:Panel>
                <asp:Panel ID="plnChangeTranslationApp" runat="server" Visible="False">
                    <tr>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Change Translation Approver :"></asp:Label></td>
                        <td>
                            <asp:DropDownList ID="ddlTranslationApprover" runat="server" Class="dropdown" AutoPostBack="True"></asp:DropDownList>
                        </td>
                    </tr>
                </asp:Panel>
                <tr>
                    <td></td>
                    <td style="text-align: right; padding: 30px 0px 0px 0;">
                        <asp:LinkButton ID="btnChangeApprover" runat="server" OnClick="ChangeApproverClicked" Visible="false" Text="Change Approvers" Style="text-align: left; color: #aa2029;"> </asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td style="padding: 30px 150px 50px 0;">
                        <asp:Button ID="btnUpdateApprover" runat="server" Text="Update Approvers" Class="button" OnClick="btnUpdateApprover_Click" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Label ID="lblmessage" runat="server" Visible="false" />
                    </td>
                </tr>
            </table>
        </div>
    </telerik:RadAjaxPanel>
</asp:Content>
