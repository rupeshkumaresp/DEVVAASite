<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewApprover.aspx.cs" Inherits="VAA.UI.ViewApprover" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Approver Details</title>
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

        function IsPageValid() {
            if (Page_ClientValidate("ManageUser")) {
                alert(Page_ClientValidate("ManageUser"));
                return false;
            }
            else { return false; }
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
                    <h3 style="text-decoration: underline">
                        <asp:Label ID="lblUserActionType" runat="server"></asp:Label>
                    </h3>
                </div>
                <br />
                <div align="center">
                    <table style="text-align: left; border-collapse: separate; border-spacing: 8px;">                     
                        <tr>
                            <td align="center">
                                <table style="width: 300px!important;">
                                    <tr>
                                        <td style="padding-bottom:10px;">Name :</td>
                                        <td>                                            
                                            <asp:Label ID="lblName" runat="server"></asp:Label>                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-bottom:10px;">Designation:</td>
                                        <td>                                            
                                            <asp:Label ID ="lblDesignation" runat="server"></asp:Label>                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-bottom:10px;">Department:</td>
                                        <td>                                           
                                            <asp:Label ID="lblDepartment" runat="server"></asp:Label>                                           
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-bottom:10px;">Telephone :</td>
                                        <td>                                                                                      
                                             <asp:Label ID="lblTelephone" runat="server" ></asp:Label>                                          
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-bottom:10px;">Mobile :</td>
                                        <td>                                            
                                             <asp:Label ID="lblMobile" runat="server"></asp:Label>                                         
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Email:</td>
                                        <td>                                            
                                            <asp:LinkButton ID="LinkButton1" runat="server" OnClick="MailtoButton_clicked" Style="text-decoration:underline;">                                              
                                                 <asp:Label ID="lblUserName" runat="server" ></asp:Label>
                                            </asp:LinkButton>                                            
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>                    
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>

