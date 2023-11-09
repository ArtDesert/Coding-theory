using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lab1
{
	public struct Error
	{
		public int[] Arr { get; set; }

		public Error(int size, int position)
		{
			Arr = new int[size];
			++Arr[position];
		}

		public Error(Matrix error)
		{
			Arr = new int[error.Col];
			for (int i = 0; i < error.Col; ++i)
			{
				Arr[i] = error[0, i];
			}
		}

		public int this[int i]
		{
			get
			{
				return Arr[i];
			}
			set
			{
				Arr[i] = value;
			}
		}

		public override bool Equals(object obj)
		{
			var error = (Error)obj;
			for (int i = 0; i < Arr.Length; ++i)
			{
				if (Arr[i] != error[i])
				{
					return false;
				}
			}
			return true;
		}

		public static List<Matrix> GetAllErrorsWithSizeK(int n, int multiple)
		{
			var subsets = GetSubsets(n, multiple);
			var errors = subsets
				.Select(subset =>
				{
					var error = new int[n];
					foreach (var item in subset)
					{
						error[item] = 1;
					}
					return new Matrix(error);
				})
				.ToList();
			return errors;
		}

		private static List<List<int>> GetSubsets(int n, int k)
		{
			var set = Enumerable.Range(0, n).ToList();
			var subsets = new List<List<int>>();
			GetSubsetsHelper(set, new List<int>(), 0, k, subsets);
			return subsets;
		}

		private static void GetSubsetsHelper(List<int> set, List<int> currentSubset, int start, int k, List<List<int>> subsets)
		{
			if (k == 0)
			{
				subsets.Add(new List<int>(currentSubset));
				return;
			}
			for (int i = start; i < set.Count; ++i)
			{
				currentSubset.Add(set[i]);
				GetSubsetsHelper(set, currentSubset, i + 1, k - 1, subsets);
				currentSubset.RemoveAt(currentSubset.Count - 1);
			}
		}

		public override string ToString()
		{
			var builder = new StringBuilder();
			foreach (var item in Arr)
			{
				builder.Append($"{item} ");
			}
			return builder.ToString();
		}
	}
}
