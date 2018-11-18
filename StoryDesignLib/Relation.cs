using StoryDesignInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonLib;
using Newtonsoft.Json;

namespace StoryDesignLib
{
    public class Relation : IRelation
    {
        RelationBaseType _RelationType = RelationBaseType.Others;
        public RelationBaseType RelationType { get { return _RelationType; } set { _RelationType = value; } }
        public string Memo { get; set; }

        Guid _ObjectID = Guid.NewGuid();
        public Guid ObjectID { get { return _ObjectID; } set { _ObjectID = value; } }

        public Guid SourceID { get; set; }
        public Guid TargetID { get; set; }

        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }

        public TimeSpan ContinueTime { get { return EndTime - BeginTime; } }

        public bool HaveNote { get { if (NoteList.Count > 0) return true;return false; } }
        List<INote> _NoteList = new List<INote>();
        public List<INote> NoteList { get { return _NoteList; } }

        [JsonIgnore]
        public string SourceName
        {
            get
            {
                var s = Story.CurrentStory.GetEntityByID(SourceID);
                if (s != null)
                    return s.Name;
                return null;
            }
        }
        [JsonIgnore]
        public string TargetName
        {
            get
            {
                var s = Story.CurrentStory.GetEntityByID(TargetID);
                if (s != null)
                    return s.Name;
                return null;
            }
        }


        //public string Description { get; set; }

        //List<string> _KeyWordList = new List<string>();
        //public List<string> KeyWordList { get { return _KeyWordList; } }

        public ICopySupportObject Clone()
        {
            var r = new Relation() { Memo = Memo, SourceID = SourceID, TargetID = TargetID, BeginTime = BeginTime, EndTime = EndTime,  RelationType=RelationType };
            //NoteList.ForEach(v => r.NoteList.Add(v.Clone() as INote));
            //KeyWordList.ForEach(v => r.KeyWordList.Add(v));
            return r;
        }

        public bool IsRelation(Guid objAID, Guid objBID)
        {
            if (SourceID == objAID && TargetID == objBID) return true;
            if (SourceID == objBID && TargetID == objAID) return true;
            return false;
        }

        public bool IsRelationAbout(Guid objID)
        {
            if (SourceID == objID || TargetID == objID) return true;
            return false;
        }
    }
}
