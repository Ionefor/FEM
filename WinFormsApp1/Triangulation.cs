namespace MKE
{
    /// <summary>
    ///Структура "Треугольник", содержащая в себе вершины треугольника и их номера
    /// </summary>
    public struct Triangle
    {
        public PointF vertex1;
        public PointF vertex2;
        public PointF vertex3;

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
        public PointF Node;
        public int NumNode;

        public Nodes(PointF Node, int NumNode)
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
        /// <summary>
        /// Счетчик узлов сетки
        /// </summary>
        private int countNodes = 1;
        /// <summary>
        /// Начальный массив точек области
        /// </summary>
        private PointF[] points { get; set; }

        /// <summary>
        /// Список структур треугольников триангуляции Делоне
        /// </summary>
        public List<Triangle> triangulation = new List<Triangle>();

        /// <summary>
        /// Список точек после разбиения изначальных сторон области пополам
        /// </summary>
        private List<PointF> splitPoints;

        /// <summary>
        /// Треугольники, не подходящие под условия триангуляции
        /// </summary>
        private List<Triangle> badTriangle = new List<Triangle>();

        /// <summary>
        /// Узлы сетки
        /// </summary>
        /// 
        public List<Nodes> nodes = new List<Nodes>();

        public Triangulation(PointF[] points) => this.points = points;       
        public Triangulation()
        {
          
        }

        /// <summary>
        /// Разбивает стороны области пополам и выполняет разбиение на треугольники по наименьшему углу между сторонами области.
        /// </summary>
        public void InitialPartitioning()
        {
            PointF[] vectors = new PointF[2];
            Triangle tempTriangle = new Triangle();

            int iFirstIndex, iSecondIndex, iThirdIndex;
            int jFirstIndex, jSecondIndex, jThirdIndex;

            int foundIndex = 0;

            double currentAngle, minAngle = 360;

            splitPoints = MiddleSplitEdges();

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

                triangulation.Add(tempTriangle);

               FlipBadEdges(triangulation[triangulation.Count - 1], VertexMaxAngle(triangulation[triangulation.Count - 1]));

                if (RemovePoint(splitPoints[iSecondIndex], triangulation))
                {
                    splitPoints.RemoveAt(iSecondIndex);
                }
                minAngle = 360;
            }
        }
        /// <summary>
        /// Выполняет триангуляцию Делоне ..........
        /// </summary>
        public void Delaunaytriangulation()
        {
            double h = ShortestEdgeTriagnle() / 5;
            double B = Math.Sqrt(2);
            PointF midPoint = new PointF();
            int stopCount = 0;
            step_two:;

            AddBadTriangles(B, h);

            while (badTriangle.Count != 0)
            {
                stopCount++;
                if(stopCount == 25)
                {
                    break;
                }
                int index = GetIndexWorstTriangle(h);

                for (int i = 0; i < triangulation.Count; i++)
                {
                    if (!EqualTriangle(triangulation[i], badTriangle[index]))
                    {
                        if (PointIsDiametralCircle(badTriangle[index], triangulation[i].vertex1, triangulation[i].vertex2))
                        {
                            midPoint.X = (badTriangle[index].vertex1.X + badTriangle[index].vertex2.X) / 2;
                            midPoint.Y = (badTriangle[index].vertex1.Y + badTriangle[index].vertex2.Y) / 2;

                            AddPoint(midPoint);

                            goto step_two;
                        }
                        else if (PointIsDiametralCircle(badTriangle[index], triangulation[i].vertex2, triangulation[i].vertex3))
                        {
                            midPoint.X = (badTriangle[index].vertex2.X + badTriangle[index].vertex3.X) / 2;
                            midPoint.Y = (badTriangle[index].vertex2.Y + badTriangle[index].vertex3.Y) / 2;

                            AddPoint(midPoint);

                            goto step_two;
                        }
                        else if (PointIsDiametralCircle(badTriangle[index], triangulation[i].vertex3, triangulation[i].vertex1))
                        {
                            midPoint.X = (badTriangle[index].vertex3.X + badTriangle[index].vertex1.X) / 2;
                            midPoint.Y = (badTriangle[index].vertex3.Y + badTriangle[index].vertex1.Y) / 2;

                            AddPoint(midPoint);

                            goto step_two;
                        }
                    }
                }

                if (!PointBeyondArea(GetCenterPointCircleTriangle(badTriangle[index])))
                {
                    AddPoint(GetCenterPointCircleTriangle(badTriangle[index]));
                }
                else
                {
                    SplitHeightTriangle(badTriangle[index]);
                }

                goto step_two;
            }
        }
        /// <summary>
        /// Нумерует узлы, полученной триангуляции Делоне
        /// </summary>
        public void DeterminingNodeNumbers()
        {            
            List<PointF> allPoints = new List<PointF>();
            
            for (int i = 0; i < triangulation.Count; i++)
            {
                if (!ExistNode(triangulation[i].vertex1, ref allPoints))
                {
                    allPoints.Add(triangulation[i].vertex1);
                }

                if (!ExistNode(triangulation[i].vertex2, ref allPoints))
                {
                    allPoints.Add(triangulation[i].vertex2);
                }

                if (!ExistNode(triangulation[i].vertex3, ref allPoints))
                {
                    allPoints.Add(triangulation[i].vertex3);
                }
            }
           
            while (allPoints.Count > 0)
            {
                IterationNumbering(allPoints);
            }

            EqualityVertexAndNodes();
        }
        /// <summary>
        /// Разделяет ребра области, вставляя точки по середине ребра.
        /// </summary>
        /// <returns></returns>
        private List<PointF> MiddleSplitEdges()
        {
            PointF midPoint = new PointF();
            PointF startPoint = points[points.Length - 1];
            List<PointF> splitPoints = new List<PointF>();

            for (int i = 0; i < points.Length; i++)
            {
                PointF endPoint = points[i];

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
        private bool RemovePoint(PointF CurrentPoint, List<Triangle> Triangles)
        {
            bool flagO = false, flagN = false;
            int Count;

            for (int i = 0; i < splitPoints.Count; i++)
            {
                if (CurrentPoint == splitPoints[i])
                {
                    for (int j = 0; j < Triangles.Count; j++)
                    {
                        if (i == 0)
                        {
                            Count = splitPoints.Count - 1;

                            if (splitPoints[Count] == Triangles[j].vertex1 || splitPoints[Count] == Triangles[j].vertex2 || splitPoints[Count] == Triangles[j].vertex3)
                            {
                                flagO = true;
                            }
                        }
                        else
                        {
                            if (splitPoints[i - 1] == Triangles[j].vertex1 || splitPoints[i - 1] == Triangles[j].vertex2 || splitPoints[i - 1] == Triangles[j].vertex3)
                            {
                                flagO = true;
                            }
                        }

                        if (i == splitPoints.Count - 1)
                        {
                            Count = 0;
                            if (splitPoints[Count] == Triangles[j].vertex1 || splitPoints[Count] == Triangles[j].vertex2 || splitPoints[Count] == Triangles[j].vertex3)
                            {
                                flagN = true;
                            }
                        }
                        else
                        {
                            if (splitPoints[i + 1] == Triangles[j].vertex1 || splitPoints[i + 1] == Triangles[j].vertex2 || splitPoints[i + 1] == Triangles[j].vertex3)
                            {
                                flagN = true;
                            }
                        }
                    }
                }
            }

            if (flagN && flagO)
            {
                flagO = false;
                flagN = false;

                return true;
            }
            else
            {
                flagO = false;
                flagN = false;

                return false;
            }


        }
        /// <summary>
        /// Находит "плохие" треугольники и добавляет их в массив.
        /// </summary>
        /// <param name="B"></param>
        /// <param name="h"></param>
        /// <param name="XOY"></param>
        private void AddBadTriangles(double B, double h)
        {
            badTriangle.Clear();

            for (int i = 0; i < triangulation.Count; i++)
            {
                if (GetCircumRadiusShortestEdgeRatio(triangulation[i]) > B && GetSumtSidesTriangle(triangulation[i]) > 2)//&& getSumtSidesTriangle(triangulation[i]) > 5
                {
                    badTriangle.Add(triangulation[i]);
                }
                else if (GetCircumRadius(triangulation[i]) > h && ShortestEdgeTriagnle(triangulation[i]) > h && GetSumtSidesTriangle(triangulation[i]) > 2)
                {
                    badTriangle.Add(triangulation[i]);
                }
            }
        }
        /// <summary>
        /// Возвращает индекс самого "плохого" треугольника.
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        private int GetIndexWorstTriangle(double h)
        {
            double maxDifference = 0;
            int index = -1;

            for (int i = 0; i < badTriangle.Count; i++)
            {               
                if (GetSumtSidesTriangle(badTriangle[i]) > maxDifference)
                {
                    maxDifference = GetSumtSidesTriangle(badTriangle[i]);
                    index = i;
                }
            }
            return index;
        }
        /// <summary>
        /// Добавляет точку в сетку
        /// </summary>
        /// <param name="addedPoint"></param>
        private void AddPoint(PointF addedPoint)
        {
            int indexTriangle = 0;
            Triangle tempTriangle = new Triangle();
            List<EdgesAndTriangle> edgesAndTriangles = PointOnSideTriangle(addedPoint);

            if (edgesAndTriangles != null)
            {
                for (int j = 0; j < edgesAndTriangles.Count; j++)
                {
                    if (edgesAndTriangles[j].NumEdge == 1)
                    {
                        tempTriangle.vertex1 = triangulation[edgesAndTriangles[j].NumTriangle].vertex1;
                        tempTriangle.vertex2 = addedPoint;
                        tempTriangle.vertex3 = triangulation[edgesAndTriangles[j].NumTriangle].vertex3;

                       // tempTriangle = sortVertexTriangle(ref tempTriangle);
                        triangulation.Add(tempTriangle);

                        tempTriangle.vertex1 = addedPoint;
                        tempTriangle.vertex2 = triangulation[edgesAndTriangles[j].NumTriangle].vertex2;
                        tempTriangle.vertex3 = triangulation[edgesAndTriangles[j].NumTriangle].vertex3;

                       // tempTriangle = sortVertexTriangle(ref tempTriangle);
                        triangulation.Add(tempTriangle);
                    }
                    else if (edgesAndTriangles[j].NumEdge == 2)
                    {
                        tempTriangle.vertex1 = triangulation[edgesAndTriangles[j].NumTriangle].vertex1;
                        tempTriangle.vertex2 = triangulation[edgesAndTriangles[j].NumTriangle].vertex2;
                        tempTriangle.vertex3 = addedPoint;

                      //  tempTriangle = sortVertexTriangle(ref tempTriangle);
                        triangulation.Add(tempTriangle);

                        tempTriangle.vertex1 = addedPoint;
                        tempTriangle.vertex2 = triangulation[edgesAndTriangles[j].NumTriangle].vertex3;
                        tempTriangle.vertex3 = triangulation[edgesAndTriangles[j].NumTriangle].vertex1;

                      //  tempTriangle = sortVertexTriangle(ref tempTriangle);
                        triangulation.Add(tempTriangle);
                    }
                    else if (edgesAndTriangles[j].NumEdge == 3)
                    {
                        tempTriangle.vertex1 = triangulation[edgesAndTriangles[j].NumTriangle].vertex1;
                        tempTriangle.vertex2 = triangulation[edgesAndTriangles[j].NumTriangle].vertex2;
                        tempTriangle.vertex3 = addedPoint;

                      //  tempTriangle = sortVertexTriangle(ref tempTriangle);
                        triangulation.Add(tempTriangle);

                        tempTriangle.vertex1 = addedPoint;
                        tempTriangle.vertex2 = triangulation[edgesAndTriangles[j].NumTriangle].vertex2;
                        tempTriangle.vertex3 = triangulation[edgesAndTriangles[j].NumTriangle].vertex3;

                       // tempTriangle = sortVertexTriangle(ref tempTriangle);
                        triangulation.Add(tempTriangle);
                    }
                }

                if (edgesAndTriangles.Count != 1)
                {
                    if (edgesAndTriangles[0].NumTriangle > edgesAndTriangles[1].NumTriangle)
                    {
                        triangulation.RemoveAt(edgesAndTriangles[0].NumTriangle);
                        triangulation.RemoveAt(edgesAndTriangles[1].NumTriangle);
                    }
                    else
                    {
                        triangulation.RemoveAt(edgesAndTriangles[1].NumTriangle);
                        triangulation.RemoveAt(edgesAndTriangles[0].NumTriangle);
                    }

                    FlipBadEdges(triangulation[triangulation.Count - 4], addedPoint);
                    FlipBadEdges(triangulation[triangulation.Count - 3], addedPoint);
                    FlipBadEdges(triangulation[triangulation.Count - 2], addedPoint);
                    FlipBadEdges(triangulation[triangulation.Count - 1], addedPoint);
                }
                else
                {
                    triangulation.RemoveAt(edgesAndTriangles[0].NumTriangle);

                    FlipBadEdges(triangulation[triangulation.Count - 2], addedPoint);
                    FlipBadEdges(triangulation[triangulation.Count - 1], addedPoint);
                }
            }
            else
            {
                for (int i = 0; i < triangulation.Count; i++)
                {
                    if (PointOnTriangle(triangulation[i], addedPoint))
                    {
                        indexTriangle = i;

                    }
                }

                tempTriangle.vertex1 = triangulation[indexTriangle].vertex1;
                tempTriangle.vertex2 = triangulation[indexTriangle].vertex2;
                tempTriangle.vertex3 = addedPoint;

               // tempTriangle = sortVertexTriangle(ref tempTriangle);
                triangulation.Add(tempTriangle);

                tempTriangle.vertex1 = triangulation[indexTriangle].vertex1;
                tempTriangle.vertex2 = addedPoint;
                tempTriangle.vertex3 = triangulation[indexTriangle].vertex3;

              //  tempTriangle = sortVertexTriangle(ref tempTriangle);
                triangulation.Add(tempTriangle);

                tempTriangle.vertex1 = addedPoint;
                tempTriangle.vertex2 = triangulation[indexTriangle].vertex2;
                tempTriangle.vertex3 = triangulation[indexTriangle].vertex3;

               // tempTriangle = sortVertexTriangle(ref tempTriangle);
                triangulation.Add(tempTriangle);
                
                FlipBadEdges(triangulation[triangulation.Count - 3], addedPoint);
                FlipBadEdges(triangulation[triangulation.Count - 2], addedPoint);
                FlipBadEdges(triangulation[triangulation.Count - 1], addedPoint);
            }
        }
        /// <summary>
        /// Переворачивает общее ребро соседних треугольников
        /// </summary>
        /// <param name="triangle"></param>
        /// <param name="newPoint"></param>
        private void FlipBadEdges(Triangle triangle, PointF newPoint)
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
                            tempTriangle[0].vertex3 = triangulation[edgesAndTriangleNeighbor.NumTriangle].vertex3;

                         //   tempTriangle[0] = sortVertextriangle(ref tempTriangle[0]);
                            triangulation.Add(tempTriangle[0]);

                            tempTriangle[1].vertex1 = triangulation[edgesAndTriangleNeighbor.NumTriangle].vertex3;
                            tempTriangle[1].vertex2 = triangle.vertex2;
                            tempTriangle[1].vertex3 = newPoint;

                          //  tempTriangle[1] = sortVertextriangle(ref tempTriangle[1]);
                            triangulation.Add(tempTriangle[1]);

                            triangulation.Remove(triangle);
                            triangulation.Remove(triangulation[edgesAndTriangleNeighbor.NumTriangle]);

                            FlipBadEdges(tempTriangle[0], newPoint);
                            FlipBadEdges(tempTriangle[1], newPoint);
                        }
                        else if (edgesAndTriangleNeighbor.NumEdge == 2)
                        {
                            tempTriangle[0].vertex1 = newPoint;
                            tempTriangle[0].vertex2 = triangle.vertex1;
                            tempTriangle[0].vertex3 = triangulation[edgesAndTriangleNeighbor.NumTriangle].vertex1;

                         //   tempTriangle[0] = sortVertextriangle(ref tempTriangle[0]);
                            triangulation.Add(tempTriangle[0]);

                            tempTriangle[1].vertex1 = triangulation[edgesAndTriangleNeighbor.NumTriangle].vertex1;
                            tempTriangle[1].vertex2 = triangle.vertex2;
                            tempTriangle[1].vertex3 = newPoint;

                          //  tempTriangle[1] = sortVertextriangle(ref tempTriangle[1]);
                            triangulation.Add(tempTriangle[1]);

                            triangulation.Remove(triangle);
                            triangulation.Remove(triangulation[edgesAndTriangleNeighbor.NumTriangle]);

                            FlipBadEdges(tempTriangle[0], newPoint);
                            FlipBadEdges(tempTriangle[1], newPoint);
                        }
                        else if (edgesAndTriangleNeighbor.NumEdge == 3)
                        {
                            tempTriangle[0].vertex1 = newPoint;
                            tempTriangle[0].vertex2 = triangle.vertex1;
                            tempTriangle[0].vertex3 = triangulation[edgesAndTriangleNeighbor.NumTriangle].vertex2;

                          //  tempTriangle[0] = sortVertextriangle(ref tempTriangle[0]);
                            triangulation.Add(tempTriangle[0]);

                            tempTriangle[1].vertex1 = triangulation[edgesAndTriangleNeighbor.NumTriangle].vertex2;
                            tempTriangle[1].vertex2 = triangle.vertex2;
                            tempTriangle[1].vertex3 = newPoint;

                          //  tempTriangle[1] = sortVertextriangle(ref tempTriangle[1]);
                            triangulation.Add(tempTriangle[1]);

                            triangulation.Remove(triangle);
                            triangulation.Remove(triangulation[edgesAndTriangleNeighbor.NumTriangle]);

                            FlipBadEdges(tempTriangle[0], newPoint);
                            FlipBadEdges(tempTriangle[1], newPoint);
                        }
                    }
                    else if (commonEdgeIndex == 2)
                    {
                        if (edgesAndTriangleNeighbor.NumEdge == 1)
                        {
                            tempTriangle[0].vertex1 = newPoint;
                            tempTriangle[0].vertex2 = triangle.vertex2;
                            tempTriangle[0].vertex3 = triangulation[edgesAndTriangleNeighbor.NumTriangle].vertex3;

                          //  tempTriangle[0] = sortVertextriangle(ref tempTriangle[0]);
                            triangulation.Add(tempTriangle[0]);

                            tempTriangle[1].vertex1 = triangulation[edgesAndTriangleNeighbor.NumTriangle].vertex3;
                            tempTriangle[1].vertex2 = triangle.vertex3;
                            tempTriangle[1].vertex3 = newPoint;

                         //   tempTriangle[1] = sortVertextriangle(ref tempTriangle[1]);
                            triangulation.Add(tempTriangle[1]);

                            triangulation.Remove(triangle);
                            triangulation.Remove(triangulation[edgesAndTriangleNeighbor.NumTriangle]);

                            FlipBadEdges(tempTriangle[0], newPoint);
                            FlipBadEdges(tempTriangle[1], newPoint);
                        }
                        else if (edgesAndTriangleNeighbor.NumEdge == 2)
                        {
                            tempTriangle[0].vertex1 = newPoint;
                            tempTriangle[0].vertex2 = triangle.vertex2;
                            tempTriangle[0].vertex3 = triangulation[edgesAndTriangleNeighbor.NumTriangle].vertex1;

                          //  tempTriangle[0] = sortVertextriangle(ref tempTriangle[0]);
                            triangulation.Add(tempTriangle[0]);

                            tempTriangle[1].vertex1 = triangulation[edgesAndTriangleNeighbor.NumTriangle].vertex1;
                            tempTriangle[1].vertex2 = triangle.vertex3;
                            tempTriangle[1].vertex3 = newPoint;

                         //   tempTriangle[1] = sortVertextriangle(ref tempTriangle[1]);
                            triangulation.Add(tempTriangle[1]);

                            triangulation.Remove(triangle);
                            triangulation.Remove(triangulation[edgesAndTriangleNeighbor.NumTriangle]);

                            FlipBadEdges(tempTriangle[0], newPoint);
                            FlipBadEdges(tempTriangle[1], newPoint);
                        }
                        else if (edgesAndTriangleNeighbor.NumEdge == 3)
                        {
                            tempTriangle[0].vertex1 = newPoint;
                            tempTriangle[0].vertex2 = triangle.vertex2;
                            tempTriangle[0].vertex3 = triangulation[edgesAndTriangleNeighbor.NumTriangle].vertex2;

                          //  tempTriangle[0] = sortVertextriangle(ref tempTriangle[0]);
                            triangulation.Add(tempTriangle[0]);

                            tempTriangle[1].vertex1 = triangulation[edgesAndTriangleNeighbor.NumTriangle].vertex2;
                            tempTriangle[1].vertex2 = triangle.vertex3;
                            tempTriangle[1].vertex3 = newPoint;

                          //  tempTriangle[1] = sortVertextriangle(ref tempTriangle[1]);
                            triangulation.Add(tempTriangle[1]);

                            triangulation.Remove(triangle);
                            triangulation.Remove(triangulation[edgesAndTriangleNeighbor.NumTriangle]);

                            FlipBadEdges(tempTriangle[0], newPoint);
                            FlipBadEdges(tempTriangle[1], newPoint);
                        }
                    }
                    else if (commonEdgeIndex == 3)
                    {
                        if (edgesAndTriangleNeighbor.NumEdge == 1)
                        {
                            tempTriangle[0].vertex1 = newPoint;
                            tempTriangle[0].vertex2 = triangle.vertex3;
                            tempTriangle[0].vertex3 = triangulation[edgesAndTriangleNeighbor.NumTriangle].vertex3;

                         //   tempTriangle[0] = sortVertextriangle(ref tempTriangle[0]);
                            triangulation.Add(tempTriangle[0]);

                            tempTriangle[1].vertex1 = triangulation[edgesAndTriangleNeighbor.NumTriangle].vertex3;
                            tempTriangle[1].vertex2 = triangle.vertex1;
                            tempTriangle[1].vertex3 = newPoint;

                          //  tempTriangle[1] = sortVertextriangle(ref tempTriangle[1]);
                            triangulation.Add(tempTriangle[1]);

                            triangulation.Remove(triangle);
                            triangulation.Remove(triangulation[edgesAndTriangleNeighbor.NumTriangle]);

                            FlipBadEdges(tempTriangle[0], newPoint);
                            FlipBadEdges(tempTriangle[1], newPoint);
                        }
                        else if (edgesAndTriangleNeighbor.NumEdge == 2)
                        {
                            tempTriangle[0].vertex1 = newPoint;
                            tempTriangle[0].vertex2 = triangle.vertex3;
                            tempTriangle[0].vertex3 = triangulation[edgesAndTriangleNeighbor.NumTriangle].vertex1;

                         //   tempTriangle[0] = sortVertextriangle(ref tempTriangle[0]);
                            triangulation.Add(tempTriangle[0]);

                            tempTriangle[1].vertex1 = triangulation[edgesAndTriangleNeighbor.NumTriangle].vertex1;
                            tempTriangle[1].vertex2 = triangle.vertex1;
                            tempTriangle[1].vertex3 = newPoint;

                         //   tempTriangle[1] = sortVertextriangle(ref tempTriangle[1]);
                            triangulation.Add(tempTriangle[1]);

                            triangulation.Remove(triangle);
                            triangulation.Remove(triangulation[edgesAndTriangleNeighbor.NumTriangle]);

                            FlipBadEdges(tempTriangle[0], newPoint);
                            FlipBadEdges(tempTriangle[1], newPoint);
                        }
                        else if (edgesAndTriangleNeighbor.NumEdge == 3)
                        {
                            tempTriangle[0].vertex1 = newPoint;
                            tempTriangle[0].vertex2 = triangle.vertex3;
                            tempTriangle[0].vertex3 = triangulation[edgesAndTriangleNeighbor.NumTriangle].vertex2;

                         //   tempTriangle[0] = sortVertextriangle(ref tempTriangle[0]);
                            triangulation.Add(tempTriangle[0]);

                            tempTriangle[1].vertex1 = triangulation[edgesAndTriangleNeighbor.NumTriangle].vertex2;
                            tempTriangle[1].vertex2 = triangle.vertex1;
                            tempTriangle[1].vertex3 = newPoint;

                         //   tempTriangle[1] = sortVertextriangle(ref tempTriangle[1]);
                            triangulation.Add(tempTriangle[1]);

                            triangulation.Remove(triangle);
                            triangulation.Remove(triangulation[edgesAndTriangleNeighbor.NumTriangle]);

                            FlipBadEdges(tempTriangle[0], newPoint);
                            FlipBadEdges(tempTriangle[1], newPoint);
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
        private List<EdgesAndTriangle> PointOnSideTriangle(PointF Point)
        {
            EdgesAndTriangle Temp = new EdgesAndTriangle();
            List<EdgesAndTriangle> edgesAndTriangles = new List<EdgesAndTriangle>();
            edgesAndTriangles.Clear();

            for (int i = 0; i < triangulation.Count; i++)
            {
                if ((GetDistancePoints(triangulation[i].vertex1, Point) + GetDistancePoints(triangulation[i].vertex2, Point)) == GetDistancePoints(triangulation[i].vertex1, triangulation[i].vertex2))
                {
                    Temp.NumTriangle = i;
                    Temp.NumEdge = 1;

                    edgesAndTriangles.Add(Temp);
                }
                else if ((GetDistancePoints(triangulation[i].vertex2, Point) + GetDistancePoints(triangulation[i].vertex3, Point)) == GetDistancePoints(triangulation[i].vertex2, triangulation[i].vertex3))
                {
                    Temp.NumTriangle = i;
                    Temp.NumEdge = 2;

                    edgesAndTriangles.Add(Temp);
                }
                else if ((GetDistancePoints(triangulation[i].vertex3, Point) + GetDistancePoints(triangulation[i].vertex1, Point)) == GetDistancePoints(triangulation[i].vertex3, triangulation[i].vertex1))
                {
                    Temp.NumTriangle = i;
                    Temp.NumEdge = 3;

                    edgesAndTriangles.Add(Temp);
                }
            }

            if(edgesAndTriangles.Count != 0)
            {
                return edgesAndTriangles;
            }

            return null;
        }
        private EdgesAndTriangle NeighborTriangle(int indexEdge, ref Triangle triangle)
        {
            EdgesAndTriangle edgesAndTriangleRec = new EdgesAndTriangle();

            for (int i = 0; i < triangulation.Count; i++)
            {
                if (indexEdge == 1)
                {
                    if ((triangle.vertex1 == triangulation[i].vertex1 && triangle.vertex2 == triangulation[i].vertex2 && triangle.vertex3 != triangulation[i].vertex3) |
                       (triangle.vertex1 == triangulation[i].vertex2 && triangle.vertex2 == triangulation[i].vertex1 && triangle.vertex3 != triangulation[i].vertex3))
                    {
                        //1=1
                        edgesAndTriangleRec.NumTriangle = i;
                        edgesAndTriangleRec.NumEdge = 1;
                        
                    }
                    else if ((triangle.vertex1 == triangulation[i].vertex2 && triangle.vertex2 == triangulation[i].vertex3 && triangle.vertex3 != triangulation[i].vertex1) ||
                        (triangle.vertex1 == triangulation[i].vertex3 && triangle.vertex2 == triangulation[i].vertex2 && triangle.vertex3 != triangulation[i].vertex1))
                    {
                        //1=2
                        edgesAndTriangleRec.NumTriangle = i;
                        edgesAndTriangleRec.NumEdge = 2;
                    }
                    else if ((triangle.vertex1 == triangulation[i].vertex3 && triangle.vertex2 == triangulation[i].vertex1 && triangle.vertex3 != triangulation[i].vertex2) ||
                        (triangle.vertex1 == triangulation[i].vertex1 && triangle.vertex2 == triangulation[i].vertex3 && triangle.vertex3 != triangulation[i].vertex2))
                    {
                        //1=3
                        edgesAndTriangleRec.NumTriangle = i;
                        edgesAndTriangleRec.NumEdge = 3;
                    }
                }
                else if (indexEdge == 2)
                {
                    if ((triangle.vertex2 == triangulation[i].vertex1 && triangle.vertex3 == triangulation[i].vertex2 && triangle.vertex1 != triangulation[i].vertex3) ||
                        (triangle.vertex2 == triangulation[i].vertex2 && triangle.vertex3 == triangulation[i].vertex1 && triangle.vertex1 != triangulation[i].vertex3))
                    {
                        //2=1
                        edgesAndTriangleRec.NumTriangle = i;
                        edgesAndTriangleRec.NumEdge = 1;
                    }
                    else if ((triangle.vertex2 == triangulation[i].vertex2 && triangle.vertex3 == triangulation[i].vertex3 && triangle.vertex1 != triangulation[i].vertex1) ||
                        (triangle.vertex2 == triangulation[i].vertex3 && triangle.vertex3 == triangulation[i].vertex2 && triangle.vertex1 != triangulation[i].vertex1))
                    {
                        //2=2
                        edgesAndTriangleRec.NumTriangle = i;
                        edgesAndTriangleRec.NumEdge = 2;
                    }
                    else if ((triangle.vertex2 == triangulation[i].vertex3 && triangle.vertex3 == triangulation[i].vertex1 && triangle.vertex1 != triangulation[i].vertex2) ||
                       (triangle.vertex2 == triangulation[i].vertex1 && triangle.vertex3 == triangulation[i].vertex3 && triangle.vertex1 != triangulation[i].vertex2))
                    {
                        //2=3
                        edgesAndTriangleRec.NumTriangle = i;
                        edgesAndTriangleRec.NumEdge = 3;
                    }
                }
                else if (indexEdge == 3)
                {
                    if ((triangle.vertex3 == triangulation[i].vertex1 && triangle.vertex1 == triangulation[i].vertex2 && triangle.vertex2 != triangulation[i].vertex3) ||
                        (triangle.vertex3 == triangulation[i].vertex2 && triangle.vertex1 == triangulation[i].vertex1 && triangle.vertex2 != triangulation[i].vertex3))
                    {
                        //3=1
                        edgesAndTriangleRec.NumTriangle = i;
                        edgesAndTriangleRec.NumEdge = 1;
                    }
                    else if ((triangle.vertex3 == triangulation[i].vertex2 && triangle.vertex1 == triangulation[i].vertex3 && triangle.vertex2 != triangulation[i].vertex1) ||
                       (triangle.vertex3 == triangulation[i].vertex3 && triangle.vertex1 == triangulation[i].vertex2 && triangle.vertex2 != triangulation[i].vertex1))
                    {
                        //3=2
                        edgesAndTriangleRec.NumTriangle = i;
                        edgesAndTriangleRec.NumEdge = 2;
                    }
                    else if ((triangle.vertex3 == triangulation[i].vertex3 && triangle.vertex1 == triangulation[i].vertex1 && triangle.vertex2 != triangulation[i].vertex2) ||
                       (triangle.vertex3 == triangulation[i].vertex1 && triangle.vertex1 == triangulation[i].vertex3 && triangle.vertex2 != triangulation[i].vertex2))
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
        private bool PointBeyondArea(PointF addPoint)
        {
            List<EdgesAndTriangle> edgesAndTriangle = PointOnSideTriangle(addPoint);

            if (edgesAndTriangle == null)
            {
                for (int i = 0; i < triangulation.Count; i++)
                {
                    if (!PointOnTriangle(triangulation[i], addPoint))
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
            Triangle tempTriangle = new Triangle();
            PointF newPoint = newVertexTriangle(triangle);

            int numSide = RightAngleTriangle(triangle);
            

            if (numSide == 1)
            {
                tempTriangle.vertex1 = triangle.vertex1;
                tempTriangle.vertex2 = newPoint;
                tempTriangle.vertex3 = triangle.vertex3;

                triangulation.Add(tempTriangle);

                tempTriangle.vertex1 = newPoint;
                tempTriangle.vertex2 = triangle.vertex2;
                tempTriangle.vertex3 = triangle.vertex3;

                triangulation.Add(tempTriangle);

                triangulation.Remove(triangle);
            }
            else if (numSide == 2)
            {
                tempTriangle.vertex1 = triangle.vertex1;
                tempTriangle.vertex2 = triangle.vertex2;
                tempTriangle.vertex3 = newPoint;

                triangulation.Add(tempTriangle);

                tempTriangle.vertex1 = triangle.vertex1;
                tempTriangle.vertex2 = newPoint;
                tempTriangle.vertex3 = triangle.vertex3;

                triangulation.Add(tempTriangle);

                triangulation.Remove(triangle);
            }
            else if (numSide == 3)
            {
                tempTriangle.vertex1 = triangle.vertex1;
                tempTriangle.vertex2 = triangle.vertex2;
                tempTriangle.vertex3 = newPoint;

                triangulation.Add(tempTriangle);

                tempTriangle.vertex1 = newPoint;
                tempTriangle.vertex2 = triangle.vertex2;
                tempTriangle.vertex3 = triangle.vertex3;

                triangulation.Add(tempTriangle);

                triangulation.Remove(triangle);
            }
        }
        private int RightAngleTriangle(Triangle triangle)
        {
            PointF[] vector = new PointF[2];
            double firstAngle = 0, secondAngle = 0, thirdAngle = 0; // 1 и 3 / 1 и 2/  2 и 3
            int side = -1;

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

            /*  if(firstAngle <= 35 || secondAngle <= 35 || thirdAngle <= 35)
              {
                  side = (firstAngle >= secondAngle) ? ((firstAngle >= thirdAngle) ? (2) : (1)) : ((secondAngle >= thirdAngle) ? (3) : (1));
              }*/

            if (firstAngle > 120 || secondAngle > 120 || thirdAngle > 120)
            {
                side = (firstAngle >= secondAngle) ? ((firstAngle >= thirdAngle) ? (2) : (1)) : ((secondAngle >= thirdAngle) ? (3) : (1));
            }
            return side;
        }
        private PointF[] GetVectorSideTriangle(Triangle triangle, int sideIndex)
        {
            PointF[] vector = new PointF[2];

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
        private PointF newVertexTriangle(Triangle triangle)
        {
            double cat;
          
            int side = RightAngleTriangle(triangle);
            double h = 2 * GetSquareTriangle(triangle) / GetSideLenght(side, triangle);

            PointF vec = new PointF();
            PointF newVertex = new PointF();

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
        private double GetSideLenght(int num, Triangle triangle)
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
        private PointF[] GetCoordinatesVectors(int firstIndex, int secondIndex, int thirdIndex, ref List<PointF> Points)
        {
            PointF[] vector = new PointF[2];

            vector[0].X = Points[firstIndex].X - Points[secondIndex].X;
            vector[0].Y = Points[firstIndex].Y - Points[secondIndex].Y;

            vector[1].X = Points[thirdIndex].X - Points[secondIndex].X;
            vector[1].Y = Points[thirdIndex].Y - Points[secondIndex].Y;

            return vector;
        }
        /// <summary>
        /// Возвращает угол между двумя векторами в градусах.
        /// </summary>
        /// <param name="vector1"></param>
        /// <param name="vector2"></param>
        /// <returns></returns>
        private double GetAngleVectors(PointF firstVector, PointF secondVector)
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

            for (int i = 1; i < triangulation.Count; i++)
            {
                if (GetDistancePoints(triangulation[i].vertex1, triangulation[i].vertex2) < minEdge)
                {
                    minEdge = GetDistancePoints(triangulation[i].vertex1, triangulation[i].vertex2);
                }

                if (GetDistancePoints(triangulation[i].vertex2, triangulation[i].vertex3) < minEdge)
                {
                    minEdge = GetDistancePoints(triangulation[i].vertex2, triangulation[i].vertex3);
                }

                if (GetDistancePoints(triangulation[i].vertex3, triangulation[i].vertex1) < minEdge)
                {
                    minEdge = GetDistancePoints(triangulation[i].vertex3, triangulation[i].vertex1);
                }
            }

            return minEdge;
        }
        /// <summary>
        /// Возвращает длину минимального ребра треугольника.
        /// </summary>
        /// <param name="XOY"></param>
        /// <returns></returns>
        private double ShortestEdgeTriagnle(Triangle triangle)
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
        public double GetDistancePoints(PointF firstPoint, PointF secondPoint)
        {
            return Math.Sqrt((firstPoint.X - secondPoint.X) * (firstPoint.X - secondPoint.X) + (firstPoint.Y - secondPoint.Y) * (firstPoint.Y - secondPoint.Y));
        }
        /// <summary>
        /// Возвращает радиус описанной окружности треугольника.
        /// </summary>
        /// <param name="triangle"></param>
        /// <returns></returns>
        private double GetCircumRadius(Triangle triangle)
        {
            return (GetProductSidesTriangle(triangle) / (4 * GetSquareTriangle(triangle)));
        }
        /// <summary>
        /// Возвращает площадь треугольника
        /// </summary>
        /// <param name="triangle"></param>
        /// <returns></returns>
        public double GetSquareTriangle(Triangle triangle)
        {
            return Math.Abs((triangle.vertex1.X * triangle.vertex2.Y + triangle.vertex2.X * triangle.vertex3.Y + triangle.vertex3.X * triangle.vertex1.Y) -
                (triangle.vertex2.X * triangle.vertex1.Y + triangle.vertex3.X * triangle.vertex2.Y + triangle.vertex1.X * triangle.vertex3.Y)) / 2;            
        }
        /// <summary>
        /// Возвращает произведение длин сторон треугольника
        /// </summary>
        /// <param name="triangle"></param>
        /// <returns></returns>
        private double GetProductSidesTriangle(Triangle triangle)
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
        private double GetCircumRadiusShortestEdgeRatio(Triangle triangle)
        {
            return (GetCircumRadius(triangle) / ShortestEdgeTriagnle(triangle));
        }
        /// <summary>
        /// Возвращает сумму длин сторон треугольника.
        /// </summary>
        /// <param name="triangle"></param>
        /// <returns></returns>
        private double GetSumtSidesTriangle(Triangle triangle)
        {
            double[] side = new double[3];

            side[0] = Math.Sqrt((triangle.vertex2.X - triangle.vertex1.X) * (triangle.vertex2.X - triangle.vertex1.X) + (triangle.vertex2.Y - triangle.vertex1.Y) * (triangle.vertex2.Y - triangle.vertex1.Y));
            side[1] = Math.Sqrt((triangle.vertex3.X - triangle.vertex2.X) * (triangle.vertex3.X - triangle.vertex2.X) + (triangle.vertex3.Y - triangle.vertex2.Y) * (triangle.vertex3.Y - triangle.vertex2.Y));
            side[2] = Math.Sqrt((triangle.vertex1.X - triangle.vertex3.X) * (triangle.vertex1.X - triangle.vertex3.X) + (triangle.vertex1.Y - triangle.vertex3.Y) * (triangle.vertex1.Y - triangle.vertex3.Y));

            return side[0] + side[1] + side[2];
        }
        /// <summary>
        /// Возвращает true, если треугольники одинаковы или false, если нет.
        /// </summary>
        /// <param name="firsstTriangle"></param>
        /// <param name="secondTriangle"></param>
        /// <returns></returns>
        private bool EqualTriangle(Triangle firsstTriangle, Triangle secondTriangle)
        {
            if ((firsstTriangle.vertex1 == secondTriangle.vertex1 || firsstTriangle.vertex1 == secondTriangle.vertex2 || firsstTriangle.vertex1 == secondTriangle.vertex3) &&
               (firsstTriangle.vertex2 == secondTriangle.vertex1 || firsstTriangle.vertex2 == secondTriangle.vertex2 || firsstTriangle.vertex2 == secondTriangle.vertex3) &&
               (firsstTriangle.vertex3 == secondTriangle.vertex1 || firsstTriangle.vertex3 == secondTriangle.vertex2 || firsstTriangle.vertex3 == secondTriangle.vertex3))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Возвращает true, если диаметральная окружность треугольника пересекает ребро, заданное двумя точками и false, если нет.
        /// </summary>
        /// <param name="triangle"></param>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        private bool PointIsDiametralCircle(Triangle triangle, PointF startPoint, PointF endPoint)
        {
            PointF midPoint = new PointF();

            midPoint.X = (startPoint.X + endPoint.X) / 2;
            midPoint.Y = (startPoint.Y + endPoint.Y) / 2;

            if (GetDistancePoints(GetCenterPointCircleTriangle(triangle), midPoint) <= GetDistancePoints(midPoint, startPoint))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Возврщает центр описанной окружности треугольника.
        /// </summary>
        /// <param name="triangle"></param>
        /// <returns></returns>
        private PointF GetCenterPointCircleTriangle(Triangle triangle)
        {
            PointF centerCirclePoint = new PointF();

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
        private bool PointOnTriangle(Triangle triangle, PointF point)
        {
            float[] s = new float[3];

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
        private int NumberEdgeTriangle(Triangle triangle, PointF point)
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
            else
            {
                return -1;
            }
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
                if (GetDistancePoints(triangulation[neighborTriangle.NumTriangle].vertex3, GetCenterPointCircleTriangle(triangle)) <= radius)
                {
                    return true;
                }
            }
            else if (neighborTriangle.NumEdge == 2)
            {
                if (GetDistancePoints(triangulation[neighborTriangle.NumTriangle].vertex1, GetCenterPointCircleTriangle(triangle)) <= radius)
                {
                    return true;
                }
            }
            else if (neighborTriangle.NumEdge == 3)
            {
                if (GetDistancePoints(triangulation[neighborTriangle.NumTriangle].vertex2, GetCenterPointCircleTriangle(triangle)) <= radius)
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
        private bool ExistNode(PointF point, ref List<PointF> points)
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
        private void IterationNumbering(List<PointF> points)
        {
            float minX = 1000;
            Nodes tempNode = new Nodes();
            List<int> indexPoint = new List<int>();
            List<PointF> leftPoints = new List<PointF>();

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

                nodes.Add(tempNode);
            }

            foreach (int index in indexPoint.OrderByDescending(n => n))
            {
                points.RemoveAt(index);
            }

            leftPoints.Clear();
            indexPoint.Clear();
        }
        /// <summary>
        /// Функция сравнения двух точек по координате у
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="secondPoint"></param>
        /// <returns></returns>
        private int ComparisonPointByCoordinatesY(PointF firstPoint, PointF secondPoint)
        {
            if (firstPoint.Y <= secondPoint.Y)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }
        /// <summary>
        /// Сопоставление вершин треугольника и узлов сетки
        /// </summary>
        private void EqualityVertexAndNodes()
        {
            int numNode;
            Triangle tempTriangle = new Triangle();

            for (int i = 0; i < triangulation.Count; i++)
            {
                if (NodeIsVertex(triangulation[i].vertex1, out numNode))
                {
                    tempTriangle = triangulation[i];
                    tempTriangle.vertex1NumNodes = numNode;
                    triangulation[i] = tempTriangle;
                }

                if (NodeIsVertex(triangulation[i].vertex2, out numNode))
                {
                    tempTriangle = triangulation[i];
                    tempTriangle.vertex2NumNodes = numNode;
                    triangulation[i] = tempTriangle;
                }

                if (NodeIsVertex(triangulation[i].vertex3, out numNode))
                {
                    tempTriangle = triangulation[i];
                    tempTriangle.vertex3NumNodes = numNode;
                    triangulation[i] = tempTriangle;
                }
            }
        }
        /// <summary>
        /// Возвращает true, если заданная вершина является узлом сетки и false в противном случае
        /// </summary>
        /// <param name="vertex"></param>
        /// <param name="numberNode"></param>
        /// <returns></returns>
        private bool NodeIsVertex(PointF vertex, out int numberNode)
        {
            numberNode = -1;

            for (int i = 0; i < nodes.Count; i++)
            {
                if (vertex == nodes[i].Node)
                {
                    numberNode = nodes[i].NumNode;
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Возвращает true, если в образованном тремя точками треугольнике, угол при основании больше 90 градусов
        /// </summary>
        /// <param name="firstPoint"></param>
        /// <param name="secondPoint"></param>
        /// <param name="thirdPoint"></param>
        /// <returns></returns>
        public bool ObtuseAngle(PointF firstPoint, PointF secondPoint, PointF thirdPoint)
        {
            PointF[] vector = GetVectorTriangle(firstPoint, secondPoint, thirdPoint);

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
        private PointF[] GetVectorTriangle(PointF firstPoint, PointF secondPoint, PointF thirdPoint)
        {
            PointF[] vector = new PointF[2];

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
        public double MinDistanceToPoint(PointF firstPoint, PointF secondPoint, PointF thirdPoint)
        {
            if (Math.Sqrt((thirdPoint.X - firstPoint.X) * (thirdPoint.X - firstPoint.X) + (thirdPoint.Y - firstPoint.Y) * (thirdPoint.Y - firstPoint.Y)) < Math.Sqrt((thirdPoint.X - secondPoint.X) * (thirdPoint.X - secondPoint.X) + (thirdPoint.Y - secondPoint.Y) * (thirdPoint.Y - secondPoint.Y)))
            {
                return Math.Sqrt((thirdPoint.X - firstPoint.X) * (thirdPoint.X - firstPoint.X) + (thirdPoint.Y - firstPoint.Y) * (thirdPoint.Y - firstPoint.Y));
            }
            else
            {
                return Math.Sqrt((thirdPoint.X - secondPoint.X) * (thirdPoint.X - secondPoint.X) + (thirdPoint.Y - secondPoint.Y) * (thirdPoint.Y - secondPoint.Y));
            }
        }
        /// <summary>
        /// Возвращает вершину наибольшего угла треугольника 
        /// </summary>
        /// <param name="triangle"></param>
        /// <returns></returns>
        private PointF VertexMaxAngle(Triangle triangle)
        {
            PointF[] vector = new PointF[2];
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
