using StoryDesignInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;

namespace StoryDesignLib
{
    public class Note : INote
    {
        DateTime _CreateTime = DateTime.Now;
        public DateTime CreateTime { get { return _CreateTime; } }

        DateTime _LastModified = DateTime.Now;
        public DateTime LastModified { get { return _LastModified; } set { _LastModified = value; } }

        string _Description;
        public string Description { get { return _Description; } set { _Description = value;LastModified = DateTime.Now; } }

        public ICopySupportObject Clone()
        {
            return new Note() { Description = Description };
        }
    }
}
