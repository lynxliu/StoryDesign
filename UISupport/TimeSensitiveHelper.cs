using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UISupport;
using Windows.UI;

namespace UISupport
{
    public class TimeSensitiveHelper
    {
        public static Func<DateTime> GetBeginTime { get; set; }
        public static Func<TimeSpan> GetTotleTimeSpan { get; set; }

        static double _MaxWidth=5;
        public static double MaxWidth
        {
            get { return _MaxWidth; }
            set { _MaxWidth = value; }
        }
        static double _MaxFontSize = 22;
        public static double MaxFontSize
        {
            get { return _MaxFontSize; }
            set { _MaxFontSize = value; }
        }
        static double _MinFontSize = 12;
        public static double MinFontSize
        {
            get { return _MinFontSize; }
            set { _MinFontSize = value; }
        }

        public static double GetWidth(TimeSpan span)
        {
            if (GetTotleTimeSpan == null)
                return 0;
            var d = span.TotalMilliseconds / GetTotleTimeSpan().TotalMilliseconds;
            return ((int)d+1) * MaxWidth;
        }
        public static double GetFontSize(TimeSpan span)
        {
            if (GetTotleTimeSpan == null)
                return MinFontSize;
            var d = span.TotalMilliseconds / GetTotleTimeSpan().TotalMilliseconds;
            return d * (MaxFontSize-MinFontSize)+MinFontSize;
        }

        public static Color GetColor(DateTime startTime)
        {
            if (GetTotleTimeSpan == null|| GetBeginTime==null)
                return Colors.Black;
            var p=(startTime- GetBeginTime()).TotalMilliseconds/ GetTotleTimeSpan().TotalMilliseconds;
            LColor lc = new LColor();
            lc.setColorByHSB(p*360, 1, 1);

            return lc.getColor();

        }
    }
}
