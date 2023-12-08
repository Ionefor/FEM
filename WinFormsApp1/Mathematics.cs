using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKE
{
    public static class Mathematics
    {
        public static Vector2d[] GetCoordinatesVectors(int firstIndex, int secondIndex, int thirdIndex, ref List<PointD> points)
        {
            var vectors = new Vector2d[2];

            vectors[0].X = points[firstIndex].X - points[secondIndex].X;
            vectors[0].Y = points[firstIndex].Y - points[secondIndex].Y;

            vectors[1].X = points[thirdIndex].X - points[secondIndex].X;
            vectors[1].Y = points[thirdIndex].Y - points[secondIndex].Y;

            return vectors;
        }
        public static double GetAngleVectors(Vector2d firstVector, Vector2d secondVector)
        {
            return ((Math.Acos((firstVector.X * secondVector.X + firstVector.Y * secondVector.Y) /
                        (Math.Sqrt((firstVector.X * firstVector.X + firstVector.Y * firstVector.Y) *
                        (secondVector.X * secondVector.X + secondVector.Y * secondVector.Y))))) * 180) / Math.PI;
        }
        private static Vector2d[] GetVectorTriangle(PointD firstPoint, PointD secondPoint, PointD thirdPoint)
        {
            Vector2d[] vector = new Vector2d[2];

            vector[0].X = secondPoint.X - firstPoint.X;
            vector[0].Y = secondPoint.Y - firstPoint.Y;

            vector[1].X = thirdPoint.X - firstPoint.X;
            vector[1].Y = thirdPoint.Y - firstPoint.Y;

            return vector;
        }
        public static Vector2d[] GetVectorSideTriangle(Triangle triangle, int sideIndex)
        {
            var vector = new Vector2d[2];

            if (sideIndex == 1)
            {
                vector[0].X = triangle.SecondVertex.X - triangle.FirstVertex.X;
                vector[0].Y = triangle.SecondVertex.Y - triangle.FirstVertex.Y;

                vector[1].X = triangle.ThirdVertex.X - triangle.FirstVertex.X;
                vector[1].Y = triangle.ThirdVertex.Y - triangle.FirstVertex.Y;
            }
            else if (sideIndex == 2)
            {
                vector[0].X = triangle.FirstVertex.X - triangle.SecondVertex.X;
                vector[0].Y = triangle.FirstVertex.Y - triangle.SecondVertex.Y;

                vector[1].X = triangle.ThirdVertex.X - triangle.SecondVertex.X;
                vector[1].Y = triangle.ThirdVertex.Y - triangle.SecondVertex.Y;
            }
            else if (sideIndex == 3)
            {
                vector[0].X = triangle.FirstVertex.X - triangle.ThirdVertex.X;
                vector[0].Y = triangle.FirstVertex.Y - triangle.ThirdVertex.Y;

                vector[1].X = triangle.SecondVertex.X - triangle.ThirdVertex.X;
                vector[1].Y = triangle.SecondVertex.Y - triangle.ThirdVertex.Y;
            }
            return vector;
        }
        public static double GetDistancePoints(PointD firstPoint, PointD secondPoint)
        {
            return Math.Sqrt((firstPoint.X - secondPoint.X) * (firstPoint.X - secondPoint.X) + (firstPoint.Y - secondPoint.Y) * (firstPoint.Y - secondPoint.Y));
        }
        public static PointD VertexMaxAngle(Triangle triangle)
        {
            Vector2d[] vectors;
            double firstAngle = 0, secondAngle = 0, thirdAngle = 0;

            for (int i = 1; i < 4; i++)
            {
                vectors = GetVectorSideTriangle(triangle, i);

                if (i == 1)
                {
                    firstAngle = GetAngleVectors(vectors[0], vectors[1]);
                }
                else if (i == 2)
                {
                    secondAngle = GetAngleVectors(vectors[0], vectors[1]);
                }
                else
                {
                    thirdAngle = GetAngleVectors(vectors[0], vectors[1]);
                }
            }

            return (firstAngle >= secondAngle) ? ((firstAngle >= thirdAngle) ? (triangle.FirstVertex) : (triangle.ThirdVertex)) : ((secondAngle >= thirdAngle) ? (triangle.SecondVertex) : (triangle.ThirdVertex));
        }
        public static double ShortestEdgeTriagnle(Triangle triangle)
        {
            double ShortestEdge = 1000;

            if (GetDistancePoints(triangle.FirstVertex, triangle.SecondVertex) < ShortestEdge)
            {
                ShortestEdge = GetDistancePoints(triangle.FirstVertex, triangle.SecondVertex);
            }

            if (GetDistancePoints(triangle.SecondVertex, triangle.ThirdVertex) < ShortestEdge)
            {
                ShortestEdge = GetDistancePoints(triangle.SecondVertex, triangle.ThirdVertex);
            }

            if (GetDistancePoints(triangle.ThirdVertex, triangle.FirstVertex) < ShortestEdge)
            {
                ShortestEdge = GetDistancePoints(triangle.ThirdVertex, triangle.FirstVertex);
            }

            return ShortestEdge;
        }
        public static double ShortestEdgeTriagnle(ref List<Triangle> triangles)
        {
            double minEdge = 1000;

            for (int i = 1; i < triangles.Count; i++)
            {
                if (GetDistancePoints(triangles[i].FirstVertex, triangles[i].SecondVertex) < minEdge)
                {
                    minEdge = GetDistancePoints(triangles[i].FirstVertex, triangles[i].SecondVertex);
                }

                if (GetDistancePoints(triangles[i].SecondVertex, triangles[i].ThirdVertex) < minEdge)
                {
                    minEdge = GetDistancePoints(triangles[i].SecondVertex, triangles[i].ThirdVertex);
                }

                if (GetDistancePoints(triangles[i].ThirdVertex, triangles[i].FirstVertex) < minEdge)
                {
                    minEdge = GetDistancePoints(triangles[i].ThirdVertex, triangles[i].FirstVertex);
                }
            }

            return minEdge;
        }
        public static double GetSquareTriangle(Triangle triangle)
        {
            return Math.Abs((triangle.FirstVertex.X * triangle.SecondVertex.Y + triangle.SecondVertex.X * triangle.ThirdVertex.Y + triangle.ThirdVertex.X * triangle.FirstVertex.Y) -
                (triangle.SecondVertex.X * triangle.FirstVertex.Y + triangle.ThirdVertex.X * triangle.SecondVertex.Y + triangle.FirstVertex.X * triangle.ThirdVertex.Y)) / 2;
        }
        public static double GetProductSidesTriangle(Triangle triangle)
        {
            var side = new double[3];

            side[0] = Math.Sqrt((triangle.SecondVertex.X - triangle.FirstVertex.X) * (triangle.SecondVertex.X - triangle.FirstVertex.X) + (triangle.SecondVertex.Y - triangle.FirstVertex.Y) * (triangle.SecondVertex.Y - triangle.FirstVertex.Y));
            side[1] = Math.Sqrt((triangle.ThirdVertex.X - triangle.SecondVertex.X) * (triangle.ThirdVertex.X - triangle.SecondVertex.X) + (triangle.ThirdVertex.Y - triangle.SecondVertex.Y) * (triangle.ThirdVertex.Y - triangle.SecondVertex.Y));
            side[2] = Math.Sqrt((triangle.FirstVertex.X - triangle.ThirdVertex.X) * (triangle.FirstVertex.X - triangle.ThirdVertex.X) + (triangle.FirstVertex.Y - triangle.ThirdVertex.Y) * (triangle.FirstVertex.Y - triangle.ThirdVertex.Y));

            return side[0] * side[1] * side[2];
        }
        public static double GetSumtSidesTriangle(Triangle triangle)
        {
            var side = new double[3];

            side[0] = Math.Sqrt((triangle.SecondVertex.X - triangle.FirstVertex.X) * (triangle.SecondVertex.X - triangle.FirstVertex.X) + (triangle.SecondVertex.Y - triangle.FirstVertex.Y) * (triangle.SecondVertex.Y - triangle.FirstVertex.Y));
            side[1] = Math.Sqrt((triangle.ThirdVertex.X - triangle.SecondVertex.X) * (triangle.ThirdVertex.X - triangle.SecondVertex.X) + (triangle.ThirdVertex.Y - triangle.SecondVertex.Y) * (triangle.ThirdVertex.Y - triangle.SecondVertex.Y));
            side[2] = Math.Sqrt((triangle.FirstVertex.X - triangle.ThirdVertex.X) * (triangle.FirstVertex.X - triangle.ThirdVertex.X) + (triangle.FirstVertex.Y - triangle.ThirdVertex.Y) * (triangle.FirstVertex.Y - triangle.ThirdVertex.Y));

            return side[0] + side[1] + side[2];
        }
        public static double GetCircumRadius(Triangle triangle)
        {
            return (GetProductSidesTriangle(triangle) / (4 * GetSquareTriangle(triangle)));
        }
        public static double GetCircumRadiusShortestEdgeRatio(Triangle triangle)
        {
            return (GetCircumRadius(triangle) / ShortestEdgeTriagnle(triangle));
        }
        public static PointD GetCenterPointCircleTriangle(Triangle triangle)
        {
            PointD centerCirclePoint = new();

            centerCirclePoint.X = -(triangle.FirstVertex.Y * (triangle.SecondVertex.X * triangle.SecondVertex.X + triangle.SecondVertex.Y * triangle.SecondVertex.Y - triangle.ThirdVertex.X * triangle.ThirdVertex.X - triangle.ThirdVertex.Y * triangle.ThirdVertex.Y) +
                triangle.SecondVertex.Y * (triangle.ThirdVertex.X * triangle.ThirdVertex.X + triangle.ThirdVertex.Y * triangle.ThirdVertex.Y - triangle.FirstVertex.X * triangle.FirstVertex.X - triangle.FirstVertex.Y * triangle.FirstVertex.Y) +
                triangle.ThirdVertex.Y * (triangle.FirstVertex.X * triangle.FirstVertex.X + triangle.FirstVertex.Y * triangle.FirstVertex.Y - triangle.SecondVertex.X * triangle.SecondVertex.X - triangle.SecondVertex.Y * triangle.SecondVertex.Y)) /
                (2 * (triangle.FirstVertex.X * (triangle.SecondVertex.Y - triangle.ThirdVertex.Y) + triangle.SecondVertex.X * (triangle.ThirdVertex.Y - triangle.FirstVertex.Y) + triangle.ThirdVertex.X * (triangle.FirstVertex.Y - triangle.SecondVertex.Y)));

            centerCirclePoint.Y = (triangle.FirstVertex.X * (triangle.SecondVertex.X * triangle.SecondVertex.X + triangle.SecondVertex.Y * triangle.SecondVertex.Y - triangle.ThirdVertex.X * triangle.ThirdVertex.X - triangle.ThirdVertex.Y * triangle.ThirdVertex.Y) +
               triangle.SecondVertex.X * (triangle.ThirdVertex.X * triangle.ThirdVertex.X + triangle.ThirdVertex.Y * triangle.ThirdVertex.Y - triangle.FirstVertex.X * triangle.FirstVertex.X - triangle.FirstVertex.Y * triangle.FirstVertex.Y) +
               triangle.ThirdVertex.X * (triangle.FirstVertex.X * triangle.FirstVertex.X + triangle.FirstVertex.Y * triangle.FirstVertex.Y - triangle.SecondVertex.X * triangle.SecondVertex.X - triangle.SecondVertex.Y * triangle.SecondVertex.Y)) /
               (2 * (triangle.FirstVertex.X * (triangle.SecondVertex.Y - triangle.ThirdVertex.Y) + triangle.SecondVertex.X * (triangle.ThirdVertex.Y - triangle.FirstVertex.Y) + triangle.ThirdVertex.X * (triangle.FirstVertex.Y - triangle.SecondVertex.Y)));

            return centerCirclePoint;
        }
        public static bool PointIsDiametralCircle(Triangle triangle, PointD startPoint, PointD endPoint)
        {
            PointD midPoint = new()
            {
                X = (startPoint.X + endPoint.X) / 2,
                Y = (startPoint.Y + endPoint.Y) / 2
            };

            if (GetDistancePoints(GetCenterPointCircleTriangle(triangle), midPoint) <= (GetDistancePoints(midPoint, startPoint)))
            {
                return true;
            }
            return false;
        }
        public static PointD PPPfdsfs(Triangle triangle)//??????????????????
        {
            PointD pointD = new();

            pointD.X = (triangle.FirstVertex.X * GetSideLenght(2, triangle) + triangle.SecondVertex.X * GetSideLenght(3, triangle)
                + triangle.ThirdVertex.X * GetSideLenght(1, triangle)) / GetSumtSidesTriangle(triangle);

            pointD.Y = (triangle.FirstVertex.Y * GetSideLenght(2, triangle) + triangle.SecondVertex.Y * GetSideLenght(3, triangle)
                 + triangle.ThirdVertex.Y * GetSideLenght(1, triangle)) / GetSumtSidesTriangle(triangle);

            return pointD;
        }
        public static double GetSideLenght(int num, Triangle triangle)
        {
            if (num == 1)
            {
                return GetDistancePoints(triangle.FirstVertex, triangle.SecondVertex);
            }
            else if (num == 2)
            {
                return GetDistancePoints(triangle.SecondVertex, triangle.ThirdVertex);
            }
            else if (num == 3)
            {
                return GetDistancePoints(triangle.ThirdVertex, triangle.FirstVertex);

            }
            return -1;
        }
        public static int RightAngleTriangle(Triangle triangle)
        {
            Vector2d[] vectors;
            double firstAngle = 0, secondAngle = 0, thirdAngle = 0;

            for (int i = 1; i < 4; i++)
            {
                vectors = GetVectorSideTriangle(triangle, i);

                if (i == 1)
                {
                    firstAngle = GetAngleVectors(vectors[0], vectors[1]);
                }
                else if (i == 2)
                {
                    secondAngle = GetAngleVectors(vectors[0], vectors[1]);
                }
                else
                {
                    thirdAngle = GetAngleVectors(vectors[0], vectors[1]);
                }
            }

            return (firstAngle >= secondAngle) ? ((firstAngle >= thirdAngle) ? (2) : (1)) : ((secondAngle >= thirdAngle) ? (3) : (1));
        }
        public static bool ObtuseAngle(PointD firstPoint, PointD secondPoint, PointD thirdPoint)
        {
            Vector2d[] vector = GetVectorTriangle(firstPoint, secondPoint, thirdPoint);

            if (GetAngleVectors(vector[0], vector[1]) > 90)
            {
                return true;
            }
            else
            {
                vector = GetVectorTriangle(secondPoint, firstPoint, thirdPoint);

                if (GetAngleVectors(vector[0], vector[1]) > 90)
                {
                    return true;
                }
            }
            return false;
        }
        public static double MinDistanceToPoint(PointD firstPoint, PointD secondPoint, PointD thirdPoint)
        {
            if (Math.Sqrt((thirdPoint.X - firstPoint.X) * (thirdPoint.X - firstPoint.X) + (thirdPoint.Y - firstPoint.Y) * (thirdPoint.Y - firstPoint.Y)) < Math.Sqrt((thirdPoint.X - secondPoint.X) * (thirdPoint.X - secondPoint.X) + (thirdPoint.Y - secondPoint.Y) * (thirdPoint.Y - secondPoint.Y)))
            {
                return Math.Sqrt((thirdPoint.X - firstPoint.X) * (thirdPoint.X - firstPoint.X) + (thirdPoint.Y - firstPoint.Y) * (thirdPoint.Y - firstPoint.Y));
            }
            return Math.Sqrt((thirdPoint.X - secondPoint.X) * (thirdPoint.X - secondPoint.X) + (thirdPoint.Y - secondPoint.Y) * (thirdPoint.Y - secondPoint.Y));
        }
    }
}
