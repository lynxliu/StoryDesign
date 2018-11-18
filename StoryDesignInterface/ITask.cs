using CommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryDesignInterface
{
    public interface ITask: IStoryEntityObject, INoteObject
    {
        Guid ParentTaskID { get; set; }
        //List<IRole> RoleList { get; }
        double Result { get; set; }
        //List<Guid> SubTaskIDList { get; }
        //Dictionary<Guid,double> ScoreList { get; }
    }
}
