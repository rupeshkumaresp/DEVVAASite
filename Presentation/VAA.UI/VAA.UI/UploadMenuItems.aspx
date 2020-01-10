<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UploadMenuItems.aspx.cs" Inherits="VAA.UI.UploadMenuItems" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="updpnlUpload" runat="server">
        <ContentTemplate>
            <script src="Scripts/bootbox.js"></script>
            <script type="text/javascript">
                function UploadCompleted() {
                    bootbox.alert("Menu item upload completed successfully!");
                }

                function InValidUploadFile() {
                    bootbox.alert("Uploaded menu item file is empty, corrupt or not an Excel 2010 file (.xlsx), please verify the input!");
                }

            </script>
            <h3 style="text-decoration: underline">Upload Base Menu Items</h3>
            <br />
            <div>
                <table style="width: 650px;">
                    <tr>
                        <td style="text-align: right;">
                            <asp:Label ID="lblUpload" runat="server" Text="Base Item File :"></asp:Label>
                        </td>
                        <td>
                            <telerik:RadAsyncUpload ID="RadAsyncUploadBaseItem" runat="server" Skin="Silk"></telerik:RadAsyncUpload>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td style="padding: 30px 150px 50px 0;">
                            <asp:Button ID="btnUploadMenuItems" runat="server" Text="Upload" CssClass="button" OnClick="UploadMenuItemsBtnClicked" />
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
