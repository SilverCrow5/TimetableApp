using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using timetable_app.AppLogic;

namespace timetable_app
{
    public partial class YesNoForm : Form
    {
        AppLogic.Task t;
        Form1 f;
        Calendar c;
        User u;
        public TaskEditForm te; //made it optional so it can be used outside of the task edit form
        public YesNoForm(AppLogic.Task t, Form1 f, Calendar c, User u)
        {
            InitializeComponent();
            this.t = t;
            this.f = f;
            this.c = c;
            this.u = u;
            Text = "Mark " + t.name + " as completed";
        }

        private void YesNoForm_Load(object sender, EventArgs e)
        {
            Text = "Mark " + t.name + " as completed"; //here beacuse it won't let me do that in the designer form
            label1.Text = "Are you sure you want to mark " + t.name + " as completed?\r\nIf you do it will be perminantly removed from your\r\ncalendar and timetable and other tasks may take up\r\nthe time it occupied.\r\n"; //here beacuse it won't let me do that in the designer form
        }

        private void button1_Click(object sender, EventArgs e)
        {
            f.Controls.Remove(t.display); // basically the same as the origional delete function
            c.AddCompletedTask(t);
            c.GetTasks().Remove(t);

            f.TaskList.Items.Remove(t.taskDescription);
            c.DeleteTasksFromFile(t);
            c.SaveTasksToFile();

            if (c.GetTasks().Count != 0)
            {
                c.OrderTasks(f, u);
                c.UpdateTaskListControl(f);
            }
            c.orderBusyTimeDisplay(u, f);
            c.OrderDisplay(f, u);

            if(te != null)
            {
                te.Close();
            }
            Close();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
