using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace DesignTool.Lib.Model
{
    public sealed class Arrow : Line
    {
        #region Dependency Properties

        //public static readonly DependencyProperty X1Property = DependencyProperty.Register("X1", typeof(double), typeof(Arrow),new PropertyMetadata(0) );
        //public static readonly DependencyProperty Y1Property = DependencyProperty.Register("Y1", typeof(double), typeof(Arrow), new PropertyMetadata(0));
        //public static readonly DependencyProperty X2Property = DependencyProperty.Register("X2", typeof(double), typeof(Arrow), new PropertyMetadata(0));
        //public static readonly DependencyProperty Y2Property = DependencyProperty.Register("Y2", typeof(double), typeof(Arrow), new PropertyMetadata(0));
        //public static readonly DependencyProperty HeadWidthProperty = DependencyProperty.Register("HeadWidth", typeof(double), typeof(Arrow), new PropertyMetadata(0));
        //public static readonly DependencyProperty HeadHeightProperty = DependencyProperty.Register("HeadHeight", typeof(double), typeof(Arrow), new PropertyMetadata(0));

        #endregion

        //#region CLR Properties

        //public double X1
        //{
        //    get; set;
        //}

        //public double Y1
        //{
        //    get;set;
        //}

        //public double X2
        //{
        //    get; set;
        //}

        //public double Y2
        //{
        //    get; set;
        //}

        //public double HeadWidth
        //{
        //    get; set;
        //}

        //public double HeadHeight
        //{
        //    get; set;
        //}

        //#endregion

        #region Overrides

        //protected Geometry DefiningGeometry
        //{
        //    get
        //    {
        //        // Create a StreamGeometry for describing the shape
        //        StreamGeometry geometry = new StreamGeometry();
        //        geometry.FillRule = FillRule.EvenOdd;

        //        using (StreamGeometryContext context = geometry.Open())
        //        {
        //            InternalDrawArrowGeometry(context);
        //        }

        //        // Freeze the geometry for performance benefits
        //        geometry.Freeze();

        //        return geometry;
        //    }
        //}

        #endregion

        #region Privates

        //private void InternalDrawArrowGeometry(StreamGeometryContext context)
        //{
        //    double theta = Math.Atan2(Y1 - Y2, X1 - X2);
        //    double sint = Math.Sin(theta);
        //    double cost = Math.Cos(theta);

        //    Point pt1 = new Point(X1, this.Y1);
        //    Point pt2 = new Point(X2, this.Y2);

        //    Point pt3 = new Point(
        //        X2 + (HeadWidth * cost - HeadHeight * sint),
        //        Y2 + (HeadWidth * sint + HeadHeight * cost));

        //    Point pt4 = new Point(
        //        X2 + (HeadWidth * cost + HeadHeight * sint),
        //        Y2 - (HeadHeight * cost - HeadWidth * sint));

        //    context.BeginFigure(pt1, true, false);
        //    context.LineTo(pt2, true, true);
        //    context.LineTo(pt3, true, true);
        //    context.LineTo(pt2, true, true);
        //    context.LineTo(pt4, true, true);
        //}

        #endregion
    }
}
