using CommonLib;
using StoryDesignInterface.Express;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryDesignInterface
{
    public interface IGroup: IStoryEntityObject, INoteObject
    {
        Guid ParentGroupID { get; set; }
        //List<Guid> SubGroupIDList { get; }
        List<IGroup> GetSubGroup(IStory story);
        List<IPosition> PositionList { get; }
        //List<Tuple<IPosition,Guid,DateTime,DateTime>> PositionTrace { get; }

        //List<Guid> GroupMemberIDList { get; }
        //List<IStoryEntityObject> GetMemberList();
        List<Tuple<IPosition, DateTime, DateTime>> GetActorTrace();
        List<Tuple<IActor, DateTime, DateTime>> GetPositionTrace();

        List<Tuple<IPosition,DateTime,DateTime>> GetActorTrace(IActor actor);
        List<Tuple<IActor, DateTime, DateTime>> GetPositionTrace(IPosition position);
    }

    public interface IPosition : IDataObject, ICopySupportObject
    {
        //List<PositionTrace> PositionTrace { get; }
        //Guid TargetActorID { get; set; }
        //bool IsEnable { get; set; }
    }

    //public class PositionTrace: ICopySupportObject,ITimeSensitive
    //{
    //    public string ActorName { get; set; }
    //    //public string Position { get; set; }
    //    public DateTime BeginTime { get; set; }
    //    public DateTime EndTime { get; set; }

    //    public TimeSpan ContinueTime { get { return EndTime - BeginTime; } }

    //    public ICopySupportObject Clone()
    //    {
    //        return new PositionTrace() { ActorName = ActorName, BeginTime = BeginTime, EndTime = EndTime };
    //    }
    //}
}
