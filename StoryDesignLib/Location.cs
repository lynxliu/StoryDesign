using StoryDesignInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryDesignInterface.Diagram;
using CommonLib;

namespace StoryDesignLib
{
    [SerialObjectAttribute(Name = "UnNamed Location")]
    public class Location : StoryEntityObjectBase,ILocation
    {
        Guid _ParentLocationID = Guid.Empty;
        public Guid ParentLocationID { get { return _ParentLocationID; } set { _ParentLocationID = value; } }
        public List<ILocation> GetSubLocation()
        {
            return GetSubLocation(Story.CurrentStory);
        }

        //public List<Tuple<string, string, DateTime, DateTime>> GetFate(IStory story)
        //{
        //    List<Tuple<string, string, DateTime, DateTime>> rl = new List<Tuple<string, string, DateTime, DateTime>>();
        //    var l = story.GetFate(this);
        //    l.ForEach(v =>
        //    {
        //        rl.Add(new Tuple<string, string, DateTime, DateTime>(v.Item1.Name, v.Item2.Memo, v.Item2.BeginTime, v.Item2.EndTime));
        //    });
        //    return rl;
        //}

        public override ICopySupportObject Clone()
        {
            return new Location() { Name=Name,Memo=Memo,ParentLocationID=ParentLocationID};

        }

        public List<ILocation> GetSubLocation(IStory story)
        {
            return story.LocationList.Where(v => v.ParentLocationID == ObjectID).ToList();
        }

    }
}
