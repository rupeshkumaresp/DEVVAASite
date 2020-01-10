<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuViewer.aspx.cs" Inherits="VAA.UI.MenuViewer" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../Content/Site.css" rel="stylesheet" />
    <script src="Scripts/jquery-2.1.4.min.js"></script>
    <script src="Scripts/QueryData.js"></script>
    <script type="text/javascript">

        //$(document).ready(function () {


        //        $("#btnupdate").click(function () {
        //            var getData1 = new QueryData();
        //            var qid = getData1.ID;
        //            var approvalid = $('#ddlChangeStatus').val();
        //            var selText = $('#ddlChangeStatus :selected').text();
        //            var datatosend = { "ApprovalStatusId": approvalid, "qid": qid };
        //            var curur = window.location.href;
        //            curur = curur.substring(0, curur.indexOf("?"));
        //            var currentURL = curur + "/AjaxUpdateStatus";
        //            $.ajax({
        //                type: "POST",
        //                url: currentURL,
        //                data: JSON.stringify(datatosend),
        //                contentType: "application/json; charset=utf-8",
        //                dataType: "json",
        //                beforeSend: function () { showLoaderManual(); },
        //                complete: function () { hideLoaderManual(); },
        //                success: function (msg) {
        //                    if (msg.d == true) {
        //                        console.log(selText);
        //                        $("#lblCurrApprovalStatus").text(selText);
        //                        if ($('#ddlChangeStatus :selected').text() == "Approved") {
        //                            $("#changestatusDiv").css("display", "none");
        //                        }
        //                    }
        //                },
        //                error: function (e) {
        //                }
        //            });
        //        });
        //    });


            function ShowNotificationForm(id) {
                window.radopen("SendNotification.aspx", "NotificationDialog");
                return false;
            }

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

        <telerik:RadWindowManager ID="radWindowManager" runat="server" EnableShadow="true">
            <Windows>
                <telerik:RadWindow ID="NotificationDialog" runat="server" AutoSize="false" Behaviors="Close" VisibleStatusbar="false" Title="Send Notification" Height="500px" Width="600px" ReloadOnShow="true" ShowContentDuringLoad="false" Modal="true">
                </telerik:RadWindow>
            </Windows>
        </telerik:RadWindowManager>

        <asp:UpdatePanel ID="updpnlEditMenu" runat="server">
            <ContentTemplate>
                <div class="container-fluid">
                    <div align="center" class="row">
                        <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" Skin="Vista" DecoratedControls="All" />
                        <br />

                        <table style="float: left; padding: 0; border: none; width: 100%;">
                            <tr>
                                <td style="width: 20%">
                                    <table>
                                        <tr style="vertical-align: bottom;">
                                            <td style="vertical-align: bottom;">
                                                <div>
                                                    <asp:Label ID="lblCurrentStatus" runat="server" Text="Current Status" Font-Bold="True" Font-Underline="True" Font-Size="17px"></asp:Label>
                                                    <br />
                                                    <asp:Label ID="lblCurrApprovalStatus" runat="server" ClientIDMode="Static"></asp:Label>
                                                </div>

                                            </td>
                                        </tr>

                                    </table>
                                </td>

                                <td style="width: 40%; border-left-color: red; border-left-style: solid;">
                                    <div id="changestatusDiv" visible="false" runat="server">
                                        <table style="padding: 0; border: none; width: 580px;">
                                            <tr>
                                                <td style="padding: 0">
                                                    <asp:Label ID="lblStatus" runat="server" Text="Change Status:"></asp:Label>
                                                </td>
                                                <td style="padding: 0">
                                                    <asp:DropDownList ID="ddlChangeStatus" runat="server" Class="dropdown_smallest" ClientIDMode="Static"></asp:DropDownList>
                                                </td>
                                                <td style="padding: 0">
                                                    <%-- <input type="button" value="Update" class="button_small" id="btnupdate" />--%>
                                                    <asp:Button ID="btnUpdateApprovalStatus" runat="server" OnClick="btnUpdateApprovalStatus_Click" Text="Update Status" Width="200px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <font color="red">  <b>  ( Once Approved the document cannot be changed, also Notification cannot be sent.) </br>
                                                     (To allow changes in Approved menu, Please request ESP to change the menu status.)
                                                     <br />
                                                                     </b> </font>

                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                                <td style="float: right">
                                    <input type="button" value="Send Notification" runat="server" class="button_small" id="btnSendChangeNotification"  onclick="return ShowNotificationForm();"/>
                                </td>
                            </tr>
                        </table>



                    </div>
                    <br />
                    <div align="center" style="clear: both;" class="row">
                        <hr />
                        <br />
                        <asp:Button ID="btnFlash" Text="Show Flash Editor" Visible="False" runat="server" OnClick="btnFlashEditor_Click" />
                        <asp:Button ID="btnHtml5" Text="Show HTML5 Editor" Visible="False" runat="server" OnClick="btnHTML5Editor_Click" />
                        <iframe id="iframeChiliProof" height="700px" width="90%" runat="server"></iframe>
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
