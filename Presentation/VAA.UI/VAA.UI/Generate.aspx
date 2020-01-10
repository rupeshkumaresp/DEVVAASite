<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Generate.aspx.cs" Inherits="VAA.UI.Generate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


    <contenttemplate>
            <script src="Scripts/bootbox.js"></script>
            <script type="text/javascript">

                function AlertCheckPDFOnFTP() {
                    bootbox.alert("More than one menu PDF available for chosen criteria, Please refer to FTP for PDF access/download!");
                }

                function AlertNoPDFOnFTP() {
                    bootbox.alert("No PDF found on server, please make sure menu exists for this route and PDF is generated earlier!");
                }

                function PDFGenerationProcess() {
                    bootbox.alert("PDF generation process is in progress and you will be notified by email when PDF generation is complete for selected criteria");
                }


            </script>
            <h3 style="text-decoration: underline">Generate Final PDFs</h3>
            <p>(Generate the PDF and once complete, download it)</p>
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
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblFinalPDF" runat="server" Text="Final PDF :"></asp:Label>
                        </td>
                        <td style="padding-top: 40px; padding-bottom: 30px;">
                            <asp:Button ID="btnGeneratePDF" runat="server" Text="Generate" Class="button" OnClick="GeneratePdfBtnClicked" Style="padding-right: 30px;" />
                            <asp:Button ID="btnDownloadPDF" runat="server" Text="Download" Class="button" OnClick="DownloadPdfBtnClicked" />
                        </td>
                    </tr>
                    <%--<tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblAuditTrail" runat="server" Text="Audit Trial :"></asp:Label>
                        </td>
                        <td style="padding-top: 40px; padding-bottom: 30px;">
                            <asp:Button ID="btnGenerateAudit" runat="server" Text="Generate" Class="button" OnClick="GenerateAuditBtnClicked" Style="padding-right: 30px;" />
                            <asp:Button ID="btnDownloadAudit" runat="server" Text="Download" Class="button" OnClick="DownloadAuditBtnClicked" />
                        </td>
                    </tr>--%>
                </table>

                 <br />
                <b>(* Please note that Generate PDF may take long time depending upon selected criteria, you will be notified by email once PDF generation is complete!)</b>
            </div>
        </contenttemplate>
</asp:Content>
