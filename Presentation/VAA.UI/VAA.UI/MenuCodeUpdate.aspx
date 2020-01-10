﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MenuCodeUpdate.aspx.cs" Inherits="VAA.UI.MenuCodeUpdate" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:UpdatePanel ID="updpnlValidateUpdateMenuCode" runat="server">
        <ContentTemplate>
            <script src="Scripts/bootbox.js"></script>
            <script type="text/javascript">
                function RebuildCompleted() {
                    bootbox.alert("Menu code refresh is in progress and it will finish shortly. You can continue using site while the refresh is done!");
                }


            </script>
            <h3 style="text-decoration: underline">Menu Code Refresh</h3>
            <br />
            <div>
                <table style="width: 650px;">                   
                    <tr>
                        <td style="text-align: right; vertical-align: top">
                            <asp:Label ID="lblDate" runat="server" Text="Menu Re-Order Date"></asp:Label>
                        </td>
                        <td style="text-align: left">
                            <div>
                                <asp:TextBox ID="txtMenuReorderDate" runat="server" Style="height: 30px; width: 250px; overflow: scroll; resize: none; border: 1px solid #D8D8D8;"></asp:TextBox>
                            </div>
                            <div>
                                (DD/MM/YYYY)
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td style="padding-top: 40px; padding-bottom: 30px; padding-right: 150px;">
                            <asp:Button ID="btnRebuildMenu" runat="server" Text="Refresh Menu Code" Class="button" OnClick="btnRebuildMenu_Click" />
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
    <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="updpnlValidateUpdateMenuCode" runat="server" DisplayAfter="0">
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
