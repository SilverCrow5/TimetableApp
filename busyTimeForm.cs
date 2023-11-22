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
            BusyTime one = new BusyTime(reason, DateTime.Now, false, 0, 0, 0, repeat);
            
            if(checkBox1.Checked == true)
            {
                foreach(string s in checkedListBox1.CheckedItems)
                {
                    one.daysofWeek.Add(s);
                }
                one.repeatEndDate = dateTimePicker1.Value;
            }

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

    }
    
}
