using System;
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
        Label[,] labelsGrid;
        Label[] labelTimes;
        List<Label> labelTasks = new List<Label>();
        int width = 100;
        int height = 50;
        int morning = 9;
        int night = 9;
        public WeekViewForm(Form1 originalForm, Calendar calendar)
        {
            InitializeComponent();
            this.originalForm = originalForm;
            this.calendar = calendar;
        }

        private void WeekViewForm_Load(object sender, EventArgs e)
        {
            foreach (Task t in calendar.GetTasks())
            {
                Label l = new Label();
                l.BorderStyle = BorderStyle.FixedSingle;
                l.Width = width;
                l.Height = Convert.ToInt32(t.duration) * height;
                int locY = 100 + ((Convert.ToInt32(t.time) - 9) * 50); //- 9 to account for array starting at 9am, 100+ because that's the location of the first box on y axis
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
                if(t.time < 9 || t.time > 9)
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
            labelsGrid = new Label[7, 12];
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text.GetType() == typeof(int))
            {
                if(Convert.ToInt32(textBox1.Text) < 12)
                {
                    morning = Convert.ToInt32(textBox1.Text);
                }
            }
            if(textBox1.Text == null)
            {
                morning = 9;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if(textBox2.Text.GetType() == typeof(int))
            {
                if(Convert.ToInt32(textBox2.Text) < 12)
                {
                    night = Convert.ToInt32(textBox2.Text);
                }
            }
            if(textBox2.Text == null)
            {
                night = 9;
            }
        }
    }
}
