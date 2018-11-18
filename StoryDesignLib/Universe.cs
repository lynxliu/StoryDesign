using StoryDesignInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;

namespace StoryDesignLib
{
    public class Universe : IUniverse
    {
        public string Description { get; set; }

        List<IRole> _LawList = new List<IRole>();
        public List<IRole> LawList { get { return _LawList; } }

        List<IPlace> _PlaceList = new List<IPlace>();
        public List<IPlace> PlaceList { get { return _PlaceList; } }

        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }

        public TimeSpan ContinueTime { get { return EndTime - BeginTime; } }

        public ICopySupportObject Clone()
        {
            var u = new Universe() { Description=Description,BeginTime=BeginTime,EndTime=EndTime};
            LawList.ForEach(v => u.LawList.Add(v.Clone() as IRole));
            PlaceList.ForEach(v => u.PlaceList.Add(v.Clone() as IPlace));
            return u;
        }
    }
    public class Place : IPlace
    {
        public string Name { get; set; }
        public string Memo { get; set; }

        public ICopySupportObject Clone()
        {
            return new Place() { Name = Name, Memo = Memo };
        }
    }
}
