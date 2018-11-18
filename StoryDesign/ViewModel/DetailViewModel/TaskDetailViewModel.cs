using StoryDesignInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UISupport;

namespace StoryDesign.ViewModel.DetailViewModel
{
    [AttributeIcon(DefaultIconUri = "ms-appx:///Assets/Icon/Task.png")]
    public class TaskDetailViewModel : DetailViewModelBase<ITask>
    {
        //public override Uri Icon
        //{
        //    get
        //    {
        //        return new Uri("ms-appx:///Assets/Icon/Task.png");
        //    }
        //}
        public double Result { get { return TargetObject.Result; } set { TargetObject.Result = value;OnPropertyChanged("Result"); } }

        //ObservableCollection<RoleViewModel> _RoleList = new ObservableCollection<RoleViewModel>();
        public ObservableCollection<TaskDetailViewModel> TaskList
        {
            get
            {
                return MainViewModel.mainViewModel.ShowTaskList;
            }
        }
        Guid ParentTaskID
        {
            get { return TargetObject.ParentTaskID; }
            set { TargetObject.ParentTaskID = value; OnPropertyChanged("ParentTaskID"); OnPropertyChanged("ParentTask"); }
        }

        public TaskDetailViewModel ParentTask
        {
            get
            {
                var o = TaskList.FirstOrDefault(v => v.ObjectID == ParentTaskID);
                if (o != null)
                    return o;
                return null;
            }
            set
            {
                if (value != null && value != this)
                    ParentTaskID = value.ObjectID;
                else
                    ParentTaskID = Guid.Empty;
                OnPropertyChanged("ParentTaskID"); OnPropertyChanged("ParentTask");
            }
        }

        ObservableCollection<string> _SubTaskNameList = new ObservableCollection<string>();
        public ObservableCollection<string> SubTaskNameList { get { return _SubTaskNameList; } }

        public CommonCommand RefreshSubTaskCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    SubTaskNameList.Clear();
                    var l = TaskList.Where(v => v.ParentTaskID == ObjectID);
                    foreach (var g in l)
                        SubTaskNameList.Add(g.Name);
                });
            }
        }
    }
}
