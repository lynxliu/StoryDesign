using StoryDesign.View.Control;
using StoryDesignInterface;
using StoryDesignInterface.Express;
using StoryDesignLib;
using StoryDesignLib.Express;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UISupport;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace StoryDesign.ViewModel.ExpressViewModel
{
    public class SceneViewModel : EntityViewModelBase<IScene>
    {
        public Brush FinishedPercentBrush
        {
            get
            {
                if (!IsEnable) return new SolidColorBrush(Colors.Transparent);
                return new SolidColorBrush(Colors.LightGreen) { Opacity = FinishedPercent };
            }
        }
        public string FinishedPercentString
        {
            get
            {
                return "Finished " + FinishedPercent.ToString("p");
            }
        }
        public bool IsEnable
        {
            get { return TargetObject.IsEnable; }
            set { TargetObject.IsEnable = value;OnPropertyChanged("IsEnable"); OnPropertyChanged("FinishedPercentBrush"); }
        }
        public int SceneNo
        {
            get { return TargetObject.SceneNo; }
            
        }
        public double FinishedPercent
        {
            get { return TargetObject.FinishedPercent; }
            set { TargetObject.FinishedPercent = value;
                OnPropertyChanged("FinishedPercent");
                OnPropertyChanged("FinishedPercentBrush");
                OnPropertyChanged("FinishedPercentString");
            }
        }
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
        public MoodBaseType MainMoodType { get {
                if (TargetObject.MainMood == null)
                    return MoodBaseType.Neutral;
                return TargetObject.MainMood.MoodType; }
            set {
                if (TargetObject.MainMood == null)
                    TargetObject.MainMood = new Mood();
                TargetObject.MainMood.MoodType = value;OnPropertyChanged("MainMoodType"); } }
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
        public double Rhythm { get { return TargetObject.Rhythm; } set { TargetObject.Rhythm = value;OnPropertyChanged("Rhythm"); } }
        public int StartSecond { get { return TargetObject.StartSecond; } set { TargetObject.StartSecond = value; OnPropertyChanged("StartSecond"); OnPropertyChanged("StartTime"); } }
        public TimeSpan StartTime { get { return TargetObject.StartTime; } }
        public int SpendSecond { get { return TargetObject.SpendSecond; } set { TargetObject.SpendSecond = value;OnPropertyChanged("SpendSecond");OnPropertyChanged("Continue"); } }
        public TimeSpan Continue { get { return TargetObject.Continue; } }

        public ExpressObjectViewModel CurrentExpressObject { get; set; }
        ObservableCollection<ExpressObjectViewModel> _ExpressObjectList = new ObservableCollection<ExpressObjectViewModel>();
        public ObservableCollection<ExpressObjectViewModel> ExpressObjectList { get { return _ExpressObjectList; } }

        //ObservableCollection<IStoryEntityObject> _AllEntityList = new ObservableCollection<IStoryEntityObject>();
        //public ObservableCollection<IStoryEntityObject> AllEntityList { get { return _AllEntityList; } }
        //public IStoryEntityObject SelectedEntity { get; set; }

        //public void RefreshEntityList()
        //{
        //    var l = MainViewModel.mainViewModel.Target.TargetStory.GetAllEntityList();
        //    AllEntityList.Clear();
        //    l.ForEach(v => AllEntityList.Add(v));
        //}

        public string Description { get { return TargetObject.Description; } set { TargetObject.Description = value;OnPropertyChanged("Description"); } }
        public void AddEntityAsExpressObject(IStoryEntityObject obj)
        {
            var eo = new ExpressObject();
            eo.AddObject(obj);
            CurrentExpressObject = new ExpressObjectViewModel() { TargetObject = eo };
            ExpressObjectList.Add(CurrentExpressObject);
            TargetObject.ExpressObjectList.Add(CurrentExpressObject.TargetObject);
        }
        SelectEntityDialog selectEntityDialog;
        async void AddExpressObject()
        {
            if (selectEntityDialog == null)
            {
                selectEntityDialog = new SelectEntityDialog();
            }


            ContentDialogResult result = await selectEntityDialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var l = selectEntityDialog.GetSelectedEntityList();
                l.ForEach(v => AddEntityAsExpressObject(v));
            }

        }

        public CommonCommand AddExpressObjectCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    AddExpressObject();
                });
            }
        }

        public CommonCommand RemoveExpressObjectCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    if (CurrentExpressObject == null) return;
                    ExpressObjectList.Remove(CurrentExpressObject);
                    TargetObject.ExpressObjectList.Remove(CurrentExpressObject.TargetObject);


                });
            }
        }

        public override void LoadInfo()
        {
            ExpressObjectList.Clear();
            TargetObject.ExpressObjectList.ForEach(v =>
            {
                ExpressObjectList.Add(new ExpressObjectViewModel() { TargetObject = v });
            });
            base.LoadInfo();
        }

        public override void SaveInfo()
        {

            TargetObject.ExpressObjectList.Clear();
            ExpressObjectList.ToList().ForEach(v => { TargetObject.ExpressObjectList.Add(v.TargetObject); });

            base.SaveInfo();
        }
    }
}
