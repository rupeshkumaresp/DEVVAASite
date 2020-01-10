<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendNotification.aspx.cs" Inherits="VAA.UI.SendNotification" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Notification</title>
    <link href="Content/Site.css" rel="stylesheet" />
    <script type="text/javascript">
        function CloseAndRebind(args) {
            GetRadWindow().BrowserWindow.RefreshListView(args);
            GetRadWindow().close();
        }

        function GetRadWindow() {
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function CancelEdit() {
            GetRadWindow().close();
        }


    </script>
    <style type="text/css">
        td {
            padding: 2px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="rdmgr" runat="server"></telerik:RadScriptManager>
        <asp:UpdatePanel ID="updpnlAddEditUser" runat="server">
            <ContentTemplate>
                <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" Skin="Vista" DecoratedControls="All" />
                <br />
                <div align="center">
                    <table style="text-align: left; border-collapse: separate; border-spacing: 8px;">
                        <tr>
                            <td style="text-decoration: underline; font-weight: 600; width: 220px!important;">Notification Details</td>
                        </tr>
                        <tr>
                            <td>Menu Code:</td>
                            <td>
                                <asp:TextBox ID="txtMenuCode" runat="server" CssClass="textBox" ReadOnly="true"></asp:TextBox>
                            </td>

                        </tr>
                        <tr>
                            <td>Message:</td>
                            <td>
                                <asp:TextBox TextMode="MultiLine" Rows="10" Columns="5" Width="400px" Height="100px" ID="txtMessage" runat="server"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right" style="padding-bottom: 10px;padding-right: 15px;">
                                <asp:Button ID="btnSend" runat="server" Text="Send Message" CssClass="button_small" CausesValidation="true" OnClick="btnSend_Click" />
                                <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="button_small" CausesValidation="true" OnClick="btnClear_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center" style="padding-bottom: 40px;">
                                <asp:Label ID="lblSendMailStatus" runat="server" Text="Notification Sent!" CausesValidation="true" Visible="False" Font-Bold="True" ForeColor="#00CC00" Font-Size="20px" />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
