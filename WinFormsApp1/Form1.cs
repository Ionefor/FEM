using MKE;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        /// <summary>
        ///�����, �� ������� �������� �������
        /// </summary>
        PointF[] points;
        /// <summary>
        /// ������������ ����������� ��� ������ �������
        /// </summary>
        double[] h;
        /// <summary>
        /// ����� ����� ��� ������ �������
        /// </summary>
        double[] q;
        /// <summary>
        ///����������� ���������� ����� � ������ �������
        /// </summary>
        double[] t_inf;      
        /// <summary>
        /// ������� ����� �������� ���������� 
        /// </summary>
        int count = 0;
        /// <summary>
        ///����������� ���������������� � ����������� ��� �
        /// </summary>
        int Kx;
        /// <summary>
        /// ����������� ���������������� � ����������� ��� �
        /// </summary>
        int Ky;
        /// <summary>
        ///�������� ����� ������ ����(�������)
        /// </summary>
        int Q;
        bool flagDef = false;

        GraphicsMke grapMke;
        Solution_Equation solutionEq;
        Area area;
        Triangulation triangulation;

        TextBox NumVertex = new TextBox();
        Label NumVertexText = new Label();
        Label CoordinatesVertexText = new Label();

        Label XText = new();
        Label YText = new Label();       
        TextBox X = new TextBox();
        TextBox Y = new TextBox();

        Label qDis = new Label();
        Label TDis = new Label();
        Label hDis = new Label();
        Label QDis = new Label();
        Label KText = new Label();

        Button Enterdata = new Button();

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            InterfaceFirstStep();
        }

        /// <summary>
        /// ��� ���������� �� ������ ���� ������ ���������
        /// </summary>
        public void InterfaceFirstStep()
        {
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

            DataText.Text = "������� ������:";
            DataText.Location = new Point(130, 15);
            DataText.Size = new Size(300, 30);

            Parametrs.Controls.Add(NumVertexText);
            NumVertexText.Text = "���������� ������:";
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
            CoordinatesVertexText.Text = "���������� ������ �������:";
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


            QDis.Text = "�������� ����� ������ ����:";
            QDis.Location = new Point(70, 210);
            QDis.Font = new Font("Times New Roman", 15.0f);
            QDis.Size = new Size(300, 30);
            Parametrs.Controls.Add(QDis);

            Qt.Location = new Point(145, 250);
            Qv.AutoSize = false;
            Qv.Location = new Point(180, 250);
            Qv.Size = new Size(70, 30);


            hDis.Text = "����������� �����������:";
            hDis.Location = new Point(100, 290);
            hDis.Font = new Font("Times New Roman", 15.0f);
            hDis.Size = new Size(300, 30);
            Parametrs.Controls.Add(hDis);

            htxt.Location = new Point(150, 330);
            hval.AutoSize = false;
            hval.Size = new Size(70, 30);
            hval.Location = new Point(180, 330);

            TDis.Text = "����������� ���������� �����:";
            TDis.Location = new Point(65, 370);
            TDis.Font = new Font("Times New Roman", 15.0f);
            TDis.Size = new Size(300, 30);
            Parametrs.Controls.Add(TDis);

            Ttxt.Location = new Point(150, 410);
            Tval.AutoSize = false;
            Tval.Location = new Point(180, 410);
            Tval.Size = new Size(70, 30);


            qDis.Text = "����� �����:";
            qDis.Location = new Point(150, 455);
            qDis.Font = new Font("Times New Roman", 15.0f);
            qDis.Size = new Size(300, 30);
            Parametrs.Controls.Add(qDis);

            qtxt.Location = new Point(150, 490);
            qval.AutoSize = false;
            qval.Location = new Point(180, 490);
            qval.Size = new Size(70, 30);



            KText.Text = "������������ ����������������:";
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
            Enterdata.Text = "�����";
            Enterdata.Location = new Point(130, 605);
            Enterdata.Font = new Font("Times New Roman", 15.0f);
            Enterdata.Size = new Size(170, 50);
            Enterdata.BackColor = Color.FromArgb(157, 160, 163);

            Enterdata.Click += Enterdata_Click;
        }
        /// <summary>
        /// ��� ���������� �� ����������� ����� ������ ���������
        /// </summary>
        public void InterfaceOtherStep()
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

            DataText.Text = "������� ������:";
            DataText.Location = new Point(130, 15);

            CoordinatesVertexText.Text = "���������� ������� �������:";
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
        /// ��������� ��� ���������� ���������
        /// </summary>
        public void InterfaceFinalView()
        {
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
            NumNodes.Enabled = false;
            NumNodesText.Visible = true;

            EnableNodes.Visible = true;
            EnableNodes.Enabled = false;

            Grantext.Visible = true;
            Numtext.Visible = true;
            NumG.Visible = true;

            Show.Visible = true;
            Change.Visible = true;

            GetSolution.Visible = true;
            Save.Visible = true;

            DataText.Text = "��������� ���������:";
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
        /// ���������� ����� ��������� ������� � ����� ����� �� ������, ���� �� ��� ���������� �� ������ ���������, ����� -1
        /// </summary>
        /// <param name="pointOnPanel"></param>
        /// <returns></returns>
        public int PointOnLine(PointF pointOnPanel)
        {
            int numberBorder = 0;
            double distance;
            double minDistance = 100;

            pointOnPanel.X = (pointOnPanel.X - grapMke.panelWidth / 10) / 38;
            pointOnPanel.Y = (-pointOnPanel.Y + 4 * grapMke.panelHeight / 5) / 38;

            for (int i = 0; i < points.Length; i++)
            {
                if (i == points.Length - 1)
                {
                    if (triangulation.ObtuseAngle(points[i], points[0], pointOnPanel))
                    {
                        distance = triangulation.MinDistanceToPoint(points[i], points[0], pointOnPanel);
                    }
                    else
                    {
                        distance = Math.Abs((points[0].Y - points[i].Y) * pointOnPanel.X - (points[0].X - points[i].X) * pointOnPanel.Y + points[0].X * points[i].Y - points[0].Y * points[i].X) / Math.Sqrt((points[0].Y - points[i].Y) * (points[0].Y - points[i].Y) + (points[0].X - points[i].X) * (points[0].X - points[i].X));
                    }
                }
                else
                {
                    if (triangulation.ObtuseAngle(points[i], points[i + 1], pointOnPanel))
                    {
                        distance = triangulation.MinDistanceToPoint(points[i], points[i + 1], pointOnPanel);
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
        /// ���������� ������ ���������� ���� � ����� ����� �� ������, ���� �� ���� �� ������� ���������� ��� -1 ����� 
        /// </summary>
        /// <param name="pointOnPanel"></param>
        /// <returns></returns>
        public int PointCloseNode(PointF pointOnPanel)
        {
            double minDistance = 1000;
            int indexNode = 0;

            pointOnPanel.X = (pointOnPanel.X - grapMke.panelWidth / 10) / 38;
            pointOnPanel.Y = (-pointOnPanel.Y + 4 * grapMke.panelHeight / 5) / 38;

            for (int i = 0; i < triangulation.nodes.Count; i++)
            {
                if (triangulation.GetDistancePoints(triangulation.nodes[i].Node, pointOnPanel) < minDistance)
                {
                    minDistance = triangulation.GetDistancePoints(triangulation.nodes[i].Node, pointOnPanel);
                    indexNode = i;
                }
            }

            if (minDistance < 0.1)
            {
                grapMke.DrawPoint(Color.FromArgb(27, 42, 207), triangulation.nodes[indexNode].Node);
                grapMke.DrawString(triangulation.nodes[indexNode].NumNode.ToString(), triangulation.nodes[indexNode].Node);

                return indexNode;
            }
            return -1;
        }
        /// <summary>
        /// ������������ ������� �� �� ������
        /// </summary>
        /// <param name="numBorder"></param>
        public void ShowBorder(int numBorder)
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
        /// ������ ������ � �����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Enterdata_Click(object? sender, EventArgs e)
        {
            try
            {
                if (count == 0)
                {
                    InterfaceOtherStep();
                    NumVertex.Text = 13.ToString();//

                    points = new PointF[int.Parse(NumVertex.Text.ToString())];
                    h = new double[int.Parse(NumVertex.Text.ToString())];
                    q = new double[int.Parse(NumVertex.Text.ToString())];
                    t_inf = new double[int.Parse(NumVertex.Text.ToString())];

                    Kx = int.Parse(Kxx.Text);
                    Ky = int.Parse(Kyy.Text);
                    Q = int.Parse(Qv.Text);
                    
                }
                points[count].X = float.Parse(X.Text);
                points[count].Y = float.Parse(Y.Text);

                h[count] = double.Parse(hval.Text);
                q[count] = double.Parse(qval.Text);
                t_inf[count] = double.Parse(Tval.Text);
            }
            catch
            {
                /* MessageBox.Show("��������� ��������� ������. \n�������� ���������� ������ �������� ������������� �������");

                if (Count == 0)
                {
                    X.Text = null;
                    Y.Text = null;
                    Q.Text = null;
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
                count--;
                */


                count = int.Parse(NumVertex.Text);
                Qv.Text = "20";
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
            }

            if (count > 0 && count == int.Parse(NumVertex.Text))
            {
                area = new Area(points);
                area.ShiftingPointsArea();

                triangulation = new Triangulation(area.points);
                triangulation.InitialPartitioning();
                triangulation.Delaunaytriangulation();
                triangulation.DeterminingNodeNumbers();

                
                grapMke = new GraphicsMke(XOY, triangulation.triangulation);
                grapMke.DisplayAllTriangles();

                InterfaceFinalView();
            }

            if (count > 0 && count == int.Parse(NumVertex.Text) - 2)
            {
                Enterdata.Text = "��������� �����";
            }

            X.Text = null;
            Y.Text = null;
            count++;
        }
        /// <summary>
        /// ����� ��������� ��������� � ���������� ��������� �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Show_Click(object sender, EventArgs e)
        {
            try
            {
                int numBorder = int.Parse(NumG.Text);

                if (numBorder < 0 || numBorder > points.Length)
                {
                    MessageBox.Show("������� � ����� ������� �� ����������");
                    NumG.Text = "1";
                }
                else
                {
                    ShowBorder(numBorder);

                    if (numBorder != 0)
                    {
                        hval.Text = h[numBorder - 1].ToString();
                        Tval.Text = t_inf[numBorder - 1].ToString();
                        qval.Text = q[numBorder - 1].ToString();
                    }
                    else
                    {
                        hval.Text = h[0].ToString();
                        Tval.Text = t_inf[0].ToString();
                        qval.Text = q[0].ToString();
                        NumG.Text = "1";
                    }
                }
            }
            catch
            {
                MessageBox.Show("����� ������� ������ �������� ����� ������");
            }
        }
        /// <summary>
        /// �������� ��������� ��������� �� �������� �������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Change_Click(object sender, EventArgs e)
        {
            int numBorder = int.Parse(NumG.Text) - 1;
            if (Change.Text == "��������")
            {
                Change.Text = "���������";
                hval.Enabled = true;
                Tval.Enabled = true;
                qval.Enabled = true;
                Qv.Enabled = true;
                Kxx.Enabled = true;
                Kyy.Enabled = true;
            }
            else
            {
                h[numBorder] = double.Parse(hval.Text);
                t_inf[numBorder] = double.Parse(Tval.Text);
                q[numBorder] = double.Parse(qval.Text);

                Q = int.Parse(Qv.Text);
                Kx = int.Parse(Kxx.Text);
                Ky = int.Parse(Kyy.Text);

                Change.Text = "��������";

                hval.Enabled = false;
                Tval.Enabled = false;
                qval.Enabled = false;
                Qv.Enabled = false;
                Kxx.Enabled = false;
                Kyy.Enabled = false;
            }
        }
        /// <summary>
        /// �������� ������� ���������
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetSolution_Click(object sender, EventArgs e)
        {
            solutionEq = new Solution_Equation(Kx, Ky, Q, h, q, t_inf, triangulation.triangulation, triangulation.nodes, area.points);
            solutionEq.FindGlobalMatrix();
            solutionEq.FindColumnTemperature();

            TempNodeBut.Enabled = true;
            EnableNodes.Enabled = true;
            NumNodes.Enabled = true;
            Change.Enabled = false;
        }
        /// <summary>
        ///��������� ����� �� ������. ����������� ������� ������� ��� ���� �����, ���� ��� ��������� ���������� ������ � �����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XOY_MouseClick(object sender, MouseEventArgs e)
        {
            if (count != 0)
            {
                PointF pointOnPanel = new();
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
        /// ������������ ���� �����, ������ ������� ���� ������� �� ������
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
                    if ((int.Parse(nodesFirstPart) < 1 || int.Parse(nodesFirstPart) > triangulation.nodes.Count) || (int.Parse(nodesLastPart) < 1 || int.Parse(nodesLastPart) > triangulation.nodes.Count))
                    {
                        MessageBox.Show("����� ����� �� ����������");
                    }
                    else
                    {
                        grapMke.Clear(XOY);
                        for (int i = 0; i < triangulation.triangulation.Count; i++)
                        {
                            grapMke.DisplayTriangle(triangulation.triangulation[i]);
                        }

                        for (int i = int.Parse(nodesFirstPart); i <= int.Parse(nodesLastPart); i++)
                        {
                            grapMke.DrawString(i.ToString(), triangulation.nodes[i - 1].Node);
                            grapMke.DrawPoint(Color.FromArgb(27, 42, 207), triangulation.nodes[i - 1].Node);
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("������ ����� ������ �������� ������ �������");
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

                        if (numNode < 1 || numNode > triangulation.nodes.Count)
                        {
                            MessageBox.Show("������ ���� �� ����������");
                        }
                        else
                        {
                            grapMke.Clear(XOY);
                            for (int i = 0; i < triangulation.triangulation.Count; i++)
                            {
                                grapMke.DisplayTriangle(triangulation.triangulation[i]);
                            }
                            grapMke.DrawString(NumNodes.Text, triangulation.nodes[int.Parse(nodesFirstPart) - 1].Node);
                            grapMke.DrawPoint(Color.FromArgb(27, 42, 207), triangulation.nodes[int.Parse(nodesFirstPart) - 1].Node);
                        }
                    }
                    else
                    {
                        MessageBox.Show("������� ������ �����");
                    }
                }
                catch
                {
                    MessageBox.Show("����� ���� ������ �������� ����� ������");
                    NumNodes.Text = "0";
                }
            }
        }
        /// <summary>
        /// ��������� ���������� ���������� � ����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, EventArgs e)
        {
            string path = @"G:\�����\������\MKEv2.0\Temp1.txt";

            using (StreamWriter stream = new StreamWriter(path, false))
            {
                stream.Write("����� ����      ����������� � ����");
                stream.Write("\n");
                for (int i = 0; i < triangulation.nodes.Count; i++)
                {
                    for (int j = 0; j < 1; j++)
                    {
                        stream.Write("_____________|____________________|");
                        stream.Write("\n");
                        stream.Write((i + 1).ToString() + "      ");
                        stream.Write("             " + Math.Round((solutionEq.result[i] - 273), 2));
                    }
                    stream.Write("\n");
                }
                stream.Write("_____________|____________________|");
            }
        }
        /// <summary>
        /// ���������� ����������� � ��������� ����
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
                        if (numNodes < 1 || numNodes > triangulation.nodes.Count)
                        {
                            MessageBox.Show("������ ���� �� ����������");
                            NumNodes.Text = "1";
                        }
                        else
                        {
                            TempNode.Text = Math.Round((solutionEq.result[int.Parse(NumNodes.Text) - 1] - 273), 2).ToString();
                        }
                    }
                    else
                    {
                        MessageBox.Show("������� ����� ����");
                    }
                }
                catch
                {
                    MessageBox.Show("����� ���� ������ �������� ����� ������");
                    NumNodes.Text = "1";
                }
            }
            else
            {
                MessageBox.Show("������� ����� ������ ����");
            }
        }
    }
}