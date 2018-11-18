using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryDesignInterface.Diagram
{
    public interface IStructureDiagram:IDiagramBase
    {
        bool IsTimeSensitive { get; set; }
        DateTime CurrentTime { get; set; }

    }
}
