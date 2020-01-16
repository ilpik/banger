using System;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.Exocortex.DSP
{
	internal class ComplexStats
	{
		private ComplexStats()
		{
		}

		public static ComplexF Sum(ComplexF[] data)
		{
			return SumRecursion(data, 0, data.Length);
		}

		private static ComplexF SumRecursion(ComplexF[] data, int start, int end)
		{
			if (end - start <= 1000)
			{
				ComplexF zero = ComplexF.Zero;
				for (int i = start; i < end; i++)
				{
					zero += data[i];
				}
				return zero;
			}
			int num = start + end >> 1;
			return SumRecursion(data, start, num) + SumRecursion(data, num, end);
		}

		public static Complex Sum(Complex[] data)
		{
			return SumRecursion(data, 0, data.Length);
		}

		private static Complex SumRecursion(Complex[] data, int start, int end)
		{
			if (end - start <= 1000)
			{
				Complex zero = Complex.Zero;
				for (int i = start; i < end; i++)
				{
					zero += data[i];
				}
				return zero;
			}
			int num = start + end >> 1;
			return SumRecursion(data, start, num) + SumRecursion(data, num, end);
		}

		public static ComplexF SumOfSquares(ComplexF[] data)
		{
			return SumOfSquaresRecursion(data, 0, data.Length);
		}

		private static ComplexF SumOfSquaresRecursion(ComplexF[] data, int start, int end)
		{
			if (end - start <= 1000)
			{
				ComplexF zero = ComplexF.Zero;
				for (int i = start; i < end; i++)
				{
					zero += data[i] * data[i];
				}
				return zero;
			}
			int num = start + end >> 1;
			return SumOfSquaresRecursion(data, start, num) + SumOfSquaresRecursion(data, num, end);
		}

		public static Complex SumOfSquares(Complex[] data)
		{
			return SumOfSquaresRecursion(data, 0, data.Length);
		}

		private static Complex SumOfSquaresRecursion(Complex[] data, int start, int end)
		{
			if (end - start <= 1000)
			{
				Complex zero = Complex.Zero;
				for (int i = start; i < end; i++)
				{
					zero += data[i] * data[i];
				}
				return zero;
			}
			int num = start + end >> 1;
			return SumOfSquaresRecursion(data, start, num) + SumOfSquaresRecursion(data, num, end);
		}

		public static ComplexF Mean(ComplexF[] data)
		{
			return Sum(data) / data.Length;
		}

		public static Complex Mean(Complex[] data)
		{
			return Sum(data) / data.Length;
		}

		public static ComplexF Variance(ComplexF[] data)
		{
			if (data.Length == 0)
			{
				throw new DivideByZeroException("length of data is zero");
			}
			return SumOfSquares(data) / data.Length - Sum(data);
		}

		public static Complex Variance(Complex[] data)
		{
			if (data.Length == 0)
			{
				throw new DivideByZeroException("length of data is zero");
			}
			return SumOfSquares(data) / data.Length - Sum(data);
		}

		public static ComplexF StdDev(ComplexF[] data)
		{
			if (data.Length == 0)
			{
				throw new DivideByZeroException("length of data is zero");
			}
			return ComplexMath.Sqrt(Variance(data));
		}

		public static Complex StdDev(Complex[] data)
		{
			if (data.Length == 0)
			{
				throw new DivideByZeroException("length of data is zero");
			}
			return ComplexMath.Sqrt(Variance(data));
		}

		public static float RMSError(ComplexF[] alpha, ComplexF[] beta)
		{
			return (float)Math.Sqrt(SumOfSquaredErrorRecursion(alpha, beta, 0, alpha.Length));
		}

		private static float SumOfSquaredErrorRecursion(ComplexF[] alpha, ComplexF[] beta, int start, int end)
		{
			if (end - start <= 1000)
			{
				float num = 0f;
				for (int i = start; i < end; i++)
				{
					ComplexF complexF = beta[i] - alpha[i];
					num += complexF.Re * complexF.Re + complexF.Im * complexF.Im;
				}
				return num;
			}
			int num2 = start + end >> 1;
			return SumOfSquaredErrorRecursion(alpha, beta, start, num2) + SumOfSquaredErrorRecursion(alpha, beta, num2, end);
		}

		public static double RMSError(Complex[] alpha, Complex[] beta)
		{
			return Math.Sqrt(SumOfSquaredErrorRecursion(alpha, beta, 0, alpha.Length));
		}

		private static double SumOfSquaredErrorRecursion(Complex[] alpha, Complex[] beta, int start, int end)
		{
			if (end - start <= 1000)
			{
				double num = 0.0;
				for (int i = start; i < end; i++)
				{
					Complex complex = beta[i] - alpha[i];
					num += complex.Re * complex.Re + complex.Im * complex.Im;
				}
				return num;
			}
			int num2 = start + end >> 1;
			return SumOfSquaredErrorRecursion(alpha, beta, start, num2) + SumOfSquaredErrorRecursion(alpha, beta, num2, end);
		}
	}
}
