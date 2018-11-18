using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryDesignInterface.Diagram
{
    public interface IDiagramObject
    {
        List<IStructureDiagram> StructureDiagramList { get; }
        //List<ITimeDiagram> TimeDiagramList { get; }
    }
}
