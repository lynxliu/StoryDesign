using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryDesignInterface.Diagram
{
    public interface ITimeDiagram : IDiagramBase, ITimeSensitive
    {
        //ITimeLine TargetTimeLine { get; set; }
        TimeSpan MinSeparate { get; set; }
        List<ITrack> TrackList { get; }
        List<ITimeSeparate> TimeSeparateList { get; }

    }
}
