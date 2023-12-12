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
using timetable_app.AppLogic;

namespace timetable_app
{
    public partial class Form1 : Form
    {
        Pen pen;
        Graphics graphics;
        public List<AppLogic.Task> tasks;
        public List<BusyTime> busyTimes;
        public int i = 1;
        public List<AppLogic.Task> completedTasks;

        Calendar calendar;
        TableLayoutPanel layoutPanel;

        public Form1()
        {
            InitializeComponent();
            TaskList.KeyDown += TaskList_KeyDown;
            DateTime now = DateTime.Now;
            graphics = this.CreateGraphics();
            pen = new Pen(Brushes.Black);
            tasks = new List<AppLogic.Task>();
            busyTimes = new List<BusyTime>();
            completedTasks = new List<AppLogic.Task>();
            calendar = new Calendar();
            textBox1.Text = Convert.ToString(now.DayOfWeek);
            
            void display(AppLogic.Task one)
            {
                this.TaskList.Items.Add(one);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //TaskList.PreviewKeyDown += TaskList_KeyDown;
            calendar.OpenTasksFromFile(this);
            calendar.OrderTasks();
            calendar.UpdateTaskListControl(this);
            calendar.OrderDisplay(this);
        }


        private void TaskList_KeyDown(object sender, KeyEventArgs e)
        {

            if (TaskList.SelectedItem != null)
            {
                int i = 0;
                foreach (AppLogic.Task t in calendar.GetTasks())
                {
                    if (t.taskDescription == (String)TaskList.SelectedItem)
                    {
                        i = calendar.GetTasks().IndexOf(t);
                    }
                }
                AppLogic.Task current = calendar.GetTasks()[i];
                if (e.KeyData == Keys.Delete)
                {
                    this.Controls.Remove(current.display);
                    calendar.AddCompletedTask(current);
                    calendar.GetTasks().Remove(current);

                    TaskList.Items.Remove(current.taskDescription);
                    this.Controls.Remove(current.display);
                    calendar.DeleteTasksFromFile(current);
                    calendar.SaveTasksToFile();
                    if (calendar.GetTasks().Count != 0)
                    {
                        calendar.OrderTasks();
                        calendar.UpdateTaskListControl(this);
                        calendar.OrderDisplay(this);
                    }

                    if (calendar.GetTasks().Count != 0)
                    {
                        calendar.OrderTasks();
                        calendar.UpdateTaskListControl(this);
                        calendar.OrderDisplay(this);
                    }
                }
                if (e.KeyData == Keys.Enter)
                {                    
                    this.Controls.Add(current.display);
                    if (TaskList.SelectedIndex > 0)
                    {

                        int previous = 0;
                        int k = 0;
                        while (k < calendar.GetTasks().Count)
                        {
                            if (calendar.GetTasks()[k].taskDescription == Convert.ToString(TaskList.Items[TaskList.Items.IndexOf(current.taskDescription) - 1]))
                            {
                                previous = k;

                            }
                            k++;
                        }
                        calendar.OrderDisplay(this);
                        current.display.Location = new Point(Convert.ToInt32(calendar.GetTasks()[previous].display.Location.X) + Convert.ToInt32(calendar.GetTasks()[previous].display.Width), Convert.ToInt32(calendar.GetTasks()[previous].display.Location.Y));


                        if (current.display.Location.X > 500)
                        {
                            current.display.Location = new Point(100, Convert.ToInt32(calendar.GetTasks()[previous].display.Location.Y + 100));
                            if (calendar.GetTasks()[previous].display.Location.Y == current.display.Location.Y)
                            {
                                current.display.Location = new Point(Convert.ToInt32(calendar.GetTasks()[previous].display.Location.X) + Convert.ToInt32(calendar.GetTasks()[previous].display.Width), Convert.ToInt32(calendar.GetTasks()[previous].display.Location.Y));
                            }
                        }
                    }
                    current.display.Text = current.name + ", " + (current.time - (current.time % 1)) + ":" + (current.time % 1 * 60) + " - " + ((current.time + current.duration) - ((current.time + current.duration) % 1)) + ":" + ((current.time + current.duration) % 1 * 60) + ", " + current.scheduled.ToLongDateString();
                    calendar.OrderTasks();
                    calendar.OrderDisplay(this);
                    calendar.UpdateTaskListControl(this);
                }
                if (e.KeyData == Keys.Space)
                {
                    if (TaskList.SelectedItem != null && current.display.Created == true)
                    {
                        //this.Controls.Remove(current.display);
                        current.display.Visible = false;
                    }
                    calendar.OrderTasks();
                    calendar.UpdateTaskListControl(this);
                }
                if (e.KeyData == Keys.E)
                {
                    var frm = new TaskEditForm(this, current, calendar);
                    frm.ShowDialog();



                }
            }




        }
        public void update(List<AppLogic.Task> list)
        {

        }
        public Calendar GetCalendar()
        {
            return calendar;
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

            calendar.OrderTasks();
            calendar.UpdateTaskListControl(this);
            calendar.OrderDisplay(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.Text = Convert.ToString(DateTime.Now.DayOfWeek);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Form1 form = new Form1();
            var frm = new TaskEntryForm(this, calendar);
            frm.ShowDialog();
        }

        public void display(AppLogic.Task one)
        {
            this.TaskList.Items.Add(one.taskDescription);
        }

        private void TaskList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            var frm = new busyTimeForm(this, calendar);
            frm.ShowDialog();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                foreach(AppLogic.Task t in tasks)
                {
                    if(t.scheduled == dateTimePicker1.Value && t.GetType() == typeof(BusyTime))
                    {
                        this.Controls.Add(t.display);
                        t.display.Visible = true;
                    }
                }
                foreach(BusyTime u in busyTimes)
                {
                    if (u.scheduled.Date == dateTimePicker1.Value.Date)
                    {
                        this.Controls.Add(u.display);
                        u.display.Visible = true;
                    }
                    foreach (AppLogic.Task t in tasks)
                    {
                        if (t.scheduled.Date == dateTimePicker1.Value.Date)
                        {

                        }
                    }
                }
            }

        }

        private void btnWeekView_Click(object sender, EventArgs e)
        {
            var frm = new WeekViewForm(this, calendar);
            frm.ShowDialog();
        }
    }
    /*
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
        public double estimatedStartTime;
        public double latestStartTime;
        public double estimatedFinishTime;
        public double latestFinishTime;
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
            if (list.Count != 0)
            {
                list[0].estimatedFinishTime = list[0].estimatedStartTime + list[0].duration;
                int i = 1;
                while (i < list.Count)
                {
                    if (list[i].predecessors.Count != 0)
                    {
                        foreach (Task t in list[i].predecessors)
                        {
                            if (list[i].estimatedStartTime < t.estimatedFinishTime)
                            {
                                list[i].estimatedStartTime = t.estimatedFinishTime;
                            }
                        }
                    }
                    list[i].estimatedFinishTime = list[i].estimatedStartTime + list[i].duration;
                    i++;
                }
            }
            return list;
        }
        public List<Task> Behind(List<Task> list)
        {
            if (list.Count != 0)
            {
                list[list.Count - 1].latestFinishTime = list[list.Count - 1].estimatedFinishTime;
                list[list.Count - 1].latestStartTime = list[list.Count - 1].latestFinishTime - list[list.Count - 1].duration;
                int i = list.Count - 2;
                while (i >= 0)
                {
                    if (list[i].successors.Count != 0)
                    {
                        foreach (Task t in list[i].successors)
                        {
                            if (list[i].latestFinishTime == 0)
                            {
                                list[i].latestFinishTime = t.latestStartTime;
                            }
                            else
                            {
                                if (list[i].latestFinishTime > t.latestStartTime)
                                {
                                    list[i].latestFinishTime = t.latestStartTime;
                                }
                            }
                        }
                    }
                    i--;
                }
            }
            return list;
        }

        public void CritPath(List<Task> list)
        {
            if (list.Count != 0)
            {
                Console.WriteLine("\n    Critical Path: ");

                foreach (Task t in list)
                {
                    if ((t.estimatedFinishTime - t.latestFinishTime == 0) && (t.estimatedFinishTime - t.latestStartTime == 0))
                    {
                        Console.WriteLine("{0}", t.name);
                    }

                }
                Console.WriteLine("\n\n     Total duration: {0}\n\n", list[list.Count - 1].estimatedFinishTime);
            }
        }

    }
    */

    public class Calendar
    {
        private List<AppLogic.Task> tasks;
        private List<BusyTime> busyTimes;
        private List<AppLogic.Task> completedTasks;

        public Calendar()
        {
            tasks = new List<AppLogic.Task>();
            completedTasks = new List<AppLogic.Task>();
            busyTimes = new List<BusyTime>();
        }
        public List<AppLogic.Task> GetTasks()
        {
            return tasks;
        }
        public List<BusyTime> GetBusyTime()
        {
            return busyTimes;
        }
        public List<AppLogic.Task> GetCompletedTasks()
        {
            return completedTasks;
        }
        public void AddTask(AppLogic.Task t)
        {
            tasks.Add(t);
        }
        public void AddCompletedTask(AppLogic.Task t)
        {
            completedTasks.Add(t);
        }
        public void OrderTasks()
        {
            if (tasks.Count > 0)
            {
                AppLogic.Task lastAdded = tasks[tasks.Count - 1];
                tasks = tasks.OrderByDescending(x => x.priority).ThenByDescending(x => x.estimatedStartTime).ThenByDescending(x => x.duration).ToList();
                tasks[0].time = 0;
                int j = 0;

                while (j < tasks.Count)
                {
                    if (tasks[j].GetType() != typeof(BusyTime))
                    {
                        tasks[j].scheduled = DateTime.Today;
                        tasks[j].time = 0;
                        tasks[j].Ahead(tasks);
                        tasks[j].Behind(tasks);
                        orderForDay(tasks[j], tasks);
                        if (tasks[j].time + tasks[j].duration > 24)
                        {
                            if (tasks[j].scheduled.AddDays(1).DayOfYear <= tasks[j].due.DayOfYear)
                            {
                                tasks[j].scheduled = tasks[j].scheduled.AddDays(1);
                                tasks[j].time = 0;
                                orderForDay(tasks[j], tasks);
                            }
                            else
                            {
                                MessageBox.Show("There is not enough time to schedule them all");
                                tasks.Remove(lastAdded);
                                j--;
                            }
                            //OrderTasks(); makes it all go wrong
                        }
                        int i = 0;
                        orderForDay(tasks[j], tasks);
                        if (j < tasks.Count && tasks.Count != 0)
                        {
                            tasks[j].taskDescription = tasks[j].name + ", " + (tasks[j].time - (tasks[j].time % 1)) + ":" + (tasks[j].time % 1 * 60) + " - " + ((tasks[j].time + tasks[j].duration) - (tasks[j].time + tasks[j].duration % 1)) + ":" + ((tasks[j].time + tasks[j].duration) % 1 * 60) + ", " + tasks[j].scheduled.ToLongDateString();
                        }
                    }
                    j++;
                }
                tasks.OrderByDescending(x => x.time);
            }


        }
        public void UpdateTaskListControl(Form1 form)
        {
            form.TaskList.Items.Clear();
            int i = 0;
            while (i < tasks.Count)
            {
                tasks[i].taskDescription = tasks[i].name + ", " + (tasks[i].time - (tasks[i].time % 1)) + ":" + (tasks[i].time % 1 * 60) + " - " + ((tasks[i].time + tasks[i].duration) - ((tasks[i].time + tasks[i].duration) % 1)) + ":" + ((tasks[i].time + tasks[i].duration) % 1 * 60) + ", " + tasks[i].scheduled.ToLongDateString();
                if (tasks[i].scheduled.DayOfYear == form.dateTimePicker1.Value.DayOfYear /*&& tasks[i].GetType() != typeof(BusyTime)*/)
                {
                    form.TaskList.Items.Add(tasks[i].taskDescription);

                }

                //TaskList.Items.Add(tasks[i].taskDescription);
                i++;
            }
        }
        public void OrderDisplay(Form1 form)
        {
            foreach (AppLogic.Task t in tasks)
            {
                foreach (object s in form.TaskList.Items)
                {
                    if (Convert.ToString(s) == t.taskDescription)
                    {
                        AppLogic.Task previous = t;
                        if (form.TaskList.Items.IndexOf(s) == 0)
                        {
                            t.display.Location = new Point(100, 100);
                        }
                        else
                        {
                            foreach (AppLogic.Task u in tasks)
                            {
                                if (u.taskDescription == Convert.ToString(form.TaskList.Items[form.TaskList.Items.IndexOf(s) - 1]))
                                {
                                    previous = u;
                                }
                            }
                            t.display.Location = new Point(Convert.ToInt32(previous.display.Location.X) + Convert.ToInt32(previous.display.Width), Convert.ToInt32(previous.display.Location.Y));
                            if (t.display.Location.X > 500)
                            {
                                t.display.Location = new Point(100, Convert.ToInt32(previous.display.Location.Y + 100));
                            }
                        }
                    }
                }
                /*if(t.GetType() == typeof(BusyTime))
                {
                    foreach (AppLogic.Task u in tasks)
                    {

                    }
                }*/
                if (t.display.Created == true)
                {
                    t.display.Visible = false;
                    if (form.dateTimePicker1.Value.Date == t.scheduled.Date)
                    {
                        t.display.Visible = true;
                    }
                }
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

        public void OpenTasksFromFile(Form1 form)
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
                    tasks = (List<AppLogic.Task>)formatter.Deserialize(stream);
                }

                stream.Close();

            }
            UpdateTaskListControl(form);



        }
        public void DeleteTasksFromFile(AppLogic.Task t)
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

        public void Clearing(List<AppLogic.Task> tasks, Form1 form, Calendar calendar)
        {
            foreach (AppLogic.Task t in tasks)
            {
                if (t.due.DayOfYear < DateTime.Now.DayOfYear)
                {
                    form.Controls.Remove(t.display);
                    completedTasks.Add(t);
                    tasks.Remove(t);
                    form.TaskList.Items.Remove(t.taskDescription);
                    form.Controls.Remove(t.display);
                    DeleteTasksFromFile(t);
                    SaveTasksToFile();
                    if (tasks.Count != 0)
                    {
                        calendar.OrderTasks();
                        calendar.UpdateTaskListControl(form);
                        calendar.OrderDisplay(form);
                    }
                }
                if (t.scheduled.DayOfYear < DateTime.Now.DayOfYear)
                {
                    calendar.OrderTasks();
                    calendar.UpdateTaskListControl(form);
                    calendar.OrderDisplay(form);
                }
            }
        }
        public bool availabeCheck(AppLogic.Task t)
        {
            bool available = true;
            int number = 0;
            foreach (BusyTime a in busyTimes)
            {
                if(t.scheduled.Date == a.scheduled.Date)
                {
                    if(t.time >= a.startTime && t.time <= a.endTime)
                    {
                        available = false;
                        number = a.endTime;
                    }
                    if(t.end >= a.startTime && t.end <= a.endTime)
                    {
                        available = false;
                        number = a.endTime;
                    }
                    if(a.startTime >= t.time && a.startTime <= t.end)
                    {
                        available = false;
                        number = a.endTime;
                    }
                    if(a.endTime >= t.time && a.endTime <= t.end)
                    {
                        available = false;
                        number = a.endTime;
                    }
                }
            }
            return available;
        }
        public void orderForDay(AppLogic.Task t, List<AppLogic.Task> list)
        {
            int i = 0;
            int j = list.IndexOf(t);
            while (i < j)
            {
                if (t.scheduled.DayOfYear == tasks[i].scheduled.DayOfYear)
                {
                    t.time = tasks[i].time + tasks[i].duration;
                }
                i++;
            }
        }
    }

    public class BusyTime : AppLogic.Task
    {
        public int startTime;
        public int endTime;
        public double duration;
        public bool repeating;
        public List<string> daysofWeek;
        public List<DateTime> repeatDates;
        public DateTime repeatEndDate;
        public Label display;
        public BusyTime(string name, DateTime scheduled, bool completed, double time, double duration, bool repeating) : base(name, scheduled, duration, time, completed)
        {
            startTime = Convert.ToInt32(time);
            this.startTime = startTime;
            this.duration = duration;
            this.repeating = repeating;
            this.repeatEndDate = repeatEndDate;
            this.daysofWeek = daysofWeek;
            this.repeatDates = repeatDates;
            this.endTime = endTime;


            display = new Label();
            display.Text = "example";
            display.Width = 100 + 10 * Convert.ToInt32(duration); //change this based on duration
            display.Height = 100;

            display.Location = new Point(100, 100);
            display.BackColor = Color.Red;
            display.BorderStyle = BorderStyle.Fixed3D;
        }
        public void setRepeatDates()
        {
            DateTime i = DateTime.Now;
            while(i.DayOfYear < repeatEndDate.DayOfYear)
            {
                foreach(string s in daysofWeek)
                {
                    if(i.DayOfWeek.ToString() == s)
                    {
                        repeatDates.Add(i);
                    }
                }
                i.AddDays(1);
            }
        }
    }

    public class Event : AppLogic.Task
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