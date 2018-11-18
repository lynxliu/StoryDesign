using CommonLib;
using StoryDesignInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryDesignLib
{
    public class TimeSensitiveEntityBase : IDataObject, ITimeSensitive
    {
        string _Name = "UnNamed";
        public string Name { get { return _Name; } set { _Name = value; } }
        public string Memo { get; set; }
        public virtual string AbstractInfo
        {
            get
            {
                string s = "";
                if (!string.IsNullOrEmpty(Name))
                    s += Name + "( from " + BeginTime.ToString() + " to " + EndTime.ToString() + ")";
                
                    if (!string.IsNullOrEmpty(Memo))
                    if (string.IsNullOrEmpty(s))
                        s = Memo;
                    else
                        s += ":" + Memo;

                return s;
            }
        }
        List<string> _KeyWordList = new List<string>();
        public List<string> KeyWordList { get { return _KeyWordList; } }

        DateTime _BeginTime = DateTime.MinValue;
        public DateTime BeginTime { get { return _BeginTime; } set { _BeginTime = value; } }

        DateTime _EndTime = DateTime.MaxValue;
        public DateTime EndTime { get { return _EndTime; } set { _EndTime = value; } }

        public TimeSpan ContinueTime
        {
            get { return EndTime - BeginTime; }
        }
    }
    public class TimeSensitiveIdentifiedEntityBase :TimeSensitiveEntityBase, IDataObject, ITimeSensitive
    {
        Guid _ObjectID = Guid.NewGuid();
        public Guid ObjectID { get { return _ObjectID; } set { _ObjectID = value; } }
    }

    public class TimeSensitiveIdentifiedParameterEntityBase : TimeSensitiveIdentifiedEntityBase, IDataObject, ITimeSensitive
    {
        List<Parameter> _ParameterList = new List<Parameter>();
        public List<Parameter> ParameterList { get { return _ParameterList; } }
    }
}
