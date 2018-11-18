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
    [AttributeIcon(DefaultIconUri = "ms-appx:///Assets/Icon/Actor.png")]
    public class ActorDetailViewModel: DetailViewModelBase<IActor>
    {
        public bool IsMale { get
            {
                if (TargetObject.Gendar == Gendar.Male) return true;
                return false;
            }
            set
            {
                if (value)
                    TargetObject.Gendar = Gendar.Male;
                else
                    TargetObject.Gendar = Gendar.Female;
                OnPropertyChanged("IsMale");
            }
        }
        public string ActorGrade { get { return TargetObject.ActorGrade; } }
        

        public string Nation
        {
            get { return TargetObject.Nation; }
            set { TargetObject.Nation = value;OnPropertyChanged("Nation"); }
        }

        public string Race
        {
            get { return TargetObject.Race; }
            set { TargetObject.Race = value; OnPropertyChanged("Race"); }
        }

        public string Appearence
        {
            get { return TargetObject.Appearence; }
            set { TargetObject.Appearence = value; OnPropertyChanged("Appearence"); }
        }

        public string Character
        {
            get { return TargetObject.Character; }
            set { TargetObject.Character = value; OnPropertyChanged("Character"); }
        }

        ObservableCollection<string> _RelativeActorList = new ObservableCollection<string>();
        public ObservableCollection<string> RelativeActorList { get { return _RelativeActorList; } }

        public CommonCommand RefreshRelativeActorListCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    var l = TargetObject.GetRelativeActorList();
                    RelativeActorList.Clear();
                    l.ForEach(v =>
                    {
                        RelativeActorList.Add(v.Item1.Name + "(" + v.Item2.ToString() + "):" + v.Item3.ToString() + " to " + v.Item4.ToString());
                    });
                });
            }
        }
    }
}
