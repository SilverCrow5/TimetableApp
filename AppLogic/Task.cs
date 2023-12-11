using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace timetable_app.AppLogic
{
    [Serializable]
    public class Task : ISerializable
    {
        public string name;
        public double time;
        public double end;
        public DateTime scheduled;
        public bool completed;
        [NonSerialized]
        public Label display;
        public double duration;
        public string taskDescription;
        public int priority;
        public DateTime due;
        public int displayTime;
        public List<Task> predecessors;
        public List<Task> successors;
        public List<Guid> predecessors2;
        public List<Guid> successors2;
        public string details;
        public double estimatedStartTime;
        public double latestStartTime;
        public double estimatedFinishTime;
        public double latestFinishTime;
        public Guid ID;
        public Task(string name, string details, DateTime scheduled, bool completed, double time, double duration, int priority, DateTime due)
        {

            this.name = name;
            this.scheduled = scheduled;
            this.completed = false;
            this.duration = duration;
            this.time = time;
            display = new Label();
            display.Text = name + ", " + (time - (time % 1)) + ":" + (time % 1 * 60) + " - " + (end - (end % 1)) + ":" + ((end % 1) * 60) + ", " + scheduled.ToLongDateString();
            display.Width = 100 + (10 * Convert.ToInt32(duration)); //change this based on length of task
            display.Height = 100;

            end = time + duration;


            display.Location = new Point(100, 100);
            display.BackColor = Color.AliceBlue;
            display.BorderStyle = BorderStyle.Fixed3D;
            taskDescription = name + ", " + (time - (time % 1)) + ":" + (time % 1 * 60) + " - " + (end - (end % 1)) + ":" + ((end % 1) * 60) + ", " + scheduled.ToLongDateString();
            this.taskDescription = taskDescription;
            this.priority = priority;
            this.due = due;
            this.predecessors = new List<Task>();
            this.successors = new List<Task>();
            this.predecessors2 = new List<Guid>();
            this.successors2 = new List<Guid>();
            this.details = details;
            
            this.ID = Guid.NewGuid();
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
            predecessors = new List<Task>();
            successors = new List<Task>();
            predecessors2 = new List<Guid>();
            successors2 = new List<Guid>();

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
            information.AddValue("Successors", successors);
            information.AddValue("Predecessors", predecessors);
            information.AddValue("Successors2", successors2);
            information.AddValue("Predecessors2", predecessors2);


        }

        public List<Task> Ahead(List<Task> list)
        {
            if (list.Count != 0)
            {
                list[0].estimatedFinishTime = list[0].estimatedStartTime + list[0].duration;
                int i = 1;
                while (i < list.Count)
                {
                    if (list[i].predecessors != null && list[i].predecessors.Count != 0)
                    {
                        foreach (Task t in list[i].predecessors)
                        {
                            if (list[i].estimatedStartTime < t.estimatedFinishTime)
                            {
                                list[i].estimatedStartTime = t.estimatedFinishTime;
                            }
                        }
                        list[i].estimatedFinishTime = list[i].estimatedStartTime + list[i].duration;
                    }
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
                    if (list[i].successors != null && list[i].successors.Count != 0)
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
            Console.WriteLine("\n    Critical Path: ");

            if (list.Count != 0)
            {
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
}
