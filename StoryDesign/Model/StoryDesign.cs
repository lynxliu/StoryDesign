using Newtonsoft.Json;
using StoryDesignInterface;
using StoryDesignLib;
using StoryDesignLib.Diagram;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UISupport;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace StoryDesign.Model
{
    public class StoryDesign:ViewModelBase
    {


        bool _IsCopyImageResource = false;
        public bool IsCopyImageResource
        {
            get { return _IsCopyImageResource; }
            set { _IsCopyImageResource = value; }
        }

        //[JsonIgnore]
        //public StorageFile TargetFile { get; set; }
        [JsonIgnore]
        public StorageFolder ProjectFolder{ get; set; }


        //async void InitAssertFolder()
        //{
        //    _ResourceFolder = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
        //}
        //StorageFolder _ResourceFolder;
        //[JsonIgnore]
        //public StorageFolder ResourceFolder
        //{
        //    get {
        //        if (_ResourceFolder == null)
        //            InitAssertFolder();
        //        return _ResourceFolder;

        //    } set
        //    {
        //        _ResourceFolder = value;
        //    }
        //}
        //public string FilePath { get; set; }
        //Story _TargetStory;
        public Story TargetStory { get {
                if (Story.CurrentStory == null)
                {
                    Story.CurrentStory = new Story();
                }
                return Story.CurrentStory as Story; }
            set { Story.CurrentStory = value;  }
        }
        List<ViewInfo> _CurrentWorkViewList = new List<ViewInfo>();
        public List<ViewInfo> CurrentWorkViewList { get { return _CurrentWorkViewList; } }


        int _CurrentViewIndex = -1;
        public int CurrentViewIndex { get { return _CurrentViewIndex; } set { _CurrentViewIndex = value; } }

        //[JsonIgnore]
        //public ViewInfo CurrentWorkView { get; set; }

        SplitViewDisplayMode _PaneDisplayMode = SplitViewDisplayMode.CompactOverlay;
        public SplitViewDisplayMode PaneDisplayMode
        {
            get { return _PaneDisplayMode; }
            set { _PaneDisplayMode = value; }
        }
        SplitViewDisplayMode _AdditionPaneDisplayMode = SplitViewDisplayMode.CompactInline;
        public SplitViewDisplayMode AdditionPaneDisplayMode
        {
            get { return _AdditionPaneDisplayMode; }
            set { _AdditionPaneDisplayMode = value; }
        }
        double _PaneWidth = 500;
        public double PaneWidth
        {
            get { return _PaneWidth; }
            set { _PaneWidth = value; }
        }
        public bool IsPanePinned
        {
            get;set;
        }
        public bool IsAdditionPanePinned
        {
            get; set;
        }
        public bool IsFullScreen
        {
            get;set;
        }

        bool _IsPaneOpen = false;
        public bool IsPaneOpen
        {
            get { return _IsPaneOpen; }
            set { _IsPaneOpen = value; }
        }

        double _AdditionPaneWidth = 370;
        public double AdditionPaneWidth
        {
            get { return _AdditionPaneWidth; }
            set { _AdditionPaneWidth = value; }
        }

        bool _IsAdditionPaneOpen = true;
        public bool IsAdditionPaneOpen
        {
            get { return _IsAdditionPaneOpen; } set { _IsAdditionPaneOpen = value; }
        }

        double _AdditionPane1stRowHeight = 50;
        public double AdditionPane1stRowHeight
        {
            get { return _AdditionPane1stRowHeight; }
            set { _AdditionPane1stRowHeight = value; }
        }

        bool _IsNodeAutoConnection = true;
        public bool IsNodeAutoConnection
        {
            get { return _IsNodeAutoConnection; }
            set { _IsNodeAutoConnection = value; }
        }
        //GridLength _AdditionPane1stRowHeight = new GridLength(50, GridUnitType.Star);
        //GridLength AdditionPane1stRowHeight
        //{
        //    get { return _AdditionPane1stRowHeight; }
        //    set { _AdditionPane1stRowHeight = value; }
        //}
        //GridLength _AdditionPane2ndRowHeight = new GridLength(50, GridUnitType.Star);
        //GridLength AdditionPane2ndRowHeight
        //{
        //    get { return _AdditionPane2ndRowHeight; }
        //    set { _AdditionPane2ndRowHeight = value; }
        //}
    }

    public class ViewInfo
    {
        public string Title { get; set; }
        public ViewDataType ViewType { get; set; }
        public string TargetName { get; set; }
        public Guid TargetObjectID { get; set; }
        //public string IconUrl { get; set; }
        //public string CommandName { get; set; }
        //List<Guid> _ObjectIDList = new List<Guid>();
        //public List<Guid> ObjectIDList { get { return _ObjectIDList; } }

        //object _TargetObject;
        //public object TargetObject { get { return _TargetObject; }
        //    set {
        //        if (value == null) return;
        //        _TargetObject = value;
        //        _TargetObjectType = value.GetType();
        //    } }
        //Type _TargetObjectType;
        //public Type TargetObjectType { get { return _TargetObjectType; } }
        public DateTime ActiveTime { get; set; }

        [JsonIgnore]
        public FrameworkElement View { get; set; }

        
    }

    public enum ViewDataType
    {
        EntityObject,Diagram,Fate,Express,Relation,RelativeFate
    }
}
