using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryDesignInterface.Diagram
{

    public interface IFateDiagram : IDiagramBase, ITimeSensitive
    {
        Guid TargetObjectID { get; set; }
        List<ITrack> TrackList { get; }
        List<ITimeSeparate> TimeSeparateList { get; }
        FilterType ShowType { get; set; }
        void AutoSetSeparate(TimeSpan minSeparate);
        
    }
}
