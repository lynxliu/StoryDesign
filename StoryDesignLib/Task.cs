using StoryDesignInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;

namespace StoryDesignLib
{
    [SerialObjectAttribute(Name = "UnNamed Task")]
    public class Task : StoryEntityObjectBase, ITask
    {
        Guid _ParentTaskID = Guid.Empty;
        public Guid ParentTaskID { get { return _ParentTaskID; } set { _ParentTaskID = value; } }


        public double Result { get; set; }

        public override ICopySupportObject Clone()
        {
            var o = new Task() { Result=Result,ParentTaskID=ParentTaskID, };

            LoadData(o);
            return o;
        }
    }
}
