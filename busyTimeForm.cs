using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using timetable_app.AppLogic;

namespace timetable_app
{
    public partial class busyTimeForm : Form
    {
        Form1 sendingForm;
        Calendar calendar;
        User user;
        public busyTimeForm(Form1 sendingForm, Calendar calendar, User user)
        {
            InitializeComponent();
            this.calendar = calendar;
            this.sendingForm = sendingForm;
            this.user = user;

            if(textBox1.Text != "" && textBox2.Text != "")
            {
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox1.Text) + Convert.ToInt32(textBox2.Text));
            }
            if(textBox1.Text != "" && textBox3.Text != "" && textBox2.Text == "")
            {
                textBox2.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - Convert.ToInt32(textBox1.Text));
            }
            if(textBox2.Text != "" && textBox3.Text != "" && textBox1.Text == "")
            {
                textBox1.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - Convert.ToInt32(textBox2.Text));
            }

        }

        public bool Available(BusyTime b, List<AppLogic.Task> tasks)
        {
            bool available = true;
            foreach(AppLogic.Task t in tasks)
            {
                if(t.fixedTime == true && t.scheduled.DayOfYear == b.scheduled.DayOfYear)
                {
                    if(b.startTime >= t.time && b.startTime < (t.time + t.duration))
                    {
                        available = false;
                    }
                    if(b.endTime > t.time && b.endTime <= (t.time + t.duration))
                    {
                        available = false;
                    }
                    if(t.time >= b.startTime && t.time < b.endTime)
                    {
                        available = false;
                    }
                    if((t.time + t.duration) > b.startTime && (t.time + t.duration) <= b.endTime)
                    {
                        available = false;
                    }
                }
            }
            return available;
        }

        private void busyTimeForm_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Hide();
            dateTimePicker1.Enabled = false;
            checkedListBox1.Hide();
            checkedListBox1.Enabled = false;
            label6.Hide();
            label7.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool repeat = false;
            if(checkBox1.Checked == true)
            {
                repeat = true;
            }
            string reason = "Text";
            if(textBox1.Text != null)
            {
                reason = textBox1.Text;
            }
            double time = Convert.ToDouble(textBox1.Text);
            double duration = Convert.ToDouble(textBox2.Text);
            if(textBox2.Text == null)
            {
                duration = Convert.ToDouble(textBox3.Text) - time;
            }
            BusyTime one = new BusyTime(reason, DateTime.Now, false, time, duration, repeat);
            one.startTime = Convert.ToInt32(time);

            if (textBox3.Text == null)
            {
                one.endTime = one.startTime + Convert.ToInt32(one.duration);
            }
            one.endTime = Convert.ToInt32(textBox3.Text);
            one.daysofWeek = new List<string>();
            one.repeatDates = new List<DateTime>();
            if(checkBox1.Checked == true)
            {
                foreach(string s in checkedListBox1.CheckedItems)
                {
                    one.daysofWeek.Add(s);
                }
                one.repeatEndDate = dateTimePicker1.Value;
            }
            one.endTime = Convert.ToInt32(textBox3.Text);


            //Calendar calendar = sendingForm.GetCalendar();
            one.display = new Label();
            one.display.Text = "nothing schedueled: " + (one.startTime - (one.startTime % 1)) + ":" + (one.startTime % 1 * 60) + " - " + (one.endTime - (one.endTime % 1)) + ":" + ((one.endTime % 1) * 60);
            one.display.Width = 100 + 10 * Convert.ToInt32(duration);
            one.display.Height = 100;
            one.display.Location = new Point(100, 100);
            one.display.BackColor = user.busyTimeColour;
            one.display.BorderStyle = BorderStyle.Fixed3D;
            one.scheduled = dateTimePicker2.Value;

            //sendingForm.tasks.Add(one);
            //sendingForm.GetCalendar().GetTasks().Add(one);
            if (Available(one, calendar.GetTasks()) == true)
            {
                calendar.GetBusyTime().Add(one);
            }
            if (Available(one, calendar.GetTasks()) == false)
            {
                MessageBox.Show("You alrady have a task schedueled at that time");
            }

            one.setRepeatDates();
            one.addRepeats(calendar, user);

            calendar.OrderTasks(sendingForm, user);
            calendar.OrderDisplay(sendingForm, user);
            calendar.UpdateTaskListControl(sendingForm);
            calendar.orderBusyTimeDisplay(user, sendingForm);

            Close();
            
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                dateTimePicker1.Show();
                dateTimePicker1.Enabled = true;
                checkedListBox1.Show();
                checkedListBox1.Enabled = true;
                label6.Show();
                label7.Show();
            }
            if(checkBox1.Checked == false)
            {
                dateTimePicker1.Hide();
                dateTimePicker1.Enabled = false;
                checkedListBox1.Hide();
                checkedListBox1.Enabled = false;
                label6.Hide();
                label7.Hide();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }



        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox1.Text) + Convert.ToInt32(textBox2.Text));
            }
            if (textBox1.Text != "" && textBox2.Text == "" && textBox3.Text != "")
            {
                textBox2.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - Convert.ToInt32(textBox1.Text));
            }
        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            if (textBox2.Text != "" && textBox1.Text != "")
            {
                textBox3.Text = Convert.ToString(Convert.ToInt32(textBox1.Text) + Convert.ToInt32(textBox2.Text));
            }
            if (textBox3.Text != "" && textBox1.Text == "" && textBox2.Text != "")
            {
                textBox1.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - Convert.ToInt32(textBox2.Text));
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text != "" && textBox1.Text != "")
            {
                textBox2.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - Convert.ToInt32(textBox1.Text));
            }
            if (textBox2.Text != "" && textBox1.Text == "" && textBox3.Text != "")
            {
                textBox1.Text = Convert.ToString(Convert.ToInt32(textBox3.Text) - Convert.ToInt32(textBox2.Text));
            }
        }
    }
    
}
