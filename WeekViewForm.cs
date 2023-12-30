﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;
using timetable_app.AppLogic;
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
        int height = 50;

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
                int locY = 100 + ((Convert.ToInt32(t.time) - 9/*u.morningTime*/) * 50); //- 9 to account for array starting at 9am, 100+ because that's the location of the first box on y axis, that's outdated
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
                l.BackColor = Color.White;
                l.BringToFront();
                labelTasks.Add(l);
                this.Controls.Add(l);
                if(t.time < u.morningTime || t.time > u.nightTime)
                {
                    l.Hide();
                }
            }
            foreach(BusyTime b in calendar.GetBusyTime())
            {

            }



            labelTimes = new Label[12];
            for (int i = 0; i < labelTimes.Length; i++)
            {
                string time = Convert.ToString(i+9) + ":00";  
                Label l = new Label();
                l.BorderStyle = BorderStyle.FixedSingle;
                l.Width = 50;
                l.Location = new Point(50, 100 + i* height);
                l.Height = height;
                l.Text = time;
                labelTimes[i] = l;
                this.Controls.Add(l);
            }
            labelsGrid = new Label[7, 12/*u.nightTime - u.morningTime*/]; //I'll figure out a way to make it work
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
