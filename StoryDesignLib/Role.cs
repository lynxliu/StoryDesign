using StoryDesignInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;

namespace StoryDesignLib
{
    public class Role : IRole
    {
        public int Grade { get; set; }

        public string Description { get; set; }

        List<string> _TargetKeyWordList = new List<string>();
        public List<string> TargetKeyWordList { get { return _TargetKeyWordList; } }

        List<RoleTarget> _TargetTypeList = new List<RoleTarget>();
        public List<RoleTarget> TargetTypeList { get { return _TargetTypeList; } }

        public ICopySupportObject Clone()
        {
            var r = new Role() { Grade=Grade,Description=Description};
            TargetKeyWordList.ForEach(v => r.TargetKeyWordList.Add(v));
            TargetTypeList.ForEach(v => r.TargetTypeList.Add(v));
            return r;
        }

        public bool IsRelative(List<string> keyWords) {
            foreach(var v in keyWords)
            {
                foreach(var t in TargetKeyWordList)
                {
                    if (!string.IsNullOrEmpty(t) && t.Equals(v, StringComparison.CurrentCultureIgnoreCase))
                        return true;
                };

            };
            return false;
        }

        public bool IsRelative(Object targetObject)
        {
            if (targetObject == null) return false;
            if (targetObject as IStory != null && TargetTypeList.Contains(RoleTarget.Story)) return true;
            if (targetObject as IActor != null && TargetTypeList.Contains(RoleTarget.Actor)) return true;
            if (targetObject as IEvent != null && TargetTypeList.Contains(RoleTarget.Event)) return true;
            if (targetObject as IGroup != null && TargetTypeList.Contains(RoleTarget.Group)) return true;
            if (targetObject as IStuff != null && TargetTypeList.Contains(RoleTarget.Stuff)) return true;
            if (targetObject as ITask != null && TargetTypeList.Contains(RoleTarget.Task)) return true;

            var o = (targetObject as IDescriptionObject);
            if (o != null)
                return IsRelative(o.KeyWordList);
            return false;
        }
    }

}
