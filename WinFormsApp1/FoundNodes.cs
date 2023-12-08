using MathNet.Numerics.RootFinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKE
{
    public class FoundNodes
    {
        private readonly List<GridNode> _gridNodes;
        private readonly Dictionary<int, double> _temperature;
        private readonly PointD[] _points;
        private readonly List<BorderAndNode> _borderNodes;
        public List<BorderAndNode> BorderNodes { get => _borderNodes; }
        public FoundNodes(List<GridNode> gridNodes, PointD[] points, Dictionary<int, double> temperature)
        {
            _gridNodes = gridNodes;
            _points = points;
             _temperature = temperature;
            _borderNodes = new();
        }
        public void FoundNodeNumber()
        {
            for (int i = 0; i < _gridNodes.Count; i++)
            {
                if (BorderIsTempBorder(NumBorderOnNode(_gridNodes[i])))
                {
                    BorderAndNode temp = new();
                    temp.NumBorder = NumBorderOnNode(_gridNodes[i]);
                    temp.NumNode = _gridNodes[i].NumNode - 1;
                    _borderNodes.Add(temp);
                }
            }
        }
        private bool BorderIsTempBorder(int numBorder)
        {
            if (numBorder != -1)
            {
                if (_temperature.ContainsKey(numBorder))
                {
                    return true;
                }
            }
            return false;
        }
        private int NumBorderOnNode(GridNode node)
        {
            for (int i = 0; i < _points.Length; i++)
            {
                if (i == _points.Length - 1)
                {
                    if ((Mathematics.GetDistancePoints(_points[i], node.Node) + Mathematics.GetDistancePoints(node.Node, _points[0])) == Mathematics.GetDistancePoints(_points[i], _points[0]))
                    {
                        return i;
                    }
                }
                else
                {
                    if ((Mathematics.GetDistancePoints(_points[i], node.Node) + Mathematics.GetDistancePoints(node.Node, _points[i + 1])) == Mathematics.GetDistancePoints(_points[i], _points[i + 1]))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }   
    }
}
