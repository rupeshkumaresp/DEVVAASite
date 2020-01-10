<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="VAA.CrewMemberSite.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" class="full--size">
<head runat="server">
    <meta charset="UTF-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="Original Hover Effects with CSS3" />
    <meta name="keywords" content="css3, transitions, thumbnail, animation, hover, effect, description, caption" />
    <meta name="author" content="Alessio Atzeni for Codrops" />
    <link href="Content/Site.css" rel="stylesheet" />
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/1.11.3/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery.cycle2/20140415/jquery.cycle2.min.js"></script>
    <title></title>
</head>
<body class="full--size">
    <script>
        $(document).ready(function () {
            $("#background--slider > div:gt(0)").hide();

            setInterval(function () {
                $('#background--slider > div:first')
                  .fadeOut(1000)
                  .next()
                  .fadeIn(1000)
                  .end()
                  .appendTo('#background--slider');
            }, 5000);
        });
    </script>

    <div id="bgDiv" class="full--size">
        <div class="view view-eighth full--size">

            <div id="background--slider">
                <div>
                    <img src="/Images/Slider/slide_1.jpg" class="full--size" />
                </div>
                <div>
                    <img src="/Images/Slider/slide_2.jpg" class="full--size" />
                </div>
                <div>
                    <img src="/Images/Slider/slide_3.jpg" class="full--size" />
                </div>
                <div>
                    <img src="/Images/Slider/slide_4.jpg" class="full--size" />
                </div>
                <div>
                    <img src="/Images/Slider/slide_5.jpg" class="full--size" />
                </div>
                <div>
                    <img src="/Images/Slider/slide_6.jpg" class="full--size" />
                </div>
                <div>
                    <img src="/Images/Slider/slide_7.jpg" class="full--size" />
                </div>
                <div>
                    <img src="/Images/Slider/slide_8.jpg" class="full--size" />
                </div>
                <div>
                    <img src="/Images/Slider/slide_9.jpg" class="full--size" />
                </div>
            </div>
            <div id="maskId" class="mask full--size">
                <form id="loginform" runat="server" style="width: 400px; text-align: center;">                    
                    <div style="margin-top: 200px;">
                        <div>
                            <asp:TextBox ID="txtEmail" runat="server" ToolTip="Email" CssClass="formtxtboxbg" placeholder="Email Address"></asp:TextBox>
                        </div>
                        <div style="padding-left: 15px; text-align: left;">
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" Display="Dynamic" ControlToValidate="txtEmail" SetFocusOnError="true" ErrorMessage=" Please enter your email." CssClass="validation-error">
                            </asp:RequiredFieldValidator>
                        </div>
                        <div style="margin-top: 10px;">
                            <asp:TextBox ID="txtPassword" runat="server" ToolTip="Password" CssClass="formtxtboxbg" placeholder="Password"></asp:TextBox>
                        </div>
                        <div style="padding-left: 15px; text-align: left;">
                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" Display="Dynamic" ControlToValidate="txtPassword" SetFocusOnError="true" ErrorMessage=" Please enter your password." CssClass="validation-error">
                            </asp:RequiredFieldValidator>
                        </div>
                        <div style="padding-top: 5px; padding-left: 25px; color: #FFFFFF; text-align: left;">
                            Remember Me?
                            <asp:CheckBox ID="cbRememberMe" runat="server" Checked="True" ToolTip="Remember Me" Style="margin-left: 15px;" />
                        </div>
                        <div style="margin-top: 13px;">
                            <asp:Button ID="btnLogin" runat="server" Text="ENTER THE SITE" OnClick="btnLogin_Click" ToolTip="Login" CssClass="formbuttonbg" />
                        </div>
                    </div>
                    <asp:Panel ID="pnlErrorMessage" runat="server" CssClass="message error" Visible="false" Style="margin: 2px!important;">
                        <asp:Label ID="lblErrorMessage" runat="server" Text="The email and password that you entered don't match." CssClass="error"></asp:Label>
                    </asp:Panel>
                </form>
            </div>
        </div>
    </div>

</body>
</html>
