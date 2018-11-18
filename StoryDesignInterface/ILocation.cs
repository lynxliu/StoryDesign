using CommonLib;
using Newtonsoft.Json;
using StoryDesignInterface.Diagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryDesignInterface
{
    public interface ILocation: IStoryEntityObject
    {
        Guid ParentLocationID { get; set; }
        List<ILocation> GetSubLocation(IStory story);

    }

    //public interface ILocationObject
    //{
    //    List<LocationInfo> LocationTrace { get; }
    //}

    //public class LocationInfo : ITimeSensitive
    //{
    //    [JsonIgnore]
    //    public ILocation Target { get; set; }

    //    public DateTime BeginTime { get; set; }
    //    public DateTime EndTime { get; set; }

    //    [JsonIgnore]
    //    public TimeSpan ContinueTime { get { return EndTime - BeginTime; } }

    //    public string Name { get; set; }
    //    //public string Name { get { if (Target == null) return null;return Target.Name; }  }
    //    //public string Memo { get { if (Target == null) return null; return Target.Memo; } }
    //    //public string Description { get { if (Target == null) return null; return Target.Description; } }
    //    //public List<string> KeyWordList { get { if (Target == null) return null; return Target.KeyWordList; } }

    //    public LocationInfo(ILocation location, DateTime start,DateTime end )
    //    {
    //        BeginTime = start;
    //        EndTime = end;
    //        Name = location.Name;
    //    }

    //    public ILocation GetLocation(IStory story)
    //    {
    //        return story.LocationList.FirstOrDefault(v => v.Name == Name);
    //    }

    //    public bool IsValid(IStory story)
    //    {
    //        if (GetLocation(story) == null) return false;
    //        return true;
    //    }
    //}
}
