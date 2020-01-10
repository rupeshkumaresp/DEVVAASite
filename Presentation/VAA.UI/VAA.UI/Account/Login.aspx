<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="VAA.UI.Account.Login" %>
<link href="../Content/Site.css" rel="stylesheet" /> 
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>EMMA - LOGIN</title>   
</head>
<body>
    <form id="from1" runat="server">
        <asp:Panel ID="pnlErrorMessage" runat="server" CssClass="message error" Visible ="false" style="margin:2px!important;">
            <asp:Label ID="lblErrorMessage" runat="server" Text="The email and password that you entered don't match." CssClass="error"></asp:Label>
        </asp:Panel>
        <div style="padding: 30px;">
            <a href="Login.aspx">
                <%--<img src="../Images/Virgin-Atlantic-Logo_small.png" />--%>
                <img src="../Images/EMMA_logo_small.png" />
            </a>
        </div>
        <div id="loginform" style="padding-top: 28px;">
            <div style="padding: 15px; color: #FFFFFF; text-align: left;">
                Email address:
            </div>
            <div>
                <asp:TextBox ID="txtEmail" runat="server" ToolTip="Email" ></asp:TextBox>
            </div>
            <div style="padding-left: 15px; text-align: left;">
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" Display="Dynamic" ControlToValidate="txtEmail" SetFocusOnError="true" ErrorMessage=">> Please enter your email." CssClass="validation-error">
                </asp:RequiredFieldValidator>
            </div>
            <div style="padding: 15px; color: #FFFFFF; text-align: left;">
                Password:
            </div>
            <div>
                <asp:TextBox ID="txtPassword" runat="server" ToolTip="Password"></asp:TextBox>
            </div>
            <div style="padding-left: 15px; text-align: left;">
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" Display="Dynamic" ControlToValidate="txtPassword" SetFocusOnError="true" ErrorMessage=">> Please enter your password." CssClass="validation-error">
                </asp:RequiredFieldValidator>
            </div>
            <div style="padding: 15px; color: #FFFFFF; text-align: left;">
                Remember Me?
                <asp:CheckBox ID="cbRememberMe" runat="server" Checked="True" ToolTip="Remember Me" Style="margin-left: 15px;" />
            </div>
            <div>
                <asp:Button ID="btnLogin" runat="server" Text="LOGIN" OnClick="btnLogin_Click" ToolTip="Login"  />
            </div>
            <div style="padding-top: 30px; padding-left:15px; text-align: left;">
                <p><a href="ForgotPwd.aspx" style="color: #fff;">Did you forget your password?</a></p>               
            </div>
        </div>
    </form>
</body>
</html>
