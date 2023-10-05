using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra.Double;
namespace MKE
{
    public class SolutionEquation
    {
        /// <summary>
        /// Коэффициент теплопроводности в направлении х
        /// </summary>
        private double _Kxx;
        /// <summary>
        /// Коэффициент теплопроводности в направлении у
        /// </summary>
        private double _Kyy;
        /// <summary>
        /// Источник тепла внутри тела
        /// </summary>
        private double _Q;
        /// <summary>
        /// Коэффициент теплообмена на каждой стороне области
        /// </summary>
        private double[] _h;
        /// <summary>
        /// Поток тепла для каждой границе области
        /// </summary>
        private double[] _q;
        /// <summary>
        /// Окружающая температура с каждой из сторон области
        /// </summary>
        private double[] _t_inf;
        /// <summary>
        /// Глобальный вектор-столбец нагрузки
        /// </summary>
        private double[] _F;
        /// <summary>
        /// Глобальная матрица жесткости
        /// </summary>
        private double[,] _K;
        private double[] _result;
        private int _countNodes;
        /// <summary>
        /// Изначальные точки области
        /// </summary>
        private PointD[] _points;
        /// <summary>
        /// Список структур треугольников триангуляции Делоне
        /// </summary>
        private List<Triangle> _triangles;
        /// <summary>
        /// Искомый столбец температур
        /// </summary>
        public double[] Result { get => _result; }
        public SolutionEquation(double Kxx, double Kyy, double Q, double[] h, double[] q, double[] t_inf, int countNodes, List<Triangle> triangles, PointD[] points)
        {
            _Kxx = Kxx;
            _Kyy = Kyy;
            _Q = Q;
            _h = h;
            _q = q;
            _t_inf = t_inf;
            _points = points;
            _triangles = triangles;
            _countNodes = countNodes;
            _F = new double[_countNodes];
            _K = new double[_countNodes, _countNodes];
        }
        public void FindGlobalMatrix()
        {
            for (int i = 0; i < _triangles.Count; i++)
            {
                double squareTriangle = Mathematics.GetSquareTriangle(_triangles[i]);

                double[,] firstIntegralK_e;
                double[,] secondIntegralK_e;
                double[,] matrixK_e;

                double[] firstIntegralF_e;
                double[] secondIntegralF_e;
                double[] matrixF_e;

                firstIntegralK_e = FindFirstIntegralK_e(_triangles[i], squareTriangle);
                secondIntegralK_e = FindSecondIntegralK_e(_triangles[i]);
                matrixK_e = CompletionMatrixK_e(firstIntegralK_e, secondIntegralK_e);

                firstIntegralF_e = FindFirstIntegralF_e(squareTriangle);
                secondIntegralF_e = FindSecondIntegralF_e(_triangles[i]);
                matrixF_e = CompletionMatrixF_e(firstIntegralF_e, secondIntegralF_e);

                CompletionMatrixF(_triangles[i], matrixF_e);
                CompletionMatrixK(_triangles[i], matrixK_e);
            }
        }
        /// <summary>
        /// Нахождение искомого столбца температур
        /// </summary>
        public void FindColumnTemperature()
        {
            SparseMatrix matrixK = new(_countNodes, _countNodes);
            double[] rightSide = new double[_countNodes];

            for (int i = 0; i < _countNodes; i++)
            {
                rightSide[i] = _F[i];

                for (int j = 0; j < _countNodes; j++)
                {
                    matrixK[i, j] = _K[i, j];
                }
            }

            var columnTemperature = matrixK.Solve(DenseVector.Build.DenseOfArray(rightSide)).ToArray();
            _result = new double[columnTemperature.Length];

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
            b[0] = (triangle.SecondVertex.Y - triangle.ThirdVertex.Y);
            c[0] = (triangle.ThirdVertex.X - triangle.SecondVertex.X);

            /* Вычисление b[j] и  c[j], для узла j */
            b[1] = (triangle.ThirdVertex.Y - triangle.FirstVertex.Y);
            c[1] = (triangle.FirstVertex.X - triangle.ThirdVertex.X);

            /* Вычисление b[k] и  c[k], для узла k */
            b[2] = (triangle.FirstVertex.Y - triangle.SecondVertex.Y);
            c[2] = (triangle.SecondVertex.X - triangle.FirstVertex.X);

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    firstI[i, j] = (_Kxx / (4 * squareTriangle)) * (b[i] * b[j]) + (_Kyy / (4 * squareTriangle)) * (c[i] * c[j]);
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

            numberBorder[0] = SideTriangleOnBorder(triangle.FirstVertex, triangle.SecondVertex);
            numberBorder[1] = SideTriangleOnBorder(triangle.SecondVertex, triangle.ThirdVertex);
            numberBorder[2] = SideTriangleOnBorder(triangle.ThirdVertex, triangle.FirstVertex);

            distanceSide[0] = Mathematics.GetDistancePoints(triangle.FirstVertex, triangle.SecondVertex);
            distanceSide[1] = Mathematics.GetDistancePoints(triangle.SecondVertex, triangle.ThirdVertex);
            distanceSide[2] = Mathematics.GetDistancePoints(triangle.ThirdVertex, triangle.FirstVertex);


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
                                    secondI[j, k] += (_h[numberBorder[i]] * distanceSide[0]) / 3;
                                }
                                else
                                {
                                    secondI[j, k] += (_h[numberBorder[i]] * distanceSide[0]) / 6;
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
                                    secondI[j, k] += (_h[numberBorder[i]] * distanceSide[1]) / 3;
                                }
                                else
                                {
                                    secondI[j, k] += (_h[numberBorder[i]] * distanceSide[1]) / 6;
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
                                    secondI[j, k] += (_h[numberBorder[i]] * distanceSide[2]) / 3;
                                }
                                else
                                {
                                    secondI[j, k] += (_h[numberBorder[i]] * distanceSide[2]) / 6;
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
        private static double[,] CompletionMatrixK_e(double[,] firstI, double[,] secondI)
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
        /// Нахождение первого интеграла _F для отдельного элемента
        /// </summary>
        /// <param name="squareTriangle"></param>
        /// <returns></returns>
        private double[] FindFirstIntegralF_e(double squareTriangle)
        {
            double[] firstIf_e = new double[3];

            for (int i = 0; i < 3; i++)
            {
                firstIf_e[i] = -(_Q * squareTriangle) / 3;
            }

            return firstIf_e;
        }
        /// <summary>
        /// Нахождение второго интеграла _F для отдельного элемента
        /// </summary>
        /// <param name="triangle"></param>
        /// <returns></returns>
        private double[] FindSecondIntegralF_e(Triangle triangle)
        {
            int[] numberBorder = new int[3];
            double[] distanceSide = new double[3];
            double[] secondIf = new double[3];

            numberBorder[0] = SideTriangleOnBorder(triangle.FirstVertex, triangle.SecondVertex);
            numberBorder[1] = SideTriangleOnBorder(triangle.SecondVertex, triangle.ThirdVertex);
            numberBorder[2] = SideTriangleOnBorder(triangle.ThirdVertex, triangle.FirstVertex);

            distanceSide[0] = Mathematics.GetDistancePoints(triangle.FirstVertex, triangle.SecondVertex);
            distanceSide[1] = Mathematics.GetDistancePoints(triangle.SecondVertex, triangle.ThirdVertex);
            distanceSide[2] = Mathematics.GetDistancePoints(triangle.ThirdVertex, triangle.FirstVertex);

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
                                secondIf[j] += distanceSide[0] * (_q[numberBorder[i]] - _h[numberBorder[i]] * _t_inf[numberBorder[i]]) / 2;
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
                                secondIf[j] += distanceSide[1] * (_q[numberBorder[i]] - _h[numberBorder[i]] * _t_inf[numberBorder[i]]) / 2;
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
                                secondIf[j] += distanceSide[2] * (_q[numberBorder[i]] - _h[numberBorder[i]] * _t_inf[numberBorder[i]]) / 2;
                            }
                        }
                    }
                }
            }

            return secondIf;
        }
        /// <summary>
        /// Сборка локального столбца _F
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
        /// Добавление локального столбца _F в глобальный
        /// </summary>
        /// <param name="triangle"></param>
        /// <param name="matrixF_e"></param>
        private void CompletionMatrixF(Triangle triangle, double[] matrixF_e)
        {
            for (int i = 0; i < _countNodes; i++)
            {
                if (i == (triangle.FirstNodeNum - 1))
                {
                    _F[i] -= matrixF_e[0];
                }

                if (i == (triangle.SecondNodeNum - 1))
                {
                    _F[i] -= matrixF_e[1];
                }

                if (i == (triangle.ThirdNodeNum - 1))
                {
                    _F[i] -= matrixF_e[2];
                }
            }
        }
        /// <summary>
        /// Добавление локальной матрицы _K в глобальную
        /// </summary>
        /// <param name="triangle"></param>
        /// <param name="matrixK_e"></param>
        private void CompletionMatrixK(Triangle triangle, double[,] matrixK_e)
        {
            for (int i = 0; i < _countNodes; i++)
            {
                for (int j = 0; j < _countNodes; j++)
                {
                    //1
                    if (i == (triangle.FirstNodeNum - 1) && j == (triangle.FirstNodeNum - 1))
                    {
                        _K[i, j] += matrixK_e[0, 0];
                    }

                    if (i == (triangle.FirstNodeNum - 1) && j == (triangle.SecondNodeNum - 1))
                    {
                        _K[i, j] += matrixK_e[0, 1];
                    }

                    if (i == (triangle.FirstNodeNum - 1) && j == (triangle.ThirdNodeNum - 1))
                    {
                        _K[i, j] += matrixK_e[0, 2];
                    }

                    //2
                    if (i == (triangle.SecondNodeNum - 1) && j == (triangle.FirstNodeNum - 1))
                    {
                        _K[i, j] += matrixK_e[1, 0];
                    }

                    if (i == (triangle.SecondNodeNum - 1) && j == (triangle.SecondNodeNum - 1))
                    {
                        _K[i, j] += matrixK_e[1, 1];
                    }

                    if (i == (triangle.SecondNodeNum - 1) && j == (triangle.ThirdNodeNum - 1))
                    {
                        _K[i, j] += matrixK_e[1, 2];
                    }

                    //3
                    if (i == (triangle.ThirdNodeNum - 1) && j == (triangle.FirstNodeNum - 1))
                    {
                        _K[i, j] += matrixK_e[2, 0];
                    }

                    if (i == (triangle.ThirdNodeNum - 1) && j == (triangle.SecondNodeNum - 1))
                    {
                        _K[i, j] += matrixK_e[2, 1];
                    }

                    if (i == (triangle.ThirdNodeNum - 1) && j == (triangle.ThirdNodeNum - 1))
                    {
                        _K[i, j] += matrixK_e[2, 2];
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
            for (int i = 0; i < _points.Length; i++)
            {
                if (i == _points.Length - 1)
                {
                    if (((Mathematics.GetDistancePoints(_points[i], firstVertex) + Mathematics.GetDistancePoints(firstVertex, _points[0])) == Mathematics.GetDistancePoints(_points[i], _points[0])) &&
                    ((Mathematics.GetDistancePoints(_points[i], secondVertex) + Mathematics.GetDistancePoints(secondVertex, _points[0])) == Mathematics.GetDistancePoints(_points[i], _points[0])))
                    {
                        return i;
                    }
                }
                else
                {
                    if (((Mathematics.GetDistancePoints(_points[i], firstVertex) + Mathematics.GetDistancePoints(firstVertex, _points[i + 1])) == Mathematics.GetDistancePoints(_points[i], _points[i + 1])) &&
                    ((Mathematics.GetDistancePoints(_points[i], secondVertex) + Mathematics.GetDistancePoints(secondVertex, _points[i + 1])) == Mathematics.GetDistancePoints(_points[i], _points[i + 1])))
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
    }
}
