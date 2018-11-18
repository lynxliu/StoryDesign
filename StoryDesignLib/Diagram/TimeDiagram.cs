using StoryDesignInterface.Diagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;
using StoryDesignInterface;

namespace StoryDesignLib.Diagram
{
    public class TimeDiagram : DiagramBase, ITimeDiagram
    {
        TimeSpan _MinSeparate = TimeSpan.FromHours(1);
        public TimeSpan MinSeparate { get { return _MinSeparate; } set { _MinSeparate = value; } }

        List<ITrack> _TrackList = new List<ITrack>();
        public List<ITrack> TrackList { get { return _TrackList; } }

        List<ITimeSeparate> _TimeSeparateList = new List<ITimeSeparate>();
        public List<ITimeSeparate> TimeSeparateList { get { return _TimeSeparateList; } }

        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }

        public TimeSpan ContinueTime { get { return EndTime-BeginTime; } }

        public override ICopySupportObject Clone()
        {
            var o = new TimeDiagram()
            {
                BeginTime = BeginTime,
                EndTime = EndTime,
                Height = Height,
                Memo = Memo,
                Name = Name,
                Width = Width,
            };
            NodeList.ForEach(v => o.NodeList.Add(v.Clone() as INode));
            NoteList.ForEach(v => o.NoteList.Add(v.Clone() as INote));
            ConnectionList.ForEach(v => o.ConnectionList.Add(v.Clone() as IConnection));

            TimeSeparateList.ForEach(v => o.TimeSeparateList.Add(new TimeSeparate() { Description = v.Description, BeginTime = v.BeginTime, EndTime = v.EndTime }));
            TrackList.ForEach(v => o.TrackList.Add(v.Clone() as ITrack));
            return o;
        }
    }
}
