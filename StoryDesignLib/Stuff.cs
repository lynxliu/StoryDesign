using StoryDesignInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;

namespace StoryDesignLib
{
    [SerialObjectAttribute(Name = "UnNamed Stuff")]
    public class Stuff : StoryEntityObjectBase, IStuff
    {
        List<IStuffFunction> _FunctionList = new List<IStuffFunction>();
        public List<IStuffFunction> FunctionList { get { return _FunctionList; } }

        public double Number { get; set; }
        public double Value { get; set; }

        public override ICopySupportObject Clone()
        {
            var o = new Stuff() { Number = Number, Value = Value};
            FunctionList.ForEach(v => o.FunctionList.Add(v.Clone() as IStuffFunction));
            LoadData(o);
            return o;
        }
    }

    public class StuffFunction : IStuffFunction
    {
        public double Efficiency { get; set; }
        public string Memo { get; set; }

        public ICopySupportObject Clone()
        {
            return new StuffFunction() { Efficiency = Efficiency, Memo = Memo };
        }
    }
}
