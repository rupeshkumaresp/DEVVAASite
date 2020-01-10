<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Add_BaseItem.aspx.cs" Inherits="VAA.UI.Add_BaseItem" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Base Item</title>
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
            padding: 0px!important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="rdmgr" runat="server"></telerik:RadScriptManager>
        <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" Skin="Vista" DecoratedControls="All" />
        <br />
        <div align="center">
            <h3 style="text-decoration: underline">
                <asp:Label ID="lblBaseItemActionType" runat="server"></asp:Label>
            </h3>
        </div>
        <br />
        <div align="center">
            <table style="width: 536px!important; border-collapse: separate; border-spacing: 8px;">
                <tr>
                    <td style="width: 110px!important;">Item Title:</td>
                    <td>
                        <div>
                            <asp:TextBox ID="txtItemTitle" runat="server" CssClass="textBox"></asp:TextBox>
                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="rvItemTitle" runat="server" ControlToValidate="txtItemTitle" ErrorMessage="Please enter Title" Display="Dynamic" ValidationGroup="AddBaseItem" CssClass="validationErrorMsg" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>Category:</td>
                    <td>
                        <div>
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_TextChanged"></asp:DropDownList>
                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="rvCategory" runat="server" ControlToValidate="ddlCategory" InitialValue="0" ErrorMessage="Please select category" Display="Dynamic" ValidationGroup="AddBaseItem" CssClass="validationErrorMsg" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>Category Code:</td>
                    <td>
                        <div>
                            <asp:TextBox ID="txtCategoryCode" runat="server" CssClass="textBox" Enabled="false" ReadOnly="true"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>Item Code:</td>
                    <td>
                        <div>
                            <asp:TextBox ID="txtBaseItemCode" runat="server" CssClass="textBox"></asp:TextBox>
                        </div>
                        <div>
                            <asp:RequiredFieldValidator ID="rvBaseItemCode" runat="server" ControlToValidate="txtBaseItemCode" ErrorMessage="Please enter base item code" Display="Dynamic" ValidationGroup="AddBaseItem" CssClass="validationErrorMsg" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>Title Description:</td>
                    <td>
                        <asp:TextBox ID="txtTitleDesc" runat="server" CssClass="textBox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top">Description:</td>
                    <td>
                        <asp:TextBox ID="txtDesc" runat="server" TextMode="multiline" Style="height: 100px; width: 400px; border: 1px solid #534741; border-radius: 0px!important; overflow-y: scroll; overflow-x: hidden; resize: none"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Sub Description:</td>
                    <td>
                        <asp:TextBox ID="txtSubDesc" runat="server" CssClass="textBox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Attributes:</td>
                    <td>
                        <asp:TextBox ID="txtAttributes" runat="server" CssClass="textBox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="btnAddBaseItem" runat="server" Text="Add Base Item" CssClass="button" Visible="false" ValidationGroup="AddBaseItem" CausesValidation="true" OnClick="btnAddBaseItem_Click" />
                        <asp:Button ID="btnUpdateBaseItem" runat="server" Text="Update Base Item" CssClass="button" Visible="false" ValidationGroup="AddBaseItem" CausesValidation="true" OnClick="btnUpdateBaseItem_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
