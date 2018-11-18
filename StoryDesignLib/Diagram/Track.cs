using StoryDesignInterface.Diagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;

namespace StoryDesignLib.Diagram
{
    public class Track : ITrack
    {
        public bool Selected { get; set; }

        double _Width = 55;
        public double Width { get { return _Width; } set { _Width = value; } }
        public string Name { get; set; }
        List<FateEntity> _EntityList = new List<FateEntity>();
        public List<FateEntity> EntityList { get { return _EntityList; } }
        public ICopySupportObject Clone()
        {
            var t= new Track() { Name = Name, Width = Width };
            EntityList.ForEach(v => t.EntityList.Add(v));
            return t;
        }
    }
}
