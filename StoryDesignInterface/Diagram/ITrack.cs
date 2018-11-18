using CommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryDesignInterface.Diagram
{
    public interface ITrack:ICopySupportObject
    {
        double Width { get; set; }
        string Name { get; set; }
        List<FateEntity> EntityList { get; }
    }

    public enum EntityType
    {
        Actor,Group,Event,Stuff,Task,Location,Others
    }
}
