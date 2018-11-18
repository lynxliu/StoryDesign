using DesignTool.Lib;
using DesignTool.Lib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace UISupport.Diagram.Controls
{
    public class ArrowLine : Control
    {
        Brush activeBrush = new SolidColorBrush(Colors.OrangeRed);
        Brush deActiveBrush ;
        public void ChangeActive(bool isActive)
        {
            if (isActive)
            {
                if(deActiveBrush==null)
                    deActiveBrush = Background;
                Background = activeBrush;
            }
            else
            {
                Background = deActiveBrush;
            }

        }


        #region 常量
        internal const string rootElementName = "RootElement";
        internal const string LineElementName = "LineElement";
        internal const string ArrowElementName = "ArrowElement";
        internal const string ArrowStartElementName = "ArrowStartElement";
        internal const string LinePathFigureElementName = "LinePathFigureElement";
        internal const string TextElementName = "TextElement";
        #endregion

        #region 变量

        /// <summary>
        /// 开始点
        /// </summary>
        public PathFigure LinePathFigureElement=new PathFigure();
        /// <summary>
        /// 线条路径
        /// </summary>
        public LineSegment lineElement=new LineSegment();

        /// <summary>
        /// 箭头路径
        /// </summary>
        public PolyLineSegment arrowElement=new PolyLineSegment();
        public PathFigure arrowStartElement=new PathFigure();

        public TextBox textBoxElement = new TextBox();
        #endregion

        #region 属性

        public Canvas RootElement
        {
            get;
            private set;
        }

        ///// <summary>
        ///// 开始点
        ///// </summary>
        //public Point StartPoint
        //{
        //    get
        //    {
        //        return (Point)this.GetValue(StartPointProperty);
        //    }
        //    set
        //    {
        //        this.SetValue(StartPointProperty, value);
        //    }
        //}
        //public static readonly DependencyProperty StartPointProperty = DependencyProperty.Register(
        //   "StartPoint",
        //   typeof(Point),
        //   typeof(ArrowLine),
        //   new PropertyMetadata(new Point(10, 10), new PropertyChangedCallback(ReSize)));

        ///// <summary>
        ///// 终点
        ///// </summary>
        //public Point EndPoint
        //{
        //    get
        //    {
        //        return (Point)this.GetValue(EndPointProperty);
        //    }
        //    set
        //    {
        //        this.SetValue(EndPointProperty, value);
        //    }
        //}
        //public static readonly DependencyProperty EndPointProperty = DependencyProperty.Register(
        //   "EndPoint",
        //   typeof(Point),
        //   typeof(ArrowLine),
        //   new PropertyMetadata(new Point(100, 10), new PropertyChangedCallback(ReSize)));

        public double StartPointX
        {
            get
            {
                return (double)this.GetValue(StartPointXProperty);
            }
            set
            {
                this.SetValue(StartPointXProperty, value);
            }
        }
        public static readonly DependencyProperty StartPointXProperty = DependencyProperty.Register(
           "StartPointX",
           typeof(double),
           typeof(ArrowLine),
           new PropertyMetadata(0d, new PropertyChangedCallback(ReSize)));

        public double StartPointY
        {
            get
            {
                return (double)this.GetValue(StartPointYProperty);
            }
            set
            {
                this.SetValue(StartPointYProperty, value);
            }
        }
        public static readonly DependencyProperty StartPointYProperty = DependencyProperty.Register(
           "StartPointY",
           typeof(double),
           typeof(ArrowLine),
           new PropertyMetadata(0d, new PropertyChangedCallback(ReSize)));

        public double EndPointX
        {
            get
            {
                return (double)this.GetValue(EndPointXProperty);
            }
            set
            {
                this.SetValue(EndPointXProperty, value);
            }
        }
        public static readonly DependencyProperty EndPointXProperty = DependencyProperty.Register(
           "EndPointX",
           typeof(double),
           typeof(ArrowLine),
           new PropertyMetadata(0d, new PropertyChangedCallback(ReSize)));
        public double EndPointY
        {
            get
            {
                return (double)this.GetValue(EndPointYProperty);
            }
            set
            {
                this.SetValue(EndPointYProperty, value);
            }
        }
        public static readonly DependencyProperty EndPointYProperty = DependencyProperty.Register(
           "EndPointY",
           typeof(double),
           typeof(ArrowLine),
           new PropertyMetadata(0d, new PropertyChangedCallback(ReSize)));

        public string Text
        {
            get
            {
                return (string)this.GetValue(TextProperty);
            }
            set
            {
                this.SetValue(TextProperty, value);
            }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
           "Text",
           typeof(string),
           typeof(ArrowLine),
           new PropertyMetadata(null, null));

        /// <summary>
        /// 宽度
        /// </summary>
        public double StrokeThickness
        {
            get
            {
                return (double)this.GetValue(StrokeThicknessProperty);
            }
            set
            {
                this.SetValue(StrokeThicknessProperty, value);
            }

        }
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(
           "StrokeThickness",
           typeof(double),
           typeof(ArrowLine),
           new PropertyMetadata(2.0, null));

        /// <summary>
        /// 箭头宽度
        /// </summary>
        public double ArrowLineThickness
        {
            get
            {
                return (double)this.GetValue(ArrowLineThicknessProperty);
            }
            set
            {
                this.SetValue(ArrowLineThicknessProperty, value);
            }
        }
        public static readonly DependencyProperty ArrowLineThicknessProperty = DependencyProperty.Register(
           "ArrowLineThickness",
           typeof(double),
           typeof(ArrowLine),
           new PropertyMetadata(3.0, null));

        /// <summary>
        /// 箭头的宽和高
        /// </summary>
        public Size ArrowSize
        {
            get
            {
                return (Size)this.GetValue(ArrowSizeProperty);
            }
            set
            {
                this.SetValue(ArrowSizeProperty, value);
            }

        }
        public static readonly DependencyProperty ArrowSizeProperty = DependencyProperty.Register(
           "ArrowSize",
           typeof(Size),
           typeof(ArrowLine),
           new PropertyMetadata(new Size(5, 10), new PropertyChangedCallback(ReSize)));

        /// <summary>
        /// 虚线
        /// </summary>
        public DoubleCollection StrokeDashArray
        {
            get
            {
                return (DoubleCollection)this.GetValue(StrokeDashArrayProperty);
            }
            set
            {
                this.SetValue(StrokeDashArrayProperty, value);
            }
        }
        public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register(
           "StrokeDashArray",
           typeof(DoubleCollection),
           typeof(ArrowLine),
           new PropertyMetadata(null));

        /// <summary>
        /// 箭头可见性
        /// </summary>
        public Visibility ArrowVisibility
        {
            get
            {
                return (Visibility)this.GetValue(ArrowVisibilityProperty);
            }
            set
            {
                this.SetValue(ArrowVisibilityProperty, value);
            }
        }
        public static readonly DependencyProperty ArrowVisibilityProperty = DependencyProperty.Register(
           "ArrowVisibility",
           typeof(Visibility),
           typeof(ArrowLine),
           new PropertyMetadata(Visibility.Visible, null));
        #endregion

        #region 生命周期
        public ArrowLine()
        {
            this.DefaultStyleKey = typeof(ArrowLine);

        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.arrowElement = (PolyLineSegment)this.GetTemplateChild(UISupport.Diagram.Controls.ArrowLine.ArrowElementName);
            this.lineElement = (LineSegment)this.GetTemplateChild(UISupport.Diagram.Controls.ArrowLine.LineElementName);
            this.arrowStartElement = (PathFigure)this.GetTemplateChild(UISupport.Diagram.Controls.ArrowLine.ArrowStartElementName);
            this.LinePathFigureElement = (PathFigure)this.GetTemplateChild(UISupport.Diagram.Controls.ArrowLine.LinePathFigureElementName);
            this.RootElement = (Canvas)this.GetTemplateChild(UISupport.Diagram.Controls.ArrowLine.rootElementName);
            this.textBoxElement = (TextBox)this.GetTemplateChild(UISupport.Diagram.Controls.ArrowLine.TextElementName);
            this.SetPosition();

            this.PointerPressed += ArrowLine_PointerPressed;
        }

        private void ArrowLine_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ChangeActive(true);
            if (DesignManager.SetCurrent != null)
            {
                if(DataContext is DesignLink)
                    DesignManager.SetCurrent((DataContext as DesignLink).TargetObject);
                else
                    DesignManager.SetCurrent(DataContext);
            }
            e.Handled = true;
        }
        #endregion

        private static void ReSize(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ArrowLine arrowArcSegment = (ArrowLine)d;
            if (arrowArcSegment.lineElement != null)
                arrowArcSegment.SetPosition();
        }

        public void SetPosition()
        {
            Canvas.SetLeft(textBoxElement, (StartPointX + EndPointX) / 2 - textBoxElement.ActualWidth / 2);
            Canvas.SetTop(textBoxElement, (StartPointY + EndPointY) / 2 - textBoxElement.ActualHeight / 2);

            #region 弧线
            this.lineElement.Point =new Point( this.EndPointX,EndPointY);
            this.LinePathFigureElement.StartPoint =new Point( this.StartPointX,StartPointY);
            #endregion

            #region 箭头
            //切线斜率
            double k;
            k = (this.StartPointY - this.EndPointY) / (this.StartPointX - this.EndPointX);
            Point centerPoint1, centerPoint2, endPoint11, endPoint12, endPoint21, endPoint22;

            if (double.IsInfinity(k)) //与Y轴平行
            {
                if (this.StartPointY < this.EndPointY)
                {
                    endPoint11 = new Point(-1 * this.ArrowSize.Width + this.EndPointX, this.EndPointY - this.ArrowSize.Height);
                    endPoint12 = new Point(this.ArrowSize.Width + this.EndPointX, this.EndPointY - this.ArrowSize.Height);
                }
                else
                {
                    endPoint11 = new Point(-1 * this.ArrowSize.Width + this.EndPointX, this.EndPointY + this.ArrowSize.Height);
                    endPoint12 = new Point(this.ArrowSize.Width + this.EndPointX, this.EndPointY + this.ArrowSize.Height);
                }

                this.arrowStartElement.StartPoint = endPoint11;
                this.arrowElement.Points.Clear();
                this.arrowElement.Points.Add(new Point(EndPointX,EndPointY));
                this.arrowElement.Points.Add(endPoint12);
            }
            else if (Math.Round(k, 4) == 0)//与X轴平行
            {

                if (this.StartPointX < this.EndPointX)
                {
                    endPoint11 = new Point(this.EndPointX - this.ArrowSize.Height, -1 * this.ArrowSize.Width + this.EndPointY);
                    endPoint12 = new Point(this.EndPointX - this.ArrowSize.Height, this.ArrowSize.Width + this.EndPointY);
                }
                else
                {
                    endPoint11 = new Point(this.EndPointX + this.ArrowSize.Height, -1 * this.ArrowSize.Width + this.EndPointY);
                    endPoint12 = new Point(this.EndPointX + this.ArrowSize.Height, this.ArrowSize.Width + this.EndPointY);
                }

                this.arrowStartElement.StartPoint = endPoint11;
                this.arrowElement.Points.Clear();
                this.arrowElement.Points.Add(new Point(EndPointX, EndPointY));
                this.arrowElement.Points.Add(endPoint12);
            }
            else
            {
                double xOffset = this.ArrowSize.Height / (Math.Sqrt(1 + k * k));
                double xTemp, yTemp;
                #region 第一个点
                xTemp = this.EndPointX + xOffset;
                yTemp = k * (xTemp - this.EndPointX) + this.EndPointY;
                centerPoint1 = new Point(Math.Round(xTemp, 4), Math.Round(yTemp, 4));
                #endregion

                #region 第二个点
                xTemp = this.EndPointX - xOffset;
                yTemp = k * (xTemp - this.EndPointX) + this.EndPointY;
                centerPoint2 = new Point(Math.Round(xTemp, 4), Math.Round(yTemp, 4));
                #endregion

                k = (-1) / k;
                xOffset = this.ArrowSize.Width / (Math.Sqrt(1 + k * k));

                //比较点与起点的距离,取距离近的两点
                double d1 = (centerPoint1.X - this.StartPointX) * (centerPoint1.X - this.StartPointX) + (centerPoint1.Y - this.StartPointY) * (centerPoint1.Y - this.StartPointY);
                double d2 = (centerPoint2.X - this.StartPointX) * (centerPoint2.X - this.StartPointX) + (centerPoint2.Y - this.StartPointY) * (centerPoint2.Y - this.StartPointY);

                if (d1 < d2)
                {
                    #region 第一个点对应的两个箭头端点
                    xTemp = centerPoint1.X + xOffset;
                    yTemp = k * (xTemp - centerPoint1.X) + centerPoint1.Y;
                    endPoint11 = new Point(Math.Round(xTemp, 4), Math.Round(yTemp, 4));

                    xTemp = centerPoint1.X - xOffset;
                    yTemp = k * (xTemp - centerPoint1.X) + centerPoint1.Y;
                    endPoint12 = new Point(Math.Round(xTemp, 4), Math.Round(yTemp, 4));
                    #endregion

                    this.arrowStartElement.StartPoint = endPoint11;
                    this.arrowElement.Points.Clear();
                    this.arrowElement.Points.Add(new Point(EndPointX, EndPointY));
                    this.arrowElement.Points.Add(endPoint12);
                }
                else
                {
                    #region 第二个点对应的两个箭头端点
                    xTemp = centerPoint2.X + xOffset;
                    yTemp = k * (xTemp - centerPoint2.X) + centerPoint2.Y;
                    endPoint21 = new Point(Math.Round(xTemp, 4), Math.Round(yTemp, 4));

                    xTemp = centerPoint2.X - xOffset;
                    yTemp = k * (xTemp - centerPoint2.X) + centerPoint2.Y;
                    endPoint22 = new Point(Math.Round(xTemp, 4), Math.Round(yTemp, 4));
                    #endregion

                    this.arrowStartElement.StartPoint = endPoint21;
                    this.arrowElement.Points.Clear();
                    this.arrowElement.Points.Add(new Point(EndPointX, EndPointY));
                    this.arrowElement.Points.Add(endPoint22);
                }
            }
            #endregion
        }
    }

}
