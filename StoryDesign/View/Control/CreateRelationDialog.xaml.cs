using StoryDesign.ViewModel.DetailViewModel;
using StoryDesignInterface;
using StoryDesignLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UISupport;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace StoryDesign.View.Control
{
    public sealed partial class CreateRelationDialog : ContentDialog
    {
        public CreateRelationDialog()
        {
            this.InitializeComponent();
            var dc = new CreateRelationDialogViewModel();
            DataContext = dc;
        }
        public void Clear()
        {
            var dc = DataContext as CreateRelationDialogViewModel;
            if (dc != null)
                dc.CreatedRelationList.Clear();
        }
        private void ContentDialog_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args)
        {
            var dc = DataContext as CreateRelationDialogViewModel;
            if (dc != null)
                dc.Init();
        }
        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var dc = DataContext as CreateRelationDialogViewModel;
            var ui = sender as FrameworkElement;
            if(ui!=null)
            {
                var data = ui.DataContext;
                if (data != null && data is Relation)
                    dc.CreatedRelationList.Remove(data as Relation);
            }
        }

        private void sourceFilterTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var dc = DataContext as CreateRelationDialogViewModel;
            if (dc != null)
            {
                dc.FilterShowList(dc.ShowSourceEntityList, sourceFilterTextbox.Text);
                dc._SourceFilter = sourceFilterTextbox.Text;
            }
        }
        private void targetFilterTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var dc = DataContext as CreateRelationDialogViewModel;
            if (dc != null)
            {
                dc.FilterShowList(dc.ShowTargetEntityList, targetFilterTextbox.Text);
                dc._TargetFilter = sourceFilterTextbox.Text;
            }
        }

        public void CreateRelation()
        {
            var dc = DataContext as CreateRelationDialogViewModel;
            foreach(var r in dc.CreatedRelationList)
            {
                MainViewModel.mainViewModel.AddRelation(r);
            }
        }

        private void ComboBox_TargetSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combox = sender as ComboBox;
            var dc = DataContext as CreateRelationDialogViewModel;
            if (dc == null) return;
            if (combox.SelectedIndex == 0)
                dc.TypeFilter(dc.ShowTargetEntityList, null, dc._SourceFilter);
            if (combox.SelectedIndex == 1)
                dc.TypeFilter(dc.ShowTargetEntityList, typeof(ActorDetailViewModel), dc._TargetFilter);
            if (combox.SelectedIndex == 2)
                dc.TypeFilter(dc.ShowTargetEntityList, typeof(StuffDetailViewModel), dc._TargetFilter);
            if (combox.SelectedIndex == 3)
                dc.TypeFilter(dc.ShowTargetEntityList, typeof(GroupDetailViewModel), dc._TargetFilter);
            if (combox.SelectedIndex == 4)
                dc.TypeFilter(dc.ShowTargetEntityList, typeof(EventDetailViewModel), dc._TargetFilter);
            if (combox.SelectedIndex == 5)
                dc.TypeFilter(dc.ShowTargetEntityList, typeof(TaskDetailViewModel), dc._TargetFilter);
            if (combox.SelectedIndex == 6)
                dc.TypeFilter(dc.ShowTargetEntityList, typeof(LocationDetailViewModel), dc._TargetFilter);
        }

        private void ComboBox_SourceSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combox = sender as ComboBox;
            var dc = DataContext as CreateRelationDialogViewModel;
            if (dc == null) return;
            if (combox.SelectedIndex == 0)
                dc.TypeFilter(dc.ShowSourceEntityList, null, dc._SourceFilter);
            if (combox.SelectedIndex == 1)
                dc.TypeFilter(dc.ShowSourceEntityList, typeof(ActorDetailViewModel), dc._SourceFilter);
            if (combox.SelectedIndex == 2)
                dc.TypeFilter(dc.ShowSourceEntityList, typeof(StuffDetailViewModel), dc._SourceFilter);
            if (combox.SelectedIndex == 3)
                dc.TypeFilter(dc.ShowSourceEntityList, typeof(GroupDetailViewModel), dc._SourceFilter);
            if (combox.SelectedIndex == 4)
                dc.TypeFilter(dc.ShowSourceEntityList, typeof(EventDetailViewModel), dc._SourceFilter);
            if (combox.SelectedIndex == 5)
                dc.TypeFilter(dc.ShowSourceEntityList, typeof(TaskDetailViewModel), dc._SourceFilter);
            if (combox.SelectedIndex == 6)
                dc.TypeFilter(dc.ShowSourceEntityList, typeof(LocationDetailViewModel), dc._SourceFilter);
        }
    }

    public class CreateRelationDialogViewModel : ViewModelBase
    {
        public void TypeFilter(ObservableCollection<EntityShow> list,Type filterType,string filter)
        {
            list.Clear();
            if(filterType==null)
                AllEntityList.ForEach(v =>
                {
                    if (IsFit(v, filter))
                        list.Add(v);
                });
            else
                AllEntityList.ForEach(v =>
                {
                    if(v.GetType().GetTypeInfo().IsSubclassOf(filterType))
                        if (IsFit(v, filter))
                            list.Add(v);
                });
        }

        public void Init()
        {
            foreach (var v in MainViewModel.mainViewModel.ActorList)
            {
                var e = new EntityShow() { Target = v.TargetObject, IconImage = v.IconImage };
                AllEntityList.Add(e);
                ShowSourceEntityList.Add(e);
                ShowTargetEntityList.Add(e);
            }
            foreach (var v in MainViewModel.mainViewModel.EventList)
            {
                var e = new EntityShow() { Target = v.TargetObject, IconImage = v.IconImage };
                AllEntityList.Add(e);
                ShowSourceEntityList.Add(e);
                ShowTargetEntityList.Add(e);
            }
            foreach (var v in MainViewModel.mainViewModel.GroupList)
            {
                var e = new EntityShow() { Target = v.TargetObject, IconImage = v.IconImage };
                AllEntityList.Add(e);
                ShowSourceEntityList.Add(e);
                ShowTargetEntityList.Add(e);
            }
            foreach (var v in MainViewModel.mainViewModel.LocationList)
            {
                var e = new EntityShow() { Target = v.TargetObject, IconImage = v.IconImage };
                AllEntityList.Add(e);
                ShowSourceEntityList.Add(e);
                ShowTargetEntityList.Add(e);
            }
            foreach (var v in MainViewModel.mainViewModel.StuffList)
            {
                var e = new EntityShow() { Target = v.TargetObject, IconImage = v.IconImage };
                AllEntityList.Add(e);
                ShowSourceEntityList.Add(e);
                ShowTargetEntityList.Add(e);
            }
            foreach (var v in MainViewModel.mainViewModel.TaskList)
            {
                var e = new EntityShow() { Target = v.TargetObject, IconImage = v.IconImage };
                AllEntityList.Add(e);
                ShowSourceEntityList.Add(e);
                ShowTargetEntityList.Add(e);
            }
            RelationTypeList.ForEach(v => ShowRelationTypeList.Add(v));
        }
        public string _SourceFilter;
        public string _TargetFilter;
        public void FilterShowList(ObservableCollection<EntityShow> list,string filter)
        {
            //_SourceFilter = filter;
            var l = list.ToList();
            list.Clear();
            l.ForEach(v =>
            {
                if (IsFit(v, filter))
                    list.Add(v);
            });

        }
        //public void FilterTarget(string filter)
        //{
        //    _TargetFilter = filter;
        //    ShowTargetEntityList.Clear();
        //    AllEntityList.ForEach(v =>
        //    {
        //        if (IsFit(v, filter))
        //            ShowTargetEntityList.Add(v);
        //    });

        //}
        bool IsFit(EntityShow obj, string filter)
        {

            if (string.IsNullOrEmpty(filter)) return true;
            if (obj.Name.IndexOf(filter, 0, StringComparison.OrdinalIgnoreCase) >= 0) return true;
            return false;
        }
        List<EntityShow> _AllEntityList = new List<EntityShow>();
        public List<EntityShow> AllEntityList { get { return _AllEntityList; } }

        ObservableCollection<EntityShow> _ShowSourceEntityList = new ObservableCollection<EntityShow>();
        public ObservableCollection<EntityShow> ShowSourceEntityList { get { return _ShowSourceEntityList; } }
        ObservableCollection<EntityShow> _ShowTargetEntityList = new ObservableCollection<EntityShow>();
        public ObservableCollection<EntityShow> ShowTargetEntityList { get { return _ShowTargetEntityList; } }

        bool EntityIsActor(EntityShow entity)
        {
            if (entity == null||entity.Target==null) return false;
            if ((entity.Target as IActor) != null) return true;
            if ((entity.Target as ActorDetailViewModel) != null) return true;
            return false;
        }
        bool EntityIsEvent(EntityShow entity)
        {
            if (entity == null || entity.Target == null) return false;
            if ((entity.Target as IEvent) != null) return true;
            if ((entity.Target as EventDetailViewModel) != null) return true;
            return false;

        }
        bool EntityIsGroup(EntityShow entity)
        {
            if (entity == null || entity.Target == null) return false;
            if ((entity.Target as IGroup) != null) return true;
            if ((entity.Target as GroupDetailViewModel) != null) return true;
            return false;

        }
        bool EntityIsLocation(EntityShow entity)
        {
            if (entity == null || entity.Target == null) return false;
            if ((entity.Target as ILocation) != null) return true;
            if ((entity.Target as LocationDetailViewModel) != null) return true;
            return false;

        }
        bool EntityIsStuff(EntityShow entity)
        {
            if (entity == null || entity.Target == null) return false;
            if ((entity.Target as IStuff) != null) return true;
            if ((entity.Target as StuffDetailViewModel) != null) return true;
            return false;

        }
        bool EntityIsTask(EntityShow entity)
        {
            if (entity == null || entity.Target == null) return false;
            if ((entity.Target as ITask) != null) return true;
            if ((entity.Target as TaskDetailViewModel) != null) return true;
            return false;

        }
        void RefreshRelationTypeList()
        {
            if (EntityIsActor(CurrentSourceEntity))
            {
                if (EntityIsActor(CurrentTargetEntity))
                {
                    ShowRelationTypeList.Clear();
                    ShowRelationTypeList.Add(RelationBaseType.Kin);
                    ShowRelationTypeList.Add(RelationBaseType.Mate);
                    ShowRelationTypeList.Add(RelationBaseType.Friend);
                    ShowRelationTypeList.Add(RelationBaseType.Others);
                }
                if (EntityIsEvent(CurrentTargetEntity))
                {
                    ShowRelationTypeList.Clear();
                    ShowRelationTypeList.Add(RelationBaseType.Attend);
                    ShowRelationTypeList.Add(RelationBaseType.Others);
                }
                if (EntityIsTask(CurrentTargetEntity))
                {
                    ShowRelationTypeList.Clear();
                    ShowRelationTypeList.Add(RelationBaseType.Participate);
                    ShowRelationTypeList.Add(RelationBaseType.Responsive);
                    ShowRelationTypeList.Add(RelationBaseType.Others);
                }
                if (EntityIsLocation(CurrentTargetEntity))
                {
                    ShowRelationTypeList.Clear();
                    ShowRelationTypeList.Add(RelationBaseType.Appear);
                    ShowRelationTypeList.Add(RelationBaseType.Others);
                }
                if (EntityIsStuff(CurrentTargetEntity))
                {
                    ShowRelationTypeList.Clear();
                    ShowRelationTypeList.Add(RelationBaseType.Own);
                    ShowRelationTypeList.Add(RelationBaseType.Others);
                }
                if (EntityIsGroup(CurrentTargetEntity))
                {
                    ShowRelationTypeList.Clear();
                    ShowRelationTypeList.Add(RelationBaseType.Office);
                    ShowRelationTypeList.Add(RelationBaseType.Others);
                }

            }
            if (EntityIsStuff(CurrentSourceEntity))
            {
                if (EntityIsEvent(CurrentTargetEntity))
                {
                    ShowRelationTypeList.Clear();
                    ShowRelationTypeList.Add(RelationBaseType.Attend);
                    ShowRelationTypeList.Add(RelationBaseType.Others);
                }
                if (EntityIsTask(CurrentTargetEntity))
                {
                    ShowRelationTypeList.Clear();
                    ShowRelationTypeList.Add(RelationBaseType.Participate);
                    ShowRelationTypeList.Add(RelationBaseType.Others);
                }
                if (EntityIsLocation(CurrentTargetEntity))
                {
                    ShowRelationTypeList.Clear();
                    ShowRelationTypeList.Add(RelationBaseType.Appear);
                    ShowRelationTypeList.Add(RelationBaseType.Others);
                }

            }
            if (EntityIsEvent(CurrentSourceEntity))
            {
                ShowRelationTypeList.Clear();
                ShowRelationTypeList.Add(RelationBaseType.Others);
            }
            if (EntityIsGroup(CurrentSourceEntity))
            {
                ShowRelationTypeList.Clear();
                ShowRelationTypeList.Add(RelationBaseType.Others);
            }
            if (EntityIsLocation(CurrentSourceEntity))
            {
                ShowRelationTypeList.Clear();
                ShowRelationTypeList.Add(RelationBaseType.Others);
            }
            if (EntityIsTask(CurrentSourceEntity))
            {
                ShowRelationTypeList.Clear();
                ShowRelationTypeList.Add(RelationBaseType.Others);
            }
        }
        EntityShow _CurrentSourceEntity;
        public EntityShow CurrentSourceEntity
        {
            get { return _CurrentSourceEntity; }
            set
            {
                _CurrentSourceEntity = value;
                RefreshRelationTypeList();
            }
        }
        EntityShow _CurrentTargetEntity;
        public EntityShow CurrentTargetEntity
        {
            get { return _CurrentTargetEntity; }
            set
            {
                _CurrentTargetEntity = value;
                RefreshRelationTypeList();
            }
        }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Memo { get; set; }
        List<RelationBaseType> _RelationTypeList = new List<RelationBaseType>() { RelationBaseType.Appear, RelationBaseType.Attend,
            RelationBaseType.Friend, RelationBaseType.Kin,RelationBaseType.Mate, RelationBaseType.Office,
            RelationBaseType.Others, RelationBaseType.Own, RelationBaseType.Participate, RelationBaseType.Responsive };
        public List<RelationBaseType> RelationTypeList { get { return _RelationTypeList; } }
        ObservableCollection<RelationBaseType> _ShowRelationTypeList = new ObservableCollection<RelationBaseType>();
        public ObservableCollection<RelationBaseType> ShowRelationTypeList { get { return _ShowRelationTypeList; } }
        public RelationBaseType CurrentRelationType { get; set; }


        ObservableCollection<Relation> _CreatedRelationList = new ObservableCollection<Relation>();
        public ObservableCollection<Relation> CreatedRelationList
        {
            get { return _CreatedRelationList; }
        }

        public CommonCommand CreateRelationCommand
        {
            get
            {
                return new CommonCommand((o =>
                {
                    var r = new Relation()
                    {
                        SourceID = CurrentSourceEntity.Target.ObjectID,
                        TargetID = CurrentTargetEntity.Target.ObjectID,
                        RelationType = CurrentRelationType,
                        BeginTime = BeginTime,
                        EndTime = EndTime,
                        Memo = Memo

                    };
                    CreatedRelationList.Add(r);
                }));
                
            }
        }
    }

}
