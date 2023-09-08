namespace MKE
{
    /// <summary>
    /// Класс, в котором реализуется работа с графическим отображением элементом на панели
    /// </summary>
    internal class GraphicsMke
    {
        /// <summary>
        ///Ширина панели
        /// </summary>
        public int panelWidth;
        /// <summary>
        ///Длина панели
        /// </summary>
        public int panelHeight;
        private Graphics graphics;
        /// <summary>
        ///Набор треугольников, образующих триангуляцию области 
        /// </summary>
        private List<Triangle> triangles { get; set; }
        public GraphicsMke(Panel XOY, List<Triangle> triangulation)
        {
            panelHeight = XOY.Height;
            panelWidth = XOY.Width;
            triangles = triangulation;
            graphics = XOY.CreateGraphics();
        }

        /// <summary>
        ///Отрисовывает на панеле данный треугольник
        /// </summary>
        /// <param name="XOY"></param>
        /// <param name="triangle"></param>
        public void DisplayTriangle(Triangle triangle)
        {
            graphics.DrawLine(new Pen(Color.FromArgb(0, 0, 0), 2f), panelWidth / 10 + 38 * triangle.vertex1.X, 4 * panelHeight / 5 - 38 * triangle.vertex1.Y, panelWidth / 10 + 38 * triangle.vertex2.X, 4 * panelHeight / 5 - 38 * triangle.vertex2.Y);
            graphics.DrawLine(new Pen(Color.FromArgb(0, 0, 0), 2f), panelWidth / 10 + 38 * triangle.vertex1.X, 4 * panelHeight / 5 - 38 * triangle.vertex1.Y, panelWidth / 10 + 38 * triangle.vertex3.X, 4 * panelHeight / 5 - 38 * triangle.vertex3.Y);
            graphics.DrawLine(new Pen(Color.FromArgb(0, 0, 0), 2f), panelWidth / 10 + 38 * triangle.vertex2.X, 4 * panelHeight / 5 - 38 * triangle.vertex2.Y, panelWidth / 10 + 38 * triangle.vertex3.X, 4 * panelHeight / 5 - 38 * triangle.vertex3.Y);
        }
        /// <summary>
        ///Отрисовывает  на панели все треугольники
        /// </summary>
        public void DisplayAllTriangles()
        {
            for (int i = 0; i < triangles.Count; i++)
            {
                if (i == 0)
                {
                    DisplayTriangle(triangles[i]);
                }
                else
                {
                    DisplayTriangle(triangles[i]);
                }
            }
        }
        /// <summary>
        ///Рисует линию на панели, заданную двумя точками
        /// </summary>
        /// <param name="color"></param>
        /// <param name="firstPoint"></param>
        /// <param name="secondPoint"></param>
        public void DrawLine(Color color, PointF firstPoint, PointF secondPoint)
        {
            graphics.DrawLine(new Pen(color, 2f), panelWidth / 10 + 38 * firstPoint.X, 4 * panelHeight / 5 - 38 * firstPoint.Y, panelWidth / 10 + 38 * secondPoint.X, 4 * panelHeight / 5 - 38 * secondPoint.Y);
        }
        /// <summary>
        ///Отрисовывает заданную строку на панели, в указанной точке
        /// </summary>
        /// <param name="str"></param>
        /// <param name="point"></param>
        public void DrawString(string str, PointF point)
        {
            graphics.DrawString(str, new Font(FontFamily.GenericSansSerif, 12f), new SolidBrush(Color.Red), panelWidth / 10 + 38 * point.X, 4 * panelHeight / 5 - 38 * point.Y);                                                                                                                                                
        }
        /// <summary>
        ///Отрисовывает точку на панели
        /// </summary>
        /// <param name="color"></param>
        /// <param name="point"></param>
        public void DrawPoint(Color color, PointF point)
        {
            graphics.DrawEllipse(new Pen(color, 2.0f), panelWidth / 10 + 38 * point.X, 4 * panelHeight / 5 - 38 * point.Y, 3, 3);
        }
        /// <summary>
        ///Очищает задний фон панели
        /// </summary>
        public void Clear(Panel panel)
        {
            graphics.Clear(panel.BackColor);
        }
    }
}
