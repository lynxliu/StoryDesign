using CommonLib;
using StoryDesignInterface.Express;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryDesignInterface
{
    public interface IEvent: IStoryEntityObject,ISubject
    {
        IMood MainMood { get; set; }
        Guid HappenPlaceID { get; set; }
        List<Clue> ClueList { get; }
    }

    public class Clue
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
