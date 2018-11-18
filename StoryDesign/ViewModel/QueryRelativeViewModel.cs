using StoryDesignInterface;
using StoryDesignInterface.Diagram;
using StoryDesignLib.Diagram;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UISupport;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace StoryDesign.ViewModel
{
    class QueryRelativeViewModel:ViewModelBase
    {
        public bool IsTimeSpanEditable { get { return true; } }
        public Uri Icon
        {
            get
            {
                return new Uri("ms-appx:///Assets/Icon/Fate.png");
            }
        }
        public string Name { get; set; }

        double _ListPaneWidth = 350;
        public double ListPaneWidth
        {
            get { return _ListPaneWidth; }
            set { _ListPaneWidth = value; OnPropertyChanged("ListPaneWidth"); }
        }

        //public Guid TargetObjectID { get { return TargetObject.TargetObjectID; } set { TargetObject.TargetObjectID = value;OnPropertyChanged("TargetObjectID"); } }
        //public TimeSpan MinSeparate { get { return TargetObject.MinSeparate; } set { TargetObject.MinSeparate = value; OnPropertyChanged("MinSeparate"); } }
        //FilterType _ShowType = FilterType.NoFilter;


        public ObservableCollection<ITrack> TrackList { get { return helper.TrackList; } }

        public ObservableCollection<ITimeSeparate> TimeSeparateList { get { return helper.TimeSeparateList; } }

        public DateTime BeginTime { get { return helper.BeginTime; } set { helper.BeginTime = value; OnPropertyChanged("BeginTime"); } }
        public DateTime EndTime { get { return helper.EndTime; } set { helper.EndTime = value; OnPropertyChanged("EndTime"); } }

        public TimeSpan Continue { get { return EndTime - BeginTime; } }
        FateDiagramHelper helper = new FateDiagramHelper();

        public double SeparateWidth { get { return helper.SeparateWidth; } set { helper.SeparateWidth = value; OnPropertyChanged("SeparateWidth"); } }

        public double LeftMargin { get { return helper.LeftMargin; } set { helper.LeftMargin = value; OnPropertyChanged("LeftMargin"); } }
        public double RightMargin { get { return helper.RightMargin; } set { helper.RightMargin = value; OnPropertyChanged("RightMargin"); } }
        public double BottomMargin { get { return helper.BottomMargin; } set { helper.BottomMargin = value; OnPropertyChanged("BottomMargin"); } }

        public Canvas TargetCanvas { get { return helper.TargetCanvas; } set { helper.TargetCanvas = value; } }
        ObservableCollection<QueryItem> _EntityList = new ObservableCollection<QueryItem>();
        public ObservableCollection<QueryItem> EntityList { get { return _EntityList; } }

        void LoadEneieyList()
        {

            MainViewModel.mainViewModel.Target.TargetStory.ActorList.ForEach(v => EntityList.Add(new QueryItem(v)));
            MainViewModel.mainViewModel.Target.TargetStory.EventList.ForEach(v => EntityList.Add(new QueryItem(v)));
            MainViewModel.mainViewModel.Target.TargetStory.LocationList.ForEach(v => EntityList.Add(new QueryItem(v)));
            MainViewModel.mainViewModel.Target.TargetStory.GroupList.ForEach(v => EntityList.Add(new QueryItem(v)));
            MainViewModel.mainViewModel.Target.TargetStory.StuffList.ForEach(v => EntityList.Add(new QueryItem(v)));
            MainViewModel.mainViewModel.Target.TargetStory.TaskList.ForEach(v => EntityList.Add(new QueryItem(v)));
        }
        public CommonCommand BackCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    MainPageViewModel.ActiveMainView();
                    //var rootFrame = Window.Current.Content as Frame;
                    //if (rootFrame.Content != null)
                    //{
                    //    var main = rootFrame.Content as MainPage;
                    //    main.SetView(MainViewModel.mainViewModel.CurrentMainView);
                    //    //rootFrame.GoBack();
                    //}
                });
            }
        }
        public CommonCommand FilterTypeCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    var l = EntityList.Where(v => v.IsSelected);
                    EntityList.Clear();
                    if (o == null)
                        LoadEneieyList();
                    else
                    {
                        var s = o.ToString();
                        if (s == "Actor")
                            MainViewModel.mainViewModel.Target.TargetStory.ActorList.ForEach(v => EntityList.Add(new QueryItem(v)));
                        if (s == "Event")
                            MainViewModel.mainViewModel.Target.TargetStory.EventList.ForEach(v => EntityList.Add(new QueryItem(v)));
                        if (s == "Group")
                            MainViewModel.mainViewModel.Target.TargetStory.GroupList.ForEach(v => EntityList.Add(new QueryItem(v)));
                        if (s == "Location")
                            MainViewModel.mainViewModel.Target.TargetStory.LocationList.ForEach(v => EntityList.Add(new QueryItem(v)));
                        if (s == "Stuff")
                            MainViewModel.mainViewModel.Target.TargetStory.StuffList.ForEach(v => EntityList.Add(new QueryItem(v)));
                        if (s == "Task")
                            MainViewModel.mainViewModel.Target.TargetStory.TaskList.ForEach(v => EntityList.Add(new QueryItem(v)));

                    }
                    foreach (var s in l)
                    {
                        var e = EntityList.FirstOrDefault(v => v.ObjectID == s.ObjectID);
                        if (e != null)
                            e.IsSelected = true;
                    }
                });
            }
        }

        public CommonCommand QueryFateCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    var list = MainViewModel.mainViewModel.Target.TargetStory.GetRelativeFate(GetQueryEntity());
                    RefreshTrack(GetQueryEntity(), list);
                    DrawDiagram();
                });
            }
        }
        void RefreshTrack(List<ISubject> l, List<Tuple<IRelation, IStoryEntityObject, IStoryEntityObject>> fl)
        {
            TrackList.Clear();
            foreach (var s in l)
            {
                var t = new Track() { Name = s.Name };
                var sl = fl.Where(v => v.Item2.ObjectID == s.ObjectID || v.Item3.ObjectID == s.ObjectID);
                foreach (var r in sl)
                {
                    var e = new FateEntity()
                    {
                        Name = s.Name,
                        RelationDescription = r.Item1.Memo,
                        BeginTime = r.Item1.BeginTime,
                        EndTime = r.Item1.EndTime,
                        RelationType = r.Item1.RelationType
                    };
                    if (s.ObjectID == r.Item1.SourceID)
                        e.Description = "To " + r.Item3.Name;
                    if (s.ObjectID == r.Item1.TargetID)
                        e.Description = "From " + r.Item2.Name;
                    t.EntityList.Add(e);

                }
                TrackList.Add(t);
            }
        }
        List<ISubject> GetQueryEntity()
        {
            List<ISubject> l = new List<ISubject>();
            var sl = EntityList.Where(v => v.IsSelected);
            foreach (var s in sl)
            {
                var e = MainViewModel.mainViewModel.Target.TargetStory.GetEntityByID(s.ObjectID);
                if (e != null)
                    l.Add(e);
            }
            return l;
        }


        public void DrawDiagram()
        {
            TargetCanvas.Children.Clear();
            helper.BeginTime = BeginTime;
            helper.EndTime = EndTime;
            helper.SetWidth();
            helper.DrawTimeLine();
            helper.DrawSeparate();
            var th = TrackList.Sum(v => v.Width);
            var d = 0d;
            foreach (var t in TrackList)
            {
                d += helper.DrawTrack(t, th, d);
            }
        }
    }

    class QueryItem : ViewModelBase
    {
        public string Name { get; set; }
        public string Memo { get; set; }

        bool _IsSelected = false;
        public bool IsSelected { get { return _IsSelected; } set { _IsSelected = value; OnPropertyChanged("IsSelected"); } }
        public Guid ObjectID { get; set; }
        public string ObjectType { get; set; }

        public QueryItem(IStoryEntityObject obj)
        {
            Name = obj.Name;
            Memo = obj.Memo;
            IsSelected = false;
            ObjectID = obj.ObjectID;
            if (obj as IActor != null)
                ObjectType = "Actor";
            if (obj as IEvent != null)
                ObjectType = "Event";
            if (obj as IGroup != null)
                ObjectType = "Group";
            if (obj as ILocation != null)
                ObjectType = "Location";
            if (obj as IStuff != null)
                ObjectType = "Stuff";
            if (obj as ITask != null)
                ObjectType = "Task";

        }
    }
}
