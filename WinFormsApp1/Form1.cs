using MKE;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        /// <summary>
        ///Точки, по котором задается область
        /// </summary>
        private PointD[] points;
        /// <summary>
        /// Коэффициенты теплообмена для каждой границы
        /// </summary>
        private double[] h;
        /// <summary>
        /// Поток тепла для каждой границы
        /// </summary>
        private double[] q;
        /// <summary>
        ///Температура окружающей среды с каждой границы
        /// </summary>
        private double[] t_inf;
        /// <summary>
        /// Счетчик шагов введения параметров 
        /// </summary>
        private int countStepEnterData = 0;
        /// <summary>
        ///Коэффициент теплопроводности в направлении оси х
        /// </summary>
        private int Kx;
        /// <summary>
        /// Коэффициент теплопроводности в направлении оси у
        /// </summary>
        private int Ky;
        /// <summary>
        ///Источник тепла внутри тела(области)
        /// </summary>
        private int Q;
        private bool flagDef = false;
        private int sizeGridChoice;

        private GraphicsMke grapMke;
        private Solution_Equation solutionEq;
        private Area area;
        private Triangulation triangulation;

        private readonly TextBox NumVertex = new();
        private readonly TextBox X = new();
        private readonly TextBox Y = new();
        private readonly Label NumVertexText = new();
        private readonly Label CoordinatesVertexText = new();
        private readonly Label XText = new();
        private readonly Label YText = new();
        private readonly Label qDis = new();
        private readonly Label TDis = new();
        private readonly Label hDis = new();
        private readonly Label QDis = new();
        private readonly Label KText = new();
        private readonly Button Enterdata = new();

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            InterfaceFirstStep();
        }
        /// <summary>
        /// Метод с тестовыми данными
        /// </summary>
        private void TestData()
        {
            KText.Visible = false;
            QDis.Visible = false;
            hDis.Visible = false;
            qDis.Visible = false;
            TDis.Visible = false;
            NumVertex.Text = 13.ToString();//

            points = new PointD[int.Parse(NumVertex.Text.ToString())];
            h = new double[int.Parse(NumVertex.Text.ToString())];
            q = new double[int.Parse(NumVertex.Text.ToString())];
            t_inf = new double[int.Parse(NumVertex.Text.ToString())];

            Kx = 3;
            Ky = 3;
            Q = 20;

            Kxx.Text = Kx.ToString();
            Kyy.Text = Ky.ToString();
            Qv.Text = Q.ToString();

          /*  points[0].X = 1;
             points[0].Y = 1;

             points[1].X = 4;
             points[1].Y = 1;

             points[2].X = 4;
             points[2].Y = 4;

             points[3].X = 1;
             points[3].Y = 4;*/

       /*    points[0].X = 1;
            points[0].Y = 0;

            points[1].X = 5;
            points[1].Y = 2;

            points[2].X = 5;
            points[2].Y = 3;

            points[3].X = 4;
            points[3].Y = 3;

            points[4].X = 3;
            points[4].Y = 4;

            points[5].X = 2;
            points[5].Y = 2;

            points[6].X = 0;
            points[6].Y = 3;*/


            h[0] = 150;
            t_inf[0] = 320;
            q[0] = 0;

            h[1] = 150;
            t_inf[1] = 350;
            q[1] = 0;

            h[2] = 150;
            t_inf[2] = 330;
            q[2] = 0;

            h[3] = 150;
            t_inf[3] = 300;
            q[3] = 0;

            h[4] = 150;
            t_inf[4] = 300;
            q[4] = 0;

            h[5] = 150;
            t_inf[5] = 300;
            q[5] = 0;

            h[6] = 150;
            t_inf[6] = 300;
            q[6] = 0;

             h[7] = 150;
              t_inf[7] = 300;
              q[7] = 0;

              h[8] = 150;
              t_inf[8] = 301;
              q[8] = 0;

              h[9] = 150;
              t_inf[9] = 300;
              q[9] = 0;

              h[10] = 150;
              t_inf[10] = 300;
              q[10] = 0;

              h[11] = 150;
              t_inf[11] = 300;
              q[11] = 0;

              h[12] = 150;
              t_inf[12] = 300;
              q[12] = 0;

              //
              points[0].X = 1;
              points[0].Y = 6;

              points[1].X = 2.5f;
              points[1].Y = 2;

              points[2].X = 4;
              points[2].Y = 3.5f;

              points[3].X = 6.5f;
              points[3].Y = 3.5f;

              points[4].X = 7.5f;
              points[4].Y = 0.5f;

              points[5].X = 9.5f;
              points[5].Y = 2;

              points[6].X = 9;
              points[6].Y = 4.5f;

              points[7].X = 13;
              points[7].Y = 4;

              points[8].X = 12.5f;
              points[8].Y = 7.5f;

              points[9].X = 9.5f;
              points[9].Y = 7.5f;

              points[10].X = 6;
              points[10].Y = 7;

              points[11].X = 5;
              points[11].Y = 8.5f;

              points[12].X = 4;
              points[12].Y = 6;


            area = new Area(points);
            area.ShiftingPointsArea();

           
           sizeGridChoice = 1;
           maxSizeGrid.Checked = true;

            triangulation = new Triangulation(area.Points, sizeGridChoice);
            triangulation.InitialPartitioning();
            triangulation.DelaunayTriangulation();
            triangulation.DeterminingNodeNumbers();

            grapMke = new GraphicsMke(XOY, triangulation.Triangles);
            grapMke.DisplayAllTriangles();
            sizeText.Text = "Size:";
            sizeText.Visible = true;
            maxSizeGrid.Visible = true;
            mediumSizeGrid.Visible = true;
            minSizeGrid.Visible = true;
            InterfaceFinalView();


            hval.Text = h[0].ToString();
            Tval.Text = t_inf[0].ToString();
            qval.Text = q[0].ToString();
        }
        /// <summary>
        /// Вид интерфейса на первом шаге работы программы
        /// </summary>
        private void InterfaceFirstStep()
        {
            maxSizeGrid.Visible = false;
            mediumSizeGrid.Visible = false;
            minSizeGrid.Visible = false;
            sizeText.Visible = false;

            TempNode.Visible = false;
            TempNodeText.Visible = false;
            TempNodeBut.Visible = false;

            NumNodes.Visible = false;
            NumNodesText.Visible = false;

            Grantext.Visible = false;
            Numtext.Visible = false;
            NumG.Visible = false;

            Show.Visible = false;
            Change.Visible = false;
            GetSolution.Visible = false;
            Save.Visible = false;
            EnableNodes.Visible = false;

            DataText.Text = "Введите данные:";
            DataText.Location = new Point(130, 15);
            DataText.Size = new Size(300, 30);

            Parametrs.Controls.Add(NumVertexText);
            NumVertexText.Text = "Количество вершин:";
            NumVertexText.Location = new Point(120, 50);
            NumVertexText.Font = new Font("Times New Roman", 15.0f);
            NumVertexText.Size = new Size(300, 30);

            Parametrs.Controls.Add(NumVertex);
            NumVertex.Location = new Point(180, 90);
            NumVertex.Font = new Font("Times New Roman", 15.0f);
            NumVertex.AutoSize = false;
            NumVertex.Size = new Size(70, 30);
            NumVertex.TextAlign = HorizontalAlignment.Center;


            Parametrs.Controls.Add(CoordinatesVertexText);
            CoordinatesVertexText.Text = "Координаты первой вершины:";
            CoordinatesVertexText.Location = new Point(80, 130);
            CoordinatesVertexText.Font = new Font("Times New Roman", 15.0f);
            CoordinatesVertexText.Size = new Size(400, 30);

            Parametrs.Controls.Add(XText);
            XText.Text = "X:";
            XText.Location = new Point(90, 170);
            XText.Font = new Font("Times New Roman", 15.0f);
            XText.Size = new Size(30, 30);

            Parametrs.Controls.Add(X);
            X.Location = new Point(120, 167);
            X.Font = new Font("Times New Roman", 15.0f);
            X.AutoSize = false;
            X.Size = new Size(70, 30);
            X.TextAlign = HorizontalAlignment.Center;


            Parametrs.Controls.Add(YText);
            YText.Text = "Y:";
            YText.Location = new Point(220, 170);
            YText.Font = new Font("Times New Roman", 15.0f);
            YText.Size = new Size(30, 30);

            Parametrs.Controls.Add(Y);
            Y.Location = new Point(250, 167);
            Y.Font = new Font("Times New Roman", 15.0f);
            Y.AutoSize = false;
            Y.Size = new Size(70, 30);
            Y.TextAlign = HorizontalAlignment.Center;


            QDis.Text = "Источник тепла внутри тела:";
            QDis.Location = new Point(70, 210);
            QDis.Font = new Font("Times New Roman", 15.0f);
            QDis.Size = new Size(300, 30);
            Parametrs.Controls.Add(QDis);

            Qt.Location = new Point(145, 250);
            Qv.AutoSize = false;
            Qv.Location = new Point(180, 250);
            Qv.Size = new Size(70, 30);


            hDis.Text = "Коэффициент теплообмена:";
            hDis.Location = new Point(100, 290);
            hDis.Font = new Font("Times New Roman", 15.0f);
            hDis.Size = new Size(300, 30);
            Parametrs.Controls.Add(hDis);

            htxt.Location = new Point(150, 330);
            hval.AutoSize = false;
            hval.Size = new Size(70, 30);
            hval.Location = new Point(180, 330);

            TDis.Text = "Температура окружающей среды:";
            TDis.Location = new Point(65, 370);
            TDis.Font = new Font("Times New Roman", 15.0f);
            TDis.Size = new Size(300, 30);
            Parametrs.Controls.Add(TDis);

            Ttxt.Location = new Point(150, 410);
            Tval.AutoSize = false;
            Tval.Location = new Point(180, 410);
            Tval.Size = new Size(70, 30);


            qDis.Text = "Поток тепла:";
            qDis.Location = new Point(150, 455);
            qDis.Font = new Font("Times New Roman", 15.0f);
            qDis.Size = new Size(300, 30);
            Parametrs.Controls.Add(qDis);

            qtxt.Location = new Point(150, 490);
            qval.AutoSize = false;
            qval.Location = new Point(180, 490);
            qval.Size = new Size(70, 30);



            KText.Text = "Коэффициенты теплопроводности:";
            KText.Location = new Point(70, 530);
            KText.Font = new Font("Times New Roman", 14.0f);
            KText.Size = new Size(300, 30);
            Parametrs.Controls.Add(KText);

            KxxText.Location = new Point(70, 565);
            Kxx.Location = new Point(120, 565);
            Kxx.Size = new Size(70, 30);
            Kxx.AutoSize = false;

            KyyText.Location = new Point(235, 560);
            Kyy.Location = new Point(285, 565);
            Kyy.Size = new Size(70, 30);
            Kyy.AutoSize = false;

            Parametrs.Controls.Add(Enterdata);
            Enterdata.Text = "Далее";
            Enterdata.Location = new Point(130, 605);
            Enterdata.Font = new Font("Times New Roman", 15.0f);
            Enterdata.Size = new Size(170, 50);
            Enterdata.BackColor = Color.FromArgb(157, 160, 163);

            Enterdata.Click += Enterdata_Click;
        }
        /// <summary>
        /// Вид интерфейса на последующих шагах ввода параметров, кроме последнего
        /// </summary>
        private void InterfaceOtherStep()
        {
            Kxx.Visible = false;
            Kyy.Visible = false;
            KxxText.Visible = false;
            KyyText.Visible = false;
            KText.Visible = false;
            NumVertexText.Visible = false;
            NumVertex.Visible = false;

            Qv.Visible = false;
            Qt.Visible = false;
            QDis.Visible = false;

            DataText.Text = "Введите данные:";
            DataText.Location = new Point(130, 15);

            CoordinatesVertexText.Text = "Координаты текущей вершины:";
            CoordinatesVertexText.Location = new Point(70, 60);
            CoordinatesVertexText.Font = new Font("Times New Roman", 15.0f);
            CoordinatesVertexText.Size = new Size(400, 30);

            XText.Location = new Point(90, 100);
            XText.Font = new Font("Times New Roman", 15.0f);
            X.Location = new Point(120, 97);

            YText.Location = new Point(220, 100);
            Y.Location = new Point(250, 97);

            hDis.Location = new Point(80, 140);
            htxt.Location = new Point(150, 180);
            hval.Location = new Point(180, 180);

            TDis.Location = new Point(65, 230);
            Ttxt.Location = new Point(150, 270);
            Tval.Location = new Point(180, 270);

            qDis.Location = new Point(150, 320);
            qtxt.Location = new Point(150, 360);
            qval.Location = new Point(180, 360);

            Enterdata.Location = new Point(140, 420);
            Enterdata.Size = new Size(150, 60);
        }
        /// <summary>
        /// Вид интерфейса на последнем шаге ввода параметров
        /// </summary>
        private void InterfaceLastStep()
        {
            maxSizeGrid.Visible = true;
            mediumSizeGrid.Visible = true;
            minSizeGrid.Visible = true;
            sizeText.Visible = true;

            Kxx.Visible = false;
            Kyy.Visible = false;
            KxxText.Visible = false;
            KyyText.Visible = false;
            KText.Visible = false;
            NumVertexText.Visible = false;
            NumVertex.Visible = false;

            Qv.Visible = false;
            Qt.Visible = false;
            QDis.Visible = false;

            DataText.Text = "Введите данные:";
            DataText.Location = new Point(130, 15);

            CoordinatesVertexText.Text = "Координаты текущей вершины:";
            CoordinatesVertexText.Location = new Point(70, 60);
            CoordinatesVertexText.Font = new Font("Times New Roman", 15.0f);
            CoordinatesVertexText.Size = new Size(400, 30);

            XText.Location = new Point(90, 100);
            XText.Font = new Font("Times New Roman", 15.0f);
            X.Location = new Point(120, 97);

            YText.Location = new Point(220, 100);
            Y.Location = new Point(250, 97);

            hDis.Location = new Point(80, 140);
            htxt.Location = new Point(150, 180);
            hval.Location = new Point(180, 180);

            TDis.Location = new Point(65, 230);
            Ttxt.Location = new Point(150, 270);
            Tval.Location = new Point(180, 270);

            qDis.Location = new Point(150, 320);
            qtxt.Location = new Point(150, 360);
            qval.Location = new Point(180, 360);

            sizeText.Text = "Выберите размер сетки:";
            sizeText.Location = new Point(90, 410);

            maxSizeGrid.Location = new Point(170, 450);
            mediumSizeGrid.Location = new Point(170, 490);
            minSizeGrid.Location = new Point(170, 530);

            Enterdata.Text = "Построить сетку";
            Enterdata.Location = new Point(140, 580);
        }
        /// <summary>
        /// Финальный вид интерфейса программы
        /// </summary>
        private void InterfaceFinalView()
        {
            maxSizeGrid.Enabled = false;
            mediumSizeGrid.Enabled = false;
            minSizeGrid.Enabled = false;

            NumVertexText.Visible = false;
            NumVertex.Visible = false;
            CoordinatesVertexText.Visible = false;

            XText.Visible = false;
            X.Visible = false;

            YText.Visible = false;
            Y.Visible = false;

            Qv.Visible = true;
            Qt.Visible = true;

            hDis.Visible = false;
            TDis.Visible = false;

            Kxx.Visible = true;
            Kyy.Visible = true;
            KxxText.Visible = true;
            KyyText.Visible = true;

            Enterdata.Visible = false;

            TempNode.Visible = true;
            TempNodeText.Visible = true;
            TempNodeBut.Visible = true;
            TempNodeBut.Enabled = false;

            NumNodes.Visible = true;
            NumNodes.Enabled = true;
            NumNodesText.Visible = true;

            EnableNodes.Visible = true;
            EnableNodes.Enabled = true;

            Grantext.Visible = true;
            Numtext.Visible = true;
            NumG.Visible = true;

            Show.Visible = true;
            Change.Visible = true;

            GetSolution.Visible = true;
            Save.Visible = true;
            Save.Enabled = false;

            DataText.Text = "Введенные параметры:";
            DataText.Location = new Point(93, 9);
            DataText.AutoSize = false;
            DataText.Size = new Size(224, 28);

            Qt.Location = new Point(129, 55);
            Qv.Location = new Point(166, 55);
            Qv.Size = new Size(100, 34);
            Qv.Enabled = false;

            htxt.Location = new Point(38, 192);
            hval.Location = new Point(71, 192);
            hval.Enabled = false;

            Ttxt.Location = new Point(159, 192);
            Tval.Location = new Point(191, 192);
            Tval.Enabled = false;

            qtxt.Location = new Point(267, 192);
            qval.Location = new Point(301, 192);
            qval.Enabled = false;

            KxxText.Location = new Point(38, 246);
            Kxx.Location = new Point(90, 246);
            Kxx.Enabled = false;

            KyyText.Location = new Point(191, 246);
            Kyy.Location = new Point(244, 246);
            Kyy.Enabled = false;

            sizeText.Text = "Size:";

            Show.Location = new Point(129, 297);
            Change.Location = new Point(129, 371);

            hval.Text = h[0].ToString();
            Tval.Text = t_inf[0].ToString();
            qval.Text = q[0].ToString();
            Kxx.Text = Kx.ToString();
            Kyy.Text = Ky.ToString();
            NumG.Text = "1";
        }
        /// <summary>
        /// Возвращает номер ближайшей границе к точке клика по панели, если до нее расстояние не больше константы, иначе -1
        /// </summary>
        /// <param name="pointOnPanel"></param>
        /// <returns></returns>
        private int PointOnLine(PointD pointOnPanel)
        {
            int numberBorder = 0;
            double distance;
            double minDistance = 100;

            pointOnPanel.X = (pointOnPanel.X - grapMke.PanelWidth / 10) / 38;
            pointOnPanel.Y = (-pointOnPanel.Y + 4 * grapMke.PanelHeight / 5) / 38;

            for (int i = 0; i < points.Length; i++)
            {
                if (i == points.Length - 1)
                {
                    if (Triangulation.ObtuseAngle(points[i], points[0], pointOnPanel))
                    {
                        distance = Triangulation.MinDistanceToPoint(points[i], points[0], pointOnPanel);
                    }
                    else
                    {
                        distance = Math.Abs((points[0].Y - points[i].Y) * pointOnPanel.X - (points[0].X - points[i].X) * pointOnPanel.Y + points[0].X * points[i].Y - points[0].Y * points[i].X) / Math.Sqrt((points[0].Y - points[i].Y) * (points[0].Y - points[i].Y) + (points[0].X - points[i].X) * (points[0].X - points[i].X));
                    }
                }
                else
                {
                    if (Triangulation.ObtuseAngle(points[i], points[i + 1], pointOnPanel))
                    {
                        distance = Triangulation.MinDistanceToPoint(points[i], points[i + 1], pointOnPanel);
                    }
                    else
                    {
                        distance = Math.Abs((points[i + 1].Y - points[i].Y) * pointOnPanel.X - (points[i + 1].X - points[i].X) * pointOnPanel.Y + points[i + 1].X * points[i].Y - points[i + 1].Y * points[i].X) / Math.Sqrt((points[i + 1].Y - points[i].Y) * (points[i + 1].Y - points[i].Y) + (points[i + 1].X - points[i].X) * (points[i + 1].X - points[i].X));
                    }
                }

                if (distance < minDistance)
                {
                    minDistance = distance;
                    numberBorder = i + 1;
                }
            }

            if (minDistance <= 0.1)
            {
                return numberBorder;
            }
            return -1;
        }
        /// <summary>
        /// Возвращает индекс ближайшего узла к точке клика по панели, если до него не большое расстояние или -1 иначе 
        /// </summary>
        /// <param name="pointOnPanel"></param>
        /// <returns></returns>
        private int PointCloseNode(PointD pointOnPanel)
        {
            double minDistance = 1000;
            int indexNode = 0;

            pointOnPanel.X = (pointOnPanel.X - grapMke.PanelWidth / 10) / 38;
            pointOnPanel.Y = (-pointOnPanel.Y + 4 * grapMke.PanelHeight / 5) / 38;

            for (int i = 0; i < triangulation.Nodes.Count; i++)
            {
                if (Triangulation.GetDistancePoints(triangulation.Nodes[i].Node, pointOnPanel) < minDistance)
                {
                    minDistance = Triangulation.GetDistancePoints(triangulation.Nodes[i].Node, pointOnPanel);
                    indexNode = i;
                }
            }

            if (minDistance < 0.1)
            {
                grapMke.DrawPoint(Color.FromArgb(27, 42, 207), triangulation.Nodes[indexNode].Node);
                grapMke.DrawString(triangulation.Nodes[indexNode].NumNode.ToString(), triangulation.Nodes[indexNode].Node);

                return indexNode;
            }
            return -1;
        }
        /// <summary>
        /// Отрисовывает границу по ее номеру
        /// </summary>
        /// <param name="numBorder"></param>
        private void ShowBorder(int numBorder)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (i == numBorder - 1)
                {
                    if (numBorder == points.Length)
                    {
                        grapMke.DrawLine(Color.FromArgb(237, 41, 57), points[numBorder - 1], points[0]);
                    }
                    else
                    {
                        grapMke.DrawLine(Color.FromArgb(237, 41, 57), points[numBorder - 1], points[numBorder]);

                    }
                }
                else
                {
                    if (i != points.Length - 1)
                    {
                        grapMke.DrawLine(Color.FromArgb(0, 0, 0), points[i], points[i + 1]);
                    }
                    else
                    {
                        grapMke.DrawLine(Color.FromArgb(0, 0, 0), points[i], points[0]);
                    }
                }
            }
        }
        /// <summary>
        /// Вводит данные с формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Enterdata_Click(object? sender, EventArgs e)
        {
            TestData();
            return;
            try
            {
                if (countStepEnterData == 0)
                {
                    points = new PointD[int.Parse(NumVertex.Text.ToString())];
                    h = new double[int.Parse(NumVertex.Text.ToString())];
                    q = new double[int.Parse(NumVertex.Text.ToString())];
                    t_inf = new double[int.Parse(NumVertex.Text.ToString())];

                    Kx = int.Parse(Kxx.Text);
                    Ky = int.Parse(Kyy.Text);
                    Q = int.Parse(Qv.Text);
                    InterfaceOtherStep();
                }
                else if (countStepEnterData == int.Parse(NumVertex.Text) - 1)
                {
                    if (maxSizeGrid.Checked)
                    {
                        sizeGridChoice = 1;
                    }
                    else if (mediumSizeGrid.Checked)
                    {
                        sizeGridChoice = 2;
                    }
                    else
                    {
                        sizeGridChoice = 3;
                    }
                }
                points[countStepEnterData].X = float.Parse(X.Text);
                points[countStepEnterData].Y = float.Parse(Y.Text);

                h[countStepEnterData] = double.Parse(hval.Text);
                q[countStepEnterData] = double.Parse(qval.Text);
                t_inf[countStepEnterData] = double.Parse(Tval.Text);


            }
            catch
            {
                MessageBox.Show("Проверьте введенные данные. \nЗначения параметров должны являться вещественными числами");

                if (countStepEnterData == 0)
                {
                    X.Text = null;
                    Y.Text = null;
                    Qv.Text = null;
                    NumVertex.Text = null;
                }
                else
                {
                    X.Text = null;
                    Y.Text = null;
                    hval.Text = null;
                    Tval.Text = null;
                    qval.Text = null;
                }
                countStepEnterData--;
            }

            if (countStepEnterData > 0 && countStepEnterData == int.Parse(NumVertex.Text) - 1)
            {
                area = new Area(points);
                area.ShiftingPointsArea();

                triangulation = new Triangulation(area.Points, sizeGridChoice);
                triangulation.InitialPartitioning();
                triangulation.DelaunayTriangulation();
                triangulation.DeterminingNodeNumbers();

                grapMke = new GraphicsMke(XOY, triangulation.Triangles);
                grapMke.DisplayAllTriangles();

                InterfaceFinalView();

                hval.Text = h[0].ToString();
                Tval.Text = t_inf[0].ToString();
                qval.Text = q[0].ToString();
            }
            else
            {
                X.Text = null;
                Y.Text = null;
                hval.Text = null;
                Tval.Text = null;
                qval.Text = null;
            }

            if (countStepEnterData > 0 && countStepEnterData == int.Parse(NumVertex.Text) - 2)
            {
                InterfaceLastStep();
            }

            countStepEnterData++;
        }
        /// <summary>
        /// Вывод параметры уравнения и показывает выбранную границу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Show_Click(object sender, EventArgs e)
        {
             bool result  = int.TryParse(NumG.Text, out int numBorder);
 
            if(result)
            {
                if (numBorder > 0 && numBorder < points.Length)
                {
                    ShowBorder(numBorder);

                    if (numBorder != 0)
                    {
                        hval.Text = h[numBorder - 1].ToString();
                        Tval.Text = t_inf[numBorder - 1].ToString();
                        qval.Text = q[numBorder - 1].ToString();
                    }
                }
                else
                {
                    hval.Text = h[0].ToString();
                    Tval.Text = t_inf[0].ToString();
                    qval.Text = q[0].ToString();
                    NumG.Text = "1";
                    MessageBox.Show("Границы с таким номером не существует");
                }
            }
            else
            {
                NumG.Text = "1";
                MessageBox.Show("Номер границы должен являться целым числом");
            }
        }
        /// <summary>
        /// Изменяет параметры уравнения на заданной границе
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Change_Click(object sender, EventArgs e)
        {
            bool result  = int.TryParse(NumG.Text, out int numBorder);
 
            if(result)
            {
                if (numBorder > 0 && numBorder < points.Length)
                {
                    if (Change.Text == "Изменить")
                    {
                        Change.Text = "Применить";

                        hval.Enabled = true;
                        Tval.Enabled = true;
                        qval.Enabled = true;
                        Qv.Enabled = true;
                        Kxx.Enabled = true;
                        Kyy.Enabled = true;
                        maxSizeGrid.Enabled = true;
                        mediumSizeGrid.Enabled = true;
                        minSizeGrid.Enabled = true;
                        GetSolution.Enabled = false;
                        EnableNodes.Enabled = false;
                        NumNodes.Enabled = false;
                    }
                    else
                    {
                        if (maxSizeGrid.Checked)
                        {
                            sizeGridChoice = 1;
                        }
                        else if (mediumSizeGrid.Checked)
                        {
                            sizeGridChoice = 2;
                        }
                        else if (minSizeGrid.Checked)
                        {
                            sizeGridChoice = 3;
                        }

                        triangulation = new Triangulation(area.Points, sizeGridChoice);
                        triangulation.InitialPartitioning();
                        triangulation.DelaunayTriangulation();
                        triangulation.DeterminingNodeNumbers();

                        grapMke = new GraphicsMke(XOY, triangulation.Triangles);
                        grapMke.DisplayAllTriangles();

                        h[numBorder] = double.Parse(hval.Text);
                        t_inf[numBorder] = double.Parse(Tval.Text);
                        q[numBorder] = double.Parse(qval.Text);

                        Q = int.Parse(Qv.Text);
                        Kx = int.Parse(Kxx.Text);
                        Ky = int.Parse(Kyy.Text);

                        Change.Text = "Изменить";

                        hval.Enabled = false;
                        Tval.Enabled = false;
                        qval.Enabled = false;
                        Qv.Enabled = false;
                        Kxx.Enabled = false;
                        Kyy.Enabled = false;
                        maxSizeGrid.Enabled = false;
                        mediumSizeGrid.Enabled = false;
                        minSizeGrid.Enabled = false;
                        GetSolution.Enabled = true;
                        EnableNodes.Enabled = true;
                        NumNodes.Enabled = true;
                    }
                }
                else
                {
                    NumG.Text = "1";
                    hval.Text = h[0].ToString();
                    Tval.Text = t_inf[0].ToString();
                    qval.Text = q[0].ToString();
                    MessageBox.Show("Границы с таким номером не существует");
                }
            }
            else
            {
                NumG.Text = "1";
                MessageBox.Show("Номер границы должен являться целым числом");
            }
        }
        /// <summary>
        /// Получает решение уравнения
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetSolution_Click(object sender, EventArgs e)
        {
            solutionEq = new Solution_Equation(Kx, Ky, Q, h, q, t_inf, triangulation.Triangles, triangulation.Nodes, area.Points);
            solutionEq.FindGlobalMatrix();
            solutionEq.FindColumnTemperature();

            TempNodeBut.Enabled = true;
            EnableNodes.Enabled = true;
            Change.Enabled = false;
            Save.Enabled = true;
        }
        /// <summary>
        ///Обработка клика по панели. Отображание границы области или узла сетки, если они находятся достаточно близко к клику
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XOY_MouseClick(object sender, MouseEventArgs e)
        {
            if (countStepEnterData != 0)
            {
                PointD pointOnPanel = new();
                int numBorder;

                pointOnPanel.X = e.Location.X;
                pointOnPanel.Y = e.Location.Y;

                if (PointCloseNode(pointOnPanel) != -1)
                {
                    NumNodes.Text = (PointCloseNode(pointOnPanel) + 1).ToString();
                }

                numBorder = PointOnLine(pointOnPanel);

                if (numBorder != -1)
                {
                    ShowBorder(numBorder);

                    NumG.Text = numBorder.ToString();
                    hval.Text = h[numBorder - 1].ToString();
                    Tval.Text = t_inf[numBorder - 1].ToString();
                    qval.Text = q[numBorder - 1].ToString();
                }
            }
        }
        /// <summary>
        /// Отрисовывает узлы сетки, номера которых были введены на панели
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnableNodes_Click(object sender, EventArgs e)
        {
            string nodesFirstPart = "", nodesLastPart = "";
            int nodesIndex = NumNodes.Text.Length;
            flagDef = false;

            for (int i = 0; i < NumNodes.Text.Length; i++)
            {
                if (NumNodes.Text[i] == '-')
                {
                    nodesIndex = i;
                    flagDef = true;
                    break;
                }
            }
            for (int i = 0; i < NumNodes.Text.Length; i++)
            {
                if (i < nodesIndex)
                {
                    nodesFirstPart += NumNodes.Text[i];
                }
                else if (i > nodesIndex)
                {
                    nodesLastPart += NumNodes.Text[i];
                }
            }

            if (nodesLastPart != "")
            {
                try
                {
                    if ((int.Parse(nodesFirstPart) < 1 || int.Parse(nodesFirstPart) > triangulation.Nodes.Count) || (int.Parse(nodesLastPart) < 1 || int.Parse(nodesLastPart) > triangulation.Nodes.Count))
                    {
                        MessageBox.Show("Таких узлов не существует");
                    }
                    else
                    {
                        grapMke.Clear(XOY);
                        for (int i = 0; i < triangulation.Triangles.Count; i++)
                        {
                            grapMke.DisplayTriangle(triangulation.Triangles[i]);
                        }

                        for (int i = int.Parse(nodesFirstPart); i <= int.Parse(nodesLastPart); i++)
                        {
                            grapMke.DrawString(i.ToString(), triangulation.Nodes[i - 1].Node);
                            grapMke.DrawPoint(Color.FromArgb(27, 42, 207), triangulation.Nodes[i - 1].Node);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Номера узлов должны являться целыми числами");
                    NumNodes.Text = "0";
                    return;
                }
            }
            else
            {
                try
                {
                    if (nodesFirstPart != "")
                    {
                        int numNode = int.Parse(nodesFirstPart);

                        if (numNode < 1 || numNode > triangulation.Nodes.Count)
                        {
                            MessageBox.Show("Такого узла не существует");
                        }
                        else
                        {
                            grapMke.Clear(XOY);
                            for (int i = 0; i < triangulation.Triangles.Count; i++)
                            {
                                grapMke.DisplayTriangle(triangulation.Triangles[i]);
                            }
                            grapMke.DrawString(NumNodes.Text, triangulation.Nodes[int.Parse(nodesFirstPart) - 1].Node);
                            grapMke.DrawPoint(Color.FromArgb(27, 42, 207), triangulation.Nodes[int.Parse(nodesFirstPart) - 1].Node);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Введите номера узлов");
                    }
                }
                catch
                {
                    MessageBox.Show("Номер узла должен являться целым числом");
                    NumNodes.Text = "0";
                }
            }
        }
        /// <summary>
        /// Сохраняет полученные результаты в файл
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog result = new()
            {
                Filter = "Текстовые файлы(*.txt) | *.txt | All files(*.*) | *.* "
            };

            if (result.ShowDialog() == DialogResult.OK)
            {
                string path = result.FileName;
                using StreamWriter stream = new(path, false);
                stream.WriteLine("Номер узла      Температура в узле (°C)");
                for (int i = 0; i < triangulation.Nodes.Count; i++)
                {
                    stream.WriteLine("_____________|_________________________|");
                    stream.WriteLine((i + 1).ToString() + "                       " + Math.Round((solutionEq.Result[i] - 273), 2));
                }
                stream.Write("_____________|_________________________|");
            }
        }
        /// <summary>
        /// Показывает температуру в указанном узле
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TempNodeBut_Click(object sender, EventArgs e)
        {
            TempNode.Enabled = false;

            if (!flagDef)
            {
                try
                {
                    if (NumNodes.Text != "")
                    {
                        int numNodes = int.Parse(NumNodes.Text);
                        if (numNodes < 1 || numNodes > triangulation.Nodes.Count)
                        {
                            MessageBox.Show("Такого узла не существует");
                            NumNodes.Text = "1";
                        }
                        else
                        {
                            TempNode.Text = Math.Round((solutionEq.Result[int.Parse(NumNodes.Text) - 1] - 273), 2).ToString();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Введите номер узла");
                    }
                }
                catch
                {
                    MessageBox.Show("Номер узла должен являться целым числом");
                    NumNodes.Text = "1";
                }
            }
            else
            {
                MessageBox.Show("Укажите номер одного узла");
            }
        }
    }
}