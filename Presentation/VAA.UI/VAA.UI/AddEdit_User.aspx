<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddEdit_User.aspx.cs" Inherits="VAA.UI.AddEdit_User" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>User</title>
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
                        <asp:Label ID="lblUserActionType" runat="server"></asp:Label></h3>
                </div>
                <br />
                <div align="center">
                    <table style="text-align: left; border-collapse: separate; border-spacing: 8px;">
                        <tr>
                            <td style="text-decoration: underline; font-weight: 600; width: 220px!important;">User Details</td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table style="width: 555px!important;">
                                    <tr>
                                        <td>User name:</td>
                                        <td>
                                            <div>
                                                <asp:TextBox ID="txtUserName" runat="server" CssClass="textBox"></asp:TextBox>
                                            </div>
                                            <asp:RequiredFieldValidator ID="rfvUserName" runat="server" ControlToValidate="txtUserName" Display="Dynamic" SetFocusOnError="true" ErrorMessage=">> Please enter user name." ValidationGroup="ManageUser" CssClass="validationErrorMsg"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Password:</td>
                                        <td>
                                            <div>
                                                <asp:TextBox ID="txtPassword" runat="server" CssClass="textBox"></asp:TextBox>
                                            </div>
                                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" Display="Dynamic" SetFocusOnError="true" ErrorMessage=">> Required" ValidationGroup="ManageUser" CssClass="validationErrorMsg"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Retype password:</td>
                                        <td>
                                            <div>
                                                <asp:TextBox ID="txtReTypePassword" runat="server" CssClass="textBox"></asp:TextBox>
                                            </div>
                                            <asp:RequiredFieldValidator ID="rfvReTypePassword" runat="server" ControlToValidate="txtReTypePassword" Display="Dynamic" SetFocusOnError="true" ErrorMessage=">> Required" ValidationGroup="ManageUser" CssClass="validationErrorMsg"></asp:RequiredFieldValidator>
                                            <asp:CompareValidator runat="server" ID="cvReTypePassword" ControlToValidate="txtPassword" ControlToCompare="txtReTypePassword" Operator="Equal" Type="String" ErrorMessage=">> Password Mismatch" Display="Dynamic" CssClass="validationErrorMsg" ValidationGroup="ManageUser" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>First name:</td>
                                        <td>
                                            <div>
                                                <asp:TextBox ID="txtFirstName" runat="server" CssClass="textBox"></asp:TextBox>
                                            </div>
                                            <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName" Display="Dynamic" SetFocusOnError="true" ErrorMessage=">> Please enter first name." ValidationGroup="ManageUser" CssClass="validationErrorMsg"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Last name:</td>
                                        <td>
                                            <div>
                                                <asp:TextBox ID="txtLastName" runat="server" CssClass="textBox"></asp:TextBox>
                                            </div>
                                            <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName" Display="Dynamic" SetFocusOnError="true" ErrorMessage=">> Please enter last name." ValidationGroup="ManageUser" CssClass="validationErrorMsg"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td>UserType:</td>
                                        <td>
                                            <div>
                                                <asp:DropDownList ID="ddlUsertype" runat="server" CssClass="dropdown"></asp:DropDownList>  
                                            </div>
                                            <asp:RequiredFieldValidator ID="rfvUserType" runat="server" ControlToValidate="ddlUsertype" Display="Dynamic" SetFocusOnError="true" ErrorMessage=">> Please enter UserType." ValidationGroup="ManageUser" CssClass="validationErrorMsg" InitialValue="NA"></asp:RequiredFieldValidator>                                          
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Department:</td>
                                        <td>
                                            <asp:TextBox ID="txtDepartment" runat="server" CssClass="textBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Designation:</td>
                                        <td>
                                            <asp:TextBox ID="txtDesignation" runat="server" CssClass="textBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Address :</td>
                                        <td>
                                            <asp:TextBox ID="txtAddress1" runat="server" CssClass="textBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <asp:TextBox ID="txtAddress2" runat="server" CssClass="textBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <asp:TextBox ID="txtAddress3" runat="server" CssClass="textBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>City:</td>
                                        <td>
                                            <asp:TextBox ID="txtCity" runat="server" CssClass="textBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Postcode:</td>
                                        <td>
                                            <asp:TextBox ID="txtPostcode" runat="server" CssClass="textBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>County:</td>
                                        <td>
                                            <asp:TextBox ID="txtCounty" runat="server" CssClass="textBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Country:</td>
                                        <td>
                                            <asp:TextBox ID="txtCountry" runat="server" CssClass="textBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Mobile:</td>
                                        <td>
                                            <div>
                                                <asp:TextBox ID="txtMobile" runat="server" CssClass="textBox"></asp:TextBox>
                                            </div>
                                            <asp:RequiredFieldValidator ID="rfvMobile" runat="server" ControlToValidate="txtMobile" Display="Dynamic" SetFocusOnError="true" ErrorMessage=">> Required" ValidationGroup="ManageUser" CssClass="validationErrorMsg"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Telephone:</td>
                                        <td>
                                            <asp:TextBox ID="txtTelephone" runat="server" CssClass="textBox"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-decoration: underline; font-weight: 600;">Permissions</td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table>
                                    <tr>
                                        <td>Is super:</td>
                                        <td>
                                            <asp:RadioButtonList RepeatLayout="Flow" RepeatDirection="Horizontal" ID="rblIsSuper" runat="server">
                                                <asp:ListItem Text="Yes" Value="YES"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="NO" Selected="True"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>Is blocked:</td>
                                        <td>
                                            <asp:RadioButtonList RepeatLayout="Flow" RepeatDirection="Horizontal" ID="rblIsBlocked" runat="server">
                                                <asp:ListItem Text="Yes" Value="YES"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="NO" Selected="True"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Can import:</td>
                                        <td>
                                            <asp:RadioButtonList RepeatLayout="Flow" RepeatDirection="Horizontal" ID="rblCanImport" runat="server">
                                                <asp:ListItem Text="Yes" Value="YES"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="NO" Selected="True"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>Can export:</td>
                                        <td>
                                            <asp:RadioButtonList RepeatLayout="Flow" RepeatDirection="Horizontal" ID="rblCanExport" runat="server">
                                                <asp:ListItem Text="Yes" Value="YES"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="NO" Selected="True"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <%--<tr>
                                        <td>Can create campaign:</td>
                                        <td>
                                            <asp:RadioButtonList RepeatLayout="Flow" RepeatDirection="Horizontal" ID="rblCanCreateCampaign" runat="server">
                                                <asp:ListItem Text="Yes" Value="YES"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="NO" Selected="True"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>Can delete campaign:</td>
                                        <td>
                                            <asp:RadioButtonList RepeatLayout="Flow" RepeatDirection="Horizontal" ID="rblCanDeleteCampaign" runat="server">
                                                <asp:ListItem Text="Yes" Value="YES"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="NO" Selected="True"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Can view pricing:</td>
                                        <td>
                                            <asp:RadioButtonList RepeatLayout="Flow" RepeatDirection="Horizontal" ID="rblCanViewPricing" runat="server">
                                                <asp:ListItem Text="Yes" Value="YES"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="NO" Selected="True"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td></td>
                                        <td></td>
                                    </tr>--%>
                                </table>
                            </td>
                        </tr>                        
                        <tr>
                            <td align="center" style="padding-bottom:40px;">
                                <asp:Button ID="btnAddNewUser" runat="server" Text="Add New User" CssClass="button" Visible="false" ValidationGroup="ManageUser" CausesValidation="true" OnClick="btnAddNewUser_Click" />
                                <asp:Button ID="btnUpdateUser" runat="server" Text="Update user" CssClass="button" Visible="false" ValidationGroup="ManageUser" CausesValidation="true" OnClick="btnUpdateUser_Click"  />
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
