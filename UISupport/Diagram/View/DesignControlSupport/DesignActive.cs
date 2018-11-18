using DesignTool.Lib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace DesignTool.Lib.View.DesignControlSupport
{
    public class DesignActive:IDisposable//control design target selected or unselected
    {
        public static FrameworkElement CurrentSelectedControl { get; set; }

        int getTopIndex()
        {
            if (TargetCanvas == null) return 0;
            return (from UIElement c in TargetCanvas.Children select Canvas.GetZIndex(c)).Concat(new[] {0}).Max();
        }

        private Canvas TargetCanvas = null;
        public DesignActive(FrameworkElement target)
        {
            TargetControl = target;
            TargetControl.PointerPressed += TargetControl_PointerPressed;
        }

        void TargetControl_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            IsActive = !IsActive;
            //IsActive = true;
            DesignActive.CurrentSelectedControl = TargetControl;
            if (TargetControl == null || TargetControl.Parent == null || !(TargetControl.Parent is Canvas)) return;
            TargetCanvas = TargetControl.Parent as Canvas;
            Canvas.SetZIndex(TargetControl,getTopIndex()+1);
            if (DesignManager.SetCurrent != null)
            {
                if(TargetControl.DataContext!=null&&TargetControl.DataContext as DesignItem!=null)
                {
                    var ditem = TargetControl.DataContext as DesignItem;
                    DesignManager.SetCurrent(ditem.TargetObject);
                }
                else
                    DesignManager.SetCurrent(TargetControl.DataContext);
            }
            //e.Handled = true;
        }

        private List<FrameworkElement> _ActiveControlList = new List<FrameworkElement>();
        public List<FrameworkElement> ActiveControlList
        {
            get { return _ActiveControlList; }
        }

        public FrameworkElement TargetControl { get; set; }

        private bool _IsActive = false;
        public bool IsActive
        {
            get { return _IsActive; }
            set 
            {
                _IsActive = value;
                if (TargetControl != null)
                {
                    var dc = TargetControl.DataContext;
                    if (dc != null && dc is DesignItem)
                    {
                        (dc as DesignItem).IsActive = value;
                    }
                }
                if(!value)
                    ActiveControlList.ForEach(v =>
                        {
                            //v.IsEnabled = false;
                            v.Visibility = Visibility.Collapsed;
                        });
                else
                {
                    ActiveControlList.ForEach(v =>
                    {
                        //v.IsEnabled = true;
                        v.Visibility = Visibility.Visible;
                    });
                }
            }
        }

        public void Dispose()
        {
            if (TargetControl!=null)
                TargetControl.PointerPressed -= TargetControl_PointerPressed;

            TargetControl = null;
        }
    }
}
