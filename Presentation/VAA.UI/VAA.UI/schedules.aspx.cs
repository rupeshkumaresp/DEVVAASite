using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using VAA.DataAccess;
using VAA.DataAccess.Model;
using Elmah;
using VAA.CommonComponents;

namespace VAA.UI
{
    /// <summary>
    /// Scheduler for managing the appointments, sessions, schedules
    /// </summary>
    public partial class schedules : System.Web.UI.Page
    {
        readonly AccountManagement _accountManagement = new AccountManagement();
        readonly ScheduleManagement _scheduleManagement = new ScheduleManagement();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindSchedules();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["USERID"])))
                Response.Redirect("~/Account/Login.aspx");

            VaaScheduler.SelectedDate = DateTime.Today;
            var userid = Convert.ToInt16(Session["USERID"]);

            var user = _accountManagement.GetUserById(userid);

            if (user.UserType != "ESP")
            {
                VaaScheduler.AllowEdit = false;
                VaaScheduler.AllowInsert = false;
                VaaScheduler.AllowDelete = false;
            }
            else
            {
                VaaScheduler.AllowEdit = true;
                VaaScheduler.AllowInsert = true;
                VaaScheduler.AllowDelete = true;
            }


        }

        private void BindSchedules()
        {
            try
            {
                var schedulerdata = _scheduleManagement.GetAllSchedules();
                VaaScheduler.DataSource = schedulerdata;
                VaaScheduler.DataBind();
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        protected void VaaScheduler_DataBound(object sender, EventArgs e)
        {
            VaaScheduler.ResourceTypes.FindByName("User").AllowMultipleValues = true;

        }

        //scheduler events
        public void VaaScheduler_AppointmentInsert(object sender, AppointmentInsertEventArgs e)
        {
            try
            {
                string assignedTo;
                if (e.Appointment.Subject == String.Empty)
                    e.Cancel = true;
                else
                {
                    List<string> users = new List<string>();

                    foreach (Resource user in e.Appointment.Resources.GetResourcesByType("User"))
                    {
                        users.Add(user.Key.ToString());
                    }
                    if (users.Count > 0)
                        assignedTo = string.Join(", ", users.ToArray());
                    else
                        assignedTo = users.ToString();

                    Resource color = e.Appointment.Resources.GetResourceByType("Colors");
                    Schedules schedules = new Schedules()
                    {
                        Subject = e.Appointment.Subject,
                        Start = e.Appointment.Start,
                        End = e.Appointment.End,
                        UserID = assignedTo,
                        RecurrenceRule = e.Appointment.RecurrenceRule,
                        RecurrenceParentID = Convert.ToInt16(e.Appointment.RecurrenceParentID),
                        Description = e.Appointment.Description,
                        Remainder = Convert.ToString(e.Appointment.Reminders),
                        Completed = false,
                        ColorID = Convert.ToInt16(color.Key)
                    };
                    var newScheduleID = _scheduleManagement.CreateNewSchedule(schedules);

                    //send email notification
                    if (newScheduleID != 0)
                    {
                        var schedule = _scheduleManagement.GetScheduleById(Convert.ToInt16(newScheduleID));

                        if (schedule != null)
                        {
                            var userIds = schedule.UserID;

                            if (userIds != null)
                            {
                                var usersArray = userIds.Split(new char[] { ',' });

                                for (int i = 0; i < usersArray.Length; i++)
                                {
                                    var user = _accountManagement.GetUserById(Convert.ToInt32(usersArray[i].Trim()));

                                    var email = user.Username;
                                    var firstname = user.FirstName;
                                    var subject = schedule.Subject;
                                    var description = schedule.Description;
                                    var startdatetime = Convert.ToString(schedule.Start);
                                    var enddatetime = Convert.ToString(schedule.End);

                                    EmailHelper.SendAppointmentNotification(email, firstname, subject, description, startdatetime, enddatetime);
                                }
                            }
                        }
                    }
                }
                BindSchedules();
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        public void VaaScheduler_AppointmentUpdate(object sender, AppointmentUpdateEventArgs e)
        {
            try
            {
                string assignedTo;

                List<string> users = new List<string>();

                foreach (Resource user in e.ModifiedAppointment.Resources.GetResourcesByType("User"))
                {
                    users.Add(user.Key.ToString());
                }
                if (users.Count > 0)
                    assignedTo = string.Join(", ", users.ToArray());
                else
                    assignedTo = users.ToString();

                var appointmentId = Convert.ToInt16(e.Appointment.ID);

                var appointmentdata = _scheduleManagement.GetScheduleById(appointmentId);
                if (appointmentdata != null)
                {
                    Resource color = e.ModifiedAppointment.Resources.GetResourceByType("Colors");
                    Schedules newSchedules = new Schedules()
                    {
                        ID = appointmentId,
                        Subject = e.ModifiedAppointment.Subject,
                        Start = e.ModifiedAppointment.Start,
                        End = e.ModifiedAppointment.End,
                        UserID = assignedTo,
                        RecurrenceRule = e.ModifiedAppointment.RecurrenceRule,
                        RecurrenceParentID = Convert.ToInt16(e.ModifiedAppointment.RecurrenceParentID),
                        Description = e.ModifiedAppointment.Description,
                        Remainder = Convert.ToString(e.ModifiedAppointment.Reminders),
                        Completed = false,
                        ColorID = Convert.ToInt16(color.Key)
                    };
                    var newScheduleID = _scheduleManagement.UpdateSchedules(newSchedules);
                    //send email notification
                    if (newScheduleID != 0)
                    {
                        var schedule = _scheduleManagement.GetScheduleById(Convert.ToInt16(newScheduleID));

                        if (schedule != null)
                        {
                            var userIds = schedule.UserID;

                            if (userIds != null)
                            {
                                var usersArray = userIds.Split(new char[] { ',' });

                                for (int i = 0; i < usersArray.Length; i++)
                                {
                                    var user = _accountManagement.GetUserById(Convert.ToInt32(usersArray[i].Trim()));

                                    var email = user.Username;
                                    var firstname = user.FirstName;
                                    var subject = schedule.Subject;
                                    var description = schedule.Description;
                                    var startdatetime = Convert.ToString(schedule.Start);
                                    var enddatetime = Convert.ToString(schedule.End);

                                    EmailHelper.SendUpdateAppointmentNotification(email, firstname, subject, description, startdatetime, enddatetime);
                                }
                            }
                        }
                    }

                }
                BindSchedules();
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        public void VaaScheduler_AppointmentDelete(object sender, AppointmentDeleteEventArgs e)
        {
            try
            {
                var appointmentId = Convert.ToInt16(e.Appointment.ID);

                if (appointmentId != null)
                {
                    bool isdeleted = _scheduleManagement.DeleteSchedule(appointmentId);
                }
                BindSchedules();
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        protected void VaaScheduler_AppointmentCreated(object sender, AppointmentCreatedEventArgs e)
        {
            try
            {
                List<string> users = new List<string>();
                foreach (Resource user in e.Appointment.Resources.GetResourcesByType("User"))
                {
                    users.Add(user.Key.ToString());
                }
                var schedule = _scheduleManagement.GetScheduleById(Convert.ToInt16(e.Appointment.ID));

                if (schedule != null)
                {
                    Label assignedTo = (Label)e.Container.FindControl("AssignedTo");

                    var userIds = schedule.UserID;

                    if (userIds != null)
                    {
                        var usersArray = userIds.Split(new char[] { ',' });

                        var firstnames = "";

                        for (int i = 0; i < usersArray.Length; i++)
                        {
                            var user = _accountManagement.GetUserById(Convert.ToInt32(usersArray[i].Trim()));

                            firstnames += user.FirstName + ",";
                        }

                        if (firstnames.Contains(","))
                            firstnames = firstnames.Substring(0, firstnames.Length - 1);

                        assignedTo.Text = firstnames;
                    }
                    else
                        assignedTo.Text = "NONE";
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }

        public void VaaScheduler_AppointmentDataBound(object sender, SchedulerEventArgs e)
        {
            try
            {
                var schedule = _scheduleManagement.GetScheduleById(Convert.ToInt16(e.Appointment.ID));

                if (schedule != null)
                {
                    var userIds = schedule.UserID;

                    if (userIds != null)
                    {
                        var usersArray = userIds.Split(new char[] { ',' });

                        for (int i = 0; i < usersArray.Length; i++)
                        {
                            if (i < ((RadScheduler)sender).Resources.GetResourcesByType("User").Count)
                            {
                                for (int j = 0; j < ((RadScheduler)sender).Resources.GetResourcesByType("User").Count; j++)
                                {
                                    if (((RadScheduler)sender).Resources.GetResourcesByType("User")[j].Key.ToString().Trim() == usersArray[i].Trim())
                                    {
                                        //add the array
                                        e.Appointment.Resources.Add(((RadScheduler)sender).Resources.GetResourcesByType("User")[j]);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                if (e.Appointment.Attributes["Completed"] == "True")
                {
                    e.Appointment.BackColor = System.Drawing.Color.Aquamarine;
                }
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
        public void VaaScheduler_RecurrenceExceptionCreated(object sender, RecurrenceExceptionCreatedEventArgs e)
        {
            Response.Write("An exception with subject: " + e.ExceptionAppointment.Subject + " was created");
        }
        protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
        {
            if (e.Argument == "RebindScheduler")
            {
                VaaScheduler.Rebind();
            }
        }
        protected void CompletedStatusCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox CompletedStatusCheckBox = (CheckBox)sender;
                //Find the appointment object to directly interact with it
                SchedulerAppointmentContainer appContainer = (SchedulerAppointmentContainer)CompletedStatusCheckBox.Parent;
                Appointment appointment = appContainer.Appointment;
                Appointment appointmentToUpdate = VaaScheduler.PrepareToEdit(appointment, VaaScheduler.EditingRecurringSeries);
                var appointmentId = Convert.ToInt16(appointmentToUpdate.ID);
                appointmentToUpdate.Attributes["Completed"] = CompletedStatusCheckBox.Checked.ToString();
                //VaaScheduler.(appointmentToUpdate);
                if (CompletedStatusCheckBox.Checked == true)
                {
                    Schedules newSchedules = new Schedules()
                        {
                            ID = appointmentId,
                            Completed = true
                        };
                    _scheduleManagement.UpdateStatus(newSchedules);

                }
                else
                {
                    Schedules newSchedules = new Schedules()
                    {
                        ID = appointmentId,
                        Completed = false
                    };
                    _scheduleManagement.UpdateStatus(newSchedules);
                }
                Response.Redirect("~/schedules.aspx");

                VaaScheduler.Rebind();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "mykey", "rebind();", true);
            }
            catch (Exception ex)
            {
                //write to Elma
                ErrorSignal.FromCurrentContext().Raise(ex);
            }
        }
    }
}