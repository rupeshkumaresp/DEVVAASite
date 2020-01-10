<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="VAA.CrewMemberSite._Default" %>

<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent">
    <script>
        $(document).ready(function () {
            resizeDiv();
        });

        window.onresize = function (event) {
            resizeDiv();
        }

        function resizeDiv() {
            vpw = $(window).width();
            vph = $(window).height() - 230;
            $('#leftDivPanel').css({ 'height': vph + 'px' });
        }

        //Limit the Filter control options
        var column = null;
        function MenuShowing(sender, args) {
            if (column === null)
                return;
            var menu = sender; var items = menu.get_items();
            if (column.get_dataType() === "System.String") {
                var i = 0;
                while (i < items.get_count()) {
                    if (!(items.getItem(i).get_value() in { 'NoFilter': '', 'Contains': '', 'DoesNotContain': '', 'StartsWith': '', 'EndsWith': '' })) {
                        var item = items.getItem(i);
                        if (item !== null)
                            item.set_visible(false);
                    }
                    else {
                        var item = items.getItem(i);
                        if (item !== null)
                            item.set_visible(true);
                    } i++;
                }
            }
            if (column.get_dataType() === "System.Int64") {
                var j = 0; while (j < items.get_count()) {
                    if (!(items.getItem(j).get_value() in { 'NoFilter': '', 'Contains': '', 'DoesNotContain': '', 'StartsWith': '', 'EndsWith': '' })) {
                        var item = items.getItem(j); if (item !== null)
                            item.set_visible(false);
                    }
                    else { var item = items.getItem(j); if (item !== null) item.set_visible(true); } j++;
                }
            }
            column = null;
            menu.repaint();
        }
        function filterMenuShowing(sender, eventArgs) {
            column = eventArgs.get_column();
        }
    </script>


    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">

            function ShowEditBook(id, rowIndex) {

                var rwin = $find("<%= MenuViewerDialog.ClientID %>");
                    rwin.set_title("");
                    window.radopen("MenuViewer.aspx?ID=" + id, "MenuViewerDialog");

                    return false;
                }
                function ShowEditPDF(id, rowIndex) {

                    var rwin = $find("<%= PDFViewerDialog.ClientID %>");
                    rwin.set_title("");
                    window.radopen("PDFViewer.aspx?ID=" + id, "PDFViewerDialog");

                    return false;
                }


                $(window).resize(function () {
                    var grid = $find("<%= MenuViewerDialog.ClientID %>");
                    if (grid.isVisible()) {
                        setWindowsize();
                    }

                    var grid = $find("<%= PDFViewerDialog.ClientID %>");
                    if (grid.isVisible()) {
                        setWindowsizePDF();
                    }
                });

                function setWindowsize() {
                    var grid = $find("<%= MenuViewerDialog.ClientID %>");
                    var viewportWidth = $(window).width();
                    var viewportHeight = $(window).height();
                    grid.setSize(Math.ceil(viewportWidth * 95 / 100), Math.ceil(viewportHeight * 95 / 100));
                    grid.center();
                }
                function setWindowsizePDF() {
                    var grid = $find("<%= PDFViewerDialog.ClientID %>");
                    var viewportWidth = $(window).width();
                    var viewportHeight = $(window).height();
                    grid.setSize(Math.ceil(viewportWidth * 95 / 100), Math.ceil(viewportHeight * 95 / 100));
                    grid.center();
                }

        </script>
    </telerik:RadCodeBlock>
    <asp:UpdatePanel ID="updpnlShowMenu" runat="server">
        <ContentTemplate>

            <div id="leftDivPanel">
                <div id="cycleDiv" runat="server" style="padding-bottom: 10px;">
                    <asp:Label ID="lblCycle" runat="server" Text="Cycle :"></asp:Label><br />
                    <asp:DropDownList ID="ddlCycle" runat="server" AutoPostBack="True" Class="dropdown"></asp:DropDownList>
                </div>

                <div style="padding-bottom: 10px;">
                    <asp:Label ID="lblClass" runat="server" Text="Class :"></asp:Label><br />
                    <asp:DropDownList ID="ddlClass" runat="server" AutoPostBack="True" Class="dropdown" OnSelectedIndexChanged="ddlClass_SelectedIndexChanged"></asp:DropDownList>
                </div>

                <div style="padding-bottom: 10px;">
                    <asp:Label ID="lblMenutype" runat="server" Text="Menutype :"></asp:Label><br />
                    <asp:DropDownList ID="ddlMenutype" runat="server" AutoPostBack="True" Class="dropdown"></asp:DropDownList>
                </div>

                <div style="padding-bottom: 10px;">
                    <asp:Label ID="lblDeparture" runat="server" Text="Departure :"></asp:Label><br />
                    <asp:DropDownList ID="ddlDeparture" runat="server" AutoPostBack="True" Class="dropdown" OnSelectedIndexChanged="ddlDeparture_SelectedIndexChanged"></asp:DropDownList>
                </div>
                <div style="padding-bottom: 10px;">
                    <asp:Label ID="lblArrival" runat="server" Text="Arrival :"></asp:Label><br />
                    <asp:DropDownList ID="ddlArrival" runat="server" AutoPostBack="True" Class="dropdown" OnSelectedIndexChanged="ddlArrival_SelectedIndexChanged"></asp:DropDownList>
                </div>
                <div style="padding-bottom: 10px;">
                    <asp:Label ID="lblFlight" runat="server" Text="Flight No :"></asp:Label><br />
                    <asp:DropDownList ID="ddlFlight" runat="server" AutoPostBack="True" Class="dropdown" OnSelectedIndexChanged="ddlFlightNo_SelectedIndexChanged"></asp:DropDownList>
                </div>
                <div style="padding: 10px 0px 10px 5px;">
                    <asp:Button ID="btnGetMenuList" runat="server" CssClass="button" OnClick="btnGetMenuList_Clicked" Text="View Menu" />
                </div>
                <triggers>
                <asp:AsyncPostBackTrigger ControlID="ddlClass" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlDeparture" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlArrival" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="ddlFlight" EventName="SelectedIndexChanged" />
                <asp:AsyncPostBackTrigger ControlID="btnGetMenuList" EventName="Click" />
            </triggers>

            </div>
            <div id="rightDivPanel">
                <telerik:RadAjaxManager ID="radAjaxManager" runat="server">
                    <AjaxSettings>
                        <telerik:AjaxSetting>
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="radGridMenu" LoadingPanelID="gridLoadingPanel"></telerik:AjaxUpdatedControl>
                                <telerik:AjaxUpdatedControl ControlID="MenuViewerDialog"></telerik:AjaxUpdatedControl>
                                <telerik:AjaxUpdatedControl ControlID="PDFViewerDialog"></telerik:AjaxUpdatedControl>
                            </UpdatedControls>
                        </telerik:AjaxSetting>

                    </AjaxSettings>
                </telerik:RadAjaxManager>
                <telerik:RadWindowManager ID="RadWindowManagerMenuViewer" runat="server" EnableShadow="true">
                    <Windows>
                        <telerik:RadWindow ID="MenuViewerDialog" runat="server" Modal="true" Behaviors="Close" OnClientShow="setWindowsize" ReloadOnShow="true"
                            VisibleOnPageLoad="false" VisibleStatusbar="false" Title="" ShowContentDuringLoad="false">
                        </telerik:RadWindow>
                        <telerik:RadWindow ID="PDFViewerDialog" runat="server" Modal="true" Behaviors="Close" OnClientShow="setWindowsizePDF" ReloadOnShow="true"
                            VisibleOnPageLoad="false" VisibleStatusbar="false" Title="" ShowContentDuringLoad="false">
                        </telerik:RadWindow>
                        <%--OnClientShow="setWindowsize OnClientShow" OnClientPageLoad="OnClientPageLoad"--%>
                    </Windows>
                </telerik:RadWindowManager>

                <div id="MenuGridDiv" runat="server">
                    <telerik:RadGrid ID="radGridMenu" Visible="false" runat="server" Width="100%" Height="440px" AutoGenerateColumns="False" Skin="Default" ShowHeader="true" PageSize="10" PagerStyle-AlwaysVisible="true" HeaderStyle-VerticalAlign="Middle" HeaderStyle-HorizontalAlign="Center" PagerStyle-BorderStyle="None" AllowSorting="true" AllowPaging="true" AllowMultiRowSelection="false" AllowMultiRowEdit="false" GridLines="Both" AllowFilteringByColumn="true" EnableViewState="true" ViewStateMode="Enabled" HeaderStyle-BackColor="#69557e" HeaderStyle-ForeColor="#FFFFFF" GroupingSettings-IgnorePagingForGroupAggregates="False" ItemStyle-BorderColor="#999999" FilterMenu-BackColor="#FF6600" OnPageSizeChanged="radGridMenu_PageSizeChange" OnNeedDataSource="radGridMenu_NeedDataSource" OnItemDataBound="radGridMenu_ItemDataBound" OnItemCommand="radGridMenu_ItemCommand" OnPreRender="radGridMenu_PreRender">
                        <PagerStyle Mode="NextPrevAndNumeric" BorderStyle="None" />
                        <GroupingSettings CaseSensitive="false" />
                        <ClientSettings AllowColumnsReorder="false" ReorderColumnsOnClient="false" AllowKeyboardNavigation="true" EnableRowHoverStyle="true">
                            <Selecting AllowRowSelect="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                            <Resizing EnableRealTimeResize="false" AllowColumnResize="false" />
                            <ClientEvents OnFilterMenuShowing="filterMenuShowing" />
                        </ClientSettings>
                        <FilterMenu OnClientShowing="MenuShowing" />
                        <SelectedItemStyle CssClass="SelectedItem"></SelectedItemStyle>
                        <MasterTableView TableLayout="Fixed" AutoGenerateColumns="false" CommandItemDisplay="Top" CssClass="rdheaderCss" FilterItemStyle-HorizontalAlign="Center">
                            <CommandItemSettings />
                            <SortExpressions>
                                <telerik:GridSortExpression FieldName="Id" SortOrder="Descending" />
                            </SortExpressions>
                            <CommandItemTemplate>
                                <asp:LinkButton ID="lnkbtnClearAllFilter" runat="server" CommandName="ClearFilter" Text="Clear all filter" TabIndex="13" CssClass="FilterGridCommand"></asp:LinkButton>
                                <asp:LinkButton ID="lnkbtnRefreshGrid" runat="server" CommandName="Refresh" Text="Refresh" TabIndex="14" CssClass="FilterGridCommand marginLeft"></asp:LinkButton>
                            </CommandItemTemplate>
                            <Columns>
                                <telerik:GridBoundColumn HeaderText="Menu Id" DataType="System.Int64" UniqueName="Id" DataField="Id" Display="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn HeaderText="FlightNo" DataType="System.String" UniqueName="FlightNo" DataField="FlightNo" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FilterControlWidth="70%" HeaderStyle-Width="20%"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn HeaderText="CycleName" DataType="System.String" UniqueName="CycleName" DataField="CycleName" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FilterControlWidth="70%" HeaderStyle-Width="20%"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn HeaderText="Class" DataType="System.String" UniqueName="ClassName" DataField="ClassName" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FilterControlWidth="70%" HeaderStyle-Width="20%"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn HeaderText="Menu Type" DataType="System.String" UniqueName="MenuTypeName" DataField="MenuTypeName" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FilterControlWidth="70%" HeaderStyle-Width="20%"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn HeaderText="Route" DataType="System.String" UniqueName="Route" DataField="Route" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FilterControlWidth="70%" HeaderStyle-Width="10%"></telerik:GridBoundColumn>
                                <telerik:GridBoundColumn HeaderText="Menu Code" DataType="System.String" UniqueName="MenuCode" DataField="MenuCode" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" FilterControlWidth="70%" HeaderStyle-Width="10%"></telerik:GridBoundColumn>
                                <telerik:GridTemplateColumn UniqueName="TemplateEditColumn" HeaderText="View" AllowFiltering="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%">

                                    <ItemTemplate>
                                        <asp:ImageButton ID="ViewPDFLink" runat="server" CommandName="ViewPDF" ImageUrl="~/images/pdf_icon.png" Style="display: inline-block" ToolTip="PDFview" Class="pdfbutton" />&nbsp;&nbsp;
                                    <asp:ImageButton ID="ViewBookLink" runat="server" CommandName="ViewBook" ImageUrl="~/images/book_icon.png" Style="display: inline-block" ToolTip="BookView" Class="bookbutton" />
                                        <%--<asp:LinkButton ID="ViewMenuLink" runat="server" CommandName="ViewMenu" Class="pdfbutton" Text="">sgsw</asp:LinkButton>                              
                                        <telerik:RadButton ID="ViewPDFLink" runat="server" CommandName="ViewPDF"  Class="pdfbutton"></telerik:RadButton>
                                        <telerik:RadButton ID="ViewBookLink" runat="server" CommandName="ViewBook" Class="bookbutton"></telerik:RadButton>--%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="false" EnablePostBackOnRowClick="false">
                            <Selecting AllowRowSelect="false" EnableDragToSelectRows="false" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="100%" />
                        </ClientSettings>
                    </telerik:RadGrid>
                    <div id="spacer" class="clear-fix"></div>
                </div>


            </div>
        </ContentTemplate>

    </asp:UpdatePanel>
    <asp:UpdateProgress ID="updProgressShowMenu" AssociatedUpdatePanelID="updpnlShowMenu" runat="server" DisplayAfter="0">
        <ProgressTemplate>
            <div align="center" class="updateprogressdivHome">
            </div>
            <div id="divmiddle" align="center" class="updateprogressdiv2Home">
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
</asp:Content>
