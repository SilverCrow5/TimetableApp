using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using timetable_app.AppLogic;

namespace timetable_app
{
    public partial class TaskEditForm : Form
    {
        Form1 mainForm;
        Calendar calendar;
        AppLogic.Task t;
        User user;
        public bool isFixed; // so it goes to defaul if they re schedule it to an unavailable time
        public TaskEditForm(Form1 mainForm, AppLogic.Task t, Calendar calendar, User user)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.calendar = calendar;
            this.t = t;
            this.user = user;

            maskedTextBox1.ValidatingType = typeof(double);
            maskedTextBox2.ValidatingType = typeof(double);
            maskedTextBox3.ValidatingType = typeof(int);

            textBox1.Text = t.name;
            dateTimePicker1.Value = t.scheduled;
            dateTimePicker2.Value = t.due;
            textBox2.Text = t.details;
            maskedTextBox1.Text = Convert.ToString(t.duration);
            //maskedTextBox2.Text = Convert.ToString(t.time - (t.time % 1)) + ":" + Convert.ToString(t.time % 1 * 60);
            maskedTextBox2.Text = Convert.ToString(t.time);
            maskedTextBox3.Text = Convert.ToString(t.priority);
            if (t.predecessors2 != null)
            {
                foreach (AppLogic.Task u in mainForm.GetCalendar().GetTasks())
                {
                    if (u != t)
                    {
                        checkedListBox1.Items.Add(u);
                    }
                    if (t.predecessors2.Contains(u.ID))
                    {
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(u), true);
                    }
                }
            }
            
        }

        private void TaskEditForm_Load(object sender, EventArgs e)
        {
            if(t.fixedTime == true)
            {
                checkBox1.Checked = true;
            }
            if(t.fixedTime == false)
            {
                checkBox1.Checked = false;
            }
            maskedTextBox1.ValidatingType = typeof(double);
            maskedTextBox2.ValidatingType = typeof(double);
            maskedTextBox3.ValidatingType = typeof(int);

            isFixed = t.fixedTime;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            
            double origionalTime = t.time; // so it can do availbility checks but still revert back to the other value
            double origionalDuration = t.duration;
            DateTime origionalDate = t.scheduled;

            t.name = textBox1.Text;
            t.due = dateTimePicker2.Value;
            t.details = textBox2.Text;

            if(checkBox1.Checked == true)
            {
                t.fixedTime = true;
                t.time = Convert.ToDouble(maskedTextBox2.Text);
                t.scheduled = dateTimePicker1.Value;
            }
            
            t.duration = Convert.ToDouble(maskedTextBox1.Text);
            t.priority = Convert.ToInt32(maskedTextBox3.Text);
            t.predecessors2.Clear();
            t.time = Convert.ToDouble(maskedTextBox2.Text);
            if(calendar.availableCheck(mainForm, t) == false) // in case the time is unavailable
            {
                t.time = origionalTime;
                t.duration = origionalDuration;
                t.scheduled = origionalDate;
                if(isFixed == false)
                {
                    t.fixedTime = false;
                }
                MessageBox.Show("the new date, time or duration you set caused a clash, they have been set back to the original, you should look at your timetable to see if something is already schedueled at that time"); // if only functions could return more than 1 value
            }
            foreach (AppLogic.Task v in mainForm.GetCalendar().GetTasks())
            {
                if (v.successors2.Contains(t.ID))
                {
                    v.successors2.Remove(t.ID);
                }
            }
            foreach (AppLogic.Task u in checkedListBox1.CheckedItems)
            {
                t.predecessors2.Add(u.ID);
                u.successors2.Add(t.ID);
            }
            
            t.end = t.time + t.duration;
            calendar.OrderTasks(mainForm, user);
            calendar.UpdateTaskListControl(mainForm);
            calendar.OrderDisplay(mainForm, user);
            calendar.SaveTasksToFile();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e) // so they can choose a time
        {
            if(checkBox1.Checked == true)
            {
                t.fixedTime = true;
            }
            if(checkBox1.Checked == false)
            {
                t.fixedTime = false;
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if (t.fixedTime == false)
            {
                dateTimePicker1.Value = t.scheduled;
            }
        }

        private void maskedTextBox2_TextChanged(object sender, EventArgs e)
        {
            if(t.fixedTime == false)
            {
                maskedTextBox2.Text = Convert.ToString(t.time);
            }
        }

        private void button3_Click(object sender, EventArgs e) // just a final confirmation in case tey cahneg their mind or do it by accident
        {
            var form = new YesNoForm(t, mainForm, calendar, user);
            form.te = this;
            form.ShowDialog();
        }
    }
}
