using System;
using System.Windows;

namespace Common.Tools
{
    public class GeometryTool
    {
        // https://stackoverflow.com/questions/16028752/how-do-i-get-all-the-points-between-two-point-objects
        /// <summary>
        /// 두 점 사이의 점 컬렉션을 반환하는 메소드.
        /// </summary>
        /// <param name="p1">첫번째 점 정보.</param>
        /// <param name="p2">두번째 점 정보.</param>
        /// <param name="quantity">반환하는 점의 개수.</param>
        /// <returns>두 점 사이의 점 컬렉션.</returns>
        public static Point[] GetPoints(Point p1, Point p2, int quantity)
        {
            Point[] points = new Point[quantity];
            double ydiff = p2.Y - p1.Y, xdiff = p2.X - p1.X;
            double slope = (double)(p2.Y - p1.Y) / (p2.X - p1.X);
            double x, y;

            --quantity;

            for (double i = 0; i < quantity; i++)
            {
                y = Equals(slope, 0.0) ? 0.0 : ydiff * (i / quantity);
                x = Equals(slope, 0.0) ? xdiff * (i / quantity) : y / slope;

                points[(int)i].X = x + p1.X;
                points[(int)i].Y = y + p1.Y;
            }

            points[quantity] = p2;
            return points;
        }

        /// https://kayuse88.github.io/haversine/
        /// <summary>
        /// 두 점 사이의 거리를 계산 합니다.
        /// </summary>
        /// <returns></returns>
        public static double DistanceInMeterByHaversine(Point p1, Point p2)
        {
            double distance;
            double radius = 6371; // 지구 반지름(km)
            double toRadian = Math.PI / 180;

            double deltaLatitude = Math.Abs(p1.X - p2.X) * toRadian;
            double deltaLongitude = Math.Abs(p1.Y - p2.Y) * toRadian;

            double sinDeltaLat = Math.Sin(deltaLatitude / 2);
            double sinDeltaLng = Math.Sin(deltaLongitude / 2);
            double squareRoot = Math.Sqrt(
                sinDeltaLat * sinDeltaLat +
                Math.Cos(p1.X * toRadian) * Math.Cos(p2.X * toRadian) * sinDeltaLng * sinDeltaLng);

            distance = 2 * radius * Math.Asin(squareRoot);

            return distance * 1000;
        }
    }
}
