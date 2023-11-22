using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

            if(maskedTextBox1.Text != "" && maskedTextBox2.Text != "")
            {
                maskedTextBox3.Text = Convert.ToString(Convert.ToInt32(maskedTextBox1.Text) + Convert.ToInt32(maskedTextBox2.Text));
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
            double time = Convert.ToDouble(maskedTextBox1.Text);
            double duration = Convert.ToDouble(maskedTextBox2.Text);
            if(maskedTextBox2.Text == null)
            {
                duration = Convert.ToDouble(maskedTextBox3.Text) - time;
            }
            BusyTime one = new BusyTime(reason, DateTime.Now, false, time, duration, repeat);

            if (maskedTextBox3.Text == "")
            {
                one.endTime = one.startTime + Convert.ToInt32(one.duration);
            }
            if(checkBox1.Checked == true)
            {
                foreach(string s in checkedListBox1.CheckedItems)
                {
                    one.daysofWeek.Add(s);
                }
                one.repeatEndDate = dateTimePicker1.Value;
            }
            one.endTime = Convert.ToInt32(maskedTextBox3.Text);


            //Calendar calendar = sendingForm.GetCalendar();
            calendar.GetTasks().Add(one);
            calendar.GetBusyTime().Add(one);
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

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            if(maskedTextBox1.Text != "" && maskedTextBox2.Text != "")
            {
                maskedTextBox3.Text = Convert.ToString(Convert.ToInt32(maskedTextBox1.Text) + Convert.ToInt32(maskedTextBox2.Text));
            }
            if(maskedTextBox1.Text != "" && maskedTextBox2.Text == "" && maskedTextBox3.Text != "")
            {
                maskedTextBox2.Text = Convert.ToString(Convert.ToInt32(maskedTextBox3.Text) - Convert.ToInt32(maskedTextBox1.Text));
            }
        }

        private void maskedTextBox2_TextChanged(object sender, EventArgs e)
        {
            if (maskedTextBox2.Text != "" && maskedTextBox1.Text != "")
            {
                maskedTextBox3.Text = Convert.ToString(Convert.ToInt32(maskedTextBox1.Text) + Convert.ToInt32(maskedTextBox2.Text));
            }
            if(maskedTextBox3.Text != "" && maskedTextBox1.Text == "" && maskedTextBox1.Text != "")
            {
                maskedTextBox1.Text = Convert.ToString(Convert.ToInt32(maskedTextBox3.Text) - Convert.ToInt32(maskedTextBox2.Text));
            }
        }

        private void maskedTextBox3_TextChanged(object sender, EventArgs e)
        {
            if(maskedTextBox3.Text != "" && maskedTextBox1.Text != "")
            {
                maskedTextBox2.Text = Convert.ToString(Convert.ToInt32(maskedTextBox3.Text) - Convert.ToInt32(maskedTextBox1.Text));
            }
            if(maskedTextBox2.Text != "" && maskedTextBox1.Text == "" && maskedTextBox3.Text != "")
            {
                maskedTextBox1.Text = Convert.ToString(Convert.ToInt32(maskedTextBox3.Text) - Convert.ToInt32(maskedTextBox2.Text));
            }
        }
    }
    
}
