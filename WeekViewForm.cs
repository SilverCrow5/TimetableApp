using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using timetable_app.AppLogic;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace timetable_app
{
    public partial class WeekViewForm : Form
    {
        Form1 originalForm;
        Calendar calendar;
        User u;
        Label[,] labelsGrid;
        Label[] labelTimes;
        List<Label> labelTasks = new List<Label>();
        int width = 100;
        int height = 30; //changed from 50 to 30 for more space

        public WeekViewForm(Form1 originalForm, Calendar calendar, User u)
        {
            InitializeComponent();
            this.originalForm = originalForm;
            this.calendar = calendar;
            this.u = u;
            BackColor = u.calendarColour;
        }

        private void WeekViewForm_Load(object sender, EventArgs e)
        {
            foreach (Task t in calendar.GetTasks())
            {
                Label l = new Label();
                l.BorderStyle = BorderStyle.FixedSingle;
                l.Width = width;
                l.Height = Convert.ToInt32(t.duration) * height;
                int locY = 100 + ((Convert.ToInt32(t.time) - /*9*/u.morningTime) * 30); //- 9 to account for array starting at 9am, 100+ because that's the location of the first box on y axis, that's outdated, it's start time instead of 9
                switch (t.scheduled.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        l.Location = new Point(1 * width, locY);
                        break;
                    case DayOfWeek.Tuesday:
                        l.Location = new Point(2 * width, locY);
                        break;
                    case DayOfWeek.Wednesday:
                        l.Location = new Point(3 * width, locY);
                        break;
                    case DayOfWeek.Thursday:
                        l.Location = new Point(4 * width, locY);
                        break;
                    case DayOfWeek.Friday:
                        l.Location = new Point(5 * width, locY);
                        break;
                    case DayOfWeek.Saturday:
                        l.Location = new Point(6 * width, locY);
                        break;
                    case DayOfWeek.Sunday:
                        l.Location = new Point(7 * width, locY);
                        break;

                }
                l.Text = t.taskDescription;
                l.BackColor = u.taskColour;
                l.BringToFront();
                labelTasks.Add(l);
                this.Controls.Add(l);
                if(t.time < u.morningTime || t.time > u.nightTime)
                {
                    l.Hide();
                }
            }
            foreach(BusyTime b in calendar.GetBusyTime()) // just repeated what was done with tasks
            {
                Label l = new Label();
                l.BorderStyle = BorderStyle.FixedSingle;
                l.Width = width;
                l.Height = Convert.ToInt32(b.duration) * height;
                int locY = 100 + ((Convert.ToInt32(b.time) - /*9*/u.morningTime) * 30); //- 9 to account for array starting at 9am, 100+ because that's the location of the first box on y axis, that's outdated, it's start time instead of 9
                switch (b.scheduled.DayOfWeek)
                {
                    case DayOfWeek.Monday:
                        l.Location = new Point(1 * width, locY);
                        break;
                    case DayOfWeek.Tuesday:
                        l.Location = new Point(2 * width, locY);
                        break;
                    case DayOfWeek.Wednesday:
                        l.Location = new Point(3 * width, locY);
                        break;
                    case DayOfWeek.Thursday:
                        l.Location = new Point(4 * width, locY);
                        break;
                    case DayOfWeek.Friday:
                        l.Location = new Point(5 * width, locY);
                        break;
                    case DayOfWeek.Saturday:
                        l.Location = new Point(6 * width, locY);
                        break;
                    case DayOfWeek.Sunday:
                        l.Location = new Point(7 * width, locY);
                        break;
                }
                l.Text = "nothing, " + (b.time - (b.time % 1)) + ":" + (b.time % 1 * 60) + " - " + (b.endTime - (b.endTime % 1)) + ":" + ((b.endTime % 1) * 60) + ", " + b.scheduled.ToLongDateString();
                l.BackColor = u.busyTimeColour;
                l.BringToFront();
                labelTasks.Add(l);
                this.Controls.Add(l);
                if (b.time < u.morningTime || b.time > u.nightTime)
                {
                    l.Hide();
                }
            }



            labelTimes = new Label[/*12*/u.nightTime - u.morningTime]; //I might have to go back to a fixed morning and night time if this doesen't work
            for (int i = 0; i < labelTimes.Length; i++)
            {
                string time = Convert.ToString(i+u.morningTime) + ":00";  
                Label l = new Label();
                l.BorderStyle = BorderStyle.FixedSingle;
                l.Width = 50;
                l.Location = new Point(50, 100 + i* height);
                l.Height = height;
                l.Text = time;
                labelTimes[i] = l;
                this.Controls.Add(l);
            }
            labelsGrid = new Label[7, /*12*/u.nightTime - u.morningTime]; //I might have to go back to a fixed morning and night time if this doesen't work
            for (int i = 0; i < labelsGrid.GetLength(0); i++)
            {
                for(int j = 0; j < labelsGrid.GetLength(1); j++)
                {
                    Label l = new Label();
                    l.BorderStyle = BorderStyle.FixedSingle;
                    l.Width = width;
                    l.Location = new Point(100 + i*width, 100 + j*height);
                    l.Height = height;
                    labelsGrid[i, j] = l;
                    this.Controls.Add(l);

                }
            }
           
            Console.WriteLine("boo");
        }

    }
}
