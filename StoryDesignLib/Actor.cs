using StoryDesignInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;
using StoryDesignInterface.Express;

namespace StoryDesignLib
{
    [SerialObjectAttribute(Name ="UnNamed Actor")]
    public class Actor :StoryEntityObjectBase, IActor
    {
        public string ActorGrade
        {
            get
            {
                var l = Story.CurrentStory.ActorList.Where(v => v.Gendar == _Gendar).OrderByDescending(v => v.Grade).ToList();
                for (int i = 1; i < l.Count(); i++)
                    if (l[i].ObjectID == ObjectID)
                        return Gendar.ToString()+"-" + i.ToString();
                return "Nothing";
            }
        }
        Gendar _Gendar = Gendar.Neutral;
        public Gendar Gendar { get { return _Gendar; } set { _Gendar = value; } }

        public string Race { get; set; }
        public string Nation { get; set; }
        public string Appearence { get; set; }
        public string Character { get; set; }

        //List<Tuple<DateTime, DateTime, IMood>> _MoodTrace = new List<Tuple<DateTime, DateTime, IMood>>();
        //public List<Tuple<DateTime, DateTime, IMood>> MoodTrace { get { return _MoodTrace; } }

        //List<Tuple<DateTime, DateTime, ILocation>> _PlaceTrace = new List<Tuple<DateTime, DateTime, ILocation>>();
        //public List<Tuple<DateTime, DateTime, ILocation>> PlaceTrace { get { return _PlaceTrace; } }
        public List<Tuple<IActor, RelationBaseType, DateTime, DateTime>> GetRelativeActorList()
        {
            List<Tuple<IActor, RelationBaseType, DateTime, DateTime>> rl = new List<Tuple<IActor, RelationBaseType, DateTime, DateTime>>();
            var l = Story.CurrentStory.RelationList.Where(v => (v.RelationType == RelationBaseType.Friend|| v.RelationType == RelationBaseType.Kin|| v.RelationType == RelationBaseType.Mate)
            && (v.SourceID == ObjectID)
            );

            foreach (var r in l)
            {
                var entity = Story.CurrentStory.GetEntityByID(r.TargetID);
                if (entity != null&&entity is IActor)
                    rl.Add(new Tuple<IActor, RelationBaseType, DateTime, DateTime>(entity as IActor,r.RelationType, r.BeginTime, r.EndTime));
            }
             l = Story.CurrentStory.RelationList.Where(v => (v.RelationType == RelationBaseType.Friend || v.RelationType == RelationBaseType.Kin || v.RelationType == RelationBaseType.Mate)
            && (v.TargetID == ObjectID)
            );

            foreach (var r in l)
            {
                var entity = Story.CurrentStory.GetEntityByID(r.SourceID);
                if (entity != null && entity is IActor)
                    rl.Add(new Tuple<IActor, RelationBaseType, DateTime, DateTime>(entity as IActor, r.RelationType, r.BeginTime, r.EndTime));
            }
            return rl;
        }
        public override ICopySupportObject Clone()
        {
            var o = new Actor() { Gendar=_Gendar,Race=Race,Nation=Nation,Character=Character};
            //MoodTrace.ForEach(v => o.MoodTrace.Add(new Tuple<DateTime, DateTime, IMood>(v.Item1, v.Item2, v.Item3.Clone() as IMood)));
            LoadData(o);
            return o;
        }
    }
}
