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
    public partial class SettingsForm : Form
    {
        Form1 f;
        Calendar c;
        User u;
        public SettingsForm(Form1 f, Calendar c, User u)
        {
            InitializeComponent();
            this.f = f;
            this.c = c;
            this.u = u;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            maskedTextBox1.ValidatingType = typeof(int);
            maskedTextBox2.ValidatingType = typeof(int);
            maskedTextBox1.Text = Convert.ToString(u.morningTime);
            maskedTextBox2.Text = Convert.ToString(u.nightTime);
            comboBox1.Text = reverseColour(u.taskColour);
            comboBox2.Text = reverseColour(u.busyTimeColour);
            comboBox3.Text = reverseColour(u.calendarColour);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text != "")
            {
                u.taskColour = colour(comboBox1.Text);
            }
            if(comboBox1.Text == "")
            {
                u.taskColour = Color.AliceBlue;
            }
            if (comboBox2.Text != "")
            {
                u.busyTimeColour = colour(comboBox2.Text);
            }
            if(comboBox2.Text == "")
            {
                u.busyTimeColour = Color.Red;
            }
            if(comboBox3.Text != "")
            {
                u.calendarColour = colour(comboBox3.Text);
            }
            if(comboBox3.Text == "")
            {
                u.calendarColour = DefaultBackColor;
            }
            if(maskedTextBox1.Text == "")
            {
                maskedTextBox1.Text = "0";
            }
            if(maskedTextBox2.Text == "")
            {
                maskedTextBox2.Text = "24";
            }

            if(maskedTextBox1.Text != "" && Convert.ToInt32(maskedTextBox1.Text) < 24 && Convert.ToInt32(maskedTextBox1.Text) >= 0 && Convert.ToInt32(maskedTextBox1.Text) < Convert.ToInt32(maskedTextBox2.Text))
            {
                u.morningTime = Convert.ToInt32(maskedTextBox1.Text); //there must be a more efficient way to do this
            }
            if(maskedTextBox1.Text == "" || Convert.ToInt32(maskedTextBox1.Text) >= 24 || Convert.ToInt32(maskedTextBox1.Text) < 0 || Convert.ToInt32(maskedTextBox1.Text) >= Convert.ToInt32(maskedTextBox2.Text)) //this is pretty long but it would be to many lines if I had them as individual conditions
            {
                u.morningTime = 0;
                MessageBox.Show("you either left the morning time box blank, set it to an hour that doesen't exist or made it after the night time, so it's been set to 00:00");
                maskedTextBox1.Text = "0";
            }
            if(maskedTextBox2.Text != "" && Convert.ToInt32(maskedTextBox2.Text) <= 24 && Convert.ToInt32(maskedTextBox2.Text) > 0 && Convert.ToInt32(maskedTextBox1.Text) < Convert.ToInt32(maskedTextBox2.Text))
            {
                u.nightTime = Convert.ToInt32(maskedTextBox2.Text);
            }
            if (maskedTextBox2.Text == "" || Convert.ToInt32(maskedTextBox2.Text) > 24 || Convert.ToInt32(maskedTextBox2.Text) <= 0 || Convert.ToInt32(maskedTextBox1.Text) >= Convert.ToInt32(maskedTextBox2.Text))
            {
                u.nightTime = 0;
                MessageBox.Show("you either left the night time box blank, set it to an hour that doesen't exist or made it before the night time, so it's been sent to 00:00");
                maskedTextBox2.Text = "24";
                
            }

            c.OrderTasks(f, u);
            c.UpdateTaskListControl(f);
            c.OrderDisplay(f, u);
            c.orderBusyTimeDisplay(u, f);
            c.SaveSettings(u);
            Close();

        }

        public Color colour(string text) // for choosing colours using teh dropdown box
        {
            Color c = Color.White; // default colour I guess
            if(text == "Red")
            {
                c = Color.Red;
            }
            if(text == "Yellow")
            {
                c = Color.Yellow;
            }
            if(text == "Green")
            {
                c = Color.Green;
            }
            if(text == "Blue")
            {
                c = Color.Blue;
            }
            if(text == "Purple")
            {
                c = Color.Purple;
            }
            if(text == "Orange")
            {
                c = Color.Orange;
            }
            if(text == "Grey")
            {
                c = Color.Gray;
            }
            if(text == "Light blue")
            {
                c = Color.AliceBlue;
            }
            if(text == "Maroon")
            {
                c = Color.Maroon;
            }
            if(text == "Pink")
            {
                c = Color.Pink;
            }
            if(text == "Light grey")
            {
                c = DefaultBackColor;
            }
            if(text == "Light green")
            {
                c = Color.LightGreen;
            }
            return c;
        }

        public string reverseColour(Color c)
        {
            string colour = "White";
            if(c == Color.Red)
            {
                colour = "Red";
            }
            if(c == Color.Yellow)
            {
                colour = "Yellow";
            }
            if(c == Color.Green)
            {
                colour = "Green";
            }
            if(c == Color.Blue)
            {
                colour = "Blue";
            }
            if(c == Color.Purple)
            {
                colour = "Purple";
            }
            if(c == Color.Orange)
            {
                colour = "Orange";
            }
            if(c == Color.Gray)
            {
                colour = "Grey";
            }
            if(c == Color.AliceBlue)
            {
                colour = "Light blue";
            }
            if(c == Color.Maroon)
            {
                colour = "Maroon";
            }
            if(c == Color.Pink)
            {
                colour = "Pink";
            }
            if(c == DefaultBackColor)
            {
                colour = "Light grey";
            }
            if(c == Color.LightGreen)
            {
                colour = "Light green";
            }
            return colour;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e) // because most settings menus have this option and I hate when they don't
        {
            maskedTextBox1.Text = "0";
            maskedTextBox2.Text = "24";
            comboBox1.Text = "Light blue";
            comboBox2.Text = "Red";
            comboBox3.Text = "Light grey";
        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            maskedTextBox1.Text = "0";
        }

        private void maskedTextBox2_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            maskedTextBox2.Text = "24";
        }
    }
}
