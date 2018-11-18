using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Reflection;
using CommonLib;
using Newtonsoft.Json;
using UISupport;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;
using UISupport.Diagram.Controls;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.Foundation;

namespace DesignTool.Lib.Model
{
    public class DesignLink : ViewModelBase
    {
        public void CreateLink(Guid id)
        {
            
            if (SourcePoint != null)
            {
                var vm = SourcePoint.DataContext as DesignItem;
                if (vm != null)
                {
                    SourceID = vm.TargetObjectID;
                }
            }
            if (TargetPoint != null)
            {
                var vm = TargetPoint.DataContext as DesignItem;
                if (vm != null)
                {
                    TargetID = vm.TargetObjectID;
                }
            }
            TargetObjectID = id;
            
        }

        void LoadTargetObjectID(PropertyInfo prop)
        {
            if (prop != null)
            {
                TargetObjectID =(Guid) prop.GetValue(_TargetObject);
            }
        }
        object _TargetObject;
        [JsonIgnore]
        public object TargetObject
        {
            get { return _TargetObject; }
            set { _TargetObject = value; OnPropertyChanged("TargetObject"); OnPropertyChanged("Name");
                if (_TargetObject != null) {
                    var prop = _TargetObject.GetType().GetProperty("ObjectID");
                    if(prop==null)
                        prop= _TargetObject.GetType().GetProperty("ObjectId");
                    if (prop == null)
                        prop = _TargetObject.GetType().GetProperty("ID");
                    if (prop == null)
                        prop = _TargetObject.GetType().GetProperty("Id");
                    if (prop == null)
                        prop = _TargetObject.GetType().GetProperty("Identity");
                    if (prop != null)
                        LoadTargetObjectID(prop);
                }
            }
        }
        string _Text;
        public string Text
        {
            get
            {
                if (_TargetObject != null)
                {
                    var prop = _TargetObject.GetType().GetProperty("Name");
                    if(prop==null)
                        prop = _TargetObject.GetType().GetProperty("Memo");
                    if (prop == null)
                        prop = _TargetObject.GetType().GetProperty("Description");
                    if (prop != null)
                    {
                        if(prop.GetValue(_TargetObject)!=null)
                            _Text = prop.GetValue(_TargetObject).ToString();
                    }
                        
                }
                return _Text;
            }
            set
            {
                _Text = value; OnPropertyChanged("Name");
                if (_TargetObject != null)
                {
                    var prop = _TargetObject.GetType().GetProperty("Name");
                    if (prop == null)
                        prop = _TargetObject.GetType().GetProperty("Memo");
                    if (prop == null)
                        prop = _TargetObject.GetType().GetProperty("Description");
                    if (prop != null)
                        prop.SetValue(_TargetObject, value);
                }
            }
        }

        [JsonIgnore]
        public double StrokeThickness
        {
            get
            {
                if (_TargetObject != null)
                {
                    var prop = CommonProc.GetPropertyValue( _TargetObject,new List<string>() { "Continue", "ContinueTime" });
                    if (prop == null)
                    {
                        var start= CommonProc.GetPropertyValue(_TargetObject, new List<string>() { "Start", "StartTime", "Begin", "BeginTime" });
                        var end = CommonProc.GetPropertyValue(_TargetObject, new List<string>() { "End", "EndTime", "Finish", "FinishTime" });
                        DateTime startTime, endTime;
                        if(start != null && end != null&&DateTime.TryParse(start.ToString(),out startTime)&&DateTime.TryParse(end.ToString(),out endTime))
                            return TimeSensitiveHelper.GetWidth(endTime - startTime);
                    }
                    if (prop != null)
                    {
                        return TimeSensitiveHelper.GetWidth((TimeSpan)prop);
                    }

                }
                return 0.5;
            }
            
        }
        [JsonIgnore]
        public Color Background
        {
            get
            {
                if (_TargetObject != null)
                {
                    var start = CommonProc.GetPropertyValue(_TargetObject, new List<string>() { "Start", "StartTime", "Begin", "BeginTime" });
                    DateTime startTime;
                        
                    if (start != null&&DateTime.TryParse(start.ToString(), out startTime))
                        return TimeSensitiveHelper.GetColor(startTime);

                }
                return Colors.Black;
            }
        }

        public void SetLinkLine()
        {
            SetLinkLine(SourceX, SourceY, TargetX, TargetY);
        }
        public void SetLinkLine(double x1,double y1,double x2,double y2)
        {
            _LinkLine= new ArrowCurveSegment()
            {
                //StrokeThickness = 5,
                Background = new SolidColorBrush(Colors.Blue),
                StartPointX =x1,
                StartPointY=y1,
                EndPointX =x2,
                EndPointY=y2,
                RadiusRadio=0,
                DataContext=this
            };
            Canvas.SetZIndex(_LinkLine, -1);
            _LinkLine.ChangeActive(true);

            Binding bindingsx = new Binding() { Path = new PropertyPath("SourceX"),Mode= BindingMode.TwoWay };
            _LinkLine.SetBinding(ArrowCurveSegment.StartPointXProperty, bindingsx);

            Binding bindingsy = new Binding() { Path = new PropertyPath("SourceY"), Mode = BindingMode.TwoWay };
            _LinkLine.SetBinding(ArrowCurveSegment.StartPointYProperty, bindingsy);

            Binding bindingtx = new Binding() { Path = new PropertyPath("TargetX"), Mode = BindingMode.TwoWay };
            _LinkLine.SetBinding(ArrowCurveSegment.EndPointXProperty, bindingtx);

            Binding bindingty = new Binding() { Path = new PropertyPath("TargetY"), Mode = BindingMode.TwoWay };
            _LinkLine.SetBinding(ArrowCurveSegment.EndPointYProperty, bindingty);

            Binding bindingtext = new Binding() { Path = new PropertyPath("Text"), Mode = BindingMode.TwoWay };
            _LinkLine.SetBinding(ArrowCurveSegment.TextProperty, bindingtext);

            Binding bindingr = new Binding() { Path = new PropertyPath("RadiusRadio"), Mode = BindingMode.TwoWay };
            _LinkLine.SetBinding(ArrowCurveSegment.RadiusRadioProperty, bindingr);

            Binding bindingsweep = new Binding() { Path = new PropertyPath("LineSweepDirection"), Mode = BindingMode.TwoWay };
            _LinkLine.SetBinding(ArrowCurveSegment.LineSweepDirectionProperty, bindingsweep);

            Binding bindingwidth = new Binding() { Path = new PropertyPath("StrokeThickness"), Mode = BindingMode.OneWay };
            _LinkLine.SetBinding(ArrowCurveSegment.StrokeThicknessProperty, bindingwidth);

            Binding bindingbackground = new Binding() { Path = new PropertyPath("Background"), Mode = BindingMode.OneWay, Converter=new ConvertColorToBrush() };
            _LinkLine.SetBinding(ArrowCurveSegment.BackgroundProperty, bindingbackground);

        }



        double _RadiusRadio = 0;
        public double RadiusRadio
        {
            get { return _RadiusRadio; }
            set { _RadiusRadio = value;OnPropertyChanged("RadiusRadio"); }
        }

        SweepDirection _LineSweepDirection = SweepDirection.Clockwise;
        public SweepDirection LineSweepDirection
        {
            get { return _LineSweepDirection; }
            set { _LineSweepDirection = value;OnPropertyChanged("LineSweepDirection"); }

        }

        bool? _IsValid = true;
        public bool? IsValid
        {
            get { return _IsValid; }
            set { _IsValid = value; OnPropertyChanged("IsValid"); }
        }

        ArrowCurveSegment _LinkLine =null;
        [JsonIgnore]
        public ArrowCurveSegment LinkLine//never null
        {
            get
            {
                if (_LinkLine == null)
                    SetLinkLine();
                return _LinkLine;
            }
            set
            {
                _LinkLine = value; 
                OnPropertyChanged("LinkLine");
                //SourcePoint=new Point(value.X1,value.Y1);
                //TargetPoint=new Point(value.X2,value.Y2);
            }
        }

        private bool _IsSelected = false;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (value && SourcePoint != null && SourcePoint.Visibility != Visibility.Visible)
                    SourcePoint.Visibility = Visibility.Visible;
                if (value && SourcePoint != null && TargetPoint.Visibility != Visibility.Visible)
                    TargetPoint.Visibility = Visibility.Visible;

                if (!value && SourcePoint != null && SourcePoint.Visibility == Visibility.Visible)
                    SourcePoint.Visibility = Visibility.Collapsed;
                if (!value && SourcePoint != null && TargetPoint.Visibility == Visibility.Visible)
                    TargetPoint.Visibility = Visibility.Collapsed;
                _IsSelected = value;
            }
        }
        [JsonIgnore]
        public FrameworkElement SourcePoint { get; set; }//connection point
        [JsonIgnore]
        public FrameworkElement TargetPoint { get; set; }//connection point
        Guid _DesignLinkID = Guid.NewGuid();
        public Guid DesignLinkID { get { return _DesignLinkID; } set { _DesignLinkID = value; } }
        public Guid? TargetObjectID { get; set; }

        public Guid? SourceID { get; set; }
        public Guid? TargetID { get; set; }

        public Guid SourceDesignItemID { get; set; }
        public Guid TargetDesignItemID { get; set; }

        //private DesignItem _SourceItem = null;
        //[JsonIgnore]
        //public DesignItem SourceItem
        //{
        //    get { return _SourceItem; }
        //    set
        //    {

        //        if (value == null)
        //        {
        //            SourceID = null;
        //        }
        //        _SourceItem = value;
        //        if (value != null)
        //        {
        //            SourceID = value.TargetObjectID;

        //        }
        //        OnPropertyChanged("SourceItem");
        //    }
        //}
        //private DesignItem _TargetItem = null;
        //[JsonIgnore]
        //public DesignItem TargetItem
        //{
        //    get { return _TargetItem; }
        //    set
        //    {
        //        if (value == null)
        //        {
        //            TargetID = null;

        //        }
        //        _TargetItem = value;
        //        if (value != null)
        //        {
        //            TargetID = value.TargetObjectID;

        //        }
        //        OnPropertyChanged("TargetItem");
        //    }
        //}

        private double _SourceX = 0;
        public double SourceX
        {
            get
            {
                return _SourceX; 
            }
            set
            {
                _SourceX = value;
                OnPropertyChanged("SourceX");
            }
        }

        private double _SourceY = 0;
        public double SourceY
        {
            get {  return _SourceY; }
            set
            {
                _SourceY = value;
                OnPropertyChanged("SourceY");
            }
        }

        private double _TargetX = 0;
        public double TargetX
        {
            get {  return _TargetX; }
            set 
            { 
                _TargetX = value;
                OnPropertyChanged("TargetX"); 
            }
        }

        private double _TargetY = 0;
        public double TargetY
        {
            get { return _TargetY; }
            set
            {
                _TargetY = value;
                OnPropertyChanged("TargetY");
            }
        }

        public RelativePosition SourcePosition { get; set; }
        public RelativePosition TargetPosition { get; set; }

        public static double GetNextRadius( double maxRadius)
        {
            if (maxRadius <= 0.5) return 0.75;
            return maxRadius + 0.25;

        }
        public void SetCurveRadius(DesignItem source, DesignItem target)
        {
            var linkList = source.OutLinkList.Where(v => v.TargetID == target.TargetObjectID && v != this);
            var samePositionLinkList = linkList.Where(v => v.SourcePosition == SourcePosition
            && v.TargetPosition == TargetPosition);
            if (samePositionLinkList.Count() == 0) return;
            var d = samePositionLinkList.Max(v => v.RadiusRadio);
            var l = samePositionLinkList.FirstOrDefault(v => v.RadiusRadio == d);
            if (samePositionLinkList.Count() % 2 == 0)
            {
                LinkLine.RadiusRadio = d;
                if (l.LineSweepDirection == SweepDirection.Clockwise)
                    LinkLine.LineSweepDirection = SweepDirection.Counterclockwise;
                else
                    LinkLine.LineSweepDirection = SweepDirection.Clockwise;
            }
            else
                LinkLine.RadiusRadio = GetNextRadius(d);
        }
    }

}
