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

namespace timetable_app
{
    public partial class TaskEntryForm : Form
    {
        Form1 sendingForm;
        Calendar calendar;
        public TaskEntryForm(Form1 sendingForm, Calendar calendar)
        {
            InitializeComponent();
            this.calendar = calendar;
            this.sendingForm = sendingForm;
            this.UpdateTaskSelector();
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

            Task task = new Task(taskName, description, DateTime.Now, false, time, duration, priority, dateDue);

            
            foreach (object o in priotiryTaskSelecter.CheckedItems)
            {
                foreach (Task t in calendar.GetTasks())
                {
                    if (t.taskDescription == o.ToString())
                    {
                        task.predecessors.Add(t);
                        t.successors.Add(task);
                        
                    }

                }
            }

            //form.TaskList.Items.Add(task.taskDescription); // this is supposed to add it to the list box but won't work and I keep it as a reminder
            calendar.GetTasks().Add(task);

            if (task.successors != null && task.predecessors != null)
            {
                task.Ahead(sendingForm.tasks);
                task.Behind(sendingForm.tasks);
                task.CritPath(sendingForm.tasks);
            }


            //https://code-maze.com/sort-list-by-object-property-dotnet/
            //sendingForm.tasks = sendingForm.tasks.OrderBy(x => x.name).ThenBy(x=>x.priority)ToList();

            calendar.OrderTasks();
            calendar.UpdateTaskListControl(sendingForm);
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
                Task task = new Task(taskName, description, DateTime.Now, false, time, duration, priority, dateDue);
                sendingForm.tasks.Add(task);
                calendar.OrderTasks();
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
            priotiryTaskSelecter.Items.Clear();
            foreach (Task t in calendar.GetTasks())
            {
                t.taskDescription = t.name + ", " + (t.time - (t.time % 1)) + ":" + (t.time % 1 * 60) + " - " + ((t.time + t.duration) - ((t.time + t.duration) % 1)) + ":" + ((t.time + t.duration) % 1 * 60) + ", " + t.scheduled.ToLongDateString();
                if (t.due.DayOfYear >= dateTimePicker1.Value.DayOfYear)
                {
                    priotiryTaskSelecter.Items.Add(t.taskDescription);

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
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

}
