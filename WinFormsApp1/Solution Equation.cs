﻿using MathNet.Numerics.LinearAlgebra.Double;

namespace MKE
{
    /// <summary>
///В данном классе выполняется поиск решения уравнения
/// </summary>
    internal class Solution_Equation
    {
        /// <summary>
        /// Коэффициент теплопроводности в направлении х
        /// </summary>
        private double Kxx { get; set; }
        /// <summary>
        /// Коэффициент теплопроводности в направлении у
        /// </summary>
        private double Kyy { get; set; }
        /// <summary>
        /// Источник тепла внутри тела
        /// </summary>
        private double Q { get; set; }
        /// <summary>
        /// Коэффициент теплообмена на каждой стороне области
        /// </summary>
        private double[] h { get; set; }
        /// <summary>
        /// Поток тепла для каждой границе области
        /// </summary>
        private double[] q { get; set; }
        /// <summary>
        /// Окружающая температура с каждой из сторон области
        /// </summary>
        private double[] t_inf { get; set; }      
        /// <summary>
        /// Глобальный вектор-столбец нагрузки
        /// </summary>
        private double[] F { get; set; }
        /// <summary>
        /// Глобальная матрица жесткости
        /// </summary>
        private double[,] K { get; set; }
        /// <summary>
        /// Искомый столбец температур
        /// </summary>
        public double[] Result { get; private set; }
        /// <summary>
        /// Изначальные точки области
        /// </summary>
        private PointD[] Points { get; set; }
        /// <summary>
        /// Список структур треугольников триангуляции Делоне
        /// </summary>
        private List<Triangle> Triangles { get; set; }
        /// <summary>
        /// Узлы сетки
        /// </summary>
        /// 
        private List<Nodes> Nodes { get; set; }

        public Solution_Equation(double Kxx, double Kyy, double Q, double[] h, double[] q, double[] t_inf, List<Triangle> triangulation, List<Nodes> nodes, PointD[] points) 
        { 
            this.Kxx = Kxx;
            this.Kyy = Kyy;
            this.Q = Q;
            this.h = h;
            this.q = q;
            this.t_inf = t_inf;
            Nodes = nodes;
            Points = points;
            Triangles = triangulation;
            
            F = new double[Nodes.Count];
            K = new double[Nodes.Count, Nodes.Count];
        }
        
        /// <summary>
        /// Вычисление глобальных матриц К и F
        /// </summary>
        public void FindGlobalMatrix()
        {
            for (int i = 0; i < Triangles.Count; i++)
            {
                double squareTriangle = Triangulation.GetSquareTriangle(Triangles[i]);

                double[,] firstIntegralK_e;
                double[,] secondIntegralK_e;
                double[,] matrixK_e;

                double[] firstIntegralF_e;
                double[] secondIntegralF_e;
                double[] matrixF_e;

                firstIntegralK_e = FindFirstIntegralK_e(Triangles[i], squareTriangle);
                secondIntegralK_e = FindSecondIntegralK_e(Triangles[i]);
                matrixK_e = CompletionMatrixK_e(firstIntegralK_e, secondIntegralK_e);

                firstIntegralF_e = FindFirstIntegralF_e(squareTriangle);
                secondIntegralF_e = FindSecondIntegralF_e(Triangles[i]);
                matrixF_e = CompletionMatrixF_e(firstIntegralF_e, secondIntegralF_e);

                CompletionMatrixF(Triangles[i], matrixF_e);
                CompletionMatrixK(Triangles[i], matrixK_e);
            }
        }
        /// <summary>
        /// Нахождение искомого столбца температур
        /// </summary>
        public void FindColumnTemperature()
        {
            SparseMatrix matrixK = new(Nodes.Count, Nodes.Count);
            double[] rightSide = new double[Nodes.Count];

            for (int i = 0; i < Nodes.Count; i++)
            {
                rightSide[i] = F[i];

                for (int j = 0; j < Nodes.Count; j++)
                {
                    matrixK[i, j] = K[i, j];
                }
            }

            var columnTemperature = matrixK.Solve(DenseVector.Build.DenseOfArray(rightSide)).ToArray();         
            Result = new double[columnTemperature.Length];

            for (int i = 0; i < columnTemperature.Length; i++)
            {
                Result[i] = columnTemperature[i];
            }
        }
        /// <summary>
        /// Нахождение первого интеграла К для отдельного элемента
        /// </summary>
        /// <param name="triangle"></param>
        /// <param name="squareTriangle"></param>
        /// <returns></returns>
        private double[,] FindFirstIntegralK_e(Triangle triangle, double squareTriangle)
        {
            double[] b = new double[3];
            double[] c = new double[3];
            double[,] firstI = new double[3, 3];

            /* Вычисление b[i] и  c[i], для узла i */
            b[0] = (triangle.vertex2.Y - triangle.vertex3.Y);
            c[0] = (triangle.vertex3.X - triangle.vertex2.X);
           
            /* Вычисление b[j] и  c[j], для узла j */
            b[1] = (triangle.vertex3.Y - triangle.vertex1.Y);
            c[1] = (triangle.vertex1.X - triangle.vertex3.X);

            /* Вычисление b[k] и  c[k], для узла k */
            b[2] = (triangle.vertex1.Y - triangle.vertex2.Y);
            c[2] = (triangle.vertex2.X - triangle.vertex1.X);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    firstI[i, j] = (Kxx / (4 * squareTriangle)) * (b[i] * b[j]) + (Kyy / (4 * squareTriangle)) * (c[i] * c[j]);
                }
            }
            return firstI;
        }
        /// <summary>
        /// Нахождение второго интеграла К для отдельного элемента
        /// </summary>
        /// <param name="triangle"></param>
        /// <returns></returns>
        private double[,] FindSecondIntegralK_e(Triangle triangle)
        {
            int[] numberBorder = new int[3];
            double[] distanceSide = new double[3];
            double[,] secondI = new double[3, 3];

            numberBorder[0] = SideTriangleOnBorder(triangle.vertex1, triangle.vertex2);
            numberBorder[1] = SideTriangleOnBorder(triangle.vertex2, triangle.vertex3);
            numberBorder[2] = SideTriangleOnBorder(triangle.vertex3, triangle.vertex1);

            distanceSide[0] = Triangulation.GetDistancePoints(triangle.vertex1, triangle.vertex2);
            distanceSide[1] = Triangulation.GetDistancePoints(triangle.vertex2, triangle.vertex3);
            distanceSide[2] = Triangulation.GetDistancePoints(triangle.vertex3, triangle.vertex1);


            for (int i = 0; i < numberBorder.Length; i++)
            {
                if (numberBorder[i] != -1)
                {
                    if (i == 0)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            for (int k = 0; k < 3; k++)
                            {
                                if (j == 2 || k == 2)
                                {
                                    secondI[j, k] += 0;
                                }
                                else if (j == k)
                                {
                                    secondI[j, k] += (h[numberBorder[i]] * distanceSide[0]) / 3;
                                }
                                else
                                {
                                    secondI[j, k] += (h[numberBorder[i]] * distanceSide[0]) / 6;
                                }
                            }
                        }
                    }
                    else if (i == 1)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            for (int k = 0; k < 3; k++)
                            {
                                if (j == 0 || k == 0)
                                {
                                    secondI[j, k] += 0;
                                }
                                else if (j == k)
                                {
                                    secondI[j, k] += (h[numberBorder[i]] * distanceSide[1]) / 3;
                                }
                                else
                                {
                                    secondI[j, k] += (h[numberBorder[i]] * distanceSide[1]) / 6;
                                }
                            }
                        }
                    }
                    else if (i == 2)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            for (int k = 0; k < 3; k++)
                            {
                                if (j == 1 || k == 1)
                                {
                                    secondI[j, k] += 0;
                                }
                                else if (j == k)
                                {
                                    secondI[j, k] += (h[numberBorder[i]] * distanceSide[2]) / 3;
                                }
                                else
                                {
                                    secondI[j, k] += (h[numberBorder[i]] * distanceSide[2]) / 6;
                                }
                            }
                        }
                    }
                }
            }

            return secondI;
        }
        /// <summary>
        /// Сборка локальной матрицы К 
        /// </summary>
        /// <param name="firstI"></param>
        /// <param name="secondI"></param>
        /// <returns></returns>
        private static double[,] CompletionMatrixK_e(double[,]  firstI, double[,] secondI)
        {
            double[,] matrixK_e = new double[3, 3];

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    matrixK_e[i, j] = firstI[i, j] + secondI[i, j];
                }
            }

            return matrixK_e;
        }
        /// <summary>
        /// Нахождение первого интеграла F для отдельного элемента
        /// </summary>
        /// <param name="squareTriangle"></param>
        /// <returns></returns>
        private double[] FindFirstIntegralF_e(double squareTriangle)
        {
            double[] firstIf_e = new double[3];

            for (int i = 0; i < 3; i++)
            {
                firstIf_e[i] = -(Q * squareTriangle) / 3;
            }

            return firstIf_e;
        }
        /// <summary>
        /// Нахождение второго интеграла F для отдельного элемента
        /// </summary>
        /// <param name="triangle"></param>
        /// <returns></returns>
        private double[] FindSecondIntegralF_e(Triangle triangle)
        {
            int[] numberBorder = new int[3];
            double[] distanceSide = new double[3];
            double[] secondIf = new double[3];

            numberBorder[0] = SideTriangleOnBorder(triangle.vertex1, triangle.vertex2);
            numberBorder[1] = SideTriangleOnBorder(triangle.vertex2, triangle.vertex3);
            numberBorder[2] = SideTriangleOnBorder(triangle.vertex3, triangle.vertex1);

            distanceSide[0] = Triangulation.GetDistancePoints(triangle.vertex1, triangle.vertex2);
            distanceSide[1] = Triangulation.GetDistancePoints(triangle.vertex2, triangle.vertex3);
            distanceSide[2] = Triangulation.GetDistancePoints(triangle.vertex3, triangle.vertex1);

            for (int i = 0; i < numberBorder.Length; i++)
            {
                if (numberBorder[i] != -1)
                {
                    if (i == 0)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (j == 2)
                            {
                                secondIf[j] += 0;
                            }
                            else
                            {
                                secondIf[j] += distanceSide[0] * (q[numberBorder[i]] - h[numberBorder[i]] * t_inf[numberBorder[i]]) / 2;
                            }
                        }
                    }
                    else if (i == 1)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (j == 0)
                            {
                                secondIf[j] += 0;
                            }
                            else
                            {
                                secondIf[j] += distanceSide[1] * (q[numberBorder[i]] - h[numberBorder[i]] * t_inf[numberBorder[i]]) / 2;
                            }
                        }
                    }
                    else if (i == 2)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (j == 1)
                            {
                                secondIf[j] += 0;
                            }
                            else
                            {
                                secondIf[j] += distanceSide[2] * (q[numberBorder[i]] - h[numberBorder[i]] * t_inf[numberBorder[i]]) / 2;
                            }
                        }
                    }
                }
            }

            return secondIf;
        }
        /// <summary>
        /// Сборка локального столбца F
        /// </summary>
        /// <param name="firstIf"></param>
        /// <param name="secondIf"></param>
        /// <returns></returns>
        private static double[] CompletionMatrixF_e(double[] firstIf, double[] secondIf)
        {
            double[] matrixF_e = new double[3];

            for (int i = 0; i < 3; i++)
            {
                matrixF_e[i] = firstIf[i] + secondIf[i];
            }

            return matrixF_e;
        }
        /// <summary>
        /// Добавление локального столбца F в глобальный
        /// </summary>
        /// <param name="triangle"></param>
        /// <param name="matrixF_e"></param>
        private void CompletionMatrixF(Triangle triangle, double[] matrixF_e)
        {
            for(int i = 0; i < Nodes.Count; i++)
            {
                if(i == (triangle.vertex1NumNodes - 1))
                {
                    F[i] -= matrixF_e[0];
                }

                if (i == (triangle.vertex2NumNodes - 1))
                {
                    F[i] -= matrixF_e[1];
                }

                if (i == (triangle.vertex3NumNodes - 1))
                {
                    F[i] -= matrixF_e[2];
                }
            }
        }
        /// <summary>
        /// Добавление локальной матрицы K в глобальную
        /// </summary>
        /// <param name="triangle"></param>
        /// <param name="matrixK_e"></param>
        private void CompletionMatrixK(Triangle triangle, double[,] matrixK_e)
        {
            for(int i = 0; i < Nodes.Count;i++)
            {
                for (int j = 0; j < Nodes.Count; j++)
                {
                    //1
                    if(i == (triangle.vertex1NumNodes - 1) && j == (triangle.vertex1NumNodes - 1))
                    {
                        K[i, j] += matrixK_e[0, 0];
                    }

                    if (i == (triangle.vertex1NumNodes - 1) && j == (triangle.vertex2NumNodes - 1))
                    {
                        K[i, j] += matrixK_e[0, 1];
                    }

                    if (i == (triangle.vertex1NumNodes - 1) && j == (triangle.vertex3NumNodes - 1))
                    {
                        K[i, j] += matrixK_e[0, 2];
                    }

                    //2
                    if (i == (triangle.vertex2NumNodes - 1) && j == (triangle.vertex1NumNodes - 1))
                    {
                        K[i, j] += matrixK_e[1, 0];
                    }

                    if (i == (triangle.vertex2NumNodes - 1) && j == (triangle.vertex2NumNodes - 1))
                    {
                        K[i, j] += matrixK_e[1, 1];
                    }

                    if (i == (triangle.vertex2NumNodes - 1) && j == (triangle.vertex3NumNodes - 1))
                    {
                        K[i, j] += matrixK_e[1, 2];
                    }

                    //3
                    if (i == (triangle.vertex3NumNodes - 1) && j == (triangle.vertex1NumNodes - 1))
                    {
                        K[i, j] += matrixK_e[2, 0];
                    }

                    if (i == (triangle.vertex3NumNodes - 1) && j == (triangle.vertex2NumNodes - 1))
                    {
                        K[i, j] += matrixK_e[2, 1];
                    }

                    if (i == (triangle.vertex3NumNodes - 1) && j == (triangle.vertex3NumNodes - 1))
                    {
                        K[i, j] += matrixK_e[2, 2];
                    }
                }
            }
        }
        /// <summary>
        /// Возвращает номер стороны области, на которую опирается треугольник, иначе возвращает -1
        /// </summary>
        /// <param name="firstVertex"></param>
        /// <param name="secondVertex"></param>
        /// <returns></returns>
        private int SideTriangleOnBorder(PointD firstVertex, PointD secondVertex)
        {
            for (int i = 0; i < Points.Length; i++)
            {
                if (i == Points.Length - 1)
                {
                    if (((Triangulation.GetDistancePoints(Points[i], firstVertex) + Triangulation.GetDistancePoints(firstVertex, Points[0])) == Triangulation.GetDistancePoints(Points[i], Points[0])) &&
                    ((Triangulation.GetDistancePoints(Points[i], secondVertex) + Triangulation.GetDistancePoints(secondVertex, Points[0])) == Triangulation.GetDistancePoints(Points[i], Points[0])))
                    {
                        return i;
                    }
                }
                else
                {
                    if (((Triangulation.GetDistancePoints(Points[i], firstVertex) + Triangulation.GetDistancePoints(firstVertex, Points[i + 1])) == Triangulation.GetDistancePoints(Points[i], Points[i + 1])) &&
                    ((Triangulation.GetDistancePoints(Points[i], secondVertex) + Triangulation.GetDistancePoints(secondVertex, Points[i + 1])) == Triangulation.GetDistancePoints(Points[i], Points[i + 1])))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
    }
}
