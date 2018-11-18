using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;
using System.Reflection;
using DesignTool.Lib.Model;
using DesignTool.Lib.View;
using UISupport.Diagram.View;
using Windows.UI.Xaml.Media;
using Windows.UI;
using Windows.UI.Xaml.Data;
using UISupport;
using DesignTool.Lib.View.DesignControlSupport;
using CommonLib;

namespace DesignTool.Lib
{
    public class DesignManager
    {
        static Brush _NotValidBrush = new SolidColorBrush(Colors.Gray);
        static Brush _NotExistBrush = new SolidColorBrush(Colors.Red);

        public static Brush NotValidBrush
        {
            get { return _NotValidBrush; }
            set { _NotValidBrush = value; }
        }
        public static Brush NotExistBrush
        {
            get { return _NotExistBrush; }
            set { _NotExistBrush = value; }
        }

        #region interface

        public static Func<bool> IsAutoConnectNode { get; set; }

        static Dictionary<string, Func<FrameworkElement>> _DefaultCreateCommandList = new Dictionary<string, Func<FrameworkElement>>();
        public static Dictionary<string,Func<FrameworkElement>> DefaultCreateCommandList { get { return _DefaultCreateCommandList; } }
        public static Func<Guid,Guid,object> CreateConnection { get; set; }
        public static Func<Guid, Guid, List<object>> GetConnection { get; set; }
        public static Func<Guid?, object> GetObject { get; set; }//must fill 
        public static Func<object,FrameworkElement> GetDesignControl { get; set; }
        public static Func<object, Tuple<Guid,Guid,object>> GetDesignLink { get; set; }
        public static Action<object> SetCurrent { get; set; }

        //public static void RegisterCreateMenu(string name,Func<FrameworkElement> createFunc)
        //{
        //    if (string.IsNullOrEmpty(name))
        //        return;
        //    if (CreateCommandList.ContainsKey(name))
        //        CreateCommandList[name] = createFunc;
        //    else
        //        CreateCommandList.Add(name, createFunc);
        //}
        //public static void Register<Model, ViewModel, View>(string command, Func<Guid?, FrameworkElement> getControl = null)
        //{
        //    CommandList.Add(new DesignCommand()
        //    {
        //        ModelType = typeof(Model),
        //        ViewType = typeof(View),
        //        ViewModelType = typeof(ViewModel),
        //        TargetCommand = command,
        //        GetDesignControl = getControl
        //    });
        //}

        #endregion
        public static DesignControl CreateDesignControl(Guid dataID)
        {
            var data = GetObject(dataID);
            return CreateDesignControl(data);
        }
        public static DesignControl CreateDesignControl(object data)
        {
            if (GetDesignControl == null)
                return null;
            var control = GetDesignControl(data);
            return CreateDesignControl(control);
        }
        public static DesignControl CreateDesignControl(FrameworkElement control, DesignItem vm=null)
        {
            if (vm == null)
            {
                vm = new DesignItem() { Width=200,Height=150};
            }
            vm.TargetControl = control;
            var dc = control.DataContext;
            if (dc != null && dc as IUIActiveSupport != null)
            {
                (dc as IUIActiveSupport).SetActive.Add(() => { vm.IsActive = true; });
                (dc as IUIActiveSupport).SetDeActive.Add(() => { vm.IsActive = false; });
            }
            return new DesignControl() { DataContext = vm };

        }
        //public static FrameworkElement GetDesignControl(Guid? objectID, string command)
        //{
        //    var contentControl = GetDesignContentControl(objectID, command);
        //    if (contentControl != null)
        //        return CreateDesignControl(contentControl);
        //    return new FaildControl() { DataContext = new DesignItem() { TargetObjectID = objectID, DesignCommand = command } };
        //}
        //public static FrameworkElement GetDesignContentControl(Guid? objectID,string command)
        //{
        //    if (GetObject == null)
        //        return null;
        //    var obj = GetObject(objectID);
        //    if(obj!=null)
        //    {
        //        var type = obj.GetType();
        //        var c = CommandList.Where(v => v.TargetCommand == command);
        //        if (c.Count() == 0)
        //            return null;
        //        var tc = c.Where(v => v.ModelType == type);
        //        if (tc.Count() >= 1)
        //            return tc.FirstOrDefault().GetControl(objectID);
        //        else
        //        {
        //            tc = c.Where(v => type.GetTypeInfo().IsSubclassOf(v.ModelType)||type.GetInterfaces().Contains(v.ModelType));
        //            if (tc.Count() == 0) return null;
        //            List<Tuple<int, DesignCommand>> rl = new List<Tuple<int, DesignCommand>>();
        //            foreach (var v in tc)
        //            {
        //                rl.Add(new Tuple<int, DesignCommand>(GetParentTypeStep(type, v.ModelType), v));
        //            }
        //            rl.OrderBy(v => v.Item1);
        //            return rl.FirstOrDefault().Item2.GetControl(objectID);
        //        }
        //    }
        //    return null;
        //}
        public static int GetParentTypeStep(Type t,Type parent)
        {

            if (!t.GetTypeInfo().IsSubclassOf(parent)&&!t.GetInterfaces().Contains(parent)) return -1;
            if (t == parent) return 0;
            if (t.GetTypeInfo().BaseType == parent) return 1;
            
            return GetParentTypeStep(t.GetTypeInfo().BaseType, parent) + 1;
        }
        //static List<DesignCommand> _CommandList = new List<DesignCommand>();
        //public static List<DesignCommand> CommandList { get { return _CommandList; } }

        public static Tuple<Point,Point> GetRelationPosition(DesignItem source,DesignItem target)
        {
            var rp = GetRelativePosition(source, target);
            if(rp== RelativePosition.LeftTop)
                return new Tuple<Point, Point>(source.LeftTopPoint, target.RightBottomPoint);
            if (rp == RelativePosition.Top)
                return new Tuple<Point, Point>(source.TopPoint, target.BottomPoint);
            if (rp == RelativePosition.TopRight)
                return new Tuple<Point, Point>(source.TopRightPoint, target.BottomLeftPoint);
            if (rp == RelativePosition.Right)
                return new Tuple<Point, Point>(source.RightPoint, target.LeftPoint);
            if (rp == RelativePosition.RightBottom)
                return new Tuple<Point, Point>(source.RightBottomPoint, target.LeftTopPoint);
            if (rp == RelativePosition.Bottom)
                return new Tuple<Point, Point>(source.BottomPoint, target.TopPoint);
            if (rp == RelativePosition.BottomLeft)
                return new Tuple<Point, Point>(source.BottomLeftPoint, target.TopRightPoint);
            if (rp == RelativePosition.Left)
                return new Tuple<Point, Point>(source.LeftPoint, target.RightPoint);

                return new Tuple<Point, Point>(source.CenterPoint, target.CenterPoint);

        }

        static RelativePosition GetRelativePosition(DesignItem source, DesignItem target)
        {
            var sc = source.CenterPoint;
            var tc = target.CenterPoint;
            var dx = tc.X - sc.X;
            var dy = tc.Y - sc.Y;
            var hd = source.Width / 2 + target.Width / 2;
            var vd = source.Height / 2 + target.Height / 2;

            if (dx<hd*-1)
            {
                if (dy<vd*-1)
                {
                    return RelativePosition.LeftTop;
                }
                if (dy > vd)
                    return RelativePosition.BottomLeft;
                return RelativePosition.Left;
            }
            if (dx > hd )
            {
                if (dy < vd * -1)
                {
                    return RelativePosition.TopRight;
                }
                if (dy > vd)
                    return RelativePosition.RightBottom;
                return RelativePosition.Right;
            }
            if (dy < vd * -1)
                return RelativePosition.Top;
            if (dy > vd)
                return RelativePosition.Bottom;
            return RelativePosition.Overlay;
        }
        public static Point GetPoint(DesignItem item, RelativePosition position)
        {
            if (position == RelativePosition.Left)
                return item.LeftPoint;
            if (position == RelativePosition.Right)
                return item.RightPoint;
            if (position == RelativePosition.Top)
                return item.TopPoint;
            if (position == RelativePosition.Bottom)
                return item.BottomPoint;
            return item.CenterPoint;
        }
        public static Tuple<RelativePosition, RelativePosition> GetLinkConnection(DesignItem source,DesignItem target)
        {
            var sc = source.CenterPoint;
            var tc = target.CenterPoint;
            var dx = target.CenterPoint.X - source.CenterPoint.X;
            var dy = target.CenterPoint.Y - source.CenterPoint.Y;

            if (Math.Abs(dx) > Math.Abs(dy))
            {
                if(dx>0)
                    return new Tuple<RelativePosition, RelativePosition>
                        (RelativePosition.Right, RelativePosition.Left);
                else
                    return new Tuple<RelativePosition, RelativePosition>
                        (RelativePosition.Left, RelativePosition.Right);
            }
            else
            {
                if (dy > 0)
                    return new Tuple<RelativePosition, RelativePosition>
                        (RelativePosition.Bottom, RelativePosition.Top);
                else
                    return new Tuple<RelativePosition, RelativePosition>
                        (RelativePosition.Top, RelativePosition.Bottom);
            }
        }
    }

    public class DesignCommand
    {
        public Type ModelType { get; set; }
        public string TargetCommand { get; set; }
        public Func<Guid?, FrameworkElement> GetDesignControl { get; set; }
        public FrameworkElement GetControl(Guid? id)
        {
            if (GetDesignControl != null)
                return GetDesignControl(id);
            var c= GetViewControl(id);
            if (c != null)
            {
                return DesignManager.CreateDesignControl(c);
            }
            c = new FaildControl();
            return DesignManager.CreateDesignControl(c);
        }

        public Type ViewType { get; set; }
        public Type ViewModelType { get; set; }

        FrameworkElement GetViewControl(Guid? id)
        {
            var view= Activator.CreateInstance(ViewType) as FrameworkElement;
            var viewModel = Activator.CreateInstance(ViewModelType);
            var property = ViewModelType.GetProperty("Target");
            if(property==null)
                property = ViewModelType.GetProperty("TargetObject");
            if (property == null)
                property = ViewModelType.GetProperty("DataObject");
            if (property != null)
            {
                property.SetValue(viewModel, DesignManager.GetObject(id));
            }

            
            view.DataContext = viewModel;
            return view;
        }
    }

    public class ConvertValidToBorderThickness : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || (bool)value == false)
                return new Thickness() { Bottom = 2, Left = 2, Right = 2, Top = 2 };
            return new Thickness() { Bottom = 0, Left = 0, Right = 0, Top = 0 };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ConvertValidToBorder : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return DesignManager.NotExistBrush;
            if ((bool)value == true)
                return DesignManager.NotValidBrush;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ConvertValidToOpacity : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
                return 0.3;
            if ((bool)value == true)
                return 1;
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class ConvertDoubleToBorderThickness : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null  || string.IsNullOrEmpty(value.ToString()))
                return new Thickness() { Bottom = 0.5, Left = 0.5, Right = 0.5, Top = 0.5 };
            double width = 0.5;
            var d = double.TryParse(value.ToString(), out width);
            return new Thickness() { Bottom = width, Left = width, Right = width, Top = width };
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class ConvertColorToBrush : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || (Color)value == null)
                return DesignManager.NotValidBrush;

            return new SolidColorBrush((Color)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
