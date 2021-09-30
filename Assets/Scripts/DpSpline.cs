using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



    /// <summary>
    /// Дискретный периодический сплайн с векторными коэфициентами.
    /// Теоретические материалы: 
    /// http://www.math.spbu.ru/ru/mmeh/AspDok/pub/2010/Chashnikov.pdf
    /// http://dha.spb.ru/PDF/discreteSplines.pdf
    /// </summary>
    public class DpSpline : MonoBehaviour
    {

        // Для кеширования q-сплайна.
        private static int previousPoleCount = -1;
        private static int previousPointsBeetwinPoleCount = -1;
        private static double[] qSpline;

        /// <summary>
        /// Вычисляет узловые точки дискретного N-периодического сплайна с векторными коэфициентами.
        /// </summary>
        /// <param name="aPoints">Полюса сплайна (исходные точки). Должно быть не менее 2-х полюсов.</param>
        /// <param name="r">Порядок сплайна.</param>
        /// <param name="n">Число узлов между полюсами сплайна.</param>
        /// <param name="aIsIncludeOriginalPoints">True - сплайн будет проходить через полюса, false - сплайн не будет проходить через полюса.</param>
        /// <returns></returns>
        public static Vector3[] Calculate(Vector3[] aPoints, int r, int n = 5, bool aIsIncludeOriginalPoints = true) {
            if (aPoints == null) {
                throw new ArgumentNullException("aPoints");
            }

            if (aPoints.Length <= 2) {
                throw new ArgumentException("Число полюсов должно быть > 2.");
            }

            if (r <= 0) {
                throw new ArgumentException("Порядок сплайна должен быть > 0.");
            }

            if (n < 1) {
                throw new ArgumentException("Число узлов между полюсами сплайна должно быть >= 1.");
            }

            var m = aPoints.Length;
            var N = n * m;

            Vector3[] vectors;
            if (aIsIncludeOriginalPoints) {
                vectors = RecalculateVectors(aPoints, r, n, m);
            } else {
                vectors = new Vector3[m];
                aPoints.CopyTo(vectors, 0);
            }

            if (n != previousPointsBeetwinPoleCount || m != previousPoleCount) {
                previousPointsBeetwinPoleCount = n;
                previousPoleCount = m;
                qSpline = CalculateQSpline(n, m);
            }

            var resultPoints = CalculateSSpline(vectors, qSpline, r, n, m);

            return resultPoints;
        }

        /// <summary>
        /// Вычисляет вектора дискретного периодического сплайна с векторными коэфициентами, согласно
        /// формулам http://www.math.spbu.ru/ru/mmeh/AspDok/pub/2010/Chashnikov.pdf (страница 7).
        /// </summary>
        /// <param name="vectors"></param>
        /// <param name="qSpline"></param>
        /// <param name="aQSpline"></param>
        /// <param name="r"></param>
        /// <param name="n"></param>
        /// <param name="m"></param>
        /// <param name="aVectors"></param>
        /// <returns></returns>
        private static Vector3[] CalculateSSpline(Vector3[] aVectors, double[] aQSpline, int r, int n, int m) {            
            var N = n * m;
            var sSpline = new Vector3[r + 1][];
            for (var i = 1; i <= r; ++i) {
                sSpline[i] = new Vector3[N];
            }

            for (var j = 0; j < N; ++j) {
                sSpline[1][j] = new Vector3(0, 0, 0);
                for (var p = 0; p < m; ++p) {
                    sSpline[1][j] += aVectors[p] * (float)aQSpline[GetPositiveIndex(j - p * n, N)];
                }
            }

            for (var v = 2; v <= r; ++v) {
                for (var j = 0; j < N; ++j) {
                    sSpline[v][j] = new Vector3(0, 0, 0);
                    for (var k = 0; k < N; ++k) {
                        sSpline[v][j] += (float)aQSpline[k] * sSpline[v - 1][GetPositiveIndex(j - k, N)];
                    }
                    sSpline[v][j] /= n;
                }
            }

            return sSpline[r];
        }        

        /// <summary>
        /// Вычисляет коэфициенты дискретного периодического Q-сплайна 1-ого порядка, согдасно 
        /// формулам http://www.math.spbu.ru/ru/mmeh/AspDok/pub/2010/Chashnikov.pdf (страница 6).
        /// </summary>
        /// <param name="n">Число узлов между полюсами.</param>
        /// <param name="m">Число полюсов.</param>
        /// <returns>Коэфициенты дискретного периодического Q-сплайна 1-ого порядка.</returns>
        private static double[] CalculateQSpline(int n, int m) {
            var N = n * m;
            var qSpline = new double[N];

            for (var j = 0; j < N; ++j) {
                if (j >= 0 && j <= n - 1) {
                    qSpline[j] = (1.0 * n - j) / n;
                }
                if (j >= n && j <= N - n) {
                    qSpline[j] = 0;
                }
                if (j >= N - n + 1 && j <= N - 1) {
                    qSpline[j] = (1.0 * j - N + n) / n;
                }
            }

            return qSpline;
        }

        /// <summary>
        /// Пересчитывает коэфициенты сплайна для того, чтобы результирующий сплайн проходил через полюса.
        /// http://dha.spb.ru/PDF/discreteSplines.pdf (страница 6 и 7). 
        /// </summary>
        /// <param name="aPoints">Исходные точки.</param>
        /// <param name="r">Порядок сплайна.</param>
        /// <param name="n">Количество узлов между полюсами сплайна.</param>
        /// <param name="m">Количество полюсов.</param>
        /// <returns></returns>
        private static Vector3[] RecalculateVectors(Vector3[] aPoints, int r, int n, int m) {
            var N = n * m;

            // Вычисляем знаменатель.
            var tr = new double[m];
            tr[0] = 1;            
            for (var k = 1; k < m; ++k) {
                for (var q = 0; q < n; ++q) {
                    tr[k] += Math.Pow(2 * n * Math.Sin((Math.PI * (q * m + k)) / N), -2 * r);
                }
                tr[k] *= Math.Pow(2 * Math.Sin((Math.PI * k) / m), 2 * r);
            }

            // Вычисляем числитель.
            var zre = new Vector3[m];
            var zim = new Vector3[m];
            for (var j = 0; j < m; ++j) {
                zre[j] = new Vector3(0, 0);
                zim[j] = new Vector3(0, 0);
                for (var k = 0; k < m; ++k) {
                    zre[j] += aPoints[k] * (float)Math.Cos((-2 * Math.PI * j * k) / m);
                    zim[j] += aPoints[k] * (float)Math.Sin((-2 * Math.PI * j * k) / m);
                }
            }

            // Считаем результат.
            var result = new Vector3[m];
            for (var p = 0; p < m; ++p) {
                result[p] = new Vector3(0, 0, 0);
                for (var k = 0; k < m; ++k) {
                    var d = (zre[k] * (float)Math.Cos((2 * Math.PI * k * p) / m)) - (zim[k] * (float)Math.Sin((2 * Math.PI * k * p) / m));
                    d *= 1.0f / (float)tr[k];
                    result[p] += d;
                }
                result[p] /= m;
            }

            return result;
        }

        /// <summary>
        /// Обеспечивает периодичность для заданного множества.
        /// </summary>
        /// <param name="j">Индекс элемента.</param>
        /// <param name="N">Количество элементов.</param>
        /// <returns>Периодический индекс элемента.</returns>
        private static int GetPositiveIndex(int j, int N) {
            if (j >= 0) {
                return j % N;
            }

            return N - 1 + ((j + 1) % N);
        }
    }

