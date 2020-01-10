<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="VAA.UI.Account.Login" %>

<link href="../Content/Site.css" rel="stylesheet" />
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>VAA - Forgot Password</title>
</head>
<body>
    <form id="fromForgotPassword" runat="server">

        <div style="padding: 30px;">
            <a href="Login.aspx">
                <img src="../Images/Virgin-Atlantic-Logo_small.png" />
            </a>
        </div>
        <div id="loginform" style="padding-top: 28px;">
            <div style="padding: 15px; color: #FFFFFF; text-align: left;">
                Email address:
            </div>
            <div>
                <asp:TextBox runat="server" ToolTip="Email" Style="border: 1px solid #ccc; border-radius: 5px; color: #444; font-family: Calibri; font-size: 14px; height: 30px; padding: 2px 5px 2px 10px; width: 400px;"></asp:TextBox>
            </div>
            <div style="padding-left: 15px; text-align: left;">
                <asp:RequiredFieldValidator runat="server" Display="Dynamic" ControlToValidate="txtEmail"
                    SetFocusOnError="true" ErrorMessage=">> Please enter your email."
                    Style="color: #be3e16; font-weight: bold; font-size: 1.1em;"></asp:RequiredFieldValidator>
            </div>
            <div>
                <asp:Button ID="btnForgotPassword" runat="server" Text="Forgot Password"
                    Style="background-color: #aa2029; border: 1px outset #aa2029; border-radius: 5px; color: #fff; font-family: 'Century Gothic'; font-size: 16px; height: 40px; line-height: 0; padding: 0; width: 400px;"
                    ToolTip="Forgot Password" />
            </div>
        </div>
    </form>
</body>
</html>
