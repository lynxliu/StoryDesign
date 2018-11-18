using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using CommonLib;
using Newtonsoft.Json;
using UISupport;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;
using System.Reflection;
using Windows.UI;

namespace DesignTool.Lib.Model
{
    public class DesignItem : ViewModelBase
    {
        bool? _IsValid = true;
        public bool? IsValid
        {
            get { return _IsValid; }
            set { _IsValid = value;OnPropertyChanged("IsValid"); }
        }
        private double _ZIndex = 0;
        public double ZIndex
        {
            get { return _ZIndex; }
            set { _ZIndex = value; OnPropertyChanged("ZIndex"); }
        }

        private double _Left = 0;
        public double Left
        {
            get { return _Left; }
            set { _Left = value; OnPropertyChanged("Left"); }
        }

        private double _Top = 0;
        public double Top
        {
            get { return _Top; }
            set { _Top = value; OnPropertyChanged("Top"); }
        }

        private double _Width = 0;
        public double Width
        {
            get
            {
                return _Width;
            }
            set { _Width = value; OnPropertyChanged("Width"); }
        }

        private double _Height = 0;
        public double Height
        {
            get { return _Height; }
            set { _Height = value; OnPropertyChanged("Height"); }
        }

        [JsonIgnore]
        public Point CenterPoint
        {
            get
            {
                return new Point()
                {
                    X = Left + Width / 2,
                    Y = Top + Height / 2
                };
            }
        }
        [JsonIgnore]
        public Point LeftTopPoint
        {
            get
            {
                return new Point()
                {
                    X = Left,
                    Y = Top
                };
            }
        }
        [JsonIgnore]
        public Point TopPoint
        {
            get
            {
                return new Point()
                {
                    X = Left + Width / 2,
                    Y = Top 
                };
            }
        }
        [JsonIgnore]
        public Point TopRightPoint
        {
            get
            {
                return new Point()
                {
                    X = Left + Width,
                    Y = Top
                };
            }
        }
        [JsonIgnore]
        public Point RightPoint
        {
            get
            {
                return new Point()
                {
                    X = Left + Width,
                    Y = Top + Height / 2
                };
            }
        }
        [JsonIgnore]
        public Point RightBottomPoint
        {
            get
            {
                return new Point()
                {
                    X = Left + Width,
                    Y = Top + Height
                };
            }
        }
        [JsonIgnore]
        public Point BottomPoint
        {
            get
            {
                return new Point()
                {
                    X = Left + Width / 2,
                    Y = Top + Height
                };
            }
        }
        [JsonIgnore]
        public Point BottomLeftPoint
        {
            get
            {
                return new Point()
                {
                    X = Left,
                    Y = Top + Height
                };
            }
        }
        [JsonIgnore]
        public Point LeftPoint
        {
            get
            {
                return new Point()
                {
                    X = Left,
                    Y = Top + Height / 2
                };
            }
        }

        List<DesignLink> _InLinkList = new List<DesignLink>();
        List<DesignLink> _OutLinkList = new List<DesignLink>();

        public string DesignCommand { get; set; }

        [JsonIgnore]
        public List<DesignLink> InLinkList { get { return _InLinkList; } }
        [JsonIgnore]
        public List<DesignLink> OutLinkList { get { return _OutLinkList; } }

        Guid _DesignItemID = Guid.NewGuid();
        public Guid DesignItemID { get { return _DesignItemID; } set { _DesignItemID = value; } }
        public Guid? TargetObjectID { get; set; }

        void SetObjectID(PropertyInfo idProperty,object vm)
        {
            if (idProperty != null)
            {
                var id = idProperty.GetValue(vm);
                if (id != null)
                    TargetObjectID = Guid.Parse(id.ToString());
            }
        }
        void SetObjectInfo(PropertyInfo nameProperty, PropertyInfo memoProperty, object vm)
        {
            var s = "";
            if (nameProperty != null)
            {
                var vs = nameProperty.GetValue(vm);
                if(vs!=null)
                    s = vs.ToString();
                
            }
            if(memoProperty!=null)
            {
                var vs = memoProperty.GetValue(vm);
                if (vs != null)
                {
                    if (!string.IsNullOrEmpty(s)) s += ":";
                    s += vs.ToString();
                }
            }
            if (!string.IsNullOrEmpty(s))
                Info = s;
        }

        object _TargetObject;
        [JsonIgnore]
        public object TargetObject
        {
            get
            {
                if (_TargetObject == null)
                    TargetControl = DesignManager.GetDesignControl(TargetObjectID);
                return _TargetObject;
            }
            set {
                if (value != null)
                {
                    var idProperty = value.GetType().GetProperty("ObjectID");
                    if (idProperty == null)
                        idProperty = value.GetType().GetProperty("ObjectId");
                    if (idProperty == null)
                        idProperty = value.GetType().GetProperty("ID");
                    if (idProperty == null)
                        idProperty = value.GetType().GetProperty("Id");
                    if (idProperty == null)
                        idProperty = value.GetType().GetProperty("Identity");
                    if (idProperty != null)
                    {
                        SetObjectID(idProperty, value);
                    }

                    var nameProperty = value.GetType().GetProperty("Name");
                    var memoProperty = value.GetType().GetProperty("Memo");
                    SetObjectInfo(nameProperty, memoProperty, value);
                }
                _TargetObject = value;
                OnPropertyChanged("TargetObject");
            } }

        FrameworkElement _TargetContent;
        [JsonIgnore]
        public FrameworkElement TargetControl
        {
            get {
                if (_TargetContent == null)
                    _TargetContent = DesignManager.GetDesignControl(TargetObjectID);
                return _TargetContent; }
            set {
                _TargetContent = value;
                if (value != null && value.DataContext != null)
                {
                    var dc = value.DataContext;
                    TargetObject = dc;
                }
                OnPropertyChanged("TargetControl");

            }
        }
        private Image IconImage = new Image();
        [JsonIgnore]
        public FrameworkElement TargetContent
        {
            get
            {
                if (ShowMode == DesignItemShowMode.Full)
                    return TargetControl;
                else
                {
                    IconImage.Source = Icon;
                    return IconImage;
                }
            }
        }
        public string IconPath { get; set; }

        private BitmapImage _Icon = null;
        [JsonIgnore]
        public BitmapImage Icon
        {
            get
            {
                if (_Icon == null)
                {
                    if (string.IsNullOrEmpty(IconPath))
                    {
                        IconPath = "Diagram/Images/DefaultIcon.png";
                    }
                    _Icon = new BitmapImage(new Uri(IconPath, UriKind.RelativeOrAbsolute));
                }
                return _Icon;
            }
            set
            {
                _Icon = value; OnPropertyChanged("Icon");

            }
        }

        bool _IsActive = false;
        public bool IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value;OnPropertyChanged("IsActive"); }
        }

        DesignItemShowMode _ShowMode = DesignItemShowMode.Full;
        public DesignItemShowMode ShowMode
        {
            get { return _ShowMode; }
            set { _ShowMode = value;OnPropertyChanged("ShowMode"); OnPropertyChanged("TargetContent"); }
        }

        string _Info;
        public string Info
        {
            get
            {
                return _Info; 
            }
            set { _Info = value;OnPropertyChanged("Info"); }
        }

        [JsonIgnore]
        public double FontSize
        {
            get
            {
                if (_TargetObject != null)
                {
                    var prop = CommonProc.GetPropertyValue(_TargetObject, new List<string>() { "Continue", "ContinueTime" });
                    if (prop == null)
                    {
                        var start = CommonProc.GetPropertyValue(_TargetObject, new List<string>() { "Start", "StartTime", "Begin", "BeginTime" });
                        var end = CommonProc.GetPropertyValue(_TargetObject, new List<string>() { "End", "EndTime", "Finish", "FinishTime" });
                        DateTime startTime, endTime;
                        if (start != null && end != null && DateTime.TryParse(start.ToString(), out startTime) && DateTime.TryParse(end.ToString(), out endTime))
                            return TimeSensitiveHelper.GetFontSize(endTime - startTime);
                    }
                    if (prop != null)
                    {
                        return TimeSensitiveHelper.GetFontSize((TimeSpan)prop);
                    }

                }
                return TimeSensitiveHelper.MinFontSize;
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

                    if (start != null && DateTime.TryParse(start.ToString(), out startTime))
                        return TimeSensitiveHelper.GetColor(startTime);

                }
                return Colors.Black;
            }
        }
    }

    public enum DesignItemShowMode
    {
        Full, Icon
    }
}
