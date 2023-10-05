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
        public int PanelWidth { get; private set; }
        /// <summary>
        ///Длина панели
        /// </summary>
        public int PanelHeight { get; private set; }
        private Panel XOY { get; set; }
        /// <summary>
        ///Набор треугольников, образующих триангуляцию области 
        /// </summary>
        private List<Triangle> Triangles { get; set; }
        private readonly Graphics graphics;            
        public GraphicsMke(Panel XOY, List<Triangle> triangulation)
        {
            this.XOY = XOY;
            PanelHeight = XOY.Height;
            PanelWidth = XOY.Width;
            Triangles = triangulation;
            graphics = XOY.CreateGraphics();
        }
        /// <summary>
        ///Отрисовывает на панеле данный треугольник
        /// </summary>
        /// <param name="XOY"></param>
        /// <param name="triangle"></param>
        public void DisplayTriangle(Triangle triangle)
        {
            graphics.DrawLine(new Pen(Color.FromArgb(0, 0, 0), 2f), PanelWidth / 10 + 38 * (float)triangle.FirstVertex.X, 4 * PanelHeight / 5 - 38 * (float)triangle.FirstVertex.Y, PanelWidth / 10 + 38 * (float)triangle.SecondVertex.X, 4 * PanelHeight / 5 - 38 * (float)triangle.SecondVertex.Y);
            graphics.DrawLine(new Pen(Color.FromArgb(0, 0, 0), 2f), PanelWidth / 10 + 38 * (float)triangle.FirstVertex.X, 4 * PanelHeight / 5 - 38 * (float)triangle.FirstVertex.Y, PanelWidth / 10 + 38 * (float)triangle.ThirdVertex.X, 4 * PanelHeight / 5 - 38 * (float)triangle.ThirdVertex.Y);
            graphics.DrawLine(new Pen(Color.FromArgb(0, 0, 0), 2f), PanelWidth / 10 + 38 * (float)triangle.SecondVertex.X, 4 * PanelHeight / 5 - 38 * (float)triangle.SecondVertex.Y, PanelWidth / 10 + 38 * (float)triangle.ThirdVertex.X, 4 * PanelHeight / 5 - 38 * (float)triangle.ThirdVertex.Y);           
        }
        public void DisplayTriangle(Triangle triangle, Color color)
        {
            graphics.DrawLine(new Pen(color, 2f), PanelWidth / 10 + 38 * (float)triangle.FirstVertex.X, 4 * PanelHeight / 5 - 38 * (float)triangle.FirstVertex.Y, PanelWidth / 10 + 38 * (float)triangle.SecondVertex.X, 4 * PanelHeight / 5 - 38 * (float)triangle.SecondVertex.Y);
            graphics.DrawLine(new Pen(color, 2f), PanelWidth / 10 + 38 * (float)triangle.FirstVertex.X, 4 * PanelHeight / 5 - 38 * (float)triangle.FirstVertex.Y, PanelWidth / 10 + 38 * (float)triangle.ThirdVertex.X, 4 * PanelHeight / 5 - 38 * (float)triangle.ThirdVertex.Y);
            graphics.DrawLine(new Pen(color, 2f), PanelWidth / 10 + 38 * (float)triangle.SecondVertex.X, 4 * PanelHeight / 5 - 38 * (float)triangle.SecondVertex.Y, PanelWidth / 10 + 38 * (float)triangle.ThirdVertex.X, 4 * PanelHeight / 5 - 38 * (float)triangle.ThirdVertex.Y);
        }
        /// <summary>
        ///Отрисовывает  на панели все треугольники
        /// </summary>
        public void DisplayAllTriangles()
        {
            Clear(XOY);
            for (int i = 0; i < Triangles.Count; i++)
            {
                if (i == 0)
                {
                    DisplayTriangle(Triangles[i]);
                }
                else
                {
                    DisplayTriangle(Triangles[i]);
                }
            }
        }
        /// <summary>
        ///Рисует линию на панели, заданную двумя точками
        /// </summary>
        /// <param name="color"></param>
        /// <param name="firstPoint"></param>
        /// <param name="secondPoint"></param>
        public void DrawLine(Color color, PointD firstPoint, PointD secondPoint)
        {
            graphics.DrawLine(new Pen(color, 2f), PanelWidth / 10 + 38 * (float)firstPoint.X, 4 * PanelHeight / 5 - 38 * (float)firstPoint.Y, PanelWidth / 10 + 38 * (float)secondPoint.X, 4 * PanelHeight / 5 - 38 * (float)secondPoint.Y);
        }
        /// <summary>
        ///Отрисовывает заданную строку на панели, в указанной точке
        /// </summary>
        /// <param name="str"></param>
        /// <param name="point"></param>
        public void DrawString(string str, PointD point)
        {
            graphics.DrawString(str, new Font(FontFamily.GenericSansSerif, 12f), new SolidBrush(Color.Red), PanelWidth / 10 + 38 * (float)point.X, 4 * PanelHeight / 5 - 38 * (float)point.Y);                                                                                                                                                
        }
        /// <summary>
        ///Отрисовывает точку на панели
        /// </summary>
        /// <param name="color"></param>
        /// <param name="point"></param>
        public void DrawPoint(Color color, PointD point)
        {
            graphics.DrawEllipse(new Pen(color, 3.0f), PanelWidth / 10 + 38 * (float)point.X, 4 * PanelHeight / 5 - 38 * (float)point.Y, 3, 3);
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
