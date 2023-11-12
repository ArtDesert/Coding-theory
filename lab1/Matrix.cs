using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using lab1;

namespace lab1
{
	public class Matrix
	{
		public int Row { get; set; }
		public int Col { get; set; }
		public int[,] Arr { get; set; }
		public HashSet<(int, int)> Leads { get; set; }
		public HashSet<int> NoLeads { get; set; }

		public Matrix(Matrix matrix)
		{
			Row = matrix.Row;
			Col = matrix.Col;
			Arr = new int[Row, Col];
			Leads = new HashSet<(int, int)>();
			for (int i = 0; i < Row; ++i)
			{
				for (int j = 0; j < Col; ++j)
				{
					Arr[i, j] = matrix.Arr[i, j];
				}
			}
		}
		public static Matrix Error(int size, int position)
		{
			var result = new Matrix(new int[size]);
			result[0, position] = 1;
			return result;
		}

		public static Matrix Error(int size, int position1, int position2)
		{
			var result = new Matrix(new int[size]);
			++result[0, position1];
			++result[0, position2];
			return result;
		}

		public Matrix(int[,] arr)
		{
			Row = arr.GetLength(0);
			Col = arr.GetLength(1);
			Arr = new int[Row, Col];
			Leads = new HashSet<(int, int)>();
			NoLeads = new HashSet<int>();
			for (int i = 0; i < Row; ++i)
			{
				for (int j = 0; j < Col; ++j)
				{
					Arr[i, j] = arr[i, j];
				}
			}
		}

		/// <summary>
		/// Создаёт единичную матрицу указанного порядка
		/// </summary>
		/// <param name="order"></param>
		public Matrix(int order)
		{
			Row = Col = order;
			Arr = new int[Row, Col];
			Leads = new HashSet<(int, int)>();
			NoLeads = new HashSet<int>();
			for (int i = 0; i < Row; ++i)
			{
				for (int j = 0; j < Col; ++j)
				{
					Arr[i, j] = i == j ? 1 : 0;
				}
			}
		}

		public Matrix(int[] arr)
		{
			Arr = new int[1, arr.Length];
			Row = 1;
			Col = arr.Length;
			for (int i = 0; i < Col; ++i)
			{
				Arr[0, i] = arr[i];
			}
		}

		public int this[int i, int j]
		{
			get
			{
				return Arr[i, j];
			}
			set
			{
				Arr[i, j] = value;
			}
		}

		public void Print()
		{
			for (int i = 0; i < Row; ++i)
			{
				for (int j = 0; j < Col; ++j)
				{
					Console.Write($"{Arr[i, j]} ");
				}
				Console.WriteLine();
			}
		}

		public void PrintWithSelectedLeadElements(ConsoleColor color)
		{
			for (int i = 0; i < Row; ++i)
			{
				for (int j = 0; j < Col; ++j)
				{
					if (Leads.Contains((i, j)))
					{
						Console.BackgroundColor = color;
					}
					Console.Write($"{Arr[i, j]}");
					Console.BackgroundColor = ConsoleColor.Black;
					Console.Write(" ");
				}
				Console.Write("\n");
			}
		}

		public void PrintWithNoSelectedLeadColumns(ConsoleColor color)
		{
			var leads = new HashSet<int>();
			foreach (var item in Leads)
			{
				leads.Add(item.Item2);
			}
			for (int i = 0; i < Row; ++i)
			{
				for (int j = 0; j < Col; ++j)
				{
					if (!leads.Contains(j))
					{
						Console.BackgroundColor = color;
					}
					Console.Write($"{Arr[i, j]}");
					Console.BackgroundColor = ConsoleColor.Black;
					Console.Write(" ");
				}
				Console.WriteLine();
			}
		}

		public void PrintWithSelectedRows(ConsoleColor leadColor, ConsoleColor noLeadColor)
		{
			var leads = new HashSet<int>();
			foreach (var item in Leads)
			{
				leads.Add(item.Item2);
			}
			for (int i = 0; i < Row; ++i)
			{
				for (int j = 0; j < Col; ++j)
				{
					if (leads.Contains(i))
					{
						Console.BackgroundColor = leadColor;
					}
					else
					{
						Console.BackgroundColor = noLeadColor;
					}
					Console.Write(Arr[i, j]);
				}
				Console.WriteLine();
			}
			Console.BackgroundColor = ConsoleColor.Black;
		}

		/// <summary>
		/// Приводит матрицу к ступенчатому виду алгоритмом Гаусса
		/// </summary>
		/// <returns>Матрица в ступенчатом виде</returns>
		public Matrix REF()
		{
			var result = new Matrix(this);
			(int i, int j) leadIndex = (-1, -1);
			for (int k = 0; k < Row; ++k)
			{
				//Находим очередной ведущий элемент
				bool isFoundLead = false;
				for (int j = leadIndex.j + 1; j < Col && !isFoundLead; ++j)
				{
					for (int i = leadIndex.i + 1; i < Row; i++)
					{
						double curElement = result.Arr[i, j];
						if (curElement != 0)
						{
							leadIndex = (i, j);
							result.Leads.Add(leadIndex);
							isFoundLead = true;
							break;
						}
					}
				}

				//Зануляем все элементы под ведущим элементом
				for (int i = leadIndex.i + 1; i < Row; ++i)
				{
					if (result.Arr[i, leadIndex.j] != 0)
					{
						for (int j = 0; j < Col; ++j)
						{
							result.Arr[i, j] = (result.Arr[i, j] + result.Arr[leadIndex.i, j]) % 2;
						}
					}
				}
			}
			return result;
		}

		public Matrix PREF()
		{
			var result = REF();
			var leads = result.Leads.Reverse().ToList();
			//Зануляем все элементы над ведущим элементом
			foreach ((int i, int j) leadIndex in leads)
			{
				for (int i = leadIndex.i - 1; i >= 0; --i)
				{
					if (result.Arr[i, leadIndex.j] != 0)
					{
						for (int j = 0; j < Col; ++j)
						{
							result.Arr[i, j] = (result.Arr[i, j] + result.Arr[leadIndex.i, j]) % 2;
						}
					}
				}
			}
			return result;
		}

		/// <summary>
		/// Меняет местами две строки
		/// </summary>
		/// <param name="index1">Индекс первой строки</param>
		/// <param name="index2">Индекс второй строки</param>
		public void SwapRows(int index1, int index2)
		{
			var temp = new int[Col];
			for (int i = 0; i < Col; ++i)
			{
				temp[i] = Arr[index1, i];
			}
			for (int i = 0; i < Col; ++i)
			{
				Arr[index1, i] = Arr[index2, i];
			}
			for (int i = 0; i < Col; ++i)
			{
				Arr[index2, i] = temp[i];
			}
		}

		/// <summary>
		/// Меняет местами два столбца
		/// </summary>
		/// <param name="index1">Индекс первого столбца</param>
		/// <param name="index2">Индекс второго столбца</param>
		public void SwapColumns(int index1, int index2)
		{
			var temp = new int[Row];
			for (int i = 0; i < Row; ++i)
			{
				temp[i] = Arr[i, index1];
			}
			for (int i = 0; i < Row; ++i)
			{
				Arr[i, index1] = Arr[i, index2];
			}
			for (int i = 0; i < Row; ++i)
			{
				Arr[i, index2] = temp[i];
			}
		}

		public Matrix ClearNullableRows()
		{
			//Количество ненулевых строк
			int newI = 0;
			var notNullRows = new List<(int, int)>();
			for (int i = 0; i < Row; ++i, ++newI)
			{
				bool isNull = true;
				for (int j = 0; j < Col; ++j)
				{
					if (Arr[i, j] != 0)
					{
						isNull = false;
						break;
					}
				}
				if (!isNull)
				{
					notNullRows.Add((newI, i));
				}
				else
				{
					--newI;
				}
			}
			var arr = new int[newI, Col];
			foreach ((int newI, int i) row in notNullRows)
			{
				for (int j = 0; j < Col; ++j)
				{
					arr[row.newI, j] = Arr[row.i, j];
				}
			}
			return new Matrix(arr);
		}

		public Matrix ClearLeadColumns()
		{
			var leads = new HashSet<int>();
			foreach (var item in Leads)
			{
				leads.Add(item.Item2);
			}
			var noLeads = Enumerable.Range(0, Col).Where(x => !leads.Contains(x)).ToList();
			var arr = new int[Row, Col - Leads.Count];
			int newJ = 0;
			foreach (var noLead in noLeads)
			{
				for (int i = 0; i < Row; ++i)
				{
					arr[i, newJ] = Arr[i, noLead];
				}
				++newJ;
			}
			var result = new Matrix(arr);
			result.Leads = Leads;
			return result;
		}

		public Matrix UnionWithUnitMatrix()
		{
			var leads = new HashSet<int>();
			foreach (var item in Leads)
			{
				leads.Add(item.Item2);
			}
			var noLeads = Enumerable.Range(0, Row + Col).Where(x => !leads.Contains(x)).ToList();
			var I = new Matrix(Row + Col - Leads.Count);
			int i_fromX = 0;
			int i_fromI = 0;
			var arr = new int[Row + Col, Col];
			for (int i = 0; i < Row + Col; ++i)
			{
				if (leads.Contains(i))
				{
					for (int j = 0; j < Col; ++j)
					{
						arr[i, j] = Arr[i_fromX, j];
					}
					++i_fromX;
				}
				else
				{
					for (int j = 0; j < Col; ++j)
					{
						arr[i, j] = I.Arr[i_fromI, j];
					}
					++i_fromI;
				}
			}
			var result = new Matrix(arr);
			result.NoLeads = NoLeads;
			result.Leads = Leads;
			return result;
		}

		public static Matrix operator *(Matrix m1, Matrix m2)
		{
			var arr = new int[m1.Row, m2.Col];
			for (int i = 0; i < m1.Row; ++i)
			{
				for (int j = 0; j < m2.Col; ++j)
				{
					int sum = 0;
					for (int k = 0; k < m1.Col; ++k)
					{
						sum = (sum + m1.Arr[i, k] * m2.Arr[k, j]) % 2;
					}
					arr[i, j] = sum;
				}
			}
			var result = new Matrix(arr);
			return result;
		}

		public int GetCodeDistance()
		{
			int result = int.MaxValue;
			for (int i = 0; i < Row; ++i)
			{
				for (int j = i + 1; j < Row; ++j)
				{
					int curDistance = 0;
					for (int k = 0; k < Col; ++k)
					{
						curDistance += Arr[i, k] != Arr[j, k] ? 1 : 0;
					}
					if (curDistance < result)
					{
						result = curDistance;
					}
				}
			}
			return result;
		}
		/// <summary>
		/// Только для векторов!
		/// </summary>
		/// <param name="position"></param>
		/// <returns></returns>
		public Matrix SumWithError1(params int[] positions)
		{
			if (Row > 1)
			{
				throw new Exception();
			}
			var result = new Matrix(this);
			foreach (var position in positions)
			{
				result[0, position] = (result[0, position] + 1) % 2;
			}
			return result;
		}

		public static Matrix operator +(Matrix m1, Matrix m2)
		{
			var result = new Matrix(new int[m1.Row, m1.Col]);
			for (int i = 0; i < m1.Row; ++i)
			{
				for (int j = 0; j < m1.Col; ++j)
				{
					result[i, j] = (m1[i, j] + m2[i, j]) % 2;
				}
			}
			return result;
		}

		public override bool Equals(object obj)
		{
			var matrix = obj as Matrix;
			for (int i = 0; i < Row; ++i)
			{
				for (int j = 0; j < Col; ++j)
				{
					if (Arr[i, j] != matrix[i, j])
					{
						return false;
					}
				}
			}
			return true;
		}

		public override int GetHashCode()
		{
			var hash = 0;
			var mod = (int)(Math.Pow(2, 32) - 5);
			foreach (var item in Arr)
			{
				hash += item.GetHashCode() % mod;
			}
			return hash % mod;
		}

		public override string ToString()
		{
			var builder = new StringBuilder();
			for (int i = 0; i < Row; ++i)
			{
				for (int j = 0; j < Col; ++j)
				{
					builder.Append($"{Arr[i, j]} ");
				}
				if (i != Row - 1)
				{
					builder.Append("\r\n");
				}
			}
			return builder.ToString();
		}

		public static Matrix GenerateX(int n, int k)
		{
			bool flag = true;
			var random = new Random();
			var X = new Matrix(new int[k, n]);
			while (flag)
			{
				flag = false;
				for (int i = 0; i < k; ++i)
				{
					for (int j = 0; j < n; ++j)
					{
						X[i, j] = random.Next(2);
					}
				}

				for (int i = 0; i < k; ++i) //не менее 4 единиц
				{
					int sum = 0;
					for (int j = 0; j < n; ++j)
					{
						sum += X[i, j];
					}
					if (sum < 4)
					{
						flag = true;
						continue;
					}
				}

				for (int i = 0; i < k - 1; ++i) // сумма любых двух строк содержала не менее 3 единиц;
				{
					for (int j = i + 1; j < k; ++j)
					{
						int sum = 0;
						for (int s = 0; s < n; ++s)
						{
							sum += X[i, s] + X[j, s];
						}
						if (sum < 3)
						{
							flag = true;
							continue;
						}
					}
				}

				for (int i = 0; i < k - 2; ++i) // сумма любых трёх строк содержала не менее 2 единиц;
				{
					for (int j = i + 1; j < k - 1; ++j)
					{
						int sum = 0;
						for (int s = j + 1; s < k; ++s)
						{
							for (int m = 0; m < n; ++m)
							{
								sum += X[i, m] + X[j, m] + X[s, m];
							}
							if (sum < 2)
							{
								flag = true;
							}
						}
					}
				}

				for (int i = 0; i < k - 3; ++i) //сумма любых четырёх строк содержала не менее 1 единицы;
				{
					for (int j = 0; j < k - 2; ++j)
					{
						for (int m = j + 1; m < k - 1; ++m)
						{
							for (int p = m + 1; p < k; ++p)
							{
								int sum = 0;
								for (int s = 0; s < n; ++s)
								{
									sum += X[i, s] + X[j, s] + X[m, s] + X[p, s];
								}
								if (sum < 1)
								{
									flag = true;
								}
							}
						}
					}
				}
			}
			return X;
		}

		public Matrix Concat(Matrix matrix)
		{
			int n = Row, k = Col + matrix.Col;
			var result = new Matrix(new int[n, k]);
			for (int i = 0; i < n; ++i)
			{
				for (int j = 0; j < n; ++j)
				{
					result[i, j] = Arr[i, j];
				}
			}
			for (int i = 0; i < n; ++i)
			{
				for (int j = n; j < k; ++j)
				{
					result[i, j] = matrix[i, j - n];
				}
			}
			return result;
		}

		public Matrix StackDown(Matrix matrix)
		{
			int n = Row + matrix.Row, k = Col;
			var result = new Matrix(new int[n, k]);
			for (int i = 0; i < Row; ++i)
			{
				for (int j = 0; j < Col; ++j)
				{
					result[i, j] = Arr[i, j];
				}
			}
			for (int i = 0; i < matrix.Row; ++i)
			{
				for (int j = 0; j < matrix.Col; ++j)
				{
					result[i + Row, j] = matrix[i, j];
				}
			}
			return result;
		}

		public static Matrix CreateX(int r)
		{
			int n = (int)(Math.Pow(2, r) - r - 1);
			var X = new Matrix(new int[n, r]);
			var subsets = GetAllSubsets(r);
			for (int i = 0; i < r; ++i)
			{
				subsets
					.Remove(subsets.Where(x => x.Sum() == 1 && x[i] == 1)
					.FirstOrDefault());
			}
			subsets.Remove(subsets.Where(x => x.Sum() == 0).FirstOrDefault());
			for (int i = 0; i < n; ++i)
			{
				for (int j = 0; j < r; ++j)
				{
					X[i, j] = subsets[i][j];
				}
			}
			return X;
		}

		public static List<List<int>> GetAllSubsets(int n)
		{
			var set = Enumerable.Range(0, n).ToArray();
			var subsets = new List<List<int>>();
			for (int i = 0; i < (1 << n); i++)
			{
				List<int> subset = new List<int>();
				for (int j = 0; j < n; j++)
				{
					if ((i & (1 << j)) > 0)
					{
						subset.Add(set[j]);
					}
				}
				subsets.Add(subset);
			}
			return subsets.Select(x =>
			{
				var subset = new int[n];
				foreach (var item in x)
				{
					subset[item] = 1;
				}
				return subset.ToList();
			}).ToList();
		}
	}
}
