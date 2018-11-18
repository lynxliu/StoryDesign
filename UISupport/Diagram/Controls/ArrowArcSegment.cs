using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace UISupport.Diagram.Controls
{
    public class ArrowArcSegment : ContentControl
    {
        #region 常量
        internal const string LineElementName = "LineElement";
        internal const string RootElementName = "RootElement";
        internal const string ArrowElementName = "ArrowElement";
        internal const string CenterGridElementName = "CenterGridElement";
        internal const string ArrowStartElementName = "ArrowStartElement";
        internal const string LinePathFigureElementName = "LinePathFigureElement";
        internal const string ContentPresenterElementName = "ContentPresenterElement";
        internal const string ContentPresenterCanvasElementName = "ContentPresenterCanvasElement";
        #endregion

        #region 变量

        /// <summary>
        /// 开始点
        /// </summary>
        public PathFigure LinePathFigureElement;
        /// <summary>
        /// 线条路径
        /// </summary>
        public ArcSegment lineElement;

        /// <summary>
        /// 箭头路径
        /// </summary>
        public PolyLineSegment arrowElement;
        public PathFigure arrowStartElement;

        private ContentPresenter contentPresenterElement;
        #endregion

        #region 属性

        /// <summary>
        /// 是否直线
        /// </summary>
        public bool IsBeeline
        {
            get
            {
                if (null == this.lineElement)
                {
                    return false;
                }
                else
                {
                    if (this.lineElement.Size == new Size(0, 1))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Canvas RootElement { get; private set; }

        /// <summary>
        /// 中心点
        /// </summary>
        private Point CenterPoint
        {
            get
            {
                return this._CenterPoint;
            }
            set
            {
                this._CenterPoint = value;
            }
        }
        Point _CenterPoint = new Point();

        private Grid CenterGridElement { get; set; }
        private Canvas ContentPresenterCanvasElement { get; set; }
        #endregion

        #region 依赖属性
        /// <summary>
        /// 开始点
        /// </summary>
        public Point StartPoint
        {
            get
            {
                return (Point)this.GetValue(StartPointProperty);
            }
            set
            {
                this.SetValue(StartPointProperty, value);
            }
        }
        public static readonly DependencyProperty StartPointProperty = DependencyProperty.Register(
           "StartPoint",
           typeof(Point),
           typeof(ArrowArcSegment),
           new PropertyMetadata(new Point(10, 10), new PropertyChangedCallback(ReSize)));

        /// <summary>
        /// 终点
        /// </summary>
        public Point EndPoint
        {
            get
            {
                return (Point)this.GetValue(EndPointProperty);
            }
            set
            {
                this.SetValue(EndPointProperty, value);
            }
        }
        public static readonly DependencyProperty EndPointProperty = DependencyProperty.Register(
           "EndPoint",
           typeof(Point),
           typeof(ArrowArcSegment),
           new PropertyMetadata(new Point(100, 10), new PropertyChangedCallback(ReSize)));

        /// <summary>
        /// 半径
        /// </summary>
        public double Radius
        {
            get
            {
                return (double)this.GetValue(RadiusProperty);
            }
            set
            {
                this.SetValue(RadiusProperty, value);
            }

        }
        public static readonly DependencyProperty RadiusProperty = DependencyProperty.Register(
           "Radius",
           typeof(double),
           typeof(ArrowArcSegment),
           new PropertyMetadata(120.0, new PropertyChangedCallback(ReSize)));

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
           typeof(ArrowArcSegment),
           new PropertyMetadata(2.0, null));

        /// <summary>
        /// 箭头宽度
        /// </summary>
        public double LineThickness
        {
            get
            {
                return (double)this.GetValue(LineThicknessProperty);
            }
            set
            {
                this.SetValue(LineThicknessProperty, value);
            }

        }
        public static readonly DependencyProperty LineThicknessProperty = DependencyProperty.Register(
           "LineThickness",
           typeof(double),
           typeof(ArrowArcSegment),
           new PropertyMetadata(2.0, null));

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
           typeof(ArrowArcSegment),
           new PropertyMetadata(new Size(4, 8), new PropertyChangedCallback(ReSize)));

        /// <summary>
        /// 绘制方向是顺时针还是逆时针.
        /// </summary>
        public SweepDirection LineSweepDirection
        {
            get
            {
                return (SweepDirection)this.GetValue(LineSweepDirectionProperty);
            }
            set
            {
                this.SetValue(LineSweepDirectionProperty, value);
            }
        }
        public static readonly DependencyProperty LineSweepDirectionProperty = DependencyProperty.Register(
           "SweepDirection",
           typeof(SweepDirection),
           typeof(ArrowArcSegment),
           new PropertyMetadata(SweepDirection.Clockwise, new PropertyChangedCallback(ReSize)));

        /// <summary>
        /// 是否是大弧
        /// </summary>
        public bool IsLargeArc
        {
            get
            {
                return (bool)this.GetValue(IsLargeArcProperty);
            }
            set
            {
                this.SetValue(IsLargeArcProperty, value);
            }

        }
        public static readonly DependencyProperty IsLargeArcProperty = DependencyProperty.Register(
           "IsLargeArc",
           typeof(bool),
           typeof(ArrowArcSegment),
           new PropertyMetadata(false, new PropertyChangedCallback(ReSize)));

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
           typeof(ArrowArcSegment),
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
           typeof(ArrowArcSegment),
           new PropertyMetadata(Visibility.Visible, null));
        #endregion

        #region 生命周期
        public ArrowArcSegment()
        {
            this.DefaultStyleKey = typeof(ArrowArcSegment);
            this.LayoutUpdated += ArrowArcSegment_LayoutUpdated1;
        }

        private void ArrowArcSegment_LayoutUpdated1(object sender, object e)
        {
            this.SetContentPosition(this.CenterPoint);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.arrowElement = (PolyLineSegment)this.GetTemplateChild(ArrowArcSegment.ArrowElementName);
            this.lineElement = (ArcSegment)this.GetTemplateChild(ArrowArcSegment.LineElementName);
            this.arrowStartElement = (PathFigure)this.GetTemplateChild(ArrowArcSegment.ArrowStartElementName);
            this.LinePathFigureElement = (PathFigure)this.GetTemplateChild(ArrowArcSegment.LinePathFigureElementName);
            this.contentPresenterElement = (ContentPresenter)this.GetTemplateChild(ArrowArcSegment.ContentPresenterElementName);
            this.CenterGridElement = (Grid)this.GetTemplateChild(ArrowArcSegment.CenterGridElementName);
            this.ContentPresenterCanvasElement = (Canvas)this.GetTemplateChild(ArrowArcSegment.ContentPresenterCanvasElementName);
            this.RootElement = (Canvas)this.GetTemplateChild(ArrowArcSegment.RootElementName);

            this.SetPosition();
        }
        #endregion

        /// <summary>
        /// 重绘
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ReSize(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ArrowArcSegment arrowArcSegment = (ArrowArcSegment)d;
            if (arrowArcSegment.lineElement != null)
            {
                arrowArcSegment.SetPosition();
            }
        }

        /// <summary>
        /// 设置位置
        /// </summary>
        public void SetPosition()
        {
            #region 弧线
            this.lineElement.Point = this.EndPoint;
            this.LinePathFigureElement.StartPoint = this.StartPoint;
            this.lineElement.IsLargeArc = this.IsLargeArc;
            this.lineElement.SweepDirection = this.LineSweepDirection;
            this.lineElement.Size = new Size(this.Radius, this.Radius);
            #endregion

            #region 箭头
            Point roundCenter = new Point();
            double k1;
            //求圆心
            try
            {
                roundCenter = MathExtension.GetRoundCenter(this.StartPoint, this.EndPoint, this.Radius, this.LineSweepDirection, this.IsLargeArc);

                //圆心与箭头点连线的斜率
                k1 = (roundCenter.Y - this.EndPoint.Y) / (roundCenter.X - this.EndPoint.X);
            }
            catch (Exception ex)
            {
                if (ex is RadiusException)
                {
                    k1 = (-1) * (this.StartPoint.X - this.EndPoint.X) / (this.StartPoint.Y - this.EndPoint.Y);
                    this.lineElement.Size = new Size(0, 1);
                }
                else
                    return;
            }

            Point centerPoint1, centerPoint2, endPoint11, endPoint12, endPoint21, endPoint22;


            //切线斜率
            double k2 = 0;

            if (double.IsInfinity(k1)) //与Y轴平行
            {
                //中间点
                this._CenterPoint.X = (this.StartPoint.X + this.EndPoint.X) / 2;
                this._CenterPoint.Y = (this.StartPoint.Y + this.EndPoint.Y) / 2;

                //如果是直线,直线与X轴平行
                if (this.IsBeeline)
                {
                    if (this.StartPoint.X < this.EndPoint.X)
                    {
                        this.arrowStartElement.StartPoint = new Point(this.EndPoint.X - this.ArrowSize.Height, this.EndPoint.Y - this.ArrowSize.Width);
                        this.arrowElement.Points.Clear();
                        this.arrowElement.Points.Add(this.EndPoint);
                        this.arrowElement.Points.Add(new Point(this.EndPoint.X - this.ArrowSize.Height, this.EndPoint.Y + this.ArrowSize.Width));
                    }
                    else
                    {
                        this.arrowStartElement.StartPoint = new Point(this.EndPoint.X + this.ArrowSize.Height, this.EndPoint.Y - this.ArrowSize.Width);
                        this.arrowElement.Points.Clear();
                        this.arrowElement.Points.Add(this.EndPoint);
                        this.arrowElement.Points.Add(new Point(this.EndPoint.X + this.ArrowSize.Height, this.EndPoint.Y + this.ArrowSize.Width));
                    }
                }
            }
            else if (Math.Round(k1, 4) == 0)//与X轴平行
            {
                //中间点
                this._CenterPoint.X = (this.StartPoint.X + this.EndPoint.X) / 2;
                this._CenterPoint.Y = (this.StartPoint.Y + this.EndPoint.Y) / 2;

                //如果是直线
                if (this.IsBeeline)
                {
                    if (this.StartPoint.Y < this.EndPoint.Y)
                    {
                        this.arrowStartElement.StartPoint = new Point(this.EndPoint.X - this.ArrowSize.Width, this.EndPoint.Y - this.ArrowSize.Height);
                        this.arrowElement.Points.Clear();
                        this.arrowElement.Points.Add(this.EndPoint);
                        this.arrowElement.Points.Add(new Point(this.EndPoint.X + this.ArrowSize.Width, this.EndPoint.Y - this.ArrowSize.Height));
                    }
                    else
                    {
                        this.arrowStartElement.StartPoint = new Point(this.EndPoint.X - this.ArrowSize.Width, this.EndPoint.Y + this.ArrowSize.Height);
                        this.arrowElement.Points.Clear();
                        this.arrowElement.Points.Add(this.EndPoint);
                        this.arrowElement.Points.Add(new Point(this.EndPoint.X + this.ArrowSize.Width, this.EndPoint.Y + this.ArrowSize.Height));
                    }
                }
            }
            else
            {
                k2 = (-1) / k1;
                double xOffset = this.ArrowSize.Height / (Math.Sqrt(1 + k2 * k2));

                double xTemp, yTemp;

                #region 第一个点
                xTemp = this.EndPoint.X + xOffset;
                yTemp = k2 * (xTemp - this.EndPoint.X) + this.EndPoint.Y;
                centerPoint1 = new Point(Math.Round(xTemp, 4), Math.Round(yTemp, 4));
                #endregion

                #region 第二个点
                xTemp = this.EndPoint.X - xOffset;
                yTemp = k2 * (xTemp - this.EndPoint.X) + this.EndPoint.Y;
                centerPoint2 = new Point(Math.Round(xTemp, 4), Math.Round(yTemp, 4));
                #endregion


                k2 = k1;
                xOffset = this.ArrowSize.Width / (Math.Sqrt(1 + k2 * k2));

                #region 第一个点对应的两个箭头端点
                xTemp = centerPoint1.X + xOffset;
                yTemp = k2 * (xTemp - centerPoint1.X) + centerPoint1.Y;
                endPoint11 = new Point(Math.Round(xTemp, 4), Math.Round(yTemp, 4));

                xTemp = centerPoint1.X - xOffset;
                yTemp = k2 * (xTemp - centerPoint1.X) + centerPoint1.Y;
                endPoint12 = new Point(Math.Round(xTemp, 4), Math.Round(yTemp, 4));
                #endregion

                #region 第二个点对应的两个箭头端点
                xTemp = centerPoint2.X + xOffset;
                yTemp = k2 * (xTemp - centerPoint2.X) + centerPoint2.Y;
                endPoint21 = new Point(Math.Round(xTemp, 4), Math.Round(yTemp, 4));

                xTemp = centerPoint2.X - xOffset;
                yTemp = k2 * (xTemp - centerPoint2.X) + centerPoint2.Y;
                endPoint22 = new Point(Math.Round(xTemp, 4), Math.Round(yTemp, 4));
                #endregion

                //小弧
                if (!this.IsLargeArc)
                {
                    #region 小弧
                    //如果是直线,则比较点与起点的距离,取距离近的两点
                    if (this.IsBeeline)
                    {
                        double d1 = (endPoint11.X - this.StartPoint.X) * (endPoint11.X - this.StartPoint.X) + (endPoint11.Y - this.StartPoint.Y) * (endPoint11.Y - this.StartPoint.Y);
                        double d2 = (endPoint21.X - this.StartPoint.X) * (endPoint21.X - this.StartPoint.X) + (endPoint21.Y - this.StartPoint.Y) * (endPoint21.Y - this.StartPoint.Y);

                        if (d1 < d2)
                        {
                            this.arrowStartElement.StartPoint = endPoint11;
                            this.arrowElement.Points.Clear();
                            this.arrowElement.Points.Add(this.EndPoint);
                            this.arrowElement.Points.Add(endPoint12);
                        }
                        else
                        {
                            this.arrowStartElement.StartPoint = endPoint21;
                            this.arrowElement.Points.Clear();
                            this.arrowElement.Points.Add(this.EndPoint);
                            this.arrowElement.Points.Add(endPoint22);
                        }
                    }
                    else
                    {
                        //是否是同一边
                        if (MathExtension.GetPointLeftOrRight(roundCenter, this.EndPoint, this.StartPoint) == MathExtension.GetPointLeftOrRight(roundCenter, this.EndPoint, centerPoint1))
                        {
                            this.arrowStartElement.StartPoint = endPoint11;
                            this.arrowElement.Points.Clear();
                            this.arrowElement.Points.Add(this.EndPoint);
                            this.arrowElement.Points.Add(endPoint12);
                        }
                        else
                        {
                            this.arrowStartElement.StartPoint = endPoint21;
                            this.arrowElement.Points.Clear();
                            this.arrowElement.Points.Add(this.EndPoint);
                            this.arrowElement.Points.Add(endPoint22);
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 大弧
                    //如果是直线,则比较点与起点的距离,取距离近的两点
                    if (this.IsBeeline)
                    {
                        double d1 = (endPoint11.X - this.StartPoint.X) * (endPoint11.X - this.StartPoint.X) + (endPoint11.Y - this.StartPoint.Y) * (endPoint11.Y - this.StartPoint.Y);
                        double d2 = (endPoint21.X - this.StartPoint.X) * (endPoint21.X - this.StartPoint.X) + (endPoint21.Y - this.StartPoint.Y) * (endPoint21.Y - this.StartPoint.Y);

                        if (d1 < d2)
                        {
                            this.arrowStartElement.StartPoint = endPoint11;
                            this.arrowElement.Points.Clear();
                            this.arrowElement.Points.Add(this.EndPoint);
                            this.arrowElement.Points.Add(endPoint12);
                        }
                        else
                        {
                            this.arrowStartElement.StartPoint = endPoint21;
                            this.arrowElement.Points.Clear();
                            this.arrowElement.Points.Add(this.EndPoint);
                            this.arrowElement.Points.Add(endPoint22);
                        }
                    }
                    else
                    {
                        //是否是不同一边
                        if (MathExtension.GetPointLeftOrRight(roundCenter, this.EndPoint, this.StartPoint) == (-1) * MathExtension.GetPointLeftOrRight(roundCenter, this.EndPoint, centerPoint1))
                        {
                            this.arrowStartElement.StartPoint = endPoint11;
                            this.arrowElement.Points.Clear();
                            this.arrowElement.Points.Add(this.EndPoint);
                            this.arrowElement.Points.Add(endPoint12);
                        }
                        else
                        {
                            this.arrowStartElement.StartPoint = endPoint21;
                            this.arrowElement.Points.Clear();
                            this.arrowElement.Points.Add(this.EndPoint);
                            this.arrowElement.Points.Add(endPoint22);
                        }
                    }
                    #endregion
                }

                this.GetCenterPositen(roundCenter);
            }
            #endregion
        }

        /// <summary>
        /// 获取中心点
        /// </summary>
        public void GetCenterPositen(Point roundCenter)
        {
            //连线中点
            Point centerPointTemp = new Point();
            this.CenterPoint = new Point();
            centerPointTemp.X = (this.EndPoint.X + this.StartPoint.X) / 2;
            centerPointTemp.Y = (this.EndPoint.Y + this.StartPoint.Y) / 2;

            if (this.IsBeeline)
            {
                this._CenterPoint = centerPointTemp;
            }
            else
            {
                double d = Math.Sqrt((centerPointTemp.X - roundCenter.X) * (centerPointTemp.X - roundCenter.X) + (centerPointTemp.Y - roundCenter.Y) * (centerPointTemp.Y - roundCenter.Y));

                if (this.IsLargeArc)
                {
                    this._CenterPoint.X = ((this.Radius + d) * (roundCenter.X - centerPointTemp.X) + d * centerPointTemp.X) / d;
                    this._CenterPoint.Y = ((this.Radius + d) * (roundCenter.Y - centerPointTemp.Y) + d * centerPointTemp.Y) / d;
                }
                else
                {
                    this._CenterPoint.X = (this.Radius * (centerPointTemp.X - roundCenter.X) + d * roundCenter.X) / d;
                    this._CenterPoint.Y = (this.Radius * (centerPointTemp.Y - roundCenter.Y) + d * roundCenter.Y) / d;
                }
            }
        }

        /// <summary>
        /// 设置内容位置
        /// </summary>
        /// <param name="centerPoint"></param>
        public void SetContentPosition(Point centerPoint)
        {
            if (this.contentPresenterElement != null)
            {
                this.ContentPresenterCanvasElement.Height = this.contentPresenterElement.ActualHeight;
                this.ContentPresenterCanvasElement.Width = this.contentPresenterElement.ActualWidth;
            }
            Canvas.SetLeft(this.CenterGridElement, centerPoint.X);
            Canvas.SetTop(this.CenterGridElement, centerPoint.Y);

            //if (this.contentPresenterElement != null)
            //{
            //    Size size = new Size(0, 0);
            //    if (this.contentPresenterElement.Content != null)
            //    {
            //        FrameworkElement ui = (FrameworkElement)this.contentPresenterElement.Content;
            //        TraversingChildMaxSize(ui, ref size);
            //    }

            //    Canvas.SetLeft(this.contentPresenterElement, centerPoint.X - size.Width / 2);
            //    Canvas.SetTop(this.contentPresenterElement, centerPoint.Y - size.Height / 2);
            //}
        }

        /// <summary>
        /// 遍历子对象,取出最大长宽
        /// </summary>
        /// <param name="d">根节点</param>
        /// <param name="size">返回值</param>
        public void TraversingChildMaxSize(DependencyObject d, ref Size size)
        {
            int childCont = VisualTreeHelper.GetChildrenCount(d);
            if (childCont > 0)
            {
                for (int i = 0; i < childCont; i++)
                {
                    DependencyObject dTemp = VisualTreeHelper.GetChild(d, i);
                    TraversingChildMaxSize(dTemp, ref size);
                }
            }
            else
            {
                try
                {
                    FrameworkElement fe = (FrameworkElement)d;

                    if (fe.ActualHeight > size.Height)
                    {
                        size.Height = fe.ActualHeight;
                    }

                    if (fe.ActualWidth > size.Width)
                    {
                        size.Width = fe.ActualWidth;
                    }
                }
                catch { }
            }
        }
    }

}
