using StoryDesignInterface.Diagram;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using UISupport.Diagram.Controls;

namespace StoryDesign.ViewModel
{
    class FateDiagramHelper
    {
        public Canvas TargetCanvas { get; set; }
        double _SeparateWidth = 75;
        public double SeparateWidth { get { return _SeparateWidth; } set { _SeparateWidth = value; } }

        double _LeftMargin = 30;
        double _RightMargin = 30;
        double _BottomMargin = 30;
        public double LeftMargin { get { return _LeftMargin; } set { _LeftMargin = value; } }
        public double RightMargin { get { return _RightMargin; } set { _RightMargin = value; } }
        public double BottomMargin { get { return _BottomMargin; } set { _BottomMargin = value; } }

        public DateTime BeginTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Continue { get {return EndTime - BeginTime; } }

        double minWidth = 75;
        ObservableCollection<ITimeSeparate> _TimeSeparateList = new ObservableCollection<ITimeSeparate>();
        public ObservableCollection<ITimeSeparate> TimeSeparateList { get { return _TimeSeparateList; } }
        ObservableCollection<ITrack> _TrackList = new ObservableCollection<ITrack>();
        public ObservableCollection<ITrack> TrackList { get { return _TrackList; } }

        public void SetWidth()
        {
            var w = GetWidth(minWidth);
            if ((TargetCanvas.ActualWidth) <= w)
                TargetCanvas.Width = w;
        }
        double GetWidth(double minWidth)
        {
            var t = GetMinContinue();
            if (t == null)
                return TargetCanvas.ActualWidth;
            var minSec = t.Value.TotalSeconds;
            var tSec = Continue.TotalSeconds;
            var minW = GetTimeWidth() * minSec / tSec;
            if (minW > minWidth)
                return TargetCanvas.ActualWidth;

            return GetTimeWidth() * minWidth / minW + LeftMargin + RightMargin;
        }
        TimeSpan? GetMinContinue( )
        {
            TimeSpan? timespan = null;
            foreach(var t in TrackList)
            {
                foreach (var e in t.EntityList)
                {
                    if (timespan == null || timespan.Value > (e.EndTime - e.BeginTime))
                        timespan = (e.EndTime - e.BeginTime);
                }
            }

            return timespan;
        }
        public void DrawSeparate()
        {
            foreach (var s in TimeSeparateList)
            {
                var b = new Border()
                {
                    CornerRadius = new CornerRadius(5),
                    BorderBrush = new SolidColorBrush(Colors.Blue),
                    BorderThickness = new Thickness(3),
                    Width = SeparateWidth
                };
                var mark = new TextBlock() { Text = s.Description };
                b.Child = mark;
                var x = GetPosition(s.BeginTime);
                Canvas.SetLeft(b, x);
                TargetCanvas.Children.Add(b);

            }
        }
        void DrawTimeMark(DateTime begin, DateTime end)
        {
            var x0 = GetPosition(begin);
            var x1 = GetPosition(end);
            var markNum = Convert.ToInt32(x1 - x0) / 100;
            if (markNum < 2)
            {
                Line markLine = new Line() { X1 = x0, Y1 = TargetCanvas.ActualHeight - BottomMargin, X2 = x0, Y2 = TargetCanvas.ActualHeight - BottomMargin + 5 };
                TargetCanvas.Children.Add(markLine);
                var mark = new TextBlock() { Text = begin.ToString() };
                Canvas.SetLeft(mark, x0);
                Canvas.SetTop(mark, TargetCanvas.ActualHeight - BottomMargin);
                TargetCanvas.Children.Add(mark);

                markLine = new Line() { X1 = x1, Y1 = TargetCanvas.ActualHeight - BottomMargin, X2 = x1, Y2 = TargetCanvas.ActualHeight - BottomMargin + 5 };
                TargetCanvas.Children.Add(markLine);
                mark = new TextBlock() { Text = end.ToString() };
                Canvas.SetLeft(mark, x1);
                Canvas.SetTop(mark, TargetCanvas.ActualHeight - BottomMargin);
                TargetCanvas.Children.Add(mark);

                return;
            }
            var timeStep = TimeSpan.FromSeconds(Continue.TotalSeconds / markNum);

            for (int i = 1; i < markNum; i++)
            {
                var t = BeginTime + TimeSpan.FromSeconds(timeStep.TotalSeconds * i);
                var x = GetPosition(t);
                Line markLine = new Line() { X1 = x, Y1 = TargetCanvas.ActualHeight - BottomMargin, X2 = x, Y2 = TargetCanvas.ActualHeight - BottomMargin + 5 };
                TargetCanvas.Children.Add(markLine);
                var mark = new TextBlock() { Text = t.ToString() };
                Canvas.SetLeft(mark, x);
                Canvas.SetTop(mark, TargetCanvas.ActualHeight - BottomMargin);
                TargetCanvas.Children.Add(mark);
            }
        }
        List<Tuple<DateTime, DateTime>> GetTimeLine()
        {
            List<Tuple<DateTime, DateTime>> l = new List<Tuple<DateTime, DateTime>>();
            if (TimeSeparateList.Count == 0)
            {
                l.Add(new Tuple<DateTime, DateTime>(BeginTime, EndTime));
                return l;
            }
            var sl = TimeSeparateList.Where(v => v.IsEnable).OrderBy(v => v.BeginTime).ToList();
            l.Add(new Tuple<DateTime, DateTime>(BeginTime, sl[0].BeginTime));
            for (int i = 1; i < sl.Count; i++)
            {
                l.Add(new Tuple<DateTime, DateTime>(sl[i - 1].EndTime, sl[i].BeginTime));
            }
            l.Add(new Tuple<DateTime, DateTime>(sl[sl.Count - 1].EndTime, EndTime));
            return l;
        }
        public void DrawTimeLine()
        {
            var l = GetTimeLine();
            for (int i = 0; i < l.Count - 2; i++)
            {
                DrawTimeLine(l[i].Item1, l[2].Item2, false);
            }
            DrawTimeLine(l.LastOrDefault().Item1, l.LastOrDefault().Item2, true);
        }
        void DrawTimeLine(DateTime begin, DateTime end, bool isArrow)
        {
            if (isArrow)
            {
                var timeLine = new SimpleArrow();
                timeLine.StartPointX = GetPosition(begin);
                timeLine.StartPointY = TargetCanvas.ActualHeight - BottomMargin;
                timeLine.EndPointX = GetPosition(end);
                timeLine.EndPointY = TargetCanvas.ActualHeight - BottomMargin;

                timeLine.StrokeThickness = 3;
                timeLine.Background = new SolidColorBrush() { Color = Colors.Black };
                TargetCanvas.Children.Add(timeLine);
            }
            else
            {
                var timeLine = new Line();
                timeLine.X1 = GetPosition(begin); ;
                timeLine.Y1 = TargetCanvas.ActualHeight - BottomMargin;
                timeLine.X2 = GetPosition(end);
                timeLine.Y2 = TargetCanvas.ActualHeight - BottomMargin;

                timeLine.StrokeThickness = 3;
                timeLine.Stroke = new SolidColorBrush() { Color = Colors.Black };
                TargetCanvas.Children.Add(timeLine);
            }
            DrawTimeMark(begin, end);

        }
        void DrawEntity(FateEntity entity, double y, double h)
        {
            var x1 = GetPosition(entity.BeginTime);
            var x2 = GetPosition(entity.EndTime);
            var b = new Border()
            {
                BorderThickness = new Windows.UI.Xaml.Thickness() { Left = 1, Right = 1, Top = 2, Bottom = 2 },
                Background = new SolidColorBrush() { Color = Colors.LightGray, Opacity = 0.3 },
                Height = h,
                Width = x2 - x1,
                VerticalAlignment = VerticalAlignment.Center
            };

            Canvas.SetLeft(b, x1);
            Canvas.SetTop(b, y);
            TargetCanvas.Children.Add(b);
        }
        public double DrawTrack(ITrack track, double totleHeight, double y)
        {
            var d = TargetCanvas.ActualHeight * track.Width / totleHeight;
            var textName = new TextBlock() { Text = track.Name, Width = 30, Height = d, VerticalAlignment = VerticalAlignment.Center };
            Canvas.SetTop(textName, y);
            TargetCanvas.Children.Add(textName);
            track.EntityList.ForEach(v =>
            {
                DrawEntity(v, y, d);
            });

            return y + d;
        }

        int GetSeparateNum(DateTime time)
        {
            var n = 0;
            foreach (var v in TimeSeparateList.OrderBy(v => v.BeginTime))
            {
                if (time >= v.EndTime)
                    n++;
            }
            return n;
        }
        int GetSeparateNum(double point)
        {
            var n = 0;
            foreach (var v in TimeSeparateList.OrderBy(v => v.BeginTime))
            {
                var ex = GetPosition(v.EndTime);
                if (point >= ex)
                    n++;
            }
            return n;
        }
        DateTime? GetRecentTimeLineBegin(int n)
        {
            if (n > TimeSeparateList.Count) return null;
            if (n == 0) return BeginTime;
            return TimeSeparateList.OrderBy(v => v.BeginTime).ToList()[n - 1].EndTime;
        }
        double GetWidthPerSecond()
        {
            var tw = (GetTimeWidth() - SeparateWidth * TimeSeparateList.Count);
            var ts = Continue.TotalSeconds - TimeSeparateList.Sum(v => v.ContinueTime.TotalSeconds);
            return tw / ts;
        }
        double GetTimeWidth()
        {
            return TargetCanvas.ActualWidth - LeftMargin - RightMargin;
        }
        double GetPosition(DateTime time)
        {
            if (BeginTime >= EndTime) return -1;
            var n = GetSeparateNum(time);
            var rt = GetRecentTimeLineBegin(n);

            var ts = (time - BeginTime).TotalSeconds - TimeSeparateList.Take(n).Sum(v => v.ContinueTime.TotalSeconds);

            return ts * GetWidthPerSecond() + n * SeparateWidth + LeftMargin;
        }
        DateTime GetTime(double x)
        {

            int n = GetSeparateNum(x);
            var ts = (x - LeftMargin) / GetWidthPerSecond();

            return BeginTime + TimeSpan.FromSeconds(ts) + TimeSpan.FromSeconds(TimeSeparateList.Take(n).Sum(v => v.ContinueTime.TotalSeconds));
        }
    }
}
