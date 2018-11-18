using StoryDesignInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using CommonLib;
using StoryDesignInterface.Diagram;
using StoryDesignInterface.Express;
using StoryDesignLib.Diagram;
using Newtonsoft.Json;
using StoryDesignLib.Express;
using Windows.Storage;

namespace StoryDesignLib
{
    public class Story : IStory
    {
        string _Name = "UnNamed Story"+DateTime.Now.ToString("yy-MM-dd-hh-mm");
        public string Name { get { return _Name; } set { _Name = value; } }
        public string Memo { get; set; }
        public virtual string AbstractInfo
        {
            get
            {
                string s = "";
                if (!string.IsNullOrEmpty(Name))
                    s += Name + "( from " + BeginTime.ToString() + " to " + EndTime.ToString() + ")";

                if (!string.IsNullOrEmpty(Memo))
                    if (string.IsNullOrEmpty(s))
                        s = Memo;
                    else
                        s += ":" + Memo;

                return s;
            }
        }

        DateTime _BeginTime = DateTime.MinValue;
        public DateTime BeginTime { get { return _BeginTime; } set { _BeginTime = value; } }

        DateTime _EndTime = DateTime.MaxValue;
        public DateTime EndTime { get { return _EndTime; } set { _EndTime = value; } }

        public TimeSpan ContinueTime
        {
            get { return EndTime - BeginTime; }
        }

        public static IStory CurrentStory { get; set; }
        public static void CreateStory()
        {
            CurrentStory = new Story();
        }
        //public static void OpenStory(Action<StorageFile> callback)
        //{
        //    CommonProc.LoadFromFile<Story>((f,s) => { CurrentStory = s; if (callback != null) callback(f); });
        //}
        //public static void SaveStory(Action<StorageFile> callback)
        //{
        //    CommonProc.SaveToFile((f)=> { callback(f); },CurrentStory);
        //}
        List<IActor> _ActorList = new List<IActor>();
        public List<IActor> ActorList { get { return _ActorList; } }

        List<IStuff> _StuffList = new List<IStuff>();
        public List<IStuff> StuffList { get { return _StuffList; } }

        List<ITask> _TaskList = new List<ITask>();
        public List<ITask> TaskList { get { return _TaskList; } }

        List<ILocation> _LocationList = new List<ILocation>();
        public List<ILocation> LocationList { get { return _LocationList; } }


        List<IRelation> _RelationList = new List<IRelation>();
        public List<IRelation> RelationList { get { return _RelationList; } }

        List<IGroup> _GroupList = new List<IGroup>();
        public List<IGroup> GroupList { get { return _GroupList; } }

        List<IEvent> _EventList = new List<IEvent>();
        public List<IEvent> EventList { get { return _EventList; } }

        List<IExpress> _ExpressList = new List<IExpress>();
        public List<IExpress> ExpressList { get { return _ExpressList; } }

        public string Author { get; set; }

        Version _StoryVersion = new Version(1, 0);
        public Version StoryVersion { get { return _StoryVersion; } set { _StoryVersion = value; } }

        DateTime _CreateTime = DateTime.Now;
        public DateTime CreateTime { get { return _CreateTime; } set { _CreateTime = value; } }



        List<INote> _NoteList = new List<INote>();
        public List<INote> NoteList { get { return _NoteList; } }

        List<IStructureDiagram> _StructureDiagramList = new List<IStructureDiagram>();
        public List<IStructureDiagram> StructureDiagramList { get { return _StructureDiagramList; } }

        public bool HaveNote { get { if (NoteList.Count > 0) return true;return false; } }

        //List<IRole> _RoleList = new List<IRole>();
        //public List<IRole> RoleList { get { return _RoleList; } }

        public ICopySupportObject Clone()
        {
            var o = new Story() { Name=Name,Memo=Memo,BeginTime=BeginTime,EndTime=EndTime,Author=Author,StoryVersion=StoryVersion};
            //o.CurrentUniverse = CurrentUniverse.Clone() as IUniverse;
            ActorList.ForEach(v => o.ActorList.Add(v.Clone() as IActor));
            StuffList.ForEach(v => o.StuffList.Add(v.Clone() as IStuff));
            ExpressList.ForEach(v => o.ExpressList.Add(v.Clone() as IExpress));
            NoteList.ForEach(v => o.NoteList.Add(v.Clone() as INote));
            EventList.ForEach(v => o.EventList.Add(v.Clone() as IEvent));
            RelationList.ForEach(v => o.RelationList.Add(v.Clone() as IRelation));
            TaskList.ForEach(v => o.TaskList.Add(v.Clone() as ITask));
            GroupList.ForEach(v => o.GroupList.Add(v.Clone() as IGroup));
            StructureDiagramList.ForEach(v => o.StructureDiagramList.Add(v.Clone() as IStructureDiagram));

            return o;
        }
        T GetEntityByID<T>(Guid id) where T : IStoryEntityObject
        {
            
            if (typeof(IActor).IsAssignableFrom(typeof(T)))
            {
                var r=  ActorList.FirstOrDefault(v => v.ObjectID == id);
                if (r == null) return default(T);
                return (T)r;
            }
            if (typeof(IEvent).IsAssignableFrom(typeof(T)))
            {
                var r = EventList.FirstOrDefault(v => v.ObjectID == id);
                if (r == null) return default(T);
                return (T)r;
            }
            if (typeof(IGroup).IsAssignableFrom(typeof(T)))
            {
                var r = GroupList.FirstOrDefault(v => v.ObjectID == id);
                if (r == null) return default(T);
                return (T)r;
            }
            if (typeof(IStuff).IsAssignableFrom(typeof(T)))
            {
                var r = StuffList.FirstOrDefault(v => v.ObjectID == id);
                if (r == null) return default(T);
                return (T)r;
            }
            if (typeof(ITask).IsAssignableFrom(typeof(T)))
            {
                var r = TaskList.FirstOrDefault(v => v.ObjectID == id);
                if (r == null) return default(T);
                return (T)r;
            }
            if (typeof(ILocation).IsAssignableFrom(typeof(T)))
            {
                var r = LocationList.FirstOrDefault(v => v.ObjectID == id);
                if (r == null) return default(T);
                return (T)r;
            }
            return default(T);
        }
        public void AddEntity(IStoryEntityObject obj) 
        {
            if (obj == null) return;
            if ((obj as IActor)!=null)
            {
                ActorList.Add(obj as IActor);
                return;
            }
            if ((obj as IEvent) != null)
            {
                EventList.Add(obj as IEvent);
                return;
            }
            if ((obj as IStuff) != null)
            {
                StuffList.Add(obj as IStuff);
                return;
            }
            if ((obj as IGroup) != null)
            {
                GroupList.Add(obj as IGroup);
                return;
            }
            if ((obj as ITask) != null)
            {
               TaskList.Add(obj as ITask);
                return;
            }
            if ((obj as ILocation) != null)
            {
                LocationList.Add(obj as ILocation);
                return;
            }
        }
        public void RemoveEntity(IStoryEntityObject obj)
        {
            if (obj == null) return;
            if ((obj as IActor) != null)
            {
                ActorList.Remove(obj as IActor);
                return;
            }
            if ((obj as IEvent) != null)
            {
                EventList.Remove(obj as IEvent);
                return;
            }
            if ((obj as IStuff) != null)
            {
                StuffList.Remove(obj as IStuff);
                return;
            }
            if ((obj as IGroup) != null)
            {
                GroupList.Remove(obj as IGroup);
                return;
            }
            if ((obj as ITask) != null)
            {
                TaskList.Remove(obj as ITask);
                return;
            }
            if ((obj as ILocation) != null)
            {
                LocationList.Remove(obj as ILocation);
                return;
            }
        }

        public IStoryEntityObject GetEntityByID(Guid id)
        {
            if (id == Guid.Empty) return null;
            IStoryEntityObject o = GetEntityByID<IActor>(id);
            if (o != null) return o;
            o = GetEntityByID<IEvent>(id);
            if (o != null) return o;
            o = GetEntityByID<IGroup>(id);
            if (o != null) return o;
            o = GetEntityByID<ITask>(id);
            if (o != null) return o;
            o = GetEntityByID<IStuff>(id);
            if (o != null) return o;
            o = GetEntityByID<ILocation>(id);
            if (o != null) return o;
            return null;
        }
        public List<Tuple<IStoryEntityObject, IRelation>> GetFate(ISubject subject)
        {
            List<Tuple<IStoryEntityObject, IRelation>> l = new List<Tuple<IStoryEntityObject, IRelation>>();
            var rl = RelationList.Where(r => r.IsRelationAbout(subject.ObjectID));
            foreach(var r in rl)
            {
                Guid sid;
                if (r.SourceID == subject.ObjectID)
                    sid = r.TargetID;
                else
                    sid = r.SourceID;
                var o = GetEntityByID(sid);
                if (o != null)
                    l.Add(new Tuple<IStoryEntityObject, IRelation>(o, r));
            }
            return l;
        }
        public List<Tuple<IRelation, IStoryEntityObject, IStoryEntityObject>> GetRelativeFate(List<ISubject> subjectList)
        {
            List<Tuple<IRelation, IStoryEntityObject, IStoryEntityObject>> l = new List<Tuple<IRelation, IStoryEntityObject, IStoryEntityObject>>();
            foreach (var subject in subjectList)
            {
                foreach (var r in RelationList.Where(v=> v.SourceID==subject.ObjectID))
                {
                    var target = subjectList.FirstOrDefault(v => v.ObjectID == r.TargetID);
                    if (target != null)
                        l.Add(
                            new Tuple<IRelation, IStoryEntityObject, IStoryEntityObject>
                            (r, GetEntityByID(subject.ObjectID), GetEntityByID(target.ObjectID)));
                   
                }
            }
            return l;
        }
        public List<T> GetRelatedEntityList<T>(Guid id)
        {
            var r = new List<T>();
            var trl = RelationList.Where(v => v.SourceID == id ).ToList();
            var srl = RelationList.Where(v => v.TargetID == id).ToList();
            trl.ForEach(v =>
            {
                var o = GetEntityByID(v.TargetID);
                if (o != null && (T)o != null)
                    r.Add((T)o);
            });
            srl.ForEach(v =>
            {
                var o = GetEntityByID(v.SourceID);
                if (o != null && (T)o != null)
                    r.Add((T)o);
            });
            return r;
        }

        //void AddKeyWords(List<IDescriptionObject> source,List<string> kl)
        //{
        //    source.ForEach(v =>
        //    {
        //        v.KeyWordList.ForEach(s =>
        //        {
        //            if (!kl.Contains(s)) kl.Add(s);
        //        });
        //    });
        //}
        //public List<string> GetAllKeyWords()
        //{
        //    List<string> l = new List<string>(KeyWordList) ;
        //    AddKeyWords(ActorList.Cast<IDescriptionObject>().ToList(), l);
        //    AddKeyWords(EventList.Cast<IDescriptionObject>().ToList(), l);
        //    AddKeyWords(GroupList.Cast<IDescriptionObject>().ToList(), l);
        //    AddKeyWords(TaskList.Cast<IDescriptionObject>().ToList(), l);
        //    AddKeyWords(StuffList.Cast<IDescriptionObject>().ToList(), l);
        //    AddKeyWords(RelationList.Cast<IDescriptionObject>().ToList(), l);

        //    return l;
        //}
        void AddMeasureName(List<IStoryEntityObject> source, List<string> kl)
        {
            source.ForEach(v =>
            {
                v.MeasureList.ForEach(s =>
                {
                    if (!string.IsNullOrEmpty(s.Name) && !kl.Contains(s.Name)) kl.Add(s.Name);
                });
            });
        }
        void AddParameterName(List<IStoryEntityObject> source, List<string> kl)
        {
            source.ForEach(v =>
            {
                v.ParameterList.ForEach(s =>
                {
                    if (!string.IsNullOrEmpty(s.Name)&&!kl.Contains(s.Name)) kl.Add(s.Name);
                });
            });
        }
        public List<string> GetAllMeasureName()
        {
            List<string> l = new List<string>();
            AddMeasureName(ActorList.Cast<IStoryEntityObject>().ToList(), l);
            AddMeasureName(EventList.Cast<IStoryEntityObject>().ToList(), l);
            AddMeasureName(GroupList.Cast<IStoryEntityObject>().ToList(), l);
            AddMeasureName(TaskList.Cast<IStoryEntityObject>().ToList(), l);
            AddMeasureName(StuffList.Cast<IStoryEntityObject>().ToList(), l);
            AddMeasureName(RelationList.Cast<IStoryEntityObject>().ToList(), l);

            return l;
        }

        public List<string> GetAllParameterName()
        {
            List<string> l = new List<string>();
            AddParameterName(ActorList.Cast<IStoryEntityObject>().ToList(), l);
            AddParameterName(EventList.Cast<IStoryEntityObject>().ToList(), l);
            AddParameterName(GroupList.Cast<IStoryEntityObject>().ToList(), l);
            AddParameterName(TaskList.Cast<IStoryEntityObject>().ToList(), l);
            AddParameterName(StuffList.Cast<IStoryEntityObject>().ToList(), l);
            AddParameterName(RelationList.Cast<IStoryEntityObject>().ToList(), l);

            return l;
        }
        public List<IStoryEntityObject> GetAllEntityList()
        {
            var el = new List<IStoryEntityObject>();
            ActorList.ForEach(v => el.Add(v));
            GroupList.ForEach(v => el.Add(v));
            EventList.ForEach(v => el.Add(v));
            LocationList.ForEach(v => el.Add(v));
            StuffList.ForEach(v => el.Add(v));
            TaskList.ForEach(v => el.Add(v));
            return el;
        }
        //public List<IExpressObject> GetAllExpressObjectList()
        //{
        //    List<IExpressObject> ol = new List<IExpressObject>();
        //    ActorList.ForEach(v =>
        //    {
        //        ol.Add(new ExpressObject()
        //        {
        //             ExpressType= "Actor", Name=v.Name,Memo=v.Memo, TargetObjectID=v.ObjectID
        //        });
        //    });
        //    EventList.ForEach(v =>
        //    {
        //        ol.Add(new ExpressObject()
        //        {
        //            ExpressType = "Event",
        //            Name = v.Name,
        //            Memo = v.Memo,
        //            TargetObjectID = v.ObjectID
        //        });
        //    });
        //    GroupList.ForEach(v =>
        //    {
        //        ol.Add(new ExpressObject()
        //        {
        //            ExpressType = "Group",
        //            Name = v.Name,
        //            Memo = v.Memo,
        //            TargetObjectID = v.ObjectID
        //        });
        //    });
        //    LocationList.ForEach(v =>
        //    {
        //        ol.Add(new ExpressObject()
        //        {
        //            ExpressType = "Location",
        //            Name = v.Name,
        //            Memo = v.Memo,
        //            TargetObjectID = v.ObjectID
        //        });
        //    });
        //    StuffList.ForEach(v =>
        //    {
        //        ol.Add(new ExpressObject()
        //        {
        //            ExpressType = "Stuff",
        //            Name = v.Name,
        //            Memo = v.Memo,
        //            TargetObjectID = v.ObjectID
        //        });
        //    });
        //    TaskList.ForEach(v =>
        //    {
        //        ol.Add(new ExpressObject()
        //        {
        //            ExpressType = "Task",
        //            Name = v.Name,
        //            Memo = v.Memo,
        //            TargetObjectID = v.ObjectID
        //        });
        //    });
        //    return ol;
        //}
        public static List<TimeStep> GetTimeStepList()
        {

            return CommonProc.GetValueList<TimeStep>();
        }
        public static TimeSpan? GetTimeSpan(TimeStep time)
        {
            if (time == TimeStep.Century)
                return TimeSpan.FromDays(36524);
            if (time == TimeStep.Day)
                return TimeSpan.FromDays(1);
            if (time == TimeStep.Decade)
                return TimeSpan.FromDays(3652);
            if (time == TimeStep.FifteenMinutes)
                return TimeSpan.FromMinutes(15);
            if (time == TimeStep.FiveHours)
                return TimeSpan.FromHours(5);
            if (time == TimeStep.FiveMinutes)
                return TimeSpan.FromMinutes(5);
            if (time == TimeStep.FiveSeconds)
                return TimeSpan.FromSeconds(5);
            if (time == TimeStep.FiveYears)
                return TimeSpan.FromDays(1826);
            if (time == TimeStep.HalfCentury)
                return TimeSpan.FromDays(18262);
            if (time == TimeStep.HalfDay)
                return TimeSpan.FromDays(0.5);
            if (time == TimeStep.HalfHour)
                return TimeSpan.FromMinutes(30);
            if (time == TimeStep.HalfMinute)
                return TimeSpan.FromSeconds(30);
            if (time == TimeStep.HalfMonth)
                return TimeSpan.FromDays(15);
            if (time == TimeStep.HalfYear)
                return TimeSpan.FromDays(183);
            if (time == TimeStep.Hour)
                return TimeSpan.FromHours(1);
            if (time == TimeStep.Minute)
                return TimeSpan.FromMinutes(1);
            if (time == TimeStep.Month)
                return TimeSpan.FromDays(30);
            if (time == TimeStep.Season)
                return TimeSpan.FromDays(91);
            if (time == TimeStep.Second)
                return TimeSpan.FromSeconds(1);
            if (time == TimeStep.TenSeconds)
                return TimeSpan.FromSeconds(10);
            if (time == TimeStep.TwoDays)
                return TimeSpan.FromDays(2);
            if (time == TimeStep.TwoDecades)
                return TimeSpan.FromDays(7305);
            if (time == TimeStep.TwoHours)
                return TimeSpan.FromHours(2);
            if (time == TimeStep.TwoMonths)
                return TimeSpan.FromDays(61);
            if (time == TimeStep.TwoYears)
                return TimeSpan.FromDays(730);
            if (time == TimeStep.Week)
                return TimeSpan.FromDays(7);
            if (time == TimeStep.Year)
                return TimeSpan.FromDays(365);
            return null;
        }
    }
}
