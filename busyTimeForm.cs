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

        }

        private void button1_Click(object sender, EventArgs e)
        {
            BusyTime one = new BusyTime("Text", DateTime.Now, false, 0, 0, 0, false, DateTime.Now);
            calendar.GetTasks.().Add(one);
        }
    }
}
