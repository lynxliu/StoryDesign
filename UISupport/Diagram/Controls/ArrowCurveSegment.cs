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
    public class ArrowCurveSegment : ContentControl
    {
        Brush activeBrush = new SolidColorBrush(Colors.OrangeRed);
        Brush deActiveBrush;
        public void ChangeActive(bool isActive)
        {
            if (isActive)
            {
                if (deActiveBrush == null)
                    deActiveBrush = Background;
                Background = activeBrush;
            }
            else
            {
                Background = deActiveBrush;
            }

        }
        #region 常量
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
        public PathFigure LinePathFigureElement;
        /// <summary>
        /// 线条路径
        /// </summary>
        public QuadraticBezierSegment lineElement;

        /// <summary>
        /// 箭头路径
        /// </summary>
        public PolyLineSegment arrowElement;
        public PathFigure arrowStartElement;
        public TextBox textBoxElement = new TextBox();

        #endregion

        #region 依赖属性
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
           typeof(ArrowCurveSegment),
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
           typeof(ArrowCurveSegment),
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
           typeof(ArrowCurveSegment),
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
           typeof(ArrowCurveSegment),
           new PropertyMetadata(0d, new PropertyChangedCallback(ReSize)));

        /// <summary>
        /// 半径
        /// </summary>
        //public double Radius
        //{
        //    get
        //    {
        //        return (double)this.GetValue(RadiusProperty);
        //    }
        //    set
        //    {
        //        this.SetValue(RadiusProperty, value);
        //    }

        //}
        public double RadiusRadio//半径是长度的倍数
        {
            get
            {
                return (double)this.GetValue(RadiusRadioProperty) ;
            }
            set
            {
                this.SetValue(RadiusRadioProperty, value);
            }

        }
        public static readonly DependencyProperty RadiusRadioProperty = DependencyProperty.Register(
           "RadiusRadio",
           typeof(double),
           typeof(ArrowCurveSegment),
           new PropertyMetadata(1.5, new PropertyChangedCallback(ReSize)));

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
           typeof(ArrowCurveSegment),
           new PropertyMetadata(1.0, null));

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
           typeof(ArrowCurveSegment),
           new PropertyMetadata(1, null));

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
           typeof(ArrowCurveSegment),
           new PropertyMetadata(new Size(4, 8), new PropertyChangedCallback(ReSize)));




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
           typeof(ArrowCurveSegment),
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
           typeof(ArrowCurveSegment),
           new PropertyMetadata(Visibility.Visible, null));

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
           typeof(ArrowCurveSegment),
           new PropertyMetadata(SweepDirection.Clockwise, new PropertyChangedCallback(ReSize)));

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
           typeof(ArrowCurveSegment),
           new PropertyMetadata(null, null));

        public double Length
        {
            get
            {
                return Math.Sqrt((EndPointX - StartPointX) * (EndPointX - StartPointX) +
                    (EndPointY - StartPointY) * (EndPointY - StartPointY));
            }
        }

        #endregion

        #region 生命周期
        public ArrowCurveSegment()
        {
            this.DefaultStyleKey = typeof(ArrowCurveSegment);
            this.LayoutUpdated += ArrowCurveSegment_LayoutUpdated1;
        }

        private void ArrowCurveSegment_LayoutUpdated1(object sender, object e)
        {
            //this.SetContentPosition(this.CenterPoint);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.arrowElement = (PolyLineSegment)this.GetTemplateChild(ArrowCurveSegment.ArrowElementName);
            this.lineElement = (QuadraticBezierSegment)this.GetTemplateChild(ArrowCurveSegment.LineElementName);
            this.arrowStartElement = (PathFigure)this.GetTemplateChild(ArrowCurveSegment.ArrowStartElementName);
            this.LinePathFigureElement = (PathFigure)this.GetTemplateChild(ArrowCurveSegment.LinePathFigureElementName);
            this.textBoxElement = (TextBox)this.GetTemplateChild(ArrowCurveSegment.TextElementName);
            this.SetPosition();

            this.PointerPressed += ArrowLine_PointerPressed;
        }
        private void ArrowLine_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            ChangeActive(true);
            if (DesignManager.SetCurrent != null)
            {
                if (DataContext is DesignLink)
                    DesignManager.SetCurrent((DataContext as DesignLink).TargetObject);
                else
                    DesignManager.SetCurrent(DataContext);
            }
            e.Handled = true;
        }
        #endregion

        /// <summary>
        /// 重绘
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void ReSize(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ArrowCurveSegment ArrowCurveSegment = (ArrowCurveSegment)d;
            if (ArrowCurveSegment.lineElement != null)
            {
                ArrowCurveSegment.SetPosition();
            }
        }



        void SetLinePosition()
        {
            lineElement.Point1 =
            new Point()
            {
                X = (this.StartPointX + this.EndPointX) / 2,
                Y = (this.StartPointY + this.EndPointY) / 2
            };

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
                this.arrowElement.Points.Add(new Point(EndPointX, EndPointY));
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
        /// <summary>
        /// 设置位置
        /// </summary>
        public void SetPosition()
        {
            #region 弧线
            lineElement.Point2 =new Point(EndPointX,EndPointY);
            LinePathFigureElement.StartPoint =new Point(StartPointX,StartPointY);

            #endregion

            #region 箭头
            Point roundCenter = new Point();
            Point textCenterPoint;
            double k1;
            //求圆心
            try
            {
                roundCenter = MathExtension.GetBezierCenter(LinePathFigureElement.StartPoint, lineElement.Point2, RadiusRadio*Length, this.LineSweepDirection);
                var cp = new Point()
                {
                    X = (StartPointX + EndPointX) / 2,
                    Y = (StartPointY + EndPointY) / 2,
                };

                textCenterPoint = new Point()
                {
                    X = (cp.X + roundCenter.X) / 2,
                    Y = (cp.Y + roundCenter.Y) / 2,
                };
                //圆心与箭头点连线的斜率
                k1 = (roundCenter.Y - this.EndPointY) / (roundCenter.X - this.EndPointX);

            }
            catch (Exception ex)
            {
                if (ex is RadiusException)
                {
                    k1 = (-1) * (this.StartPointX - this.EndPointX) / (this.StartPointY - this.EndPointY);
                    textCenterPoint = roundCenter = new Point()
                    {
                        X = (this.StartPointX + this.EndPointX) / 2,
                        Y = (this.StartPointY + this.EndPointY) / 2
                    };

                    SetLinePosition();
                    textBoxElement.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));

                    Canvas.SetLeft(textBoxElement, textCenterPoint.X - textBoxElement.DesiredSize.Width / 2);
                    Canvas.SetTop(textBoxElement, textCenterPoint.Y - textBoxElement.DesiredSize.Height / 2);
                    return;
                }
                else
                    return;
            }
            this.lineElement.Point1 = roundCenter;
            Point centerPoint1, centerPoint2, endPoint11, endPoint12, endPoint21, endPoint22;


            //切线斜率
            double k2 = 0;

            {

                k2 = k1;

                double xOffset = this.ArrowSize.Height / (Math.Sqrt(1 + k2 * k2));

                double xTemp, yTemp;

                #region 第一个点
                xTemp = this.EndPointX + xOffset;
                yTemp = k2 * (xTemp - this.EndPointX) + this.EndPointY;
                centerPoint1 = new Point(Math.Round(xTemp, 4), Math.Round(yTemp, 4));
                #endregion

                #region 第二个点
                xTemp = this.EndPointX - xOffset;
                yTemp = k2 * (xTemp - this.EndPointX) + this.EndPointY;
                centerPoint2 = new Point(Math.Round(xTemp, 4), Math.Round(yTemp, 4));
                #endregion

                k2 = (-1) / k1;

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

                #region 永远找和圆心距离近的那组点

                {
                    double d1 = (endPoint11.X - roundCenter.X) * (endPoint11.X - roundCenter.X) + (endPoint11.Y - roundCenter.Y) * (endPoint11.Y - roundCenter.Y);
                    double d2 = (endPoint21.X - roundCenter.X) * (endPoint21.X - roundCenter.X) + (endPoint21.Y - roundCenter.Y) * (endPoint21.Y - roundCenter.Y);


                    //if (this.LineSweepDirection== SweepDirection.Clockwise)
                    if (d1 < d2)
                    {
                        this.arrowStartElement.StartPoint = endPoint11;
                        this.arrowElement.Points.Clear();
                        this.arrowElement.Points.Add(lineElement.Point2);
                        this.arrowElement.Points.Add(endPoint12);
                    }
                    else
                    {
                        this.arrowStartElement.StartPoint = endPoint21;
                        this.arrowElement.Points.Clear();
                        this.arrowElement.Points.Add(lineElement.Point2);
                        this.arrowElement.Points.Add(endPoint22);
                    }
                }
                #endregion
            }
            #endregion

            textBoxElement.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));

            Canvas.SetLeft(textBoxElement, textCenterPoint.X - textBoxElement.DesiredSize.Width / 2);
            Canvas.SetTop(textBoxElement, textCenterPoint.Y - textBoxElement.DesiredSize.Height / 2);

        }

    }

}
