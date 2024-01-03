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
using System.Security.Cryptography;
using System.Security.Policy;

namespace timetable_app
{
    public partial class Form1 : Form
    {
        Pen pen;
        Graphics graphics;
        //public List<AppLogic.Task> tasks;
        //public List<BusyTime> busyTimes;
        public int i = 1;
        public List<AppLogic.Task> completedTasks;

        Calendar calendar;
        TableLayoutPanel layoutPanel;
        User user;
        

        public Form1()
        {
            InitializeComponent();
            TaskList.KeyDown += TaskList_KeyDown;
            DateTime now = DateTime.Now;
            graphics = this.CreateGraphics();
            pen = new Pen(Brushes.Black);
            //tasks = new List<AppLogic.Task>();
            //busyTimes = new List<BusyTime>();
            completedTasks = new List<AppLogic.Task>();
            calendar = new Calendar();
            user = new User();
            user.morningTime = 0;
            user.nightTime = 24;
            user.taskColour = Color.AliceBlue;
            user.busyTimeColour = Color.Red;
            user.calendarColour = DefaultBackColor; // I don't know the colour's name
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
            user = calendar.rememberPreferances(this, user);
            calendar.OrderTasks(this, user);
            calendar.UpdateTaskListControl(this);
            calendar.OrderDisplay(this, user);
            calendar.Clearing(calendar.GetTasks(), this, user);
            BackColor = user.calendarColour;
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
                    var form = new YesNoForm(current, this, calendar, user); // figured I might was well use it here too
                    form.ShowDialog();

                    
                    /*this.Controls.Remove(current.display);
                    calendar.AddCompletedTask(current);
                    calendar.GetTasks().Remove(current);


                    TaskList.Items.Remove(current.taskDescription);
                    calendar.DeleteTasksFromFile(current);
                    calendar.SaveTasksToFile();
                    if (calendar.GetTasks().Count != 0)
                    {
                        calendar.OrderTasks(this, user);
                        calendar.UpdateTaskListControl(this);
                    }
                    calendar.orderBusyTimeDisplay(calendar, user);
                    calendar.OrderDisplay(this, user);*/

                    /*.GetTasks().Count != 0)
                    {   
                        calendar.OrderTasks(this);
                        calendar.UpdateTaskListControl(this);
                        calendar.OrderDisplay(this);
                    }*/
                }
                if (e.KeyData == Keys.Enter)
                {
                    calendar.OrderTasks(this, user);
                    calendar.UpdateTaskListControl(this);
                    calendar.OrderDisplay(this, user);
                    calendar.orderBusyTimeDisplay(user, this);
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
                        calendar.OrderDisplay(this, user);
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
                }
                if (e.KeyData == Keys.Space)
                {
                    if (TaskList.SelectedItem != null && current.display.Created == true)
                    {
                        //this.Controls.Remove(current.display);
                        current.display.Visible = false;
                    }
                    calendar.OrderTasks(this, user);
                    calendar.UpdateTaskListControl(this);
                }
                if (e.KeyData == Keys.E)
                {
                    var frm = new TaskEditForm(this, current, calendar, user);
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
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e) // so the display can be correct for each day
        {

            //calendar.OrderTasks(this);
            calendar.UpdateTaskListControl(this);
            calendar.OrderDisplay(this, user);
            calendar.orderBusyTimeDisplay(user, this); 
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
            var frm = new TaskEntryForm(this, calendar, user);
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
            var frm = new busyTimeForm(this, calendar, user);
            frm.ShowDialog();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            calendar.OrderTasks(this, user);
            calendar.OrderDisplay(this, user);
            calendar.UpdateTaskListControl(this);
            if (checkBox1.Checked == true)
            {
                /*foreach(AppLogic.Task t in tasks)
                {
                    if(t.scheduled == dateTimePicker1.Value && t.GetType() == typeof(BusyTime))
                    {
                        this.Controls.Add(t.display);
                        t.display.Visible = true;
                    }
                }*/
                calendar.orderBusyTimeDisplay(user, this);
                foreach (BusyTime u in calendar.GetBusyTime())
                {
                    if (u.scheduled.Date == dateTimePicker1.Value.Date)
                    {
                        this.Controls.Add(u.display);
                        u.display.Visible = true;
                        
                        /*int i = 0;
                        while (i < busyTimes.IndexOf(u))
                        {
                            if (busyTimes[i].scheduled.Date == u.scheduled.Date && busyTimes[i] != u)
                            {
                                if (busyTimes[i].time + busyTimes[i].duration == u.startTime)
                                {
                                    u.display.Location = new Point(busyTimes[i].display.Location.X + busyTimes[i].display.Width, busyTimes[i].display.Location.Y);
                                }
                                if (u.endTime == busyTimes[i].time)
                                {
                                    busyTimes[i].display.Location = new Point(u.display.Location.X + u.display.Width, u.display.Location.Y);
                                }
                            }
                            i++;
                        }
                        foreach (AppLogic.Task t in calendar.GetTasks())
                        {
                            if (t.scheduled.Date == u.scheduled.Date)
                            {
                                if (t.time + t.duration == u.startTime)
                                {
                                    u.display.Location = new Point(t.display.Location.X + t.display.Width, t.display.Location.Y);
                                }
                                if(u.endTime == t.time)
                                {
                                    t.display.Location = new Point(u.display.Location.X + u.display.Width, u.display.Location.Y);
                                }
                            }
                        }*/
                    }
                }
            }
            if(checkBox1.Checked == false)
            {
                foreach(BusyTime u in calendar.GetBusyTime())
                {
                    u.display.Hide();
                }
            }
            calendar.OrderTasks(this, user); //need this twice so it can be sorted before the busy times are ordered and sort them again afterwards
            calendar.UpdateTaskListControl(this);
            calendar.OrderDisplay(this, user);
        }

        private void btnWeekView_Click(object sender, EventArgs e)
        {
            var frm = new WeekViewForm(this, calendar, user);
            frm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var form = new SettingsForm(this, calendar, user);
            form.ShowDialog();
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
        public void OrderTasks(Form1 form, User u)
        {
            if (tasks.Count > 0)
            {
                AppLogic.Task lastAdded = tasks[tasks.Count - 1];
                tasks = Ahead(tasks);
                tasks = Behind(tasks);
                tasks = tasks.OrderByDescending(x => x.priority).ThenBy(x => x.earliestStartTime).ThenByDescending(x => x.duration).ToList();
                if (tasks[0].fixedTime == false)
                {
                    tasks[0].time = u.morningTime;
                }
                int j = 0;

                while (j < tasks.Count)
                {
                    if (tasks[j].GetType() != typeof(BusyTime) && tasks[j].fixedTime == false)
                    {
                        tasks = tasks[j].Ahead(tasks);
                        tasks = tasks[j].Behind(tasks);
                        tasks = tasks.OrderByDescending(x => x.priority).ThenBy(x => x.earliestStartTime).ThenByDescending(x => x.duration).ToList();
                        tasks[j].time = u.morningTime;
                        tasks[j].scheduled = DateTime.Today;
                        if (availableCheck(form, tasks[j]) == false)
                        {
                            foreach (BusyTime a in busyTimes)
                            {
                                tasks[j].time = Reschedule(tasks[j], a);
                            }
                            orderForDay(tasks[j], tasks, form);
                            if (tasks[j].time + tasks[j].duration > u.nightTime)
                            {
                                if (tasks[j].scheduled.AddDays(1).DayOfYear <= tasks[j].due.DayOfYear)
                                {
                                    tasks[j].scheduled = tasks[j].scheduled.AddDays(1);
                                    tasks[j].time = u.morningTime;
                                    orderForDay(tasks[j], tasks, form);
                                }
                                else
                                {
                                    MessageBox.Show("There is not enough time to schedule them all");
                                    tasks.Remove(lastAdded);
                                    if (j <= tasks.IndexOf(lastAdded)) // I think this works
                                    {
                                        j--;
                                    }
                                }
                                //OrderTasks(); makes it all go wrong
                            }
                            orderForDay(tasks[j], tasks, form);
                            foreach (BusyTime a in busyTimes)
                            {
                                tasks[j].time = Reschedule(tasks[j], a);
                            }
                            if (j < tasks.Count && tasks.Count != 0)
                            {
                                tasks[j].taskDescription = tasks[j].name + ", " + (tasks[j].time - (tasks[j].time % 1)) + ":" + (tasks[j].time % 1 * 60) + " - " + ((tasks[j].time + tasks[j].duration) - (tasks[j].time + tasks[j].duration % 1)) + ":" + ((tasks[j].time + tasks[j].duration) % 1 * 60) + ", " + tasks[j].scheduled.ToLongDateString();
                            }
                        }
                    }
                    j++;
                }
                //tasks = tasks.OrderByDescending(x => x.priority).ThenByDescending(x => x.earliestStartTime).ThenByDescending(x => x.duration).ToList();
                tasks = tasks.OrderBy(x => x.time).ToList();
            }


        }
        public void UpdateTaskListControl(Form1 form)
        {
            form.TaskList.Items.Clear();
            int i = 0;
            while (i < tasks.Count)
            {
                tasks[i].taskDescription = tasks[i].name + ", " + (tasks[i].time - (tasks[i].time % 1)) + ":" + (tasks[i].time % 1 * 60) + " - " + ((tasks[i].time + tasks[i].duration) - ((tasks[i].time + tasks[i].duration) % 1)) + ":" + ((tasks[i].time + tasks[i].duration) % 1 * 60) + ", " + tasks[i].scheduled.ToLongDateString();
                if (tasks[i].scheduled.DayOfYear == form.dateTimePicker1.Value.DayOfYear && tasks[i].GetType() != typeof(BusyTime))
                {
                    form.TaskList.Items.Add(tasks[i].taskDescription);

                }

                //TaskList.Items.Add(tasks[i].taskDescription);
                i++;
            }
        }
        public void OrderDisplay(Form1 form, User user)
        {
            form.BackColor = user.calendarColour;
            foreach (AppLogic.Task t in tasks)
            {
                if(t.GetType() != typeof(BusyTime))
                {
                    t.display.BackColor = user.taskColour;
                    t.display.Text = t.taskDescription;
                    foreach (object s in form.TaskList.Items)
                    {
                        if (Convert.ToString(s) == t.taskDescription)
                        {
                            AppLogic.Task previous = t;
                            if (t.time == 0)
                            {
                                t.display.Location = new Point(100, 100);
                            }
                            if(form.TaskList.Items.IndexOf(s) != 0)
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
        }
        public void SaveTasksToFile()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(@"ExampleNew.dat", FileMode.Create, FileAccess.Write);
            //foreach (Task t in tasks)
            //{
            //    formatter.Serialize(stream, t); //this doesen't work with labels so I have to disable it when lables are enabled until I can fix it and now I don't need it
            //}
            formatter.Serialize(stream, tasks);
            stream.Close();
        }
        public void saveBusyTimeToFile()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(@"NewFile.dat", FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, busyTimes);
            stream.Close();
        }
        public void SaveSettings(User user)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(@"UserPreferances.dat", FileMode.Create, FileAccess.Write);
            formatter.Serialize(stream, user);
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
        public void loadBusyTime(Form1 form, User user)
        {
            var filePath = @"NewFile.dat";
            if (!File.Exists(filePath))
            {
                var fileCreator = File.Create(filePath);
                fileCreator.Close();
            }
            else
            {
                BinaryFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(@"NewFile.dat", FileMode.Open, FileAccess.Read);
                if (stream.Length > 0)
                {
                    busyTimes = (List<BusyTime>)formatter.Deserialize(stream);
                }

                stream.Close();

            }
            orderBusyTimeDisplay(user, form);
        }
        public User rememberPreferances(Form1 form, AppLogic.User user) // hope I can get this to work
        {
            var filePath = @"UserPreferances.dat";
            if (!File.Exists(filePath))
            {
                var fileCreator = File.Create(filePath);
                fileCreator.Close();
            }
            else
            {
                BinaryFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(@"UserPreferances.dat", FileMode.Open, FileAccess.Read);
                if (stream.Length > 0)
                {
                    user = (AppLogic.User)formatter.Deserialize(stream);
                }

                stream.Close();

            }
            return user; //I had to add this so that it would work, the changes wouldn't save when it was a void
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

        public void Clearing(List<AppLogic.Task> tasks, Form1 form, User user) 
        {
            foreach (AppLogic.Task t in tasks)
            {
                if (t.due.Date < DateTime.Now.Date && t.completed == false)
                {
                    form.Controls.Remove(t.display);
                    completedTasks.Add(t);
                    tasks.Remove(t);
                    form.TaskList.Items.Remove(t.taskDescription);
                    form.Controls.Remove(t.display);
                    DeleteTasksFromFile(t);
                    SaveTasksToFile();
                    MessageBox.Show("You either missed the deadline for " + t.name + " or forgot to mark it as completed, it has been removed from your calendar"); // made sure to include messages
                    /*if (tasks.Count != 0)
                    {
                        this.OrderTasks(form, user); //thought it would take too long if they were here, might un-comment them if I realise I need them
                        this.UpdateTaskListControl(form);
                        this.OrderDisplay(form, user);
                    }*/
                }
                if (t.scheduled.DayOfYear < DateTime.Now.DayOfYear) // so it doesn't just dissaper if the time passes
                {
                    if(t.fixedTime == true)
                    {
                        t.fixedTime = false;
                        MessageBox.Show("It is passed the fixed time for " + t.name + " but before the deadline so it has been reschedueled");
                    }
                    //this.OrderTasks(form, user);
                    //this.UpdateTaskListControl(form);
                    //this.OrderDisplay(form, user);
                }
            }
            if (tasks.Count != 0)
            {
                this.OrderTasks(form, user);
                this.UpdateTaskListControl(form);
                this.OrderDisplay(form, user);
            }
        }
        public double Reschedule(AppLogic.Task t, BusyTime a)
        {

            double number = t.time;
            if(t.scheduled.Date == a.scheduled.Date)
            {
                if(t.time >= a.startTime && t.time < a.endTime)
                {
                    number = a.endTime;
                }
                if(t.time + t.duration > a.startTime && t.time + t.duration <= a.endTime)
                {
                    number = a.endTime;
                }
                if(a.startTime >= t.time && a.startTime < t.time + t.duration)
                {
                    number = a.endTime;
                }
                if(a.endTime > t.time && a.endTime <= t.time + t.duration)
                {
                    number = a.endTime;
                }

            }
            return number;
        }
        public void orderForDay(AppLogic.Task t, List<AppLogic.Task> list, Form f) // so I don't have to repeat this code
        {
            int i = 0;
            int j = list.IndexOf(t);
            while (i < list.Count && availableCheck(f, t) == false)
            {
                if (t.predecessors2.Contains(list[i].ID) && t.scheduled.Date < list[i].scheduled.Date)
                {
                    t.scheduled = list[i].scheduled;
                }
                if (t.scheduled.DayOfYear == list[i].scheduled.DayOfYear)
                {
                    t.time = list[i].time + list[i].duration;
                }
                i++;
            }
        }
        public void AvailableStartTimes(Form f)
        {
            int i = 0;
            while(i < 24)
            {

            }
        }
        public void AvailableEndTimes(Form f)
        {
            int i = 0;
            while (i < 24)
            {

            }
        }
        public bool availableCheck(Form f, AppLogic.Task t)
        {
            bool avilable = true;
            foreach(AppLogic.Task u in tasks)
            {
                if(u != t && u.scheduled.Date == t.scheduled.Date)
                {
                    if (u.fixedTime == true || (t.fixedTime == false && tasks.IndexOf(u) < tasks.IndexOf(t) && u.priority >= t.priority)) // a lot of variables are at play here
                    {
                        if (t.time >= u.time && t.time < u.time + u.duration)
                        {
                            avilable = false;
                        }
                        if (t.time + t.duration > u.time && t.time + t.duration <= u.time + u.duration)
                        {
                            avilable = false;
                        }
                        if (u.time >= t.time && u.time < t.time + t.duration)
                        {
                            avilable = false;
                        }
                        if (u.time + u.duration > t.time && u.time + u.duration <= t.time + t.duration)
                        {
                            avilable = false;
                        }
                    }
                }
                if(t.predecessors2.Contains(u.ID))
                {
                    if(t.scheduled.Date < u.scheduled.Date)
                    {
                        avilable = false;
                    }
                    if(t.scheduled.Date == u.scheduled.Date && t.time <= u.time)
                    {
                        avilable = false;
                    }
                }
            }
            foreach(BusyTime a in busyTimes)
            {
                if (t.scheduled.Date == a.scheduled.Date)
                {
                    if (t.time >= a.startTime && t.time < a.endTime)
                    {
                        avilable = false;
                    }
                    if (t.time + t.duration > a.startTime && t.time + t.duration <= a.endTime)
                    {
                        avilable = false;
                    }
                    if (a.startTime >= t.time && a.startTime < t.time + t.duration)
                    {
                        avilable = false;
                    }
                    if (a.endTime > t.time && a.endTime <= t.time + t.duration)
                    {
                        avilable = false;
                    }
                }
            }
            return avilable;
        }

        public void orderBusyTimeDisplay(User user, Form1 f) 
        {
            foreach (BusyTime u in busyTimes)
            {
                int i = 0;
                u.display.BackColor = user.busyTimeColour; 
                while (i < busyTimes.IndexOf(u))
                {
                    if (busyTimes[i].scheduled.Date == u.scheduled.Date && busyTimes[i] != u)
                    {
                        if (busyTimes[i].time + busyTimes[i].duration == u.startTime)
                        {
                            u.display.Location = new Point(busyTimes[i].display.Location.X + busyTimes[i].display.Width, busyTimes[i].display.Location.Y);
                        }
                        if (u.endTime == busyTimes[i].time)
                        {
                            busyTimes[i].display.Location = new Point(u.display.Location.X + u.display.Width, u.display.Location.Y);
                        }
                    }
                    i++;
                }
                foreach (AppLogic.Task t in tasks)  // decided to remove the need for it to take in a calendar instance and did this, hope that doesen't cause any problems
                {
                    if (t.scheduled.Date == u.scheduled.Date)
                    {
                        if (t.time + t.duration == u.startTime)
                        {
                            u.display.Location = new Point(t.display.Location.X + t.display.Width, t.display.Location.Y);
                        }
                        if (u.endTime == t.time)
                        {
                            t.display.Location = new Point(u.display.Location.X + u.display.Width, u.display.Location.Y);
                        }
                    }
                }
                f.Controls.Add(u.display);
                u.display.Visible = false;
                if(u.scheduled.Date == f.dateTimePicker1.Value.Date && f.checkBox1.Checked == true)
                {
                    u.display.Visible = true; //can't belive I went so long forgetting to put that in
                }
            }
        }
        public List<AppLogic.Task> Ahead(List<AppLogic.Task> list) // I had this in the task class form but figured it would be better here
        {
            if (list.Count != 0)
            {
                list[0].earliestFinishTime = list[0].earliestStartTime + list[0].duration;
                int i = 1;
                while (i < list.Count)
                {
                    if (list[i].predecessors2 != null && list[i].predecessors2.Count != 0)
                    {
                        foreach (Guid g in list[i].predecessors2)
                        {
                            foreach (AppLogic.Task t in list)
                            {
                                if (t.ID == g)
                                {
                                    if (list[i].earliestStartTime < t.earliestFinishTime)
                                    {
                                        list[i].earliestStartTime = t.earliestFinishTime;
                                    }
                                }
                            }

                        }
                        list[i].earliestFinishTime = list[i].earliestStartTime + list[i].duration;
                    }
                    i++;
                }
            }
            return list;
        }
        public List<AppLogic.Task> Behind(List<AppLogic.Task> list)
        {
            if (list.Count != 0)
            {
                list[list.Count - 1].latestFinishTime = list[list.Count - 1].earliestFinishTime;
                list[list.Count - 1].latestStartTime = list[list.Count - 1].latestFinishTime - list[list.Count - 1].duration;
                int i = list.Count - 2;
                while (i >= 0)
                {
                    if (list[i].successors2 != null && list[i].successors2.Count != 0)
                    {
                        foreach (Guid g in list[i].successors2)
                        {
                            foreach (AppLogic.Task t in list)
                            {
                                if (t.ID == g)
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
                        }
                    }
                    i--;
                }
            }
            return list;
        }

        public void CritPath(List<AppLogic.Task> list)
        {
            Console.WriteLine("\n    Critical Path: ");

            if (list.Count != 0)
            {
                foreach (AppLogic.Task t in list)
                {
                    if ((t.earliestFinishTime - t.latestFinishTime == 0) && (t.earliestFinishTime - t.latestStartTime == 0))
                    {
                        Console.WriteLine("{0}", t.name);
                    }

                }
                Console.WriteLine("\n\n     Total duration: {0}\n\n", list[list.Count - 1].earliestFinishTime);
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
        public BusyTime repeatOf;
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

            this.scheduled = scheduled;
            this.duration = duration;
            this.time = time;
            this.fixedTime = true;


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
            DateTime i = scheduled.AddDays(1);
            while(i.Date < repeatEndDate.Date) //goes tehrought the days from now to the end time of teh repetition
            {
                foreach(string s in daysofWeek)
                {
                    if(i.DayOfWeek.ToString() == s)
                    {
                        repeatDates.Add(i); // addes the dates if they correspond to the selected days of the week
                    }
                }
                i = i.AddDays(1);
            }
        }
        public void addRepeats(Calendar c, User u) //suprised this works
        {
            foreach(DateTime i in repeatDates)
            {
                BusyTime b = new BusyTime(name, i, false, time, duration, false); // makes a new busy time instance with similar properties for every repeat
                b.repeatOf = this; // might need this for when I make it so you can remove busy time
                b.startTime = Convert.ToInt16(b.time);
                b.endTime = b.startTime + Convert.ToInt16(b.duration);

                b.display = new Label();
                b.display.Text = "nothing schedueled: " + (b.startTime - (b.startTime % 1)) + ":" + (b.startTime % 1 * 60) + " - " + (b.endTime - (b.endTime % 1)) + ":" + ((b.endTime % 1) * 60);
                b.display.Width = 100 + 10 * Convert.ToInt32(duration);
                b.display.Height = 100;
                b.display.Location = new Point(100, 100);
                b.display.BackColor = u.busyTimeColour;
                b.display.BorderStyle = BorderStyle.Fixed3D;

                c.GetBusyTime().Add(b); // just to make it appear on different days when there's repeats, hope this doesn't cause any problems
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