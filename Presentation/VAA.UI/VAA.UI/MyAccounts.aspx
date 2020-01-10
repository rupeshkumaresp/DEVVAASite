<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyAccounts.aspx.cs" Inherits="VAA.UI.MyAccounts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3 style="text-decoration: underline">Your Details :</h3>
    <br />
    <hr />
    <div class="row">
        <div class="col-md-8">
            <table style="text-align: left;">
                <tr>
                    <td style="text-align: left; width: 150px;">
                        <asp:Label ID="lblUserIdTitle" runat="server" Visible="false" Text="UserID:"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:Label ID="lblUserID" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left;">
                        <asp:Label ID="lblUserNameTitle" runat="server" Text="User Name :"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:Label ID="lblUserName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left;">
                        <asp:Label ID="lblPasswordTitle" runat="server" Text="Password :"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:Label ID="lblPassword" runat="server" Text="************"></asp:Label>
                        <asp:LinkButton ID="btnChangePassword" runat="server" OnClick="ChangePasswordClicked" Text="Change Password"> </asp:LinkButton>
                    </td>
                </tr>
                <asp:Panel ID="plnChangePassword" runat="server" Visible="False">
                    <tr>
                        <td style="text-align: left;">
                            <asp:Label ID="lblNewPassword" runat="server" Text="New Password :"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtBoxNewPassword" TextMode="Password" runat="server" CssClass="textBox"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server" Display="Dynamic" ControlToValidate="txtBoxNewPassword"
                                SetFocusOnError="true" ErrorMessage="* Required"
                                CssClass="validationErrorMsg"></asp:RequiredFieldValidator>

                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: left;">
                            <asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm Password :"></asp:Label>
                        </td>
                        <td style="text-align: left;">
                            <asp:TextBox ID="txtBoxConfirmPassword" TextMode="Password" runat="server" CssClass="textBox"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" Display="Dynamic" ControlToValidate="txtBoxConfirmPassword" SetFocusOnError="true" ErrorMessage="* Required" CssClass="validationErrorMsg"></asp:RequiredFieldValidator>
                            <asp:CompareValidator runat="server" ID="pwdComparer" ControlToValidate="txtBoxNewPassword" ControlToCompare="txtBoxConfirmPassword" Operator="Equal" Type="String" ErrorMessage="* Mismatch" CssClass="validationErrorMsg" />
                        </td>
                    </tr>
                </asp:Panel>
                <tr>
                    <td style="text-align: left;">
                        <asp:Label ID="lblFirstName" runat="server" Text="First Name :"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="textBox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left;">
                        <asp:Label ID="lblLastName" runat="server" Text="Last Name :"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtLastName" runat="server" CssClass="textBox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left;">
                        <asp:Label ID="lblDepartment" runat="server" Text="Department :"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtDepartment" runat="server" CssClass="textBox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left;">
                        <asp:Label ID="lblDesignation" runat="server" Text="Designation :"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtDesignation" runat="server" CssClass="textBox"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td style="text-align: left; vertical-align: top;">
                        <asp:Label ID="lblAddress" runat="server" Text="Address :"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtAddress1" runat="server" CssClass="textBox"></asp:TextBox><br />
                        <asp:TextBox ID="txtAddress2" runat="server" CssClass="textBox"></asp:TextBox><br />
                        <asp:TextBox ID="txtAddress3" runat="server" CssClass="textBox"></asp:TextBox><br />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left;">
                        <asp:Label ID="lblCity" runat="server" Text="City"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtCity" runat="server" CssClass="textBox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left;">
                        <asp:Label ID="lblCounty" runat="server" Text="County"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtCounty" runat="server" CssClass="textBox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left;">
                        <asp:Label ID="lblCountry" runat="server" Text="Country"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtCountry" runat="server" CssClass="textBox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left;">
                        <asp:Label ID="lblPostcode" runat="server" Text="Postcode"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtPostcode" runat="server" CssClass="textBox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left;">
                        <asp:Label ID="lblTelephoneNumber" runat="server" Text="Telephone Number :"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtTelephoneNumber" runat="server" CssClass="textBox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left;">
                        <asp:Label ID="lblMobileNumber" runat="server" Text="Mobile Number :"></asp:Label>
                    </td>
                    <td style="text-align: left;">
                        <asp:TextBox ID="txtMobileNumber" runat="server" CssClass="textBox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left;"></td>
                    <td style="text-align: left; padding-top: 50px;">
                        <asp:Button ID="btnUpdateDetails" runat="server" Text="Update Details" Class="button" OnClick="UpdateBtnClicked" />
                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" Class="button" CausesValidation="false" OnClick="CancelBtnClicked" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="col-md-4">
            <div style="height: 208px; width: 183px; border: 2px solid #bd1325; padding: 2px;">
                <div id="profilePic" style="height: 200px; width: 175px;">
                    <telerik:RadBinaryImage ID="radBinaryUserImage" runat="server" AlternateText="User Image" ToolTip="User Image" ImageUrl="~/Images/Noavatar.png" Style="height: 200px!important; width: 175px!important;" ResizeMode="Fit"></telerik:RadBinaryImage>
                </div>
            </div>
            <br />
            <div align="left">
                <telerik:RadUpload ID="radUploadUserImage" runat="server" Width="475px" MaxFileInputsCount="1" AllowedFileExtensions="jpg, JPG, jpeg, JPEG, bmp, BMP, gif, GIF, png, PNG" ControlObjectsVisibility="None"></telerik:RadUpload>
                <asp:Button ID="btnUploadPicture" runat="server" Text="Upload Picture" Class="button" OnClick="UploadBtnClicked" Style="width: 235px!important;" />
            </div>
        </div>
    </div>
    <style type="text/css">
        .ruFileWrap {
            width: 235px !important;
        }

        .ruButton {
            background: none!important; /*#e3e3e3 linear-gradient(#fefefe, #e3e3e3) repeat-x scroll 0 0;*/
            border: 1px solid #AA2029!important;
            border-radius: 3px!important;
            background-color: #AA2029!important;
            color: #FFFFFF!important;
        }
    </style>
</asp:Content>
