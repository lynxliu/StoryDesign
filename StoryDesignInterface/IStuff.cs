using CommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryDesignInterface
{
    public interface IStuff: IStoryEntityObject, INoteObject
    {
        List<IStuffFunction> FunctionList { get; }
        double Number { get; set; }
        double Value { get; set; }
    }

    public interface IStuffFunction:ICopySupportObject
    {
        string Memo { get; set; }
        double Efficiency { get; set; }
    }
}
