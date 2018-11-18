using StoryDesignInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;

namespace StoryDesignLib
{
    [SerialObjectAttribute(Name = "UnNamed Group")]
    public class Group : StoryEntityObjectBase, IGroup
    {
        Guid _ParentGroupID = Guid.Empty;
        public Guid ParentGroupID { get { return _ParentGroupID; } set { _ParentGroupID = value; } }
        public List<IGroup> GetSubGroup(IStory story)
        {
            return story.GroupList.Where(v=>v.ParentGroupID==ObjectID).ToList();
        }

        List<IPosition> _PositionList = new List<IPosition>();
        public List<IPosition> PositionList { get { return _PositionList; } }

        //List<Guid> _GroupMemberIDList = new List<Guid>();
        //public List<Guid> GroupMemberIDList { get { return _GroupMemberIDList; } }

        //public List<IStoryEntityObject> GetMemberList()
        //{
        //    List<IStoryEntityObject> l = new List<IStoryEntityObject>();
        //    GroupMemberIDList.ForEach(v =>
        //    {
        //        var o = Story.CurrentStory.GetEntityByID(v);
        //        if (o != null)
        //            l.Add(o);
        //    });
        //    return l;
        //}

        public List<Tuple<IPosition, DateTime, DateTime>> GetActorTrace()
        {
            List<Tuple<IPosition, DateTime, DateTime>> rl = new List<Tuple<IPosition, DateTime, DateTime>>();
            Story.CurrentStory.ActorList.ForEach(v =>
            {
                var l = GetActorTrace(v);
                rl.AddRange(l);
            });
            return rl;
        }
        public List<Tuple<IPosition, DateTime, DateTime>> GetActorTrace(IActor actor)
        {
            List<Tuple<IPosition, DateTime, DateTime>> rl = new List<Tuple<IPosition, DateTime, DateTime>>();
            var l = Story.CurrentStory.RelationList.Where(v => (v.RelationType == RelationBaseType.Office) && (v.SourceID ==actor. ObjectID) 
            &&(v.TargetID==ObjectID));
            
            foreach (var r in l)
            {
                var position = PositionList.FirstOrDefault(v => v.Name == r.Memo);
                if(position!=null)
                    rl.Add(new Tuple<IPosition, DateTime, DateTime>(position, r.BeginTime, r.EndTime));
            }
            return rl;
        }
        public List<Tuple<IActor, DateTime, DateTime>> GetPositionTrace()
        {
            List<Tuple<IActor, DateTime, DateTime>> rl = new List<Tuple<IActor, DateTime, DateTime>>();
            PositionList.ForEach(v =>
            {
                var l = GetPositionTrace(v);
                rl.AddRange(l);
            });
            return rl;
        }
        public List<Tuple<IActor, DateTime, DateTime>> GetPositionTrace(IPosition position)
        {
            List<Tuple<IActor, DateTime, DateTime>> rl = new List<Tuple<IActor, DateTime, DateTime>>();
            var l = Story.CurrentStory.RelationList.Where(v => (v.RelationType == RelationBaseType.Office) && (v.TargetID == ObjectID)&&(v.Memo==position.Name));
            foreach (var r in l)
            {
                var actor = Story.CurrentStory.GetEntityByID(r.SourceID);
                if (actor != null && (actor as IActor) != null)
                {
                    rl.Add(new Tuple<IActor, DateTime, DateTime>(actor as IActor,  r.BeginTime,  r.EndTime));
                }
            }
            return rl;
        }

        public override ICopySupportObject Clone()
        {
            var o = new Group() { };
            //PositionTrace.ForEach(v => o.PositionTrace.Add(new Tuple<IPosition, Guid, DateTime, DateTime>(v.Item1, v.Item2, v.Item3,v.Item4)));
            PositionList.ForEach(v => o.PositionList.Add(v));
            //SubGroupIDList.ForEach(v => o.SubGroupIDList.Add(v));
            o.ParentGroupID = ParentGroupID;
            LoadData(o);
            return o;
        }
    }
}
