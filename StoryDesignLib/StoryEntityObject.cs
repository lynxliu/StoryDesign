using StoryDesignInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;
using System.Reflection;
using StoryDesignInterface.Diagram;
using StoryDesignLib.Diagram;

namespace StoryDesignLib
{
    public abstract class StoryEntityObjectBase : IStoryEntityObject
    {
        public string Icon { get; set; }
        public DateTime CurrentTime { get; set; }

        Guid _ObjectID = Guid.NewGuid();
        public Guid ObjectID { get { return _ObjectID; } set { _ObjectID = value; } }

        DateTime _BeginTime = DateTime.MinValue;
        public DateTime BeginTime { get { return _BeginTime; } set { _BeginTime = value; } }

        DateTime _EndTime = DateTime.MaxValue;
        public DateTime EndTime { get { return _EndTime; } set { _EndTime = value; } }

        public TimeSpan ContinueTime { get { return EndTime - BeginTime; } }

        public bool HaveNote { get { if (NoteList.Count > 0) return true; return false; } }
        List<INote> _NoteList = new List<INote>();
        public List<INote> NoteList { get { return _NoteList; } }

        string _Key;
        public string Key
        {
            get
            {
                if (string.IsNullOrEmpty(_Key))
                {
                    var al = GetType().GetTypeInfo().GetCustomAttributes(typeof(SerialObjectAttribute), true);
                    if (al != null && al.Count() > 0)
                    {
                        var a = al.FirstOrDefault() as SerialObjectAttribute;
                        if (a != null && !string.IsNullOrEmpty(a.Key))
                            _Key = a.Key;

                        else
                            _Key = GetType().FullName;
                    }
                }
                return _Key;
            }
        }

        List<Parameter> _ParameterList = new List<Parameter>();
        public List<Parameter> ParameterList { get { return _ParameterList; } }

        protected string _Name;
        [ParameterOperation]
        public virtual string Name
        {
            get
            {
                if (string.IsNullOrEmpty(_Name))
                {
                    var al = GetType().GetTypeInfo().GetCustomAttributes(typeof(SerialObjectAttribute), true);
                    if (al != null && al.Count() > 0)
                    {
                        var a = al.FirstOrDefault() as SerialObjectAttribute;
                        if (a != null && !string.IsNullOrEmpty(a.Name))
                            _Name = a.Name;

                        else
                            _Name = GetType().Name;
                    }
                }
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        protected string _Memo;
        [ParameterOperation]
        public virtual string Memo
        {
            get
            {
                if (string.IsNullOrEmpty(_Memo))
                {
                    var al = GetType().GetTypeInfo().GetCustomAttributes(typeof(SerialObjectAttribute), true);
                    if (al != null && al.Count() > 0)
                    {
                        var a = al.FirstOrDefault() as SerialObjectAttribute;
                        if (a != null && !string.IsNullOrEmpty(a.Memo))
                            _Memo = a.Memo;
                    }
                }
                return _Memo;
            }
            set { _Memo = value; }
        }
        
        //public string Description { get; set; }

        //List<IStructureDiagram> _StructureDiagramList = new List<IStructureDiagram>();
        //public List<IStructureDiagram> StructureDiagramList { get { return _StructureDiagramList; } }

        IFateDiagram _TargetFate;
        public IFateDiagram TargetFate
        {
            get
            {
                if (_TargetFate == null)
                    _TargetFate = new FateDiagram() { TargetObjectID=ObjectID};
                return _TargetFate;
            }
                set
            {
                _TargetFate = value;
            }
        }

        List<IMeasure> _MeasureList = new List<IMeasure>();
        public List<IMeasure> MeasureList { get { return _MeasureList; } }

        double _Grade=-1;
        public double Grade { get { if (_Grade < 0) _Grade = ContinueTime.TotalMilliseconds / Story.CurrentStory.ContinueTime.TotalMilliseconds; return _Grade; }
            set {
                if (value <= 0) _Grade = ContinueTime.TotalMilliseconds / Story.CurrentStory.ContinueTime.TotalMilliseconds;
                if (value >= 1) _Grade = 1;
                _Grade = value;
            } }

        public virtual string GetSerialParameter()
        {
            SaveToParameterList();
            return CommonLib.CommonProc.ConvertObjectToString(ParameterList);
        }

        public virtual void DeserialParameter(string parameterSerialString)
        {
            var l = CommonLib.CommonProc.ConvertStringToObject<List<Parameter>>(parameterSerialString);
            if (l != null)
                l.ForEach(v => ParameterList.Add(v));
            LoadFromParameterList();
        }
        public virtual void LoadFromParameterList()
        {
            Parameter.LoadInfoFromParameterList(this, ParameterList);
        }

        public virtual void SaveToParameterList()
        {
            ParameterList.Clear();
            ParameterList.AddRange(Parameter.SaveInfoToParameterList(this));
        }
        public abstract ICopySupportObject Clone();
        protected virtual void LoadData(IStoryEntityObject target)
        {
            target.Name = Name;
            target.Memo = Memo;

            target.BeginTime = BeginTime;
            target.EndTime =EndTime;
            ParameterList.ForEach(v => target.ParameterList.Add(v.GetData()));
            NoteList.ForEach(v => target.NoteList.Add(v.Clone() as INote));
            //KeyWordList.ForEach(v => target.KeyWordList.Add(v.ToString()));
            MeasureList.ForEach(v => target.MeasureList.Add(v.Clone() as IMeasure));
        }
        EntityType GetEntityType(object o)
        {
            if (o == null) return EntityType.Others;
            if (o as IActor != null)
                return EntityType.Actor;
            if (o as IEvent != null)
                return EntityType.Event;
            if (o as IGroup != null)
                return EntityType.Group;
            if (o as IStuff != null)
                return EntityType.Stuff;
            if (o as ILocation != null)
                return EntityType.Location;
            if (o as ITask != null)
                return EntityType.Task;
            return EntityType.Others;
            
        }
        public List<FateEntity> GetFate(IStory story)
        {
            List<FateEntity> rl = new List<FateEntity>();
            var l = story.GetFate(this);
            //if(filterType== FilterType.Type)
            //{
            //    l = l.Where(v => v.Item1.GetType().Name == filter).ToList();
            //}
            //if(filterType== FilterType.Name)
            //{
            //    l = l.Where(v => v.Item1.Name == filter).ToList();
            //}
            l.ForEach(v =>
            {
                var fateEntity = new FateEntity()
                {
                    Name = v.Item1.Name,
                    FateEntityType = GetEntityType(v.Item1),
                    Description = v.Item2.Memo,
                    BeginTime =CommonProc.GetMaxTime( v.Item2.BeginTime,v.Item1.BeginTime),
                    EndTime = CommonProc.GetMinTime(v.Item2.EndTime, v.Item1.EndTime),
                    RelationType = v.Item2.RelationType,
                    RelationDescription = v.Item2.Memo
                };
                //fateEntity.KeywordList.AddRange(v.Item1.KeyWordList);
                rl.Add(fateEntity);
            });
            return rl;
        }

        public void RefreshFateDiagram()
        {
            if (TargetFate == null)
                TargetFate = new FateDiagram() { Name="Fate of "+Name};
            var l = GetFate(Story.CurrentStory);
            TargetFate.TrackList.Clear();
            if (TargetFate.ShowType== FilterType.EntityName)
            {
                var nl = l.Select(v => v.Name).Distinct().ToList();
                nl.ForEach(n =>
                {
                    var track = new Track() { Name = n };
                    foreach (var e in l.Where(v => v.Name == n))
                        track.EntityList.Add(e);
                    TargetFate.TrackList.Add(track);
                });
                return;
            }
            if (TargetFate.ShowType == FilterType.EntityType)
            {
                var nl = l.Select(v => v.FateEntityType).Distinct().ToList();
                nl.ForEach(n =>
                {
                    var track = new Track() { Name = n.ToString() };
                    foreach (var e in l.Where(v => v.FateEntityType == n))
                        track.EntityList.Add(e);
                    TargetFate.TrackList.Add(track);
                });
                return;
            }

            if (TargetFate.ShowType == FilterType.RelationType)
            {
                var nl = l.Select(v => v.RelationType).Distinct().ToList();
                nl.ForEach(n =>
                {
                    var track = new Track() { Name = n.ToString() };
                    foreach (var e in l.Where(v => v.RelationType == n))
                        track.EntityList.Add(e);
                    TargetFate.TrackList.Add(track);
                });
                return;
            }
            if (TargetFate.ShowType == FilterType.NoFilter)
            {
                foreach (var e in l)
                {
                    var t = TargetFate.TrackList.FirstOrDefault(v => v.Name == e.Name);
                    if (t != null&& t.EntityList.Count > 0 && t.EntityList.FirstOrDefault().FateEntityType == e.FateEntityType)
                    {
                        t.EntityList.Add(e);
                    }
                    else
                    {
                        t = new Track() { Name = e.Name };
                        t.EntityList.Add(e);
                    }
                    TargetFate.TrackList.Add(t);
                }

            }
            //if (TargetFate.ShowType == FilterType.EntityKeyword)
            //{
            //    var sl = new List<string>();
            //    l.ForEach(v => sl.AddRange(v.KeywordList));
            //    sl = sl.Distinct().ToList();

            //    sl.ForEach(n =>
            //    {
            //        var track = new Track() { Name = n.ToString() };
            //        foreach (var e in l.Where(v => v.KeywordList.Contains(n) ))
            //            track.EntityList.Add(e);
            //        TargetFate.TrackList.Add(track);
            //    });
            //    return;
            //}

            
        }
    }
}
