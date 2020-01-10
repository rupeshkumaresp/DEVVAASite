<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GenerateAudit.aspx.cs" Inherits="VAA.UI.GenerateAudit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="updpnlAddEditUser" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDownloadAudit" />
        </Triggers>
        <ContentTemplate>
            <script src="Scripts/bootbox.js"></script>
            <script type="text/javascript">
                function AlertFileNotExist() {
                    bootbox.alert("Audit file is not found on server. Please ensure audit is generated earlier");
                }

                function AuditGenerationComplete() {
                    bootbox.alert("Audit generation is completed successfully.");
                }

                function AlertCheckPDFOnFTP() {
                    bootbox.alert("More than one menu PDF available for chosen criteria, Please refer to FTP for PDF access/download!");
                }

                function AlertNoPDFOnFTP() {
                    bootbox.alert("No PDF found on server, please make sure menu exists for this route and PDF is generated earlier!");
                }
                
                
            </script>
            <h3 style="text-decoration: underline">Generate Audit</h3>
            <p>(Generate the Audit and once complete, download it)</p>
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
                            <asp:Label ID="lblFlightFrom" runat="server" Text="Flight From :"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlFlightFrom" AutoPostBack="True" runat="server" Class="dropdown" OnSelectedIndexChanged="ddlFlightFrom_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblFlightTo" runat="server" Text="Flight To :"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlFlightTo" AutoPostBack="True" runat="server" Class="dropdown"></asp:DropDownList>
                        </td>
                    </tr>
                    <%--<tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblFinalPDF" runat="server" Text="Final PDF :"></asp:Label>
                        </td>
                        <td style="padding-top: 40px; padding-bottom: 30px;">
                            <asp:Button ID="btnGeneratePDF" runat="server" Text="Generate" Class="button" OnClick="GeneratePdfBtnClicked" Style="padding-right: 30px;" />
                            <asp:Button ID="btnDownloadPDF" runat="server" Text="Download" Class="button" OnClick="DownloadPdfBtnClicked" />
                        </td>
                    </tr>--%>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblAuditTrail" runat="server" Text="Audit Trial :"></asp:Label>
                        </td>
                        <td style="padding-top: 40px; padding-bottom: 30px;">
                            <asp:Button ID="btnGenerateAudit" runat="server" Text="Generate" Class="button" OnClick="GenerateAuditBtnClicked" Style="padding-right: 30px;" />
                            <asp:Button ID="btnDownloadAudit" runat="server" Text="Download" Class="button" OnClick="DownloadAuditBtnClicked" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="updpnlAddEditUser" runat="server" DisplayAfter="0">
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
