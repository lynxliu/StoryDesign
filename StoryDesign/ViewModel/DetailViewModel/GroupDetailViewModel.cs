using StoryDesignInterface;
using StoryDesignLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UISupport;

namespace StoryDesign.ViewModel.DetailViewModel
{
    [AttributeIcon(DefaultIconUri = "ms-appx:///Assets/Icon/Group.png")]
    public class GroupDetailViewModel : DetailViewModelBase<IGroup>
    {
        public ObservableCollection<GroupDetailViewModel> GroupList
        {
            get
            {
                return MainViewModel.mainViewModel.ShowGroupList;
            }
        }
        Guid ParentGroupID
        {
            get { return TargetObject.ParentGroupID; }
            set { TargetObject.ParentGroupID = value;OnPropertyChanged("ParentGroupID"); OnPropertyChanged("ParentGroup"); }
        }

        public GroupDetailViewModel ParentGroup
        {
            get
            {
                var o= GroupList.FirstOrDefault(v=>v.ObjectID==ParentGroupID);
                if (o != null )
                    return o;
                return null;
            }
            set
            {
                if (value != null&&value!=this)
                    ParentGroupID = value.ObjectID;
                else
                    ParentGroupID = Guid.Empty;
                OnPropertyChanged("ParentGroupID"); OnPropertyChanged("ParentGroup");
            }
        }
        ObservableCollection<string> _SubGroupList = new ObservableCollection<string>();
        public ObservableCollection<string> SubGroupList { get { return _SubGroupList; } }

        public CommonCommand RefreshSubGroupCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    SubGroupList.Clear();
                    var l = MainViewModel.mainViewModel.GroupList.Where(v => v.ParentGroupID == ObjectID);
                    foreach (var g in l)
                        SubGroupList.Add(g.Name);
                });
            }
        }

        void RefreshTrace()
        {
            if (CurrentPosition == null) return;
            var l = TargetObject.GetPositionTrace(CurrentPosition);
            CurrentPositionTrace.Clear();
            l.ForEach(v =>
            {
                CurrentPositionTrace.Add(v.Item1.Name + ":" + v.Item2.ToString() + " - " + v.Item3.ToString());
            });
        }
        public void AddPosition()
        {
            var c =new Position() { Name = "New Position" };
            PositionList.Add(c);
            CurrentPosition = c;
        }
        public void RemoveCurrentPosition()
        {
            if (CurrentPosition == null) return;
            if (PositionList.Contains(CurrentPosition))
                PositionList.Remove(CurrentPosition);
            CurrentPosition = null;
        }

        ObservableCollection<string> _CurrentPositionTrace = new ObservableCollection<string>();
        public ObservableCollection<string> CurrentPositionTrace { get { return _CurrentPositionTrace; } }


        IPosition _CurrentPosition;
        public IPosition CurrentPosition { get { return _CurrentPosition; }
            set
            {
                _CurrentPosition = value;
                OnPropertyChanged("CurrentPosition");
                RefreshTrace();
            }
        }
        ObservableCollection<IPosition> _PositionList = new ObservableCollection<IPosition>();
        public ObservableCollection<IPosition> PositionList { get { return  _PositionList; } }

        public override void LoadInfo()
        {
            base.LoadInfo();

            PositionList.Clear();
            TargetObject.PositionList.ForEach(v => PositionList.Add(v));
        }

        public override void SaveInfo()
        {
            base.SaveInfo();

            TargetObject.PositionList.Clear();
            PositionList.ToList().ForEach(v => TargetObject.PositionList.Add(v));
        }
    }
}
