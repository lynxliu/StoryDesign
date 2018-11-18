using DesignTool.Lib.Model;
using DesignTool.Lib.ViewModel;
using StoryDesignInterface;
using StoryDesignInterface.Diagram;
using StoryDesignLib;
using StoryDesignLib.Diagram;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UISupport;
using Windows.UI.Xaml;

namespace StoryDesign.ViewModel.DesignViewModel
{
    public class StructureDiagramViewModel: EntityViewModelBase<IStructureDiagram>
    {
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

        public bool IsTimeSpanEditable { get { return false; } }
        public double Width { get { return TargetObject.Width; } set { TargetObject.Width = value;OnPropertyChanged("Width"); } }
        public double Height { get { return TargetObject.Height; } set { TargetObject.Height = value; OnPropertyChanged("Height"); } }
        public Uri Icon
        {
            get
            {
                return new Uri("ms-appx:///Assets/Icon/Diagram.png");
            }
        }
        public GridLength IsShowTimeControl
        {
            get
            {
                if (IsTimeSensitive)
                    return new GridLength(55);
                return new GridLength(0);
            }
        }

        public bool IsTimeSensitive
        {
            get { return TargetObject.IsTimeSensitive; }
            set { TargetObject.IsTimeSensitive = value;OnPropertyChanged("IsTimeSensitive");OnPropertyChanged("IsShowTimeControl");
                if(value)
                    RefreshDiagramTime();
            }
        }
        //public DateTime CurrentDate
        //{
        //    get { if (TargetObject != null) return TargetObject.CurrentTime.Date; return DateTime.Now.Date; }
        //    set { if (TargetObject != null) TargetObject.CurrentTime = new DateTime(value.Year, value.Month, value.Day);
        //        OnPropertyChanged("CurrentDate"); RefreshDiagramTime(); }
        //}

        //public DateTime CurrentTime
        //{
        //    get { if (TargetObject != null) return new DateTime(1, 1, 1, TargetObject.CurrentTime.Hour, TargetObject.CurrentTime.Minute, TargetObject.CurrentTime.Second); return DateTime.Now; }
        //    set
        //    {
        //        if (TargetObject != null) TargetObject.CurrentTime = new DateTime(CurrentDate.Year, CurrentDate.Month, CurrentDate.Day, value.Hour, value.Minute, value.Second);
        //        OnPropertyChanged("CurrentTime");
        //        RefreshDiagramTime();
        //    }
        //}
        public DateTime CurrentTime
        {
            get { if (TargetObject != null) return TargetObject.CurrentTime; return DateTime.Now; }
            set
            {
                if (TargetObject != null) TargetObject.CurrentTime = value;
                OnPropertyChanged("CurrentTime");
                RefreshDiagramTime();
            }
        }
        DispatcherTimer timer = new DispatcherTimer();
        public CommonCommand PlayCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    timer.Stop();
                    timer.Interval = TimeSpan.FromSeconds(1);
                    timer.Tick += timer_Tick;
                    timer.Start();

                });
            }
        }
        
        List<TimeStep> _PlaySpanList = null;
        public List<TimeStep> PlaySpanList
        {
            get
            {
                if (_PlaySpanList == null)
                {
                    _PlaySpanList = Story.GetTimeStepList();
                }
                return _PlaySpanList;
            }
        }
        TimeStep _PlaySpan;
        public TimeStep PlaySpan
        {
            get { return _PlaySpan; }
            set { _PlaySpan = value;OnPropertyChanged("PlaySpan"); }
        }
        public CommonCommand StopCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    Stop();

                });
            }
        }
        void Stop()
        {
            timer.Stop();
            timer.Tick -= timer_Tick;
        }
        private void timer_Tick(object sender, object e)
        {
            if (TargetObject.CurrentTime <= MainViewModel.mainViewModel.Target.TargetStory.EndTime)
            {
                if (TargetObject.CurrentTime + Story.GetTimeSpan( PlaySpan) > MainViewModel.mainViewModel.Target.TargetStory.EndTime)
                    TargetObject.CurrentTime = MainViewModel.mainViewModel.Target.TargetStory.EndTime;
                else
                    TargetObject.CurrentTime = TargetObject.CurrentTime+ Story.GetTimeSpan(PlaySpan).Value;
                OnPropertyChanged("CurrentTime");
                OnPropertyChanged("CurrentDate");
                RefreshDiagramTime();
            }
            Stop();
        }

        //public DateTime CurrentTime { get { return TargetObject.CurrentTime; } set { TargetObject.CurrentTime = value;OnPropertyChanged("CurrentTime");RefreshDiagramTime(); } }
        void RefreshDiagramTime()
        {
            foreach(var node in TargetDesignCanvasViewModel.ItemList)
            {
                var n = node.TargetObject;
                if (n != null && n is Node)
                    node.IsValid = (n as INode).IsValid(MainViewModel.mainViewModel.Target.TargetStory, TargetObject.CurrentTime);
            }
            foreach (var node in TargetDesignCanvasViewModel.LinkList)
            {
                var n = node.TargetObject;
                if (n != null && n is Connection)
                    node.IsValid = (n as Connection).IsValid(MainViewModel.mainViewModel.Target.TargetStory, TargetObject.CurrentTime);
            }
        }



        DesignCanvasViewModel _TargetDesignCanvasViewModel;
        public DesignCanvasViewModel TargetDesignCanvasViewModel
        {
            get { if (_TargetDesignCanvasViewModel == null)
                {
                    _TargetDesignCanvasViewModel = new DesignCanvasViewModel() ;
                    if (TargetObject == null)
                        TargetObject = new StructureDiagram();
                    LoadInfo();
                }
                return _TargetDesignCanvasViewModel;
            }
            set
            {
                if(value!=null)
                {
                    _TargetDesignCanvasViewModel = value;LoadInfo();OnPropertyChanged("TargetDesignCanvasViewModel");
                }
            }
        }
        public override void LoadInfo()
        {

            TargetDesignCanvasViewModel.ItemList.Clear();
            TargetDesignCanvasViewModel.LinkList.Clear();
            TargetObject.NodeList.ForEach(v =>
            {
                var item = new DesignItem()
                {
                    Width = v.Width,
                    Height = v.Height,
                    Left = v.Left,
                    Top = v.Top,
                    ZIndex = v.ZIndex,
                    IconPath = v.IconPath,
                    TargetObjectID = v.TargetObjectID,
                    Info = v.NodeName,
                    IsValid=v.IsValid(MainViewModel.mainViewModel.Target.TargetStory,TargetObject.CurrentTime),
                    TargetObject=MainViewModel.mainViewModel.GetViewModelByID(v.TargetObjectID),
                    DesignItemID=v.DesignObjectID
                };
                if (v.IsIconMode)
                    item.ShowMode = DesignItemShowMode.Icon;
                else
                    item.ShowMode = DesignItemShowMode.Full;
                TargetDesignCanvasViewModel.ItemList.Add(item);
            });
            TargetObject.ConnectionList.ForEach(v =>
            {
                var link = new DesignLink()
                {
                    SourceX = v.StartLeft,
                    SourceY = v.StartTop,
                    TargetX = v.EndLeft,
                    TargetY = v.EndTop,
                    SourceID = v.StartObjectID,
                    TargetID = v.EndObjectID,
                    TargetObjectID = v.TargetObjectID,
                     DesignLinkID=v.DesignObjectID,
                     SourceDesignItemID=v.SourceDesignItemID,
                     TargetDesignItemID=v.TargetDesignItemID,
                    Text = v.Memo,
                    IsValid = v.IsValid(MainViewModel.mainViewModel.Target.TargetStory, TargetObject.CurrentTime),
                    TargetObject = MainViewModel.mainViewModel.GetViewModelByID(v.TargetObjectID),
                    SourcePosition=v. LinkLineInfo.SourcePosition,
                    TargetPosition=v.LinkLineInfo.TargetPosition,
                    RadiusRadio=v.LinkLineInfo.RadiusRadio,
                    LineSweepDirection=v.LinkLineInfo.LineSweepDirection
                };
                if (v.ConnectionType == ConnectionType.TwoWay)
                    link.LinkLine.ArrowVisibility = Visibility.Collapsed;
                TargetDesignCanvasViewModel.LinkList.Add(link);
            });
            TargetDesignCanvasViewModel.Refresh();
            OnPropertyChanged("Height");
            OnPropertyChanged("Width");
            LoadNoteList();
            base.LoadInfo();
        }

        public override void SaveInfo()
        {
            TargetObject.NodeList.Clear();
            TargetObject.ConnectionList.Clear();
            TargetDesignCanvasViewModel.ItemList.ForEach(v =>
            {
                var item = new Node()
                {
                    Width = v.Width,
                    Height = v.Height,
                    Left = v.Left,
                    Top = v.Top,
                    ZIndex = (int)v.ZIndex,
                    IconPath = v.IconPath,
                    TargetObjectID = v.TargetObjectID.Value,
                    DesignObjectID=v.DesignItemID
                    //Info = v.NodeName
                };
                if (v.ShowMode == DesignItemShowMode.Icon)
                    item.IsIconMode = true;
                else
                    item.IsIconMode = false;
                TargetObject.NodeList.Add(item);
            });
            TargetDesignCanvasViewModel.LinkList.ForEach(v =>
            {
                var link = new Connection()
                {
                    StartLeft = v.SourceX,
                    StartTop = v.SourceY,
                    EndLeft = v.TargetX,
                    EndTop = v.TargetY,
                    StartObjectID = v.SourceID.Value,
                    EndObjectID = v.TargetID.Value,
                    TargetObjectID = v.TargetObjectID.Value,
                    DesignObjectID=v.DesignLinkID,
                    SourceDesignItemID=v.SourceDesignItemID,
                    TargetDesignItemID=v.TargetDesignItemID,
                    Memo=v.Text,
                    LinkLineInfo=new LinkInfo()
                    {
                         LineSweepDirection=v.LineSweepDirection,
                         RadiusRadio=v.RadiusRadio,
                         SourcePosition=v.SourcePosition,
                         TargetPosition=v.TargetPosition
                    }
                };
                if (v.LinkLine.ArrowVisibility == Visibility.Collapsed)
                    link.ConnectionType = ConnectionType.TwoWay;

                TargetObject.ConnectionList.Add(link);
            });
            SaveNoteList();
            base.SaveInfo();
        }
        public string Name
        {
            get { if (TargetObject != null) return TargetObject.Name; return null; }
            set { if (TargetObject != null) TargetObject.Name = value; OnPropertyChanged("Name");MainViewModel.RefreshTitle(value, this); }
        }

        public string Memo { get { if (TargetObject != null) return TargetObject.Memo; return null; } set { if (TargetObject != null) TargetObject.Memo = value; OnPropertyChanged("Memo"); } }

        public DateTime Date
        {
            get
            {
                return TargetObject.CurrentTime.Date;
            }
            set
            {
                TargetObject.CurrentTime = new DateTime(value.Year,value.Month,value.Day)+TargetObject.CurrentTime.TimeOfDay;
                OnPropertyChanged("Date");
            }
        }
        public DateTime Time
        {
            get
            {
                return TargetObject.CurrentTime;
            }
            set
            {
                TargetObject.CurrentTime = TargetObject.CurrentTime.Date+value.TimeOfDay;
                OnPropertyChanged("Time");
            }
        }
    }
}
