using OpenTK.Mathematics;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;

namespace MKE
{
    public struct PointD : IEquatable<PointD>
    {
        /// <summary>
        /// Creates a new instance of the <see cref='MKE.PointD'/> class with member data left uninitialized.
        /// </summary>
        public static readonly PointD Empty;
        private double x; // Do not rename (binary serialization)
        private double y; // Do not rename (binary serialization)

        /// <summary>
        /// Initializes a new instance of the <see cref='MKE.PointD'/> class with the specified coordinates.
        /// </summary>
        public PointD(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref='MKE.PointD'/> struct from the specified
        /// <see cref="OpenTK.Mathematics.Vector2d"/>.
        /// </summary>
        public PointD(Vector2d vector)
        {
            x = vector.X;
            y = vector.Y;
        }

        /// <summary>
        /// Creates a new <see cref="OpenTK.Mathematics.Vector2d"/> from this <see cref="MKE.PointD"/>.
        /// </summary>

        public readonly Vector2d ToVector2d() => new(x, y);
        /// <summary>
        /// Gets a value indicating whether this <see cref='MKE.PointD'/> is empty.
        /// </summary>
        [Browsable(false)]
        public readonly bool IsEmpty => x == 0d && y == 0d;

        /// <summary>
        /// Gets the x-coordinate of this <see cref='MKE.PointD'/>.
        /// </summary>
        public double X
        {
            readonly get => x;
            set => x = value;
        }

        /// <summary>
        /// Gets the y-coordinate of this <see cref='MKE.PointD'/>.
        /// </summary>
        public double Y
        {
            readonly get => y;
            set => y = value;
        }

        /// <summary>
        /// Converts the specified <see cref="MKE.PointD"/> to a <see cref="OpenTK.Mathematics.Vector2d"/>.
        /// </summary>


        /// <summary>
        /// Converts the specified <see cref="OpenTK.Mathematics.Vector2d"/> to a <see cref="MKE.PointD"/>.
        /// </summary>
        public static explicit operator PointD(Vector2d vector) => new(vector);

        /// <summary>
        /// Translates a <see cref='MKE.PointD'/> by a given <see cref='System.Drawing.Size'/> .
        /// </summary>
        public static PointD operator +(PointD pt, Size sz) => Add(pt, sz);

        /// <summary>
        /// Translates a <see cref='MKE.PointD'/> by the negative of a given <see cref='System.Drawing.Size'/> .
        /// </summary>
        public static PointD operator -(PointD pt, Size sz) => Subtract(pt, sz);

        /// <summary>
        /// Translates a <see cref='MKE.PointD'/> by a given <see cref='System.Drawing.SizeF'/> .
        /// </summary>
        public static PointD operator +(PointD pt, SizeF sz) => Add(pt, sz);

        /// <summary>
        /// Translates a <see cref='MKE.PointD'/> by the negative of a given <see cref='System.Drawing.SizeF'/> .
        /// </summary>
        public static PointD operator -(PointD pt, SizeF sz) => Subtract(pt, sz);

        /// <summary>
        /// Compares two <see cref='MKE.PointD'/> objects. The result specifies whether the values of the
        /// <see cref='MKE.PointD.X'/> and <see cref='MKE.PointD.Y'/> properties of the two
        /// <see cref='MKE.PointD'/> objects are equal.
        /// </summary>
        public static bool operator ==(PointD left, PointD right) => left.X == right.X && left.Y == right.Y;

        /// <summary>
        /// Compares two <see cref='MKE.PointD'/> objects. The result specifies whether the values of the
        /// <see cref='MKE.PointD.X'/> or <see cref='MKE.PointD.Y'/> properties of the two
        /// <see cref='MKE.PointD'/> objects are unequal.
        /// </summary>
        public static bool operator !=(PointD left, PointD right) => !(left == right);

        /// <summary>
        /// Translates a <see cref='MKE.PointD'/> by a given <see cref='System.Drawing.Size'/> .
        /// </summary>
        public static PointD Add(PointD pt, Size sz) => new(pt.X + sz.Width, pt.Y + sz.Height);

        /// <summary>
        /// Translates a <see cref='MKE.PointD'/> by the negative of a given <see cref='System.Drawing.Size'/> .
        /// </summary>
        public static PointD Subtract(PointD pt, Size sz) => new(pt.X - sz.Width, pt.Y - sz.Height);

        /// <summary>
        /// Translates a <see cref='MKE.PointD'/> by a given <see cref='System.Drawing.SizeF'/> .
        /// </summary>
        public static PointD Add(PointD pt, SizeF sz) => new(pt.X + sz.Width, pt.Y + sz.Height);

        /// <summary>
        /// Translates a <see cref='MKE.PointD'/> by the negative of a given <see cref='System.Drawing.SizeF'/> .
        /// </summary>
        public static PointD Subtract(PointD pt, SizeF sz) => new(pt.X - sz.Width, pt.Y - sz.Height);

        public override readonly bool Equals([NotNullWhen(true)] object? obj) => obj is PointD && Equals((PointD)obj);

        public readonly bool Equals(PointD other) => this == other;

        public override readonly int GetHashCode() => HashCode.Combine(X.GetHashCode(), Y.GetHashCode());

        public override readonly string ToString() => $"{{X={x}, Y={y}}}";
    }
    /// <summary>
    ///Структура "Треугольник", содержащая в себе вершины треугольника и их номера
    /// </summary>
    public struct Triangle
    {
        public PointD vertex1;
        public PointD vertex2;
        public PointD vertex3;

        public int vertex1NumNodes;
        public int vertex2NumNodes;
        public int vertex3NumNodes;
    }
    /// <summary>
    ///Структура, содержащая в себе номер треугольника и его ребра
    /// </summary>
    public struct EdgesAndTriangle
    {
        public int NumTriangle;
        public int NumEdge;
    }
    /// <summary>
    /// Структура, содержащая в себе номер треугольника и номер его вершины
    /// </summary>
    public struct VertexAndTriangle
    {
        public int NumTriangle;
        public int NumVertex;
    }
    /// <summary>
    ///Структура узлов сетки, содержащая номер узла и точку с координатами этого узла
    /// </summary>
    public struct Nodes
    {
        public PointD Node;
        public int NumNode;

        public Nodes(PointD Node, int NumNode)
        {
            this.Node = Node;
            this.NumNode = NumNode;
        }
    }
    /// <summary>
    /// В данном классе выполняется триангуляция области.
    /// </summary>
    public class Triangulation
    {
        private int SizeGridChoice { get; set; }
        /// <summary>
        /// Начальный массив точек области
        /// </summary>
        private PointD[] Points { get; set; }
        /// <summary>
        /// Список структур треугольников триангуляции Делоне
        /// </summary>
        public List<Triangle> Triangles { get; private set; }
        /// <summary>
        /// Узлы сетки
        /// </summary>
        /// 
        public List<Nodes> Nodes { get; private set; }
        public Triangulation(PointD[] points, int sizeGridChoice)
        {
            SizeGridChoice = sizeGridChoice;
            Points = points;
            Triangles = new();
            Nodes = new();
        }

        /// <summary>
        /// Разбивает стороны области пополам и выполняет разбиение на треугольники по наименьшему углу между сторонами области.
        /// </summary>
        public void InitialPartitioning()
        {
            PointD[] vectors;
            Triangle tempTriangle = new();

            int iFirstIndex, iSecondIndex, iThirdIndex;
            int jFirstIndex, jSecondIndex, jThirdIndex;

            int foundIndex = 0;

            double currentAngle, minAngle = 360;

             List<PointD> splitPoints = MiddleSplitEdges();

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

                    vectors = GetCoordinatesVectors(jFirstIndex, jSecondIndex, jThirdIndex, ref splitPoints);
                    currentAngle = GetAngleVectors(vectors[0], vectors[1]);

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

                tempTriangle.vertex1 = splitPoints[iFirstIndex];
                tempTriangle.vertex2 = splitPoints[iSecondIndex];
                tempTriangle.vertex3 = splitPoints[iThirdIndex];

                Triangles.Add(tempTriangle);

               FlipBadEdges(Triangles[Triangles.Count - 1], VertexMaxAngle(Triangles[Triangles.Count - 1]));

                if (RemovePoint(splitPoints[iSecondIndex], Triangles, splitPoints))
                {
                    splitPoints.RemoveAt(iSecondIndex);
                }
                minAngle = 360;
            }
        }
        /// <summary>
        /// Выполняет триангуляцию Делоне ..........
        /// </summary>
        public void DelaunayTriangulation()
        {
            PointD midPoint = new();
            double hInitial = ShortestEdgeTriagnle();      

            step_two:;
            int indexBadTriangle = FindIndexBadTriangle(hInitial);

            if (indexBadTriangle != -1)
            {
                for (int i = 0; i < Triangles.Count; i++)
                {
                    if (PointIsDiametralCircle(Triangles[indexBadTriangle], Triangles[i].vertex1, Triangles[i].vertex2))
                    {
                        midPoint.X = (Triangles[i].vertex1.X + Triangles[i].vertex2.X) / 2;
                        midPoint.Y = (Triangles[i].vertex1.Y + Triangles[i].vertex2.Y) / 2;

                        AddPoint(midPoint);

                        goto step_two;
                    }
                    else if (PointIsDiametralCircle(Triangles[indexBadTriangle], Triangles[i].vertex2, Triangles[i].vertex3))
                    {
                        midPoint.X = (Triangles[i].vertex2.X + Triangles[i].vertex3.X) / 2;
                        midPoint.Y = (Triangles[i].vertex2.Y + Triangles[i].vertex3.Y) / 2;

                        AddPoint(midPoint);

                        goto step_two;
                    }
                    else if (PointIsDiametralCircle(Triangles[indexBadTriangle], Triangles[i].vertex3, Triangles[i].vertex1))
                    {
                        midPoint.X = (Triangles[i].vertex3.X + Triangles[i].vertex1.X) / 2;
                        midPoint.Y = (Triangles[i].vertex3.Y + Triangles[i].vertex1.Y) / 2;

                        AddPoint(midPoint);

                        goto step_two;
                    }
                }

                if (!PointBeyondArea(GetCenterPointCircleTriangle(Triangles[indexBadTriangle])))
                {
                    AddPoint(GetCenterPointCircleTriangle(Triangles[indexBadTriangle]));
                }
                else
                {
                    SplitHeightTriangle(Triangles[indexBadTriangle]);
                }

                goto step_two;
            }
        }
        /// <summary>
        /// Нумерует узлы, полученной триангуляции Делоне
        /// </summary>
        public void DeterminingNodeNumbers()
        {            
            List<PointD> allPoints = new();
            int countNodes = 1;
            for (int i = 0; i < Triangles.Count; i++)
            {
                if (!ExistNode(Triangles[i].vertex1, ref allPoints))
                {
                    allPoints.Add(Triangles[i].vertex1);
                }

                if (!ExistNode(Triangles[i].vertex2, ref allPoints))
                {
                    allPoints.Add(Triangles[i].vertex2);
                }

                if (!ExistNode(Triangles[i].vertex3, ref allPoints))
                {
                    allPoints.Add(Triangles[i].vertex3);
                }
            }
           
            while (allPoints.Count > 0)
            {
                countNodes = IterationNumbering(allPoints, countNodes);
            }

            EqualityVertexAndNodes();
        }
        /// <summary>
        /// Разделяет ребра области, вставляя точки по середине ребра.
        /// </summary>
        /// <returns></returns>
        private List<PointD> MiddleSplitEdges()
        {
            PointD midPoint = new();
            List<PointD> splitPoints = new();
            PointD startPoint = Points[Points.Length - 1];
            
            for (int i = 0; i < Points.Length; i++)
            {
                PointD endPoint = Points[i];

                midPoint.X = (startPoint.X + endPoint.X) / 2;
                midPoint.Y = (startPoint.Y + endPoint.Y) / 2;

                splitPoints.Add(startPoint);
                splitPoints.Add(midPoint);             

                startPoint = endPoint;
            }

            return splitPoints;
        }
        /// <summary>
        /// Удаляет текущую точку из рассматриваемых точек, если она больше не может использоваться. Я НЕ ЗНАЮ КАК ЭТО РАБОТАЕТ!!!!
        /// </summary>
        /// <param name="CurrentPoint"></param>
        /// <param name="Triangles"></param>
        /// <returns></returns>
        private static  bool RemovePoint(PointD currentPoint, List<Triangle> triangles, List<PointD> splitPoints)
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

                            if (splitPoints[count] == triangles[j].vertex1 || splitPoints[count] == triangles[j].vertex2 || splitPoints[count] == triangles[j].vertex3)
                            {
                                flagO = true;
                            }
                        }
                        else
                        {
                            if (splitPoints[i - 1] == triangles[j].vertex1 || splitPoints[i - 1] == triangles[j].vertex2 || splitPoints[i - 1] == triangles[j].vertex3)
                            {
                                flagO = true;
                            }
                        }

                        if (i == splitPoints.Count - 1)
                        {
                            count = 0;
                            if (splitPoints[count] == triangles[j].vertex1 || splitPoints[count] == triangles[j].vertex2 || splitPoints[count] == triangles[j].vertex3)
                            {
                                flagN = true;
                            }
                        }
                        else
                        {
                            if (splitPoints[i + 1] == triangles[j].vertex1 || splitPoints[i + 1] == triangles[j].vertex2 || splitPoints[i + 1] == triangles[j].vertex3)
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
        /// <summary>
        ///Находит и возвращает индекс самого плохого треугольника или -1 в противном случае
        /// </summary>
        /// <param name="B"></param>
        /// <param name="h"></param>
        /// <param name="XOY"></param>
        private int FindIndexBadTriangle(double hInitial)
        {
            double h, desiredSumSides, B = Math.Sqrt(2);
            double maxSumSides = 0;
            int indexBadTriangle = -1;

            if (SizeGridChoice == 1)
            {
                h = hInitial;
                desiredSumSides = 3;
            }
            else if (SizeGridChoice == 2)
            {
                h = hInitial / 3;
                desiredSumSides = 2;
            }
            else
            {
                h = hInitial / 5;
                desiredSumSides = 1;
            }

            for (int i = 0; i < Triangles.Count; i++)
            {
                if (GetCircumRadiusShortestEdgeRatio(Triangles[i]) > B && GetSumtSidesTriangle(Triangles[i]) > desiredSumSides)
                {
                   
                }
                else if (GetCircumRadius(Triangles[i]) > h && ShortestEdgeTriagnle(Triangles[i]) > h && GetSumtSidesTriangle(Triangles[i]) > desiredSumSides)
                {
                    
                }
                else
                {
                    continue;
                }

                if (GetSumtSidesTriangle(Triangles[i]) > maxSumSides)
                {
                    maxSumSides = GetSumtSidesTriangle(Triangles[i]);
                    indexBadTriangle = i;
                }
            }
            
            return indexBadTriangle;
        }
        /// <summary>
        /// Добавляет точку в сетку
        /// </summary>
        /// <param name="addedPoint"></param>
        private void AddPoint(PointD addedPoint)
        {
            List<EdgesAndTriangle> edgesAndTriangles = PointOnSideTriangle(addedPoint);

            if (edgesAndTriangles.Count != 0)
            {
                for (int i = 0; i < edgesAndTriangles.Count; i++)
                {
                    SplitNeighborTriangles(edgesAndTriangles[i].NumEdge, edgesAndTriangles[i].NumTriangle, addedPoint);
                }

                if (edgesAndTriangles.Count != 1)
                {
                    if (edgesAndTriangles[0].NumTriangle > edgesAndTriangles[1].NumTriangle)
                    {
                        Triangles.RemoveAt(edgesAndTriangles[0].NumTriangle);
                        Triangles.RemoveAt(edgesAndTriangles[1].NumTriangle);
                    }
                    else
                    {
                        Triangles.RemoveAt(edgesAndTriangles[1].NumTriangle);
                        Triangles.RemoveAt(edgesAndTriangles[0].NumTriangle);
                    }

                    FlipBadEdges(Triangles[Triangles.Count - 4], addedPoint);
                    FlipBadEdges(Triangles[Triangles.Count - 3], addedPoint);
                    FlipBadEdges(Triangles[Triangles.Count - 2], addedPoint);
                    FlipBadEdges(Triangles[Triangles.Count - 1], addedPoint);
                }
                else
                {
                    Triangles.RemoveAt(edgesAndTriangles[0].NumTriangle);

                    FlipBadEdges(Triangles[Triangles.Count - 2], addedPoint);
                    FlipBadEdges(Triangles[Triangles.Count - 1], addedPoint);
                }
            }
            else
            {
                for (int i = 0; i < Triangles.Count; i++)
                {
                    if (PointOnTriangle(Triangles[i], addedPoint))
                    {
                        SplitTriangle(i, addedPoint);
                        Triangles.RemoveAt(i);

                        FlipBadEdges(Triangles[Triangles.Count - 3], addedPoint);
                        FlipBadEdges(Triangles[Triangles.Count - 2], addedPoint);
                        FlipBadEdges(Triangles[Triangles.Count - 1], addedPoint);
                        break;
                    }
                }              
            }
        }
        /// <summary>
        /// Переворачивает общее ребро соседних треугольников
        /// </summary>
        /// <param name="triangle"></param>
        /// <param name="newPoint"></param>
        private void FlipBadEdges(Triangle triangle, PointD newPoint)
        {
            Triangle[] tempTriangle = new Triangle[2];

            //ребро в новом треугольнике, которое не содержит NewPoint
            int commonEdgeIndex = NumberEdgeTriangle(triangle, newPoint);

            //номер другого треугольника и его общее ребро с новым треугольником
            EdgesAndTriangle edgesAndTriangleNeighbor = NeighborTriangle(commonEdgeIndex, ref triangle);
        
            if (edgesAndTriangleNeighbor.NumEdge != 0)
            {
                if (PointInCircleNeighborTriangle(ref edgesAndTriangleNeighbor, ref triangle))
                {
                    if (commonEdgeIndex == 1)
                    {
                        if (edgesAndTriangleNeighbor.NumEdge == 1)
                        {
                            tempTriangle[0].vertex1 = newPoint;
                            tempTriangle[0].vertex2 = triangle.vertex1;
                            tempTriangle[0].vertex3 = Triangles[edgesAndTriangleNeighbor.NumTriangle].vertex3;

                            Triangles.Add(tempTriangle[0]);

                            tempTriangle[1].vertex1 = Triangles[edgesAndTriangleNeighbor.NumTriangle].vertex3;
                            tempTriangle[1].vertex2 = triangle.vertex2;
                            tempTriangle[1].vertex3 = newPoint;

                            Triangles.Add(tempTriangle[1]);

                            Triangles.Remove(triangle);
                            Triangles.Remove(Triangles[edgesAndTriangleNeighbor.NumTriangle]);

                            FlipBadEdges(Triangles[Triangles.Count - 2], newPoint);
                            FlipBadEdges(Triangles[Triangles.Count - 1], newPoint);
                        }
                        else if (edgesAndTriangleNeighbor.NumEdge == 2)
                        {
                            tempTriangle[0].vertex1 = newPoint;
                            tempTriangle[0].vertex2 = triangle.vertex1;
                            tempTriangle[0].vertex3 = Triangles[edgesAndTriangleNeighbor.NumTriangle].vertex1;

                            Triangles.Add(tempTriangle[0]);

                            tempTriangle[1].vertex1 = Triangles[edgesAndTriangleNeighbor.NumTriangle].vertex1;
                            tempTriangle[1].vertex2 = triangle.vertex2;
                            tempTriangle[1].vertex3 = newPoint;

                            Triangles.Add(tempTriangle[1]);

                            Triangles.Remove(triangle);
                            Triangles.Remove(Triangles[edgesAndTriangleNeighbor.NumTriangle]);

                            FlipBadEdges(Triangles[Triangles.Count - 2], newPoint);
                            FlipBadEdges(Triangles[Triangles.Count - 1], newPoint);
                        }
                        else if (edgesAndTriangleNeighbor.NumEdge == 3)
                        {
                            tempTriangle[0].vertex1 = newPoint;
                            tempTriangle[0].vertex2 = triangle.vertex1;
                            tempTriangle[0].vertex3 = Triangles[edgesAndTriangleNeighbor.NumTriangle].vertex2;

                            Triangles.Add(tempTriangle[0]);

                            tempTriangle[1].vertex1 = Triangles[edgesAndTriangleNeighbor.NumTriangle].vertex2;
                            tempTriangle[1].vertex2 = triangle.vertex2;
                            tempTriangle[1].vertex3 = newPoint;

                            Triangles.Add(tempTriangle[1]);

                            Triangles.Remove(triangle);
                            Triangles.Remove(Triangles[edgesAndTriangleNeighbor.NumTriangle]);

                            FlipBadEdges(Triangles[Triangles.Count - 2], newPoint);
                            FlipBadEdges(Triangles[Triangles.Count - 1], newPoint);
                        }
                    }
                    else if (commonEdgeIndex == 2)
                    {
                        if (edgesAndTriangleNeighbor.NumEdge == 1)
                        {
                            tempTriangle[0].vertex1 = newPoint;
                            tempTriangle[0].vertex2 = triangle.vertex2;
                            tempTriangle[0].vertex3 = Triangles[edgesAndTriangleNeighbor.NumTriangle].vertex3;

                            Triangles.Add(tempTriangle[0]);

                            tempTriangle[1].vertex1 = Triangles[edgesAndTriangleNeighbor.NumTriangle].vertex3;
                            tempTriangle[1].vertex2 = triangle.vertex3;
                            tempTriangle[1].vertex3 = newPoint;

                            Triangles.Add(tempTriangle[1]);

                            Triangles.Remove(triangle);
                            Triangles.Remove(Triangles[edgesAndTriangleNeighbor.NumTriangle]);

                            FlipBadEdges(Triangles[Triangles.Count - 2], newPoint);
                            FlipBadEdges(Triangles[Triangles.Count - 1], newPoint);
                        }
                        else if (edgesAndTriangleNeighbor.NumEdge == 2)
                        {
                            tempTriangle[0].vertex1 = newPoint;
                            tempTriangle[0].vertex2 = triangle.vertex2;
                            tempTriangle[0].vertex3 = Triangles[edgesAndTriangleNeighbor.NumTriangle].vertex1;

                            Triangles.Add(tempTriangle[0]);

                            tempTriangle[1].vertex1 = Triangles[edgesAndTriangleNeighbor.NumTriangle].vertex1;
                            tempTriangle[1].vertex2 = triangle.vertex3;
                            tempTriangle[1].vertex3 = newPoint;

                            Triangles.Add(tempTriangle[1]);

                            Triangles.Remove(triangle);
                            Triangles.Remove(Triangles[edgesAndTriangleNeighbor.NumTriangle]);

                            FlipBadEdges(Triangles[Triangles.Count - 2], newPoint);
                            FlipBadEdges(Triangles[Triangles.Count - 1], newPoint);
                        }
                        else if (edgesAndTriangleNeighbor.NumEdge == 3)
                        {
                            tempTriangle[0].vertex1 = newPoint;
                            tempTriangle[0].vertex2 = triangle.vertex2;
                            tempTriangle[0].vertex3 = Triangles[edgesAndTriangleNeighbor.NumTriangle].vertex2;

                            Triangles.Add(tempTriangle[0]);

                            tempTriangle[1].vertex1 = Triangles[edgesAndTriangleNeighbor.NumTriangle].vertex2;
                            tempTriangle[1].vertex2 = triangle.vertex3;
                            tempTriangle[1].vertex3 = newPoint;

                            Triangles.Add(tempTriangle[1]);

                            Triangles.Remove(triangle);
                            Triangles.Remove(Triangles[edgesAndTriangleNeighbor.NumTriangle]);

                            FlipBadEdges(Triangles[Triangles.Count - 2], newPoint);
                            FlipBadEdges(Triangles[Triangles.Count - 1], newPoint);
                        }
                    }
                    else if (commonEdgeIndex == 3)
                    {
                        if (edgesAndTriangleNeighbor.NumEdge == 1)
                        {
                            tempTriangle[0].vertex1 = newPoint;
                            tempTriangle[0].vertex2 = triangle.vertex3;
                            tempTriangle[0].vertex3 = Triangles[edgesAndTriangleNeighbor.NumTriangle].vertex3;

                            Triangles.Add(tempTriangle[0]);

                            tempTriangle[1].vertex1 = Triangles[edgesAndTriangleNeighbor.NumTriangle].vertex3;
                            tempTriangle[1].vertex2 = triangle.vertex1;
                            tempTriangle[1].vertex3 = newPoint;

                            Triangles.Add(tempTriangle[1]);

                            Triangles.Remove(triangle);
                            Triangles.Remove(Triangles[edgesAndTriangleNeighbor.NumTriangle]);

                            FlipBadEdges(Triangles[Triangles.Count - 2], newPoint);
                            FlipBadEdges(Triangles[Triangles.Count - 1], newPoint);
                        }
                        else if (edgesAndTriangleNeighbor.NumEdge == 2)
                        {
                            tempTriangle[0].vertex1 = newPoint;
                            tempTriangle[0].vertex2 = triangle.vertex3;
                            tempTriangle[0].vertex3 = Triangles[edgesAndTriangleNeighbor.NumTriangle].vertex1;

                            Triangles.Add(tempTriangle[0]);

                            tempTriangle[1].vertex1 = Triangles[edgesAndTriangleNeighbor.NumTriangle].vertex1;
                            tempTriangle[1].vertex2 = triangle.vertex1;
                            tempTriangle[1].vertex3 = newPoint;

                            Triangles.Add(tempTriangle[1]);

                            Triangles.Remove(triangle);
                            Triangles.Remove(Triangles[edgesAndTriangleNeighbor.NumTriangle]);

                            FlipBadEdges(Triangles[Triangles.Count - 2], newPoint);
                            FlipBadEdges(Triangles[Triangles.Count - 1], newPoint);
                        }
                        else if (edgesAndTriangleNeighbor.NumEdge == 3)
                        {
                            tempTriangle[0].vertex1 = newPoint;
                            tempTriangle[0].vertex2 = triangle.vertex3;
                            tempTriangle[0].vertex3 = Triangles[edgesAndTriangleNeighbor.NumTriangle].vertex2;

                            Triangles.Add(tempTriangle[0]);

                            tempTriangle[1].vertex1 = Triangles[edgesAndTriangleNeighbor.NumTriangle].vertex2;
                            tempTriangle[1].vertex2 = triangle.vertex1;
                            tempTriangle[1].vertex3 = newPoint;

                            Triangles.Add(tempTriangle[1]);

                            Triangles.Remove(triangle);
                            Triangles.Remove(Triangles[edgesAndTriangleNeighbor.NumTriangle]);

                            FlipBadEdges(Triangles[Triangles.Count - 2], newPoint);
                            FlipBadEdges(Triangles[Triangles.Count - 1], newPoint); ;
                        }
                    }
                }
            }
        }
        /// <summary>
        ///  Если точка лежит на ребре, возвращает структуру, содержащую номер ребра и треугольника или null, если не лежит.
        /// </summary>
        /// <param name="Point"></param>
        /// <returns></returns>
        private List<EdgesAndTriangle> PointOnSideTriangle(PointD point)
        {
            EdgesAndTriangle temp = new();
            List<EdgesAndTriangle> edgesAndTriangles = new();
          
            for (int i = 0; i < Triangles.Count; i++)
            {
                if (point != Triangles[i].vertex1 && point != Triangles[i].vertex2 && point != Triangles[i].vertex3)
                {
                    if (Math.Abs(GetDistancePoints(Triangles[i].vertex1, point) + GetDistancePoints(Triangles[i].vertex2, point) - GetDistancePoints(Triangles[i].vertex1, Triangles[i].vertex2)) == 0)
                    {
                        temp.NumTriangle = i;
                        temp.NumEdge = 1;

                        if (!AddEdgesAndTriangles(edgesAndTriangles, temp))
                        {
                            edgesAndTriangles.Add(temp);
                        }
                    }
                    else if (Math.Abs(GetDistancePoints(Triangles[i].vertex2, point) + GetDistancePoints(Triangles[i].vertex3, point) - GetDistancePoints(Triangles[i].vertex2, Triangles[i].vertex3)) == 0)
                    {
                        temp.NumTriangle = i;
                        temp.NumEdge = 2;

                        if (!AddEdgesAndTriangles(edgesAndTriangles, temp))
                        {
                            edgesAndTriangles.Add(temp);
                        }
                    }
                    else if (Math.Abs(GetDistancePoints(Triangles[i].vertex3, point) + GetDistancePoints(Triangles[i].vertex1, point) - GetDistancePoints(Triangles[i].vertex3, Triangles[i].vertex1)) == 0)
                    {
                        temp.NumTriangle = i;
                        temp.NumEdge = 3;

                        if (!AddEdgesAndTriangles(edgesAndTriangles, temp))
                        {
                            edgesAndTriangles.Add(temp);
                        }
                    }
                }
            }
    
            return edgesAndTriangles;                  
        }
        private void SplitNeighborTriangles(int numEdge, int numTriangle, PointD addedPoint)
        {
            Triangle tempTriangle = new();
            if (numEdge == 1)
            {
                tempTriangle.vertex1 = Triangles[numTriangle].vertex1;
                tempTriangle.vertex2 = addedPoint;
                tempTriangle.vertex3 = Triangles[numTriangle].vertex3;

                Triangles.Add(tempTriangle);

                tempTriangle.vertex1 = addedPoint;
                tempTriangle.vertex2 = Triangles[numTriangle].vertex2;
                tempTriangle.vertex3 = Triangles[numTriangle].vertex3;

                Triangles.Add(tempTriangle);
            }
            else if (numEdge == 2)
            {
                tempTriangle.vertex1 = Triangles[numTriangle].vertex1;
                tempTriangle.vertex2 = Triangles[numTriangle].vertex2;
                tempTriangle.vertex3 = addedPoint;

                Triangles.Add(tempTriangle);

                tempTriangle.vertex1 = addedPoint;
                tempTriangle.vertex2 = Triangles[numTriangle].vertex3;
                tempTriangle.vertex3 = Triangles[numTriangle].vertex1;

                Triangles.Add(tempTriangle);
            }
            else if (numEdge == 3)
            {
                tempTriangle.vertex1 = Triangles[numTriangle].vertex1;
                tempTriangle.vertex2 = Triangles[numTriangle].vertex2;
                tempTriangle.vertex3 = addedPoint;

                Triangles.Add(tempTriangle);

                tempTriangle.vertex1 = addedPoint;
                tempTriangle.vertex2 = Triangles[numTriangle].vertex2;
                tempTriangle.vertex3 = Triangles[numTriangle].vertex3;

                Triangles.Add(tempTriangle);
            }
        }
        private void SplitTriangle(int numTriangle, PointD addedPoint)
        {
            Triangle tempTriangle = new();
            tempTriangle.vertex1 = Triangles[numTriangle].vertex1;
            tempTriangle.vertex2 = Triangles[numTriangle].vertex2;
            tempTriangle.vertex3 = addedPoint;

            Triangles.Add(tempTriangle);

            tempTriangle.vertex1 = Triangles[numTriangle].vertex1;
            tempTriangle.vertex2 = addedPoint;
            tempTriangle.vertex3 = Triangles[numTriangle].vertex3;

            Triangles.Add(tempTriangle);

            tempTriangle.vertex1 = addedPoint;
            tempTriangle.vertex2 = Triangles[numTriangle].vertex2;
            tempTriangle.vertex3 = Triangles[numTriangle].vertex3;

            Triangles.Add(tempTriangle);
        }
        private static bool AddEdgesAndTriangles(List<EdgesAndTriangle> edgesAndTriangles, EdgesAndTriangle edgesAndTriangle)
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
        private EdgesAndTriangle NeighborTriangle(int indexEdge, ref Triangle triangle)
        {
            EdgesAndTriangle edgesAndTriangleRec = new();

            for (int i = 0; i < Triangles.Count; i++)
            {
                if (indexEdge == 1)
                {
                    if ((triangle.vertex1 == Triangles[i].vertex1 && triangle.vertex2 == Triangles[i].vertex2 && triangle.vertex3 != Triangles[i].vertex3) |
                       (triangle.vertex1 == Triangles[i].vertex2 && triangle.vertex2 == Triangles[i].vertex1 && triangle.vertex3 != Triangles[i].vertex3))
                    {
                        //1=1
                        edgesAndTriangleRec.NumTriangle = i;
                        edgesAndTriangleRec.NumEdge = 1;
                        
                    }
                    else if ((triangle.vertex1 == Triangles[i].vertex2 && triangle.vertex2 == Triangles[i].vertex3 && triangle.vertex3 != Triangles[i].vertex1) ||
                        (triangle.vertex1 == Triangles[i].vertex3 && triangle.vertex2 == Triangles[i].vertex2 && triangle.vertex3 != Triangles[i].vertex1))
                    {
                        //1=2
                        edgesAndTriangleRec.NumTriangle = i;
                        edgesAndTriangleRec.NumEdge = 2;
                    }
                    else if ((triangle.vertex1 == Triangles[i].vertex3 && triangle.vertex2 == Triangles[i].vertex1 && triangle.vertex3 != Triangles[i].vertex2) ||
                        (triangle.vertex1 == Triangles[i].vertex1 && triangle.vertex2 == Triangles[i].vertex3 && triangle.vertex3 != Triangles[i].vertex2))
                    {
                        //1=3
                        edgesAndTriangleRec.NumTriangle = i;
                        edgesAndTriangleRec.NumEdge = 3;
                    }
                }
                else if (indexEdge == 2)
                {
                    if ((triangle.vertex2 == Triangles[i].vertex1 && triangle.vertex3 == Triangles[i].vertex2 && triangle.vertex1 != Triangles[i].vertex3) ||
                        (triangle.vertex2 == Triangles[i].vertex2 && triangle.vertex3 == Triangles[i].vertex1 && triangle.vertex1 != Triangles[i].vertex3))
                    {
                        //2=1
                        edgesAndTriangleRec.NumTriangle = i;
                        edgesAndTriangleRec.NumEdge = 1;
                    }
                    else if ((triangle.vertex2 == Triangles[i].vertex2 && triangle.vertex3 == Triangles[i].vertex3 && triangle.vertex1 != Triangles[i].vertex1) ||
                        (triangle.vertex2 == Triangles[i].vertex3 && triangle.vertex3 == Triangles[i].vertex2 && triangle.vertex1 != Triangles[i].vertex1))
                    {
                        //2=2
                        edgesAndTriangleRec.NumTriangle = i;
                        edgesAndTriangleRec.NumEdge = 2;
                    }
                    else if ((triangle.vertex2 == Triangles[i].vertex3 && triangle.vertex3 == Triangles[i].vertex1 && triangle.vertex1 != Triangles[i].vertex2) ||
                       (triangle.vertex2 == Triangles[i].vertex1 && triangle.vertex3 == Triangles[i].vertex3 && triangle.vertex1 != Triangles[i].vertex2))
                    {
                        //2=3
                        edgesAndTriangleRec.NumTriangle = i;
                        edgesAndTriangleRec.NumEdge = 3;
                    }
                }
                else if (indexEdge == 3)
                {
                    if ((triangle.vertex3 == Triangles[i].vertex1 && triangle.vertex1 == Triangles[i].vertex2 && triangle.vertex2 != Triangles[i].vertex3) ||
                        (triangle.vertex3 == Triangles[i].vertex2 && triangle.vertex1 == Triangles[i].vertex1 && triangle.vertex2 != Triangles[i].vertex3))
                    {
                        //3=1
                        edgesAndTriangleRec.NumTriangle = i;
                        edgesAndTriangleRec.NumEdge = 1;
                    }
                    else if ((triangle.vertex3 == Triangles[i].vertex2 && triangle.vertex1 == Triangles[i].vertex3 && triangle.vertex2 != Triangles[i].vertex1) ||
                       (triangle.vertex3 == Triangles[i].vertex3 && triangle.vertex1 == Triangles[i].vertex2 && triangle.vertex2 != Triangles[i].vertex1))
                    {
                        //3=2
                        edgesAndTriangleRec.NumTriangle = i;
                        edgesAndTriangleRec.NumEdge = 2;
                    }
                    else if ((triangle.vertex3 == Triangles[i].vertex3 && triangle.vertex1 == Triangles[i].vertex1 && triangle.vertex2 != Triangles[i].vertex2) ||
                       (triangle.vertex3 == Triangles[i].vertex1 && triangle.vertex1 == Triangles[i].vertex3 && triangle.vertex2 != Triangles[i].vertex2))
                    {
                        //3=3
                        edgesAndTriangleRec.NumTriangle = i;
                        edgesAndTriangleRec.NumEdge = 3;
                    }
                }
            }

            return edgesAndTriangleRec;
        }
        /// <summary>
        /// Возвращает true, если точка не содержится внутри области и false, если находится внутри.
        /// </summary>
        /// <param name="addPoint"></param>
        /// <returns></returns>
        private bool PointBeyondArea(PointD addPoint)
        {
            List<EdgesAndTriangle> edgesAndTriangle = PointOnSideTriangle(addPoint);

            if (edgesAndTriangle.Count == 0)
            {
                for (int i = 0; i < Triangles.Count; i++)
                {
                    if (!PointOnTriangle(Triangles[i], addPoint))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        /// <summary>
        /// Разделяет треугольник на два, проводя высоту, в последствии удаляя изначальный.
        /// </summary>
        /// <param name="triangle"></param>
        private void SplitHeightTriangle(Triangle triangle)
        {
            Triangle tempTriangle = new();
            PointD newPoint = NewVertexTriangle(triangle);

            int numSide = RightAngleTriangle(triangle);
            
            if (numSide == 1)
            {
                tempTriangle.vertex1 = triangle.vertex1;
                tempTriangle.vertex2 = newPoint;
                tempTriangle.vertex3 = triangle.vertex3;

                Triangles.Add(tempTriangle);

                tempTriangle.vertex1 = newPoint;
                tempTriangle.vertex2 = triangle.vertex2;
                tempTriangle.vertex3 = triangle.vertex3;

                Triangles.Add(tempTriangle);

                Triangles.Remove(triangle);
            }
            else if (numSide == 2)
            {
                tempTriangle.vertex1 = triangle.vertex1;
                tempTriangle.vertex2 = triangle.vertex2;
                tempTriangle.vertex3 = newPoint;

                Triangles.Add(tempTriangle);

                tempTriangle.vertex1 = triangle.vertex1;
                tempTriangle.vertex2 = newPoint;
                tempTriangle.vertex3 = triangle.vertex3;

                Triangles.Add(tempTriangle);

                Triangles.Remove(triangle);
            }
            else if (numSide == 3)
            {
                tempTriangle.vertex1 = triangle.vertex1;
                tempTriangle.vertex2 = triangle.vertex2;
                tempTriangle.vertex3 = newPoint;

                Triangles.Add(tempTriangle);

                tempTriangle.vertex1 = newPoint;
                tempTriangle.vertex2 = triangle.vertex2;
                tempTriangle.vertex3 = triangle.vertex3;

                Triangles.Add(tempTriangle);

                Triangles.Remove(triangle);
            }
        }
        private static int RightAngleTriangle(Triangle triangle)
        {
            PointD[] vector;
            double firstAngle = 0, secondAngle = 0, thirdAngle = 0; // 1 и 3 / 1 и 2/  2 и 3

            for (int i = 1; i < 4; i++)
            {
                vector = GetVectorSideTriangle(triangle, i);

                if (i == 1)
                {
                    firstAngle = GetAngleVectors(vector[0], vector[1]);
                }
                else if (i == 2)
                {
                    secondAngle = GetAngleVectors(vector[0], vector[1]);
                }
                else
                {
                    thirdAngle = GetAngleVectors(vector[0], vector[1]);
                }
            }
            
            return (firstAngle >= secondAngle) ? ((firstAngle >= thirdAngle) ? (2) : (1)) : ((secondAngle >= thirdAngle) ? (3) : (1));
        }
        private static  PointD[] GetVectorSideTriangle(Triangle triangle, int sideIndex)
        {
            PointD[] vector = new PointD[2];

            if (sideIndex == 1)
            {
                vector[0].X = triangle.vertex2.X - triangle.vertex1.X;
                vector[0].Y = triangle.vertex2.Y - triangle.vertex1.Y;

                vector[1].X = triangle.vertex3.X - triangle.vertex1.X;
                vector[1].Y = triangle.vertex3.Y - triangle.vertex1.Y;
            }
            else if (sideIndex == 2)
            {
                vector[0].X = triangle.vertex1.X - triangle.vertex2.X;
                vector[0].Y = triangle.vertex1.Y - triangle.vertex2.Y;

                vector[1].X = triangle.vertex3.X - triangle.vertex2.X;
                vector[1].Y = triangle.vertex3.Y - triangle.vertex2.Y;
            }
            else if(sideIndex == 3)
            {
                vector[0].X = triangle.vertex1.X - triangle.vertex3.X;
                vector[0].Y = triangle.vertex1.Y - triangle.vertex3.Y;

                vector[1].X = triangle.vertex2.X - triangle.vertex3.X;
                vector[1].Y = triangle.vertex2.Y - triangle.vertex3.Y;
            }
            return vector;
        }
        private static PointD NewVertexTriangle(Triangle triangle)
        {
            double cat;
          
            int side = RightAngleTriangle(triangle);
            double h = 2 * GetSquareTriangle(triangle) / GetSideLenght(side, triangle);

            PointD vec = new();
            PointD newVertex = new();

            if (side == 1)
            {
                //2
                cat = Math.Sqrt(GetDistancePoints(triangle.vertex2, triangle.vertex3) * GetDistancePoints(triangle.vertex2, triangle.vertex3) - h * h);

                vec.X = (triangle.vertex1.X - triangle.vertex2.X) / (float)GetDistancePoints(triangle.vertex1, triangle.vertex2);
                vec.Y = (triangle.vertex1.Y - triangle.vertex2.Y) / (float)GetDistancePoints(triangle.vertex1, triangle.vertex2);

                newVertex.X = (float)(triangle.vertex2.X + vec.X * cat);
                newVertex.Y = (float)(triangle.vertex2.Y + vec.Y * cat);
            }
            else if (side == 2)
            {
                //3
                cat = Math.Sqrt(GetDistancePoints(triangle.vertex3, triangle.vertex1) * GetDistancePoints(triangle.vertex3, triangle.vertex1) - h * h);

                vec.X = (triangle.vertex2.X - triangle.vertex3.X) / (float)GetDistancePoints(triangle.vertex2, triangle.vertex3);
                vec.Y = (triangle.vertex2.Y - triangle.vertex3.Y) / (float)GetDistancePoints(triangle.vertex2, triangle.vertex3);

                newVertex.X = (float)(triangle.vertex3.X + vec.X * cat);
                newVertex.Y = (float)(triangle.vertex3.Y + vec.Y * cat);
            }
            else if (side == 3)
            {
                //1
                cat = Math.Sqrt(GetDistancePoints(triangle.vertex1, triangle.vertex2) * GetDistancePoints(triangle.vertex1, triangle.vertex2) - h * h);

                vec.X = (triangle.vertex3.X - triangle.vertex1.X) / (float)GetDistancePoints(triangle.vertex3, triangle.vertex1);
                vec.Y = (triangle.vertex3.Y - triangle.vertex1.Y) / (float)GetDistancePoints(triangle.vertex3, triangle.vertex1);

                newVertex.X = (float)(triangle.vertex1.X + vec.X * cat);
                newVertex.Y = (float)(triangle.vertex1.Y + vec.Y * cat);
            }

            return newVertex;
        }
        /// <summary>
        /// Возвращает длину указанной стороны треугольника.
        /// </summary>
        /// <param name="num"></param>
        /// <param name="triangle"></param>
        /// <returns></returns>
        private static double GetSideLenght(int num, Triangle triangle)
        {
            if (num == 1)
            {
                return GetDistancePoints(triangle.vertex1, triangle.vertex2);
            }
            else if (num == 2)
            {
                return GetDistancePoints(triangle.vertex2, triangle.vertex3);
            }
            else if (num == 3)
            {
                return GetDistancePoints(triangle.vertex3, triangle.vertex1);

            }
            return -1;
        }
        /// <summary>
        /// Возвращает координаты двух векторов, составленных по координатам переданных точек.
        /// </summary>
        /// <param name="firstIndex"></param>
        /// <param name="secondIndex"></param>
        /// <param name="thirdIndex"></param>
        /// <param name="Points"></param>
        /// <returns></returns>
        private static PointD[] GetCoordinatesVectors(int firstIndex, int secondIndex, int thirdIndex, ref List<PointD> points)
        {
            PointD[] vector = new PointD[2];

            vector[0].X = points[firstIndex].X - points[secondIndex].X;
            vector[0].Y = points[firstIndex].Y - points[secondIndex].Y;

            vector[1].X = points[thirdIndex].X - points[secondIndex].X;
            vector[1].Y = points[thirdIndex].Y - points[secondIndex].Y;

            return vector;
        }
        /// <summary>
        /// Возвращает угол между двумя векторами в градусах.
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        private static  double GetAngleVectors(PointD firstVector, PointD secondVector)
        {
            return ((Math.Acos((firstVector.X * secondVector.X + firstVector.Y * secondVector.Y) /
                        (Math.Sqrt((firstVector.X * firstVector.X + firstVector.Y * firstVector.Y) *
                        (secondVector.X * secondVector.X + secondVector.Y * secondVector.Y))))) * 180) / Math.PI;
        }
        /// <summary>
        /// Возвращает длину минимального ребра из всех треугольников.
        /// </summary>
        /// <param name="XOY"></param>
        /// <returns></returns>
        private double ShortestEdgeTriagnle()
        {
            double minEdge = 1000;

            for (int i = 1; i < Triangles.Count; i++)
            {
                if (GetDistancePoints(Triangles[i].vertex1, Triangles[i].vertex2) < minEdge)
                {
                    minEdge = GetDistancePoints(Triangles[i].vertex1, Triangles[i].vertex2);
                }

                if (GetDistancePoints(Triangles[i].vertex2, Triangles[i].vertex3) < minEdge)
                {
                    minEdge = GetDistancePoints(Triangles[i].vertex2, Triangles[i].vertex3);
                }

                if (GetDistancePoints(Triangles[i].vertex3, Triangles[i].vertex1) < minEdge)
                {
                    minEdge = GetDistancePoints(Triangles[i].vertex3, Triangles[i].vertex1);
                }
            }

            return minEdge;
        }
        /// <summary>
        /// Возвращает длину минимального ребра треугольника.
        /// </summary>
        /// <param name="XOY"></param>
        /// <returns></returns>
        private static double ShortestEdgeTriagnle(Triangle triangle)
        {
            double ShortestEdge = 1000;

            if (GetDistancePoints(triangle.vertex1, triangle.vertex2) < ShortestEdge)
            {
                ShortestEdge = GetDistancePoints(triangle.vertex1, triangle.vertex2);
            }

            if (GetDistancePoints(triangle.vertex2, triangle.vertex3) < ShortestEdge)
            {
                ShortestEdge = GetDistancePoints(triangle.vertex2, triangle.vertex3);
            }

            if (GetDistancePoints(triangle.vertex3, triangle.vertex1) < ShortestEdge)
            {
                ShortestEdge = GetDistancePoints(triangle.vertex3, triangle.vertex1);
            }

            return ShortestEdge;
        }
        /// <summary>
        /// Возвращает расстояние между двумя точками.
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="secondPoint"></param>
        /// <returns></returns>
        public static double GetDistancePoints(PointD firstPoint, PointD secondPoint)
        {
            return Math.Sqrt((firstPoint.X - secondPoint.X) * (firstPoint.X - secondPoint.X) + (firstPoint.Y - secondPoint.Y) * (firstPoint.Y - secondPoint.Y));
        }
        /// <summary>
        /// Возвращает радиус описанной окружности треугольника.
        /// </summary>
        /// <param name="triangle"></param>
        /// <returns></returns>
        private static double GetCircumRadius(Triangle triangle)
        {
            return (GetProductSidesTriangle(triangle) / (4 * GetSquareTriangle(triangle)));
        }
        /// <summary>
        /// Возвращает площадь треугольника
        /// </summary>
        /// <param name="triangle"></param>
        /// <returns></returns>
        public static double GetSquareTriangle(Triangle triangle)
        {
            return Math.Abs((triangle.vertex1.X * triangle.vertex2.Y + triangle.vertex2.X * triangle.vertex3.Y + triangle.vertex3.X * triangle.vertex1.Y) -
                (triangle.vertex2.X * triangle.vertex1.Y + triangle.vertex3.X * triangle.vertex2.Y + triangle.vertex1.X * triangle.vertex3.Y)) / 2;            
        }
        /// <summary>
        /// Возвращает произведение длин сторон треугольника
        /// </summary>
        /// <param name="triangle"></param>
        /// <returns></returns>
        private static  double GetProductSidesTriangle(Triangle triangle)
        {
            double[] side = new double[3];

            side[0] = Math.Sqrt((triangle.vertex2.X - triangle.vertex1.X) * (triangle.vertex2.X - triangle.vertex1.X) + (triangle.vertex2.Y - triangle.vertex1.Y) * (triangle.vertex2.Y - triangle.vertex1.Y));
            side[1] = Math.Sqrt((triangle.vertex3.X - triangle.vertex2.X) * (triangle.vertex3.X - triangle.vertex2.X) + (triangle.vertex3.Y - triangle.vertex2.Y) * (triangle.vertex3.Y - triangle.vertex2.Y));
            side[2] = Math.Sqrt((triangle.vertex1.X - triangle.vertex3.X) * (triangle.vertex1.X - triangle.vertex3.X) + (triangle.vertex1.Y - triangle.vertex3.Y) * (triangle.vertex1.Y - triangle.vertex3.Y));

            return side[0] * side[1] * side[2];
        }
        /// <summary>
        /// Возвращает отношение радиуса описанной окружности к самому короткому ребру треугольника.
        /// </summary>
        /// <param name="triangle"></param>
        /// <returns></returns>
        private static double GetCircumRadiusShortestEdgeRatio(Triangle triangle)
        {
            return (GetCircumRadius(triangle) / ShortestEdgeTriagnle(triangle));
        }
        /// <summary>
        /// Возвращает сумму длин сторон треугольника.
        /// </summary>
        /// <param name="triangle"></param>
        /// <returns></returns>
        private static double GetSumtSidesTriangle(Triangle triangle)
        {
            double[] side = new double[3];

            side[0] = Math.Sqrt((triangle.vertex2.X - triangle.vertex1.X) * (triangle.vertex2.X - triangle.vertex1.X) + (triangle.vertex2.Y - triangle.vertex1.Y) * (triangle.vertex2.Y - triangle.vertex1.Y));
            side[1] = Math.Sqrt((triangle.vertex3.X - triangle.vertex2.X) * (triangle.vertex3.X - triangle.vertex2.X) + (triangle.vertex3.Y - triangle.vertex2.Y) * (triangle.vertex3.Y - triangle.vertex2.Y));
            side[2] = Math.Sqrt((triangle.vertex1.X - triangle.vertex3.X) * (triangle.vertex1.X - triangle.vertex3.X) + (triangle.vertex1.Y - triangle.vertex3.Y) * (triangle.vertex1.Y - triangle.vertex3.Y));

            return side[0] + side[1] + side[2];
        }
        /// <summary>
        /// Возвращает true, если диаметральная окружность треугольника пересекает ребро, заданное двумя точками и false, если нет.
        /// </summary>
        /// <param name="triangle"></param>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        private static bool PointIsDiametralCircle(Triangle triangle, PointD startPoint, PointD endPoint)
        {
            PointD midPoint = new();

            midPoint.X = (startPoint.X + endPoint.X) / 2;
            midPoint.Y = (startPoint.Y + endPoint.Y) / 2;

            if (GetDistancePoints(GetCenterPointCircleTriangle(triangle), midPoint) <= (GetDistancePoints(midPoint, startPoint) / 2 ))
            {
                return true;
            }
            return false;          
        }
        /// <summary>
        /// Возврщает центр описанной окружности треугольника.
        /// </summary>
        /// <param name="triangle"></param>
        /// <returns></returns>
        private static PointD GetCenterPointCircleTriangle(Triangle triangle)
        {
            PointD centerCirclePoint = new();

            centerCirclePoint.X = -(triangle.vertex1.Y * (triangle.vertex2.X * triangle.vertex2.X + triangle.vertex2.Y * triangle.vertex2.Y - triangle.vertex3.X * triangle.vertex3.X - triangle.vertex3.Y * triangle.vertex3.Y) +
                triangle.vertex2.Y * (triangle.vertex3.X * triangle.vertex3.X + triangle.vertex3.Y * triangle.vertex3.Y - triangle.vertex1.X * triangle.vertex1.X - triangle.vertex1.Y * triangle.vertex1.Y) +
                triangle.vertex3.Y * (triangle.vertex1.X * triangle.vertex1.X + triangle.vertex1.Y * triangle.vertex1.Y - triangle.vertex2.X * triangle.vertex2.X - triangle.vertex2.Y * triangle.vertex2.Y)) /
                (2 * (triangle.vertex1.X * (triangle.vertex2.Y - triangle.vertex3.Y) + triangle.vertex2.X * (triangle.vertex3.Y - triangle.vertex1.Y) + triangle.vertex3.X * (triangle.vertex1.Y - triangle.vertex2.Y)));

            centerCirclePoint.Y = (triangle.vertex1.X * (triangle.vertex2.X * triangle.vertex2.X + triangle.vertex2.Y * triangle.vertex2.Y - triangle.vertex3.X * triangle.vertex3.X - triangle.vertex3.Y * triangle.vertex3.Y) +
               triangle.vertex2.X * (triangle.vertex3.X * triangle.vertex3.X + triangle.vertex3.Y * triangle.vertex3.Y - triangle.vertex1.X * triangle.vertex1.X - triangle.vertex1.Y * triangle.vertex1.Y) +
               triangle.vertex3.X * (triangle.vertex1.X * triangle.vertex1.X + triangle.vertex1.Y * triangle.vertex1.Y - triangle.vertex2.X * triangle.vertex2.X - triangle.vertex2.Y * triangle.vertex2.Y)) /
               (2 * (triangle.vertex1.X * (triangle.vertex2.Y - triangle.vertex3.Y) + triangle.vertex2.X * (triangle.vertex3.Y - triangle.vertex1.Y) + triangle.vertex3.X * (triangle.vertex1.Y - triangle.vertex2.Y)));

            return centerCirclePoint;
        }
        /// <summary>
        /// Возвращает true, если точка содержится внутри треугольника и false, если нет.
        /// </summary>
        /// <param name="Triangle"></param>
        /// <param name="Point"></param>
        /// <returns></returns>
        private static bool PointOnTriangle(Triangle triangle, PointD point)
        {
            double[] s = new double[3];

            s[0] = (triangle.vertex1.X - point.X) * (triangle.vertex2.Y - triangle.vertex1.Y) - (triangle.vertex2.X - triangle.vertex1.X) * (triangle.vertex1.Y - point.Y);
            s[1] = (triangle.vertex2.X - point.X) * (triangle.vertex3.Y - triangle.vertex2.Y) - (triangle.vertex3.X - triangle.vertex2.X) * (triangle.vertex2.Y - point.Y);
            s[2] = (triangle.vertex3.X - point.X) * (triangle.vertex1.Y - triangle.vertex3.Y) - (triangle.vertex1.X - triangle.vertex3.X) * (triangle.vertex3.Y - point.Y);

            return (Math.Sign(s[0]) == Math.Sign(s[1]) && Math.Sign(s[0]) == Math.Sign(s[2]));
        }
        /// <summary>
        /// Возвращает номер ребра треугольника, которое лежит напротив данной точки(вершины).
        /// </summary>
        /// <param name="triangle"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        private static int NumberEdgeTriangle(Triangle triangle, PointD point)
        {
            if (point == triangle.vertex1)
            {
                return 2;
            }
            else if (point == triangle.vertex2)
            {
                return 3;
            }
            else if (point == triangle.vertex3)
            {
                return 1;
            }
            return -1;            
        }
        /// <summary>
        /// Возврщает true, если какая либо из вершин треугольника лежит внутри "соседнего" треугольника.
        /// </summary>
        /// <param name="neighborTriangle"></param>
        /// <param name="triangle"></param>
        /// <returns></returns>
        private bool PointInCircleNeighborTriangle(ref EdgesAndTriangle neighborTriangle, ref Triangle triangle)
        {
            double radius = GetProductSidesTriangle(triangle) / (4 * GetSquareTriangle(triangle));

            if (neighborTriangle.NumEdge == 1)
            {
                if (GetDistancePoints(Triangles[neighborTriangle.NumTriangle].vertex3, GetCenterPointCircleTriangle(triangle)) <= radius)
                {
                    return true;
                }
            }
            else if (neighborTriangle.NumEdge == 2)
            {
                if (GetDistancePoints(Triangles[neighborTriangle.NumTriangle].vertex1, GetCenterPointCircleTriangle(triangle)) <= radius)
                {
                    return true;
                }
            }
            else if (neighborTriangle.NumEdge == 3)
            {
                if (GetDistancePoints(Triangles[neighborTriangle.NumTriangle].vertex2, GetCenterPointCircleTriangle(triangle)) <= radius)
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// Возвращает true, если переданная точка является узлом
        /// </summary>
        /// <param name="point"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        private static bool ExistNode(PointD point, ref List<PointD> points)
        {
            for (int i = 0; i < points.Count; i++)
            {
                if (point == points[i])
                {
                    return true;
                }
            }
            return false;
        }
        private int IterationNumbering(List<PointD> points, int  countNodes)
        {
            double minX = 1000;
            Nodes tempNode = new();
            List<int> indexPoint = new();
            List<PointD> leftPoints = new();

            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].X < minX)
                {
                    minX = points[i].X;
                }
            }
            
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].X == minX)
                {
                    indexPoint.Add(i);
                    leftPoints.Add(points[i]);
                }
            }
        
            leftPoints.Sort(ComparisonPointByCoordinatesY);

            for (int i = 0; i < leftPoints.Count; i++)
            {
                tempNode.Node = leftPoints[i];
                tempNode.NumNode = countNodes;
                countNodes++;

                Nodes.Add(tempNode);
            }

            foreach (int index in indexPoint.OrderByDescending(n => n))
            {
                points.RemoveAt(index);
            }

            leftPoints.Clear();
            indexPoint.Clear();

            return countNodes;
        }
        /// <summary>
        /// Функция сравнения двух точек по координате у
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="secondPoint"></param>
        /// <returns></returns>
        private int ComparisonPointByCoordinatesY(PointD firstPoint, PointD secondPoint)
        {
            if (firstPoint.Y <= secondPoint.Y)
            {
                return -1;
            }

            return 1;          
        }
        /// <summary>
        /// Сопоставление вершин треугольника и узлов сетки
        /// </summary>
        private void EqualityVertexAndNodes()
        {
            Triangle tempTriangle = new();

            for (int i = 0; i < Triangles.Count; i++)
            {
                if (NumberNodeIsVertex(Triangles[i].vertex1) != -1)
                {
                    tempTriangle = Triangles[i];
                    tempTriangle.vertex1NumNodes = NumberNodeIsVertex(Triangles[i].vertex1);
                    Triangles[i] = tempTriangle;
                }

                if (NumberNodeIsVertex(Triangles[i].vertex2) != -1)
                {
                    tempTriangle = Triangles[i];
                    tempTriangle.vertex2NumNodes = NumberNodeIsVertex(Triangles[i].vertex2);
                    Triangles[i] = tempTriangle;
                }

                if (NumberNodeIsVertex(Triangles[i].vertex3) != -1)
                {
                    tempTriangle = Triangles[i];
                    tempTriangle.vertex3NumNodes = NumberNodeIsVertex(Triangles[i].vertex3);
                    Triangles[i] = tempTriangle;
                }
            }
        }
        /// <summary>
        /// Возвращает true, если заданная вершина является узлом сетки и false в противном случае
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="numberNode"></param>
        /// <returns></returns>
        private int NumberNodeIsVertex(PointD vertex)
        {
            int numberNode = -1;

            for (int i = 0; i < Nodes.Count; i++)
            {
                if (vertex == Nodes[i].Node)
                {
                    numberNode = Nodes[i].NumNode;
                    break;
                }
            }
            return numberNode;
        }
        /// <summary>
        /// Возвращает true, если в образованном тремя точками треугольнике, угол при основании больше 90 градусов
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="secondPoint"></param>
        /// <param name="thirdPoint"></param>
        /// <returns></returns>
        public static bool ObtuseAngle(PointD firstPoint, PointD secondPoint, PointD thirdPoint)
        {
            PointD[] vector = GetVectorTriangle(firstPoint, secondPoint, thirdPoint);

            if (GetAngleVectors(vector[0], vector[1]) > 90)
            {
                return true;
            }
            else 
            {
                vector = GetVectorTriangle(secondPoint, firstPoint, thirdPoint);

                if(GetAngleVectors(vector[0], vector[1]) > 90)
                {
                    return true;
                }
            }
            return false;
        }
        private static PointD[] GetVectorTriangle(PointD firstPoint, PointD secondPoint, PointD thirdPoint)
        {
            PointD[] vector = new PointD[2];

            vector[0].X = secondPoint.X - firstPoint.X;
            vector[0].Y = secondPoint.Y - firstPoint.Y;

            vector[1].X = thirdPoint.X - firstPoint.X;
            vector[1].Y = thirdPoint.Y - firstPoint.Y;

            return vector;
        }
        /// <summary>
        /// Возвращает минимальное расстояние от первой или второй точки, до третьей
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="secondPoint"></param>
        /// <param name="thirdPoint"></param>
        /// <returns></returns>
        public static double MinDistanceToPoint(PointD firstPoint, PointD secondPoint, PointD thirdPoint)
        {
            if (Math.Sqrt((thirdPoint.X - firstPoint.X) * (thirdPoint.X - firstPoint.X) + (thirdPoint.Y - firstPoint.Y) * (thirdPoint.Y - firstPoint.Y)) < Math.Sqrt((thirdPoint.X - secondPoint.X) * (thirdPoint.X - secondPoint.X) + (thirdPoint.Y - secondPoint.Y) * (thirdPoint.Y - secondPoint.Y)))
            {
                return Math.Sqrt((thirdPoint.X - firstPoint.X) * (thirdPoint.X - firstPoint.X) + (thirdPoint.Y - firstPoint.Y) * (thirdPoint.Y - firstPoint.Y));
            }

            return Math.Sqrt((thirdPoint.X - secondPoint.X) * (thirdPoint.X - secondPoint.X) + (thirdPoint.Y - secondPoint.Y) * (thirdPoint.Y - secondPoint.Y));
        }
        /// <summary>
        /// Возвращает вершину наибольшего угла треугольника 
        /// </summary>
        /// <param name="triangle"></param>
        /// <returns></returns>
        private static PointD VertexMaxAngle(Triangle triangle)
        {
            PointD[] vector;
            double firstAngle = 0, secondAngle = 0, thirdAngle = 0; // 1 и 3 / 1 и 2/  2 и 3

            for (int i = 1; i < 4; i++)
            {
                vector = GetVectorSideTriangle(triangle, i);

                if (i == 1)
                {
                    firstAngle = GetAngleVectors(vector[0], vector[1]);
                }
                else if (i == 2)
                {
                    secondAngle = GetAngleVectors(vector[0], vector[1]);
                }
                else
                {
                    thirdAngle = GetAngleVectors(vector[0], vector[1]);
                }
            }

            return (firstAngle >= secondAngle) ? ((firstAngle >= thirdAngle) ? (triangle.vertex1) : (triangle.vertex3)) : ((secondAngle >= thirdAngle) ? (triangle.vertex2) : (triangle.vertex3));
        }      
    }
}
