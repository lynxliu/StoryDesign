using CommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryDesignInterface.Diagram
{
    public interface INode: ICopySupportObject
    {
        Guid DesignObjectID { get; set; }
        Guid TargetObjectID { get; set; }
        double Left { get; set; }
        double Top { get; set; }
        double Width { get; set; }
        double Height { get; set; }
        string IconPath { get; set; }
        int ZIndex { get; set; }
        string NodeName { get; set; }
        bool IsIconMode { get; set; }
        bool? IsValid(IStory story, DateTime time);
        //List<IConnection> InConnectionList { get; }
        //List<IConnection> OutConnectionList { get; }
    }
}
