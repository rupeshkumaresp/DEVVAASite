<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PrintStatusReport.aspx.cs" Inherits="VAA.UI.PrintStatusReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="updpnlPrintStatusReport" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDownloadAudit" />
        </Triggers>
        <ContentTemplate>
            <script src="Scripts/bootbox.js"></script>
            <script type="text/javascript">
               

            </script>
            <h3 style="text-decoration: underline">Print Status Report</h3>
            <br />
            <div>
                <table style="width: 650px;">
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblCycleName" runat="server" Text="Cycle Name:"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlCycle" runat="server" AutoPostBack="True" Class="dropdown"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblClass" runat="server" Text="Class Name :"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlClass" runat="server" AutoPostBack="True" Class="dropdown" OnSelectedIndexChanged="ddlClass_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblMenuType" runat="server" Text="Menu Type :"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlMenuType" AutoPostBack="True" runat="server" Class="dropdown"></asp:DropDownList>
                        </td>
                    </tr>

                    <tr>
                        <td style="text-align: right;">
                        </td>
                        <td style="padding-top: 40px; padding-bottom: 30px;">
                            <asp:Button ID="btnDownloadAudit" runat="server" Text="Download" Class="button" OnClick="GenerateAuditBtnClicked" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="updpnlPrintStatusReport" runat="server" DisplayAfter="0">
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
