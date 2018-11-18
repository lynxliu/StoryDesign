using CommonLib;
using StoryDesignInterface.Express;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryDesignInterface
{
    public interface IActor: IStoryEntityObject
    {
        string ActorGrade { get; }
        Gendar Gendar { get; set; }
        string Race { get; set; }
        string Nation { get; set; }
        string Appearence { get; set; }
        string Character { get; set; }
        //List<Tuple<DateTime,DateTime,IMood>> MoodTrace { get;  }
        //List<Tuple<DateTime,DateTime ,ILocation>> PlaceTrace { get; }
        List<Tuple<IActor, RelationBaseType, DateTime, DateTime>> GetRelativeActorList();
    }

    public enum Gendar
    {
        Male,Female,Neutral
    }
}
