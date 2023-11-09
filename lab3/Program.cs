using lab1;
using System;
using System.Collections.Generic;

namespace lab3
{
	public class Program
	{
		static void Main(string[] args)
		{
			int r = 4, multiple = 1;
			Task(r, multiple);
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
				syndroms[error * H] = error;
			}
			return syndroms;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="G">Порождающая матрица</param>
		/// <param name="H">Проверочная матрица</param>
		public static void Task(int r, int multiple)
		{
			var X = Matrix.CreateX(r);
			int n = (int)(Math.Pow(2, r) - 1), k = n - r;
			var I = new Matrix(k);
			var G = I.Concat(X);
			var H = X.StackDown(new Matrix(n - k));
			var syndroms = GetAllSyndroms(H, k, multiple);
			Console.WriteLine("X");
			X.Print();
            Console.WriteLine("\r\nG");
            G.Print();
            Console.WriteLine("\r\nH");
            H.Print();
            Console.WriteLine("\r\nsyndroms");
            syndroms.Print();
			Console.WriteLine("\r\nword");
			//word.Print();
			//Console.WriteLine("\r\nword");
		}
	}
}
