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
    public partial class TaskEditForm : Form
    {
        Form1 mainForm;
        AppLogic.Task t;
        public TaskEditForm(Form1 mainForm, AppLogic.Task t)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.t = t;

            textBox1.Text = t.name;
            dateTimePicker1.Value = t.scheduled;
            dateTimePicker2.Value = t.due;
            textBox2.Text = t.details;
            textBox3.Text = Convert.ToString(t.duration);
            textBox4.Text = Convert.ToString(t.time - (t.time % 1)) + ":" + Convert.ToString(t.time % 1 * 60);
            textBox5.Text = Convert.ToString(t.priority);
            if (t.predecessors != null)
            {
                foreach (AppLogic.Task u in mainForm.GetCalendar().GetTasks())
                {
                    checkedListBox1.Items.Add(u);
                    if (t.predecessors.Contains(u))
                    {
                        checkedListBox1.SetItemChecked(checkedListBox1.Items.IndexOf(u), true);
                    }
                }
            }
        }

        private void TaskEditForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            t.name = textBox1.Text;
            t.due = dateTimePicker2.Value;
            t.details = textBox2.Text;
            t.duration = Convert.ToInt32(textBox3.Text);
            t.priority = Convert.ToInt32(textBox5.Text);
            t.predecessors.Clear();
            foreach (AppLogic.Task v in mainForm.GetCalendar().GetTasks())
            {
                if (v.successors.Contains(t))
                {
                    v.successors.Remove(t);
                }
            }
            foreach (AppLogic.Task u in checkedListBox1.CheckedItems)
            {
                t.predecessors.Add(u);
                u.successors.Add(t);
            }
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            Close();

        }
    }
}
