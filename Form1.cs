using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Management.Instrumentation;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;
using System.Xml.Linq;

namespace timetable_app
{
    public partial class Form1 : Form
    {
        Pen pen;
        Graphics graphics;
        public List<Task> tasks;
        public int i = 1;
        public List<Task> completedTasks;

        TableLayoutPanel layoutPanel;

        public Form1()
        {
            InitializeComponent();
            TaskList.PreviewKeyDown += TaskList_KeyDown;
            DateTime now = DateTime.Now;
            graphics = this.CreateGraphics();
            pen = new Pen(Brushes.Black);
            tasks = new List<Task>();
            completedTasks = new List<Task>();
            textBox1.Text = Convert.ToString(now.DayOfWeek);
            
            void display(Task one)
            {
                this.TaskList.Items.Add(one);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TaskList.PreviewKeyDown += TaskList_KeyDown;
            OpenTasksFromFile();
            button1.Visible = false;
            button1.Enabled = false;
            textBox2.Visible = false;
            textBox2.Enabled = false;
        }


        private void TaskList_KeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (TaskList.SelectedItem != null)
            {
                int i = 0; 
                foreach(Task t in tasks)
                {
                    if(t.taskDescription == (String)TaskList.SelectedItem)
                    {
                        i = tasks.IndexOf(t);
                    }
                }
                Task current = tasks[i];
                if (e.KeyData == Keys.Delete)
                {
                    this.Controls.Remove(current.display);
                    completedTasks.Add(current);
                    tasks.Remove(current);
                    completedTasks.Add(current);
                    TaskList.Items.Remove(current.taskDescription);
                    this.Controls.Remove(current.display);
                    SaveTasksToFile();
                    if (tasks.Count != 0)
                    {
                        OrderTasks();
                        UpdateTaskListControl();
                    }
                    int j = 0;
                    while (j < tasks.Count)
                    {
                        foreach (object s in TaskList.Items)
                        {
                            if (tasks[j].taskDescription == Convert.ToString(s))
                            {
                                if (tasks[j].display.Created == true)
                                {
                                    this.Controls.Remove(tasks[j].display);
                                    if (TaskList.Items.IndexOf(s) > 0)
                                    {
                                        int previous = 0;
                                        int k = 0;
                                        while (k < tasks.Count)
                                        {
                                            if (tasks[k].taskDescription == Convert.ToString(TaskList.Items[TaskList.Items.IndexOf(s) - 1]))
                                            {
                                                previous = k;

                                            }
                                            k++;
                                        }
                                        //tasks[j].display.Location = new Point(Convert.ToInt32(tasks[j - 1].display.Location.X) + Convert.ToInt32(tasks[j - 1].display.Width), 200);
                                        tasks[j].display.Location = new Point(Convert.ToInt32(tasks[previous].display.Location.X) + Convert.ToInt32(tasks[previous].display.Width), Convert.ToInt32(tasks[previous].display.Location.Y));


                                        if (tasks[j].display.Location.X > 500)
                                        {
                                            tasks[j].display.Location = new Point(100, Convert.ToInt32(tasks[previous].display.Location.Y + 100));
                                            if (tasks[previous].display.Location.Y == tasks[j].display.Location.Y)
                                            {
                                                tasks[j].display.Location = new Point(Convert.ToInt32(tasks[previous].display.Location.X) + Convert.ToInt32(tasks[previous].display.Width), Convert.ToInt32(tasks[previous].display.Location.Y));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        tasks[j].display.Location = new Point(100, 100);
                                    }
                                    /*if (tasks[j].display.Location.X > 500)
                                    {
                                        nextLine--;
                                        tasks[j].display.Location = new Point(100, 400);
                                        if (nextLine > 0)
                                        {
                                            tasks[j].display.Location = new Point(Convert.ToInt32(tasks[j - 1].display.Location.X) + Convert.ToInt32(tasks[j - 1].display.Width), 400);
                                        }
                                    }*/
                                    this.Controls.Add(tasks[j].display);
                                    tasks[j].display.Text = tasks[j].name + ", " + (tasks[j].time - (tasks[j].time % 1)) + ":" + (tasks[j].time % 1 * 60) + " - " + ((tasks[j].time + tasks[j].duration) - ((tasks[j].time + tasks[j].duration) % 1)) + ":" + ((tasks[j].time + tasks[j].duration) % 1 * 60) + ", " + tasks[j].scheduled.ToLongDateString();
                                }
                            }
                        }
                        j++;
                    }
                    if (tasks.Count != 0)
                    {
                        OrderTasks();
                        UpdateTaskListControl();
                    }
                }
                if (e.KeyData == Keys.Enter)
                {
                    this.Controls.Add(current.display);
                    if (TaskList.SelectedIndex > 0)
                    {

                        int previous = 0;
                        int k = 0;
                        while (k < tasks.Count)
                        {
                            if (tasks[k].taskDescription == Convert.ToString(TaskList.Items[TaskList.Items.IndexOf(current.taskDescription) - 1]))
                            {
                                previous = k;

                            }
                            k++;
                        }
                        current.display.Location = new Point(Convert.ToInt32(tasks[previous].display.Location.X) + Convert.ToInt32(tasks[previous].display.Width), Convert.ToInt32(tasks[previous].display.Location.Y));


                        if (current.display.Location.X > 500)
                        {
                            current.display.Location = new Point(100, Convert.ToInt32(tasks[previous].display.Location.Y + 100));
                            if (tasks[previous].display.Location.Y == current.display.Location.Y)
                            {
                                current.display.Location = new Point(Convert.ToInt32(tasks[previous].display.Location.X) + Convert.ToInt32(tasks[previous].display.Width), Convert.ToInt32(tasks[previous].display.Location.Y));
                            }
                        }
                    }
                    current.display.Text = current.name + ", " + (current.time - (current.time % 1)) + ":" + (current.time % 1 * 60) + " - " + ((current.time + current.duration) - ((current.time + current.duration) % 1)) + ":" + ((current.time + current.duration) % 1 * 60) + ", " + current.scheduled.ToLongDateString();
                    OrderTasks();
                    UpdateTaskListControl();
                }
                if(e.KeyData == Keys.Space)
                {
                    if (TaskList.SelectedItem != null && current.display.Created == true)
                    {
                        this.Controls.Remove(current.display);
                    }
                    OrderTasks();
                    UpdateTaskListControl();
                }
                if(e.KeyData == Keys.E)
                {
                    var frm = new TaskEditForm(this, current);
                    frm.ShowDialog();
                    
                }
            }


        }
        public void update(List<Task> list)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            //dateTimePicker1.MinDate = DateTime.Now;
            //TaskList.Items.Clear();
            //int i = 0;
            //while(i < tasks.Count)
            //{
            //    if(tasks[i].scheduled.Date == dateTimePicker1.Value.Date)
            //    {
            //        TaskList.Items.Add(tasks[i].taskDescription); old
            //    }
            //    i++;
            //}
            UpdateTaskListControl();
            int j = 0;
            while (j < tasks.Count)
            {
                if(tasks[j].display.Created == true)
                {
                    this.Controls.Remove(tasks[j].display);
                }
                foreach (object s in TaskList.Items)
                {
                    if (tasks[j].taskDescription == Convert.ToString(s))
                    {
                        if (tasks[j].display.Created == true)
                        {
                            this.Controls.Remove(tasks[j].display);
                            if (TaskList.Items.IndexOf(tasks[j].taskDescription) > 0)
                            {
                                int k = 0;
                                int previous = 0;
                                while (k < tasks.Count)
                                {
                                    if (tasks[k].taskDescription == Convert.ToString(TaskList.Items[TaskList.Items.IndexOf(s) - 1]))
                                    {
                                        previous = k;

                                    }
                                    k++;
                                }
                                if (tasks[j].display.Location.X > 500)
                                {
                                    tasks[j].display.Location = new Point(100, Convert.ToInt32(tasks[previous].display.Location.Y + 100));
                                    if (tasks[previous].display.Location.Y == tasks[j].display.Location.Y)
                                    {
                                        tasks[j].display.Location = new Point(Convert.ToInt32(tasks[previous].display.Location.X) + Convert.ToInt32(tasks[previous].display.Width), Convert.ToInt32(tasks[previous].display.Location.Y));
                                    }
                                }

                            }
                            else
                            {
                                tasks[j].display.Location = new Point(100, 100);
                            }
                            this.Controls.Add(tasks[j].display);
                        }
                    }

                }
                j++;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("you can enter stuff when I get my shit together");
            /*DateTime d = DateTime.Now; OLD STUFF
            Task task = new Task(textBox2.Text, d, false, 13, 2, 1);
            tasks.Add(task);
            if (Convert.ToDateTime(dateTimePicker1.Text).Date == d.Date)
            {
                if (task.completed == false)
                {
                    this.Controls.Add(task.display);
                }
            }*/

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = Convert.ToString(DateTime.Now.DayOfWeek);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Form1 form = new Form1();
            var frm = new TaskEntryForm(this);
            frm.ShowDialog();
        }

        public void display(Task one)
        {
            this.TaskList.Items.Add(one.taskDescription);
        }

        private void TaskList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void OrderTasks()
        {
            if (tasks.Count > 0)
            {
                Task lastAdded = tasks[tasks.Count - 1];
                tasks = tasks.OrderByDescending(x => x.priority).ThenByDescending(x => x.duration).ToList();
                tasks[0].time = 0;
                int j = 0;

                while (j < tasks.Count)
                {
                    tasks[j].scheduled = DateTime.Today;
                    tasks[j].time = 0;
                    tasks[j].Ahead(tasks);
                    tasks[j].Behind(tasks);
                    int k = 0;
                    while (k < j)
                    {
                        if (tasks[j].scheduled.DayOfYear == tasks[k].scheduled.DayOfYear)
                        {
                            tasks[j].time = tasks[k].time + tasks[k].duration;
                        }
                        k++;
                    }
                    if (tasks[j].time + tasks[j].duration > 24)
                    {
                        if (tasks[j].scheduled.AddDays(1).DayOfYear <= tasks[j].due.DayOfYear)
                        {
                            tasks[j].scheduled = tasks[j].scheduled.AddDays(1);
                            tasks[j].time = 0;
                            int l = 0;
                            while (l < j)
                            {
                                if (tasks[j].scheduled.DayOfYear == tasks[l].scheduled.DayOfYear)
                                {
                                    tasks[j].time = tasks[l].time + tasks[l].duration;
                                }
                                l++;
                            }
                        }
                        else
                        {
                            MessageBox.Show("There is not enough time to schedule them all");
                            tasks.Remove(lastAdded);
                        } 
                        //OrderTasks(); makes it all go wrong
                    }
                    int i = 0;
                    while (i < j)
                    {
                        if (tasks[j].scheduled.DayOfYear == tasks[i].scheduled.DayOfYear)
                        {
                            tasks[j].time = tasks[i].time + tasks[i].duration;
                        }
                        i++;
                    }
                    if (j < tasks.Count && tasks.Count != 0)
                    {
                        tasks[j].taskDescription = tasks[j].name + ", " + (tasks[j].time - (tasks[j].time % 1)) + ":" + (tasks[j].time % 1 * 60) + " - " + ((tasks[j].time + tasks[j].duration) - (tasks[j].time + tasks[j].duration % 1)) + ":" + ((tasks[j].time + tasks[j].duration) % 1 * 60) + ", " + tasks[j].scheduled.ToLongDateString();
                    }
                    j++;
                }
            }
            

        }
        public void UpdateTaskListControl()
        {
            TaskList.Items.Clear();
            int i = 0;
            while (i < tasks.Count)
            {
                tasks[i].taskDescription = tasks[i].name + ", " + (tasks[i].time - (tasks[i].time % 1)) + ":" + (tasks[i].time % 1 * 60) + " - " + ((tasks[i].time + tasks[i].duration) - ((tasks[i].time + tasks[i].duration) % 1)) + ":" + ((tasks[i].time + tasks[i].duration) % 1 * 60) + ", " + tasks[i].scheduled.ToLongDateString();
                if (tasks[i].scheduled.DayOfYear == dateTimePicker1.Value.DayOfYear)
                {
                    TaskList.Items.Add(tasks[i].taskDescription);

                }

                //TaskList.Items.Add(tasks[i].taskDescription);
                i++;
            }
        }
        public void SaveTasksToFile()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(@"ExampleNew.dat", FileMode.Create, FileAccess.Write);
            //foreach (Task t in tasks)
            //{
            //    formatter.Serialize(stream, t); //this doesen't work with labels so I have to disable it when leables are enabled until I can fix it
            //}
            formatter.Serialize(stream, tasks);
            stream.Close();
        }

        public void OpenTasksFromFile()
        {
            var filePath = @"ExampleNew.dat";

            if (!File.Exists(filePath))
            {
                var fileCreator = File.Create(filePath);
                fileCreator.Close();
            }
            else //Load the tasks into task from the stream
            {
                BinaryFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(@"ExampleNew.dat", FileMode.Open, FileAccess.Read);
                if (stream.Length > 0)
                {
                    tasks = (List<Task>)formatter.Deserialize(stream);
                }

                stream.Close();
                
            }
            UpdateTaskListControl();



        }
        public void DeleteTasksFromFile(Task t)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            //Stream stream = new FileStream(@"ExampleNew.dat", FileMode.Create, FileAccess.ReadWrite);
            if (File.Exists(@"ExampleNew.dat"))
            {
                String[] contentsArray = File.ReadAllLines(@"ExampleNew.dat");
                List<string> contentsList = new List<string>();
                foreach (string contents in contentsArray)
                {
                    
                    contentsList.Add(contents);
                }
                foreach (string a in contentsList)
                {
                    if (a == Convert.ToString(t))
                    {
                        contentsList.Remove(a);
                    }
                }
                File.WriteAllLines(@"ExampleNew.dat", contentsList);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
    [Serializable]
    public class Task : ISerializable
    {
        public string name;
        public double time;
        public DateTime scheduled;
        public bool completed;
        [NonSerialized]
        public Label display; //this doesent' work with the saving feature so I comment out everything refrencing while I eneable the saving feature it until I can make them work together
        public double duration;
        public string taskDescription;
        public int priority;
        public DateTime due;
        public int displayTime;
        public List<Task> predecessors;
        public List<Task> successors;
        public string details;
        public double EST;
        public double LST;
        public double EFT;
        public double LFT;
        public Task(string name, string details, DateTime scheduled, bool completed, double time, double duration, int priority, DateTime due)
        {

            this.name = name;
            this.scheduled = scheduled;
            this.completed = false;
            this.duration = duration;
            this.time = time;
            display = new Label();
            display.Text = name + ", " + (time - (time % 1)) + ":" + (time % 1 * 60) + " - " + ((time + duration) - ((time + duration) % 1)) + ":" + (((time + duration) % 1) * 60) + ", " + scheduled.ToLongDateString();
            display.Width = 100 + 10 * Convert.ToInt32(duration); //change this based on length of task
            display.Height = 100;
         
            display.Location = new Point(100, 100);
            display.BackColor = Color.AliceBlue;
            display.BorderStyle = BorderStyle.Fixed3D;
            taskDescription = name + ", " + (time - (time % 1)) + ":" + (time % 1 * 60) + " - " + ((time + duration) - (time + duration) % 1) + ":" + ((time + duration) % 1 * 60) + ", " + scheduled.ToLongDateString();
            this.taskDescription = taskDescription;
            this.priority = priority;
            this.due = due;
            this.predecessors = new List<Task>();
            this.successors = new List<Task>();
            this.details = details;
        }
        public Task(string name, DateTime scheduled, double duration, Double time, bool completed)
        {

        }

        //Constructor to load from file 
        public Task(SerializationInfo information, StreamingContext context)
        {
            name = information.GetString("Name");
            details = information.GetString("Details");
            scheduled = information.GetDateTime("Scheduled");
            completed = information.GetBoolean("Completed");
            time = information.GetDouble("Time");
            duration = information.GetDouble("Duration");
            priority = information.GetInt16("Priority");
            due = information.GetDateTime("Due");

            display = new Label();
            display.Text = name;
            display.Width = 100 + (10 * Convert.ToInt32(duration)); //change this based on length of task
            display.Height = 100;
           
            display.Location = new Point(100, 100);
            display.BackColor = Color.AliceBlue;
            display.BorderStyle = BorderStyle.Fixed3D;

        }
        public void GetObjectData(SerializationInfo information, StreamingContext context)
        {
      
            information.AddValue("Name", name);
            information.AddValue("Details", details);
            information.AddValue("Scheduled", scheduled);
            information.AddValue("Completed", completed);
            information.AddValue("Time", time);
            information.AddValue("Duration", duration);
            information.AddValue("Priority", priority);
            information.AddValue("Due", due);


        }
    
    public List<Task> Ahead(List<Task> list)
        {
            list[0].EFT = list[0].EST + list[0].duration;
            int i = 1;
            while (i < list.Count)
            {
                foreach (Task t in list[i].predecessors)
                {
                    if (list[i].EST < t.EFT)
                    {
                        list[i].EST = t.EFT;
                    }
                }
                list[i].EFT = list[i].EST + list[i].duration;
                i++;
            }
            return list;
        }
        public List<Task> Behind(List<Task> list)
        {
            list[list.Count - 1].LFT = list[list.Count - 1].EFT;
            list[list.Count - 1].LST = list[list.Count - 1].LFT - list[list.Count - 1].duration;
            int i = list.Count - 2;
            while (i >= 0)
            {
                foreach (Task t in list[i].successors)
                {
                    if (list[i].LFT == 0)
                    {
                        list[i].LFT = t.LST;
                    }
                    else
                    {
                        if (list[i].LFT > t.LST)
                        {
                            list[i].LFT = t.LST;
                        }
                    }
                }
                i--;
            }
            return list;
        }

        public void CritPath(List<Task> list)
        {
            Console.WriteLine("\n    Critical Path: ");

            foreach (Task t in list)
            {
                if ((t.EFT - t.LFT == 0) && (t.EFT - t.LST == 0))
                {
                    Console.WriteLine("{0}", t.name);
                }

            }
            Console.WriteLine("\n\n     Total duration: {0}\n\n", list[list.Count - 1].EFT);
        }

    }
    class Calendar
    {

    }

    public class BusyTime
    {
        int startTime;
    }

    public class Event : Task
    {
        public Event(string name, DateTime scheduled, bool completed, double time, double duration) : base(name, scheduled, duration, time, completed)
        {
            this.name = name;
            this.scheduled = scheduled;
            this.duration = duration;
            this.time = time;
            this.completed = false;
        }
    }



}    


// I've had enough of this shit