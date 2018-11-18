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
    public class Connection :IConnection
    {
        public bool? IsValid(IStory story, DateTime time)
        {
            var entity = story.GetEntityByID(TargetObjectID);
            if (entity != null)
            {
                var timeentity = entity as ITimeSensitive;
                if (timeentity != null)
                    if (time >= timeentity.BeginTime && time <= timeentity.EndTime)
                        return true;
                    else
                        return false;
                else
                    return true;
            }
            return null;
        }
        public Guid SourceDesignItemID { get; set; }
        public Guid TargetDesignItemID { get; set; }
        public Guid DesignObjectID { get; set; }
        public Guid TargetObjectID { get; set; }
        public Guid StartObjectID { get; set; }
        public Guid EndObjectID { get; set; }

        ConnectionType _ConnectionType = ConnectionType.OneWay;
        public ConnectionType ConnectionType { get { return _ConnectionType; } set { _ConnectionType = value; } }
        public double StartLeft { get; set; }
        public double StartTop { get; set; }
        public double EndLeft { get; set; }
        public double EndTop { get; set; }
        public string Memo { get; set; }

        public ICopySupportObject Clone()
        {
            return new Connection() {  EndLeft = EndLeft, EndObjectID = EndObjectID,
                EndTop = EndTop, Memo = Memo, StartLeft = StartLeft, StartObjectID = StartObjectID,
                StartTop = StartTop, TargetObjectID = TargetObjectID };

        }

        LinkInfo _LinkLineInfo = new LinkInfo();
        public LinkInfo LinkLineInfo { get { return _LinkLineInfo; } set { _LinkLineInfo = value; } }

    }
}
