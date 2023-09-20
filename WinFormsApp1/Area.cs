using System.Collections.Generic;

namespace MKE
{
    /// <summary>
/// Этот класс является заданной областью
/// </summary>
    public class Area
    {
        /// <summary>
        /// Начальные точки сетки
        /// </summary>
        public PointD[] Points { get;  set; }
        public Area(PointD[] points) => Points = points;

        /// <summary>
        /// Выполняется сдвиг области в I координатную четверть.
        /// </summary>
        public void ShiftingPointsArea()
        {
            PointD Shift = new();
            Shift.X = 0; Shift.Y = 0;

            for (int i = 0; i < Points.Length; i++)
            {
                if (Points[i].X <= Shift.X)
                {
                    Shift.X = Points[i].X;
                }

                if (Points[i].Y <= Shift.Y)
                {
                    Shift.Y = Points[i].Y;
                }
            }

            for (int i = 0; i < Points.Length; i++)
            {
                Points[i].X += Math.Abs(Shift.X);
                Points[i].Y += Math.Abs(Shift.Y);
            }
        }      
    }
}
