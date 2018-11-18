using StoryDesignInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;

namespace StoryDesignLib
{
    public class Measure : IMeasure
    {
        public DateTime Time { get; set; }
        public double Quantity { get; set; }
        public string Name { get; set; }
        public string Memo { get; set; }
        public virtual string AbstractInfo
        {
            get
            {
                string s = "";
                if (!string.IsNullOrEmpty(Name))
                    s += Name ;

                    if (!string.IsNullOrEmpty(Memo))
                    if (string.IsNullOrEmpty(s))
                        s = Memo;
                    else
                        s += ":" + Memo;
                s += ";Quantity:" + Quantity.ToString();
                s += ";Time:" + Time.ToString();
                return s;
            }
        }
        public ICopySupportObject Clone()
        {
            return new Measure() { Name = Name, Memo = Memo, Quantity = Quantity, Time = Time };
        }
    }
}
