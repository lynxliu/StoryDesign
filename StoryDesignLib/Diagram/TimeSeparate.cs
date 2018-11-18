using StoryDesignInterface.Diagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoryDesignInterface;

namespace StoryDesignLib.Diagram
{
    public class TimeSeparate : ITimeSeparate
    {
        bool _IsEnable = true;
        public bool IsEnable { get { return _IsEnable; } set { _IsEnable = value; } }

        string _Description;
        public string Description
        {
            get
            {
                if (string.IsNullOrEmpty(_Description))
                    _Description = BeginTime.ToString() + "-" + EndTime.ToString();
                return _Description;
            }
            set
            {
                _Description = value;
            }
        }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }

        public TimeSpan ContinueTime { get { return EndTime - BeginTime; } }
        public bool IsValid(TimeSpan minSeparate)
        {
            if (ContinueTime <= minSeparate)
                return false;
            return true;
        }
    }
}
