using lab1;
using System;
using System.Collections.Generic;

namespace lab3
{
	public static class DictionaryExtensions
	{
		public static void Print(this Dictionary<Matrix, Matrix> dict)
		{
			foreach (var item in dict)
			{
				Console.WriteLine($"{item.Key} | {item.Value}");
            }
		}
	}
}
