<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPwd.aspx.cs" Inherits="VAA.UI.Account.ForgotPwd" %>
<link href="../Content/Site.css" rel="stylesheet" /> 
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>VAA - Forgot Password</title>    
</head>
<body>
    <form id="from1" runat="server">
        <div style="padding: 30px;">
            <a href="Login.aspx">
                <%--<img src="../Images/Virgin-Atlantic-Logo_small.png" />--%>
                <img src="../Images/EMMA_logo_small.png" />
            </a>
        </div>
        <div id="loginform" style="padding-top: 28px;">
            <h2 style="color:#fff;">Send my login details</h2>
            <div style="padding: 15px; color: #FFFFFF; text-align: left;">
                Email address:
            </div>
            <div>
                <asp:TextBox ID="txtEmail" runat="server" ToolTip="Email" Style="border: 1px solid #ccc; border-radius: 5px; color: #444; font-family: Calibri; font-size: 14px; height: 30px; padding: 2px 5px 2px 10px; width: 400px;"></asp:TextBox>
            </div>
            <div style="padding-left: 15px; text-align: left;">
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" Display="Dynamic" ControlToValidate="txtEmail" 
                    SetFocusOnError="true" ErrorMessage=">> Please enter your email." 
                    Style="color: #aa2029; font-weight: bold; font-size: 1.1em;"></asp:RequiredFieldValidator>
            </div>
            <br/><br/>
            <div>
                <asp:Button ID="btnForgotPAssword" runat="server" Text="SUBMIT" OnClick="BtnForgotPssswordClick"
                    Style="background-color: #aa2029; border: 1px outset #aa2029; border-radius: 5px; color: #fff; font-family: 'Century Gothic'; font-size: 16px; height: 40px; line-height: 0; padding: 0; width: 400px;" 
                    ToolTip="Login" />
            </div>
              <br/>
            <div>
              
            <asp:Label ID="lblPwdSentMessage" runat="server" Text="Email with your login details has been sent."  style="padding: 15px; color: #FFFFFF; font-weight: bold;" Visible="False"></asp:Label>
     
                <br/>
               <p style="font-family: Calibri; font-size: 12px; color: #fff; padding: 30px 30px 30px 10px;">
                    <a href="Login.aspx" style="font-family: 'Century Gothic'; font-size: 14px; text-decoration: underline; color: #fff;">BACK TO LOGIN</a>
                   
                </p>
            </div>
        </div>
    </form>
</body>
</html>
