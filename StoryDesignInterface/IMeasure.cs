using CommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryDesignInterface
{
    public interface IMeasure:IDataObject,ICopySupportObject
    {
        DateTime Time { get; set; }
        double Quantity { get; set; }
    }
}
