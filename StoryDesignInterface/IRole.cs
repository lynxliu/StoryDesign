using CommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryDesignInterface
{
    public interface IRole: ICopySupportObject
    {
        int Grade { get; set; }
        string Description { get; set; }
        List<string> TargetKeyWordList { get; }
        List<RoleTarget> TargetTypeList { get; }
        bool IsRelative(List<string> keyWords);

        bool IsRelative(Object targetObject);
    }

    public enum RoleTarget
    {
        Story,Actor,Stuff,Event,Task,Group
    }
}
