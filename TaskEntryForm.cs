using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;
using timetable_app.AppLogic;

namespace timetable_app
{
    public partial class TaskEntryForm : Form
    {
        Form1 sendingForm;
        Calendar calendar;
        User user;
        public TaskEntryForm(Form1 sendingForm, Calendar calendar, User user)
        {
            InitializeComponent();
            this.calendar = calendar;
            this.sendingForm = sendingForm;
            this.user = user;
            UpdateTaskSelector();
            maskedTextBox2.ValidatingType = typeof(double);
            maskedTextBox3.ValidatingType = typeof(int);
            maskedTextBox1.ValidatingType = typeof(double);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string taskName = textBox1.Text;
            string description = textBox2.Text;
            DateTime dateDue = new DateTime();
            dateDue = this.dateTimePicker1.Value.Date;
            double duration = Convert.ToDouble(maskedTextBox2.Text);
            int priority = Convert.ToInt32(maskedTextBox3.Text);
            int time = 0;

            AppLogic.Task task = new AppLogic.Task(taskName, description, DateTime.Now, false, time, duration, priority, dateDue);

            task.fixedTime = false;
            if(checkBox1.Checked == true)
            {
                task.fixedTime = true;
                task.scheduled = dateTimePicker2.Value;
                task.time = Convert.ToDouble(maskedTextBox1.Text);
                if(calendar.availableCheck(sendingForm, task) == false) // can't let them schedule it at an unavailable time
                {
                    task.fixedTime = false;
                    MessageBox.Show("That time is not available, " + task.name + " will be reschedueled");
                }
                if(task.scheduled.Date < task.due.Date && task.fixedTime == true)
                {
                    task.fixedTime = false;
                    MessageBox.Show("You can't schedule a task after it's due " + task.name + " will be reschedueled"); //made sure to include multiple error messages so they know what they did wrong
                }
                if (task.scheduled.Date < DateTime.Now.Date && task.fixedTime == true)
                {
                    task.fixedTime = false;
                    MessageBox.Show("You can't schedule a task in the past " + task.name + " will be reschedueled");
                }
            }
            
            foreach (object o in priorityTaskSelecter.CheckedItems)
            {
                foreach (AppLogic.Task t in calendar.GetTasks())
                {
                    if (t.taskDescription == o.ToString())
                    {
                        task.predecessors2.Add(t.ID);
                        t.successors2.Add(task.ID);
                        task.predecessors2.Add(t.ID);
                        t.successors2.Add(task.ID);
                    }

                }
            }

            //form.TaskList.Items.Add(task.taskDescription); // this is supposed to add it to the list box but won't work and I keep it as a reminder
            calendar.GetTasks().Add(task);

            if (task.successors2 != null && task.predecessors2 != null)
            {
                task.Ahead(calendar.GetTasks());
                task.Behind(calendar.GetTasks());
                task.CritPath(calendar.GetTasks());
            }


            //https://code-maze.com/sort-list-by-object-property-dotnet/
            //sendingForm.tasks = sendingForm.tasks.OrderBy(x => x.name).ThenBy(x=>x.priority)ToList();

            calendar.OrderTasks(sendingForm, user);
            calendar.UpdateTaskListControl(sendingForm);
            calendar.OrderDisplay(sendingForm, user);
            calendar.SaveTasksToFile();
            Close();
        }
        private void TaskEntryForm_KeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                string taskName = textBox1.Text;
                string description = textBox2.Text;
                DateTime dateDue = new DateTime();
                dateDue = dateTimePicker1.Value.Date;
                int duration = Convert.ToInt32(maskedTextBox2.Text);
                int priority = Convert.ToInt32(maskedTextBox3.Text);
                int time = 0;
                AppLogic.Task task = new AppLogic.Task(taskName, description, DateTime.Now, false, time, duration, priority, dateDue);
                calendar.GetTasks().Add(task);
                calendar.OrderTasks(sendingForm, user);
                calendar.UpdateTaskListControl(sendingForm);
                Close();
            }
            if (e.KeyData == Keys.Escape)
            {
                Close();
            }
        }

        public void UpdateTaskSelector()
        {
            priorityTaskSelecter.Items.Clear();
            foreach (AppLogic.Task t in calendar.GetTasks())
            {
                t.taskDescription = t.name + ", " + (t.time - (t.time % 1)) + ":" + (t.time % 1 * 60) + " - " + ((t.time + t.duration) - ((t.time + t.duration) % 1)) + ":" + ((t.time + t.duration) % 1 * 60) + ", " + t.scheduled.ToLongDateString();
                if (t.due.DayOfYear >= dateTimePicker1.Value.DayOfYear)
                {
                    priorityTaskSelecter.Items.Add(t.taskDescription);

                }
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }



        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void TaskEntryForm_Load(object sender, EventArgs e)
        {
            this.PreviewKeyDown += TaskEntryForm_KeyDown;
            this.UpdateTaskSelector();
            maskedTextBox1.Visible = false;
            maskedTextBox1.Enabled = false;
            dateTimePicker2.Value = DateTime.Now;
            dateTimePicker2.Visible = false;
            dateTimePicker2.Enabled = false;
            label6.Visible = false;
            label8.Visible = false;
            maskedTextBox2.ValidatingType = typeof(double);
            maskedTextBox3.ValidatingType = typeof(int);
            maskedTextBox1.ValidatingType = typeof(double);
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePicker2.Value = DateTime.Now;
            maskedTextBox1.Text = null;
            if (checkBox1.Checked == true)
            {
                maskedTextBox1.Visible = true;
                maskedTextBox1.Enabled = true;
                dateTimePicker2.Visible = true;
                dateTimePicker2.Enabled = true;
                label6.Visible = true;
                label8.Visible = true;
            }
            if(checkBox1.Checked == false)
            {
                maskedTextBox1.Visible = false;
                maskedTextBox1.Enabled = false;
                dateTimePicker2.Visible = false;
                dateTimePicker2.Enabled = false;
                label6.Visible = false;
                label8.Visible = false;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }

}
