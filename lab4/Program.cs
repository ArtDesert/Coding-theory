using System;
using System.Collections.Generic;
using System.Linq;
using lab1;

namespace lab4
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var word = new Matrix(new[] { 1, 1, 1, 0, 1, 0, 1, 1, 1, 1, 1, 0 });
			Task1Goley(word);

			//(1,3) -> len = 4
			//(1,4) -> len = 5

            //var word = new Matrix(new[] { 1, 1, 0, 0, 1 }); 
			//int r = 1, m = 4;
			//Task2Reed_Muller(word, r, m);

        }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="word">Входное слово</param>
		public static void Task1Goley(Matrix word)
		{
			int[,] B_arr =
			{
				{ 1, 1, 0, 1, 1, 1, 0, 0, 0, 1, 0, 1 },
				{ 1, 0, 1, 1, 1, 0, 0, 0, 1, 0, 1, 1 },
				{ 0, 1, 1, 1, 0, 0, 0, 1, 0, 1, 1, 1 },
				{ 1, 1, 1, 0, 0, 0, 1, 0, 1, 1, 0, 1 },
				{ 1, 1, 0, 0, 0, 1, 0, 1, 1, 0, 1, 1 },
				{ 1, 0, 0, 0, 1, 0, 1, 1, 0, 1, 1, 1 },
				{ 0, 0, 0, 1, 0, 1, 1, 0, 1, 1, 1, 1 },
				{ 0, 0, 1, 0, 1, 1, 0, 1, 1, 1, 0, 1 },
				{ 0, 1, 0, 1, 1, 0, 1, 1, 1, 0, 0, 1 },
				{ 1, 0, 1, 1, 0, 1, 1, 1, 0, 0, 0, 1 },
				{ 0, 1, 1, 0, 1, 1, 1, 0, 0, 0, 1, 1 },
				{ 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
			};
			var B = new Matrix(B_arr); //12х12

			var I_12 = new Matrix(12);
			var G = I_12.Concat(B); //Порождающая матрица кода Голея 12х24
			var H = I_12.StackDown(B); //Проверочная матрица кода Голея 24х12

			var encodedWord = word * G;
			var w = encodedWord.SumWithError1(0, 1, 2);
			var s = w * H;
            Console.WriteLine($"w = {w}");
			Matrix u;
			if(s.Weight <= 3)
			{
				u = s.Concat(new Matrix(new int[12]));
				Console.WriteLine($"u = {u}");
			}
			for (int i = 0; i < B.Row; ++i)
			{
				var b_i = B.SelectRow(i);
				var e_i = new Matrix(new int[12]).SumWithError1(i);
				if ((s + b_i).Weight <= 2)
				{
					u = (s + b_i).Concat(e_i);
					Console.WriteLine($"u = {u}");
				}
			}

			var sb = s * B;
			if (sb.Weight <= 3)
			{
				u = (new Matrix(new int[12])).Concat(sb);
				Console.WriteLine($"u = {u}");
			}
			for (int i = 0; i < B.Row; ++i)
			{
				var b_i = B.SelectRow(i);
				var e_i = new Matrix(new int[12]).SumWithError1(i);
				if ((sb + b_i).Weight <= 2)
				{
					u = e_i.Concat(sb + b_i);
					Console.WriteLine($"u = {u}");
				}
			}
		}

		public static void Task2Reed_Muller(Matrix word, int r, int m)
		{
			var G = Create_G_RM(r, m);
			//var H = Create_H_RM(r, m);
			//var encodedWord = word * G;
			var w = word * G;
			//Console.WriteLine($"encodedWord = {encodedWord}");
			//var w = encodedWord.SumWithError1(0);
            Console.WriteLine($"w = {w}");
            var _w = new Matrix(w);
			for (int j = 0; j < w.Col; ++j)
			{
				if (_w[0, j] == 0)
				{
					_w[0, j] = -1;
				}
			}
			var H_1_m = Create_H_RM(1, m);
			var w1 = Matrix.Mult(_w, H_1_m);
			var wList = new List<Matrix>() { _w, w1 };
			for (int i = 2; i < m + 1; ++i)
			{
				var H_i_m = Create_H_RM(i, m);
				wList.Add(Matrix.Mult(wList[i - 1], H_i_m)); 
			}
			int maxIndex = 0;
			var w_m = wList[m];
			for (int j = 0; j < w_m.Col; ++j)
			{
				int max = 0;
				if (Math.Abs(w_m[0, j]) > max)
				{
					maxIndex = j;
					max = Math.Abs(w_m[0, j]);
				}
			}
			int k = GetSizeRM_Matrix(r, m);
			var binaryRepresentation = GetBinaryRepresentation(maxIndex, m);
			if (w_m[0, maxIndex] > 0)
			{
				var temp = new Matrix(new int[k - binaryRepresentation.Col]);
				for (int j = 0; j < temp.Col; ++j)
				{
					temp[0, j] = 1;
				}
				var u = temp.Concat(binaryRepresentation);
				Console.WriteLine($"u = {u}");
			}
			else
			{
				var u = new Matrix(new int[k]);
				for (int j = 0; j < binaryRepresentation.Col; ++j)
				{
					u[0, k - binaryRepresentation.Col + j] = binaryRepresentation[0, j];
				}
				Console.WriteLine($"u = {u}");
			}

		}

		public static int GetSizeRM_Matrix(int r, int m)
		{
			return Enumerable.Range(0, r + 1).Select(x => GetCombinationsNumber(m, x)).Sum();
		}

		public static int GetCombinationsNumber(int n, int k)
		{
			return GetFactorial(n) / (GetFactorial(k) * GetFactorial(n - k));
		}

		public static int GetFactorial(int n)
		{
			if (n == 0)
			{
				return 1;
			}
			int result = 1;
			for (int i = 1; i < n + 1; ++i)
			{
				result *= i;
			}
			return result;
		}

		public static Matrix Create_G_RM(int r, int m)
		{
			int n = (int)(Math.Pow(2, m));
			if (r == 0)
			{
				var arr = new int[(int)(Math.Pow(2, m))];
				return new Matrix(arr.Select(x => 1).ToArray());
			}
			else if (r == m)
			{
				int k = GetSizeRM_Matrix(m - 1, m);
				var arr = new int[k + 1, n];
				var curMatrix = Create_G_RM(m - 1, m);
				for (int i = 0; i < k; ++i)
				{
					for (int j = 0; j < n; ++j)
					{
						arr[i, j] = curMatrix[i, j];
					}
				}
				arr[k, n - 1] = 1;
				return new Matrix(arr);
			}
			else
			{
				var topMatrix = Create_G_RM(r, m - 1);
				int kTop = GetSizeRM_Matrix(r, m - 1);
				var botMatrix = Create_G_RM(r - 1, m - 1);
				int kBot = GetSizeRM_Matrix(r - 1, m - 1);
				int nCur = (int)(Math.Pow(2, m - 1));
				var arr = new int[kTop + kBot, 2 * nCur];
				for (int i = 0; i < kTop; ++i)
				{
					for (int j = 0; j < nCur; ++j)
					{
						arr[i, j] = arr[i, j + nCur] = topMatrix[i, j];
					}
				}
				for (int i = kTop; i < kTop + kBot; ++i)
				{
					for (int j = 0; j < nCur; ++j)
					{
						arr[i, j + nCur] = botMatrix[i - kTop, j];
					}
				}
				return new Matrix(arr);
			}
		}

		public static Matrix Create_H_RM(int r, int m)
		{
			var H = new Matrix(new[,] { { 1, 1 }, { 1, -1 } });
			var leftI = new Matrix((int)(Math.Pow(2, m - r)));
			var rightI = new Matrix((int)(Math.Pow(2, r - 1)));
			return leftI % H % rightI;
		}

		public static Matrix GetBinaryRepresentation(int number, int m)
		{
			var result = new Matrix(new int[m]);
			var binaryRepresentation = Convert.ToString(number, 2);
			for (int i = 0; i < binaryRepresentation.Length; ++i)
			{
				result[0, i] = int.Parse(binaryRepresentation[i].ToString());
			}
			return result;
		}
	}
}
