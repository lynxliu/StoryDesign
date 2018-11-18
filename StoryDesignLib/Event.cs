using StoryDesignInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;

namespace StoryDesignLib
{
    [SerialObjectAttribute(Name = "UnNamed Event")]
    public class Event : StoryEntityObjectBase, IEvent
    {
        List<Clue> _ClueList = new List<Clue>();
        public List<Clue> ClueList { get { return _ClueList; }  }
        IMood _MainMood = new Mood() { MoodType= MoodBaseType.Neutral };
        public IMood MainMood { get { return _MainMood; } set { _MainMood = value; } }

        public Guid HappenPlaceID { get; set; }

        public override ICopySupportObject Clone()
        {
            var o = new Event() { Grade = Grade, MainMood = MainMood.Clone() as IMood,HappenPlaceID=HappenPlaceID};
            LoadData(o);
            return o;
        }
    }
}
