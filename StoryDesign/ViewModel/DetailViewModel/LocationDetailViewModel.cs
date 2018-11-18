using StoryDesignInterface;
using StoryDesignLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UISupport;
using Windows.UI.Xaml;

namespace StoryDesign.ViewModel.DetailViewModel
{
    [AttributeIcon(DefaultIconUri = "ms-appx:///Assets/Icon/Location.png")]
    public class LocationDetailViewModel: DetailViewModelBase<ILocation>
    {
        public ObservableCollection<LocationDetailViewModel> LocationList
        {
            get
            {
                return MainViewModel.mainViewModel.ShowLocationList;
            }
        }
        Guid ParentLocationID
        {
            get { return TargetObject.ParentLocationID; }
            set { TargetObject.ParentLocationID = value; OnPropertyChanged("ParentLocationID"); OnPropertyChanged("ParentLocation"); }
        }

        public LocationDetailViewModel ParentLocation
        {
            get
            {
                var o = LocationList.FirstOrDefault(v=>v.ObjectID==ParentLocationID);
                if (o != null )
                    return o;
                return null;
            }
            set
            {
                if (value != null && value != this)
                    ParentLocationID = value.ObjectID;
                else
                    ParentLocationID = Guid.Empty;
                OnPropertyChanged("ParentLocationID"); OnPropertyChanged("ParentLocation");
            }
        }

        ObservableCollection<string> _SubLocationNameList = new ObservableCollection<string>();
        public ObservableCollection<string> SubLocationNameList { get { return _SubLocationNameList; } }

        public CommonCommand RefreshSubLocationCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    SubLocationNameList.Clear();
                    var l = LocationList.Where(v => v.ParentLocationID == ObjectID);
                    foreach (var g in l)
                        SubLocationNameList.Add(g.Name);
                });
            }
        }

        public override void LoadInfo()
        {

            base.LoadInfo();
        }

        public override void SaveInfo()
        {

            base.SaveInfo();
        }
    }
}
