using StoryDesign.ViewModel.DetailViewModel;
using StoryDesignInterface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace StoryDesign.View.Control
{
    public sealed partial class SelectEntityDialog : ContentDialog
    {
        public SelectEntityDialog()
        {
            this.InitializeComponent();
            var dc = new SelectEntityDialogViewModel();
            
            DataContext = dc;
        }
        public List<IStoryEntityObject> GetSelectedEntityList()
        {
            var dc = DataContext as SelectEntityDialogViewModel;
            if (dc != null)
                return dc.GetSelected();
            return null;
        }
        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var ui = sender as FrameworkElement;
            if (ui == null) return;
            var dc = ui.DataContext;
            if (dc != null && dc is EntityShow)
                (dc as EntityShow).IsSelected = !(dc as EntityShow).IsSelected;

        }

        private void ContentDialog_Opened(ContentDialog sender, ContentDialogOpenedEventArgs args)
        {
            var dc = DataContext as SelectEntityDialogViewModel;
            if (dc != null)
                dc.Init();
        }
        void UpdateSelectInfo()
        {
            var dc = DataContext as SelectEntityDialogViewModel;
            if (dc != null)
                dc.UpdateSelectedNo();
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateSelectInfo();

        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateSelectInfo();
        }

        private void filterTextbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var dc = DataContext as SelectEntityDialogViewModel;
            if (dc != null)
                dc.Filter(filterTextbox.Text);
        }
    }

    public class SelectEntityDialogViewModel : ViewModelBase
    {
        public int TotalNo
        {
            get
            {
                return AllEntityList.Count;
            }
        }
        public int SelectedNo
        {
            get
            {
                return AllEntityList.Count(v=>v.IsSelected);
            }
        }
        public void UpdateSelectedNo()
        {
            OnPropertyChanged("SelectedNo");
        }
        public string _Filter = null;
        //public string Filter
        //{
        //    get { return _Filter; }
        //    set
        //    {
        //        if (_Filter != value)
        //        {
        //            ShowEntityList.Clear();
        //            AllEntityList.ForEach(v =>
        //            {
        //                if (IsFit(v))
        //                    ShowEntityList.Add(v);
        //            });
                    
        //        }
        //        _Filter = value;
        //        OnPropertyChanged("Filter");
        //    }
        //}
        public void Filter(string filter)
        {
            _Filter = filter;
                ShowEntityList.Clear();
                AllEntityList.ForEach(v =>
                {
                    if (IsFit(v, filter))
                        ShowEntityList.Add(v);
                });

        }
        List<EntityShow> _AllEntityList = new List<EntityShow>();
        public List<EntityShow> AllEntityList { get { return _AllEntityList; } }

        ObservableCollection<EntityShow> _ShowEntityList = new ObservableCollection<EntityShow>();
        public ObservableCollection<EntityShow> ShowEntityList { get { return _ShowEntityList; } }

        public List<IStoryEntityObject> GetSelected()
        {
            var l = new List<IStoryEntityObject>();
            foreach (var e in AllEntityList.Where(v => v.IsSelected))
                l.Add(e.Target);
            return l;
        }

        public void Init()
        {
            foreach(var v in MainViewModel.mainViewModel.ActorList)
            {
                var e = new EntityShow() { Target = v.TargetObject, IconImage = v.IconImage };
                AllEntityList.Add(e);
                ShowEntityList.Add(e);
            }
            foreach (var v in MainViewModel.mainViewModel.EventList)
            {
                var e = new EntityShow() { Target = v.TargetObject, IconImage = v.IconImage };
                AllEntityList.Add(e);
                ShowEntityList.Add(e);
            }
            foreach (var v in MainViewModel.mainViewModel.GroupList)
            {
                var e = new EntityShow() { Target = v.TargetObject, IconImage = v.IconImage };
                AllEntityList.Add(e);
                ShowEntityList.Add(e);
            }
            foreach (var v in MainViewModel.mainViewModel.LocationList)
            {
                var e = new EntityShow() { Target = v.TargetObject, IconImage = v.IconImage };
                AllEntityList.Add(e);
                ShowEntityList.Add(e);
            }
            foreach (var v in MainViewModel.mainViewModel.StuffList)
            {
                var e = new EntityShow() { Target = v.TargetObject, IconImage = v.IconImage };
                AllEntityList.Add(e);
                ShowEntityList.Add(e);
            }
            foreach (var v in MainViewModel.mainViewModel.TaskList)
            {
                var e = new EntityShow() { Target = v.TargetObject, IconImage = v.IconImage };
                AllEntityList.Add(e);
                ShowEntityList.Add(e);
            }
            OnPropertyChanged("TotalNo");
        }
        public void ClearSelect()
        {
            AllEntityList.ForEach(v => v.IsSelected = false);
        }
        bool IsFit(EntityShow obj,string filter)
        {

            if (string.IsNullOrEmpty(filter)) return true;
            if (obj.Name.IndexOf(filter, 0,StringComparison.OrdinalIgnoreCase)>=0) return true;
            return false;
        }
        public CommonCommand ClearSelectCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    ClearSelect();
                });
            }
        }
        public CommonCommand ShowAllCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    ShowEntityList.Clear();
                    AllEntityList.ForEach(v =>
                    {
                        if (IsFit(v, _Filter))
                            ShowEntityList.Add(v);
                    });
                });
            }
        }
        public CommonCommand ShowActorCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    ShowEntityList.Clear();
                    AllEntityList.ForEach(v =>
                    {
                        if(IsFit(v, _Filter) &&v.Target as IActor!=null)
                            ShowEntityList.Add(v);
                    });
                });
            }
        }
        public CommonCommand ShowEventCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    ShowEntityList.Clear();
                    AllEntityList.ForEach(v =>
                    {
                        if (IsFit(v, _Filter) && v.Target as IEvent != null)
                            ShowEntityList.Add(v);
                    });
                });
            }
        }
        public CommonCommand ShowGroupCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    ShowEntityList.Clear();
                    AllEntityList.ForEach(v =>
                    {
                        if (IsFit(v, _Filter) && v.Target as IGroup != null)
                            ShowEntityList.Add(v);
                    });
                });
            }
        }
        public CommonCommand ShowLocationCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    ShowEntityList.Clear();
                    AllEntityList.ForEach(v =>
                    {
                        if (IsFit(v, _Filter) && v.Target as ILocation != null)
                            ShowEntityList.Add(v);
                    });
                });
            }
        }
        public CommonCommand ShowStuffCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    ShowEntityList.Clear();
                    AllEntityList.ForEach(v =>
                    {
                        if (IsFit(v, _Filter) && v.Target as IStuff != null)
                            ShowEntityList.Add(v);
                    });
                });
            }
        }
        public CommonCommand ShowTaskCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    ShowEntityList.Clear();
                    AllEntityList.ForEach(v =>
                    {
                        if (IsFit(v, _Filter) && v.Target as ITask != null)
                            ShowEntityList.Add(v);
                    });
                });
            }
        }
    }

    public class EntityShow : ViewModelBase
    {
        public IStoryEntityObject Target { get; set; }
        public string Name { get
            {
                if (Target != null) return Target.Name;
                return "Not valid";
            } }
        public string Memo
        {
            get
            {
                if (Target != null) return Target.Memo;
                return null;
            }
        }
        bool _IsSelected = false;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                _IsSelected = value;OnPropertyChanged("IsSelected");
            }
        }

        public BitmapImage IconImage
        {
            get;set;
        }
    }
}
