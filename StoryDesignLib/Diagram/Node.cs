using StoryDesignInterface.Diagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;
using StoryDesignInterface;

namespace StoryDesignLib.Diagram
{
    public class Node : INode
    {
        public Guid DesignObjectID { get; set; }
        public Guid TargetObjectID { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string IconPath { get; set; }
        public int ZIndex { get; set; }
        public string NodeName { get; set; }
        public bool IsIconMode { get; set; }

        public bool? IsValid(IStory story,DateTime time)
        {
            var entity = story.GetEntityByID(TargetObjectID);
            if(entity!=null)
            {
                var timeentity = entity as ITimeSensitive;
                if (timeentity != null)
                    if (time >= timeentity.BeginTime && time <= timeentity.EndTime)
                        return true;
                    else
                        return false;
                else
                    return true;
            }
            return null;
        }

        public ICopySupportObject Clone()
        {
            return new Node()
            {
                NodeName=NodeName,TargetObjectID=TargetObjectID,Left=Left,Top=Top,Width=Width,Height=Height,IconPath=IconPath,ZIndex=ZIndex
            };
        }
    }
}
