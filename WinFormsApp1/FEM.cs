using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKE
{
    public class FEM
    {
        private Area _area;
        private Triangulation _triangulation;
        private NumberingNodes _numberingNodes;
        private SolutionEquation _solution;
        private FoundNodes _foundNodes;
        private GraphicsMke _grapMke;     
        private Dictionary<int, double> _borderTemperature;
        private Panel _XOY;
        private List<BorderData> _borderData;
        private List<GridNode> _gridNodes;
        private int _sizeGrid;
        private double _Q;
        private double _Kxx;
        private double _Kyy;
        private double[] _result;      
        public List<GridNode> GridNodes { get => _gridNodes; }
        public double[] Result { get => _result; }
        public FEM(PointD[] points, List<BorderData> borderData, Dictionary<int, double> borderTemperature,
            Panel XOY, double Kxx, double Kyy, double Q, int sizeGrid) 
        {
            _Kxx = Kxx;
            _Kyy = Kyy;
            _sizeGrid = sizeGrid;
            _XOY = XOY;
            _borderData = borderData;
            _borderTemperature = borderTemperature;
            _area = new(points);

        }    

        public void BuildingGrid()
        {
            _area.ShiftingPointsArea();

            _triangulation = new(_area.Points, _sizeGrid, _XOY);
            _triangulation.InitialPartitioningArea();
            _triangulation.DelaunayTriangulation();

            _numberingNodes = new NumberingNodes(_triangulation.Triangles);
            _numberingNodes.DeterminingNodeNumbers();
            _gridNodes = _numberingNodes.GridNodes;

            _foundNodes = new FoundNodes(_numberingNodes.GridNodes, _area.Points, _borderTemperature);
            _foundNodes.FoundNodeNumber();

            _grapMke = new GraphicsMke(_XOY, _triangulation.Triangles);
            _grapMke.DisplayAllTriangles(); 
        }
        public void СalculatingResult()
        {
            _solution = new SolutionEquation(_Kxx, _Kyy, _Q, _borderData, _numberingNodes.GridNodes.Count, _triangulation.Triangles, _area.Points, _foundNodes.BorderNodes, _borderTemperature);
            _solution.FindGlobalMatrix();
            _solution.FindColumnTemperature();
            _result = _solution.Result;
        }
    }
}
