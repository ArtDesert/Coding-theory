using System;
using System.Collections.Generic;
using System.Linq;
using lab1;

namespace lab2
{
	public class Program
	{
		public static void Main(string[] args)
		{
			//1 часть
			int n = 7, k = 4, d = 3;
			int[,] X_arr = {
				{ 0, 1, 1 }, 
				{ 1, 0, 1 }, 
				{ 1, 1, 1 }, 
				{ 1, 1, 0 } };
			var X = new Matrix(X_arr);
			var I_4 = new Matrix(4);

			var G_arr = new int[k, n]; //[I_4|X]

			for (int i = 0; i < k; ++i)
			{
				for (int j = 0; j < I_4.Col; ++j)
				{
					G_arr[i, j] = I_4[i, j];	
				}
				for (int j = 0; j < X.Col; ++j)
				{
					G_arr[i, j + I_4.Col] = X[i, j];
				}
			}
			var G = new Matrix(G_arr); //Порождающая матрица линейного кода (7, 4, 3)
            Console.WriteLine("G = ");
            G.Print();

			// H = [X]
			//	   [I_n-k]
			var H_arr = new int[n, d]; //Проверочная матрица
			var I_3 = new Matrix(n - k);

			for (int i = 0; i < k; ++i)
			{
				for (int j = 0; j < d; ++j)
				{
					H_arr[i, j] = X[i, j];
				}
			}
			for (int i = 0; i < d; ++i)
			{
				for (int j = 0; j < d; ++j)
				{
					H_arr[i + k, j] = I_3[i, j];
				}
			}
			var H = new Matrix(H_arr);
            Console.WriteLine("\nH = ");
			H.Print();
            Console.WriteLine();

			//Cловарь синдромов: ключ - синдром, значение - ошибка, которую этот синдром может обнаружить
			var syndroms = new Dictionary<Matrix, Matrix>(); 
			for (int i = 0; i < n; ++i)
			{
				var curError = Matrix.Error(n, i);
				syndroms[curError * H] = curError;
			}

			var v1 = new Matrix(new[] { 1, 0, 0, 0, 1, 1, 1 }); //Кодовое слово длины n (7)
			var w1 = v1 + Matrix.Error(7, 1); //[1, 1, 0, 0, 1, 1, 1] - слово с одной ошибкой
            Console.Write("w1 = ");
            w1.Print();
			var syndrom1 = w1 * H;
            Console.Write("syndrom1 = ");
            syndrom1.Print();
			var error1 = syndroms.Where(x => x.Key.Equals(syndrom1)).FirstOrDefault().Value;
            Console.Write("error1 = ");
            error1.Print();
			var correctedWord1 = w1.SumWithError1(6);
            Console.Write("correctedWord1 = ");
			correctedWord1.Print();
            Console.Write("Проверка: ");
            (correctedWord1 * H).Print();
            
            var v2 = new Matrix(new[] { 1, 1, 1, 1, 1, 0, 0 }); //Кодовое слово длины n (7)
			var w2 = v2.SumWithError1(5, 6); //[1, 1, 1, 1, 1, '1', '1'] - слово с двумя ошибками
			Console.Write("\nw2 = ");
			w2.Print();
			var syndrom2 = w2 * H;
			Console.Write("syndrom2 = "); //[0, 0, 0] - такого синдрома в таблице нет!
			syndrom2.Print();
			//try
			//{
			//	var error2 = syndroms.Where(x => x.Key.Equals(syndrom2)).FirstOrDefault().Value;
			//	Console.Write("error2 = ");
			//	error2.Print();
			//}
			//catch (Exception)
			//{
   //             Console.WriteLine("Синдрома [0, 0, 0] в таблице нет!\n");
   //         }
			//---------------------------------------------------------------------------------------
			//2 часть
			int n2 = 11, k2 = 4, d2 = 5;
			//var X2 = Matrix.GenerateX(n2 - k2, k2); //Сгенерированная матрица
			int[,] X_arr2 =
			{
				{ 1, 1, 0, 1, 0, 1, 0 },
				{ 0, 1, 1, 1, 0, 1, 1,},
				{ 0, 1, 1, 1, 1, 1, 0 },
				{ 0, 0, 1, 1, 1, 1, 1 },
			};
			var X2 = new Matrix(X_arr2);
			var I_k = new Matrix(k2);
			var G_arr2 = new int[k2, n2];
			for (int i = 0; i < k2; ++i)
			{
				for (int j = 0; j < I_k.Col; ++j)
				{
					G_arr2[i, j] = I_k[i, j];
				}
				for (int j = 0; j < X2.Col; ++j)
				{
					G_arr2[i, j + I_k.Col] = X2[i, j];
				}
			}
			var G2 = new Matrix(G_arr2);
            Console.WriteLine("G2 = ");
            G2.Print();

			var H_arr2 = new int[n2, n2 - k2]; //Проверочная матрица
			var I_kH = new Matrix(n2 - k2);

			for (int i = 0; i < k2; ++i)
			{
				for (int j = 0; j < n2 - k2; ++j)
				{
					H_arr2[i, j] = X2[i, j];
				}
			}
			for (int i = 0; i < n2 - k2; ++i)
			{
				for (int j = 0; j < n2 - k2; ++j)
				{
					H_arr2[i + k2, j] = I_kH[i, j];
				}
			}
			var H2 = new Matrix(H_arr2);
			Console.WriteLine("\nH2 = ");
			H2.Print();
			Console.WriteLine();

			var singleErrors = new List<Matrix>();
			for (int i = 0; i < n2; ++i)
			{
				singleErrors.Add(Matrix.Error(n2, i)); //одиночные ошибки
			}

			var syndroms2 = new Dictionary<Matrix, Matrix>();
			foreach (var error in singleErrors)
			{
				syndroms2[error * H2] = error;
			}

			var doubleErrors = new List<Matrix>();
			for (int i = 0; i < n2; ++i)
			{
				for (int j = i + 1; j < n2; ++j)
				{
					doubleErrors.Add(singleErrors[i] + singleErrors[j]);
				}
			}
			foreach (var error in doubleErrors)
			{
				syndroms2[error * H2] = error;
			}

			var u3 = new Matrix(new[] { 1, 0, 0, 0 });
			var v3 = u3 * G2; //[1, 0, 0, 0, 1, 1, 0, 1, 0, 1, 0] - кодовое слово длины 11
            Console.Write("v3 = ");
			v3.Print();
			var w3 = v3.SumWithError1(1); //[1, '1', 0, 0, 1, 1, 0, 1, 0, 1, 0] - слово с одной ошибкой
			Console.Write("w3 = ");
			w3.Print();
			var syndrom3 = w3 * H2;
            Console.Write("syndrom3 = ");
			syndrom3.Print();
			var error3 = syndroms2.Where(x => x.Key.Equals(syndrom3)).FirstOrDefault().Value;
            Console.Write("error3 = ");
            error3.Print();
			var correctedWord3 = w3 + error3;
            Console.WriteLine("correctedWord3 = ");
			correctedWord3.Print();
			(correctedWord3 * H2).Print(); //проверка

			var u4 = new Matrix(new[] { 1, 1, 1, 1 });
			var v4 = u4 * G2;
			Console.Write("v4 = "); //[1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0] - кодовое слово длины 11
			v4.Print();


		}
	}
}
