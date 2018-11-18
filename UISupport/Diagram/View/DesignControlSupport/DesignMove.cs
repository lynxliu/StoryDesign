using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using DesignTool.Lib.Model;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.Foundation;

namespace DesignTool.Lib.View.DesignControlSupport
{
    public class DesignMove:IDisposable
    {
        public DesignMove(FrameworkElement target)
        {
            if(target==null) throw new Exception("No avalid target control");
            TargetControl = target;
            MoveControl = target;
        }
        public DesignMove(FrameworkElement target, FrameworkElement movearea)
        {
            if (target == null) throw new Exception("No avalid target control");
            TargetControl = target;
            MoveControl = movearea ?? target;
        }
        public FrameworkElement TargetControl { get; set; }
        public FrameworkElement MoveControl { get; set; }

        private Canvas TargetCanvas;

        private bool _IsEnable = false;
        public bool IsEnable
        {
            get { return _IsEnable; }
            set 
            { 
                _IsEnable = value;
                if (_IsEnable)
                {
                    MoveControl.PointerPressed += MoveControl_PointerPressed;
                    //MoveControl.ManipulationMode = Windows.UI.Xaml.Input.ManipulationModes.All;
                    //MoveControl.ManipulationDelta += MoveControl_ManipulationDelta;
                }
                else
                {
                    MoveControl.PointerPressed -= MoveControl_PointerPressed;
                }
            }
        }

        //private void MoveControl_ManipulationDelta(object sender, Windows.UI.Xaml.Input.ManipulationDeltaRoutedEventArgs e)
        //{
        //    Canvas.SetLeft(MoveControl, Canvas.GetLeft(MoveControl) + e.Delta.Translation.X);
        //    Canvas.SetTop(MoveControl, Canvas.GetTop(MoveControl) + e.Delta.Translation.Y);
        //}

        void MoveControl_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (TargetControl == null || TargetControl.Parent == null || !(TargetControl.Parent is Canvas)) return;
            TargetCanvas = TargetControl.Parent as Canvas;

            Point offset =new Point(Canvas.GetLeft(TargetControl),Canvas.GetTop(TargetControl));
            CurrentX = offset.X;
            CurrentY = offset.Y;

            PointX = e.GetCurrentPoint(TargetCanvas).Position.X;
            PointY = e.GetCurrentPoint(TargetCanvas).Position.Y;

            //MoveControl.MouseMove += MoveControl_MouseMove;
            //MoveControl.MouseLeftButtonUp += MoveControl_MouseLeftButtonUp;

            TargetCanvas.PointerMoved += TargetCanvas_PointerMoved;
            TargetCanvas.PointerReleased += TargetCanvas_PointerReleased;
            isMoving = true;
            e.Handled = true;
        }

        void TargetCanvas_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            TargetCanvas.PointerMoved -= TargetCanvas_PointerMoved;
            TargetCanvas.PointerReleased -= TargetCanvas_PointerReleased;
            isMoving = false;
            e.Handled = true;
        }

        void TargetCanvas_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var pos = e.GetCurrentPoint(TargetCanvas);
            if (isMoving &&pos.Properties.IsLeftButtonPressed)
            {
                var dx = e.GetCurrentPoint(TargetCanvas).Position.X - PointX;
                var dy = e.GetCurrentPoint(TargetCanvas).Position.Y - PointY;

                PointX = e.GetCurrentPoint(TargetCanvas).Position.X;
                PointY = e.GetCurrentPoint(TargetCanvas).Position.Y;

                CurrentX += dx;
                CurrentY += dy;
                Canvas.SetLeft(TargetControl, CurrentX);
                Canvas.SetTop(TargetControl, CurrentY);

                if (TargetControl.DataContext is DesignItem)
                {
                    (TargetControl.DataContext as DesignItem).InLinkList.ForEach(v =>
                    {
                        v.TargetX +=dx;
                        v.TargetY += dy;
                    });
                    (TargetControl.DataContext as DesignItem).OutLinkList.ForEach(v =>
                    {
                        v.SourceX += dx;
                        v.SourceY += dy;
                    });
                }
                e.Handled = true;
            }
            
        }

        private bool isMoving = false;
        private double CurrentX = 0;
        private double CurrentY = 0;

        private double PointX = 0;
        private double PointY = 0;

        public void Dispose()
        {
            if(MoveControl!=null)
                MoveControl.PointerPressed -= MoveControl_PointerPressed;
            if (TargetCanvas != null)
            {
                TargetCanvas.PointerMoved -= TargetCanvas_PointerMoved;
                TargetCanvas.PointerReleased -= TargetCanvas_PointerReleased;
            }
            MoveControl = null;
            TargetControl = null;
            TargetCanvas = null;
        }
    }
}
