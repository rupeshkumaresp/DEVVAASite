<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MenuItems.aspx.cs" Inherits="VAA.UI.MenuItems" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3 style="text-decoration: underline">Add a New Cycle</h3>
            <br/>
    <div style="padding-right: 150px; text-align: left;">
            <table style="text-align: left;" >
                <tr>
                    <td style="text-align: left;">
                        <asp:Label ID="Label1" runat="server" Text="Menu Class :"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                         <asp:DropDownList ID="DropDownList1" runat="server" Class="dropdown"></asp:DropDownList>
                    </td>
                </tr>
                 <tr>
                   <td style="text-align: left;">
                         <asp:Label ID="Label2" runat="server" Text="Menu Type :"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:DropDownList ID="DropDownList2" runat="server" Class="dropdown"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left;"></td>
                   <td style="text-align: left; padding-bottom: 30px;">
                        <asp:Button ID="Button1" runat="server" Text="View Menu List" Class="button" OnClick="ViewMenuListBtnClicked" />
                    </td>
                </tr>
            </table>
        </div>
    <div>
        <h3 style="text-decoration: underline">Existing Menu List :</h3>
            <br/>
        <h3 class = "noteDiv">Existing Menu using Telerik List View Control goes here..</h3>
    </div>
</asp:Content>
