using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryDesignInterface.Diagram
{
    public class FateEntity
    {
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        EntityType _FateEntityType = EntityType.Others;
        public EntityType FateEntityType { get { return _FateEntityType; } set { _FateEntityType = value; } }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RelationDescription { get; set; }
        public RelationBaseType RelationType { get; set; }
    }

}
