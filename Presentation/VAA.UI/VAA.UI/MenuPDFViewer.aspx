<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuPDFViewer.aspx.cs" Inherits="VAA.UI.MenuPDFViewer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Content/Site.css" rel="stylesheet" />
    <script src="Scripts/jquery-2.1.4.min.js"></script>
    <script src="Scripts/QueryData.js"></script>
    <script type="text/javascript">


        function showLoaderManual() {
            document.getElementById("divloader1").style.display = '';
            document.getElementById("divmiddle").style.display = '';
        }
        function hideLoaderManual() {
            document.getElementById("divloader1").style.display = 'none';
            document.getElementById("divmiddle").style.display = 'none';
        }

        function CloseAndRebind(args) {
            document.title = "";
            GetRadWindow().BrowserWindow.refreshGrid(args);
            GetRadWindow().close();
        }

        function GetRadWindow() {
            document.title = "";
            var oWindow = null;
            if (window.radWindow) oWindow = window.radWindow;
            else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
            return oWindow;
        }

        function CancelEdit() {
            document.title = "";
            GetRadWindow().close();
        }

    </script>
    <style>
        .riTextBox {
            width: 372px !important;
        }
    </style>
</head>
<body>
    <form id="frmMenuViewer" runat="server">
        <telerik:RadScriptManager ID="rdmgr" runat="server"></telerik:RadScriptManager>
        
        <asp:UpdatePanel ID="updpnlEditMenu" runat="server">
            <ContentTemplate>
                <div class="container-fluid">
                    <div align="center" class="row">
                        <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" Skin="Vista" DecoratedControls="All" />
                    </div>
                    <div align="center" style="clear: both;" class="row">
                        <iframe id="iframePDF" name ="iframePDF" height="600px" width="90%" runat="server"></iframe>
                    </div>


                    <div id="divloader1" align="center" class="updateprogressdiv1" style="display: none;"></div>
                    <div id="divmiddle" align="center" class="updateprogressdiv2" style="display: none;">
                        <div align="center" id="divdeepInner" class="deepInnerdiv">
                            <div align="center" style="border: none; background-color: #FFFFFF; border: 1px solid #000000; -webkit-border-radius: 10px; -moz-border-radius: 10px; padding-bottom: 10px;">
                                <img src="../Images/gear.gif" alt="Loading" height="100" width="100" style="border: none;" />
                                <br />
                                <span style="font-family: sans-serif; font-size: 14px; font-weight: 700; color: #BC1527;">Please wait...</span>
                            </div>
                        </div>
                    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="updpnlEditMenu" runat="server" DisplayAfter="0">
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
    </form>
</body>
</html>
