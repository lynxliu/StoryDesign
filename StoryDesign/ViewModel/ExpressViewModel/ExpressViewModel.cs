using StoryDesign.Model;
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
using Telerik.UI.Xaml.Controls.Chart;
using UISupport;
using UISupport.Diagram.Controls;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace StoryDesign.ViewModel.ExpressViewModel
{
    public class ExpressViewModel:EntityViewModelBase<IExpress>
    {
        public bool SceneIsEnable
        {
            get { if (SceneList.Count > 0) return true;return false; }
        }
        public Uri Icon
        {
            get
            {
                return new Uri("ms-appx:///Assets/Icon/Express.png");
            }
        }
        double _ViewWidth = 1280;
        public double ViewWidth { get
            {
                return _ViewWidth;
            }
            set { _ViewWidth = value;OnPropertyChanged("ViewWidth"); }
        }

        public Canvas TargetCanvas { get; set; }
        //public TimeSpan SpendTime { get { return TargetObject.SpendTime; } set { TargetObject.SpendTime = value;OnPropertyChanged("SpendTime"); } }
        public int SpendSecond
        {
            get { return TargetObject.SpendSecond; }
            set { TargetObject.SpendSecond = value;
                OnPropertyChanged("SpendSecond");
                OnPropertyChanged("SpendTime");
                Hours = TimeSpan.FromSeconds(value).Hours;
                Minutes = TimeSpan.FromSeconds(value).Minutes;
                Seconds = TimeSpan.FromSeconds(value).Seconds;
            } }

        public TimeSpan SpendTime { get { return TimeSpan.FromSeconds(SpendSecond); } }

        ObservableCollection<EpisodeViewModel> _EpisodeList = new ObservableCollection<EpisodeViewModel>();
        public ObservableCollection<EpisodeViewModel> EpisodeList { get { return _EpisodeList; } }

        public string Name { get { return TargetObject.Name; } set { TargetObject.Name = value; OnPropertyChanged("Name"); MainViewModel.RefreshTitle(value, this); } }
        public string Memo { get { return TargetObject.Memo; } set { TargetObject.Memo = value; OnPropertyChanged("Memo"); } }

        #region NoteCommand
        public bool HaveNote { get { if (NoteList.Count > 0) return true; return false; } }

        ObservableCollection<NoteViewModel> _NoteList = new ObservableCollection<NoteViewModel>();
        public ObservableCollection<NoteViewModel> NoteList { get { return _NoteList; } }
        public NoteViewModel CurrentNote { get; set; }

        public CommonCommand AddNoteCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    CurrentNote = new NoteViewModel() { TargetObject = new Note() };
                    NoteList.Add(CurrentNote);
                    SaveNoteList();
                });
            }
        }
        public CommonCommand RemoveNoteCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    if (CurrentNote != null)
                    {
                        NoteList.Remove(CurrentNote);
                    }
                    SaveNoteList();
                });
            }
        }
        void SaveNoteList()
        {
            TargetObject.NoteList.Clear();
            foreach (var note in NoteList)
            {
                TargetObject.NoteList.Add(note.TargetObject);
            }
            CurrentNote = NoteList.FirstOrDefault();
        }
        void LoadNoteList()
        {
            if (TargetObject == null) return;
            NoteList.Clear();
            foreach (var note in TargetObject.NoteList)
            {
                NoteList.Add(new NoteViewModel() { TargetObject = note });
            }
            CurrentNote = NoteList.FirstOrDefault();
        }
        #endregion

        void RefreshSceneSummary()
        {
            if (SceneList.Count == 0)
                _SceneSummary = "No scene!";
            else
                _SceneSummary = "Total scenes is " + SceneList.Count.ToString() + ",unfinished is " + 
                    SceneList.Count(v => v.FinishedPercent < 1).ToString() + ", total finish percent is "
                + (SceneList.Sum(v => v.FinishedPercent) / SceneList.Count).ToString();
            OnPropertyChanged("SceneSummary");
        }
        string _SceneSummary;
        public string SceneSummary
        {
            get
            {
                return _SceneSummary;
            }
        }

        SceneViewModel _CurrentScene;
        public SceneViewModel CurrentScene { get { return _CurrentScene; } set { _CurrentScene = value; OnPropertyChanged("CurrentScene"); } }
        ObservableCollection<SceneViewModel> _SceneList = new ObservableCollection<SceneViewModel>();
        public ObservableCollection<SceneViewModel> SceneList { get { return _SceneList; } }

        public CommonCommand AddSceneCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    var scene = new Scene(TargetObject);
                    CurrentScene = new SceneViewModel() { TargetObject = scene };
                    SceneList.Add(CurrentScene);
                    OnPropertyChanged("CurrentScene");
                    TargetObject.SceneList.Add(scene);
                    ShowExpress();
                    OnPropertyChanged("SceneIsEnable");
                });
            }
        }
        public CommonCommand RemoveCurrentSceneCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    if (CurrentScene == null) return;
                    SceneList.Remove(CurrentScene);
                    TargetObject.SceneList.Remove(CurrentScene.TargetObject);
                    CurrentScene = null;
                    OnPropertyChanged("CurrentScene");
                    ShowExpress();
                    OnPropertyChanged("SceneIsEnable");
                });
            }
        }

        public double Hours
        {
            get
            {
                return TargetObject.SpendTime.Hours;
            }
            set
            {
                TargetObject.SpendSecond = (int)(Hours * 3600 + Minutes * 60 + Seconds);
                OnPropertyChanged("Hours");
                ShowExpress();
            }
        }
        public double Minutes
        {
            get
            {
                return TargetObject.SpendTime.Minutes;
            }
            set
            {
                TargetObject.SpendSecond = (int)(Hours * 3600 + Minutes * 60 + Seconds);
                OnPropertyChanged("Minutes");
                ShowExpress();
            }
        }
        public double Seconds
        {
            get
            {
                return TargetObject.SpendTime.Seconds;
            }
            set
            {
                TargetObject.SpendSecond = (int)(Hours * 3600 + Minutes * 60 + Seconds);
                OnPropertyChanged("Seconds");
                ShowExpress();
            }
        }
        int _SpendSecondPerEpisode = 2000;
        public int SpendSecondPerEpisode
        {
            get
            {
                return _SpendSecondPerEpisode;
            }
            set
            {
                _SpendSecondPerEpisode = value;
                OnPropertyChanged("SpendSecondPerEpisode");
            }
        }
        public bool IsOverTime
        {
            get { return TargetObject.IsOverTime; }
        }

        double _LeftMargin=20;
        double _RightMargin = 10;
        double _BottomMargin = 30;
        public double BottomMargin { get { return _BottomMargin; } set { _BottomMargin = value;OnPropertyChanged("BottomMargin"); } }
        public double LeftMargin
        {
            get { return _LeftMargin; }
            set { _LeftMargin = value;OnPropertyChanged("LeftMargin"); }
        }
        public double RightMargin
        {
            get
            {
                return _RightMargin;

            }
            set { _RightMargin = value;OnPropertyChanged("RightMargin"); }
        }

        double GetPosition(TimeSpan time)
        {
            if (SpendTime.TotalSeconds == 0) return 0;
            var ts = time.TotalSeconds / SpendTime.TotalSeconds;

            return ts * (ViewWidth-LeftMargin-RightMargin)+LeftMargin;
        }

        Color GetSceneColor(SceneViewModel scene)
        {
            var c = new LColor() { HSB_H=scene.Rhythm/100*360, HSB_S=1, HSB_B=1};

            return c.getColor();
        }
        public CommonCommand RefreshCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    ShowExpress();
                });
            }
        }
        public void ShowExpress()
        {
            TargetCanvas.UpdateLayout();
            TargetCanvas.Children.Clear();
            if (SceneList.Count == 0 && EpisodeList.Count == 0)
                return;
            if (TargetCanvas.ActualHeight == 0 || TargetCanvas.ActualWidth == 0)
                return;
            DrawTimeLine();
            DrawTimeMark();
            if(IsShowEpisode)
                foreach(var e in EpisodeList)
                    DrawEpisode(e);
            foreach(var s in SceneList)
                DrawScene(s);
            RefreshSceneSummary();
        }
        void DrawTimeLine()
        {
            //TargetCanvas. UpdateLayout();
            var timeLine = new SimpleArrow();
            timeLine.StartPointX = LeftMargin;
            timeLine.StartPointY = TargetCanvas.ActualHeight - BottomMargin;
            timeLine.EndPointX = ViewWidth - RightMargin;
            timeLine.EndPointY = TargetCanvas.ActualHeight - BottomMargin;

            timeLine.StrokeThickness = 3;
            timeLine.Background = new SolidColorBrush() { Color = Colors.Black };
            TargetCanvas.Children.Add(timeLine);
            DrawTimeMark();

        }
        void DrawTimeMark()
        {
            var markNum = (ViewWidth-LeftMargin-RightMargin) / 100;
            if (markNum < 2)
            {
                Line markLine = new Line() { X1 = LeftMargin, Y1 = TargetCanvas.ActualHeight - BottomMargin, X2 = LeftMargin, Y2 = TargetCanvas.ActualHeight - BottomMargin + 5 };
                TargetCanvas.Children.Add(markLine);
                var mark = new TextBlock() { Text = "0" };
                Canvas.SetLeft(mark, LeftMargin);
                Canvas.SetTop(mark, TargetCanvas.ActualHeight - BottomMargin);
                TargetCanvas.Children.Add(mark);

                var end = ViewWidth - RightMargin;
                markLine = new Line() { X1 = end, Y1 = TargetCanvas.ActualHeight - BottomMargin, X2 = end, Y2 = TargetCanvas.ActualHeight - BottomMargin + 5 };
                TargetCanvas.Children.Add(markLine);
                mark = new TextBlock() { Text = end.ToString() };
                Canvas.SetLeft(mark, end);
                Canvas.SetTop(mark, TargetCanvas.ActualHeight - BottomMargin);
                TargetCanvas.Children.Add(mark);

                return;
            }
            var timeStep = TimeSpan.FromSeconds(SpendTime.TotalSeconds / markNum);

            for (int i = 1; i < markNum; i++)
            {
                var t = TimeSpan.FromSeconds(timeStep.TotalSeconds * i);
                var x = GetPosition(t);
                Line markLine = new Line() { X1 = x, Y1 = TargetCanvas.ActualHeight - BottomMargin, X2 = x, Y2 = TargetCanvas.ActualHeight - BottomMargin + 5 };
                TargetCanvas.Children.Add(markLine);
                var mark = new TextBlock() { Text = t.ToString() };
                Canvas.SetLeft(mark, x);
                Canvas.SetTop(mark, TargetCanvas.ActualHeight - BottomMargin);
                TargetCanvas.Children.Add(mark);
            }
        }

        void DrawScene(SceneViewModel scene)
        {
            var opacity = 0.9;
            if (!scene.IsEnable)
                opacity = 0.35;
            var h = (TargetCanvas.ActualHeight - BottomMargin) * 0.7;
            if (h < 0)
                h = 0;
            var x1 = GetPosition(scene.StartTime);
            var x2 = GetPosition(scene.StartTime+scene.Continue);
            var b = new Border()
            {
                BorderThickness = new Windows.UI.Xaml.Thickness() { Left = 1, Right = 1, Top = 2, Bottom = 2 },
                Background = new SolidColorBrush() { Color = GetSceneColor(scene), Opacity =opacity },
                Height = h,
                Width = x2 - x1,
                VerticalAlignment = VerticalAlignment.Center
            };

            Canvas.SetLeft(b, x1);
            Canvas.SetTop(b, TargetCanvas.ActualHeight-h);

            var t = new TextBlock() { Text = scene.SceneNo.ToString(),HorizontalAlignment= HorizontalAlignment.Center };
            b.Child = t;
            TargetCanvas.Children.Add(b);
            b.PointerPressed += (s, e) =>
            {
                MainViewModel.SetCurrent(scene);
                CurrentScene = scene;
            };
        }
        TimeSpan GetEpisodeStartTime(EpisodeViewModel episode)
        {
            var t = new TimeSpan();
            foreach(var e in EpisodeList)
            {
                if (e.TargetObject == episode)
                {
                    return t;
                }
                t = t + e.SpendTime;
            }
            return t;
        }
        void DrawEpisode(EpisodeViewModel episode)
        {

            var h = (TargetCanvas.ActualHeight - BottomMargin) * 0.9;
            var x1 = GetPosition(GetEpisodeStartTime(episode));
            var x2 = GetPosition(GetEpisodeStartTime(episode) + episode.SpendTime);
            var b = new Border()
            {
                BorderThickness = new Windows.UI.Xaml.Thickness() { Left = 3, Right = 3, Top = 1, Bottom = 1 },
                BorderBrush = new SolidColorBrush(Colors.YellowGreen),
                
                Height = h,
                Width = x2 - x1,
                VerticalAlignment = VerticalAlignment.Center
            };

            Canvas.SetLeft(b, x1);
            Canvas.SetTop(b, TargetCanvas.ActualHeight - h);
            var t = new TextBlock() { Text = episode.EpisodeName.ToString(),
                HorizontalAlignment = HorizontalAlignment.Center ,VerticalAlignment= VerticalAlignment.Top};
            b.Child = t;

            TargetCanvas.Children.Add(b);
        }

        //public CommonCommand RefreshToDoListCommand
        //{
        //    get
        //    {
        //        return new CommonCommand((o) =>
        //        {
        //            RefreshToDoList();
        //        });
        //    }
        //}
        public override void LoadInfo()
        {

            EpisodeList.Clear();
            TargetObject.EpisodeList.ForEach(v =>
            {
                EpisodeList.Add(new EpisodeViewModel() { TargetObject = v });
            });
            SceneList.Clear();
            TargetObject.SceneList.ForEach(v =>
            {
                SceneList.Add(new SceneViewModel() { TargetObject = v });
            });
            NoteList.Clear();
            TargetObject.NoteList.ForEach(v => NoteList.Add(new NoteViewModel() { TargetObject = v }));

            //RefreshToDoList();

            base.LoadInfo();
        }

        public override void SaveInfo()
        {
            TargetObject.SceneList.Clear();
            foreach (var s in SceneList)
                TargetObject.SceneList.Add(s.TargetObject);
            TargetObject.EpisodeList.Clear();
            EpisodeList.ToList().ForEach(v =>
            {
                TargetObject.EpisodeList.Add(v.TargetObject);
            });

            TargetObject.NoteList.Clear();
            NoteList.ToList().ForEach(v => TargetObject.NoteList.Add(v.TargetObject));

            //TargetObject.KeyWordList.Clear();
            //KeyWordList.ToList().ForEach(v => TargetObject.KeyWordList.Add(v));

            base.SaveInfo();
        }

        //public double GetWidth(TimeSpan start,TimeSpan end)
        //{
        //    var sp= TargetChart.ConvertDataToPoint(new Tuple<object, object>(new DateTime() + start, 0));
        //    var ep = TargetChart.ConvertDataToPoint(new Tuple<object, object>(new DateTime() + end, 0));
        //    return ep.X - sp.X;

        //}

        //List<SceneViewModel> GetSceneList()
        //{
        //    List<SceneViewModel> sl = new List<SceneViewModel>();
        //    TargetObject.EpisodeList.ForEach(v =>
        //    {
        //        v.SceneList.ForEach(s =>
        //        {
        //            sl.Add(new SceneViewModel() { TargetObject = s });
        //        });
        //    });
        //    return sl;
        //}
        //public SceneViewModel CurrentToDo { get; set; }
        //ObservableCollection<SceneViewModel> _ToDoList = new ObservableCollection<SceneViewModel>();
        //public ObservableCollection<SceneViewModel> ToDoList { get { return _ToDoList; } }

        ObservableCollection<MoodBaseType> _MoodList;
        public ObservableCollection<MoodBaseType> MoodList { get { if (_MoodList == null)
                {
                    _MoodList = new ObservableCollection<MoodBaseType>();
                    var l = Mood.GetMoodList();
                    l.ForEach(v => _MoodList.Add(v));
                }
                return _MoodList;
                } }
        //public void RefreshToDoList()
        //{
        //    ToDoList.Clear();
        //    var l = TargetObject.GetTodoList();
        //    l.ForEach(v => ToDoList.Add(new SceneViewModel() { TargetObject=v}));
        //}

        ObservableCollection<SceneRhythmPoint> _ScenePointList = new ObservableCollection<SceneRhythmPoint>();
        public ObservableCollection<SceneRhythmPoint> ScenePointList { get { return _ScenePointList; } }
        //public void ShowExpress()
        //{
        //    TargetChart.Series.Clear();
        //    ScenePointList.Clear();
        //    var sl = new LineSeries();
        //    Binding bind = new Binding();
        //    bind.Path = new PropertyPath("ScenePointList");
        //    sl.SetBinding(LineSeries.ItemsSourceProperty, bind);
        //    sl.ValueBinding = new PropertyNameDataPointBinding() { PropertyName = "Rhythm" };
        //    sl.CategoryBinding = new PropertyNameDataPointBinding() { PropertyName = "Time" };
        //    TargetChart.Series.Add(sl);

        //    TargetChart.Annotations.Clear();

        //    DateTime d = new DateTime();
        //    TargetObject.EpisodeList.ForEach(episold =>
        //    {
        //        var anLine = new CartesianGridLineAnnotation();
        //        anLine.Value = d;
        //        anLine.Axis = TargetChart.HorizontalAxis;
        //        anLine.Stroke = new SolidColorBrush(Colors.LightBlue);
        //        anLine.StrokeThickness = 5;
                
        //        TargetChart.Annotations.Add(anLine);
                
        //        d += episold.SpendTime;
        //    });
        //    SceneList.ToList().ForEach(scene =>
        //    {
        //        var an = new CartesianCustomAnnotation();
        //        var content = scene.Continue.ToString();

        //        //var datatemplate = (DataTemplate)this.Resources["template"];
        //        an.DataContext = content;
        //        an.ContentTemplate = SceneTemplate;
        //        an.Background = new SolidColorBrush(Mood.GetColor(scene.MainMood));
        //        //an.Content = content;
        //        an.VerticalValue = 0;
        //        an.HorizontalValue = d + TimeSpan.FromSeconds(scene.Continue.TotalSeconds / 2);

        //        TargetChart.Annotations.Add(an);
        //        ScenePointList.Add(new SceneRhythmPoint() { Time = d + scene.Continue, Rhythm = scene.Rhythm });

        //    });
        //}

        //ExpressInfoView expressInfoView = new ExpressInfoView();
        //EpisoldInfoView episoldInfoView = new EpisoldInfoView();
        public EpisodeViewModel CurrentEpisode { get; set; }
        //public SceneViewModel CurrentScene { get; set; }
        //void OpenEpisode(EpisodeViewModel vm)
        //{
        //    if (vm == null) return;

        //    MainViewModel.mainViewModel.OpenEpisode(vm);
            
        //    //MainPageViewModel.OpenView(view, CurrentEpisode.Title, ViewDataType.Episode, CurrentEpisode.Title, Guid.Empty);
        //}
        public CommonCommand InsertEpisodeCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    var index = EpisodeList.IndexOf(CurrentEpisode);
                    if (index < 0) index = EpisodeList.Count; 
                    var episode = TargetObject.AddEpisode(index);
                    CurrentEpisode = new EpisodeViewModel() { TargetObject = episode };
                    EpisodeList.Insert(index,CurrentEpisode);
                    OnPropertyChanged("CurrentEpisode");
                    ShowExpress();
                });
            }
        }
        public CommonCommand AddEpisodeCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    var episode = TargetObject.AddEpisode();
                    CurrentEpisode = new EpisodeViewModel() { TargetObject = episode };
                    EpisodeList.Add(CurrentEpisode);
                    OnPropertyChanged("CurrentEpisode");
                    ShowExpress();
                });
            }
        }
        public CommonCommand RemoveCurrentEpisodeCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    if (CurrentEpisode == null) return;
                    EpisodeList.Remove(CurrentEpisode);
                    TargetObject.EpisodeList.Remove(CurrentEpisode.TargetObject);
                    CurrentEpisode = null;
                    OnPropertyChanged("CurrentEpisode");
                    ShowExpress();
                });
            }
        }
        public CommonCommand GenerateEpisodeCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    if (SpendSecond == 0)
                    {
                        CommonLib.CommonProc.ShowMessage("Error", "please set express continue time");
                        return;
                    }
                    if (SpendSecondPerEpisode == 0)
                    {
                        CommonLib.CommonProc.ShowMessage("Error", "please set per scene continue time");
                        return;
                    }
                    var needEpisodeSecond = SpendSecond - EpisodeList.Sum(v => v.SpendTime.TotalSeconds);
                    if (needEpisodeSecond <= 0)
                    {
                        CommonLib.CommonProc.ShowMessage("Error", "exist enabled scene have enough time to cover express");
                        return;
                    }
                    var n =Math.Ceiling( needEpisodeSecond / (double)SpendSecondPerEpisode);
                    for(int i=0;i<n-1;i++)
                    {
                        var mepisode = TargetObject.AddEpisode();
                        mepisode.SpendSecond = (SpendSecondPerEpisode);
                        EpisodeList.Add(new EpisodeViewModel() { TargetObject = mepisode });
                    }
                    var lepisode = TargetObject.AddEpisode();
                    lepisode.SpendSecond =SpendSecond- Convert.ToInt32(EpisodeList.Sum(v=>v.SpendTime.TotalMilliseconds));

                    EpisodeList.Add(new EpisodeViewModel() { TargetObject = lepisode });


                    ShowExpress();
                });
            }
        }
        public CommonCommand OpenEpisodeCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    //if (ListControl == null) throw new Exception("no valid list control");
                    if (o == null&&CurrentEpisode!=null)
                    {
                        //ListControl.Child = episoldInfoView;
                        //episoldInfoView.DataContext = CurrentEpisode;
                        MainViewModel.mainViewModel.CurrentObject = CurrentEpisode;
                        return;
                    }
                    //if(ListControl!=null&& o!=null)
                    //{
                    //    var fe = o as FrameworkElement;
                    //    if (fe == null) return;
                    //    var data = fe.DataContext as EpisodeViewModel;
                    //    CurrentEpisode = data;
                    //    ListControl.Child = episoldInfoView;
                    //    episoldInfoView.DataContext = this;
                    //    MainPageViewModel.mainViewModel.CurrentObject = CurrentEpisode;
                    //    OnPropertyChanged("CurrentEpisode");
                    //}
                });
            }
        }

        //void ReturnToExpressInfo()
        //{
        //    if (ListControl != null)
        //    {
        //        ListControl.Child = expressInfoView;
        //        expressInfoView.DataContext = this;
        //    }
        //}

        //List<UIElement> _EpisodeUIList = new List<UIElement>();
        //void DrawEpisode()
        //{

        //}

        bool _IsShowEpisode = false;
        public bool IsShowEpisode
        {
            get { return _IsShowEpisode; }
            set { _IsShowEpisode = value;OnPropertyChanged("IsShowEpisode");ShowExpress(); }
        }
    }
    public class SceneRhythmPoint
    {
        public DateTime Time { get; set; }
        public double Rhythm { get; set; }
    }
}
