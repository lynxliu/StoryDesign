using CommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryDesignInterface
{
    public interface IRelation:ITimeSensitive, ICopySupportObject,IIdentifiedObject,INoteObject
    {
        RelationBaseType RelationType { get; set; }
        string Memo { get; set; }
        Guid SourceID { get; set; }
        Guid TargetID { get; set; }
        bool IsRelation(Guid objAID, Guid objBID);
        bool IsRelationAbout(Guid objID);
    }

    // office between actor and group, own between actor and stuff, kin,mate and friend between actor and actor
    //responsive,participate between actor and task, attend butween actor or stuff and event, appear between actor, stuff or event to location
    public enum RelationBaseType
    {
        Office,Own,Kin,Mate,Friend,Responsive,Participate,Attend,Appear,Others
    }
}
