using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using DesignTool.Lib.Model;
using DesignTool.Lib.ViewModel;
using Windows.Devices.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI;
using Windows.UI.Xaml.Shapes;
using Windows.Foundation;
using UISupport.Diagram.Controls;
using CommonLib;

namespace DesignTool.Lib.View.DesignControlSupport
{
    public class DesignConnection:IDisposable
    {
        public DesignConnection(FrameworkElement targetControl, FrameworkElement connectionControl)
        {
            if(targetControl==null) throw new Exception("Can not assign target as null");
            TargetControl = targetControl;
            ConnectionControl = connectionControl;
            //if(inControlList==null&&outControlList==null)
            //    throw new Exception("Must assign a control as in control or out control");
            //OutControlList = outControlList;
            //InControlList = inControlList;
        }
        private bool isLeft = false;
        private bool isTop = false;
        private bool isRight = false;
        private bool isBottom = false;

        public bool IsLeft
        {
            get { return isLeft; }
            set { isLeft = value; }
        }
        public bool IsRight
        {
            get { return isRight; }
            set { isRight = value; }
        }
        public bool IsTop
        {
            get { return isTop; }
            set { isTop = value; }
        }
        public bool IsBottom
        {
            get { return isBottom; }
            set { isBottom = value; }
        }

        public FrameworkElement TargetControl { get; set; }
        public FrameworkElement ConnectionControl { get; set; }
        //public List<FrameworkElement> OutControlList { get; set; }
        //public List<FrameworkElement> InControlList { get; set; }

        //private FrameworkElement CurrentOutControl;
        //private FrameworkElement CurrentInControl;

        private bool _IsOutEnable = false;
        public bool IsOutEnable
        {
            get { return _IsOutEnable; }
            set
            {
                _IsOutEnable = value;
                if (value)
                    ConnectionControl.PointerPressed += OutControl_PointerPressed;
                else
                    ConnectionControl.PointerPressed -= OutControl_PointerPressed;
                //if (_IsOutEnable)
                //{
                //    OutControlList.ForEach(v=> v.PointerPressed += OutControl_PointerPressed);
                //}
                //else
                //{
                //    OutControlList.ForEach(v=>v.PointerPressed -= OutControl_PointerPressed);
                //}
            }
        }

        private bool _IsInEnable = false;
        public bool IsInEnable
        {
            get { return _IsInEnable; }
            set
            {
                _IsInEnable = value;
                if (value)
                    ConnectionControl.PointerReleased += InControl_PointerReleased;
                else
                    ConnectionControl.PointerReleased -= InControl_PointerReleased;
                //if (_IsInEnable)
                //{
                //    InControlList.ForEach(v=> v.PointerReleased += InControl_PointerReleased);
                //}
                //else
                //{
                //    InControlList.ForEach(v=>v.PointerReleased -= InControl_PointerReleased);
                //}
            }
        }
        

        private void InControl_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (CurrentLink == null) return;

            if(CurrentConnection!=null&&CurrentConnection!=this)
                CurrentConnection.Silent();
            if (DesignManager.CreateConnection != null)
            {
                var sid = (CurrentLink.SourcePoint.DataContext as DesignItem).TargetObjectID;
                var tid = (TargetControl.DataContext as DesignItem).TargetObjectID;

                CurrentLink.TargetObject = DesignManager.CreateConnection(sid.Value, tid.Value);
            }

            if (TargetControl.DataContext is DesignItem )
            {
                var targetItem = (TargetControl.DataContext as DesignItem);
                CurrentLink.TargetID = targetItem.TargetObjectID;
                CurrentLink.TargetDesignItemID = targetItem.DesignItemID;
                CurrentLink.TargetPoint = sender as FrameworkElement;

                
                targetItem.InLinkList.Add(CurrentLink);

                var sourceItem = CurrentLink.SourcePoint.DataContext as DesignItem;
                CurrentLink.SourceID = sourceItem.TargetObjectID;
                CurrentLink.SourceDesignItemID = sourceItem.DesignItemID;
                sourceItem.OutLinkList.Add(CurrentLink);

                if (IsBottom)
                    CurrentLink.TargetPosition = RelativePosition.Bottom;
                if (IsLeft)
                    CurrentLink.TargetPosition = RelativePosition.Left;
                if (IsTop)
                    CurrentLink.TargetPosition = RelativePosition.Top;
                if (IsRight)
                    CurrentLink.TargetPosition = RelativePosition.Right;

                CurrentLink.TargetX = GetCenter().X;
                CurrentLink.TargetY = GetCenter().Y;

                var model = TargetCanvas.DataContext as DesignCanvasViewModel;
                if(model!=null)
                    model.LinkList.Add(CurrentLink);

                CurrentLink.SetCurveRadius( sourceItem, targetItem);
            }

            IsConnecting = false;
            CurrentLink.LinkLine.PointerPressed += LinkLine_PointerPressed; ;

            CurrentLink = null;

            Silent();
            
            e.Handled = true;
        }

        void LinkLine_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var l = sender as FrameworkElement;
            if (l == null) return;
            var connection = l.DataContext as DesignLink;
            if (connection == null) return;

            connection.IsSelected = true;
            
            CurrentLink = connection;
            e.Handled = true;
        }

        private double PointX = 0;
        private double PointY = 0;

        public static bool IsConnecting = false;
        
        public static DesignLink CurrentLink = null;
        public static DesignConnection CurrentConnection = null;
        public void Silent()
        {
            TargetCanvas.PointerMoved -= TargetCanvas_PointerMoved;
            TargetCanvas.PointerReleased -= TargetCanvas_PointerReleased;
        }
        public Canvas TargetCanvas
        {
            get 
            { 
                //if (TargetControl==null || TargetControl.Parent == null || !(TargetControl.Parent is Canvas)) return null;
                if(TargetControl.Parent==null) throw new Exception("Lost canvas");
                return TargetControl.Parent as Canvas;
            }
        }

        public void TargetCanvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            if (TargetCanvas == null) return;
            Silent();
            if(IsConnecting)
            {
                IsConnecting = false;
                if (CurrentLink != null && TargetCanvas.Children.Contains(CurrentLink.LinkLine))
                {
                    TargetCanvas.Children.Remove(CurrentLink.LinkLine); //can not need add a connection
                }

            }
            CurrentLink = null;
            CurrentConnection = null;
            e.Handled = true;
        }

        public void TargetCanvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (IsConnecting )
            {
                Connecting(e);
                e.Handled = true;
            }
            
        }

        void OutControl_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (TargetCanvas == null) return;

            PointX = e.GetCurrentPoint(TargetCanvas).Position.X;
            PointY = e.GetCurrentPoint(TargetCanvas).Position.Y;
            CurrentLink = null;
            TargetCanvas.PointerMoved += TargetCanvas_PointerMoved;
            TargetCanvas.PointerReleased += TargetCanvas_PointerReleased;
            IsConnecting = true;
            CurrentConnection = this;
            //CurrentOutControl = sender as FrameworkElement;
            e.Handled = true;
            //System.Windows.DragDrop.DoDragDrop(OutControl, OutControl, DragDropEffects.Copy);
        }
        void Connected(PointerRoutedEventArgs e)
        {

        }
        Point GetCenter()
        {
            double bx = Canvas.GetLeft(TargetControl);
            double by = Canvas.GetTop(TargetControl);
            if (IsLeft)
            {
                by += TargetControl.ActualHeight/2;
            }
            if (IsTop)
            {
                bx += TargetControl.ActualWidth/2;
            }
            if (IsRight)
            {
                bx += TargetControl.ActualWidth;
                by += TargetControl.ActualHeight / 2;
            }
            if (IsBottom)
            {
                bx += TargetControl.ActualWidth/2;
                by += TargetControl.ActualHeight;
            }
            return new Point()
            {
                X = bx,
                Y = by,
            };
        }
        void Connecting(PointerRoutedEventArgs e)
        {
            if (TargetCanvas == null) return;
            if (CurrentLink == null)
            {
                CurrentLink=new DesignLink()
                {
                    SourceX = GetCenter().X,
                    SourceY=GetCenter().Y,
                    TargetX= e.GetCurrentPoint(TargetCanvas).Position.X,
                    TargetY= e.GetCurrentPoint(TargetCanvas).Position.Y
                };
                CurrentLink.SourcePoint = ConnectionControl;
                if (IsBottom)
                    CurrentLink.SourcePosition = RelativePosition.Bottom;
                if (IsLeft)
                    CurrentLink.SourcePosition = RelativePosition.Left;
                if (IsTop)
                    CurrentLink.SourcePosition = RelativePosition.Top;
                if (IsRight)
                    CurrentLink.SourcePosition = RelativePosition.Right;
            }
            //if (CurrentLink.LinkLine == null)
            //{
            //    CurrentLink.SetLinkLine(PointX, PointY, e.GetCurrentPoint(TargetCanvas).Position.X, e.GetCurrentPoint(TargetCanvas).Position.Y);

            //}
            //CurrentLink.SourceX = PointX;
            //CurrentLink.SourceY = PointY;

            CurrentLink.TargetX = e.GetCurrentPoint(TargetCanvas).Position.X;
            CurrentLink.TargetY = e.GetCurrentPoint(TargetCanvas).Position.Y;


            if (!TargetCanvas.Children.Contains(CurrentLink.LinkLine))
                TargetCanvas.Children.Add(CurrentLink.LinkLine);
        }

        public void Dispose()
        {
            //if (InControlList != null) InControlList.ForEach(v=>v.PointerReleased -= InControl_PointerReleased);
            //if (OutControlList != null) OutControlList.ForEach(v=>v.PointerPressed -= OutControl_PointerPressed);
            if (TargetCanvas != null)
            {
                TargetCanvas.PointerMoved -= TargetCanvas_PointerMoved;
                TargetCanvas.PointerReleased -= TargetCanvas_PointerReleased;
            }
            //InControlList = null;
            //OutControlList = null;
            TargetControl = null;
        }
    }
}
