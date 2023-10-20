using System.Runtime.Remoting;

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
	}
}
