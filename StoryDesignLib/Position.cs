using StoryDesignInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;

namespace StoryDesignLib
{
    public class Position : IPosition
    {

        public string Name { get; set; }
        public string Memo { get; set; }

        //List<PositionTrace> _PositionTrace = new List<PositionTrace>();
        //public List<PositionTrace> PositionTrace { get { return _PositionTrace; } }

        public ICopySupportObject Clone()
        {
            var p= new Position()
            {
                Name=Name,
                Memo=Memo,
            };
            //PositionTrace.ForEach(v => p.PositionTrace.Add(v.Clone() as PositionTrace));
            return p;
        }
    }
}
