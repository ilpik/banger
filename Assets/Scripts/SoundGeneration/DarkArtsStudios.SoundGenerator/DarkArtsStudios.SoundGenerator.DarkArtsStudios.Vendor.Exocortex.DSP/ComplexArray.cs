using System;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.Exocortex.DSP
{
	internal class ComplexArray
	{
		private static ComplexF[] _workspaceF = new ComplexF[0];

		private ComplexArray()
		{
		}

		public static void ClampLength(Complex[] array, double fMinimum, double fMaximum)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Complex.FromModulusArgument(Math.Max(fMinimum, Math.Min(fMaximum, array[i].GetModulus())), array[i].GetArgument());
			}
		}

		public static void Clamp(Complex[] array, Complex minimum, Complex maximum)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Re = Math.Min(Math.Max(array[i].Re, minimum.Re), maximum.Re);
				array[i].Im = Math.Min(Math.Max(array[i].Re, minimum.Im), maximum.Im);
			}
		}

		public static void ClampToRealUnit(Complex[] array)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Re = Math.Min(Math.Max(array[i].Re, 0.0), 1.0);
				array[i].Im = 0.0;
			}
		}

		private static void LockWorkspaceF(int length, ref ComplexF[] workspace)
		{
			if (length >= _workspaceF.Length)
			{
				_workspaceF = new ComplexF[length];
			}
			workspace = _workspaceF;
		}

		private static void UnlockWorkspaceF(ref ComplexF[] workspace)
		{
			workspace = null;
		}

		public static void Shift(Complex[] array, int offset)
		{
			if (offset != 0)
			{
				int num = array.Length;
				Complex[] array2 = new Complex[num];
				for (int i = 0; i < num; i++)
				{
					array2[(i + offset) % num] = array[i];
				}
				for (int j = 0; j < num; j++)
				{
					array[j] = array2[j];
				}
			}
		}

		public static void Shift(ComplexF[] array, int offset)
		{
			if (offset != 0)
			{
				int num = array.Length;
				ComplexF[] workspace = null;
				LockWorkspaceF(num, ref workspace);
				for (int i = 0; i < num; i++)
				{
					workspace[(i + offset) % num] = array[i];
				}
				for (int j = 0; j < num; j++)
				{
					array[j] = workspace[j];
				}
				UnlockWorkspaceF(ref workspace);
			}
		}

		public static void GetLengthRange(Complex[] array, ref double minimum, ref double maximum)
		{
			minimum = double.MaxValue;
			maximum = double.MinValue;
			for (int i = 0; i < array.Length; i++)
			{
				double modulus = array[i].GetModulus();
				minimum = Math.Min(modulus, minimum);
				maximum = Math.Max(modulus, maximum);
			}
		}

		public static void GetLengthRange(ComplexF[] array, ref float minimum, ref float maximum)
		{
			minimum = float.MaxValue;
			maximum = float.MinValue;
			for (int i = 0; i < array.Length; i++)
			{
				float modulus = array[i].GetModulus();
				minimum = Math.Min(modulus, minimum);
				maximum = Math.Max(modulus, maximum);
			}
		}

		public static bool IsEqual(Complex[] array1, Complex[] array2, double tolerance)
		{
			if (array1.Length != array2.Length)
			{
				return false;
			}
			for (int i = 0; i < array1.Length; i++)
			{
				if (!Complex.IsEqual(array1[i], array2[i], tolerance))
				{
					return false;
				}
			}
			return true;
		}

		public static bool IsEqual(ComplexF[] array1, ComplexF[] array2, float tolerance)
		{
			if (array1.Length != array2.Length)
			{
				return false;
			}
			for (int i = 0; i < array1.Length; i++)
			{
				if (!ComplexF.IsEqual(array1[i], array2[i], tolerance))
				{
					return false;
				}
			}
			return true;
		}

		public static void Offset(Complex[] array, double offset)
		{
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				array[i].Re += offset;
			}
		}

		public static void Offset(Complex[] array, Complex offset)
		{
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				array[i] += offset;
			}
		}

		public static void Offset(ComplexF[] array, float offset)
		{
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				array[i].Re += offset;
			}
		}

		public static void Offset(ComplexF[] array, ComplexF offset)
		{
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				array[i] += offset;
			}
		}

		public static void Scale(Complex[] array, double scale)
		{
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				array[i] *= scale;
			}
		}

		public static void Scale(Complex[] array, double scale, int start, int length)
		{
			for (int i = 0; i < length; i++)
			{
				array[i + start] *= scale;
			}
		}

		public static void Scale(Complex[] array, Complex scale)
		{
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				array[i] *= scale;
			}
		}

		public static void Scale(Complex[] array, Complex scale, int start, int length)
		{
			for (int i = 0; i < length; i++)
			{
				array[i + start] *= scale;
			}
		}

		public static void Scale(ComplexF[] array, float scale)
		{
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				array[i] *= scale;
			}
		}

		public static void Scale(ComplexF[] array, float scale, int start, int length)
		{
			for (int i = 0; i < length; i++)
			{
				array[i + start] *= scale;
			}
		}

		public static void Scale(ComplexF[] array, ComplexF scale)
		{
			int num = array.Length;
			for (int i = 0; i < num; i++)
			{
				array[i] *= scale;
			}
		}

		public static void Scale(ComplexF[] array, ComplexF scale, int start, int length)
		{
			for (int i = 0; i < length; i++)
			{
				array[i + start] *= scale;
			}
		}

		public static void Multiply(Complex[] target, Complex[] rhs)
		{
			Multiply(target, rhs, target);
		}

		public static void Multiply(Complex[] lhs, Complex[] rhs, Complex[] result)
		{
			int num = lhs.Length;
			for (int i = 0; i < num; i++)
			{
				result[i] = lhs[i] * rhs[i];
			}
		}

		public static void Multiply(ComplexF[] target, ComplexF[] rhs)
		{
			Multiply(target, rhs, target);
		}

		public static void Multiply(ComplexF[] lhs, ComplexF[] rhs, ComplexF[] result)
		{
			int num = lhs.Length;
			for (int i = 0; i < num; i++)
			{
				result[i] = lhs[i] * rhs[i];
			}
		}

		public static void Divide(Complex[] target, Complex[] rhs)
		{
			Divide(target, rhs, target);
		}

		public static void Divide(Complex[] lhs, Complex[] rhs, Complex[] result)
		{
			int num = lhs.Length;
			for (int i = 0; i < num; i++)
			{
				result[i] = lhs[i] / rhs[i];
			}
		}

		public static void Divide(ComplexF[] target, ComplexF[] rhs)
		{
			Divide(target, rhs, target);
		}

		public static void Divide(ComplexF[] lhs, ComplexF[] rhs, ComplexF[] result)
		{
			ComplexF zero = ComplexF.Zero;
			int num = lhs.Length;
			for (int i = 0; i < num; i++)
			{
				if (rhs[i] != zero)
				{
					result[i] = lhs[i] / rhs[i];
				}
				else
				{
					result[i] = zero;
				}
			}
		}

		public static void Copy(Complex[] dest, Complex[] source)
		{
			for (int i = 0; i < dest.Length; i++)
			{
				dest[i] = source[i];
			}
		}

		public static void Copy(ComplexF[] dest, ComplexF[] source)
		{
			for (int i = 0; i < dest.Length; i++)
			{
				dest[i] = source[i];
			}
		}

		public static void Reverse(Complex[] array)
		{
			int num = array.Length;
			for (int i = 0; i < num / 2; i++)
			{
				Complex complex = array[i];
				array[i] = array[num - 1 - i];
				array[num - 1 - i] = complex;
			}
		}

		public static void Normalize(Complex[] array)
		{
			double minimum = 0.0;
			double maximum = 0.0;
			GetLengthRange(array, ref minimum, ref maximum);
			Scale(array, 1.0 / (maximum - minimum));
			Offset(array, (0.0 - minimum) / (maximum - minimum));
		}

		public static void Normalize(ComplexF[] array)
		{
			float minimum = 0f;
			float maximum = 0f;
			GetLengthRange(array, ref minimum, ref maximum);
			Scale(array, 1f / (maximum - minimum));
			Offset(array, (0f - minimum) / (maximum - minimum));
		}

		public static void Invert(Complex[] array)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (Complex)1.0 / array[i];
			}
		}

		public static void Invert(ComplexF[] array)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = (ComplexF)1f / array[i];
			}
		}
	}
}
