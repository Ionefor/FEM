using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKE
{
    public class Triangulation
    {
        private int _sizeGridChoice;
        /// <summary>
        /// Список треугольников триангуляции Делоне
        /// </summary>
        private List<Triangle> _triangles;
        /// <summary>
        /// Начальный массив точек области
        /// </summary>
        private PointD[] _points;
        public List<Triangle> Triangles { get => _triangles; }
        public Triangulation(PointD[] points, int sizeGridChoice)
        {
            _sizeGridChoice = sizeGridChoice;
            _points = points;

            _triangles = new();
        }
        public void InitialPartitioningArea()
        {
            Vector2d[] vectors;
            Triangle tempTriangle = new();

            int iFirstIndex, iSecondIndex, iThirdIndex;
            int jFirstIndex, jSecondIndex, jThirdIndex;

            int foundIndex = 0;

            double currentAngle, minAngle = 360;

            var splitPoints = MiddleSplitEdgesArea();

            int numberOfPoints = splitPoints.Count;

            for (int i = 0; i < numberOfPoints - 2; i++)
            {
                for (int j = 0; j < splitPoints.Count; j++)
                {
                    if ((j + 1) >= splitPoints.Count)
                    {
                        jFirstIndex = j;
                        jSecondIndex = Math.Abs(splitPoints.Count - j - 1);
                        jThirdIndex = Math.Abs(splitPoints.Count - j - 2);
                    }
                    else if ((j + 2) >= splitPoints.Count)
                    {
                        jFirstIndex = j;
                        jSecondIndex = j + 1;
                        jThirdIndex = Math.Abs(splitPoints.Count - j - 2);
                    }
                    else
                    {
                        jFirstIndex = j;
                        jSecondIndex = j + 1;
                        jThirdIndex = j + 2;
                    }

                    vectors = Mathematics.GetCoordinatesVectors(jFirstIndex, jSecondIndex, jThirdIndex, ref splitPoints);
                    currentAngle = Mathematics.GetAngleVectors(vectors[0], vectors[1]);

                    if ((splitPoints[jFirstIndex].X * splitPoints[jSecondIndex].Y + splitPoints[jThirdIndex].X * splitPoints[jFirstIndex].Y + splitPoints[jThirdIndex].Y * splitPoints[jSecondIndex].X
                         - splitPoints[jThirdIndex].X * splitPoints[jSecondIndex].Y - splitPoints[jFirstIndex].X * splitPoints[jThirdIndex].Y - splitPoints[jSecondIndex].X * splitPoints[jFirstIndex].Y) <= 0)
                    {
                        currentAngle = 360 - currentAngle;
                    }

                    if (currentAngle < minAngle)
                    {
                        minAngle = currentAngle;
                        foundIndex = jFirstIndex;
                    }
                }
                if ((foundIndex + 1) >= splitPoints.Count)
                {
                    iFirstIndex = foundIndex;
                    iSecondIndex = Math.Abs(splitPoints.Count - foundIndex - 1);
                    iThirdIndex = Math.Abs(splitPoints.Count - foundIndex - 2);
                }
                else if ((foundIndex + 2) >= splitPoints.Count)
                {
                    iFirstIndex = foundIndex;
                    iSecondIndex = foundIndex + 1;
                    iThirdIndex = Math.Abs(splitPoints.Count - foundIndex - 2);
                }
                else
                {
                    iFirstIndex = foundIndex;
                    iSecondIndex = foundIndex + 1;
                    iThirdIndex = foundIndex + 2;
                }

                tempTriangle.FirstVertex = splitPoints[iFirstIndex];
                tempTriangle.SecondVertex = splitPoints[iSecondIndex];
                tempTriangle.ThirdVertex = splitPoints[iThirdIndex];

                _triangles.Add(tempTriangle);

                FlipBadEdges(_triangles[_triangles.Count - 1], Mathematics.VertexMaxAngle(_triangles[_triangles.Count - 1]));

                if (RemovePoint(splitPoints[iSecondIndex], ref _triangles, splitPoints))
                {
                    splitPoints.RemoveAt(iSecondIndex);
                }
                minAngle = 360;
            }
        }
        public void DelaunayTriangulation()
        {
            PointD midPoint = new();
            double hInitial = Mathematics.ShortestEdgeTriagnle(ref _triangles);

        step_two:;
            int indexBadTriangle = FindIndexBadTriangle(hInitial);

            if (indexBadTriangle != -1)
            {
                for (int i = 0; i < _triangles.Count; i++)
                {
                    if (Mathematics.PointIsDiametralCircle(_triangles[indexBadTriangle], _triangles[i].FirstVertex, _triangles[i].SecondVertex))
                    {
                        midPoint.X = (_triangles[i].FirstVertex.X + _triangles[i].SecondVertex.X) / 2;
                        midPoint.Y = (_triangles[i].FirstVertex.Y + _triangles[i].SecondVertex.Y) / 2;

                        AddPoint(midPoint);

                        goto step_two;
                    }
                    else if (Mathematics.PointIsDiametralCircle(_triangles[indexBadTriangle], _triangles[i].SecondVertex, _triangles[i].ThirdVertex))
                    {
                        midPoint.X = (_triangles[i].SecondVertex.X + _triangles[i].ThirdVertex.X) / 2;
                        midPoint.Y = (_triangles[i].SecondVertex.Y + _triangles[i].ThirdVertex.Y) / 2;

                        AddPoint(midPoint);

                        goto step_two;
                    }
                    else if (Mathematics.PointIsDiametralCircle(_triangles[indexBadTriangle], _triangles[i].ThirdVertex, _triangles[i].FirstVertex))
                    {
                        midPoint.X = (_triangles[i].ThirdVertex.X + _triangles[i].FirstVertex.X) / 2;
                        midPoint.Y = (_triangles[i].ThirdVertex.Y + _triangles[i].FirstVertex.Y) / 2;

                        AddPoint(midPoint);

                        goto step_two;
                    }
                }

                if (!PointBeyondArea(Mathematics.GetCenterPointCircleTriangle(_triangles[indexBadTriangle])))
                {
                    AddPoint(Mathematics.GetCenterPointCircleTriangle(_triangles[indexBadTriangle]));
                }
                else
                {
                    SplitHeightTriangle(_triangles[indexBadTriangle]);
                }

                goto step_two;
            }
        }
        private List<PointD> MiddleSplitEdgesArea()
        {
            PointD startPoint = _points[_points.Length - 1];
            PointD midPoint = new();

            List<PointD> splitPoints = new();

            for (int i = 0; i < _points.Length; i++)
            {
                PointD endPoint = _points[i];

                midPoint.X = (startPoint.X + endPoint.X) / 2;
                midPoint.Y = (startPoint.Y + endPoint.Y) / 2;

                splitPoints.Add(startPoint);
                splitPoints.Add(midPoint);

                startPoint = endPoint;
            }

            return splitPoints;
        }
        private static bool RemovePoint(PointD currentPoint, ref List<Triangle> triangles, List<PointD> splitPoints)
        {
            bool flagO = false, flagN = false;
            int count;

            for (int i = 0; i < splitPoints.Count; i++)
            {
                if (currentPoint == splitPoints[i])
                {
                    for (int j = 0; j < triangles.Count; j++)
                    {
                        if (i == 0)
                        {
                            count = splitPoints.Count - 1;

                            if (splitPoints[count] == triangles[j].FirstVertex || splitPoints[count] == triangles[j].SecondVertex || splitPoints[count] == triangles[j].ThirdVertex)
                            {
                                flagO = true;
                            }
                        }
                        else
                        {
                            if (splitPoints[i - 1] == triangles[j].FirstVertex || splitPoints[i - 1] == triangles[j].SecondVertex || splitPoints[i - 1] == triangles[j].ThirdVertex)
                            {
                                flagO = true;
                            }
                        }

                        if (i == splitPoints.Count - 1)
                        {
                            count = 0;
                            if (splitPoints[count] == triangles[j].FirstVertex || splitPoints[count] == triangles[j].SecondVertex || splitPoints[count] == triangles[j].ThirdVertex)
                            {
                                flagN = true;
                            }
                        }
                        else
                        {
                            if (splitPoints[i + 1] == triangles[j].FirstVertex || splitPoints[i + 1] == triangles[j].SecondVertex || splitPoints[i + 1] == triangles[j].ThirdVertex)
                            {
                                flagN = true;
                            }
                        }
                    }
                }
            }

            if (flagN && flagO)
            {
                return true;
            }

            return false;
        }
        private int FindIndexBadTriangle(double hInitial)
        {
            double h, desiredSumSides, B = Math.Sqrt(2);
            var maxSumSides = 0d;
            var indexBadTriangle = -1;

            if (_sizeGridChoice == 1)
            {
                h = hInitial;
                desiredSumSides = 3;
            }
            else if (_sizeGridChoice == 2)
            {
                h = hInitial / 3;
                desiredSumSides = 2;
            }
            else
            {
                h = hInitial / 5;
                desiredSumSides = 1;
            }

            for (int i = 0; i < _triangles.Count; i++)
            {
                if (Mathematics.GetCircumRadiusShortestEdgeRatio(_triangles[i]) > B && Mathematics.GetSumtSidesTriangle(_triangles[i]) > desiredSumSides)
                {

                }
                else if (Mathematics.GetCircumRadius(_triangles[i]) > h && Mathematics.ShortestEdgeTriagnle(_triangles[i]) > h && Mathematics.GetSumtSidesTriangle(_triangles[i]) > desiredSumSides)
                {

                }
                else
                {
                    continue;
                }

                if (Mathematics.GetSumtSidesTriangle(_triangles[i]) > maxSumSides)
                {
                    maxSumSides = Mathematics.GetSumtSidesTriangle(_triangles[i]);
                    indexBadTriangle = i;
                }
            }

            return indexBadTriangle;
        }
        private void AddPoint(PointD addedPoint)
        {
            List<EdgeAndTriangle> edgesAndTriangles = PointOnSideTriangle(addedPoint);

            if (edgesAndTriangles.Count != 0)
            {
                for (int i = 0; i < edgesAndTriangles.Count; i++)
                {
                    SplitNeighborTriangles(edgesAndTriangles[i].NumEdgeTriangle, edgesAndTriangles[i].NumTriangle, addedPoint);
                }

                if (edgesAndTriangles.Count != 1)
                {
                    if (edgesAndTriangles[0].NumTriangle > edgesAndTriangles[1].NumTriangle)
                    {
                        _triangles.RemoveAt(edgesAndTriangles[0].NumTriangle);
                        _triangles.RemoveAt(edgesAndTriangles[1].NumTriangle);
                    }
                    else
                    {
                        _triangles.RemoveAt(edgesAndTriangles[1].NumTriangle);
                        _triangles.RemoveAt(edgesAndTriangles[0].NumTriangle);
                    }

                    FlipBadEdges(_triangles[_triangles.Count - 4], addedPoint);
                    FlipBadEdges(_triangles[_triangles.Count - 3], addedPoint);
                    FlipBadEdges(_triangles[_triangles.Count - 2], addedPoint);
                    FlipBadEdges(_triangles[_triangles.Count - 1], addedPoint);
                }
                else
                {
                    _triangles.RemoveAt(edgesAndTriangles[0].NumTriangle);

                    FlipBadEdges(_triangles[_triangles.Count - 2], addedPoint);
                    FlipBadEdges(_triangles[_triangles.Count - 1], addedPoint);
                }
            }
            else
            {
                for (int i = 0; i < _triangles.Count; i++)
                {
                    if (PointOnTriangle(_triangles[i], addedPoint))
                    {
                        SplitTriangle(i, addedPoint);
                        _triangles.RemoveAt(i);

                        FlipBadEdges(_triangles[_triangles.Count - 3], addedPoint);
                        FlipBadEdges(_triangles[_triangles.Count - 2], addedPoint);
                        FlipBadEdges(_triangles[_triangles.Count - 1], addedPoint);
                        break;
                    }
                }
            }
        }
        private void FlipBadEdges(Triangle triangle, PointD newPoint)
        {
            Triangle[] tempTriangle = new Triangle[2];

            //ребро в новом треугольнике, которое не содержит NewPoint
            int commonEdgeIndex = NumberEdgeTriangle(triangle, newPoint);

            //номер другого треугольника и его общее ребро с новым треугольником
            EdgeAndTriangle edgesAndTriangleNeighbor = NeighborTriangle(commonEdgeIndex, ref triangle);

            if (edgesAndTriangleNeighbor.NumEdgeTriangle != 0)
            {
                if (PointInCircleNeighborTriangle(ref edgesAndTriangleNeighbor, ref triangle))
                {
                    if (commonEdgeIndex == 1)
                    {
                        if (edgesAndTriangleNeighbor.NumEdgeTriangle == 1)
                        {
                            tempTriangle[0].FirstVertex = newPoint;
                            tempTriangle[0].SecondVertex = triangle.FirstVertex;
                            tempTriangle[0].ThirdVertex = _triangles[edgesAndTriangleNeighbor.NumTriangle].ThirdVertex;

                            _triangles.Add(tempTriangle[0]);

                            tempTriangle[1].FirstVertex = _triangles[edgesAndTriangleNeighbor.NumTriangle].ThirdVertex;
                            tempTriangle[1].SecondVertex = triangle.SecondVertex;
                            tempTriangle[1].ThirdVertex = newPoint;

                            _triangles.Add(tempTriangle[1]);

                            _triangles.Remove(triangle);
                            _triangles.Remove(_triangles[edgesAndTriangleNeighbor.NumTriangle]);

                            FlipBadEdges(_triangles[_triangles.Count - 2], newPoint);
                            FlipBadEdges(_triangles[_triangles.Count - 1], newPoint);
                        }
                        else if (edgesAndTriangleNeighbor.NumEdgeTriangle == 2)
                        {
                            tempTriangle[0].FirstVertex = newPoint;
                            tempTriangle[0].SecondVertex = triangle.FirstVertex;
                            tempTriangle[0].ThirdVertex = _triangles[edgesAndTriangleNeighbor.NumTriangle].FirstVertex;

                            _triangles.Add(tempTriangle[0]);

                            tempTriangle[1].FirstVertex = _triangles[edgesAndTriangleNeighbor.NumTriangle].FirstVertex;
                            tempTriangle[1].SecondVertex = triangle.SecondVertex;
                            tempTriangle[1].ThirdVertex = newPoint;

                            _triangles.Add(tempTriangle[1]);

                            _triangles.Remove(triangle);
                            _triangles.Remove(_triangles[edgesAndTriangleNeighbor.NumTriangle]);

                            FlipBadEdges(_triangles[_triangles.Count - 2], newPoint);
                            FlipBadEdges(_triangles[_triangles.Count - 1], newPoint);
                        }
                        else if (edgesAndTriangleNeighbor.NumEdgeTriangle == 3)
                        {
                            tempTriangle[0].FirstVertex = newPoint;
                            tempTriangle[0].SecondVertex = triangle.FirstVertex;
                            tempTriangle[0].ThirdVertex = _triangles[edgesAndTriangleNeighbor.NumTriangle].SecondVertex;

                            _triangles.Add(tempTriangle[0]);

                            tempTriangle[1].FirstVertex = _triangles[edgesAndTriangleNeighbor.NumTriangle].SecondVertex;
                            tempTriangle[1].SecondVertex = triangle.SecondVertex;
                            tempTriangle[1].ThirdVertex = newPoint;

                            _triangles.Add(tempTriangle[1]);

                            _triangles.Remove(triangle);
                            _triangles.Remove(_triangles[edgesAndTriangleNeighbor.NumTriangle]);

                            FlipBadEdges(_triangles[_triangles.Count - 2], newPoint);
                            FlipBadEdges(_triangles[_triangles.Count - 1], newPoint);
                        }
                    }
                    else if (commonEdgeIndex == 2)
                    {
                        if (edgesAndTriangleNeighbor.NumEdgeTriangle == 1)
                        {
                            tempTriangle[0].FirstVertex = newPoint;
                            tempTriangle[0].SecondVertex = triangle.SecondVertex;
                            tempTriangle[0].ThirdVertex = _triangles[edgesAndTriangleNeighbor.NumTriangle].ThirdVertex;

                            _triangles.Add(tempTriangle[0]);

                            tempTriangle[1].FirstVertex = _triangles[edgesAndTriangleNeighbor.NumTriangle].ThirdVertex;
                            tempTriangle[1].SecondVertex = triangle.ThirdVertex;
                            tempTriangle[1].ThirdVertex = newPoint;

                            _triangles.Add(tempTriangle[1]);

                            _triangles.Remove(triangle);
                            _triangles.Remove(_triangles[edgesAndTriangleNeighbor.NumTriangle]);

                            FlipBadEdges(_triangles[_triangles.Count - 2], newPoint);
                            FlipBadEdges(_triangles[_triangles.Count - 1], newPoint);
                        }
                        else if (edgesAndTriangleNeighbor.NumEdgeTriangle == 2)
                        {
                            tempTriangle[0].FirstVertex = newPoint;
                            tempTriangle[0].SecondVertex = triangle.SecondVertex;
                            tempTriangle[0].ThirdVertex = _triangles[edgesAndTriangleNeighbor.NumTriangle].FirstVertex;

                            _triangles.Add(tempTriangle[0]);

                            tempTriangle[1].FirstVertex = _triangles[edgesAndTriangleNeighbor.NumTriangle].FirstVertex;
                            tempTriangle[1].SecondVertex = triangle.ThirdVertex;
                            tempTriangle[1].ThirdVertex = newPoint;

                            _triangles.Add(tempTriangle[1]);

                            _triangles.Remove(triangle);
                            _triangles.Remove(_triangles[edgesAndTriangleNeighbor.NumTriangle]);

                            FlipBadEdges(_triangles[_triangles.Count - 2], newPoint);
                            FlipBadEdges(_triangles[_triangles.Count - 1], newPoint);
                        }
                        else if (edgesAndTriangleNeighbor.NumEdgeTriangle == 3)
                        {
                            tempTriangle[0].FirstVertex = newPoint;
                            tempTriangle[0].SecondVertex = triangle.SecondVertex;
                            tempTriangle[0].ThirdVertex = _triangles[edgesAndTriangleNeighbor.NumTriangle].SecondVertex;

                            _triangles.Add(tempTriangle[0]);

                            tempTriangle[1].FirstVertex = _triangles[edgesAndTriangleNeighbor.NumTriangle].SecondVertex;
                            tempTriangle[1].SecondVertex = triangle.ThirdVertex;
                            tempTriangle[1].ThirdVertex = newPoint;

                            _triangles.Add(tempTriangle[1]);

                            _triangles.Remove(triangle);
                            _triangles.Remove(_triangles[edgesAndTriangleNeighbor.NumTriangle]);

                            FlipBadEdges(_triangles[_triangles.Count - 2], newPoint);
                            FlipBadEdges(_triangles[_triangles.Count - 1], newPoint);
                        }
                    }
                    else if (commonEdgeIndex == 3)
                    {
                        if (edgesAndTriangleNeighbor.NumEdgeTriangle == 1)
                        {
                            tempTriangle[0].FirstVertex = newPoint;
                            tempTriangle[0].SecondVertex = triangle.ThirdVertex;
                            tempTriangle[0].ThirdVertex = _triangles[edgesAndTriangleNeighbor.NumTriangle].ThirdVertex;

                            _triangles.Add(tempTriangle[0]);

                            tempTriangle[1].FirstVertex = _triangles[edgesAndTriangleNeighbor.NumTriangle].ThirdVertex;
                            tempTriangle[1].SecondVertex = triangle.FirstVertex;
                            tempTriangle[1].ThirdVertex = newPoint;

                            _triangles.Add(tempTriangle[1]);

                            _triangles.Remove(triangle);
                            _triangles.Remove(_triangles[edgesAndTriangleNeighbor.NumTriangle]);

                            FlipBadEdges(_triangles[_triangles.Count - 2], newPoint);
                            FlipBadEdges(_triangles[_triangles.Count - 1], newPoint);
                        }
                        else if (edgesAndTriangleNeighbor.NumEdgeTriangle == 2)
                        {
                            tempTriangle[0].FirstVertex = newPoint;
                            tempTriangle[0].SecondVertex = triangle.ThirdVertex;
                            tempTriangle[0].ThirdVertex = _triangles[edgesAndTriangleNeighbor.NumTriangle].FirstVertex;

                            _triangles.Add(tempTriangle[0]);

                            tempTriangle[1].FirstVertex = _triangles[edgesAndTriangleNeighbor.NumTriangle].FirstVertex;
                            tempTriangle[1].SecondVertex = triangle.FirstVertex;
                            tempTriangle[1].ThirdVertex = newPoint;

                            _triangles.Add(tempTriangle[1]);

                            _triangles.Remove(triangle);
                            _triangles.Remove(_triangles[edgesAndTriangleNeighbor.NumTriangle]);

                            FlipBadEdges(_triangles[_triangles.Count - 2], newPoint);
                            FlipBadEdges(_triangles[_triangles.Count - 1], newPoint);
                        }
                        else if (edgesAndTriangleNeighbor.NumEdgeTriangle == 3)
                        {
                            tempTriangle[0].FirstVertex = newPoint;
                            tempTriangle[0].SecondVertex = triangle.ThirdVertex;
                            tempTriangle[0].ThirdVertex = _triangles[edgesAndTriangleNeighbor.NumTriangle].SecondVertex;

                            _triangles.Add(tempTriangle[0]);

                            tempTriangle[1].FirstVertex = _triangles[edgesAndTriangleNeighbor.NumTriangle].SecondVertex;
                            tempTriangle[1].SecondVertex = triangle.FirstVertex;
                            tempTriangle[1].ThirdVertex = newPoint;

                            _triangles.Add(tempTriangle[1]);

                            _triangles.Remove(triangle);
                            _triangles.Remove(_triangles[edgesAndTriangleNeighbor.NumTriangle]);

                            FlipBadEdges(_triangles[_triangles.Count - 2], newPoint);
                            FlipBadEdges(_triangles[_triangles.Count - 1], newPoint); ;
                        }
                    }
                }
            }
        }
        private List<EdgeAndTriangle> PointOnSideTriangle(PointD point)
        {
            EdgeAndTriangle temp = new();
            List<EdgeAndTriangle> edgesAndTriangles = new();

            for (int i = 0; i < _triangles.Count; i++)
            {
                if (point != _triangles[i].FirstVertex && point != _triangles[i].SecondVertex && point != _triangles[i].ThirdVertex)
                {
                    if (Math.Abs(Mathematics.GetDistancePoints(_triangles[i].FirstVertex, point) + Mathematics.GetDistancePoints(_triangles[i].SecondVertex, point) - Mathematics.GetDistancePoints(_triangles[i].FirstVertex, _triangles[i].SecondVertex)) == 0)
                    {
                        temp.NumTriangle = i;
                        temp.NumEdgeTriangle = 1;

                        if (!AddEdgesAndTriangles(ref edgesAndTriangles, ref temp))
                        {
                            edgesAndTriangles.Add(temp);
                        }
                    }
                    else if (Math.Abs(Mathematics.GetDistancePoints(_triangles[i].SecondVertex, point) + Mathematics.GetDistancePoints(_triangles[i].ThirdVertex, point) - Mathematics.GetDistancePoints(_triangles[i].SecondVertex, _triangles[i].ThirdVertex)) == 0)
                    {
                        temp.NumTriangle = i;
                        temp.NumEdgeTriangle = 2;

                        if (!AddEdgesAndTriangles(ref edgesAndTriangles, ref temp))
                        {
                            edgesAndTriangles.Add(temp);
                        }
                    }
                    else if (Math.Abs(Mathematics.GetDistancePoints(_triangles[i].ThirdVertex, point) + Mathematics.GetDistancePoints(_triangles[i].FirstVertex, point) - Mathematics.GetDistancePoints(_triangles[i].ThirdVertex, _triangles[i].FirstVertex)) == 0)
                    {
                        temp.NumTriangle = i;
                        temp.NumEdgeTriangle = 3;

                        if (!AddEdgesAndTriangles(ref edgesAndTriangles, ref temp))
                        {
                            edgesAndTriangles.Add(temp);
                        }
                    }
                }
            }

            return edgesAndTriangles;
        }
        private static bool AddEdgesAndTriangles(ref List<EdgeAndTriangle> edgesAndTriangles, ref EdgeAndTriangle edgesAndTriangle)
        {
            for (int j = 0; j < edgesAndTriangles.Count; j++)
            {
                if (edgesAndTriangles[j].NumTriangle == edgesAndTriangle.NumTriangle)
                {
                    return true;
                }
            }
            return false;
        }
        private void SplitHeightTriangle(Triangle triangle)
        {
            Triangle tempTriangle = new();
            PointD newPoint = NewVertexTriangle(triangle);

            int numSide = Mathematics.RightAngleTriangle(triangle);

            if (numSide == 1)
            {
                tempTriangle.FirstVertex = triangle.FirstVertex;
                tempTriangle.SecondVertex = newPoint;
                tempTriangle.ThirdVertex = triangle.ThirdVertex;

                _triangles.Add(tempTriangle);

                tempTriangle.FirstVertex = newPoint;
                tempTriangle.SecondVertex = triangle.SecondVertex;
                tempTriangle.ThirdVertex = triangle.ThirdVertex;

                _triangles.Add(tempTriangle);

                _triangles.Remove(triangle);
            }
            else if (numSide == 2)
            {
                tempTriangle.FirstVertex = triangle.FirstVertex;
                tempTriangle.SecondVertex = triangle.SecondVertex;
                tempTriangle.ThirdVertex = newPoint;

                _triangles.Add(tempTriangle);

                tempTriangle.FirstVertex = triangle.FirstVertex;
                tempTriangle.SecondVertex = newPoint;
                tempTriangle.ThirdVertex = triangle.ThirdVertex;

                _triangles.Add(tempTriangle);

                _triangles.Remove(triangle);
            }
            else if (numSide == 3)
            {
                tempTriangle.FirstVertex = triangle.FirstVertex;
                tempTriangle.SecondVertex = triangle.SecondVertex;
                tempTriangle.ThirdVertex = newPoint;

                _triangles.Add(tempTriangle);

                tempTriangle.FirstVertex = newPoint;
                tempTriangle.SecondVertex = triangle.SecondVertex;
                tempTriangle.ThirdVertex = triangle.ThirdVertex;

                _triangles.Add(tempTriangle);

                _triangles.Remove(triangle);
            }
        }
        private static PointD NewVertexTriangle(Triangle triangle)
        {
            double cat;

            int side = Mathematics.RightAngleTriangle(triangle);
            double h = 2 * Mathematics.GetSquareTriangle(triangle) / Mathematics.GetSideLenght(side, triangle);

            PointD vec = new();
            PointD newVertex = new();

            if (side == 1)
            {
                //2
                cat = Math.Sqrt(Mathematics.GetDistancePoints(triangle.SecondVertex, triangle.ThirdVertex) * Mathematics.GetDistancePoints(triangle.SecondVertex, triangle.ThirdVertex) - h * h);

                vec.X = (triangle.FirstVertex.X - triangle.SecondVertex.X) / (float)Mathematics.GetDistancePoints(triangle.FirstVertex, triangle.SecondVertex);
                vec.Y = (triangle.FirstVertex.Y - triangle.SecondVertex.Y) / (float)Mathematics.GetDistancePoints(triangle.FirstVertex, triangle.SecondVertex);

                newVertex.X = (float)(triangle.SecondVertex.X + vec.X * cat);
                newVertex.Y = (float)(triangle.SecondVertex.Y + vec.Y * cat);
            }
            else if (side == 2)
            {
                //3
                cat = Math.Sqrt(Mathematics.GetDistancePoints(triangle.ThirdVertex, triangle.FirstVertex) * Mathematics.GetDistancePoints(triangle.ThirdVertex, triangle.FirstVertex) - h * h);

                vec.X = (triangle.SecondVertex.X - triangle.ThirdVertex.X) / (float)Mathematics.GetDistancePoints(triangle.SecondVertex, triangle.ThirdVertex);
                vec.Y = (triangle.SecondVertex.Y - triangle.ThirdVertex.Y) / (float)Mathematics.GetDistancePoints(triangle.SecondVertex, triangle.ThirdVertex);

                newVertex.X = (float)(triangle.ThirdVertex.X + vec.X * cat);
                newVertex.Y = (float)(triangle.ThirdVertex.Y + vec.Y * cat);
            }
            else if (side == 3)
            {
                //1
                cat = Math.Sqrt(Mathematics.GetDistancePoints(triangle.FirstVertex, triangle.SecondVertex) * Mathematics.GetDistancePoints(triangle.FirstVertex, triangle.SecondVertex) - h * h);

                vec.X = (triangle.ThirdVertex.X - triangle.FirstVertex.X) / (float)Mathematics.GetDistancePoints(triangle.ThirdVertex, triangle.FirstVertex);
                vec.Y = (triangle.ThirdVertex.Y - triangle.FirstVertex.Y) / (float)Mathematics.GetDistancePoints(triangle.ThirdVertex, triangle.FirstVertex);

                newVertex.X = (float)(triangle.FirstVertex.X + vec.X * cat);
                newVertex.Y = (float)(triangle.FirstVertex.Y + vec.Y * cat);
            }

            return newVertex;
        }
        private void SplitNeighborTriangles(int numEdge, int numTriangle, PointD addedPoint)
        {
            Triangle tempTriangle = new();
            if (numEdge == 1)
            {
                tempTriangle.FirstVertex = _triangles[numTriangle].FirstVertex;
                tempTriangle.SecondVertex = addedPoint;
                tempTriangle.ThirdVertex = _triangles[numTriangle].ThirdVertex;

                _triangles.Add(tempTriangle);

                tempTriangle.FirstVertex = addedPoint;
                tempTriangle.SecondVertex = _triangles[numTriangle].SecondVertex;
                tempTriangle.ThirdVertex = _triangles[numTriangle].ThirdVertex;

                _triangles.Add(tempTriangle);
            }
            else if (numEdge == 2)
            {
                tempTriangle.FirstVertex = _triangles[numTriangle].FirstVertex;
                tempTriangle.SecondVertex = _triangles[numTriangle].SecondVertex;
                tempTriangle.ThirdVertex = addedPoint;

                _triangles.Add(tempTriangle);

                tempTriangle.FirstVertex = addedPoint;
                tempTriangle.SecondVertex = _triangles[numTriangle].ThirdVertex;
                tempTriangle.ThirdVertex = _triangles[numTriangle].FirstVertex;

                _triangles.Add(tempTriangle);
            }
            else if (numEdge == 3)
            {
                tempTriangle.FirstVertex = _triangles[numTriangle].FirstVertex;
                tempTriangle.SecondVertex = _triangles[numTriangle].SecondVertex;
                tempTriangle.ThirdVertex = addedPoint;

                _triangles.Add(tempTriangle);

                tempTriangle.FirstVertex = addedPoint;
                tempTriangle.SecondVertex = _triangles[numTriangle].SecondVertex;
                tempTriangle.ThirdVertex = _triangles[numTriangle].ThirdVertex;

                _triangles.Add(tempTriangle);
            }
        }
        private void SplitTriangle(int numTriangle, PointD addedPoint)
        {
            Triangle tempTriangle = new();
            tempTriangle.FirstVertex = _triangles[numTriangle].FirstVertex;
            tempTriangle.SecondVertex = _triangles[numTriangle].SecondVertex;
            tempTriangle.ThirdVertex = addedPoint;

            _triangles.Add(tempTriangle);

            tempTriangle.FirstVertex = _triangles[numTriangle].FirstVertex;
            tempTriangle.SecondVertex = addedPoint;
            tempTriangle.ThirdVertex = _triangles[numTriangle].ThirdVertex;

            _triangles.Add(tempTriangle);

            tempTriangle.FirstVertex = addedPoint;
            tempTriangle.SecondVertex = _triangles[numTriangle].SecondVertex;
            tempTriangle.ThirdVertex = _triangles[numTriangle].ThirdVertex;

            _triangles.Add(tempTriangle);
        }
        private static bool PointOnTriangle(Triangle triangle, PointD point)
        {
            var s = new double[3];

            s[0] = (triangle.FirstVertex.X - point.X) * (triangle.SecondVertex.Y - triangle.FirstVertex.Y) - (triangle.SecondVertex.X - triangle.FirstVertex.X) * (triangle.FirstVertex.Y - point.Y);
            s[1] = (triangle.SecondVertex.X - point.X) * (triangle.ThirdVertex.Y - triangle.SecondVertex.Y) - (triangle.ThirdVertex.X - triangle.SecondVertex.X) * (triangle.SecondVertex.Y - point.Y);
            s[2] = (triangle.ThirdVertex.X - point.X) * (triangle.FirstVertex.Y - triangle.ThirdVertex.Y) - (triangle.FirstVertex.X - triangle.ThirdVertex.X) * (triangle.ThirdVertex.Y - point.Y);

            return (Math.Sign(s[0]) == Math.Sign(s[1]) && Math.Sign(s[0]) == Math.Sign(s[2]));
        }
        private bool PointBeyondArea(PointD addPoint)
        {
            List<EdgeAndTriangle> edgesAndTriangle = PointOnSideTriangle(addPoint);

            if (edgesAndTriangle.Count == 0)
            {
                for (int i = 0; i < _triangles.Count; i++)
                {
                    if (!PointOnTriangle(_triangles[i], addPoint))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        private static int NumberEdgeTriangle(Triangle triangle, PointD point)
        {
            if (point == triangle.FirstVertex)
            {
                return 2;
            }
            else if (point == triangle.SecondVertex)
            {
                return 3;
            }
            else if (point == triangle.ThirdVertex)
            {
                return 1;
            }
            return -1;
        }
        private EdgeAndTriangle NeighborTriangle(int indexEdge, ref Triangle triangle)
        {
            EdgeAndTriangle edgesAndTriangleRec = new();

            for (int i = 0; i < _triangles.Count; i++)
            {
                if (indexEdge == 1)
                {
                    if ((triangle.FirstVertex == _triangles[i].FirstVertex && triangle.SecondVertex == _triangles[i].SecondVertex && triangle.ThirdVertex != _triangles[i].ThirdVertex) |
                       (triangle.FirstVertex == _triangles[i].SecondVertex && triangle.SecondVertex == _triangles[i].FirstVertex && triangle.ThirdVertex != _triangles[i].ThirdVertex))
                    {
                        //1=1
                        edgesAndTriangleRec.NumTriangle = i;
                        edgesAndTriangleRec.NumEdgeTriangle = 1;

                    }
                    else if ((triangle.FirstVertex == _triangles[i].SecondVertex && triangle.SecondVertex == _triangles[i].ThirdVertex && triangle.ThirdVertex != _triangles[i].FirstVertex) ||
                        (triangle.FirstVertex == _triangles[i].ThirdVertex && triangle.SecondVertex == _triangles[i].SecondVertex && triangle.ThirdVertex != _triangles[i].FirstVertex))
                    {
                        //1=2
                        edgesAndTriangleRec.NumTriangle = i;
                        edgesAndTriangleRec.NumEdgeTriangle = 2;
                    }
                    else if ((triangle.FirstVertex == _triangles[i].ThirdVertex && triangle.SecondVertex == _triangles[i].FirstVertex && triangle.ThirdVertex != _triangles[i].SecondVertex) ||
                        (triangle.FirstVertex == _triangles[i].FirstVertex && triangle.SecondVertex == _triangles[i].ThirdVertex && triangle.ThirdVertex != _triangles[i].SecondVertex))
                    {
                        //1=3
                        edgesAndTriangleRec.NumTriangle = i;
                        edgesAndTriangleRec.NumEdgeTriangle = 3;
                    }
                }
                else if (indexEdge == 2)
                {
                    if ((triangle.SecondVertex == _triangles[i].FirstVertex && triangle.ThirdVertex == _triangles[i].SecondVertex && triangle.FirstVertex != _triangles[i].ThirdVertex) ||
                        (triangle.SecondVertex == _triangles[i].SecondVertex && triangle.ThirdVertex == _triangles[i].FirstVertex && triangle.FirstVertex != _triangles[i].ThirdVertex))
                    {
                        //2=1
                        edgesAndTriangleRec.NumTriangle = i;
                        edgesAndTriangleRec.NumEdgeTriangle = 1;
                    }
                    else if ((triangle.SecondVertex == _triangles[i].SecondVertex && triangle.ThirdVertex == _triangles[i].ThirdVertex && triangle.FirstVertex != _triangles[i].FirstVertex) ||
                        (triangle.SecondVertex == _triangles[i].ThirdVertex && triangle.ThirdVertex == _triangles[i].SecondVertex && triangle.FirstVertex != _triangles[i].FirstVertex))
                    {
                        //2=2
                        edgesAndTriangleRec.NumTriangle = i;
                        edgesAndTriangleRec.NumEdgeTriangle = 2;
                    }
                    else if ((triangle.SecondVertex == _triangles[i].ThirdVertex && triangle.ThirdVertex == _triangles[i].FirstVertex && triangle.FirstVertex != _triangles[i].SecondVertex) ||
                       (triangle.SecondVertex == _triangles[i].FirstVertex && triangle.ThirdVertex == _triangles[i].ThirdVertex && triangle.FirstVertex != _triangles[i].SecondVertex))
                    {
                        //2=3
                        edgesAndTriangleRec.NumTriangle = i;
                        edgesAndTriangleRec.NumEdgeTriangle = 3;
                    }
                }
                else if (indexEdge == 3)
                {
                    if ((triangle.ThirdVertex == _triangles[i].FirstVertex && triangle.FirstVertex == _triangles[i].SecondVertex && triangle.SecondVertex != _triangles[i].ThirdVertex) ||
                        (triangle.ThirdVertex == _triangles[i].SecondVertex && triangle.FirstVertex == _triangles[i].FirstVertex && triangle.SecondVertex != _triangles[i].ThirdVertex))
                    {
                        //3=1
                        edgesAndTriangleRec.NumTriangle = i;
                        edgesAndTriangleRec.NumEdgeTriangle = 1;
                    }
                    else if ((triangle.ThirdVertex == _triangles[i].SecondVertex && triangle.FirstVertex == _triangles[i].ThirdVertex && triangle.SecondVertex != _triangles[i].FirstVertex) ||
                       (triangle.ThirdVertex == _triangles[i].ThirdVertex && triangle.FirstVertex == _triangles[i].SecondVertex && triangle.SecondVertex != _triangles[i].FirstVertex))
                    {
                        //3=2
                        edgesAndTriangleRec.NumTriangle = i;
                        edgesAndTriangleRec.NumEdgeTriangle = 2;
                    }
                    else if ((triangle.ThirdVertex == _triangles[i].ThirdVertex && triangle.FirstVertex == _triangles[i].FirstVertex && triangle.SecondVertex != _triangles[i].SecondVertex) ||
                       (triangle.ThirdVertex == _triangles[i].FirstVertex && triangle.FirstVertex == _triangles[i].ThirdVertex && triangle.SecondVertex != _triangles[i].SecondVertex))
                    {
                        //3=3
                        edgesAndTriangleRec.NumTriangle = i;
                        edgesAndTriangleRec.NumEdgeTriangle = 3;
                    }
                }
            }

            return edgesAndTriangleRec;
        }
        private bool PointInCircleNeighborTriangle(ref EdgeAndTriangle neighborTriangle, ref Triangle triangle)
        {
            double radius = Mathematics.GetProductSidesTriangle(triangle) / (4 * Mathematics.GetSquareTriangle(triangle));

            if (neighborTriangle.NumEdgeTriangle == 1)
            {
                if (Mathematics.GetDistancePoints(_triangles[neighborTriangle.NumTriangle].ThirdVertex, Mathematics.GetCenterPointCircleTriangle(triangle)) <= radius)
                {
                    return true;
                }
            }
            else if (neighborTriangle.NumEdgeTriangle == 2)
            {
                if (Mathematics.GetDistancePoints(_triangles[neighborTriangle.NumTriangle].FirstVertex, Mathematics.GetCenterPointCircleTriangle(triangle)) <= radius)
                {
                    return true;
                }
            }
            else if (neighborTriangle.NumEdgeTriangle == 3)
            {
                if (Mathematics.GetDistancePoints(_triangles[neighborTriangle.NumTriangle].SecondVertex, Mathematics.GetCenterPointCircleTriangle(triangle)) <= radius)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
