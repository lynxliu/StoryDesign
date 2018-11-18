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
    public class StructureDiagram : DiagramBase, IStructureDiagram
    {
        bool _IsTimeSensitive = false;
        public bool IsTimeSensitive { get { return _IsTimeSensitive; } set { _IsTimeSensitive = value; } }
        public DateTime CurrentTime { get; set; }

        public override ICopySupportObject Clone()
        {
            var o = new StructureDiagram() {  Memo = Memo, Name = Name,IsTimeSensitive=IsTimeSensitive,CurrentTime=CurrentTime };
            NoteList.ForEach(v => o.NoteList.Add(v.Clone() as INote));
            NodeList.ForEach(v => o.NodeList.Add(v.Clone() as INode));
            ConnectionList.ForEach(v => o.ConnectionList.Add(v.Clone() as IConnection));
            return o;
        }
    }
}
