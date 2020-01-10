<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MenuTranslation.aspx.cs" Inherits="VAA.UI.MenuTranslation" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="updpnlTranslateMenu" runat="server">
        <ContentTemplate>
            <script src="Scripts/bootbox.js"></script>
            <script type="text/javascript">
                function TranslateCompleted() {
                    bootbox.alert("Translated Menu PDF is uploaded successfully!");
                }

                function TranslationFileSaved() {
                    bootbox.alert("Translated Menu PDF is saved successfully!");
                }
                function TranslationFileSaveFailed() {
                    bootbox.alert("Translated Menu PDF failed to save, please try again!");
                }


            </script>
            <h3 style="text-decoration: underline">Upload Translated Menu PDF</h3>
            <br />
            <div>
                <table style="width: 650px;">
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblUpload" runat="server" Text="Upload Translation PDF:"></asp:Label>
                        </td>
                        <td>
                            <telerik:RadAsyncUpload ID="RadAsyncUploadTranslation" runat="server" Skin="Silk"></telerik:RadAsyncUpload>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblMenuCode" runat="server" Text="Menu Code:"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtMenucode" runat="server" Style="width: 230px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="padding: 30px 150px 50px 0;">
                            <asp:Button ID="btnUploadSchedule" runat="server" Text="Upload Translation PDF" CssClass="button" OnClick="btnUpload_Click" />
                        </td>
                    </tr>
                </table>
                <hr />
                <h3 style="text-decoration: underline">Get Menu Chili Dcoument ID</h3>
                <br />
                <table style="width: 650px;">
                    <tr>
                        <tr>
                            <td style="text-align: right;">
                                <asp:Label ID="lblMenuCodeForTemplate" runat="server" Text="Menu Code:"></asp:Label>
                            </td>
                            <td style="text-align: left;">
                                <asp:TextBox ID="txtMenuCodeForTempalte" runat="server" Style="width: 270px"></asp:TextBox>
                            </td>
                        </tr>

                        <td style="text-align: right;">
                            <asp:Label ID="lblChiliDoc" runat="server" Text="Chili Document ID:" Visible="false"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtChiliDocId" runat="server" Style="width: 270px" Visible="false"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <td></td>
                        <td style="padding: 30px 150px 50px 0;">
                            <asp:Button ID="btnGetChiliTemplate" runat="server" Text="Get Chili Template" CssClass="button" OnClick="btnGetChiliTemplate_Click" />
                        </td>
                    </tr>
                </table>
            </div>
            <style type="text/css">
                .ruFileWrap {
                    width: 235px !important;
                }

                .ruButton {
                    background: none !important; /*#e3e3e3 linear-gradient(#fefefe, #e3e3e3) repeat-x scroll 0 0;*/
                    border: 1px solid #AA2029 !important;
                    border-radius: 3px !important;
                    background-color: #AA2029 !important;
                    color: #FFFFFF !important;
                }
            </style>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="updpnlTranslateMenu" runat="server" DisplayAfter="0">
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
