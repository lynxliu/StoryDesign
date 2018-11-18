
using StoryDesign.View.ExpressView;
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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace StoryDesign.ViewModel.ExpressViewModel
{
    public class EpisodeViewModel : EntityViewModelBase<IEpisode>
    {
        public Border ListControl { get; set; }
        //public Action ReturnToExpress { get; set; }
        //EpisoldInfoView episoldInfoView = new EpisoldInfoView();
        //public string ExpressName { get { if (TargetObject == null) return null; return TargetObject.ExpressName; }
        //set
        //    {
        //        TargetObject.ExpressName = value; OnPropertyChanged("ExpressName"); OnPropertyChanged("Title");
        //    }
        //    }
        public string EpisodeName { get {
                if (TargetObject == null) return null;return TargetObject.EpisodeName;
            } set {
                TargetObject.EpisodeName = value;OnPropertyChanged("EpisodeName"); OnPropertyChanged("Title");
            } }
        //public string Title { get { return TargetObject.Title; } }
        //public TimeSpan StartTime { get { return TargetObject.StartTime; } set { TargetObject.StartTime = value;OnPropertyChanged("StartTime"); } }

        //public TimeSpan EndTime { get { return TargetObject.EndTime; } set { TargetObject.EndTime = value; OnPropertyChanged("EndTime"); } }
        //public int StartSecond { get { return TargetObject.StartSecond; } }
        //public int SpendSecond { get { return TargetObject.SpendSecond; } set { TargetObject.SpendSecond = value;OnPropertyChanged("SpendSecond"); OnPropertyChanged("SpendTime"); } }

        //public TimeSpan StartTime { get { return TimeSpan.FromSeconds(StartSecond); } }
        public TimeSpan SpendTime { get { return TimeSpan.FromSeconds(TargetObject.SpendSecond); } }
        
        public int SpendHours
        {
            get
            {
                return SpendTime.Hours;
            }
            set
            {
                TargetObject.SpendSecond = value * 3600 + SpendMinutes * 60 + SpendSeconds;
                OnPropertyChanged("SpendTime");
                OnPropertyChanged("SpendHours");
                OnPropertyChanged("SpendMinutes");
                OnPropertyChanged("SpendSeconds");
            }
        }
        public int SpendMinutes
        {
            get
            {
                return SpendTime.Hours;
            }
            set
            {
                TargetObject.SpendSecond = SpendHours * 3600 + value * 60 + SpendSeconds;
                OnPropertyChanged("SpendTime");
                OnPropertyChanged("SpendHours");
                OnPropertyChanged("SpendMinutes");
                OnPropertyChanged("SpendSeconds");
            }
        }
        public int SpendSeconds
        {
            get
            {
                return SpendTime.Hours;
            }
            set
            {
                TargetObject.SpendSecond = SpendHours * 3600 + SpendMinutes * 60 + value;
                OnPropertyChanged("SpendTime");
                OnPropertyChanged("SpendHours");
                OnPropertyChanged("SpendMinutes");
                OnPropertyChanged("SpendSeconds");
            }
        }
        //SceneViewModel _CurrentScene;
        //public SceneViewModel CurrentScene { get { return _CurrentScene; } set { _CurrentScene = value;OnPropertyChanged("CurrentScene"); } }
        //ObservableCollection <SceneViewModel> _SceneList = new ObservableCollection<SceneViewModel>();
        //public ObservableCollection<SceneViewModel> SceneList { get { return _SceneList; } }

        //ObservableDictionary<AudienceViewModel, MoodViewModel> _MoodList = new ObservableDictionary<AudienceViewModel, MoodViewModel>();
        //public ObservableDictionary<AudienceViewModel, MoodViewModel> MoodList { get { return _MoodList; } }

        //ObservableCollection<INote> _NoteList = new ObservableCollection<INote>();
        //public ObservableCollection<INote> NoteList { get { return _NoteList; } }
        //public CommonCommand OpenEpisodeCommand
        //{
        //    get
        //    {
        //        return new CommonCommand((o) =>
        //        {
        //            if (ListControl == null) throw new Exception("no valid list control");

        //            if (ListControl != null && o != null)
        //            {
        //                var fe = o as FrameworkElement;
        //                if (fe == null) return;
        //                var data = fe.DataContext as EpisodeViewModel;
        //                ListControl.Child = episoldInfoView;
        //                episoldInfoView.DataContext = this;
        //                MainPageViewModel.mainViewModel.CurrentObject = this;

        //            }
        //        });
        //    }
        //}
        //public ExpressObjectViewModel CurrentSelectedExpressObject { get; set; }
        //void RefreshExpressObjectList()
        //{
        //    var ol = MainViewModel.mainViewModel.Target.TargetStory.GetAllExpressObjectList();
        //    ol.ForEach(v => _AllExpressObjextList.Add(new ExpressObjectViewModel() { TargetObject = v }));
        //}
        //ObservableCollection<ExpressObjectViewModel> _AllExpressObjextList;
        //public ObservableCollection<ExpressObjectViewModel> AllExpressObjextList { get {
        //        if(_AllExpressObjextList==null)
        //        {
        //            _AllExpressObjextList = new ObservableCollection<ExpressObjectViewModel>();
        //            RefreshExpressObjectList();
        //        }
        //        return _AllExpressObjextList;
        //    } }

        
        

        //public CommonCommand RefreshExpressObjectListCommand
        //{
        //    get
        //    {
        //        return new CommonCommand((o) =>
        //        {
        //            AllExpressObjextList.Clear();
        //            RefreshExpressObjectList();
        //        });
        //    }
        //}
        //public CommonCommand AddExpressObjectCommand
        //{
        //    get
        //    {
        //        return new CommonCommand((o) =>
        //        {
        //            if (CurrentSelectedExpressObject != null&&CurrentScene!=null)
        //            {
        //                if (!CurrentScene.ExpressObjectList.Any(v => v.TargetObjectID == CurrentSelectedExpressObject.TargetObjectID))
        //                {
        //                    CurrentScene.ExpressObjectList.Add(CurrentSelectedExpressObject);
        //                    CurrentScene.TargetObject.ExpressObjectList.Add(CurrentSelectedExpressObject.TargetObject);
        //                }
        //            }
        //        });
        //    }
        //}
        //public CommonCommand OpenEpisodeCommand
        //{
        //    get
        //    {
        //        return new CommonCommand((o) =>
        //        {
        //            MainViewModel.mainViewModel.CurrentObject = this.TargetObject;
        //            MainViewModel.mainViewModel.OpenEpisode(this);
        //            //MainPageViewModel.OpenView(MainPageViewModel.mainViewModel.OpenEpisode(TargetObject), TargetObject.Title, Model.ViewDataType.Episode, Title, Guid.Empty);
        //        });
        //    }
        //}

        public override void LoadInfo()
        {
            //NoteList.Clear();
            //TargetObject.NoteList.ForEach(v => NoteList.Add(v));

            ////KeyWordList.Clear();
            ////TargetObject.KeyWordList.ForEach(v => KeyWordList.Add(v));

            //SceneList.Clear();
            //TargetObject.SceneList.ForEach(v =>
            //{
            //    SceneList.Add(new SceneViewModel() { TargetObject = v });
            //});
            base.LoadInfo();
        }

        public override void SaveInfo()
        {
            //TargetObject.NoteList.Clear();
            //NoteList.ToList().ForEach(v => TargetObject.NoteList.Add(v));

            ////TargetObject.KeyWordList.Clear();
            ////KeyWordList.ToList().ForEach(v => TargetObject.KeyWordList.Add(v));

            //TargetObject.SceneList.Clear();
            //SceneList.ToList().ForEach(v => { TargetObject.SceneList.Add(v.TargetObject); });

            base.SaveInfo();
        }
    }
}
