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
        public PointF[] points { get; set; }      
        public Area(PointF[] points) => this.points = points;

        /// <summary>
        /// Выполняется сдвиг области в I координатную четверть.
        /// </summary>
        public void ShiftingPointsArea()
        {
            PointF Shift = new PointF();
            Shift.X = 0; Shift.Y = 0;

            for (int i = 0; i < this.points.Length; i++)
            {
                if (points[i].X <= Shift.X)
                {
                    Shift.X = points[i].X;
                }

                if (points[i].Y <= Shift.Y)
                {
                    Shift.Y = points[i].Y;
                }
            }

            for (int i = 0; i < points.Length; i++)
            {
                points[i].X += Math.Abs(Shift.X);
                points[i].Y += Math.Abs(Shift.Y);
            }
        }
    }
}
