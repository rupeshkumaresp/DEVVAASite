<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="LiveOrders.aspx.cs" Inherits="VAA.UI.LiveOrders" %>
<%--<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2015.1.401.45, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3 style="text-decoration: underline">Live Order Details</h3>
    <br />
    <div>
        <table style="width: 650px;">
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="lblLiveOrderID" runat="server" Text="Live Order ID :"></asp:Label>
                </td>
                <td style="text-align: left;">
                    <asp:Label ID="lblLiveOrderIDValue" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="lblLotNumber" runat="server" Text="LOT Number :"></asp:Label>
                </td>
                <td style="text-align: left;">
                    <asp:Label ID="lblLotNumberValue" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="text-align: right;">
                    <asp:Label ID="lblOrderNumber" runat="server" Text="Order Name :"></asp:Label>
                </td>
                <td style="text-align: left;">
                    <asp:Label ID="lblOrderNumberValue" runat="server" Text="2015-Spring-LHR"></asp:Label>
                </td>
            </tr>
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
                         <asp:Label ID="lblUpload" runat="server" Text="Upload Flight Schedules File :"></asp:Label>
                    </td>
                    <td>
                        <telerik:RadAsyncUpload ID="RadAsyncUploadSchedule" runat="server" Skin="Glow"></telerik:RadAsyncUpload>
                    </td>
                </tr>
                 <tr>
                    <td style="text-align: right;">
                         <asp:Label ID="lblClearOldUpload" runat="server" Text="Clear old flight Schedule:"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:CheckBox  ID="chkClearFlightSchedule" Checked="true" runat="server" />
                    </td>
                </tr>
            <tr>

                <td></td>
                <td style="padding: 30px 150px 50px 0;">
                    <asp:Button ID="btnUpdateQuantities" runat="server" Text="Update Quantities" Class="button" OnClick="UpdateQuantiesBtnClicked" />
                </td>
            </tr>
        </table>
    </div>
    <div>
         <h3 style="text-decoration: underline">Menus included for the above order :</h3>
        <br />
        <h3 class="noteDiv"> Approved menus to be added to the Grid and designed with telerik's grid control as per original design</h3>
    </div>
    <div class="spacerDiv"></div>
    <div style="text-align: center;">
        <asp:Button ID="btnOrderNow" runat="server" Text="Order Now" Class="button" OnClick="OrderNowBtnClicked" />
    </div>
</asp:Content>
