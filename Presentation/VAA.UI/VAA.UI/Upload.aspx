<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Upload.aspx.cs" Inherits="VAA.UI.Upload" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="updpnlUpload" runat="server">
        <ContentTemplate>
            <script src="Scripts/bootbox.js"></script>
            <script type="text/javascript">
                function UploadCompleted() {
                    bootbox.alert("Menu upload completed successfully!");
                }
            </script>
            <h3 style="text-decoration: underline">Import New Menu</h3>
            <br />
            <div>
                <table style="width: 650px;">
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblCycleName" runat="server" Text="Cycle Name:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCycle" AutoPostBack="True" runat="server" Class="dropdown"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblClassName" runat="server" Text="Class Name :"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlClass" runat="server" Class="dropdown" AutoPostBack="True" OnSelectedIndexChanged="ddlClass_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <%-- <tr>
                    <td style="text-align: right;">
                        <asp:Label ID="lblMenyType" runat="server" Text="Menu Type :"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMenuType" AutoPostBack="True" runat="server" Class="dropdown"></asp:DropDownList>
                    </td>
                </tr>--%>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblUploadExcel" runat="server" Text="Upload Excel File :"></asp:Label>
                        </td>
                        <td>
                            <telerik:RadAsyncUpload ID="RadAsyncUploadMenu" Skin="Glow" runat="server" Style="margin-left: 46px"></telerik:RadAsyncUpload>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td style="padding-top: 40px; padding-bottom: 30px; padding-right: 150px;">
                            <asp:Button ID="btnUpload" runat="server" Text="Upload Menu" Class="button" OnClick="btnUpload_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="updpnlUpload" runat="server" DisplayAfter="0">
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
