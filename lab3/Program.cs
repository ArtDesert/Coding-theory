using lab1;
using System;
using System.Collections.Generic;
using System.Linq;

namespace lab3
{
	public class Program
	{
		static void Main(string[] args)
		{
			int r = 4, multiple = 3;
			Task(r, multiple, 1);
			//Task(r, multiple, 2);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="H">Проверочная матрица</param>
		/// <param name="n">Длина кодового слова</param>
		/// <param name="multiple">Кратность ошибки</param>
		/// <returns></returns>
		public static Dictionary<Matrix,Matrix> GetAllSyndroms(Matrix H, int n, int multiple)
		{
			var syndroms = new Dictionary<Matrix, Matrix>();
			var allErrors = Error.GetAllErrorsWithSizeK(n, multiple);
			foreach (var error in allErrors)
			{
				var currentKey = error * H;
				if (!syndroms.ContainsKey(currentKey))
				{
					syndroms[currentKey] = error;
				}
			}
			return syndroms;
		}

		/// <summary>
		/// </summary>
		/// <param name="r"></param>
		/// <param name="multiple">Кратность ошибки</param>
		public static void Task(int r, int multiple, int taskNumber)
		{
			var random = new Random();
			Matrix X, G, H;
			int n = (int)(Math.Pow(2, r) - 1), k = n - r;
			if (taskNumber == 1)
			{
				(X, G, H) = CreateAllMatrix1(r);
			}
			else
			{
				(X, G, H) = CreateAllMatrix2(r);
			}
			if (taskNumber == 2)
			{
				++n;
			}
			var syndroms = GetAllSyndroms(H, n, multiple);
			Console.WriteLine("X =");
			X.Print();
            Console.WriteLine("\r\nG =");
            G.Print();
            Console.WriteLine("\r\nH =");
            H.Print();
			Console.WriteLine($"\r\n{nameof(syndroms)} =");
			syndroms.Print();

			var word = new Matrix(new int[k]);
			word[0, 0] = 1;
			Console.WriteLine($"\r\n{nameof(word)}  =");
			word.Print();

			var encodedWord = word * G;
            Console.WriteLine($"\r\n{nameof(encodedWord)} =");
			encodedWord.Print();

			var actualError = new Matrix(new int[n]).SumWithError1(0, 1, 2, 3); //Error
			Console.WriteLine($"\r\n{nameof(actualError)} =");
			actualError.Print();

			var wrongWord = encodedWord + actualError;
			Console.WriteLine($"\r\n{nameof(wrongWord)} =");
			wrongWord.Print();

			var syndrom = wrongWord * H;
			Console.WriteLine($"\r\n{nameof(syndrom)} =");
			syndrom.Print();

			var expectedErrors = syndroms.Where(x => x.Key.Equals(syndrom));
			Console.WriteLine($"\r\n{nameof(expectedErrors)} =");
			foreach (var expectedError in expectedErrors)
			{
				expectedError.Value.Print();
			}
		}

		public static (Matrix, Matrix, Matrix) CreateAllMatrix1(int r)
		{
			var X = Matrix.CreateX(r);
			int n = (int)(Math.Pow(2, r) - 1), k = n - r;
			var I = new Matrix(k);
			var G = I.Concat(X);
			var H = X.StackDown(new Matrix(n - k));
			return (X, G, H);
		}

		public static (Matrix, Matrix, Matrix) CreateAllMatrix2(int r)
		{
			var (X, G, H) = CreateAllMatrix1(r);
			int n = (int)(Math.Pow(2, r) - 1), k = n - r;

			//Расширяем матрицы 
			var H_ext = new Matrix(new int[n + 1, r + 1]);
			for (int i = 0; i < n; ++i)
			{
				for (int j = 0; j < r; ++j)
				{
					H_ext[i, j] = H[i, j];
				}
			}
			for (int i = 0; i < n + 1; ++i)
			{
				H_ext[i, r] = 1;
			}

			var G_ext = new Matrix(new int[k, n + 1]);
			for (int i = 0; i < k; ++i)
			{
				for (int j = 0; j < n; ++j)
				{
					G_ext[i, j] = G[i, j];
				}
			}
			for (int i = 0; i < k; ++i)
			{
				int currentSum = 0;
				for (int j = 0; j < n; ++j)
				{
					currentSum += (G[i, j] % 2);
				}
				G_ext[i, n] = (currentSum % 2 == 1) ? 1 : 0;
			}
			return (X, G_ext, H_ext);
		}
	}
}
