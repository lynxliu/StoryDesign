using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryDesignInterface.Diagram
{
    public interface ITimeLine:ITimeSensitive
    {
        TimeSpan MinSeparate { get; set; }

    }
}
