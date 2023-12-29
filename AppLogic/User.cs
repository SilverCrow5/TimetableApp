using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.Serialization;

namespace timetable_app.AppLogic
{
    [Serializable] // might need this
    public class User/* : ISerializable*/
    {
        public int morningTime;
        public int nightTime;
        public Color calendarColour;
        public Color taskColour;
        public Color busyTimeColour;

        public User() //not sure what to make neccessary
        {

        }
    }
}
