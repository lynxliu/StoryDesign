using StoryDesignInterface.Diagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;
using StoryDesignInterface;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;

namespace StoryDesignLib.Diagram
{
    public class FateDiagram :DiagramBase,  IFateDiagram
    {
        public Guid TargetObjectID { get; set; }
        FilterType _ShowType = FilterType.NoFilter;
        public FilterType ShowType { get { return _ShowType; } set { _ShowType = value; } }
        List<ITrack> _TrackList = new List<ITrack>();
        public List<ITrack> TrackList { get { return _TrackList; } }

        List<ITimeSeparate> _TimeSeparateList = new List<ITimeSeparate>();
        public List<ITimeSeparate> TimeSeparateList { get { return _TimeSeparateList; } }

        [JsonIgnore]
        public DateTime BeginTime { get {
                DateTime? time=null ;
                foreach(var t in TrackList)
                {
                    foreach(var e in t.EntityList)
                    {
                        if(time == null||e.BeginTime<time.Value)
                            time = e.BeginTime;
                    }
                }
                if (time == null)
                    time = new DateTime();
                return time.Value;
            } set { } }

        [JsonIgnore]
        public DateTime EndTime { get {
                DateTime? time = null;
                foreach (var t in TrackList)
                {
                    foreach (var e in t.EntityList)
                    {
                        if (time == null || e.EndTime > time.Value)
                            time = e.EndTime;
                    }
                }
                if (time == null)
                    time = new DateTime();
                return time.Value;
            } set { } }

        public TimeSpan ContinueTime { get { return EndTime - BeginTime; } }

        TimeSeparate ProcessSub(ITimeSeparate separate,DateTime begin,DateTime end)//sub
        {
            if (begin <= separate.BeginTime && end >= separate.EndTime)
            {
                separate.IsEnable = false;
                return null;
            }
            if (begin >= separate.EndTime || end <= separate.BeginTime)
            {
                separate.IsEnable = true;
                return null;
            }
            if (begin >= separate.BeginTime && end <= separate.EndTime)
            {
                var rs= new TimeSeparate() { BeginTime = end, EndTime = separate.EndTime };
                separate.EndTime = begin;
                return rs;
            }
            if (begin > separate.BeginTime )
                separate.BeginTime = begin;
            if (end < separate.EndTime)
                separate.EndTime = end;
            return null;
        }

        void ProcessList(List<ITimeSeparate> l,DateTime begin,DateTime end)//sub
        {
            List<TimeSeparate> sl = new List<TimeSeparate>();
            l.OrderBy(v=>v.BeginTime).ToList().ForEach(v =>
            {
                var sp = ProcessSub(v, begin, end);
                if (sp != null)
                {
                    sl.Add(sp);
                }
            });
            sl.ForEach(v => l.Add(v));
            l.RemoveAll(v => v.IsEnable == false);
        }
        public void AutoSetSeparate(TimeSpan minSeparate)
        {
            List<Tuple<DateTime, DateTime>> entityList = new List<Tuple<DateTime, DateTime>>();
            TrackList.ForEach(v =>
            {
                v.EntityList.ForEach(e =>
                {
                    entityList.Add(new Tuple<DateTime, DateTime>(e.BeginTime, e.EndTime));
                });
            });

            TimeSeparateList.Clear();
            TimeSeparateList.Add(new TimeSeparate() { BeginTime = BeginTime, EndTime = EndTime });
            entityList.ForEach(v =>
            {
                ProcessList(TimeSeparateList, v.Item1, v.Item2);
            });
            TimeSeparateList.RemoveAll(v => v.IsValid(minSeparate) == false);
        }

        public override ICopySupportObject Clone()
        {
            var o = new FateDiagram() { BeginTime = BeginTime, EndTime = EndTime, 
                Memo = Memo, Name = Name,   };
            NodeList.ForEach(v => o.NodeList.Add(v.Clone() as INode));
            NoteList.ForEach(v => o.NoteList.Add(v.Clone() as INote));
            ConnectionList.ForEach(v => o.ConnectionList.Add(v.Clone() as IConnection));

            TimeSeparateList.ForEach(v => o.TimeSeparateList.Add(new TimeSeparate() {BeginTime=v.BeginTime,EndTime=v.EndTime  }));
            TrackList.ForEach(v => o.TrackList.Add(v.Clone() as ITrack));
            return o;
        }


    }
}
