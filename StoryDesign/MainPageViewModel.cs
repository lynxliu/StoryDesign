using DesignTool.Lib;
using StoryDesign.Model;
using StoryDesign.View.DetailView;
using StoryDesign.ViewModel;
using StoryDesign.ViewModel.DetailViewModel;
using StoryDesignInterface;
using StoryDesignLib;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UISupport;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using StoryDesign.View.ListView;
using StoryDesignLib.Express;
using DesignTool.Lib.View;
using StoryDesign.View;
using StoryDesignLib.Diagram;
using StoryDesignInterface.Diagram;
using StoryDesign.View.DesignView;
using StoryDesign.ViewModel.DesignViewModel;
using StoryDesignInterface.Express;
using StoryDesign.View.ExpressView;
using StoryDesign.ViewModel.ExpressViewModel;
using Windows.UI.Xaml.Data;
using CommonLib;
using Windows.Storage;
using Windows.Storage.Pickers;
using StoryDesign.View.Control;
using System.IO;
using Windows.ApplicationModel;

namespace StoryDesign
{
    public class MainPageViewModel:ViewModelBase
    {
        public MainPageViewModel()
        {
            //mainViewModel = this;
            //CreateProject();
            //storyInfoView = new StoryInfoView() { DataContext = new StoryInfoViewModel() };
        }
        public static void ActiveMainView()
        {
            var rootFrame = Window.Current.Content as Frame;
            if (rootFrame.Content != null)
            {
                var main = rootFrame.Content as MainPage;
                main.SetView(_mainView);
            }
        }
        static MainView _mainView = new MainView();
        //public MainView CurrentMainView { get { return _mainView; } }
//#region Story Basic Info
//        public string Name
//        {
//            get
//            {
//                return Target.TargetStory.Name;
//            }
//            set { Target.TargetStory.Name = value; OnPropertyChanged("Name"); }
//        }

//        public string Memo
//        {
//            get { return Target.TargetStory.Memo; }
//            set { Target.TargetStory.Memo = value; OnPropertyChanged("Memo"); }
//        }
//        //public DateTime BeginDate
//        //{
//        //    get
//        //    {
//        //        return Target.TargetStory.BeginTime.Date;
//        //    }
//        //    set
//        //    {
//        //        Target.TargetStory.BeginTime =
//        //            new DateTime(value.Year, value.Month, value.Day,
//        //            Target.TargetStory.BeginTime.Hour,
//        //            Target.TargetStory.BeginTime.Minute,
//        //            Target.TargetStory.BeginTime.Second);
//        //        OnPropertyChanged("BeginDate");
//        //    }
//        //}
//        public DateTime BeginTime
//        {
//            get
//            {
//                return 
//                  Target.TargetStory.BeginTime;
//            }
//            set
//            {
//                Target.TargetStory.BeginTime = value;
//                OnPropertyChanged("BeginTime");
//            }
//        }
//        //public DateTime EndDate
//        //{
//        //    get
//        //    {
//        //        return MainPageViewModel.mainViewModel.Target.TargetStory.EndTime.Date;
//        //    }
//        //    set
//        //    {
//        //        Target.TargetStory.BeginTime =
//        //            new DateTime(value.Year, value.Month, value.Day,
//        //            Target.TargetStory.EndTime.Hour,
//        //            Target.TargetStory.EndTime.Minute,
//        //            Target.TargetStory.EndTime.Second);
//        //        OnPropertyChanged("EndDate");
//        //    }
//        //}
//        public DateTime EndTime
//        {
//            get
//            {
//                return
//                  Target.TargetStory.EndTime;
//            }
//            set
//            {
//                Target.TargetStory.EndTime = value;
//                OnPropertyChanged("EndTime");
//            }
//        }

//        public DateTime CreateDate
//        {
//            get { return Target.TargetStory.CreateTime; }
//            set { Target.TargetStory.CreateTime = value; OnPropertyChanged("CreateTime"); }
//        }

//        public string Author
//        {
//            get { return Target.TargetStory.Author; }
//            set { Target.TargetStory.Author = value; OnPropertyChanged("Author"); }
//        }

//        public Version StoryVersion
//        {
//            get { return Target.TargetStory.StoryVersion; }
//            set { Target.TargetStory.StoryVersion = value; OnPropertyChanged("StoryVersion"); }
//        }
//        #endregion

//#region List Info
//        ActorDetailViewModel _CurrentActor;
//        public ActorDetailViewModel CurrentActor { get { return _CurrentActor; } set { CurrentObject = value;_CurrentActor = value;OnPropertyChanged("CurrentActor"); } }
//        ObservableCollection<ActorDetailViewModel> _ActorList = new ObservableCollection<ActorDetailViewModel>();
//        public ObservableCollection<ActorDetailViewModel> ActorList { get { return _ActorList; } }

//        EventDetailViewModel _CurrentEvent;
//        public EventDetailViewModel CurrentEvent { get { return _CurrentEvent; } set { CurrentObject = value; _CurrentEvent = value; OnPropertyChanged("CurrentEvent"); } }
//        ObservableCollection<EventDetailViewModel> _EventList = new ObservableCollection<EventDetailViewModel>();
//        public ObservableCollection<EventDetailViewModel> EventList { get { return _EventList; } }

//        GroupDetailViewModel _CurrentGroup;
//        public GroupDetailViewModel CurrentGroup { get { return _CurrentGroup; } set { _CurrentGroup = value;CurrentObject = value;OnPropertyChanged("CurrentGroup"); } }
//        ObservableCollection <GroupDetailViewModel> _GroupList = new ObservableCollection<GroupDetailViewModel>();
//        public ObservableCollection<GroupDetailViewModel> GroupList { get { return _GroupList; } }

//        LocationDetailViewModel _CurrentLocation;
//        public LocationDetailViewModel CurrentLocation { get { return _CurrentLocation; } set { _CurrentLocation = value;CurrentObject = value;OnPropertyChanged("CurrentLocation"); } }
//        ObservableCollection <LocationDetailViewModel> _LocationList = new ObservableCollection<LocationDetailViewModel>();
//        public ObservableCollection<LocationDetailViewModel> LocationList { get { return _LocationList; } }

//        TaskDetailViewModel _CurrentTask;
//        public TaskDetailViewModel CurrentTask { get { return _CurrentTask; } set { _CurrentTask = value;OnPropertyChanged("CurrentTask");CurrentObject = value; } }
//        ObservableCollection <TaskDetailViewModel> _TaskList = new ObservableCollection<TaskDetailViewModel>();
//        public ObservableCollection<TaskDetailViewModel> TaskList { get { return _TaskList; } }

//        StuffDetailViewModel _CurrentStuff;
//        public StuffDetailViewModel CurrentStuff { get { return _CurrentStuff; } set { _CurrentStuff = value;CurrentObject = value;OnPropertyChanged("CurrentStuff"); } }
//        ObservableCollection <StuffDetailViewModel> _StuffList = new ObservableCollection<StuffDetailViewModel>();
//        public ObservableCollection<StuffDetailViewModel> StuffList { get { return _StuffList; } }

//        RelationDetailViewModel _CurrentRelation;
//        public RelationDetailViewModel CurrentRelation { get { return _CurrentRelation; } set { _CurrentRelation = value;CurrentObject = value;OnPropertyChanged("CurrentRelation"); } }
//        ObservableCollection <RelationDetailViewModel> _RelationList = new ObservableCollection<RelationDetailViewModel>();
//        public ObservableCollection<RelationDetailViewModel> RelationList { get { return _RelationList; } }

//        ExpressViewModel _CurrentExpress;
//        public ExpressViewModel CurrentExpress { get { return _CurrentExpress; } set { _CurrentExpress = value;CurrentObject = value;OnPropertyChanged("CurrentExpress"); } }
//        ObservableCollection <ExpressViewModel> _ExpressList = new ObservableCollection<ExpressViewModel>();
//        public ObservableCollection<ExpressViewModel> ExpressList { get { return _ExpressList; } }

//        StructureDiagramViewModel _CurrentDiagram;
//        public StructureDiagramViewModel CurrentDiagram { get { return _CurrentDiagram; } set { _CurrentDiagram = value;CurrentObject = value;OnPropertyChanged("CurrentDiagram"); } }
//        ObservableCollection <StructureDiagramViewModel> _DiagramList = new ObservableCollection<StructureDiagramViewModel>();
//        public ObservableCollection<StructureDiagramViewModel> DiagramList { get { return _DiagramList; } }

//        ObservableCollection<NoteViewModel> _NoteList = new ObservableCollection<NoteViewModel>();
//        public ObservableCollection<NoteViewModel> NoteList { get { return _NoteList; } }
//        public NoteViewModel CurrentNote { get; set; }

//        EpisodeViewModel _CurrentEpisode;
//        public EpisodeViewModel CurrentEpisode { get { return _CurrentEpisode; } set { _CurrentEpisode = value; CurrentObject = value; OnPropertyChanged("CurrentEpisode"); } }

//        #endregion
//        void RefreshEntityList()
//        {
//            _StoryEntityList.Clear();
//            foreach (var v in ActorList)
//            {
//                _StoryEntityList.Add(new ShowObject() { TargetObject = v });
//            }
//            foreach (var v in EventList)
//            {
//                _StoryEntityList.Add(new ShowObject() { TargetObject = v });
//            }
//            foreach (var v in GroupList)
//            {
//                _StoryEntityList.Add(new ShowObject() { TargetObject = v });
//            }
//            foreach (var v in LocationList)
//            {
//                _StoryEntityList.Add(new ShowObject() { TargetObject = v });
//            }
//            foreach (var v in StuffList)
//            {
//                _StoryEntityList.Add(new ShowObject() { TargetObject = v });
//            }
//            foreach (var v in TaskList)
//            {
//                _StoryEntityList.Add(new ShowObject() { TargetObject = v });
//            }
//        }
//        ObservableCollection<ShowObject> _StoryEntityList = new ObservableCollection<ShowObject>();
//        public ObservableCollection<ShowObject> StoryEntityList
//        {
//            get
//            {
                

//                return _StoryEntityList;
//            }
//        }

//        #region ListView
//        FrameworkElement _ActorListView ;
//        FrameworkElement ActorListView
//        {
//            get
//            {
//                if (_ActorListView == null)
//                    _ActorListView = new ActorListView() { DataContext = this };
//                return _ActorListView;
//            }
//        }
//        FrameworkElement _StuffListView;
//        FrameworkElement StuffListView
//        {
//            get
//            {
//                if (_StuffListView == null)
//                    _StuffListView = new StuffListView() { DataContext = this };
//                return _StuffListView;
//            }
//        }
//        FrameworkElement _EventListView;
//        FrameworkElement EventListView
//        {
//            get
//            {
//                if (_EventListView == null)
//                    _EventListView = new EventListView() { DataContext = this };
//                return _EventListView;
//            }
//        }

//        FrameworkElement _GroupListView;
//        FrameworkElement GroupListView
//        {
//            get
//            {
//                if (_GroupListView == null)
//                    _GroupListView = new GroupListView() { DataContext = this };
//                return _GroupListView;
//            }
//        }

//        FrameworkElement _TaskListView;
//        FrameworkElement TaskListView
//        {
//            get
//            {
//                if (_TaskListView == null)
//                    _TaskListView = new TaskListView() { DataContext = this };
//                return _TaskListView;
//            }
//        }

//        FrameworkElement _ExpressListView;
//        FrameworkElement ExpressListView
//        {
//            get
//            {
//                if (_ExpressListView == null)
//                    _ExpressListView = new ExpressListView() { DataContext = this };
//                return _ExpressListView;
//            }
//        }

//        FrameworkElement _LocationListView;
//        FrameworkElement LocationListView
//        {
//            get
//            {
//                if (_LocationListView == null)
//                    _LocationListView = new LocationListView() { DataContext = this };
//                return _LocationListView;
//            }
//        }

//        FrameworkElement _RelationListView;
//        FrameworkElement RelationListView
//        {
//            get
//            {
//                if (_RelationListView == null)
//                    _RelationListView = new RelationListView() { DataContext = this };
//                return _RelationListView;
//            }
//        }

//        FrameworkElement _StoryInfoView;
//        FrameworkElement StoryListView
//        {
//            get
//            {
//                if (_StoryInfoView == null)
//                    _StoryInfoView = new StoryInfoView() { DataContext = this };
//                return _StoryInfoView;
//            }
//        }

//        FrameworkElement _DiagramListView;
//        FrameworkElement DiagramListView
//        {
//            get
//            {
//                if (_DiagramListView == null)
//                    _DiagramListView = new DiagramListView() { DataContext = this };
//                return _DiagramListView;
//            }
//        }
//        #endregion
//#region EditObjectList
//        public void AddActor() 
//        {
//            var obj = new Actor();
//            Target.TargetStory.ActorList.Add(obj);
//            Target.IsChanged = true;
//            CurrentActor = new ActorDetailViewModel() { TargetObject = obj, GetDetailView = () => { return new ActorDetailView(); } };
//            ActorList.Add(CurrentActor);
//        }
//        public void RemoveActor(ActorDetailViewModel vm)
//        {
//            if (vm != null && ActorList.Contains(vm))
//            {
//                if (Target.TargetStory.ActorList.Contains(vm.TargetObject))
//                    Target.TargetStory.ActorList.Remove(vm.TargetObject);
//                ActorList.Remove(vm);
//                Target.IsChanged = true;
//            }
//            if (CurrentActor == vm)
//                CurrentActor = null;
//        }
//        public void OpenActor(ActorDetailViewModel vm)
//        {
//            if (vm == null) return;
//            OpenView(GetActorView(vm), GetTitle(vm), ViewDataType.EntityObject, vm.Name, vm.ObjectID);

//        }

//        public void AddEvent()
//        {
//            var obj = new Event();
//            Target.TargetStory.EventList.Add(obj);
//            CurrentEvent = new EventDetailViewModel() { TargetObject = obj };
//            EventList.Add(CurrentEvent);
//            Target.IsChanged = true;
//        }
//        public void RemoveEvent(EventDetailViewModel vm)
//        {
//            if (vm != null && EventList.Contains(vm))
//            {
//                if (Target.TargetStory.EventList.Contains(vm.TargetObject))
//                    Target.TargetStory.EventList.Remove(vm.TargetObject);
//                EventList.Remove(vm);
//                Target.IsChanged = true;
//            }
//            if (CurrentEvent == vm)
//                CurrentEvent = null;
//        }
//        public void OpenEvent(EventDetailViewModel vm)
//        {
//            if (vm == null) return;
//            OpenView(GetEventView(vm), GetTitle(vm), ViewDataType.EntityObject, vm.Name, vm.ObjectID);

//        }


//        public void AddGroup()
//        {
//            var obj = new Group();
//            Target.TargetStory.GroupList.Add(obj);
//            CurrentGroup = new GroupDetailViewModel() { TargetObject = obj };
//            GroupList.Add(CurrentGroup);
//            Target.IsChanged = true;
//        }
//        public void RemoveGroup(GroupDetailViewModel vm)
//        {
//            if (vm != null && GroupList.Contains(vm))
//            {
//                if (Target.TargetStory.GroupList.Contains(vm.TargetObject))
//                    Target.TargetStory.GroupList.Remove(vm.TargetObject);
//                GroupList.Remove(vm);
//                Target.IsChanged = true;
//            }
//            if (CurrentGroup == vm)
//                CurrentGroup = null;
//        }
//        public void OpenGroup(GroupDetailViewModel vm)
//        {
//            if (vm == null) return;
//            OpenView(GetGroupView(vm), GetTitle(vm), ViewDataType.EntityObject, vm.Name, vm.ObjectID);

//        }


//        public void AddLocation()
//        {
//            var obj = new Location();
//            Target.TargetStory.LocationList.Add(obj);
//            CurrentLocation = new LocationDetailViewModel() { TargetObject = obj };
//            LocationList.Add(CurrentLocation);
//            Target.IsChanged = true;
//        }
//        public void RemoveLocation(LocationDetailViewModel vm)
//        {
//            if (vm != null && LocationList.Contains(vm))
//            {
//                if (Target.TargetStory.LocationList.Contains(vm.TargetObject))
//                    Target.TargetStory.LocationList.Remove(vm.TargetObject);
//                LocationList.Remove(vm);
//                Target.IsChanged = true;
//            }
//            if (CurrentLocation == vm)
//                CurrentLocation = null;
//        }
//        public void OpenLocation(LocationDetailViewModel vm)
//        {
//            if (vm == null) return;
//            OpenView(GetLocationView(vm), GetTitle(vm), ViewDataType.EntityObject, vm.Name, vm.ObjectID);

//        }


//        public void AddStuff()
//        {
//            var obj = new Stuff();
//            Target.TargetStory.StuffList.Add(obj);
//            CurrentStuff = new StuffDetailViewModel() { TargetObject = obj };
//            StuffList.Add(CurrentStuff);
//            Target.IsChanged = true;
//        }
//        public void RemoveStuff(StuffDetailViewModel vm)
//        {
//            if (vm != null && StuffList.Contains(vm))
//            {
//                if (Target.TargetStory.StuffList.Contains(vm.TargetObject))
//                    Target.TargetStory.StuffList.Remove(vm.TargetObject);
//                StuffList.Remove(vm);
//                Target.IsChanged = true;
//            }
//            if (CurrentStuff == vm)
//                CurrentStuff = null;
//        }
//        public void OpenStuff(StuffDetailViewModel vm)
//        {
//            if (vm == null) return;
//            OpenView(GetStuffView(vm), GetTitle(vm), ViewDataType.EntityObject, vm.Name, vm.ObjectID);

//        }


//        public void AddTask()
//        {
//            var obj = new StoryDesignLib.Task();
//            Target.TargetStory.TaskList.Add(obj);
//            CurrentTask = new TaskDetailViewModel() { TargetObject = obj };
//            TaskList.Add(CurrentTask);
//            Target.IsChanged = true;
//        }
//        public void RemoveTask(TaskDetailViewModel vm)
//        {
//            if (vm != null && TaskList.Contains(vm))
//            {
//                if (Target.TargetStory.TaskList.Contains(vm.TargetObject))
//                    Target.TargetStory.TaskList.Remove(vm.TargetObject);
//                TaskList.Remove(vm);
//                Target.IsChanged = true;
//            }
//            if (CurrentTask == vm)
//                CurrentTask = null;
//        }
//        public void OpenTask(TaskDetailViewModel vm)
//        {
//            if (vm == null) return;
//            OpenView(GetTaskView(vm), GetTitle(vm), ViewDataType.EntityObject, vm.Name, vm.ObjectID);

//        }


//        public void AddDiagram()
//        {
//            var obj = new StructureDiagram();
//            Target.TargetStory.StructureDiagramList.Add(obj);
//            CurrentDiagram = new StructureDiagramViewModel() { TargetObject = obj };
//            DiagramList.Add(CurrentDiagram);
//            Target.IsChanged = true;
//        }
//        public void RemoveDiagram(StructureDiagramViewModel vm)
//        {
//            if (vm != null && DiagramList.Contains(vm))
//            {
//                if (Target.TargetStory.StructureDiagramList.Contains(vm.TargetObject))
//                    Target.TargetStory.StructureDiagramList.Remove(vm.TargetObject);
//                DiagramList.Remove(vm);
//                Target.IsChanged = true;
//            }
//            if (CurrentDiagram == vm)
//                CurrentDiagram = null;
//        }
//        public void OpenDiagram(StructureDiagramViewModel vm)
//        {
//            if (vm == null) return;
//            OpenView(GetDiagramView(vm), GetTitle(vm), ViewDataType.Diagram, vm.Name, Guid.Empty);

//        }


//        public void AddExpress()
//        {
//            var obj = new Express();
//            Target.TargetStory.ExpressList.Add(obj);
//            CurrentExpress = new ExpressViewModel() { TargetObject = obj };
//            ExpressList.Add(CurrentExpress);
//            Target.IsChanged = true;
//        }
//        public void RemoveExpress(ExpressViewModel vm)
//        {
//            if (vm != null && ExpressList.Contains(vm))
//            {
//                if (Target.TargetStory.ExpressList.Contains(vm.TargetObject))
//                    Target.TargetStory.ExpressList.Remove(vm.TargetObject);
//                ExpressList.Remove(vm);
//                Target.IsChanged = true;
//            }
//            if (CurrentExpress == vm)
//                CurrentExpress = null;
//        }
//        public void OpenExpress(ExpressViewModel vm)
//        {
//            if (vm == null) return;
//            OpenView(GetExpressView(vm), GetTitle(vm), ViewDataType.Express, vm.Name, Guid.Empty);

//        }
//        public void OpenEpisode(EpisodeViewModel vm)
//        {
//            if (vm == null) return;
//            OpenView(GetEpisodeView(vm), GetTitle(vm), ViewDataType.Express, vm.Title, Guid.Empty);

//        }
//        public void AddRelation()
//        {
//            var obj = new Relation();
//            var vm = new RelationDetailViewModel() { TargetObject = obj };
//            Target.TargetStory.RelationList.Add(obj);
//            RelationList.Add(vm);
//            Target.IsChanged = true;
//            CurrentRelation = vm;
//        }
//        public void RemoveRelation(RelationDetailViewModel vm)
//        {
//            if (vm != null && RelationList.Contains(vm))
//                RelationList.Remove(vm);
//            Target.TargetStory.RelationList.Remove(vm.TargetObject);
//            Target.IsChanged = true;
//            if (CurrentRelation==vm)
//                CurrentRelation = null;

//        }

//        public void OpenRelation(RelationDetailViewModel vm)
//        {
//            if (vm != null)
//                OpenView(GetRelationView(vm), CurrentRelation.RelationShowTitle, ViewDataType.Relation, null, CurrentRelation.TargetObject.ObjectID);

//        }

//        public void OpenFate(ISubject fateObject)
//        {
//            if (fateObject == null) return;
//            fateObject.RefreshFateDiagram();
//            var view = new FateDiagramView() { DataContext = new FateDiagramViewModel() { TargetObject = fateObject.TargetFate } };
//            OpenView(view, GetTitle(fateObject), ViewDataType.Fate, null, fateObject.ObjectID);

//        }
//        #endregion
//        #region ListViewCommand
//        public CommonCommand AddActorCommand
//        {
//            get {
//                return new CommonCommand((o) =>
//              {
//                  AddActor();
//              });
//            }
//        }

//        public CommonCommand RemoveActorCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    RemoveActor(CurrentActor);
//                });
//            }
//        }

//        public CommonCommand OpenActorCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    OpenActor(CurrentActor);
//                });
//            }
//        }

//        public CommonCommand AddEventCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    AddEvent();
//                });
//            }
//        }

//        public CommonCommand RemoveEventCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    RemoveEvent(CurrentEvent);
//                });
//            }
//        }

//        public CommonCommand OpenEventCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    OpenEvent(CurrentEvent);
//                });
//            }
//        }

//        public CommonCommand AddGroupCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    AddGroup();
//                });
//            }
//        }

//        public CommonCommand RemoveGroupCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    RemoveGroup(CurrentGroup);
//                });
//            }
//        }

//        public CommonCommand OpenGroupCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    OpenGroup(CurrentGroup);
//                });
//            }
//        }

//        public CommonCommand AddLocationCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    AddLocation();
//                });
//            }
//        }

//        public CommonCommand RemoveLocationCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    RemoveLocation(CurrentLocation);
//                });
//            }
//        }

//        public CommonCommand OpenLocationCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    OpenLocation(CurrentLocation);
//                });
//            }
//        }

//        public CommonCommand AddTaskCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    AddTask();
//                });
//            }
//        }

//        public CommonCommand RemoveTaskCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    RemoveTask(CurrentTask);
//                });
//            }
//        }

//        public CommonCommand OpenTaskCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    OpenTask(CurrentTask);
//                });
//            }
//        }

//        public CommonCommand AddStuffCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    AddStuff();
//                });
//            }
//        }

//        public CommonCommand RemoveStuffCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    RemoveStuff(CurrentStuff);
//                });
//            }
//        }

//        public CommonCommand OpenStuffCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    OpenStuff(CurrentStuff);
//                });
//            }
//        }

//        public CommonCommand OpenActorFateCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {

//                    if (CurrentActor != null)
//                    {
//                        OpenFate(CurrentActor.TargetObject);
//                    }
//                });
//            }
//        }
//        public CommonCommand OpenEventFateCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {

//                    if (CurrentEvent != null)
//                    {
//                        OpenFate(CurrentEvent.TargetObject);
//                    }
//                });
//            }
//        }
//        public CommonCommand OpenGroupFateCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {

//                    if (CurrentGroup != null)
//                    {
//                        OpenFate(CurrentGroup.TargetObject);
//                    }
//                });
//            }
//        }
//        public CommonCommand OpenLocationFateCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {

//                    if (CurrentLocation != null)
//                    {
//                        OpenFate(CurrentLocation.TargetObject);
//                    }
//                });
//            }
//        }
//        public CommonCommand OpenTaskFateCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {

//                    if (CurrentTask != null)
//                    {
//                        OpenFate(CurrentTask.TargetObject);
//                    }
//                });
//            }
//        }
//        public CommonCommand OpenStuffFateCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {

//                    if (CurrentStuff != null)
//                    {
//                        OpenFate(CurrentStuff.TargetObject);
//                    }
//                });
//            }
//        }
//        public CommonCommand AddDiagramCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    AddDiagram();
//                });
//            }
//        }
//        public CommonCommand RemoveDiagramCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    RemoveDiagram(CurrentDiagram);
//                });
//            }
//        }
//        public CommonCommand OpenDiagramCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    OpenDiagram(CurrentDiagram);
//                });
//            }
//        }
//        public CommonCommand AddExpressCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    AddExpress();
//                });
//            }
//        }
//        public CommonCommand RemoveExpressCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    RemoveExpress(CurrentExpress);
//                });
//            }
//        }
//        public CommonCommand OpenExpressCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    OpenExpress(CurrentExpress);
//                });
//            }
//        }
//        public CommonCommand OpenEpisodeCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    OpenEpisode(CurrentEpisode);
//                });
//            }
//        }
//        public CommonCommand AddRelationCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    AddRelation();
//                });
//            }
//        }
//        public CommonCommand RemoveRelationCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    RemoveRelation(CurrentRelation);
//                });
//            }
//        }
//        public CommonCommand OpenRelationCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    OpenRelation(CurrentRelation);
//                });
//            }
//        }
//        #endregion

//        #region NoteCommand
//        public CommonCommand AddNoteCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    CurrentNote = new NoteViewModel() { TargetObject = new Note() };
//                    Target.IsChanged = true;
//                    NoteList.Add(CurrentNote);
//                    SaveNoteList();
//                });
//            }
//        }
//        public CommonCommand RemoveNoteCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    if (CurrentNote != null)
//                    {
//                        NoteList.Remove(CurrentNote);
//                        Target.IsChanged = true;
//                    }
//                        SaveNoteList();
//                });
//            }
//        }
//        void SaveNoteList()
//        {
//            Target.TargetStory.NoteList.Clear();
//            foreach (var note in NoteList)
//            {
//                Target.TargetStory.NoteList.Add(note.TargetObject);
//            }
//            CurrentNote = NoteList.FirstOrDefault();
//        }
//        void RefreshNoteList()
//        {
//            if (Target == null||Target.TargetStory==null) return;
//            NoteList.Clear();
//            foreach (var note in Target.TargetStory.NoteList)
//            {
//                NoteList.Add(new NoteViewModel() { TargetObject = note });
//            }
//            CurrentNote = NoteList.FirstOrDefault();
//        }
//        #endregion

//        #region UIOption
//        public double ListPaneWidth
//        {
//            get { if (IsFullScreen) return 0; return Target.PaneWidth ; }
//            set { Target.PaneWidth = value;OnPropertyChanged("ListPaneWidth"); }
//        }

//        public double AdditionPanelWidth
//        {
//            get { if (IsFullScreen) return 0; return Target.AdditionPanelWidth; }
//            set { Target.AdditionPanelWidth = value; OnPropertyChanged("AdditionPanelWidth"); }
//        }
//        void ChangeFullScreen(bool isFullScren)
//        {
//            if (isFullScren)
//            {
//                _PaneDisplayMode = PaneDisplayMode;
//                _IsPaneOpen = IsPaneOpen;
//                _IsPanePinned = IsPanePinned;
//                PaneDisplayMode = SplitViewDisplayMode.Overlay;
//                IsPaneOpen = false;
//                IsPanePinned = false;
//            }
//            else
//            {
//                PaneDisplayMode = _PaneDisplayMode;
//                IsPaneOpen = _IsPaneOpen;
//                IsPanePinned = _IsPanePinned;
//            }
//        }
//        SplitViewDisplayMode _PaneDisplayMode;
//        bool _IsPaneOpen;
//        bool _IsPanePinned;
//        public bool IsFullScreen
//        {
//            get { return Target.IsFullScreen; }
//            set { Target.IsFullScreen = value;OnPropertyChanged("IsFullScreen");
//                ChangeFullScreen(value);
//            }
//        }

//        public bool IsPaneOpen
//        {
//            get {  return Target.IsPaneOpen;  }
//            set {

//                Target.IsPaneOpen = value; OnPropertyChanged("IsPaneOpen"); }
//        }
//        public bool IsAdditionPaneOpen
//        {
//            get { return Target.IsAdditionPaneOpen; }
//            set {
//                if (value) AdditionPaneDisplayMode = SplitViewDisplayMode.Inline;
//                else AdditionPaneDisplayMode = SplitViewDisplayMode.CompactOverlay;
//                Target.IsAdditionPaneOpen = value; OnPropertyChanged("IsAdditionPaneOpen"); }
//        }
//        //GridLength _AdditionPane1stRowHeight = new GridLength(50, GridUnitType.Star);
//        GridLength AdditionPane1stRowHeight
//        {
//            get { return new GridLength(Target.AdditionPane1stRowHeight, GridUnitType.Star) ; }
//            set { Target.AdditionPane1stRowHeight = value.Value;OnPropertyChanged("AdditionPane1stRowHeight"); }
//        }
//        //GridLength _AdditionPane2ndRowHeight = new GridLength(50, GridUnitType.Star);

//        public bool IsPanePinned
//        {
//            get { return Target.IsPanePinned; }
//            set {
//                if (value)
//                {
//                    PaneDisplayMode = SplitViewDisplayMode.Inline;
//                }
//                else
//                {
//                    PaneDisplayMode = SplitViewDisplayMode.CompactOverlay;
//                }
//                IsPaneOpen = true;
//                Target.IsPanePinned = value;
//                OnPropertyChanged("IsPanePinned"); }
//        }
//        public bool IsAdditionPanePinned
//        {
//            get { return Target.IsAdditionPanePinned; }
//            set {
//                if (value) AdditionPaneDisplayMode = SplitViewDisplayMode.Inline;
//                else AdditionPaneDisplayMode = SplitViewDisplayMode.Overlay;
//                IsAdditionPaneOpen = true;
//                Target.IsAdditionPanePinned = value;  OnPropertyChanged("IsAdditionPanePinned"); }
//        }
//        bool _IsNoteListView = true;
//        public bool IsNoteListView
//        {
//            get { return _IsNoteListView; }
//            set { _IsNoteListView = value;OnPropertyChanged("IsNoteListView");OnPropertyChanged("IsPropertyView"); }
//        }
//        //public bool IsPropertyView
//        //{
//        //    get { return !_IsNoteListView; }
//        //    set { _IsNoteListView = !value; OnPropertyChanged("IsNoteListView"); OnPropertyChanged("IsPropertyView"); }
//        //}
//        public SplitViewDisplayMode PaneDisplayMode
//        {
//            get { return Target.PaneDisplayMode; }
//            set { Target.PaneDisplayMode = value;OnPropertyChanged("PaneDisplayMode"); }
//        }
//        public SplitViewDisplayMode AdditionPaneDisplayMode
//        {
//            get { return Target.AdditionPaneDisplayMode; }
//            set { Target.AdditionPaneDisplayMode = value; OnPropertyChanged("AdditionPaneDisplayMode"); }
//        }
//        #endregion

//        FrameworkElement actorPropertyView = new ActorDetailView();
//        FrameworkElement eventPropertyView = new EventDetailView();
//        FrameworkElement groupPropertyView = new GroupDetailView();
//        FrameworkElement locationPropertyView = new LocationDetailView();
//        FrameworkElement stuffPropertyView = new StuffDetailView();
//        FrameworkElement taskPropertyView = new TaskDetailView();

//        FrameworkElement relationPropertyView = new RelationDetailView();

//        FrameworkElement diagramPropertyView = new StructureDiagramInfoView();
//        FrameworkElement fatePropertyView = new FateDiagramInfoView();
//        FrameworkElement expressPropertyView = new ExpressInfoView();

//        FrameworkElement storyPropertyView = new StoryInfoView();

//        object _CurrentObject;
//        public object CurrentObject
//        {
//            get { return _CurrentObject; }
//            set {
//                _CurrentObject = value;
//                //RefreshNoteList();

//                if (value != null)
//                {
//                    if((value as ActorDetailViewModel) != null)
//                    {
//                        actorPropertyView.DataContext=value;
//                        PropertyView.Child = actorPropertyView;
//                    }
//                    if ((value as EventDetailViewModel) != null)
//                    {
//                        eventPropertyView.DataContext = value;
//                        PropertyView.Child = eventPropertyView;
//                    }
//                    if ((value as GroupDetailViewModel) != null)
//                    {
//                        groupPropertyView.DataContext = value;
//                        PropertyView.Child = groupPropertyView;
//                    }
//                    if ((value as LocationDetailViewModel) != null)
//                    {
//                        locationPropertyView.DataContext = value;
//                        PropertyView.Child = locationPropertyView;
//                    }
//                    if ((value as StuffDetailViewModel) != null)
//                    {
//                        stuffPropertyView.DataContext = value;
//                        PropertyView.Child = stuffPropertyView;
//                    }
//                    if ((value as TaskDetailViewModel) != null)
//                    {
//                        taskPropertyView.DataContext = value;
//                        PropertyView.Child = taskPropertyView;
//                    }
//                    if ((value as RelationDetailViewModel) != null)
//                    {
//                        relationPropertyView.DataContext = value;
//                        PropertyView.Child = relationPropertyView;
//                    }
//                    if ((value as StructureDiagramViewModel) != null)
//                    {
//                        diagramPropertyView.DataContext = value;
//                        PropertyView.Child = diagramPropertyView;
//                    }
//                    if ((value as FateDiagramViewModel) != null)
//                    {
//                        fatePropertyView.DataContext = value;
//                        PropertyView.Child = fatePropertyView;
//                    }

//                    if ((value as ExpressViewModel) != null)
//                    {
//                        expressPropertyView.DataContext = value;
//                        PropertyView.Child = expressPropertyView;
//                    }
//                    if ((value as MainPageViewModel) != null)
//                    {
//                        storyPropertyView.DataContext = value;
//                        PropertyView.Child = storyPropertyView;
//                    }

//                }

//                if(CurrentView != null&&CurrentView.ViewType== ViewDataType.Diagram&& value!=null)
//                {
//                    var entity = (value as IStoryEntityObject);
//                    if (entity != null)
//                        (CurrentView.View.DataContext as StructureDiagramViewModel).TargetDesignCanvasViewModel.ActiveData(entity.ObjectID);
//                    var link = value as IRelation;
//                    if(link!=null)
//                        (CurrentView.View.DataContext as StructureDiagramViewModel).TargetDesignCanvasViewModel.ActiveData(link.ObjectID);

//                }
//                OnPropertyChanged("CurrentObject");
//                OnPropertyChanged("CurrentObjectTitle");
//                //if(value as IUIActiveSupport != null)
//                //{
//                //    (dc as IUIActiveSupport).SetActive.Add(() => { vm.IsActive = true});
//                //}
//            }
//        }

//        public string CurrentObjectTitle
//        {
//            get
//            {
//                if (CurrentObject != null)
//                {
//                    if ((CurrentObject as RelationDetailViewModel) != null)
//                        return GetRelationTitle(CurrentObject as RelationDetailViewModel);
//                    var s = GetTitle(CurrentObject);
//                    return s;
//                }
//                return "";
//            }
//        }


//#region ViewControl
//        ObservableCollection<ViewInfo> _ViewList = new ObservableCollection<ViewInfo>();
//        public ObservableCollection<ViewInfo> ViewList { get { return _ViewList; } }

//        Dictionary<string,object> GetViewModelList(object data)
//        {
//            Dictionary<string, object> ol = new Dictionary<string, object>();
//            foreach (var v in ViewList)
//            {
//                var vm = v.View.DataContext;
//                if (vm != null)
//                {
//                    var dataObject = GetDataObject(vm);

//                    if (dataObject != null)
//                    {
//                        if (dataObject == data)
//                            ol.Add(v.Title,vm);
//                    }
//                }
//            }
//            return ol;
//        }

//        public async void RefreshAllView(object viewModel)
//        {
//            var vmList = GetViewModelList(GetDataObject(viewModel));
//            foreach (var v in vmList)
//            {
//                if (v.Value != viewModel)
//                {
//                    var isChangedProperty = v.GetType().GetProperty("IsChanged");
//                    if (isChangedProperty == null)
//                    {
//                        isChangedProperty = v.GetType().GetProperty("Changed");
//                    }
//                    if (isChangedProperty != null)
//                    {
//                        var isChanged = (bool)isChangedProperty.GetValue(v.Value);
//                        if (isChanged)
//                            if(await CommonLib.CommonProc.Confirm("Confirm Change",
//                                "View " + v.Key + " contains value changed, confirm save value and overwrite any inconsistent information"
//                                ))
//                                LoadValueToDataObject(v.Value);
//                            else
//                            LoadValueToDataObject(v.Value);
//                    }
//                }
//            };
//        }

//        void ActiveView(FrameworkElement view)
//        {
//            if (view == null) return;
//            //MainContent.Children.Clear();
//            MainContent.Child = view;

//            CurrentObject = view.DataContext;
//        }
//        public ViewInfo CurrentView
//        {
//            get
//            {
//                return Target.CurrentWorkView;
//            }
//            set
//            {
//                Target.CurrentWorkView = value;
//                if (value != null && value.View != null && value.View.DataContext != null)
//                {
//                    ActiveView(value.View);

//                    //Title = Story.CurrentStory.Name + " : " + value.Title;
//                }

//                OnPropertyChanged("CurrentView");
//                OnPropertyChanged("Title");
//            }
//        }

//        #endregion

//#region ViewControl
//        public void OpenViewInfo(ViewInfo info)
//        {
//            if (info == null) return;
//            if (info.View == null)
//            {
//                if(info.ViewType== ViewDataType.Diagram)
//                {
//                    var obj = Target.TargetStory.StructureDiagramList.FirstOrDefault(v => v.Name == info.TargetName);
//                    if (obj != null)
//                        info.View = GetDiagramView(obj);
//                }
//                if (info.ViewType == ViewDataType.EntityObject)
//                {
//                    var id = info.TargetObjectID;
//                    var obj = Target.TargetStory.GetEntityByID(id);
//                    if (obj != null)
//                    {
//                        if ((obj as IActor) != null)
//                            info.View = GetActorView(obj as IActor);
//                        if ((obj as IEvent) != null)
//                            info.View = GetEventView(obj as IEvent);
//                        if ((obj as IGroup) != null)
//                            info.View = GetGroupView(obj as IGroup);
//                        if ((obj as ITask) != null)
//                            info.View = GetTaskView(obj as ITask);
//                        if ((obj as ILocation) != null)
//                            info.View = GetLocationView(obj as ILocation);
//                        if ((obj as IStuff) != null)
//                            info.View = GetStuffView(obj as IStuff);

//                    }
//                }
//                if (info.ViewType == ViewDataType.Express)
//                {
//                    var obj = Target.TargetStory.ExpressList.FirstOrDefault(v => v.Name == info.TargetName);
//                    if (obj != null)
//                        info.View = GetExpressView(obj);

//                }
//                if (info.ViewType == ViewDataType.Episode)
//                {
//                    var express = Target.TargetStory.ExpressList.FirstOrDefault(v => info.Title.StartsWith(v.Name+":"));
//                    if (express != null)
//                    {
//                        var obj = express.EpisodeList.FirstOrDefault(v => info.Title.EndsWith(":" + v.EpisodeName));
//                        if (obj != null)
//                            info.View =GetEpisodeView(obj);


//                    }
//                }
//                if (info.ViewType == ViewDataType.Relation)
//                {
//                    var id = info.TargetObjectID;
//                    var obj = Target.TargetStory.RelationList.FirstOrDefault(v => v.ObjectID == id);
//                    if (obj != null)
//                        info.View = GetRelationView(obj);

//                }
//                if (info.ViewType == ViewDataType.Fate)
//                {
//                    var id = info.TargetObjectID;
//                    var obj = Target.TargetStory.GetEntityByID(id);

//                    if (obj != null)
//                    {
//                        obj.RefreshFateDiagram();
//                        info.View = GetFateView(obj.TargetFate);
//                    }
//                }
//                //if (info.ViewType == ViewDataType.RelativeFate)
//                //{
//                //    var vm = new RelativeFateViewModel() { info=info};
//                //    info.TargetObjectIdList.ForEach(v => vm.TargetIdList.Add(v));
//                //    info.View = new RelativeFateView() { DataContext = vm };
//                //    vm.Refresh();
//                //    //var obj = Target.TargetStory.GetEntityByID(info.TargetObjectID);

//                //    //if (obj != null)
//                //    //{
//                //    //    obj.RefreshFateDiagram();
//                //    //    info.View = GetFateView(obj.TargetFate);
//                //    //}
//                //}

//            }
//            if (info.View != null)
//            {
//                if (MainContent.Child != info.View) MainContent.Child = info.View;
//                if (!ViewList.Contains(info))
//                    ViewList.Add(info);
//                Target.CurrentWorkView = info;
//                CurrentView = info;
//            }

//        }
//        string GetRelationTitle(RelationDetailViewModel r)
//        {
//            if (r == null || Target.TargetStory == null) return null;
//            var s = Target.TargetStory.GetEntityByID(r.SourceID);
//            var t = Target.TargetStory.GetEntityByID(r.TargetID);
//            var m = "";
//            if (!string.IsNullOrEmpty(r.Memo))
//                m = "(" + r.Memo + ")";
//            var ts = "";
//            if (s != null)
//                ts = "From " + s.Name;
//            if (t != null)
//                ts += " to " + t.Name;
//            return ts + m;
//        }
//        string GetTitle(object obj)
//        {
//            if (obj == null) return null;
//            var p = obj.GetType().GetProperty("Title");
//            object s;
//            if (p != null)
//            {
//                s = p.GetValue(obj);
//                if (s == null) return null;
//                return s.ToString();
//            }
//            p = obj.GetType().GetProperty("Name");
//            if (p != null)
//            {
//                s = p.GetValue(obj);
//                if (s == null) return null;
//                return s.ToString();
//            }
//            p = obj.GetType().GetProperty("Description");
//            if (p != null)
//            {
//                s = p.GetValue(obj);
//                if (s == null) return null;
//                return s.ToString();
//            }
//            return null;
//        }

//        #endregion
//        void LoadValueToDataObject(object vm)
//        {
//            if (vm == null)
//                return;
//            var save = vm.GetType().GetMethod("Load");
//            if (save == null)
//                save = vm.GetType().GetMethod("LoadValue");
//            if (save == null)
//                save = vm.GetType().GetMethod("LoadInfo");
//            if (save == null)
//                return;
//            save.Invoke(vm, new object[0]);
//        }
//        void SaveValueToDataObject(object vm)
//        {
//            if (vm == null)
//                return;
//            var save = vm.GetType().GetMethod("Save");
//            if (save == null)
//                save = vm.GetType().GetMethod("SaveValue");
//            if (save == null)
//                save = vm.GetType().GetMethod("SaveInfo");
//            if (save == null)
//                return;
//            save.Invoke(vm,new object[0]);
//        }
//        object GetDataObject(object viewModel)
//        {
//            var dataObjectProperty = viewModel.GetType().GetProperty("TargetObject");
//            if (dataObjectProperty == null)
//            {
//                dataObjectProperty = viewModel.GetType().GetProperty("Target");
//            }
//            if (dataObjectProperty == null)
//            {
//                dataObjectProperty = viewModel.GetType().GetProperty("Data");
//            }
//            if (dataObjectProperty == null)
//            {
//                dataObjectProperty = viewModel.GetType().GetProperty("DataObject");
//            }
//            if (dataObjectProperty == null)
//            {
//                dataObjectProperty = viewModel.GetType().GetProperty("Value");
//            }
//            return dataObjectProperty.GetValue(viewModel);
//        }

//        #region Get Object View
//        public StructureDiagramView GetDiagramView(StructureDiagramViewModel diagram)
//        {
//            if (diagram == null||diagram.TargetObject==null) return null;
            
//            return new StructureDiagramView() { DataContext = diagram };
//        }
//        public StructureDiagramView GetDiagramView(IStructureDiagram diagram)
//        {
//            if (diagram == null) return null;
//            var dc = DiagramList.FirstOrDefault(v => v.TargetObject.Name == diagram.Name);
//            if (dc == null)
//            {
//                dc = new StructureDiagramViewModel() { TargetObject = diagram };
//            }
//            return GetDiagramView(dc);
//        }
//        public FateDiagramView GetFateView(FateDiagramViewModel diagram)
//        {
//            if (diagram == null || diagram.TargetObject == null) return null;

//            return new FateDiagramView() { DataContext = diagram };
//        }
//        public FateDiagramView GetFateView(IFateDiagram diagram)
//        {
//            if (diagram == null) return null;

//            var dc = new FateDiagramViewModel() { TargetObject = diagram };
//            return GetFateView(dc);
//        }
//        public EpisodeView GetEpisodeView(EpisodeViewModel vm)
//        {
//            if (vm == null || vm.TargetObject == null) return null;
//            var view = new EpisodeView() { DataContext = vm };
//            return view;
//        }
//        public EpisodeView GetEpisodeView(IEpisode episode)
//        {
//            if (episode == null||string.IsNullOrEmpty(episode.ExpressName)) return null;
//            EpisodeViewModel dc=null;
//            foreach(var ex in ExpressList)
//            {
//                if (episode.ExpressName == ex.Name)
//                {
//                    var t = ex.EpisodeList.FirstOrDefault(v => v.EpisodeName == episode.EpisodeName);
//                    if (t != null)
//                        dc = t;
//                }
//            }

//            if (dc == null)
//            {
//                dc = new EpisodeViewModel() { TargetObject = episode };
//            }
//            return GetEpisodeView(dc);
//        }
//        public ExpressView GetExpressView(ExpressViewModel vm)
//        {
//            if (vm == null) return null;
//            var view = new ExpressView() { DataContext = vm };
//            return view;
//        }
//        public ExpressView GetExpressView(IExpress diagram)
//        {
//            if (diagram == null) return null;
//            var dc = ExpressList.FirstOrDefault(v => v.TargetObject.Name == diagram.Name);
//            if (dc == null)
//            {
//                dc = new ExpressViewModel() { TargetObject = diagram };
//            }
//            return GetExpressView(dc);
//        }
//        public ActorDetailView GetActorView(ActorDetailViewModel vm)
//        {
//            if (vm == null) return null;
            
//            var view = new ActorDetailView() { DataContext = vm };
//            return view;
//        }
//        public ActorDetailView GetActorView(IActor entity)
//        {
//            if (entity == null) return null;
//            var dc = ActorList.FirstOrDefault(v => v.TargetObject.ObjectID == entity.ObjectID);
//            if (dc == null)
//            {
//                dc = new ActorDetailViewModel() { TargetObject = entity };
//            }
//            return GetActorView(dc);
//        }
//        public StuffDetailView GetStuffView(StuffDetailViewModel vm)
//        {
//            if (vm == null) return null;
//            return new StuffDetailView() { DataContext = vm };
//        }
//        public StuffDetailView GetStuffView(IStuff entity)
//        {
//            if (entity == null) return null;
//            var dc = StuffList.FirstOrDefault(v => v.TargetObject.ObjectID == entity.ObjectID);
//            if (dc == null)
//            {
//                dc = new StuffDetailViewModel() { TargetObject = entity };
//            }
//            return GetStuffView(dc);
//        }
//        public GroupDetailView GetGroupView(GroupDetailViewModel vm)
//        {
//            if (vm == null) return null;

//            return new GroupDetailView() { DataContext = vm };
//        }
//        public GroupDetailView GetGroupView(IGroup entity)
//        {
//            if (entity == null) return null;
//            var dc = GroupList.FirstOrDefault(v => v.TargetObject.ObjectID == entity.ObjectID);
//            if (dc == null)
//            {
//                dc = new GroupDetailViewModel() { TargetObject = entity };
//            }
//            return GetGroupView(dc);
//        }
//        public EventDetailView GetEventView(EventDetailViewModel vm)
//        {
//            if (vm == null) return null;

//            return new EventDetailView() { DataContext = vm };
//        }
//        public EventDetailView GetEventView(IEvent entity)
//        {
//            if (entity == null) return null;
//            var dc = EventList.FirstOrDefault(v => v.TargetObject.ObjectID == entity.ObjectID);
//            if (dc == null)
//            {
//                dc = new EventDetailViewModel() { TargetObject = entity };
//            }
//            return GetEventView(dc);
//        }
//        public TaskDetailView GetTaskView(TaskDetailViewModel vm)
//        {
//            if (vm == null) return null;
//            return new TaskDetailView() { DataContext = vm };
//        }
//        public TaskDetailView GetTaskView(ITask entity)
//        {
//            if (entity == null) return null;
//            var dc = TaskList.FirstOrDefault(v => v.TargetObject.ObjectID == entity.ObjectID);
//            if (dc == null)
//            {
//                dc = new TaskDetailViewModel() { TargetObject = entity };
//            }
//            return GetTaskView(dc);
//        }
//        public LocationDetailView GetLocationView(LocationDetailViewModel vm)
//        {
//            if (vm == null) return null;
//            return new LocationDetailView() { DataContext = vm };
//        }
//        public LocationDetailView GetLocationView(ILocation entity)
//        {
//            if (entity == null) return null;
//            var dc = LocationList.FirstOrDefault(v => v.TargetObject.ObjectID == entity.ObjectID);
//            if (dc == null)
//            {
//                dc = new LocationDetailViewModel() { TargetObject = entity };
//            }
//            return GetLocationView(dc);
//        }
//        public RelationDetailView GetRelationView(RelationDetailViewModel vm)
//        {
//            if (vm == null) return null;
//            RefreshEntityList();
//            return new RelationDetailView() { DataContext = vm };
//        }
//        public RelationDetailView GetRelationView(IRelation entity)
//        {
//            if (entity == null) return null;
//            var dc = RelationList.FirstOrDefault(v => v.TargetObject.ObjectID == entity.ObjectID);
//            if (dc == null)
//            {
//                dc = new RelationDetailViewModel() { TargetObject = entity };
//            }
//            return GetRelationView(dc);
//        }
//        #endregion

//#region static function
//        //static List<Tuple<string, Func<object, FrameworkElement>, Func<object, string>>> _CommandList = new List<Tuple<string, Func<object, FrameworkElement>, Func<object, string>>>();
//        //public static List<Tuple<string, Func<object, FrameworkElement>, Func<object, string>>> CommandList { get { return _CommandList; } }
//        public static MainPageViewModel mainViewModel { get; set; }
//        public static void OpenView(FrameworkElement view,string title, ViewDataType viewType,string name,Guid id)
//        {
//            if(view==null)
//            {
//                CommonLib.CommonProc.ShowMessage("Error", "No valid view to show");
//                return;
//            }
//            var dc = view.DataContext;
//            var ev = mainViewModel.ViewList.FirstOrDefault(v => v.View != null && v.View.DataContext == dc);
//            if (ev != null)
//                mainViewModel.CurrentView=ev;
//            else
//            {

//                ViewInfo info = new ViewInfo() { View = view,Title=title,  ViewType=viewType, TargetName=name  };
//                info.TargetObjectID=id;
//                //var w = CommonLib.CommonProc.GetMainPage();
//                //var dc = w.DataContext as MainPageViewModel;
//                //Binding
//                mainViewModel.OpenViewInfo(info);
//            }
//        }
//        public static void SetCurrent(object obj)
//        {
//            //var w = CommonLib.CommonProc.GetMainPage();
//            //if (w == null) return;
//            //var dc = w.DataContext as MainPageViewModel;
//            //if (dc == null) return;
//            if (mainViewModel == null) return ;
//            mainViewModel.CurrentObject = obj;
//        }
//        public static object GetCurrent()
//        {
//            //var w = CommonLib.CommonProc.GetMainPage();
//            //if (w == null)
//            //    return null;
//            //var dc = w.DataContext as MainPageViewModel;
//            //if (dc == null)
//            //    return null;
//            if (mainViewModel == null) return null;
//            return mainViewModel.CurrentObject;
//        }
//        public static void RefreshTitle(string title,object obj)
//        {
//            foreach(var view in mainViewModel.ViewList)
//            {
//                if (view.View.DataContext == obj)
//                    view.Title = title;
//            }
//        }
//        #endregion
//        //public static void OpenStoryObject(object obj)
//        //{
//        //    var w = CommonLib.CommonProc.GetMainPage();
//        //    var dc = w.DataContext as MainPageViewModel;
//        //    dc.OpenObject(obj);
//        //}

//        //public void OpenObject(object obj)
//        //{
//        //    if (typeof(IActor).IsAssignableFrom(obj.GetType()))
//        //    {
//        //        var view=OpenActor(obj as IActor);
//        //        OpenView(view, GetTitle(obj),"OpenActor");
//        //    }
//        //    if (typeof(IStuff).IsAssignableFrom(obj.GetType()))
//        //    {
//        //        var view = OpenStuff(obj as IStuff);
//        //        OpenView(view, GetTitle(obj),"OpenStuff");
//        //    }
//        //    if (typeof(IGroup).IsAssignableFrom(obj.GetType()))
//        //    {
//        //        var view = OpenGroup(obj as IGroup);
//        //        OpenView(view, GetTitle(obj), "OpenGroup");
//        //    }
//        //    if (typeof(IEvent).IsAssignableFrom(obj.GetType()))
//        //    {
//        //        var view = OpenEvent(obj as IEvent);
//        //        OpenView(view, GetTitle(obj),"OpenEvent");
//        //    }
//        //    if (typeof(ITask).IsAssignableFrom(obj.GetType()))
//        //    {
//        //        var view = OpenTask(obj as ITask);
//        //        OpenView(view, GetTitle(obj), "OpenTask");
//        //    }
//        //    if (typeof(ILocation).IsAssignableFrom(obj.GetType()))
//        //    {
//        //        var view = OpenLocation(obj as ILocation);
//        //        OpenView(view, GetTitle(obj), "OpenLocation");
//        //    }
//        //    if (typeof(IExpress).IsAssignableFrom(obj.GetType()))
//        //    {
//        //        var view = OpenExpressDiagram(obj as IExpress);
//        //        OpenView(view, GetTitle(obj), "OpenExpress");
//        //    }
//        //    if (typeof(IStructureDiagram).IsAssignableFrom(obj.GetType()))
//        //    {
//        //        var view = OpenStructureDiagram(obj as IStructureDiagram);
//        //        OpenView(view, GetTitle(obj), "OpenStructureDiagram");
//        //    }
//        //    if (typeof(IFateDiagram).IsAssignableFrom(obj.GetType()))
//        //    {
//        //        var view = OpenFateDiagram(obj as IFateDiagram);
//        //        OpenView(view, GetTitle(obj), "OpenFateDiagram");
//        //    }
//        //    if (typeof(IRelation).IsAssignableFrom(obj.GetType()))
//        //    {
//        //        var view = OpenRelation(obj as IRelation);
//        //        OpenView(view, GetRelationTitle(obj as IRelation), "OpenRelation");
//        //    }
//        //}

//        IconControl GetDesignControlByID(Guid id)
//        {
//            if (ActorList.Any(v => v.ObjectID == id))
//                return new IconControl() { DataContext = ActorList.FirstOrDefault(v => v.ObjectID == id) };
//            if (EventList.Any(v => v.ObjectID == id))
//                return new IconControl() { DataContext = EventList.FirstOrDefault(v => v.ObjectID == id) };

//            if (GroupList.Any(v => v.ObjectID == id))
//                return new IconControl() { DataContext = GroupList.FirstOrDefault(v => v.ObjectID == id) };
//            if (LocationList.Any(v => v.ObjectID == id))
//                return new IconControl() { DataContext = LocationList.FirstOrDefault(v => v.ObjectID == id) };
//            if (StuffList.Any(v => v.ObjectID == id))
//                return new IconControl() { DataContext = StuffList.FirstOrDefault(v => v.ObjectID == id) };
//            if (TaskList.Any(v => v.ObjectID == id))
//                return new IconControl() { DataContext = TaskList.FirstOrDefault(v => v.ObjectID == id) };
//            return null;
//        }

//        public void Initialize()
//        {
//            if (_Target == null) return;
//            TimeSensitiveHelper.GetTotleTimeSpan = () => { return Target.TargetStory.ContinueTime; };
//            TimeSensitiveHelper.GetBeginTime = () => { return Target.TargetStory.BeginTime; };
//            if(DesignManager.GetConnection==null)
//                DesignManager.GetConnection = (sid, tid) =>
//                {
//                    var c = Target.TargetStory.RelationList.Where(v => v.SourceID == sid && v.TargetID == tid);
//                    if (c.Count() == 0)
//                    {
//                        return null;
//                    }
//                    var l = new List<object>();
//                    foreach(var r in c)
//                    {
//                        var dc = RelationList.FirstOrDefault(v => v.ObjectID == r.ObjectID);
//                        if (dc == null)
//                        {
//                            dc = new RelationDetailViewModel() { TargetObject = r };
//                            RelationList.Add(dc);
//                        }
//                        l.Add(dc);
//                    }
                
//                    return l;
//                };
//            if(DesignManager.CreateConnection==null)
//                DesignManager.CreateConnection = (sid, tid) =>
//                {
//                    var c= new Relation() { SourceID = sid, TargetID = tid };
//                    Story.CurrentStory.RelationList.Add(c);
//                    var dc = new RelationDetailViewModel() { TargetObject = c };
//                    RelationList.Add(dc);
//                    return dc;
//                };
//            if(DesignManager.GetObject==null)
//                DesignManager.GetObject = (id) =>
//                {
//                    if (id == null) return null;
//                    return Story.CurrentStory.GetEntityByID(id.Value);
//                };
//            if(!DesignManager.DefaultCreateCommandList.ContainsKey("Actor"))
//                DesignManager.DefaultCreateCommandList.Add("Actor", () =>
//                {
//                    var obj = new Actor();
//                    Target.TargetStory.ActorList.Add(obj);
//                    var dc = new ActorDetailViewModel()
//                    {
//                        TargetObject = obj,
//                        GetDetailView = () => { return new ActorDetailView(); }
//                    };
//                    ActorList.Add(dc);
//                    return new IconControl() { DataContext = dc };
//                });
//            if (!DesignManager.DefaultCreateCommandList.ContainsKey("Event"))
//                DesignManager.DefaultCreateCommandList.Add("Event", () =>
//                {
//                    var obj = new Event();
//                    Target.TargetStory.EventList.Add(obj);
//                    var dc = new EventDetailViewModel()
//                    {
//                        TargetObject = obj,
//                        GetDetailView = () => { return new EventDetailView(); }
//                    };
//                    EventList.Add(dc);
//                    return new IconControl() { DataContext = dc } ;
//                });
//            if (!DesignManager.DefaultCreateCommandList.ContainsKey("Group"))
//                DesignManager.DefaultCreateCommandList.Add("Group", () =>
//                {
//                    var obj = new Group();
//                    Target.TargetStory.GroupList.Add(obj);
//                    var dc = new GroupDetailViewModel()
//                    {
//                        TargetObject = obj,
//                        GetDetailView = () => { return new GroupDetailView(); }
//                    };
//                    GroupList.Add(dc);
//                    return new IconControl() { DataContext = dc };
//                });
//            if (!DesignManager.DefaultCreateCommandList.ContainsKey("Stuff"))
//                DesignManager.DefaultCreateCommandList.Add("Stuff", () =>
//                {
//                    var obj = new Stuff();
//                    Target.TargetStory.StuffList.Add(obj);
//                    var dc = new StuffDetailViewModel()
//                    {
//                        TargetObject = obj,
//                        GetDetailView = () => { return new StuffDetailView(); }
//                    };
//                    StuffList.Add(dc);
//                    return new IconControl() { DataContext = dc};
//                });
//            if (!DesignManager.DefaultCreateCommandList.ContainsKey("Task"))
//                DesignManager.DefaultCreateCommandList.Add("Task", () =>
//                {
//                    var obj = new StoryDesignLib.Task();
//                    Target.TargetStory.TaskList.Add(obj);
//                    var dc = new TaskDetailViewModel()
//                    {
//                        TargetObject = obj,
//                        GetDetailView = () => { return new TaskDetailView(); }
//                    };
//                    TaskList.Add(dc);
//                    return new IconControl() { DataContext = dc };
//                });
//            if (!DesignManager.DefaultCreateCommandList.ContainsKey("Location"))
//                DesignManager.DefaultCreateCommandList.Add("Location", () =>
//                {
//                    var obj = new Location();
//                    Target.TargetStory.LocationList.Add(obj);
//                    var dc = new LocationDetailViewModel()
//                    {
//                        TargetObject = obj,
//                        GetDetailView = () => { return new LocationDetailView(); }
//                    };
//                    LocationList.Add(dc);
//                    return new IconControl() { DataContext = dc };
//                });

//            if(DesignManager.GetDesignControl==null)
//                DesignManager.GetDesignControl = (o) =>
//                {
//                    if (o == null) return null;

//                    if(o is Guid)
//                    {
//                        return GetDesignControlByID((Guid)o );
//                    }

//                    if((o as IActor)!=null)
//                        return new IconControl() { DataContext =ActorList.FirstOrDefault(v=>v.ObjectID==(o as IActor).ObjectID) };
//                    if ((o as IEvent)!=null)
//                        return new IconControl() { DataContext = EventList.FirstOrDefault(v=>v.ObjectID==(o as IEvent).ObjectID)};
//                    if ((o as IGroup)!=null)
//                        return new IconControl() { DataContext = GroupList.FirstOrDefault(v=>v.ObjectID==(o as IGroup).ObjectID) };
//                    if ((o as IStuff)!=null)
//                        return new IconControl() { DataContext = StuffList.FirstOrDefault(v=>v.ObjectID==(o as IStuff).ObjectID)};
//                    if ((o as ITask)!=null)
//                        return new IconControl() { DataContext = TaskList.FirstOrDefault(v=>v.ObjectID==(o as ITask).ObjectID) };
//                    if ((o as ILocation)!=null)
//                        return new IconControl() { DataContext = LocationList.FirstOrDefault(v=>v.ObjectID==(o as ILocation).ObjectID) };

//                    if ((o as ActorDetailViewModel) != null)
//                        return new IconControl() { DataContext =o as ActorDetailViewModel };
//                    if ((o as EventDetailViewModel) != null)
//                        return new IconControl() { DataContext = o as EventDetailViewModel };
//                    if ((o as GroupDetailViewModel) != null)
//                        return new IconControl() { DataContext = o as GroupDetailViewModel };
//                    if ((o as StuffDetailViewModel) != null)
//                        return new IconControl() { DataContext = o as StuffDetailViewModel };
//                    if ((o as TaskDetailViewModel) != null)
//                        return new IconControl() { DataContext = o as TaskDetailViewModel};
//                    if ((o as LocationDetailViewModel) != null)
//                        return new IconControl() { DataContext = o as LocationDetailViewModel};

//                    return null;
//                };

//            if(DesignManager.SetCurrent==null)
//                DesignManager.SetCurrent = (o) =>
//                {
//                    CurrentObject = o;
//                };

//            DesignManager.IsAutoConnectNode = () => { return Target.IsNodeAutoConnection; };
//            DesignManager.GetDesignLink = (o) =>
//            {
//                if (o == null) return null;
//                if(o is Relation)
//                {
//                    var l = o as Relation;
//                    return new Tuple<Guid, Guid, object>(l.SourceID, l.TargetID, l);
//                }
//                if (o is RelationDetailViewModel)
//                {
//                    var l = o as RelationDetailViewModel;
//                    return new Tuple<Guid, Guid, object>(l.SourceID, l.TargetID, l);
//                }

//                return null;
//            };
//            LoadInfo();

//        }

//        Model.StoryDesign _Target=new Model.StoryDesign();
//        public Model.StoryDesign Target
//        {
//            get { return _Target; }
//            set
//            {
//                if (value != null && value != _Target)
//                { _Target = value;
//                    LoadInfo();
//                }
//            }
//        }

//        public Border MainContent { get; set; }
//        public Grid ListGrid { get; set; }
//        public Border PropertyView { get; set; }

//        void ShowAdditionPanel(bool isNoteListView)
//        {
//            AdditionPanelWidth = Target.AdditionPanelWidth;
//            IsNoteListView = isNoteListView;
//        }
//        void HideAdditionPanel()
//        {
//            AdditionPanelWidth = 0;
//        }
//        void FullScreen()
//        {
//            PaneDisplayMode = SplitViewDisplayMode.Overlay;

//        }

//        void DisFullScreen()
//        {
//            PaneDisplayMode = SplitViewDisplayMode.CompactOverlay;
//        }

//        public object GetViewModelByName(string name)
//        {
//            object o = DiagramList.FirstOrDefault(v => v.Name == name);
//            if (o != null)
//                return o;
//            o = ExpressList.FirstOrDefault(v => v.Name == name);
//            if (o != null)
//                return o;
            
//            return null;
//        }

//        public object GetViewModelByID(Guid id)
//        {
//            object o = ActorList.FirstOrDefault(v => v.ObjectID == id);
//            if (o != null)
//                return o;
//            o = EventList.FirstOrDefault(v => v.ObjectID == id);
//            if (o != null)
//                return o;
//            o = GroupList.FirstOrDefault(v => v.ObjectID == id);
//            if (o != null)
//                return o;
//            o = LocationList.FirstOrDefault(v => v.ObjectID == id);
//            if (o != null)
//                return o;
//            o = StuffList.FirstOrDefault(v => v.ObjectID == id);
//            if (o != null)
//                return o;
//            o = TaskList.FirstOrDefault(v => v.ObjectID == id);
//            if (o != null)
//                return o;
//            o = RelationList.FirstOrDefault(v => v.ObjectID == id);
//            if (o != null)
//                return o;
//            return null;
//        }

//        void LoadInfo()
//        {
//            if (Target.TargetStory == null) return;
//            ActorList.Clear();
//            Target.TargetStory.ActorList.ForEach(v => ActorList.Add(new ActorDetailViewModel() { TargetObject=v}));
//            EventList.Clear();
//            Target.TargetStory.EventList.ForEach(v => EventList.Add(new EventDetailViewModel() { TargetObject = v }));
//            GroupList.Clear();
//            Target.TargetStory.GroupList.ForEach(v => GroupList.Add(new GroupDetailViewModel() { TargetObject = v }));
//            LocationList.Clear();
//            Target.TargetStory.LocationList.ForEach(v => LocationList.Add(new LocationDetailViewModel() { TargetObject = v }));
//            TaskList.Clear();
//            Target.TargetStory.TaskList.ForEach(v => TaskList.Add(new TaskDetailViewModel() { TargetObject = v }));
//            StuffList.Clear();
//            Target.TargetStory.StuffList.ForEach(v => StuffList.Add(new StuffDetailViewModel() { TargetObject = v }));
//            RelationList.Clear();
//            Target.TargetStory.RelationList.ForEach(v => RelationList.Add(new RelationDetailViewModel() { TargetObject = v }));


//            ViewList.Clear();
//            Target.CurrentWorkViewList.ForEach(v =>
//            {
//                ViewList.Add(v);
//                //OpenViewInfo(v);
//            });
//            CurrentView = Target.CurrentWorkView;
//            ExpressList.Clear();
//            Target.TargetStory.ExpressList.ForEach(v => ExpressList.Add(new ExpressViewModel() { TargetObject = v }));
//            DiagramList.Clear();
//            Target.TargetStory.StructureDiagramList.ForEach(v => DiagramList.Add(new StructureDiagramViewModel() { TargetObject = v }));
//            if (ViewList.Count == 0)
//            {
//                if (DiagramList.Count == 0)
//                {
//                    var diagram = new StructureDiagram();
//                    var dc = new StructureDiagramViewModel() { TargetObject = diagram };
//                    Target.TargetStory.StructureDiagramList.Add(diagram);
//                    DiagramList.Add(dc);
//                }
//                CurrentDiagram = DiagramList.FirstOrDefault();
//                OpenView(GetDiagramView(CurrentDiagram),CurrentDiagram.Name, ViewDataType.Diagram,CurrentDiagram.Name,Guid.Empty);
//            }
//            OnPropertyChanged("Title");
//            RefreshEntityList();
//        }
//        void SaveInfo()
//        {
//            if (Target.TargetStory == null) return;

//            Target.TargetStory.ActorList.Clear();
//            ActorList.ToList().ForEach(v => { v.SaveInfo(); Target.TargetStory.ActorList.Add(v.TargetObject); });
//            Target.TargetStory.EventList.Clear();
//            EventList.ToList().ForEach(v => { v.SaveInfo(); Target.TargetStory.EventList.Add(v.TargetObject); });
//            Target.TargetStory.GroupList.Clear();
//            GroupList.ToList().ForEach(v => { v.SaveInfo(); Target.TargetStory.GroupList.Add(v.TargetObject); });
//            Target.TargetStory.TaskList.Clear();
//            TaskList.ToList().ForEach(v => { v.SaveInfo(); Target.TargetStory.TaskList.Add(v.TargetObject); });
//            Target.TargetStory.LocationList.Clear();
//            LocationList.ToList().ForEach(v => { v.SaveInfo(); Target.TargetStory.LocationList.Add(v.TargetObject); });
//            Target.TargetStory.StuffList.Clear();
//            StuffList.ToList().ForEach(v => { v.SaveInfo(); Target.TargetStory.StuffList.Add(v.TargetObject); });
//            Target.TargetStory.RelationList.Clear();
//            RelationList.ToList().ForEach(v => { v.SaveInfo(); Target.TargetStory.RelationList.Add(v.TargetObject); });

//            Target.TargetStory.ExpressList.Clear();
//            ExpressList.ToList().ForEach(v => { v.SaveInfo(); Target.TargetStory.ExpressList.Add(v.TargetObject); });

//            Target.TargetStory.StructureDiagramList.Clear();
//            DiagramList.ToList().ForEach(v => { v.SaveInfo(); Target.TargetStory.StructureDiagramList.Add(v.TargetObject ); });


//            Target.CurrentWorkViewList.Clear();

//            ViewList.ToList().ForEach(v =>
//            {
//                Target.CurrentWorkViewList.Add(v);
//                if (v.View != null)
//                {
//                    var dc = v.View.DataContext;
//                    if (dc != null)
//                    {
//                        var method = dc.GetType().GetMethod("SaveInfo");
//                        if (method != null)
//                        {
//                            method.Invoke(dc, new object[0]);
//                        }
//                    }
//                }
//            });
//            Target.CurrentWorkView= CurrentView;

//            OnPropertyChanged("Name");

//            //if (CurrentSelectEntity == DiagramSupportItem.Actor)
//            //{
//            //    var dc = actorListView.DataContext as EntityListViewModel<IActor,ActorDetailViewModel>;
//            //    if (dc != null)
//            //        dc.SaveInfo(Target.TargetStory.ActorList);
//            //}
//            //if (CurrentSelectEntity == DiagramSupportItem.Event)
//            //{
//            //    var dc = eventListView.DataContext as EntityListViewModel<IEvent,EventDetailViewModel>;
//            //    if (dc != null)
//            //        dc.SaveInfo(Target.TargetStory.EventList);
//            //}
//            //if (CurrentSelectEntity == DiagramSupportItem.Group)
//            //{
//            //    var dc = groupListView.DataContext as EntityListViewModel<IGroup,GroupDetailViewModel>;
//            //    if (dc != null)
//            //        dc.SaveInfo(Target.TargetStory.GroupList);
//            //}
//            //if (CurrentSelectEntity == DiagramSupportItem.Location)
//            //{
//            //    var dc = locationListView.DataContext as EntityListViewModel<ILocation,LocationDetailViewModel>;
//            //    if (dc != null)
//            //        dc.SaveInfo(Target.TargetStory.LocationList);
//            //}
//            //if (CurrentSelectEntity == DiagramSupportItem.Relation)
//            //{
//            //    var dc = relationListView.DataContext as RelationListViewModel;
//            //    if (dc != null)
//            //        dc.SaveInfo(Target.TargetStory.RelationList);

//            //}
//            //if (CurrentSelectEntity == DiagramSupportItem.Stuff)
//            //{
//            //    var dc = stuffListView.DataContext as EntityListViewModel<IStuff,StuffDetailViewModel>;
//            //    if (dc != null)
//            //        dc.SaveInfo(Target.TargetStory.StuffList);
//            //}
//            //if (CurrentSelectEntity == DiagramSupportItem.Task)
//            //{
//            //    var dc = taskListView.DataContext as EntityListViewModel<ITask,TaskDetailViewModel>;
//            //    if (dc != null)
//            //        dc.SaveInfo(Target.TargetStory.TaskList);
//            //}
//            //if (CurrentSelectEntity == DiagramSupportItem.Story)
//            //{
//            //    var dc = storyInfoView.DataContext as StoryInfoViewModel;
//            //    if (dc != null)
//            //        dc.SaveInfo(Target.TargetStory);
//            //}

//        }

//        public CommonCommand NewStoryCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    //Target.TargetStory = new Story();
//                    //Target.CurrentWorkViewList.Clear();
//                    //ViewList.Clear();
//                    //Target.CurrentWorkView = null;
//                    //CurrentView = null;
//                    //LoadInfo();
//                    //OnPropertyChanged("Title");
//                    CreateProject();
//                });
//            }
//        }
//        public CommonCommand ShowRelativeFateViewCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    var rootFrame = Window.Current.Content as Frame;
//                    if (rootFrame != null)
//                    {
//                        var main = rootFrame.Content as MainPage;
//                        main.SetView(new QueryRelativeView());
//                    }
//                    //var view = new RelativeFateView() { DataContext = new RelativeFateViewModel() };
//                    //OpenView(view, "Query Fate", ViewDataType.RelativeFate, "Query", Guid.Empty);
//                });
//            }
//        }
//        public CommonCommand OpenLogCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    CommonProc.ShowMessage(() => { }, "Log", new LogView());

//                });
//            }
//        }
//        public CommonCommand OpenStoryCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    //CommonProc.LoadFromFile<StoryDesign.Model.StoryDesign>((s) => {
//                    //    Target = s;
//                    //    LoadInfo();
//                    //    Target.TargetFile = f;
//                    //    Target.TargetFolder=f.
//                    //    OnPropertyChanged("Title");
//                    //});
//                    OpenProject();
                    
//                });
//            }
//        }
//        public CommonCommand SaveStoryAsCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    SaveAsProject();

//                });
//            }
//        }
//        public CommonCommand SaveStoryCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    SaveProject();

//                });
//            }
//        }

//        public CommonCommand CloseCurrentViewCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    if(CurrentView!=null&&ViewList.Contains(CurrentView))
//                    {
//                        ViewList.Remove(CurrentView);
//                        var view = ViewList.FirstOrDefault();
//                        if (view != null)
//                        {
//                            MainContent.Child = view.View;
//                            CurrentView = view;
//                        }
//                        else
//                            MainContent.Child = null;
//                        //if (MainContent.Children.Contains(CurrentView.View))
//                        //    MainContent.Children.Remove(CurrentView.View);
//                    }
//                });
//            }
//        }
//#region ShowList
//        void ShowList(FrameworkElement listView)
//        {
//            ListGrid.Children.Clear();
//            ListGrid.Children.Add(listView);

//                IsPaneOpen = true;
//        }

//        public CommonCommand TopButtonClickCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    CurrentObject = this;
//                    //ShowList(StoryListView);
//                });
//            }
//        }
//        public CommonCommand ShowActorListCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    ShowList(ActorListView);
//                });
//            }
//        }
//        public CommonCommand ShowStuffListCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    ShowList(StuffListView);

//                });
//            }
//        }
//        public CommonCommand ShowGroupListCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    ShowList(GroupListView);
//                });
//            }
//        }
//        public CommonCommand ShowEventListCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    ShowList(EventListView);
//                });
//            }
//        }
//        public CommonCommand ShowExpressListCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    ShowList(ExpressListView);
//                });
//            }
//        }
//        public CommonCommand ShowDiagramListCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    ShowList(DiagramListView);
//                });
//            }
//        }
//        public CommonCommand ShowTaskListCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    ShowList(TaskListView);
//                });
//            }
//        }
//        public CommonCommand ShowLocationListCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    ShowList(LocationListView);
//                });
//            }
//        }
//        public CommonCommand ShowRelationListCommand
//        {
//            get
//            {
//                return new CommonCommand((o) =>
//                {
//                    ShowList(RelationListView);
//                });
//            }
//        }
//#endregion
//        public async Task<StorageFile> GetProjectFile()
//        {
//            if (Target == null || Target.ProjectFolder == null)
//                return null;
//            var n = Target.ProjectFolder.Name+".story";
//            StorageFile f;
//            try
//            {
//                f = await Target.ProjectFolder.GetFileAsync(n);
//            }
//            catch (FileNotFoundException)
//            {
//                    f = await Target.ProjectFolder.CreateFileAsync(n);
//            }
//            return f;
//        }

//        public async void CopyAndSaveResource(Guid id,StorageFile file)
//        {
//            var folder =await GetAssertFolder();
//            var targetFolder =await folder.GetFolderAsync(id.ToString());
//            if (targetFolder == null)
//                targetFolder = await folder.CreateFolderAsync(id.ToString());
//            await file.CopyAsync(targetFolder, file.Name, NameCollisionOption.ReplaceExisting);

//        }

//        public StorageFolder GetProjectFolder()
//        {
//            return Target.ProjectFolder;
//        }
//        //public async Task<StorageFolder> GetProjectIconFolder()
//        //{
//        //    var assertFolder = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
//        //    StorageFolder iconFolder;
//        //    try
//        //    {
//        //        iconFolder = await assertFolder.GetFolderAsync("ProjectIcon");
//        //    }
//        //    catch (FileNotFoundException)
//        //    {
//        //        iconFolder = await assertFolder.CreateFolderAsync("ProjectIcon");
//        //    }
//        //    return iconFolder;
//        //}
//        public async Task<StorageFolder> GetAssertFolder()
//        {
//            if(Target.ProjectFolder==null)
//            {
//                CommonProc.ShowMessage("Info", "No valid project folder,please save story first");
//                return null;
//            }
//            StorageFolder f;
//            try
//            {
//                f = await Target.ProjectFolder.GetFolderAsync("Resource");
//            }
//            catch (FileNotFoundException)
//            {
//                f = await Target.ProjectFolder.CreateFolderAsync("Resource");
//            }
//            return f;
//        }
//        public async Task<StorageFolder> GetIconFolder()
//        {
//            if (Target.ProjectFolder == null)
//            {
//                CommonProc.ShowMessage("Info", "No valid project folder,please save story first");
//                return null;
//            }
//            StorageFolder f;
//            try
//            {
//                f = await Target.ProjectFolder.GetFolderAsync("Icon");
//            }
//            catch (FileNotFoundException)
//            {
//                f = await Target.ProjectFolder.CreateFolderAsync("Icon");
//            }
//            return f;
//        }

//        async void OpenProject()
//        {
//            FolderPicker pick = new FolderPicker();
//            pick.FileTypeFilter.Add(".story");
//            pick.SuggestedStartLocation = 0;
//            Windows.Storage.StorageFolder folder = await pick.PickSingleFolderAsync();
//            if (folder != null)
//            {
//                Target.ProjectFolder = folder;
//                try
//                {
//                    var name = folder.Name + ".story";
//                    var file = await folder.GetFileAsync(name);
//                    CommonProc.LoadFromFile<StoryDesign.Model.StoryDesign>((s) =>
//                    {

//                        Target = s;
//                        LoadInfo();

//                    }, file);
//                }
//                catch (FileNotFoundException)
//                {
//                    CommonLib.CommonProc.ShowMessage("Error", "Story file" + Name + ".story can not be find in  your select folder");
//                    return;
//                }
                
//            }

//            //var iconFolder =await GetIconFolder();
//            //var projectIconFolder =await GetProjectIconFolder();
//            //var fl = await projectIconFolder.GetFilesAsync();
//            //foreach (var f in fl)
//            //    await f.DeleteAsync();
//            //var fi = await iconFolder.GetFilesAsync();
//            //foreach (var f in fi)
//            //    await f.CopyAsync(projectIconFolder);
//        }
//        CreateStoryDialog createDialog;
//        public async void CreateProject()
//        {
//            try
//            {
//                if (Target != null && Target.IsChanged)
//                {
//                    if(await CommonProc.Confirm( "Confirm", "Current story chnged, save it?"))
//                        SaveProject();
//                }
//                if (createDialog == null)
//                {
//                    createDialog = new CreateStoryDialog();
//                }
//                createDialog.Init(Target);
                
//                ContentDialogResult result = await createDialog.ShowAsync();
//                if (result == ContentDialogResult.Primary)
//                {
//                    await createDialog.CreateProject(Target);
//                    Target.TargetStory.StructureDiagramList.Add(new StructureDiagram());
//                    Target.CurrentWorkViewList.Clear();
//                    ViewList.Clear();
//                    Target.CurrentWorkView = null;
//                    CurrentView = null;
//                    LoadInfo();
//                    OnPropertyChanged("Name");
//                }
//                else
//                {
//                    //return;
//                    // User pressed Cancel, ESC, or the back arrow.
//                    // Terms of use were not accepted.
//                }
                

                
//            }
//            catch(Exception ex)
//            {
//                CommonProc.ShowMessage("Error", ex.Message);
//            }
//        }
//        async void SaveProject()
//        {
//            SaveInfo();
//            if (Target.ProjectFolder == null)
//                SaveAsProject();
//            else
//            {
//                CommonProc.SaveToFile(await GetProjectFile(), Target);
//            }
//        }
//        async void SaveAsProject()
//        {
//            FolderPicker pick = new FolderPicker();
//            pick.FileTypeFilter.Add(".story");
//            pick.SuggestedStartLocation = 0;
//            Windows.Storage.StorageFolder folder = await pick.PickSingleFolderAsync();
//            if (folder != null)
//            {
//                try
//                {
//                    var f = await folder.GetItemAsync(Name);
//                    CommonLib.CommonProc.ShowMessage("Error", "Story " + Name + " folder can not be created, for a same name folder exist in your select folder");
//                    return;
//                }
//                catch (FileNotFoundException)
//                {
//                    //right
//                }
//                Target.ProjectFolder = await folder.CreateFolderAsync(Name);
//                SaveProject();
//            }

//        }
    }

    enum ListType
    {
        Actor,Event,Group,Location,Stuff,Task,Diagram,Express
    }
    
}
