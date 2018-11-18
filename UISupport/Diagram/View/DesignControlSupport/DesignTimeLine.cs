using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UISupport.Diagram.View.DesignControlSupport
{
    public class DesignTimeLine
    {
        double _Top = -1;
        public double Top
        {
            get { return _Top; }
            set { _Top = value; }
        }

        public double MaxHeight { get; set; }

        private Canvas TargetCanvas = null;
        public DesignTimeLine(Canvas target)
        {
            TargetCanvas = target;

        }
        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }

        public void SetPosition(DateTime begin,DateTime end,FrameworkElement target)
        {
            if (end <= BeginTime||begin>=EndTime)
            {
                if (TargetCanvas.Children.Contains(target))
                    TargetCanvas.Children.Remove(target);
                return;
            }
            var s = GetPoint(begin);
            var e = GetPoint(end);
            if (s == null)
                s = 0;
            if (e == null)
                e = TargetCanvas.ActualWidth;
            Canvas.SetLeft(target, s.Value);
            target.Width = e.Value - s.Value;

            if (Top >= 0)
                Canvas.SetTop(target, Top);
            if (MaxHeight > 0 && target.Height > MaxHeight)
                target.Height = MaxHeight;
            if (!TargetCanvas.Children.Contains(target))
                TargetCanvas.Children.Add(target);
        }

        public double? GetPoint(DateTime time)
        {
            if (time <= BeginTime || time >= EndTime)
                return null;
            return ((time - BeginTime).TotalMilliseconds / (EndTime - BeginTime).TotalMilliseconds) * TargetCanvas.ActualWidth;
        }
    }
}
