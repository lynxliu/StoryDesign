using CommonLib;
using StoryDesignInterface.Diagram;
using StoryDesignInterface.Express;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryDesignInterface
{
    public interface IStory:IDataObject,ITimeSensitive, INoteObject, ICopySupportObject, IDiagramObject
    {
        
        List<IActor> ActorList { get; }
        List<IStuff> StuffList { get; }
        List<ITask> TaskList { get; }
        List<IRelation> RelationList { get; }
        List<IGroup> GroupList { get; }
        List<IEvent> EventList { get; }
        List<ILocation> LocationList { get; }
        //List<IRole> RoleList { get; }
        
        //List<IFateDiagram> FateDiagramList { get; }
        List<IExpress> ExpressList { get; }
        string Author { get; set; }
        Version StoryVersion { get; set; }
        DateTime CreateTime { get; set; }

        List<Tuple<IStoryEntityObject, IRelation>> GetFate(ISubject subject);
        List<Tuple<IRelation, IStoryEntityObject, IStoryEntityObject>> GetRelativeFate(List<ISubject> subject);
        //List<string> GetAllKeyWords();
        List<string> GetAllMeasureName();
        List<string> GetAllParameterName();
        IStoryEntityObject GetEntityByID(Guid id);
        List<T> GetRelatedEntityList<T>(Guid id);
        void AddEntity(IStoryEntityObject obj);
        void RemoveEntity(IStoryEntityObject obj);
        List<IStoryEntityObject> GetAllEntityList();
        //List<IExpressObject> GetAllExpressObjectList();
    }

    public enum CollectionOperation
    {
        Add,Remove
    }
}
