<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AllergenSpecialMealUpload.aspx.cs" Inherits="VAA.UI.MenuCodeUpdate" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="updpnlValidateRebuildMenu" runat="server">
        <ContentTemplate>
            <script src="Scripts/bootbox.js"></script>
            <script type="text/javascript">

            </script>
            <h3 style="text-decoration: underline">Allergen & Special Meal PDF upload</h3>
            <br />
            <div>
                Please upload these PDFs directly login to server or use server FTP. PDF Path: H:\Emma\MENU PDFS

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
</asp:Content>
