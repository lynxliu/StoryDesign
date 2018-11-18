using StoryDesignInterface;
using StoryDesignLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoryDesign.ViewModel.DetailViewModel
{
    [AttributeIcon(DefaultIconUri = "ms-appx:///Assets/Icon/Event.png")]
    public class EventDetailViewModel : DetailViewModelBase<IEvent>
    {
        public LocationDetailViewModel HappenLocation
        {
            get
            {
                var o = LocationList.FirstOrDefault(v => v.ObjectID == HappenPlaceID);
                if (o != null)
                    return o;
                return null;
            }
                set
            {
                if (value != null)
                    HappenPlaceID = value.ObjectID;
                else
                    HappenPlaceID = Guid.Empty;
                OnPropertyChanged("HappenPlaceID"); OnPropertyChanged("HappenLocation");
            }
        }
        public ObservableCollection<LocationDetailViewModel> LocationList { get { return MainViewModel.mainViewModel.ShowLocationList; } }
        ObservableCollection<MoodBaseType> _MoodList;
        public ObservableCollection<MoodBaseType> AllMoodList
        {
            get
            {
                if (_MoodList == null)
                {
                    _MoodList = new ObservableCollection<MoodBaseType>();
                    var l = Mood.GetMoodList();
                    l.ForEach(v => _MoodList.Add(v));
                }
                return _MoodList;
            }
        }
        public IMood MainMood
        {
            get
            {
                return TargetObject.MainMood;
            }
            set
            {
                TargetObject.MainMood = value; OnPropertyChanged("MainMood");
            }
        }
        public MoodBaseType MainMoodType
        {
            get
            {
                if (TargetObject.MainMood == null)
                    return MoodBaseType.Neutral;
                return TargetObject.MainMood.MoodType;
            }
            set
            {
                if (TargetObject.MainMood == null)
                    TargetObject.MainMood = new Mood();
                TargetObject.MainMood.MoodType = value; OnPropertyChanged("MainMoodType");
            }
        }
        public double MainMoodRank
        {
            get
            {
                if (TargetObject.MainMood == null) return 0;
                return TargetObject.MainMood.Rank;
            }
            set
            {
                if (TargetObject.MainMood == null)
                    TargetObject.MainMood = new Mood();
                TargetObject.MainMood.Rank = value; OnPropertyChanged("MainMoodRank");
            }
        }


        public Guid HappenPlaceID
        {
            get { return TargetObject.HappenPlaceID; }
            set { TargetObject.HappenPlaceID = value; OnPropertyChanged("HappenPlaceID"); OnPropertyChanged("HappenLocation"); }
        }

        public Clue CurrentClue { get; set; }
        ObservableCollection<Clue> _ClueList = new ObservableCollection<Clue>();
        public ObservableCollection<Clue> ClueList { get { return _ClueList; } }
        public void AddClue()
        {
            CurrentClue = new Clue() { Name = "New Position" };
            ClueList.Add(CurrentClue);

            OnPropertyChanged("CurrentClue");
        }
        public void RemoveCurrentClue()
        {
            if (CurrentClue == null) return;
            if (ClueList.Contains(CurrentClue))
                ClueList.Remove(CurrentClue);
            CurrentClue = null;
            OnPropertyChanged("CurrentClue");
        }
        public override void LoadInfo()
        {
            base.LoadInfo();
            ClueList.Clear();
            TargetObject.ClueList.ForEach(v => ClueList.Add(v));
        }

        public override void SaveInfo()
        {
            base.SaveInfo();
            TargetObject.ClueList.Clear();
            ClueList.ToList().ForEach(v => TargetObject.ClueList.Add(v));
        }
    }
}
