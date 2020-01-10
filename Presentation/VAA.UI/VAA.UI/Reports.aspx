<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="VAA.UI.Reports" %>

<%@ Register Assembly="Telerik.ReportViewer.WebForms, Version=9.1.15.624, Culture=neutral, PublicKeyToken=a9d7983dfcc261be" Namespace="Telerik.ReportViewer.WebForms" TagPrefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <script src="Scripts/MenuActive.js"></script>

    <style>
        
    </style>
</head>
<body>

    <div class="col-sm-4">
        <a href="Home.aspx" class="site-title">
            <%--<img src="Images/Virgin-Atlantic-Logo_small.png" />--%>
            <img src="Images/EMMA_logo_small.png" />
        </a>
        <hr />
    </div>
    <form id="frmReport" runat="server">
        <br />
        <a href="Home.aspx" class="login">BACK TO HOMEPAGE</a>
        <br />
        <br />
        <div>
            <%--<center> <h1 style="text-decoration: underline">Reports</h1></center>--%>
            <asp:Label runat="server" Text="Choose report:" Font-Bold="True"></asp:Label>
            <asp:DropDownList ID="ddlReportType" runat="server" OnSelectedIndexChanged="ddlReportType_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Text="--Please Select--" Value="0"></asp:ListItem>
                <asp:ListItem Text="Seat Capacity" Value="1"></asp:ListItem>
                <asp:ListItem Text="Region" Value="2"></asp:ListItem>
                <asp:ListItem Text="Locations" Value="3"></asp:ListItem>
                <asp:ListItem Text="Route Details" Value="4"></asp:ListItem>
                <asp:ListItem Text="Flight Schedules" Value="5"></asp:ListItem>
                <asp:ListItem Text="Approvers" Value="6"></asp:ListItem>
                <asp:ListItem Text="Chili Templates" Value="7"></asp:ListItem>
                <asp:ListItem Text="Notifications" Value="8"></asp:ListItem>
                <asp:ListItem Text="Menu Item Category" Value="9"></asp:ListItem>
                <asp:ListItem Text="Base Item Details" Value="10"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <br />
        <div style="align-content: center">
            <telerik:ReportViewer ID="ReportViewerEmma" runat="server" Height="688px" Width="70%"></telerik:ReportViewer>
        </div>
    </form>
</body>
</html>
