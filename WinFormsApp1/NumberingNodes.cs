using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKE
{
    public class NumberingNodes
    {
        private List<Triangle> _triangles;
        /// <summary>
        /// Узлы сетки
        /// </summary>
        /// 
        private List<GridNode> _gridNodes;
        public List<GridNode> GridNodes { get => _gridNodes; }
        public NumberingNodes(List<Triangle> triangles)
        {
            _triangles = triangles;
            _gridNodes = new();
        }
        public void DeterminingNodeNumbers()
        {
            var countNodes = 1;
            List<PointD> allPoints = new();

            for (int i = 0; i < _triangles.Count; i++)
            {
                if (!ExistNode(_triangles[i].FirstVertex, ref allPoints))
                {
                    allPoints.Add(_triangles[i].FirstVertex);
                }

                if (!ExistNode(_triangles[i].SecondVertex, ref allPoints))
                {
                    allPoints.Add(_triangles[i].SecondVertex);
                }

                if (!ExistNode(_triangles[i].ThirdVertex, ref allPoints))
                {
                    allPoints.Add(_triangles[i].ThirdVertex);
                }
            }

            while (allPoints.Count > 0)
            {
                countNodes = IterationNumbering(allPoints, countNodes);
            }

            EqualityVertexAndNodes();
        }
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
        private int IterationNumbering(List<PointD> points, int countNodes)
        {
            double minX = 1000;
            GridNode tempNode = new();
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

                _gridNodes.Add(tempNode);
            }

            foreach (int index in indexPoint.OrderByDescending(n => n))
            {
                points.RemoveAt(index);
            }

            leftPoints.Clear();
            indexPoint.Clear();

            return countNodes;
        }
        private int ComparisonPointByCoordinatesY(PointD firstPoint, PointD secondPoint)
        {
            if (firstPoint.Y <= secondPoint.Y)
            {
                return -1;
            }

            return 1;
        }
        private void EqualityVertexAndNodes()
        {
            Triangle tempTriangle;

            for (int i = 0; i < _triangles.Count; i++)
            {
                if (NumberNodeIsVertex(_triangles[i].FirstVertex) != -1)
                {
                    tempTriangle = _triangles[i];
                    tempTriangle.FirstNodeNum = NumberNodeIsVertex(_triangles[i].FirstVertex);
                    _triangles[i] = tempTriangle;
                }

                if (NumberNodeIsVertex(_triangles[i].SecondVertex) != -1)
                {
                    tempTriangle = _triangles[i];
                    tempTriangle.SecondNodeNum = NumberNodeIsVertex(_triangles[i].SecondVertex);
                    _triangles[i] = tempTriangle;
                }

                if (NumberNodeIsVertex(_triangles[i].ThirdVertex) != -1)
                {
                    tempTriangle = _triangles[i];
                    tempTriangle.ThirdNodeNum = NumberNodeIsVertex(_triangles[i].ThirdVertex);
                    _triangles[i] = tempTriangle;
                }
            }
        }
        private int NumberNodeIsVertex(PointD vertex)
        {
            int numberNode = -1;

            for (int i = 0; i < _gridNodes.Count; i++)
            {
                if (vertex == _gridNodes[i].Node)
                {
                    numberNode = _gridNodes[i].NumNode;
                    break;
                }
            }
            return numberNode;
        }
    }
}
