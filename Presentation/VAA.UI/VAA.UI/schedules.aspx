<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="schedules.aspx.cs" Inherits="VAA.UI.schedules" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="VaaScheduler">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="VaaScheduler" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
    </telerik:RadAjaxLoadingPanel>
    <telerik:RadScriptBlock runat="Server" ID="RadScriptBlock1">
        <script type="text/javascript">
            function rebind(schedule) {
                var ajax = $find('<%= RadAjaxManager1.ClientID%>');
                ajax.ajaxRequest("RebindScheduler" + schedule);
            }
        </script>
    </telerik:RadScriptBlock>
    <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1">
        <div class="demo-container">
            <div class="bgWrapper">
                <telerik:RadScheduler runat="server" ID="VaaScheduler" Skin="Default" Height="700px" OverflowBehavior="Auto" SelectedView="MonthView" StartInsertingInAdvancedForm="true" StartEditingInAdvancedForm="true" CustomAttributeNames="Completed" AppointmentStyleMode="Default"
                    DataKeyField="ID"
                    DataSubjectField="Subject"
                    DataStartField="Start"
                    DataEndField="End"
                    EnableDescriptionField="True"
                    DataDescriptionField="Description"
                    DataRecurrenceField="Remainder"
                    DataRecurrenceParentKeyField="RecurrenceParentID"
                    OnDataBound="VaaScheduler_DataBound"
                    OnAppointmentCreated="VaaScheduler_AppointmentCreated"
                    OnAppointmentDataBound="VaaScheduler_AppointmentDataBound"
                    OnAppointmentInsert="VaaScheduler_AppointmentInsert"
                    OnAppointmentDelete="VaaScheduler_AppointmentDelete"
                    OnAppointmentUpdate="VaaScheduler_AppointmentUpdate">
                    <WeekView UserSelectable="true" />
                    <DayView UserSelectable="true" />
                    <MultiDayView UserSelectable="false" />
                    <TimelineView UserSelectable="false" />
                    <MonthView UserSelectable="true" />
                    <AgendaView UserSelectable="true" />
                    <AdvancedForm Modal="true" Width="1000"></AdvancedForm>
                    <AppointmentTemplate>
                        <div class="appointmentHeader">
                          <b><%# Eval("Subject") %></b>
                        </div>
                        <div class="appointmentSubject" style="height: 60px;">
                            Assigned to: 
                                <%--<asp:Label ID="UserLabel" runat="server" Text='<%# Container.Appointment.Resources.GetResourceByType("User") == null ? "None" : Container.Appointment.Resources.GetResourceByType("User").Text %>'></asp:Label>--%>
                            </strong>
                           <asp:Label runat="server" ID="AssignedTo" EnableViewState="false"></asp:Label>
                            <br />
                            <asp:CheckBox ID="CompletedStatusCheckBox" runat="server" Text="Completed? " TextAlign="Left"
                                Checked='<%# String.IsNullOrEmpty(Container.Appointment.Attributes["Completed"])? false : Boolean.Parse(Container.Appointment.Attributes["Completed"]) %>'
                                AutoPostBack="true" OnCheckedChanged="CompletedStatusCheckBox_CheckedChanged"></asp:CheckBox>
                        </div>
                    </AppointmentTemplate>
                    <ResourceTypes>
                        <telerik:ResourceType KeyField="UserID" Name="User" TextField="FirstName" ForeignKeyField="UserID"
                            DataSourceID="UsersDataSource" AllowMultipleValues="true"></telerik:ResourceType>
                        <telerik:ResourceType KeyField="ColorID" Name="Colors" TextField="ColorName" ForeignKeyField="ColorID"
                            DataSourceID="ColoursDataSource1"></telerik:ResourceType>
                    </ResourceTypes>
                    <ResourceStyles>
                        <telerik:ResourceStyleMapping Type="Colors" Key="1" BackColor="Crimson"></telerik:ResourceStyleMapping>
                        <telerik:ResourceStyleMapping Type="Colors" Key="2" BackColor="Plum"></telerik:ResourceStyleMapping>
                        <telerik:ResourceStyleMapping Type="Colors" Key="3" BackColor="Khaki "></telerik:ResourceStyleMapping>
                        <telerik:ResourceStyleMapping Type="Colors" Key="4" BackColor="LightSkyBlue "></telerik:ResourceStyleMapping>
                        <telerik:ResourceStyleMapping Type="Colors" Key="5" BackColor="LightSalmon"></telerik:ResourceStyleMapping>
                        <telerik:ResourceStyleMapping Type="Colors" Key="6" BackColor="LightPink"></telerik:ResourceStyleMapping>
                    </ResourceStyles>
                    <AppointmentContextMenuSettings EnableDefault="true"></AppointmentContextMenuSettings>
                </telerik:RadScheduler>
                <asp:SqlDataSource ID="UsersDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:VAAConnectionString %>"
                     SelectCommand="SELECT [UserID], [FirstName], [LastName] FROM [tUsers] WHERE ([UserType] = 'ESP') OR ([UserType] = 'Caterer') OR ([UserType] = 'Virgin') ORDER BY [FirstName] ASC">
                    <SelectParameters>
                        <asp:Parameter DefaultValue="ESP" Name="UserType" Type="String" />
                        <asp:Parameter DefaultValue="Virgin" Name="UserType2" Type="String" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:SqlDataSource ID="ColoursDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:VAAConnectionString %>"
                    SelectCommand="SELECT [ColorID], [ColorName], [ColorCode] FROM [tSchedulerColors]"></asp:SqlDataSource>
            </div>
        </div>
    </telerik:RadAjaxPanel>

</asp:Content>
