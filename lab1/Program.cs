
using System;

namespace lab1
{
	public class Program
	{
		static void Main(string[] args)
		{
			int[,] arr_S = 
			{
				{ 1, 0, 1, 1, 0, 0, 0, 1, 0, 0, 1 },
				{ 0, 0, 0, 1, 1, 1, 0, 1, 0, 1, 0 },
				{ 0, 0, 0, 0, 1, 0, 0, 1, 0, 0, 1 },
				{ 1, 0, 1, 0, 1, 1, 1, 0, 0, 0, 0 },
				{ 0, 0, 0, 0, 1, 0, 0, 1, 1, 1, 0 },
				{ 1, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0 }
			};

			var S = new Matrix(arr_S);
			var S_REF = S.REF(); //Порождающая матрица в ступенчатом виде
			S_REF.PrintWithSelectedLeadElements(ConsoleColor.Blue);
            Console.WriteLine();

			var G = S_REF.ClearNullableRows(); //Порождающая матрица в ступенчатом виде без нулевых строк
            Console.WriteLine();
			G.PrintWithSelectedLeadElements(ConsoleColor.Blue);
			int n = G.Col;
			int k = G.Row;
			Console.WriteLine();
            Console.WriteLine($"n = {n}");
            Console.WriteLine($"k = {k}");
            Console.WriteLine();

            var G0 = G.PREF(); //Матрица в ступенчатом виде без нулевых строк на основе порождающей матрицы
			G0.PrintWithSelectedLeadElements(ConsoleColor.Blue);
			Console.Write("lead = [");
			foreach (var (_,j) in G0.Leads)
			{
                Console.Write($"{j}, ");
            }
            Console.WriteLine("\b\b]");
			Console.WriteLine();

			G0.PrintWithNoSelectedLeadColumns(ConsoleColor.DarkGreen);
			Console.WriteLine();

			var X = G0.ClearLeadColumns(); //Сокращённая матрица, полученная удалением ведущих столбцов матрицы G*
			Console.Write("X = ");
			X.Print();
			Console.WriteLine();

			Console.Write("lead = [");
			foreach (var (_, j) in G0.Leads)
			{
				Console.Write($"{j}, ");
			}
			Console.WriteLine("\b\b]");
			Console.WriteLine("");

			var H = X.UnionWithUnitMatrix();
			H.PrintWithSelectedRows(ConsoleColor.DarkGreen, ConsoleColor.DarkYellow);

			//(G * H).Print();


			var u = new Matrix(new int[] { 1, 0, 1, 1, 0 });

			var v = u * G;
			Console.Write("v = ");
			v.Print(); //[1 0 1 1 1 0 1 0 0 1 '0']
			(v * H).Print(); //OK

			int d = G.GetCodeDistance(); //OK
			Console.WriteLine(d);
			int t = d - 1;
			var newV = v.SumWithError1(2); //[1 0 0 1 1 0 1 0 0 1 '0']
			Console.Write("newV = ");
			newV.Print();
			(newV * H).Print(); //[0 1 0 0 0 0] -error

			var newV2 = v.SumWithError1(4, 7);
			Console.Write("newV2 = "); 
			newV2.Print(); //[1 0 1 1 0 0 1 1 0 1 '0'] 
			(newV2 * H).Print();
		}
	}
}
