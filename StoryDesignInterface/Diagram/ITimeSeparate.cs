using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryDesignInterface.Diagram
{
    public interface ITimeSeparate:ITimeSensitive
    {
        string Description { get; set; }
        bool IsValid(TimeSpan minSeparate);
        bool IsEnable { get; set; }
    }
}
