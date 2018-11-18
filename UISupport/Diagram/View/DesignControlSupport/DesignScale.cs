using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using DesignTool.Lib.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Numerics;
using Windows.UI.Xaml.Media;
using Windows.Foundation;

namespace DesignTool.Lib.View.DesignControlSupport
{
    public class DesignScale:IDisposable
    {   
        public DesignScale(FrameworkElement target)
        {
            if(target==null) throw new Exception("No avalid target control");
            TargetControl = target;
            ScaleControl = target;
        }
        public DesignScale(FrameworkElement target, FrameworkElement movearea)
        {
            if (target == null) throw new Exception("No avalid target control");
            TargetControl = target;
            ScaleControl = movearea ?? target;
        }

        private bool isScaling = false;

        private bool isLeft = false;
        private bool isTop = false;
        private bool isRight = false;
        private bool isBottom = false;

        public bool IsLeft
        {
            get { return isLeft; }
            set { isLeft = value;}
        }
        public bool IsRight
        {
            get { return isRight; }
            set { isRight = value;  }
        }
        public bool IsTop
        {
            get { return isTop; }
            set { isTop = value;  }
        }
        public bool IsBottom
        {
            get { return isBottom; }
            set { isBottom = value; }
        }

        private Canvas TargetCanvas = null;

        public FrameworkElement TargetControl { get; set; }
        public FrameworkElement ScaleControl { get; set; }
        private double CurrentX = 0;
        private double CurrentY = 0;

        private double PointX = 0;
        private double PointY = 0;
        private double CurrentWidth = 0;
        private double CurrentHeight = 0;

        private bool _IsEnable = false;
        public bool IsEnable
        {
            get { return _IsEnable; }
            set
            {
                _IsEnable = value;
                if (_IsEnable)
                {
                    ScaleControl.PointerPressed += ScaleControl_PointerPressed;
                    ScaleControl.ManipulationMode = Windows.UI.Xaml.Input.ManipulationModes.Scale;
                    ScaleControl.ManipulationDelta += ScaleControl_ManipulationDelta;
                }
                else
                {
                    ScaleControl.PointerPressed -= ScaleControl_PointerPressed;
                }
            }
        }

        private void ScaleControl_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        {
            TargetControl.Width *= e.Delta.Scale;
            TargetControl.Height *= e.Delta.Scale;
        }

        void TargetCanvas_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            TargetCanvas.PointerMoved -= TargetCanvas_PointerMoved;
            TargetCanvas.PointerReleased -= TargetCanvas_PointerReleased;
            isScaling = false;
            e.Handled = true;
        }

        void TargetCanvas_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (isScaling )
            {
                Resize(e);
                e.Handled = true;
            }
            
        }

        void ScaleControl_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (TargetControl == null || TargetControl.Parent == null || !(TargetControl.Parent is Canvas)) return;
            TargetCanvas = TargetControl.Parent as Canvas;
            //Point offset =  VisualTreeHelper.GetOffset(TargetControl);
            CurrentX = Canvas.GetLeft(TargetControl);
            CurrentY = Canvas.GetTop(TargetControl);

            PointX = e.GetCurrentPoint(TargetCanvas).Position.X;
            PointY = e.GetCurrentPoint(TargetCanvas).Position.Y;

            CurrentWidth = TargetControl.ActualWidth;
            CurrentHeight = TargetControl.ActualHeight;

            TargetCanvas.PointerMoved += TargetCanvas_PointerMoved;
            TargetCanvas.PointerReleased += TargetCanvas_PointerReleased;
            isScaling = true;
            e.Handled = true;
        }

        public void Dispose()
        {
            if(ScaleControl!=null)
                ScaleControl.PointerPressed -= ScaleControl_PointerPressed;
            if (TargetCanvas != null)
            {
                TargetCanvas.PointerMoved -= TargetCanvas_PointerMoved;
                TargetCanvas.PointerReleased -= TargetCanvas_PointerReleased;
            }
            ScaleControl = null;
            TargetControl = null;
            TargetCanvas = null;
        }

        void Resize(Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            
            var dx = e.GetCurrentPoint(TargetCanvas).Position.X - PointX;
            var dy = e.GetCurrentPoint(TargetCanvas).Position.Y - PointY;
            PointX = e.GetCurrentPoint(TargetCanvas).Position.X;
            PointY = e.GetCurrentPoint(TargetCanvas).Position.Y;
            Point offset = new Point(Canvas.GetLeft(TargetControl),Canvas.GetTop(TargetControl));
            CurrentX = offset.X;
            CurrentY = offset.Y;
            if (isLeft)
            {
                
                
                if (TargetControl.ActualWidth > dx)
                {
                    Canvas.SetLeft(TargetControl, CurrentX + dx);
                    ResizeLinkPoint(dx, (TargetControl.ActualWidth - dx) / TargetControl.ActualWidth, 0, 1);
                    TargetControl.Width = TargetControl.ActualWidth - dx;
                    
                }
            }
            if (isRight)
            {
                if (TargetControl.ActualWidth + dx > 0)
                {
                    ResizeLinkPoint(0, (TargetControl.ActualWidth + dx) / TargetControl.ActualWidth, 0, 1);
                    TargetControl.Width = TargetControl.ActualWidth + dx;
                    
                }
            }
            if (isTop)
            {
                
                if (TargetControl.ActualHeight - dy > 0)
                {
                    Canvas.SetTop(TargetControl, CurrentY + dy);
                    ResizeLinkPoint(0, 1, dy, (TargetControl.ActualHeight - dy) / TargetControl.ActualHeight);
                    TargetControl.Height = TargetControl.ActualHeight - dy;
                    
                }
            }
            if (isBottom)
            {
                if (TargetControl.ActualHeight + dy > 0)
                {
                    ResizeLinkPoint(0, 1, 0, (TargetControl.ActualHeight + dy) / TargetControl.ActualHeight);
                    TargetControl.Height = TargetControl.ActualHeight + dy;
                    
                }
            }
        }

        void ResizeLinkPoint(double dx,double scaleX,double dy,double scaleY)
        {
            if (TargetControl.DataContext!=null&&TargetControl.DataContext is DesignItem)
            {
                (TargetControl.DataContext as DesignItem).InLinkList.ForEach(v =>
                    {
                        if (IsLeft)
                            v.TargetX = Canvas.GetLeft(TargetControl);
                        if (IsTop)
                            v.TargetY = Canvas.GetTop(TargetControl);
                        if (IsRight)
                            v.TargetX = Canvas.GetLeft(TargetControl)+ TargetControl.ActualWidth;
                        if (IsBottom)
                            v.TargetY = Canvas.GetTop(TargetControl)+TargetControl.ActualHeight;
                        //if (v.TargetX>TargetControl.ActualWidth/2+Canvas.GetLeft(TargetControl))
                        //var rx = v.TargetX - CurrentX;
                        //v.TargetX = rx*scaleX+CurrentX+dx;
                        //var ry = v.TargetY - CurrentY;
                        //v.TargetY = ry*scaleY+CurrentY+dy;
                    });
                (TargetControl.DataContext as DesignItem).OutLinkList.ForEach(v =>
                {
                    //var rx = v.SourceX - CurrentX;
                    //v.SourceX = rx*scaleX+CurrentX+dx;
                    //var ry = v.SourceY - CurrentY;
                    //v.SourceY = ry*scaleY + CurrentY+dy;
                    if (IsLeft)
                        v.SourceX = Canvas.GetLeft(TargetControl);
                    if (IsTop)
                        v.SourceY = Canvas.GetTop(TargetControl);
                    if (IsRight)
                        v.SourceX = Canvas.GetLeft(TargetControl) + TargetControl.ActualWidth;
                    if (IsBottom)
                        v.SourceY = Canvas.GetTop(TargetControl) + TargetControl.ActualHeight;
                });
            }
        }

    }
}
