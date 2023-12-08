using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKE
{
    public class Area
    {
        private readonly PointD[] _points;
        public PointD[] Points { get => _points; }
        public Area(PointD[] points) => _points = points;
        public void ShiftingPointsArea()
        {
            PointD Shift = new()
            {
                X = 0,
                Y = 0
            };

            for (int i = 0; i < _points.Length; i++)
            {
                if (_points[i].X <= Shift.X)
                {
                    Shift.X = _points[i].X;
                }

                if (_points[i].Y <= Shift.Y)
                {
                    Shift.Y = _points[i].Y;
                }
            }

            if (Shift.X != 0 && Shift.Y != 0)
            {
                for (int i = 0; i < _points.Length; i++)
                {
                    _points[i].X += Math.Abs(Shift.X);
                    _points[i].Y += Math.Abs(Shift.Y);
                }
            }
        }
    }
}
