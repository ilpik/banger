using System;

namespace DarkArtsStudios.SoundGenerator.DarkArtsStudios.Vendor.Exocortex.DSP
{
	internal class ComplexMath
	{
		private static double _halfOfRoot2 = 0.5 * Math.Sqrt(2.0);

		private ComplexMath()
		{
		}

		public static void Swap(ref Complex a, ref Complex b)
		{
			Complex complex = a;
			a = b;
			b = complex;
		}

		public static void Swap(ref ComplexF a, ref ComplexF b)
		{
			ComplexF complexF = a;
			a = b;
			b = complexF;
		}

		public static ComplexF Sqrt(ComplexF c)
		{
			double num = c.Re;
			double num2 = c.Im;
			double num3 = Math.Sqrt(num * num + num2 * num2);
			int num4 = (!(num2 < 0.0)) ? 1 : (-1);
			c.Re = (float)(_halfOfRoot2 * Math.Sqrt(num3 + num));
			c.Im = (float)(_halfOfRoot2 * (double)num4 * Math.Sqrt(num3 - num));
			return c;
		}

		public static Complex Sqrt(Complex c)
		{
			double re = c.Re;
			double im = c.Im;
			double num = Math.Sqrt(re * re + im * im);
			int num2 = (!(im < 0.0)) ? 1 : (-1);
			c.Re = _halfOfRoot2 * Math.Sqrt(num + re);
			c.Im = _halfOfRoot2 * (double)num2 * Math.Sqrt(num - re);
			return c;
		}

		public static ComplexF Pow(ComplexF c, double exponent)
		{
			double num = c.Re;
			double num2 = c.Im;
			double num3 = Math.Pow(num * num + num2 * num2, exponent * 0.5);
			double num4 = Math.Atan2(num2, num) * exponent;
			c.Re = (float)(num3 * Math.Cos(num4));
			c.Im = (float)(num3 * Math.Sin(num4));
			return c;
		}

		public static Complex Pow(Complex c, double exponent)
		{
			double re = c.Re;
			double im = c.Im;
			double num = Math.Pow(re * re + im * im, exponent * 0.5);
			double num2 = Math.Atan2(im, re) * exponent;
			c.Re = num * Math.Cos(num2);
			c.Im = num * Math.Sin(num2);
			return c;
		}
	}
}
