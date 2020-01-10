<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewAllVersions.aspx.cs" Inherits="VAA.UI.ViewAllVersions" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <a href="Home.aspx"><< Back</a>
    <telerik:RadSearchBox ID="RadSearchBox1" runat="server" EmptyMessage="Search Menu Code" CssClass="floatRight" Skin="Sunset">
    </telerik:RadSearchBox><p Class="floatRight noteDiv">Design this search box in telerik</p>
    <div class="spacerDiv"></div>
    <div style="padding-top: 20px;">
        <h3 class="noteDiv">Telerik Grid with list of all the menu versions for the selected cycle goes here..</h3>
    </div>
</asp:Content>
