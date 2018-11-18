using DesignTool.Lib.Model;
using DesignTool.Lib.ViewModel;
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
using UISupport.Diagram.Controls;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace StoryDesign.ViewModel.DesignViewModel
{
    public class FateDiagramViewModel: EntityViewModelBase<IFateDiagram>
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
        //public Guid TargetObjectID { get { return TargetObject.TargetObjectID; } set { TargetObject.TargetObjectID = value;OnPropertyChanged("TargetObjectID"); } }
        //public TimeSpan MinSeparate { get { return TargetObject.MinSeparate; } set { TargetObject.MinSeparate = value; OnPropertyChanged("MinSeparate"); } }
        //FilterType _ShowType = FilterType.NoFilter;
        public FilterType CurrentShowType { get { return TargetObject.ShowType; } set { TargetObject.ShowType = value;OnPropertyChanged("CurrentShowType"); } }
        List<FilterType> _ShowTypeList = new List<FilterType>() { FilterType.NoFilter, FilterType.EntityName, FilterType.EntityType, FilterType.RelationType };
        public List<FilterType> ShowTypeList { get { return _ShowTypeList; } }

        public ObservableCollection<ITrack> TrackList { get { return helper.TrackList; } }

        public ObservableCollection<ITimeSeparate> TimeSeparateList { get { return helper.TimeSeparateList; } }

        public DateTime BeginTime { get { return TargetObject.BeginTime; } set { TargetObject.BeginTime = value; OnPropertyChanged("BeginTime"); } }
        public DateTime EndTime { get { return TargetObject.EndTime; } set { TargetObject.EndTime = value; OnPropertyChanged("EndTime"); } }

        public TimeSpan Continue { get { return EndTime - BeginTime; } }
        public Canvas TargetCanvas { get { return helper.TargetCanvas; } set { helper.TargetCanvas = value; } }
        FateDiagramHelper helper = new FateDiagramHelper();

        public double SeparateWidth { get { return helper.SeparateWidth; } set { helper.SeparateWidth = value;OnPropertyChanged("SeparateWidth"); } }

        public double LeftMargin { get { return helper.LeftMargin; }set { helper.LeftMargin = value;OnPropertyChanged("LeftMargin"); } }
        public double RightMargin { get { return helper.RightMargin; } set { helper.RightMargin = value; OnPropertyChanged("RightMargin"); } }
        public double BottomMargin { get { return helper.BottomMargin; } set { helper.BottomMargin = value; OnPropertyChanged("BottomMargin"); } }

        public CommonCommand CreateFateCommand
        {
            get
            {
                return new CommonCommand((o) =>
                {
                    var entity = MainViewModel.mainViewModel.Target.TargetStory.GetEntityByID(TargetObject.TargetObjectID);
                    if(entity!=null)
                    {
                        entity.RefreshFateDiagram();
                        TargetObject = entity.TargetFate;
                    }
                    DrawDiagram();
                });
            }
        }



        
        public void DrawDiagram()
        {
            TrackList.Clear();
            TargetCanvas.Children.Clear();
            helper.BeginTime = BeginTime;
            helper.EndTime = EndTime;
            TargetObject.TrackList.ForEach(v => TrackList.Add(v));
            helper.SetWidth();
            helper.DrawTimeLine();
            helper.DrawSeparate();
            var th = TrackList.Sum(v => v.Width);
            var d = 0d;
            foreach(var t in TrackList)
            {
                d += helper.DrawTrack(t,th, d);
            }
        }
        public override void LoadInfo()
        {
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

                };
                if (v.IsIconMode)
                    item.ShowMode = DesignItemShowMode.Icon;
                else
                    item.ShowMode = DesignItemShowMode.Full;


            });


            TrackList.Clear();
            TargetObject.TrackList.ForEach(v => TrackList.Add(v));

            TimeSeparateList.Clear();
            TargetObject.TimeSeparateList.ForEach(v => TimeSeparateList.Add(v));

            if(TargetCanvas!=null)
                DrawDiagram();
            OnPropertyChanged("Name");
            OnPropertyChanged("Memo");
            OnPropertyChanged("BeginTime");
            OnPropertyChanged("EndTime");
            base.LoadInfo();
        }

        public override void SaveInfo()
        {
            TargetObject.NodeList.Clear();
            TargetObject.ConnectionList.Clear();


            TargetObject.TrackList.Clear();
            TrackList.ToList().ForEach(v => TargetObject.TrackList.Add(v));

            TargetObject.TimeSeparateList.Clear();
            TimeSeparateList.ToList().ForEach(v => TargetObject.TimeSeparateList.Add(v));

            base.SaveInfo();
        }

    }

    public class FilterTypeViewModel:ViewModelBase
    {
        public FilterType TargetObject { get; set; }
        bool _IsSelect = true;
        public bool IsSelect
        {
            get
            {
                return _IsSelect;
            }
            set
            {
                _IsSelect = value;OnPropertyChanged("IsSelect");
            }
        }
        public string Name
        {
            get
            {
                
                return TargetObject.ToString();
            }
        }

    }
}
