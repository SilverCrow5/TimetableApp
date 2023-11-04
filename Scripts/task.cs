using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace timetable_app.Scripts
{
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
}
