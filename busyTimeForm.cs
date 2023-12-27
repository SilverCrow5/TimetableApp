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
    public partial class busyTimeForm : Form
    {
        Form1 sendingForm;
        Calendar calendar;
        public busyTimeForm(Form1 sendingForm, Calendar calendar)
        {
            InitializeComponent();
            this.calendar = calendar;
            this.sendingForm = sendingForm;

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

        private void busyTimeForm_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Hide();
            dateTimePicker1.Enabled = false;
            checkedListBox1.Hide();
            checkedListBox1.Enabled = false;
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
            one.display.BackColor = Color.Red;
            one.display.BorderStyle = BorderStyle.Fixed3D;
            one.scheduled = dateTimePicker2.Value;

            //sendingForm.tasks.Add(one);
            //sendingForm.GetCalendar().GetTasks().Add(one);
            calendar.GetBusyTime().Add(one);

            sendingForm.GetCalendar().OrderTasks(sendingForm);
            sendingForm.GetCalendar().OrderDisplay(sendingForm);
            sendingForm.GetCalendar().UpdateTaskListControl(sendingForm);

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
            }
            if(checkBox1.Checked == false)
            {
                dateTimePicker1.Hide();
                dateTimePicker1.Enabled = false;
                checkedListBox1.Hide();
                checkedListBox1.Enabled = false;
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
