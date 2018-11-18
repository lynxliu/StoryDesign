using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media;

namespace UISupport.Diagram.Controls
{
    public class MathExtension
    {
        /// <summary>
        /// 获取一条线的方向
        /// </summary>
        /// <param name="startPoint">起点</param>
        /// <param name="endPoint">终点</param>
        /// <returns>区间</returns>
        public static ExtentDirectionEnum GetExtentDirection(Point startPoint, Point endPoint)
        {
            double xOffset = startPoint.X - endPoint.X;
            double yOffset = startPoint.Y - endPoint.Y;

            if (xOffset != 0 && yOffset != 0)
                if (xOffset > 0)
                    if (yOffset > 0)
                        return ExtentDirectionEnum.Quadrant1;
                    else
                        return ExtentDirectionEnum.Quadrant4;
                else
                    if (yOffset > 0)
                    return ExtentDirectionEnum.Quadrant2;
                else
                    return ExtentDirectionEnum.Quadrant3;
            else if (xOffset == 0 && yOffset != 0)
                if (yOffset > 0)
                    return ExtentDirectionEnum.YB;
                else
                    return ExtentDirectionEnum.YL;
            else if (xOffset != 0 && yOffset == 0)
                if (xOffset > 0)
                    return ExtentDirectionEnum.XB;
                else
                    return ExtentDirectionEnum.XL;
            else
                return ExtentDirectionEnum.Origin;
        }

        /// <summary>
        /// 根据弧的起点,终点,半径,绘制方向,返回圆心点
        /// </summary>
        /// <param name="startPoint">点1</param>
        /// <param name="endPoint">点2</param>
        /// <param name="radius">半径</param>
        /// <param name="sweepDirection">绘制方向</param>
        /// <returns>坐标</returns>
        /// <!--作者: 韦腾 时间:2008.12.16-->
        public static Point GetRoundCenter(Point startPoint, Point endPoint, double radius, SweepDirection sweepDirection, bool isLargeArc)
        {
            if (startPoint == endPoint)
                throw new Exception("起点与终点不能重合.");

            //弧连线的长
            double width = Math.Sqrt((startPoint.Y - endPoint.Y) * (startPoint.Y - endPoint.Y) + (startPoint.X - endPoint.X) * (startPoint.X - endPoint.X));

            if (width/2 > radius)
                throw new RadiusException("半径不能小于起点与终点的距离.");

            //中点,即交点
            Point crossingPoint = new Point((startPoint.X + endPoint.X) / 2, (startPoint.Y + endPoint.Y) / 2);

            //弧连线斜率
            double k1 = (startPoint.Y - endPoint.Y) / (startPoint.X - endPoint.X);

            //圆心连线斜率
            double k2;

            //弧于圆心的距离
            double distance;
            distance = Math.Sqrt(radius * radius - (width * width) / 4);

            //圆心点
            Point roundCenterLeft = new Point();
            Point roundCenterRight = new Point();

            if (double.IsInfinity(k1))
            {
                roundCenterLeft = new Point(crossingPoint.X + distance, crossingPoint.Y);
                roundCenterRight = new Point(crossingPoint.X - distance, crossingPoint.Y);
            }
            else if (k1 == 0)
            {
                roundCenterLeft = new Point(crossingPoint.X, crossingPoint.Y + distance);
                roundCenterRight = new Point(crossingPoint.X, crossingPoint.Y - distance);
            }
            else
            {
                k2 = (-1) / k1;
                double xOffset = distance / (Math.Sqrt(1 + k2 * k2));

                double xTemp, yTemp;
                #region 第一个圆心点
                xTemp = crossingPoint.X + xOffset;
                yTemp = k2 * (xTemp - crossingPoint.X) + crossingPoint.Y;
                roundCenterLeft = new Point(Math.Round(xTemp, 4), Math.Round(yTemp, 4));
                #endregion

                #region 第二个圆心点
                xTemp = crossingPoint.X - xOffset;
                yTemp = k2 * (xTemp - crossingPoint.X) + crossingPoint.Y;
                roundCenterRight = new Point(Math.Round(xTemp, 4), Math.Round(yTemp, 4));
                #endregion
            }

            if (-1 == MathExtension.GetPointLeftOrRight(startPoint, endPoint, roundCenterLeft))
            {
                Point pointTemp = roundCenterLeft;
                roundCenterLeft = roundCenterRight;
                roundCenterRight = pointTemp;
            }

            //大弧(注意坐标已经发先水平翻转)
            if (isLargeArc)
            {
                //顺时针 取左边圆心
                if (sweepDirection == SweepDirection.Clockwise)
                {
                    return roundCenterRight;
                }
                else//逆时针 取右边圆心
                {
                    return roundCenterLeft;
                }
            }
            else //小弧
            {
                //顺时针 取右边圆心
                if (sweepDirection == SweepDirection.Clockwise)
                {
                    return roundCenterLeft;
                }
                else//逆时针 取左边圆心
                {
                    return roundCenterRight;
                }
            }
        }

        /// <summary>
        /// 功能：求点在有向直线左边还是右边
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>返回：0共线、1左边、-1右边 </returns>
        public static int GetPointLeftOrRight(Point startPoint, Point endPoint, Point point)
        {
            double t;
            startPoint.X -= point.X;
            startPoint.Y -= point.Y;

            endPoint.X -= point.X;
            endPoint.Y -= point.Y;

            t = startPoint.X * endPoint.Y - startPoint.Y * endPoint.X;
            return t == 0 ? 0 : t < 0 ? -1 : 1;
        }

        public void ReplaceString()
        {
            string a = Regex.Replace("c:\\3\\\\33\\\\\\234\\\\\\\\fdsfsdf\\34", "\\{2,}", "\\");
        }

        public static Point GetBezierCenter(Point startPoint, Point endPoint, double radius, SweepDirection sweepDirection)
        {
            if (startPoint == endPoint)
                throw new Exception("起点与终点不能重合.");

            //弧连线的长
            double width = Math.Sqrt((startPoint.Y - endPoint.Y) * (startPoint.Y - endPoint.Y) + (startPoint.X - endPoint.X) * (startPoint.X - endPoint.X));

            if (width/2 > radius)
                throw new RadiusException("半径不能小于起点与终点的距离.");

            //中点,即交点
            Point crossingPoint = new Point((startPoint.X + endPoint.X) / 2, (startPoint.Y + endPoint.Y) / 2);

            //弧连线斜率
            double k1 = (startPoint.Y - endPoint.Y) / (startPoint.X - endPoint.X);

            //圆心连线斜率
            double k2;

            //弧于圆心的距离
            double distance;
            distance = Math.Sqrt(radius * radius - (width * width) / 4);

            //圆心点
            Point roundCenterLeft = new Point();
            Point roundCenterRight = new Point();

            if (double.IsInfinity(k1))
            {
                roundCenterLeft = new Point(crossingPoint.X + distance, crossingPoint.Y);
                roundCenterRight = new Point(crossingPoint.X - distance, crossingPoint.Y);
            }
            else if (k1 == 0)
            {
                roundCenterLeft = new Point(crossingPoint.X, crossingPoint.Y + distance);
                roundCenterRight = new Point(crossingPoint.X, crossingPoint.Y - distance);
            }
            else
            {
                k2 = (-1) / k1;
                double xOffset = distance / (Math.Sqrt(1 + k2 * k2));

                double xTemp, yTemp;
                #region 第一个圆心点
                xTemp = crossingPoint.X + xOffset;
                yTemp = k2 * (xTemp - crossingPoint.X) + crossingPoint.Y;
                roundCenterLeft = new Point(Math.Round(xTemp, 4), Math.Round(yTemp, 4));
                #endregion

                #region 第二个圆心点
                xTemp = crossingPoint.X - xOffset;
                yTemp = k2 * (xTemp - crossingPoint.X) + crossingPoint.Y;
                roundCenterRight = new Point(Math.Round(xTemp, 4), Math.Round(yTemp, 4));
                #endregion
            }

            if (-1 == MathExtension.GetPointLeftOrRight(startPoint, endPoint, roundCenterLeft))
            {
                Point pointTemp = roundCenterLeft;
                roundCenterLeft = roundCenterRight;
                roundCenterRight = pointTemp;
            }

            //顺时针 取右边圆心
            if (sweepDirection == SweepDirection.Clockwise)
            {
                return roundCenterLeft;
            }
            else//逆时针 取左边圆心
            {
                return roundCenterRight;
            }
        }

    }

    /// <summary>
    /// 说明:区间
    /// </summary>
    /// <!--作者:韦腾 时间:2008.12.16-->
    public enum ExtentDirectionEnum
    {
        /// <summary>
        /// 第一象限
        /// </summary>
        Quadrant1 = 0,

        /// <summary>
        /// 第二象限
        /// </summary>
        Quadrant2 = 1,

        /// <summary>
        /// 第三象限
        /// </summary>
        Quadrant3 = 2,

        /// <summary>
        /// 第四象限
        /// </summary>
        Quadrant4 = 3,

        /// <summary>
        /// X正轴
        /// </summary>
        XB = 4,

        /// <summary>
        /// X负轴
        /// </summary>
        XL = 5,

        /// <summary>
        /// Y正轴
        /// </summary>
        YB = 6,

        /// <summary>
        /// Y负轴
        /// </summary>
        YL = 7,

        /// <summary>
        /// 原点
        /// </summary>
        Origin = 9
    }
    public class RadiusException : Exception
    {
        public override string Message
        {
            get
            {
                return this._Message;
            }
        }
        private string _Message = string.Empty;

        public RadiusException(string message)
        {
            this._Message = message;
        }
    }

}
