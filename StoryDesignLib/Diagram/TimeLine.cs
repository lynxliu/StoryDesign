using StoryDesignInterface.Diagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryDesignLib.Diagram
{
    public class TimeLine : ITimeLine
    {
        public TimeSpan MinSeparate { get; set; }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }

        public TimeSpan ContinueTime { get { return EndTime - BeginTime; } }
    }
}
